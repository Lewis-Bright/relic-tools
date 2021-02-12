// This file (UnitTimestamp.cs) is a part of the IBBoard library and is copyright 2009 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU LGPL license, either version 3 of the License or (at your option) any later version. Please see COPYING.LGPL for more information and the full license.

using System;

namespace IBBoard
{
	/// <summary>
	/// Summary description for UnixTimestamp.
	/// </summary>
	public class UnixTimestamp
	{
		private long stamp;
		private static DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToUniversalTime();

		public UnixTimestamp(DateTime date)
		{
			stamp = GetTimestamp(date);
		}

		public UnixTimestamp(long timestamp)
		{
			stamp = timestamp;
		}

		public DateTime GetDate()
		{
			return unixEpoch.AddSeconds(stamp);
		}

		public DateTime GetDate(int timestamp)
		{
			return unixEpoch.AddSeconds(timestamp);
		}

		public long GetTimestamp()
		{
			return stamp;
		}

		public static long GetTimestamp(DateTime date)
		{
			TimeSpan span = date.ToUniversalTime() - unixEpoch;
			return (long)span.TotalSeconds;
		}
	}
}
