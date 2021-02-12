// This file is a part of the Relic Tools and is copyright 2006-2018 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU GPL license, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.
using System;

namespace IBBoard.Relic.RelicTools
{
	/// <summary>
	/// Summary description for ChunkyDataHEAD.
	/// </summary>
	public class ChunkyDataHEAD :ChunkyData
	{
		int imageType = 0;
		int numImages = 0;

		public ChunkyDataHEAD(int version_in, string name_in, byte[] innerData_in):base("HEAD", version_in, name_in)
		{
			imageType = innerData_in[0]+(innerData_in[1]<<8)+(innerData_in[2]<<16)+(innerData_in[3]<<24);
			numImages = innerData_in[4]+(innerData_in[5]<<8)+(innerData_in[6]<<16)+(innerData_in[7]<<24);
		}

		public FileFormats.ImgType ImageType
		{
			get
			{
				if (imageType==5)
				{
					return FileFormats.ImgType.DXT1DDS;
				}
				else if (imageType == 6)
				{
					return FileFormats.ImgType.DXT3DDS;
				}
				else if (imageType == 7)
				{
					return FileFormats.ImgType.DXT5DDS;
				}
				else if (imageType == 0 || imageType == 2)
				{
					return FileFormats.ImgType.TGA;
				}
				else
				{
					return FileFormats.ImgType.Unknown;
				}
			}
		}

		public override string GetDisplayDetails()
		{		
			return base.GetBaseDisplayDetails()+Environment.NewLine+
				"------------"+Environment.NewLine+
				"Image Type:\t\t"+ImageType.ToString()+Environment.NewLine+
				"Number Images(?):\t"+numImages;
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
			int pos = 0;

			data[pos++] = (byte)imageType;
			data[pos++] = (byte)(imageType>>8);
			data[pos++] = (byte)(imageType>>16);
			data[pos++] = (byte)(imageType>>24);
			data[pos++] = (byte)numImages;
			data[pos++] = (byte)(numImages>>8);
			data[pos++] = (byte)(numImages>>16);
			data[pos++] = (byte)(numImages>>24);
			return data;
		}


	}
}
