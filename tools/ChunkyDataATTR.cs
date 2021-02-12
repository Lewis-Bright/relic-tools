// This file is a part of the Relic Tools and is copyright 2006-2018 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU GPL license, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.
using System;
using System.IO;
using IBBoard.Relic.RelicTools.Exceptions;

namespace IBBoard.Relic.RelicTools
{
	/// <summary>
	/// Summary description for ChunkyDataDATA.
	/// </summary>
	public class ChunkyDataATTR : ChunkyData
	{

		int width = 0;
		int height = 0;
		int mipmaps = 0;
		int imagetype = 0;

		public ChunkyDataATTR(int version_in, string name_in, byte[] innerData_in):base("ATTR", version_in, name_in)
		{
			imagetype = innerData_in[0]+(innerData_in[1]<<8)+(innerData_in[2]<<16)+(innerData_in[3]<<24);
			width = innerData_in[4]+(innerData_in[5]<<8)+(innerData_in[6]<<16)+(innerData_in[7]<<24);
			height = innerData_in[8]+(innerData_in[9]<<8)+(innerData_in[10]<<16)+(innerData_in[11]<<24);

			if (innerData_in.Length>12)
			{
				mipmaps = innerData_in[12]+(innerData_in[13]<<8)+(innerData_in[14]<<16)+(innerData_in[15]<<24);
			}
			else
			{
				mipmaps = 0;
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

		public int MipMaps
		{
			get
			{
				return mipmaps;
			}
		}

		public int ImageType
		{
			get
			{
				return imagetype;
			}
		}

		public string ImageTypeString
		{
			get
			{
				switch(imagetype)
				{
					case 0: return "TGA";
					case 8: return "DXT1 DDS";
					case 10: return "DXT3 DDS";
					case 11: return "DXT5 DDS";
					default: return "Unknown";
				}
			}
		}

		public override string GetDisplayDetails()
		{
			return base.GetBaseDisplayDetails()+Environment.NewLine+
				"------------"+Environment.NewLine+
				"Image Type:\t"+ImageTypeString+Environment.NewLine+
				"Width:\t\t"+width+Environment.NewLine+
				"Height:\t\t"+height+Environment.NewLine+
				(ImageType!=0?"Mip Maps:\t"+mipmaps:"Unknown:\t"+mipmaps);
		}

		public override int DataLength
		{
			get
			{
				return (imagetype == 0 && mipmaps==0)?12:16;
			}
		}

		public override byte[] GetDataBytes()
		{
			byte[] data = new byte[DataLength];
			int pos = 0;

			data[pos++] = (byte)imagetype;
			data[pos++] = (byte)(imagetype>>8);
			data[pos++] = (byte)(imagetype>>16);
			data[pos++] = (byte)(imagetype>>24);
			data[pos++] = (byte)width;
			data[pos++] = (byte)(width>>8);
			data[pos++] = (byte)(width>>16);
			data[pos++] = (byte)(width>>24);
			data[pos++] = (byte)width;
			data[pos++] = (byte)(width>>8);
			data[pos++] = (byte)(width>>16);
			data[pos++] = (byte)(width>>24);

			if (DataLength>12)
			{
				data[pos++] = (byte)imagetype;
				data[pos++] = (byte)(imagetype>>8);
				data[pos++] = (byte)(imagetype>>16);
				data[pos++] = (byte)(imagetype>>24);
			}

			return data;
		}

	}
}
