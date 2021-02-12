// This file (LogNotifier.cs) is a part of the IBBoard library and is copyright 2009 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU LGPL license, either version 3 of the License or (at your option) any later version. Please see COPYING.LGPL for more information and the full license.

using System;

namespace IBBoard.Logging
{
	public delegate void LogEventOccurredDelegate(Type logFromType, Object message, Exception e);	
	
	public class LogNotifier
	{
		public static event LogEventOccurredDelegate DebugLogEventOccurred;
		public static event LogEventOccurredDelegate InfoLogEventOccurred;
		public static event LogEventOccurredDelegate WarningLogEventOccurred;
		public static event LogEventOccurredDelegate ErrorLogEventOccurred;
		public static event LogEventOccurredDelegate FatalLogEventOccurred;
		
		public static void DebugFormat(Type logFromType, String message, params object[] vals)
		{
			Debug(logFromType, String.Format(message, vals));
		}
		
		public static void Debug(Type logFromType, Object message)
		{
			if (message is Exception)
			{
				Debug(logFromType, "", (Exception)message);
			}
			else
			{
				Debug(logFromType, message, null);
			}
		}
		
		public static void Debug(Type logFromType, Object message, Exception e)
		{
			if (DebugLogEventOccurred!=null)
			{
				DebugLogEventOccurred(logFromType, message, e);
			}
		}
		
		public static void InfoFormat(Type logFromType, String message, params object[] vals)
		{
			Info(logFromType, String.Format(message, vals));
		}
		
		public static void Info(Type logFromType, Object message)
		{
			if (message is Exception)
			{
				Info(logFromType, "", (Exception)message);
			}
			else
			{
				Info(logFromType, message, null);
			}
		}
		
		public static void Info(Type logFromType, Object message, Exception e)
		{
			if (InfoLogEventOccurred!=null)
			{
				InfoLogEventOccurred(logFromType, message, e);
			}
		}
		
		public static void WarnFormat(Type logFromType, String message, params object[] vals)
		{
			Warn(logFromType, String.Format(message, vals));
		}
		
		public static void Warn(Type logFromType, Object message)
		{
			if (message is Exception)
			{
				Warn(logFromType, "", (Exception)message);
			}
			else
			{
				Warn(logFromType, message, null);
			}
		}
		
		public static void Warn(Type logFromType, Object message, Exception e)
		{
			if (WarningLogEventOccurred!=null)
			{
				WarningLogEventOccurred(logFromType, message, e);
			}
		}
		
		public static void ErrorFormat(Type logFromType, String message, params object[] vals)
		{
			Error(logFromType, String.Format(message, vals));
		}
		
		public static void Error(Type logFromType, Object message)
		{	
			if (message is Exception)
			{
				Error(logFromType, "", (Exception)message);
			}
			else
			{
				Error(logFromType, message, null);
			}
		}
		
		public static void Error(Type logFromType, Object message, Exception e)
		{
			if (ErrorLogEventOccurred!=null)
			{
				ErrorLogEventOccurred(logFromType, message, e);
			}
		}
		
		public static void FatalFormat(Type logFromType, String message, params object[] vals)
		{
			Fatal(logFromType, String.Format(message, vals));
		}
		
		public static void Fatal(Type logFromType, Object message)
		{
			if (message is Exception)
			{
				Fatal(logFromType, "", (Exception)message);
			}
			else
			{
				Fatal(logFromType, message, null);
			}
		}
		
		public static void Fatal(Type logFromType, Object message, Exception e)
		{
			if (FatalLogEventOccurred!=null)
			{
				FatalLogEventOccurred(logFromType, message, e);
			}
		}
	}
}
