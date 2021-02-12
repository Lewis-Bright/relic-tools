//  This file (XmlTools.cs) is a part of the IBBoard library and is copyright 2009 IBBoard
// 
//  The file and the library/program it is in are licensed under the GNU LGPL license, either version 3 of the License or (at your option) any later version. Please see COPYING.LGPL for more information and the full license.
// 

using System;

namespace IBBoard
{
	public class EnumTools
	{
		/// <summary>
		/// Takes a string and parses it as the required enum, returning the typed enum object. Parsing is case-insensitive. Throws a <code>ArgumentException</code> if the argument is not a valid enum value.
		/// </summary>
		/// <param name="enumString">
		/// The enum text string to parse
		/// </param>
		/// <returns>
		/// A typed enum value parsed from the text string
		/// </returns>
		public static T ParseEnum<T>(string enumString)
		{
			return ParseEnum<T>(enumString, true);
		}
		
		/// <summary>
		/// Takes a string and parses it as the required enum, returning the typed enum object. Parsing can be case-sensitive or case-insensitive. Throws a <code>ArgumentException</code> if the argument is not a valid enum value.
		/// </summary>
		/// <param name="enumString">
		/// The enum text string to parse
		/// </param>
		/// <param name="ignoreCase">
		/// <code>True</code> if parsing should be case-insensitive, else <code>false</code>
		/// </param>
		/// <returns>
		/// A typed enum value parsed from the text string
		/// </returns>
		public static T ParseEnum<T>(string enumString, bool ignoreCase)
		{
			return (T) Enum.Parse(typeof (T), enumString, ignoreCase);
		}
	}
}
