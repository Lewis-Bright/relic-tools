// This file (ITranslatable.cs) is a part of the IBBoard library and is copyright 2009 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU LGPL license, either version 3 of the License or (at your option) any later version. Please see COPYING.LGPL for more information and the full license.

using System;

namespace IBBoard.Lang
{
	/// <summary>
	/// Summary description for ITranslatable.
	/// </summary>
	public interface ITranslatable
	{
		string Name{ get; }

		string Text{ get; set;}
	}
}
