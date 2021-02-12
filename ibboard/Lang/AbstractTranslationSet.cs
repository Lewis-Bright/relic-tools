// This file (AbstractTranslationSet.cs) is a part of the IBBoard project and is copyright 2010 IBBoard
// 
// The file and the library/program it is in are licensed and distributed, without warranty, under the GNU LGPL, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.

using System;
using System.Collections.Generic;

namespace IBBoard.Lang
{
	/// <summary>
	/// A collection of translations for a given language. The abstract class must be extended by implementations that
	/// provide different ways of loading the data.
	/// </summary>
	public abstract class AbstractTranslationSet
	{
		private TranslationLanguage language;
		protected Dictionary<string, string> translations;
		
		public AbstractTranslationSet(string languageCode)
		{
			language = new TranslationLanguage(languageCode);
			translations = new Dictionary<string, string>();
		}
		
		
		/// <summary>
		/// Gets the language code that this translation claims to be for
		/// </summary>
		public string LanguageCode
		{
			get { return language.Code; }
		}
		
		/// <summary>
		/// Gets a translation from the translation set, or <code>null</code> if the translation doesn't exist.
		/// </summary>
		/// <param name="key">
		/// The key (ID) of the translation to retrieve
		/// </param>
		public virtual string this[string key]
		{
			get
			{
				string translation = DictionaryUtils.GetValue(translations, key);
				
				if (translation == null)
				{
					translation = GetParentTranslation(key);
				}
				
				return translation;
			}
		}

		private string GetParentTranslation(string key)
		{
			AbstractTranslationSet parentTranslations = GetParentTranslations();
			string parentTranslation = null;
			
			if (parentTranslations != null)
			{
				CheckForLooping(parentTranslations);
				parentTranslation = parentTranslations[key];
			}
			
			return parentTranslation;
		}

		protected abstract AbstractTranslationSet GetParentTranslations();
		
		private void CheckForLooping(AbstractTranslationSet translations)
		{
			bool hasLoop = false;
			AbstractTranslationSet slowLoop = translations;
			AbstractTranslationSet fastLoop1 = translations;
			AbstractTranslationSet fastLoop2 = translations;
			
			while (slowLoop != null && (fastLoop1 = fastLoop2.GetParentTranslations()) != null && (fastLoop2 = fastLoop1.GetParentTranslations()) != null)
			{
				if (slowLoop.Equals(fastLoop1) || slowLoop.Equals(fastLoop2))
				{
					hasLoop = true;
					break;
				}
				
				slowLoop = slowLoop.GetParentTranslations();
			}
			
			if (hasLoop)
			{
				throw new TranslationLoadException("Translations contained an inheritence loop");
			}
		}
		
		public string LanguageName
		{
			get
			{
				return language.Name;
			}
		}

		public TranslationLanguage Language
		{
			get { return language; }
		}
		
		public TranslationLanguage ParentLanguage
		{
			get
			{
				TranslationLanguage parentLang = null;
				AbstractTranslationSet parentTranslations = GetParentTranslations();
				
				if (parentTranslations != null)
				{
					parentLang = parentTranslations.Language;
				}
				
				return parentLang;
			}
		}
		
		public override bool Equals(object obj)
		{
			bool equal = true;
			
			if (obj == null || !obj.GetType().Equals(GetType()))
			{
				equal = false;
			}
			else
			{
				equal = LanguageCode.Equals(((AbstractTranslationSet)obj).LanguageCode);
			}
			
			return equal;
		}
		
		public override int GetHashCode ()
		{
			return GetType().GetHashCode() + LanguageCode.GetHashCode();
		}
	}
}
