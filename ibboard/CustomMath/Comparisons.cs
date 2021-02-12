// This file (CustomMath.cs) is a part of the IBBoard library and is copyright 2009 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU LGPL license, either version 3 of the License or (at your option) any later version. Please see COPYING.LGPL for more information and the full license.

using System;

namespace IBBoard.CustomMath
{
	/// <summary>
	/// Summary description for Comparisons.
	/// </summary>
	public class Comparisons
	{
		public static bool ValueWithinAmount(int baseline, int val, int delta)
		{
			int top = baseline + delta;
			int bottom = baseline - delta;

			return (val<=top && val>=bottom);
		}
	}
}
