// This file (TextFileLogger.cs) is a part of the IBBoard library and is copyright 2009 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU LGPL license, either version 3 of the License or (at your option) any later version. Please see COPYING.LGPL for more information and the full license.

using System;
using System.IO;
using IBBoard;

namespace IBBoard.Logging
{
	/// <summary>
	/// Summary description for FileLogger.
	/// </summary>
	public class TextFileLogger : FileLogger
	{
		public TextFileLogger() : base()
		{
		}
		
		public TextFileLogger(string path) : base(path)
		{
		}

		protected override void LogMessage(LogItem item)
		{
			string stack = item.StackTrace;
			string message = item.Message + Environment.NewLine + (stack!= "" ? stack + Environment.NewLine : "");
			LogMessageString(message);
		}
		
		protected override void LogMessageString (string str)
		{
			str.TrimEnd();
			str+= Environment.NewLine + Environment.NewLine;
			stream.Write(encoding.GetBytes(str), 0, encoding.GetByteCount(str));
			stream.Flush();
		}

	}
}
