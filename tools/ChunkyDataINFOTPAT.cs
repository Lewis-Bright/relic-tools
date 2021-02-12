// This file is a part of the Relic Tools and is copyright 2006-2018 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU GPL license, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.
using System;

namespace IBBoard.Relic.RelicTools
{
	/// <summary>
	/// Summary description for ChunkyDataINFOTPAT.
	/// </summary>
	public class ChunkyDataINFOTPAT : ChunkyDataINFO
	{
		int width = 0;
		int height = 0;

		public ChunkyDataINFOTPAT(int version_in, string name_in, byte[] innerData_in):base(version_in, name_in)
		{
			width = innerData_in[0]+(innerData_in[1]<<8)+(innerData_in[2]<<16)+(innerData_in[3]<<24);
			height= innerData_in[4]+(innerData_in[5]<<8)+(innerData_in[6]<<16)+(innerData_in[7]<<24);
		}

		public int Width
		{
			get
			{
				return width;
			}
		}

		public int Height
		{
			get
			{
				return height;
			}
		}

		public override string GetDisplayDetails()
		{		
			return base.GetBaseDisplayDetails()+Environment.NewLine+
				"------------"+Environment.NewLine+
				"Width:\t\t"+width+Environment.NewLine+
				"Height:\t\t"+height;
		}

		public override int DataLength
		{
			get
			{
				return 8;
			}
		}

		public override byte[] GetDataBytes()
		{
			byte[] data = new byte[8];
			data[0] = (byte)width;
			data[1] = (byte)(width>>8);
			data[2] = (byte)(width>>16);
			data[3] = (byte)(width>>24);
			data[4] = (byte)height;
			data[5] = (byte)(height>>8);
			data[6] = (byte)(height>>16);
			data[7] = (byte)(height>>24);
			return data;
		}
	}
}
