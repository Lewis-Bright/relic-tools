// This file (Translation.cs) is a part of the IBBoard library and is copyright 2009 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU LGPL license, either version 3 of the License or (at your option) any later version. Please see COPYING.LGPL for more information and the full license.

using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;
using IBBoard.IO;
using IBBoard.Logging;
using IBBoard.Xml;

namespace IBBoard.Lang
{
	/// <summary>
	/// A basic string translator that a specified language and returns translated strings that correspond to translation IDs. 
	/// If the string doesn't exist in the specified language then the translator falls back defined 'super' languages.
	/// If the translation doesn't exist in the hierarchy of languages then either a supplied value, null or a "no validation available"
	/// message is returned, depending on the parameters supplied to the method.
	/// 
	/// Loaded languages are referenced by two-character language code (e.g. "en" or "it")
	/// </summary>
	public class Translation
	{
		private static AbstractTranslationSet currentTranslations;
		private static Dictionary<string, AbstractTranslationSet> langToTranslationMap;
		public static event MethodInvoker TranslationChanged;
		
		static Translation()
		{
			langToTranslationMap = new Dictionary<string, AbstractTranslationSet>();
		}

		/// <summary>
		/// Initialises the translations and loads the specified language so that the Translation class can be used.
		/// Throws a TranslationLoadException if a problem occurred while loading translations. If this occurs then the translation methods can
		/// still be called but no translation will be performed.
		/// </summary>
		/// <param name="appPath">
		/// The full path that the contains the "translations" folder - normally the application directory path.
		/// </param>
		/// <param name="language">
		/// The language to use as the loaded language
		/// </param>
		public static void InitialiseTranslations(string appPath, string language)
		{
			InitialiseTranslations(appPath);
			LoadTranslation(language);
		}
		
		/// <summary>
		/// Initialises the translation class for an application or source.
		/// </summary>
		/// <param name="appPath">
		/// The full path that the contains the "translations" folder - normally the application directory path.
		/// </param>
		public static void InitialiseTranslations(string appPath)
		{
			InitialiseDefaults(appPath);
		}
		
		private static void InitialiseDefaults(string appPath)
		{
			Reset();
			LoadTranslations(appPath);
		}
		
		private static void LoadTranslations(string appPath)
		{
			DirectoryInfo dir = new DirectoryInfo(Path.Combine(appPath, "translations"));
			
			if (!dir.Exists)
			{
				throw new TranslationLoadException("Translation path not found (" + dir.FullName + ")");
			}
						
			TranslationXmlLoader loader = new TranslationXmlLoader();
			
			foreach (FileInfo file in dir.GetFiles("*.translation"))
			{
				AbstractTranslationSet translations = loader.LoadTranslations(file.FullName);
				AddTranslationSet(translations);
			}
		}
		
		/// <summary>
		/// Adds the supplied translation set to the list of available translations
		/// </summary>
		/// <param name="translations">
		/// The translation set to add
		/// </param>
		public static void AddTranslationSet(AbstractTranslationSet translations)
		{
			langToTranslationMap[translations.LanguageCode] = translations;
		}
		
		/// <summary>
		/// Resets the loaded translations and reverts to no translations.
		/// </summary>
		public static void Reset()
		{
			SetCurrentTranslations(null);
			langToTranslationMap.Clear();
		}
		
		private static void SetCurrentTranslations(AbstractTranslationSet newTranslations)
		{
			if (currentTranslations != newTranslations || (currentTranslations != null && !currentTranslations.Equals(newTranslations)))
			{
				currentTranslations = newTranslations;
				
				if (TranslationChanged != null)
				{
					TranslationChanged();
				}
			}
		}

		/// <summary>
		/// Loads translations for a given language and sets them as the current language.
		/// Throws a TranslationLoadException if a problem occurred while loading translations. If this occurs then the translation methods can
		/// still be called but all translations will fall back to the default translation.
		/// </summary>
		/// <param name="translationLang">
		/// The new local language to load
		/// </param>
		public static void LoadTranslation(string translationLanguage)
		{
			if (HasLanguage(translationLanguage))
			{
				SetCurrentTranslations(GetTranslationSet(translationLanguage));
			}
		}

		/// <summary>
		/// Gets a translation for a given ID, falling back to a "missing translation" message if none can be found. Also optionally replaces any placeholders with the supplied values.
		/// </summary>
		/// <param name="translationID">
		/// The ID to look up the translation for
		/// </param>
		/// <param name="replacements">
		/// A collection of <see cref="System.Object"/>s to replace placeholders with
		/// </param>
		/// <returns>
		/// The translation with the placeholders replaced or a "missing translation" message
		/// </returns>
		public static string GetTranslation(string translationID, params object[] replacements)
		{
			return GetTranslation(translationID, false, replacements);
		}

