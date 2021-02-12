// This file is a part of the Relic Tools and is copyright 2006-2018 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU GPL license, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.
using System;

namespace IBBoard.Relic.RelicTools.Exceptions
{
	/// <summary>
	/// Summary description for InvalidUseException.
	/// </summary>
	public class InvalidUseException:Exception
	{
		private static readonly string DefaultMessage = "One of the methods or classes was used incorrectly or instantiated in a non-standard way.";

		public InvalidUseException():base(DefaultMessage)
		{
		}

		public InvalidUseException(Exception ex):base(DefaultMessage, ex)
		{
		}

		public InvalidUseException(string message):base(message){}

		public InvalidUseException(string message, Exception innerException):base(message, innerException){}
	}
}
