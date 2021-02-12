// This file (StringManipulation.cs) is a part of the IBBoard library and is copyright 2009 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU LGPL license, either version 3 of the License or (at your option) any later version. Please see COPYING.LGPL for more information and the full license.

using System;
using System.Text;

namespace IBBoard.Lang
{
	/// <summary>
	/// Summary description for StringManipulation.
	/// </summary>
	public class StringManipulation
	{		
		private static Encoding utf8;

		static StringManipulation()
		{
			utf8 = Encoding.UTF8;
		}

		public static byte[] StringToBytes(string str)
		{
			return utf8.GetBytes(str);
		}

		public static string ByteArrayToHexString(byte[] arr)
		{
			return ByteArrayToHexString(arr, true);
		}		                                          

		public static string ByteArrayToHexString(byte[] arr, bool spaceBytes)
		{
			StringBuilder sb = new StringBuilder(arr.Length);
			string format = (spaceBytes ? "{0,-3:x2}" : "{0:x2}");

			foreach (byte b in arr)
			{
				sb.Append(String.Format(format, b));
			}

			return sb.ToString().TrimEnd();
		}

		public static string RemoveFromLast(string stringToTrim, char removeFrom)
		{
			return stringToTrim.Substring(0, stringToTrim.LastIndexOf(removeFrom));
		}

		public static string CutToLength(string stringToShorten, int length)
		{
			int strLength = stringToShorten.Length;

			if (length >= strLength-2)
			{
				return stringToShorten;
			}
			else
			{
				int diff = (strLength - length) / 2;
				int halfLength = strLength / 2;
				return stringToShorten.Substring(0, halfLength - diff)+"..."+stringToShorten.Substring(halfLength + diff);
			}
		}

		public static string UpperFirst(string toUpper)
		{
			if (string.IsNullOrEmpty(toUpper))
			{
				return string.Empty;
			}

			char[] chars = toUpper.ToCharArray();
			chars[0] = char.ToUpper(chars[0]);
			return new string(chars);
		}
	}
}
