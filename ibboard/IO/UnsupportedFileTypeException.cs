// This file (UnsupportedFileTypeException.cs) is a part of the IBBoard library and is copyright 2009 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU LGPL license, either version 3 of the License or (at your option) any later version. Please see COPYING.LGPL for more information and the full license.

using System;

namespace IBBoard.IO
{
	/// <summary>
	/// Summary description for UnsupportedFileTypeException.
	/// </summary>
	public class UnsupportedFileTypeException : Exception
	{
		public UnsupportedFileTypeException(): base("Operation attempted on an unsupported file type")
		{
		}

		public UnsupportedFileTypeException(string fileType):base("Operation attempted on an unsupported file type ("+fileType+")")
		{
		}

		public UnsupportedFileTypeException(string fileType, string operation):base("Operation "+operation+" does not support file type "+fileType)
		{
		}
	}
}
