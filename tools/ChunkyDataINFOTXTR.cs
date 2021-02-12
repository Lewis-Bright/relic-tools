// This file is a part of the Relic Tools and is copyright 2006-2018 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU GPL license, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.
using System;

namespace IBBoard.Relic.RelicTools
{
	/// <summary>
	/// Summary description for ChunkyDataINFOTXTR.
	/// </summary>
	public class ChunkyDataINFOTXTR :ChunkyDataINFO
	{
		int imageType = 0;
		int width = 0;
		int height = 0;
		int images = 0;

		public ChunkyDataINFOTXTR(int version_in, string name_in, byte[] innerData_in):base(version_in, name_in)
		{
			imageType = innerData_in[0]+(innerData_in[1]<<8)+(innerData_in[2]<<16)+(innerData_in[3]<<24);
			width = innerData_in[4]+(innerData_in[5]<<8)+(innerData_in[6]<<16)+(innerData_in[7]<<24);
			height = innerData_in[8]+(innerData_in[9]<<8)+(innerData_in[10]<<16)+(innerData_in[11]<<24);
			images = innerData_in[12]+(innerData_in[13]<<8)+(innerData_in[14]<<16)+(innerData_in[15]<<24);
		}

		public FileFormats.ImgType ImageType
		{
			get
			{
				if (imageType==5)
				{
					return FileFormats.ImgType.DXT1DDS;
				}
				else if (imageType == 7)
				{
					return FileFormats.ImgType.DXT1DDS;
				}
				else if (imageType == 0)
				{
					return FileFormats.ImgType.TGA;
				}
				else
				{
					return FileFormats.ImgType.Unknown;
				}
			}
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
				"Image Type:\t"+this.ImageType.ToString()+Environment.NewLine+
				"Width:\t\t"+width+Environment.NewLine+
				"Height:\t\t"+height+Environment.NewLine+
				"Number images(?):\t"+images;
		}

		public override int DataLength
		{
			get
			{
				return 16;
			}
		}

		public override byte[] GetDataBytes()
		{
			byte[] data = new byte[16];
			data[0] = (byte)imageType;
			data[1] = (byte)(imageType>>8);
			data[2] = (byte)(imageType>>16);
			data[3] = (byte)(imageType>>24);
			data[4] = (byte)width;
			data[5] = (byte)(width>>8);
			data[6] = (byte)(width>>16);
			data[7] = (byte)(width>>24);
			data[8] = (byte)height;
			data[9] = (byte)(height>>8);
			data[10] = (byte)(height>>16);
			data[11] = (byte)(height>>24);
			data[12] = (byte)images;
			data[13] = (byte)(images>>8);
			data[14] = (byte)(images>>16);
			data[15] = (byte)(images>>24);
			return data;
		}
	}
}
