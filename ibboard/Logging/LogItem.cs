// This file (LogItem.cs) is a part of the IBBoard library and is copyright 2009 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU LGPL license, either version 3 of the License or (at your option) any later version. Please see COPYING.LGPL for more information and the full license.

using System;

namespace IBBoard.Logging
{
	public class LogItem 
	{
		private LogLevel logLevel;
		private string logMessage, stacktrace;
		private DateTime occurance;

		public LogItem(LogLevel level, string message) : this(level, message, "")
		{
		}

		public LogItem(LogLevel level, string message, string stack)
		{
			logLevel = level;
			logMessage = message;
			occurance = DateTime.Now;
			stacktrace = stack;
		}

		public LogLevel Level
		{
			get { return logLevel; }
		}

		public string Message
		{
			get { return logMessage; }
		}

		public DateTime OccuranceTime
		{
			get { return occurance; }
		}

		public override string ToString()
		{
			return OccuranceTime.ToString()+" ("+Level+"): "+Message;
		}

		public string StackTrace
		{
			get { return stacktrace; }
		}
	}
}
