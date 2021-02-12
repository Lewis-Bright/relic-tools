// This file is a part of the Relic Tools and is copyright 2006-2018 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU GPL license, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.
using System;
using System.IO;
using IBBoard.Graphics;
using IBBoard.Relic.RelicTools.Exceptions;

namespace IBBoard.Relic.RelicTools
{
	/// <summary>
	/// Summary description for ChunkyDataPTBN.
	/// </summary>
	public class ChunkyDataPTBN : ChunkyDataLayer
	{
		private static readonly byte[] PTBN_TGA_Header = new byte[]{0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x02, 0x08, 0x08};

		const float size_x = 64;
		const float size_y = 96;
		float x;
		float y;

		public ChunkyDataPTBN(int version_in, string name_in, byte[] innerData):base("PTBN", version_in, name_in)
		{
			x = BitConverter.ToSingle(innerData,0);
			y = BitConverter.ToSingle(innerData,4);
		}

		public float Pos_x
		{
			get{ return x; }
		}

		public float Pos_y
		{
			get{ return y; }
		}

		public float Height
		{
			get{ return size_y; }
		}

		public float Width
		{
			get{ return size_x; }
		}

		/*public override bool Save(string path)
		{
			string name = this.ParentFile.Name.Substring(0, this.ParentFile.Name.LastIndexOf('.'));
			return Save(path, name);
		}

		public bool Save(string path, string name){
			this.Save(new DirectoryInfo(path), name);
			return true;
		}*/

		public override void Save(DirectoryInfo dir, string fileBaseName)
		{
			int width = Info.Width;
			int height = Info.Height;

			float right = x+size_x;
			float top = y+size_y;

			if (((int)right)>width || ((int)top)>height)
			{
				throw new InvalidChunkException("Banner layer contains an invalid banner position");
			}

			//create the TGA
			int fileLength = width*height;
			byte [] file = new byte[fileLength];

			int pos = 0;

			for (int i = 0; i<height; i++)
			{
				for (int j = 0; j<width; j++)
				{
					if (x<=j && j<right && y<=i && i<top)
					{
						file[pos] = byte.MaxValue;
					}
					else
					{
						file[pos] = byte.MinValue;
					}

					pos++;
				}
			}

			//save the TGA
			FileStream str = new FileStream(dir.FullName.TrimEnd(Path.DirectorySeparatorChar)+Path.DirectorySeparatorChar+fileBaseName+"_Banner.tga", FileMode.Create);
			BinaryWriter bw = new BinaryWriter(str);
			bw.Write(TGA_Greyscale_Header_a);				
			bw.Write((ushort)width);
			bw.Write((ushort)height);
			bw.Write(TGA_Greyscale_Header_b);
			bw.Write(file);
			bw.Flush();
			bw.Close();
		}

		public static ChunkyDataPTBN CreateFromTGA(int version, string name, byte[] tgaData)
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
					throw new InvalidFileException("_banner.tga must be an unencoded unmapped monochrome Targa image.\r\n"+
						"  Image type: "+format.formatType+"\r\n"+
						"  Image encoded: "+((format.isEncoded)?"yes":"no")+"\r\n"+
						"  Colour mapped: "+((format.isMapped)?"yes":"no"));
				}
			}

			//check colour depth
			if (tgaData[16]!=0x08)
			{
				throw new InvalidFileException("_banner.tga must be an 8-bit Targa image. Image was "+tgaData[16]+"-bit.");
			}

			float pos_x = float.MinValue;
			float pos_y = float.MinValue;
			int width = (int)tgaData[12]+(((int)tgaData[13])<<8);
			int height = (int)tgaData[14]+(((int)tgaData[15])<<8);
			int currPos = 0;
			int size_x_int = (int)size_x;

			for (currPos = 18; currPos<tgaData.Length; currPos+=size_x_int)
			{
				//try to find out badge;
				if (tgaData[currPos]==byte.MaxValue)
				{
					pos_y = (float)((currPos-18)/width);
					pos_x = (float)(currPos-18-(pos_y*height));
					break;
				}
			}

			if (pos_y!=float.MinValue)
			{
				int maxPossibleEnd = currPos+width;
				int extra = 0;

				while (currPos<maxPossibleEnd && tgaData[currPos]==byte.MaxValue)
				{
					currPos++;
					extra++;
				}

				//check where the left hand side should start
				if (pos_x>0 && (tgaData[currPos-size_x_int]!=byte.MaxValue || tgaData[currPos-size_x_int-1]!=byte.MinValue))
				{
					throw new InvalidFileException("Banner is positioned beyond the edge of the texture");
				}
				else
				{
					pos_x = pos_x+extra-size_x;
				}

				//and check for the top - not all of the block, just assume a left and bottom side are OK
				int aboveTop = currPos-size_x_int+(width*(int)size_y);
				for (int i = currPos-size_x_int; i<=aboveTop && i<tgaData.Length; i+= width)
				{
					if (tgaData[i]!=byte.MaxValue && i!=aboveTop)
					{
						throw new InvalidFileException("Banner area is too small. Banner must be 64px by 96px");
					}
				}

				//if we got here, it's all OK

				byte[] data = new byte[16];
			
				BitConverter.GetBytes(pos_x).CopyTo(data, 0);
				BitConverter.GetBytes(pos_y).CopyTo(data, 4);
				BitConverter.GetBytes((float)size_x).CopyTo(data, 8);
				BitConverter.GetBytes((float)size_y).CopyTo(data, 12);
			
				return new ChunkyDataPTBN(version, name, data);
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
			BitConverter.GetBytes(x).CopyTo(data, 0);
			BitConverter.GetBytes(y).CopyTo(data, 4);
			BitConverter.GetBytes(size_x).CopyTo(data, 8);
			BitConverter.GetBytes(size_y).CopyTo(data, 12);
			return data;
		}

		public override string GetDisplayDetails()
		{
			return base.GetBaseDisplayDetails()+Environment.NewLine+
				"------------"+Environment.NewLine+
				"Pos X:\t\t"+x+Environment.NewLine+
				"Pos Y:\t\t"+y+Environment.NewLine+
				"Width:\t\t"+size_x+Environment.NewLine+
				"Height:\t\t"+size_y;
		}
	}
}