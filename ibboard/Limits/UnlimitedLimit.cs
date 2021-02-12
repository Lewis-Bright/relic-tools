// This file (UnlimitedLimit.cs) is a part of the IBBoard project and is copyright 2009 IBBoard
// 
//  The file and the library/program it is in are licensed under the GNU LGPL license, either version 3 of the License or (at your option) any later version. Please see COPYING.LGPL for more information and the full license.

using System;

namespace IBBoard.Limits
{
	/// <summary>
	/// A special case "infinite" limit that always returns the integer number closest to infinity.
	/// </summary>
	public class UnlimitedLimit : AbstractLimit
	{
		public UnlimitedLimit () : base(double.PositiveInfinity)
		{
		}
				
		public override int GetLimit (int size)
		{
			return int.MaxValue;
		}
	}
}
