// This file (NumberParser.cs) is a part of the IBBoard utils library and is copyright 2009 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU LGPL license. Please see COPYING.LGPL for more information and the full license.

using System;

namespace IBBoard
{
	public class NumberParser
	{
		public static int ParseAsInt(string numberString, int defaultValue)
		{
			int parsedValue = defaultValue;
			int.TryParse(numberString, out parsedValue);
			return parsedValue;
		}
	}
}
