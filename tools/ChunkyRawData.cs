// This file is a part of the Relic Tools and is copyright 2006-2018 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU GPL license, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.
using System;
using System.IO;

namespace IBBoard.Relic.RelicTools
{
	/// <summary>
	/// Summary description for ChunkyRawData.
	/// </summary>
	public class ChunkyRawData : ChunkyChunk
	{
		byte[] innerData;

		public ChunkyRawData(byte[] bytes):base(ChunkyChunkType.RawData, "", 0, "")
		{
			innerData = bytes;
		}

		protected string GetBaseDisplayDetails()
		{
			return base.GetDisplayDetails();
		}

		public override byte[] GetBytes()
		{
			return innerData;
		}

		public override int DataLength
		{
			get { return innerData.Length; }
		}

		public byte[] GetDataBytes()
		{
			return innerData;
		}

		public override bool Savable
		{
			get{return false;}
		}

		public override string GetValidationString()
		{
			return "";
		}
	}
}
