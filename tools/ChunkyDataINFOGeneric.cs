// This file is a part of the Relic Tools and is copyright 2006-2018 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU GPL license, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.
using System;

namespace IBBoard.Relic.RelicTools
{
	/// <summary>
	/// Summary description for ChunkyDataINFOGeneric.
	/// </summary>
	public class ChunkyDataINFOGeneric : ChunkyDataINFO
	{
		byte[] data;

		public ChunkyDataINFOGeneric(int version_in, string name_in, byte[] innerData_in):base(version_in, name_in)
		{
			data = innerData_in;
		}

		public override int DataLength
		{
			get
			{
				return data.Length;
			}
		}

		public override byte[] GetDataBytes()
		{
			return data;
		}

		public override string GetDisplayDetails()
		{		
			return base.GetBaseDisplayDetails()+Environment.NewLine+
				"------------"+Environment.NewLine+
				ByteArrayToString(GetDataBytes());
		}
	}
}
