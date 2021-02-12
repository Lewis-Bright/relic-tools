// This file (InvalidFileException.cs) is a part of the IBBoard library and is copyright 2009 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU LGPL license, either version 3 of the License or (at your option) any later version. Please see COPYING.LGPL for more information and the full license.

using System;

namespace IBBoard.IO
{
	/// <summary>
	/// An exception thrown when the content of a file does not meet the requirements of that file.
	/// </summary>
	public class InvalidFileException : Exception
	{
		public InvalidFileException() : base() {}

		public InvalidFileException(string message) : base(message) {}

		public InvalidFileException(string message, Exception innerException) : base(message, innerException) {}
	}
}
