// This file (TranslationXmlLoader.cs) is a part of the IBBoard project and is copyright 2010 IBBoard
// 
// The file and the library/program it is in are licensed and distributed, without warranty, under the GNU LGPL, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.
using System;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Collections.Generic;
using IBBoard.IO;
using IBBoard.Xml;
using System.Reflection;

namespace IBBoard.Lang
{
	/// <summary>
	/// A simple loader of translations from XML files
	/// </summary>
	public class TranslationXmlLoader
	{
		private XmlReaderSettings settings;
		private TranslationXmlExtractor extractor;

		public TranslationXmlLoader()
		{
			extractor = new TranslationXmlExtractor();
		}

		public AbstractTranslationSet LoadTranslations(string path)
		{
			FileInfo file = new FileInfo(path);

			if (!file.Exists)
			{
				throw new TranslationLoadException("Translation file " + file.FullName + " did not exist");
			}				

			XmlDocument doc = LoadTranslationDocument(file);
			XmlTranslationSet translations = new XmlTranslationSet(extractor.GetLanguageOfDocument(doc));
			translations.SetParentLanguage(extractor.GetParentLanguageOfDocument(doc));
			translations.SetTranslationNodes(extractor.GetTranslationNodes(doc));
			return translations;
		}

		private XmlDocument LoadTranslationDocument(FileInfo file)
		{
			XmlDocument doc = new XmlDocument();			
			XmlReader valReader = XmlReader.Create(file.OpenRead(), GetReaderSettings());

			try
			{
				doc.Load(valReader);
			}
			catch (DirectoryNotFoundException ex)
			{
				throw new TranslationLoadException("Problem validating schema for translation: " + ex.Message, ex);
			}
			catch (XmlSchemaException ex)
			{
				throw new TranslationLoadException("Problem validating schema for translation: " + ex.Message, ex);
			}
			catch (XmlException ex)
			{
				throw new TranslationLoadException("Problem reading data for translation: " + ex.Message, ex);
			}
			finally
			{
				valReader.Close();
			}

			return doc;
		}

		/// <summary>
		/// Lazy-getter for XML reader settings. May throw a <see cref="TranslationLoadException"/> if there is a problem with the translation schema.
		/// </summary>
		/// <returns>
		/// A <see cref="XmlReaderSettings"/> with the default values for validating the translation document against the translation schema
		/// </returns>
		private XmlReaderSettings GetReaderSettings()
		{
			if (settings == null)
			{
				try
				{
					settings = new XmlReaderSettings();
					settings.ValidationType = ValidationType.Schema;
					settings.ValidationFlags = XmlSchemaValidationFlags.ReportValidationWarnings;
					settings.ValidationEventHandler += new ValidationEventHandler(ValidationEventMethod);
					XmlSchemaSet cache = new XmlSchemaSet();
					XmlTools.AddSchemaToSetFromResource(cache, "http://ibboard.co.uk/translation", Assembly.GetExecutingAssembly(), "IBBoard.schemas.translation.xsd");
					settings.Schemas.Add(cache);
				}
				catch (DirectoryNotFoundException ex)
				{
					throw new TranslationLoadException("Problem validating schema for translation: " + ex.Message, ex);
				}
				catch (XmlSchemaException ex)
				{
					throw new TranslationLoadException("Problem validating schema for translation: " + ex.Message, ex);
				}
				catch (XmlException ex)
				{
					throw new TranslationLoadException("Problem reading data for schema: " + ex.Message, ex);
				}
			}

			return settings;
		}

		private void ValidationEventMethod(object sender, ValidationEventArgs e)
		{
			throw new TranslationLoadException("Problem validating schema for translation: " + e.Exception.Message, e.Exception);
		}
	}
}
