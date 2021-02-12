// This file is a part of the Relic Tools and is copyright 2006-2018 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU GPL license, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.
using System;

namespace IBBoard.Relic.RelicTools.Exceptions
{
	/// <summary>
	/// Summary description for InvalidChunkValueException.
	/// </summary>
	public class InvalidChunkValueException : InvalidChunkException
	{
		string field;
		object val;

		public InvalidChunkValueException(string message, string fieldName, object valueFound): base(message)
		{
			field = fieldName;
			val = (valueFound==null ? "" : valueFound);
		}

		public string FieldName
		{
			get { return field; }
		}

		public object Value
		{
			get { return val; }
		}
	}
}
