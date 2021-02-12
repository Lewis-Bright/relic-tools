// This file is a part of the Relic Tools and is copyright 2006-2018 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU GPL license, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.
using System;

namespace IBBoard.Relic.RelicTools.Exceptions
{
	/// <summary>
	/// Summary description for Exception.
	/// </summary>
	public class Exception:System.Exception
	{
		private static readonly string DefaultMessage = "An unknown error has occured";

		public Exception():base(DefaultMessage)
		{
		}

		public Exception(string message):base(message)
		{}

		public Exception(Exception ex):base(DefaultMessage, ex)
		{
		}

		public Exception(string message, Exception ex):base(message, ex)
		{}
	}
}
