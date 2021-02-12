// This file (TranslationLanguage.cs) is a part of the IBBoard project and is copyright 2010 IBBoard
// 
// The file and the library/program it is in are licensed and distributed, without warranty, under the GNU LGPL, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.

using System;
using System.Globalization;

namespace IBBoard.Lang
{
	/// <summary>
	/// A simple object that holds the name and code pairing for a language
	/// </summary>
	public class TranslationLanguage
	{
		private string langCode;
		private string langName;
		
		public TranslationLanguage(string languageCode)
		{
			langCode = languageCode;
		}
		
		public string Name
		{
			get
			{
				if (langName == null)
				{
					LoadLangName();
				}
				
				return langName; 
			}
		}
		
		public string Code
		{
			get { return langCode; }
		}
		
		private void LoadLangName()
		{
			try
			{
				CultureInfo culture = CultureInfo.GetCultureInfo(langCode);
				langName = StringManipulation.UpperFirst(culture.NativeName);
			}
			catch (ArgumentException)
			{
				langName = "Unknown (" + langCode + ")";
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
				equal = Code.Equals(((TranslationLanguage)obj).Code);
			}
			
			return equal;
		}
		
		public override int GetHashCode()
		{
			return GetType().GetHashCode() + langCode.GetHashCode();
		}
	}
}
