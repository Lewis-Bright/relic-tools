// This file is a part of the Relic Tools and is copyright 2006-2018 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU GPL license, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.
using System;
using System.IO;
using System.Collections;
using IBBoard.Graphics;
using IBBoard.Relic.RelicTools.Exceptions;

namespace IBBoard.Relic.RelicTools
{
	public enum PTLD_Layers {Primary = 0, Secondary = 1, Trim = 2, Weapon = 3, Eyes = 4, Dirt = 5}

	/// <summary>
	/// Summary description for ChunkyDataPTLD
	/// </summary>
	public class ChunkyDataPTLD : ChunkyDataLayer
	{
		private PTLD_Layers layerType;
		byte[] image;

		public ChunkyDataPTLD(int version_in, string name_in, byte[] innerData_in):base("PTLD", version_in, name_in)
		{
			layerType = (PTLD_Layers)(int)innerData_in[0];
			int layerSize = (innerData_in[4])+(innerData_in[5]<<8)+(innerData_in[6]<<16)+(innerData_in[7]<<24);
			image = new byte[layerSize];

			for (int i = 0; i<layerSize; i++)
			{
				image[i] = innerData_in[i+8];
			}
		}

		public override void Save(DirectoryInfo dir, string fileBaseName)
		{
			FileStream str = new FileStream(dir.FullName.TrimEnd(Path.DirectorySeparatorChar)+Path.DirectorySeparatorChar+fileBaseName+"_"+layerType.ToString()+".tga", FileMode.Create);
			BinaryWriter bw = new BinaryWriter(str);
			bw.Write(TGA_Greyscale_Header_a);			
			bw.Write((ushort)Info.Width);
			bw.Write((ushort)Info.Height);
			bw.Write(TGA_Greyscale_Header_b);
			bw.Write(image);
			bw.Flush();
			bw.Close();
		}

		public PTLD_Layers Layer
		{
			get{ return layerType; }
		}

		public int LayerSize
		{
			get{ return image.Length; }
		}

		public byte[] GetColourBytes()
		{
			return image;
		}

		public static ChunkyDataPTLD CreateFromTGA(PTLD_Layers layer_in, int version, string name, byte[] tgaData)
		{
			//check image type code
			if (tgaData[2]!=0x03)
			{
				bool worked = false;

				if (tgaData[1] == 0x01 && tgaData[2] == 0x01)
				{
					tgaData = ImageConverter.ColourMapToGreyscale(tgaData);
					worked = true;
				}


				if (!worked)
				{
					ImageConverter.MapEncStruct format = ImageConverter.getMapEncStruct(tgaData[2]);
					throw new InvalidFileException("_"+layer_in.ToString()+".tga must be an unencoded unmapped monochrome Targa image.\r\n"+
						"  Image type: "+format.formatType+"\r\n"+
						"  Image encoded: "+((format.isEncoded)?"yes":"no")+"\r\n"+
						"  Colour mapped: "+((format.isMapped)?"yes":"no"));
				}
			}

			//check colour depth
			if (tgaData[16]!=0x08)
			{
				throw new InvalidFileException("_"+layer_in.ToString()+".tga must be an 8-bit Targa image. Image was "+tgaData[16]+"-bit.");
			}

			int width = tgaData[12]+(tgaData[13]<<8);
			int height = tgaData[14]+(tgaData[15]<<8);
			byte[] data = new byte[width*height+8]; //only take the correct number of bytes so that we don't include comments
			
			int layerDataLength = data.Length-8; 
			int layer = (int)layer_in;
			data[0] = (byte)(layer);
			data[1] = (byte)(layer >> 8);
			data[2] = (byte)(layer >> 16);
			data[3] = (byte)(layer >> 24);
			data[4] = (byte)layerDataLength;
			data[5] = (byte)(layerDataLength>>8);
			data[6] = (byte)(layerDataLength>>16);
			data[7] = (byte)(layerDataLength>>24);
			
			int offset = 10+tgaData[0];//should be 18, but we're starting at i=8 so remove 8 here as well
			bool nonBlack = false; //boolean to check whether they actually included any data in the layer

			for (int i = 8; i<data.Length; i++)
			{
				data[i] = tgaData[i+offset];//take in to account any possible image ID

				if (data[i]>5)
				{
					nonBlack = true;
				}
			}

			//make sure we always return a dirt layer, even if they made it all black (all teamcolourable)
			if (nonBlack || layer_in == PTLD_Layers.Dirt)
			{
				return new ChunkyDataPTLD(version, name, data);
			}
			else
			{
				return null;
			}
		}

		public override bool Savable
		{
			get
			{
				return true;
			}
		}

		public override string GetDisplayDetails()
		{
			return base.GetBaseDisplayDetails()+Environment.NewLine+
				"------------"+Environment.NewLine+
				"Layer:\t\t"+this.Layer+Environment.NewLine+
				"Layer size:\t"+this.LayerSize+Environment.NewLine+
				"Image data:\t"+Environment.NewLine+
				ByteArrayToString(image, 8);
		}
		
		public override int DataLength
		{
			get
			{
				return image.Length+8;
			}
		}

		public override byte[] GetDataBytes()
		{
			byte[] data = new byte[DataLength];
			BitConverter.GetBytes((int)layerType).CopyTo(data, 0);
			BitConverter.GetBytes(image.Length).CopyTo(data, 4);
			image.CopyTo(data, 8);
			return data;
		}
	}
}
