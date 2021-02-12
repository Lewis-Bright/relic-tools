//  This file (TranslationXmlExtractor.cs) is a part of the IBBoard project and is copyright 2010 IBBoard
// 
//  The file and the library/program it is in are licensed and distributed, without warranty, under the GNU LGPL, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.

using System;
using System.Collections.Generic;
using System.Xml;

namespace IBBoard.Lang
{
	public class TranslationXmlExtractor
	{		
		public XmlNodeList GetTranslationNodes(XmlDocument doc)
		{
			try
			{
				return doc.GetElementsByTagName("translation");
			}
			catch(Exception ex)
			{
				throw new TranslationLoadException("Error while parsing " + GetLanguageOfDocument(doc)+" translation: "+ex.Message, ex);
			}
		}
		
		public string GetLanguageOfDocument(XmlDocument doc)
		{
			return doc != null ? doc.DocumentElement.GetAttribute("lang") : "";
		}
		
		public string GetParentLanguageOfDocument(XmlDocument doc)
		{
			return doc != null ? doc.DocumentElement.GetAttribute("extends") : "";
		}
	}
}
