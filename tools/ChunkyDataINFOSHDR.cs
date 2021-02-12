// This file is a part of the Relic Tools and is copyright 2006-2018 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU GPL license, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.
using System;

namespace IBBoard.Relic.RelicTools
{
	/// <summary>
	/// Summary description for ChunkyDataINFOSHDR.
	/// </summary>
	public class ChunkyDataINFOSHDR :ChunkyDataINFO
	{
		int numChannels = 0;
		byte[] unknown1 = new byte[4];
		byte[] unknown2 = new byte[4];
		byte[] unknown3 = new byte[5];

		public ChunkyDataINFOSHDR(int version_in, string name_in, byte[] innerData_in):base(version_in, name_in)
		{
			numChannels = innerData_in[0]+(innerData_in[1]<<8)+(innerData_in[2]<<16)+(innerData_in[3]<<24);
			unknown1[0] = innerData_in[4];
			unknown1[1] = innerData_in[5];
			unknown1[2] = innerData_in[6];
			unknown1[3] = innerData_in[7];
			unknown2[0] = innerData_in[8];
			unknown2[1] = innerData_in[9];
			unknown2[2] = innerData_in[10];
			unknown2[3] = innerData_in[11];
			unknown3[0] = innerData_in[12];
			unknown3[1] = innerData_in[13];
			unknown3[2] = innerData_in[14];
			unknown3[3] = innerData_in[15];
			unknown3[4] = innerData_in[16];
		}

		public bool AdditionalShaders
		{
			get
			{
				return (unknown2[0]==0xcd);
			}
		}

		public override string GetDisplayDetails()
		{		
			return base.GetBaseDisplayDetails()+Environment.NewLine+
				"------------"+Environment.NewLine+
				"Channels:\t"+numChannels+Environment.NewLine+
				"Unknown 1:\t"+ChunkyChunk.ByteArrayToString(unknown1)+Environment.NewLine+
				//"Additional Shaders(?):\t"+AdditionalShaders+Environment.NewLine+
				"Unknown 2:\t"+ChunkyChunk.ByteArrayToString(unknown2)+Environment.NewLine+
				"Unknown 3:\t"+ChunkyChunk.ByteArrayToString(unknown3);
		}

		public override int DataLength
		{
			get
			{
				return 17;
			}
		}

		public override byte[] GetDataBytes()
		{
			byte[] data = new byte[17];
			data[0] = (byte)numChannels;
			data[1] = (byte)(numChannels>>8);
			data[2] = (byte)(numChannels>>16);
			data[3] = (byte)(numChannels>>24);
			data[4] = unknown1[0];
			data[5] = unknown1[1];
			data[6] = unknown1[2];
			data[7] = unknown1[3];
			data[8] = unknown2[0];
			data[9] = unknown2[1];
			data[10] = unknown2[2];
			data[11] = unknown2[3];
			data[12] = unknown3[0];
			data[13] = unknown3[1];
			data[14] = unknown3[2];
			data[15] = unknown3[3];
			data[16] = unknown3[4];
			return data;
		}


	}
}
