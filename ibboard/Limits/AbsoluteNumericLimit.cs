// This file (AbsoluteNumericLimit.cs) is a part of the IBBoard project and is copyright 2009 IBBoard
// 
//  The file and the library/program it is in are licensed under the GNU LGPL license, either version 3 of the License or (at your option) any later version. Please see COPYING.LGPL for more information and the full license.

using System;

namespace IBBoard.Limits
{
	/// <summary>
	/// A limit of a specified number that does not vary with the number it is limited against.
	/// </summary>
	public class AbsoluteNumericLimit : AbstractLimit, INumericLimit
	{
		public AbsoluteNumericLimit (int numericLimit) : base(numericLimit)
		{
		}
				
		public override int GetLimit (int size)
		{
			return (int)Limit;
		}
	}
}
