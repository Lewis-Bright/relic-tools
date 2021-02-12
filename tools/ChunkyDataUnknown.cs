// This file is a part of the Relic Tools and is copyright 2006-2018 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU GPL license, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.
using System;

namespace IBBoard.Relic.RelicTools
{
	/// <summary>
	/// Summary description for ChunkyDataUnknown.
	/// </summary>
	public class ChunkyDataUnknown : ChunkyData
	{
		byte[] innerData;
		public ChunkyDataUnknown(string id, int version_in, string name_in, byte[] innerData_in):base(id, version_in, name_in)
		{
			innerData = innerData_in;
		}

		public override byte[] GetDataBytes()
		{
			return innerData;
		}

		public override int DataLength
		{
			get { return innerData.Length; }
		}

		public override string GetDisplayDetails()
		{		
			return base.GetBaseDisplayDetails()+Environment.NewLine+
				"------------"+Environment.NewLine+
				ByteArrayToString(GetDataBytes());
		}
	}
}