		/// <summary>
		/// Gets a translation for a given ID, falling back to null or a warning message if a translation cannot be found. Also optionally replaces any placeholders with the supplied values.
		/// </summary>
		/// <param name="translationID">
		/// The ID to look up the translation for
		/// </param>
		/// <param name="returnNullOnFail">
		/// TRUE if null should be returned when no translation can be found, or FALSE if a "missing translation" message should be returned
		/// </param>
		/// <param name="replacements">
		/// A collection of <see cref="System.Object"/>s to replace placeholders with
		/// </param>
		/// <returns>
		/// The translation with the placeholders replaced, or a "missing translation" message or null depending on <param name="returnNullOnFail">
		/// </returns>
		public static string GetTranslation(string translationID, bool returnNullOnFail, params object[] replacements)
		{
			return GetTranslation(translationID, returnNullOnFail ? null : "", replacements);
		}

		/// <summary>
		/// Gets a translation for a given ID, falling back to a supplied default if a translation cannot be found. Also optionally replaces any placeholders with the supplied values.
		/// </summary>
		/// <param name="translationID">
		/// The ID to look up the translation for
		/// </param>
		/// <param name="defaultTranslation">
		/// The string to return if no translation can be found. Can be null or any string.
		/// </param>
		/// <param name="replacements">
		/// A collection of <see cref="System.Object"/>s to replace placeholders with
		/// </param>
		/// <returns>
		/// The translation, if one exists, or the supplied default with the placeholders replaced
		/// </returns>
		public static string GetTranslation(string translationID, string defaultTranslation, params object[] replacements)
		{
			string trans = GetTranslationFromTranslationSet(translationID);
			
			if (trans == null)
			{
				trans = GetDefaultTranslation(translationID, defaultTranslation);
			}

			trans = AddVariablesToTranslation(trans, replacements);

			return trans;
		}
		
		private static string GetTranslationFromTranslationSet(string translationID)
		{
			string translation = null;
			
			if (currentTranslations!=null)
			{
				translation = currentTranslations[translationID];
			}
			
			return translation;
		}
		
		private static string GetDefaultTranslation(string translationID, string defaultTranslation)
		{
			return (defaultTranslation != "" || defaultTranslation == null) ? defaultTranslation : GetMissingTranslationMessage(translationID);
		}

		private static string GetMissingTranslationMessage(string translationID)
		{
			return  "++ Missing Translation "+translationID+" ++";
		}
		
		private static string AddVariablesToTranslation(string translation, object[] replacements)
		{
			if (translation != null && replacements != null && replacements.Length > 0)
			{
				translation = String.Format(translation, replacements);
			}
			
			return translation;
		}

		/// <summary>
		/// Translate an <see cref="ITranslatable"/> item, with optional string replacement. If the translation
		/// does not exist then a warning message will be used as the translated text.
		/// </summary>
		/// <param name="item">
		/// A <see cref="ITranslatable"/> to set the text for
		/// </param>
		/// <param name="replacements">
		/// A collection of <see cref="System.Object"/>s that will be used to fill place-holders
		/// </param>
		public static void Translate(ITranslatable item, params object[] replacements)
		{
			Translate(item, GetMissingTranslationMessage(item.Name), replacements);
		}

		/// <summary>
		/// Translate an <see cref="ITranslatable"/> item, with optional string replacement. The <code>defaultText</code>
		/// can be used to specify an alternate translation. Passing <code>null</code> will result in a warning message
		/// about a missing translation ID.
		/// </summary>
		/// <param name="item">
		/// A <see cref="ITranslatable"/> to set the text for
		/// </param>
		/// <param name="defaultText">
		/// The default string to display if no translation could be found.
		/// </param>
		/// <param name="replacements">
		/// A collection of <see cref="System.Object"/>s that will be used to fill place-holders
		/// </param>
		public static void Translate(ITranslatable item, string defaultText, params object[] replacements)
		{
			if (item.Text == "")
			{
				//it doesn't need translating - either there is no text from the developer or it's a hyphen for a divider
				return;
			}

			item.Text = GetTranslation(item.Name, defaultText, replacements);
		}
		
		/// <summary>
		/// Get the current local translation language. This is the "language code" used by the translation and should match the ISO code for the language.
		/// </summary>
		/// <returns>
		/// The string used as the language code of the current local translation
		/// </returns>
		public static string GetTranslationLanguage()
		{
			return (currentTranslations != null ? currentTranslations.LanguageCode : "");
		}
		
		public static ICollection<TranslationLanguage> GetLanguages()
		{
			ICollection<TranslationLanguage> langs = new List<TranslationLanguage>();
			
			foreach (AbstractTranslationSet translations in langToTranslationMap.Values)
			{
				langs.Add(translations.Language);
			}
			
			return langs;
		}

		public static AbstractTranslationSet GetTranslationSet(string translationLanguage)
		{
			AbstractTranslationSet translations = null;
			
			if (translationLanguage != null)
			{
				translations = DictionaryUtils.GetValue(langToTranslationMap, translationLanguage);
				
				if (translations == null)
				{
					translations = new ModifiableTranslationSet(translationLanguage);
				}
			}
			
			return translations;
		}

		public static bool HasLanguage(string languageCode)
		{
			return langToTranslationMap.ContainsKey(languageCode);
		}
	}
}
