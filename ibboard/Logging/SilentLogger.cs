// This file (SilentLogger.cs) is a part of the IBBoard library and is copyright 2009 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU LGPL license, either version 3 of the License or (at your option) any later version. Please see COPYING.LGPL for more information and the full license.

using System;

namespace IBBoard.Logging
{
	/// <summary>
	/// Summary description for SilentLogger.
	/// </summary>
	public class SilentLogger : Logger
	{
		public SilentLogger() : base(null)
		{
		}

		protected override void LogMessage(LogItem item)
		{
			//Do nothing
		}
		
		protected override void LogMessageString (string str)
		{
			//Do nothing
		}

	}
}
