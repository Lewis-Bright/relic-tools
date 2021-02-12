// This file is a part of the Relic Tools and is copyright 2006-2018 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU GPL license, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.
using System;

namespace IBBoard.Relic.RelicTools.Exceptions
{
	/// <summary>
	/// Summary description for FileExistsException.
	/// </summary>
	public class FileExistsException:Exception
	{
		private static readonly string DefaultMessage = "The file to be extracted already existed at the specified location and \"no overwrite\" was specified.";
		private static readonly string DefaultMessageStart = "The file to be extracted (";
		private static readonly string DefaultMessageEnd = ") already existed at the specified location and \"no overwrite\" was specified.";

		public FileExistsException():base(DefaultMessage)
		{
		}

		public FileExistsException(Exception ex):base(DefaultMessage, ex)
		{
		}

		public FileExistsException(string filename):base(DefaultMessageStart+filename+DefaultMessageEnd)
		{
		}
	}
}
