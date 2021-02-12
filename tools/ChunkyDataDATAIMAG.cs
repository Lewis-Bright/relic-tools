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
	public class ChunkyDataDATAIMAG : ChunkyDataDATA
	{
		public ChunkyDataDATAIMAG(int version_in, string name_in, byte[] innerData_in):base(version_in, name_in, innerData_in)
		{
		}

		private void SaveTGA(DirectoryInfo dir, string fileBaseName)
		{
			FileStream str = new FileStream(dir.FullName.TrimEnd(Path.DirectorySeparatorChar)+Path.DirectorySeparatorChar+fileBaseName+".tga", FileMode.Create);
			BinaryWriter bw = new BinaryWriter(str);
			bw.Write(TGA_Colour_Header_a);
			bw.Write((ushort)Attributes.Width);
			bw.Write((ushort)Attributes.Height);
			bw.Write(TGA_Colour_Header_b);
			bw.Write(base.GetDataBytes());
			bw.Flush();
			bw.Close();
		}

		public override bool Savable
		{
			get
			{
				return true;
			}
		}

		public override void Save(DirectoryInfo dir, string fileBaseName)
		{
			if (Attributes.ImageType == 0)
			{
				this.SaveTGA(dir, fileBaseName);
			}
			else if (Attributes.ImageType == 8 || Attributes.ImageType == 10 || Attributes.ImageType == 11)
			{
				this.SaveDDS(dir, fileBaseName);
			}
			else
			{
				throw new InvalidOperationException("Unexpected image type for DATA chunk: "+Attributes.ImageType);
			}
		}

		public static ChunkyDataDATA CreateFromTGA(int version, string name, byte[] tgaData)
		{
			//check image type code
			if (tgaData[2]!=0x02)
			{
				throw new InvalidFileException("Base layer must be a valid 32-bit Targa image");
			}

			//check colour depth
			if (tgaData[16]!=0x20)
			{
				throw new InvalidFileException("Base layer must be a valid 32-bit Targa image (pixel depth reads as "+tgaData[16].ToString()+"-bit)");
			}

			int width = tgaData[12]+(tgaData[13]<<8);
			int height = tgaData[14]+(tgaData[15]<<8);
			byte[] data = new byte[width*height*4]; //only take the correct number of bytes so that we don't include comments
			int offset = 18+tgaData[0];
						
			for (int i = 0; i<data.Length; i++)
			{
				data[i] = tgaData[i+offset];
			}

			return new ChunkyDataDATAIMAG(version, name, data);
		}

		public static ChunkyDataDATA CreateFromDDS(int version, string name, byte[] ddsData)
		{
			//check image type code
			if (ddsData[0]!=0x44 || ddsData[1]!=0x44 ||ddsData[2]!=0x53 || ddsData[3]!=0x20)
			{
				throw new InvalidFileException("Source image must be a valid DDS image");
			}

			//check colour depth
			if (ddsData[84] != 0x44 || ddsData[85] != 0x58 || ddsData[86] !=0x54 || (ddsData[87] != 0x31 && ddsData[87] != 0x33 && ddsData[87] != 0x35))
			{
				throw new InvalidFileException("Source image must be a valid DXT1 , DXT3 or DXT5 DDS image");
			}



			byte[] data = new byte[ddsData.Length-128];
						
			int max = data.Length;
			for (int i = 0; i<max; i++)
			{
				data[i] = ddsData[i+128];
			}

			return new ChunkyDataDATAIMAG(version, name, data);
		}

		private void SaveDDS(DirectoryInfo dir, string fileBaseName)//, int type)
		{
			if (Attributes == null)
			{
				throw new InvalidOperationException("DATA chunk must have a related ATTR chunk before it can be saved");
			}

			int mipmaps = (int)Math.Log(Attributes.Width, 2)+1;
			int type = Attributes.ImageType;

			FileStream str = new FileStream(dir.FullName.TrimEnd(Path.DirectorySeparatorChar)+Path.DirectorySeparatorChar+fileBaseName+".dds", FileMode.Create);
			BinaryWriter bw = new BinaryWriter(str);
			bw.Write(DTX_Header_a);
			bw.Write(Attributes.Height);
			bw.Write(Attributes.Width);

			if (type == 0x8)
			{
				bw.Write(DataLength);
			}
			else
			{
				bw.Write(Attributes.Width*Attributes.Height);
			}

			bw.Write(new byte[]{0x00, 0x00, 0x00, 0x00});//volume depth - always 0
			bw.Write(mipmaps);
			
			if (type == 0x8)
			{
				bw.Write(DXT1_Header_b);
			}
			else if (type == 0xA)
			{
				bw.Write(DXT3_Header_b);
			}
			else
			{
				bw.Write(DXT5_Header_b);
			}

			bw.Write(base.GetDataBytes());
			bw.Flush();
			bw.Close();
		}

		/*
		public void SaveDDS(DirectoryInfo dir, string fileBaseName, int width, int height, byte id)
		{
			FileStream str = new FileStream(dir.FullName.TrimEnd(Path.DirectorySeparatorChar)+Path.DirectorySeparatorChar+fileBaseName+".dds", FileMode.Create);
			BinaryWriter bw = new BinaryWriter(str);
			bw.Write(DTX1_Header_a);
			bw.Write(width);
			bw.Write(height);
			bw.Write(DataLength);
			bw.Write(new byte[]{0x00, 0x00, 0x00, 0x00});//volume depth - always 0
			bw.Write((int)Math.Log(width, 2)+1);//calculate the number of MIP Maps
			bw.Write(DXT1_Header_b);
			bw.Write(innerData);
			bw.Flush();
			bw.Close();
		}*/
	}
}
