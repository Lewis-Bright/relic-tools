// This file is a part of the Relic Tools and is copyright 2006-2018 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU GPL license, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.
using System;

namespace IBBoard.Relic.RelicTools.Exceptions
{
	/// <summary>
	/// Summary description for FileNotFoundException.
	/// </summary>
	public class FileNotFoundException:Exception
	{
		private static readonly string DefaultMessage = "The file requested was not found in the folder specified.";

		public FileNotFoundException():base(DefaultMessage){}

		public FileNotFoundException(string message):base(message){}

		public FileNotFoundException(Exception innerException):base(DefaultMessage,innerException){}
	}
}
