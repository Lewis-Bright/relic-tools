// This file (TranslationSet.cs) is a part of the IBBoard project and is copyright 2010 IBBoard
// 
// The file and the library/program it is in are licensed and distributed, without warranty, under the GNU LGPL, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.

using System;
using System.Collections.Generic;

namespace IBBoard.Lang
{
	/// <summary>
	/// A set of translations in a given language that allow setting of translations as well as getting
	/// </summary>
	public class ModifiableTranslationSet : AbstractTranslationSet
	{		
		private AbstractTranslationSet parentTranslationSet;
		
		public ModifiableTranslationSet(string languageCode) : base(languageCode)
		{
			//Do nothing extra
		}
		
		public void SetTranslation(string key, string translation)
		{
			translations[key] = translation;
		}

		public void SetParentTranslations(ModifiableTranslationSet parentTranslations)
		{
			parentTranslationSet = parentTranslations;
		}
		
		protected override AbstractTranslationSet GetParentTranslations()
		{
			return parentTranslationSet;
		}

	}
}
