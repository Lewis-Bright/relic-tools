// This file is a part of the Relic Tools and is copyright 2006-2018 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU GPL license, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.
using System;

namespace IBBoard.Relic.RelicTools.Exceptions
{
	/// <summary>
	/// Summary description for FileNotZippedException.
	/// </summary>
	public class FileNotZippedException:Exception
	{
		private static readonly string DefaultMessage = "Unzipping was attempted on a file that does not appear to be compressed.";

		public FileNotZippedException():base(DefaultMessage)
		{
		}

		public FileNotZippedException(Exception ex):base(DefaultMessage, ex)
		{
		}
	}
}
