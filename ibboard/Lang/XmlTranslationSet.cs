//  This file (XmlTranslationSet.cs) is a part of the IBBoard project and is copyright 2010 IBBoard
// 
//  // The file and the library/program it is in are licensed and distributed, without warranty, under the GNU LGPL, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.

using System;
using System.Collections.Generic;
using System.Xml;

namespace IBBoard.Lang
{
	public class XmlTranslationSet : AbstractTranslationSet
	{
		private string parentLanguage;
		private XmlNodeList nodes;
		
		public XmlTranslationSet(String languageCode) : base(languageCode)
		{
			//Do nothing extra
		}
		
		public override string this[string key]
		{
			get
			{
				if (translations.Count == 0 && nodes != null)
				{
					PopulateTranslations();
				}
				
				return base[key];
			}
		}

		private void PopulateTranslations()
		{
			foreach (XmlElement node in nodes)
			{
				translations.Add(node.GetAttribute("id"), node.InnerText);
			}
		}
		
		public void SetParentLanguage(string parentLanguageCode)
		{
			parentLanguage = parentLanguageCode;
		}
		
		protected override AbstractTranslationSet GetParentTranslations()
		{
			return Translation.GetTranslationSet(parentLanguage);
		}
		
		public void SetTranslationNodes(XmlNodeList translationNodes)
		{
			nodes = translationNodes;
		}
	}
}
