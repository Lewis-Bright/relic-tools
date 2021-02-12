// This file (Logger.cs) is a part of the IBBoard library and is copyright 2009 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU LGPL license, either version 3 of the License or (at your option) any later version. Please see COPYING.LGPL for more information and the full license.

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace IBBoard.Logging
{
	public enum LogLevel {Debug = 1, Info = 2, Warning = 3, Error = 4, Critical = 5}
	/// <summary>
	/// Summary description for Logger.
	/// </summary>
	public abstract class Logger : TextWriter
	{

		protected LogLevel logLevel;
		protected Stream stream;
		protected UTF8Encoding encoding = new UTF8Encoding();
		private List<LogItem> logMessages = new List<LogItem>();

		public delegate void LogUpdatedDelegate(LogItem item);
		public event LogUpdatedDelegate LogUpdatedEvent;

		protected Logger(Stream logStream)
		{
			logLevel = LogLevel.Error;
			stream = logStream;	
			
			//null stream means we're not actually logging out so we don't need to check it is writable
			if (stream!=null)
			{
				if (!stream.CanWrite)
				{
					throw new ArgumentException("Log stream was not writable");
				}
			}
			
			LogMessageString("Log started at " + String.Format("{0:HH:mm:ss, yyyy-MM-dd}", DateTime.Now));
		}
		
		~Logger()
		{
			LogMessageString("Log closed at " + String.Format("{0:HH:mm:ss, yyyy-MM-dd}", DateTime.Now));
		}

		public LogLevel LogLevel
		{
			get { return logLevel; }
			set { logLevel = value; }
		}

		public void Log(Exception ex, LogLevel level)
		{
			Log(ex.Message, ex.StackTrace, level);
		}

		public void Log(string message, LogLevel level)
		{	
			Log(message, "", level);
		}

		private void Log(string message, string stacktrace, LogLevel level)
		{
			if (level >= LogLevel)
			{		
				LogItem item = new LogItem(level, message, stacktrace);
				LogMessage(item);
				logMessages.Add(item);
				OnLogUpdated(item);
			}
		}

		private void OnLogUpdated(LogItem item)
		{
			if (LogUpdatedEvent!=null)
			{
				LogUpdatedEvent(item);
			}
		}

		protected abstract void LogMessage(LogItem item);
		protected abstract void LogMessageString(string str);

		public void ResetInternalLog()
		{
			logMessages = new List<LogItem>();
			LogMessageString("Log reset at " + String.Format("{0:yyyy-MM-dd-HHmmss}", DateTime.Now));
		}

		public int LogLength
		{
			get { return logMessages.Count; }
		}

		public LogItem GetLogItem(int index)
		{
			if (index < LogLength)
			{
				return logMessages[index];
			}
			else
			{
				throw new IndexOutOfRangeException();
			}
		}

		public LogItem[] GetLogItems(LogLevel minLogLevel)
		{
			List<LogItem> logItems = new List<LogItem>();

			foreach (LogItem item in logMessages)
			{
				if (item.Level>=minLogLevel)
				{
					logItems.Add(item);
				}
			}

			return logItems.ToArray();
		}
		
		//Allow the Logger to be used as the output point for System.Console.Error. In this case assume all messages are Error level
		public override Encoding Encoding {
			get { return encoding; }
		}
		
		public override void Write (object value)
		{
			if (value is Exception)
			{
				Log((Exception)value, LogLevel.Error);
			}
		}

		public override void Write (string value)
		{
			Log(value, LogLevel.Error);
		}

		public override void WriteLine(object value)
		{
			Write(value);
		}
		
		public override void WriteLine (string value)
		{
			Write(value);	
		}

	}
}
