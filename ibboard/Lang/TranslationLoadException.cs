//  This file (TranslationLoadException.cs) is a part of the IBBoard project and is copyright 2009 IBBoard
// 
//  The file and the library/program it is in are licensed under the GNU LGPL license, either version 3 of the License or (at your option) any later version. Please see COPYING.LGPL for more information and the full license.
// 

using System;
using System.IO;

namespace IBBoard.Lang
{
	/// <summary>
	/// A sub-class of Exceptions that is thrown when translations fail to load.
	/// </summary>
	public class TranslationLoadException : Exception
	{		
		public TranslationLoadException(string message) : base(message)
		{
		}
		
		public TranslationLoadException(string message, Exception ex) : base(message, ex)
		{
		}
	}
}
