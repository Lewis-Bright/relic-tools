// This file (ImageConverter.cs) is a part of the IBBoard.Graphics library and is copyright 2009 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU LGPL license, either version 3 of the License or (at your option) any later version. Please see COPYING.LGPL for more information and the full license.

using System;
using System.Drawing;
using System.IO;
using System.Collections;
using IBBoard.IO;

namespace IBBoard.Graphics
{
	public class ImageConverter
	{
		public struct MapEncStruct
		{
			public bool isMapped, isEncoded;
			public string formatType;

			public MapEncStruct(string type, bool mapped, bool encoded)
			{
				formatType = type;
				isMapped = mapped;
				isEncoded = encoded;
			}
		}

		private static ArrayList mapEncStructs;

		static ImageConverter()
		{
			mapEncStructs = new ArrayList(12);
			mapEncStructs.Insert(0, new MapEncStruct("No Image", false, false));
			mapEncStructs.Insert(1, new MapEncStruct("Colourmapped", true, false));
			mapEncStructs.Insert(2, new MapEncStruct("Truecolour", false, false));
			mapEncStructs.Insert(3, new MapEncStruct("Monochrome", false, false));
			mapEncStructs.Insert(4, null);
			mapEncStructs.Insert(5, null);
			mapEncStructs.Insert(6, null);
			mapEncStructs.Insert(7, null);
			mapEncStructs.Insert(8, null);
			mapEncStructs.Insert(9, new MapEncStruct("Colourmapped", true, true));
			mapEncStructs.Insert(10, new MapEncStruct("Truecolour", false, true));
			mapEncStructs.Insert(11, new MapEncStruct("Monochrome", false, true));
		}

		public static MapEncStruct getMapEncStruct(byte id)
		{
			return (MapEncStruct)mapEncStructs[id];
		}
		
		public static readonly byte[] TGA_Colour_Header = new byte[]{0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x20, 0x08};
		public static readonly byte[] TGA_Greyscale_Header = new byte[]{0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x08, 0x08};

		public static uint rgb565_to_rgba(ushort color) 
		{
			uint red, green, blue;
			red = ((uint)(color & 0xf800))>>11;
			red = ((red * 255) / 31)<<16;
			green = ((uint)(color & 0x07e0))>>5;
			green = ((green * 255) / 63)<<8;
			blue = ((uint)(color & 0x001f) * 255) / 31;

			return red | green | blue | (uint)0xff000000;
		}


		public static void TGAtoDDS(IntPtr handle, byte[] tgaFile, string filename)
		{
			FlipTGAbytes(ref tgaFile);

			//DirectXWrapper.ImageConverter.TGAtoDDS(handle, tgaFile, filename);
		}

		public static Bitmap TGAtoBMP(string tgaPath, Color colour)
		{
			FileStream fs = null;
			try
			{
				fs = new FileStream(tgaPath, FileMode.Open);
				byte[] tgaFile = new byte[fs.Length];
				fs.Read(tgaFile, 0, (int)fs.Length);

				if (tgaFile[2]!=0x02)
				{
					throw new InvalidOperationException("Image must be a 32-bit TGA");
				}

				bool hasAlpha = true;

				if (tgaFile[16]!=0x20)
				{
					if (tgaFile[16]==0x18)
					{
						hasAlpha = false;
					}
					else
					{
						throw new InvalidOperationException("Image must be a 32-bit TGA");
					}
				}

				FlipTGAbytes(ref tgaFile);
				byte[] data;
				int last = 0;
				
				if (hasAlpha)
				{
					data = new byte[(((tgaFile.Length-18-tgaFile[0])/4)*3)+54];
					last = tgaFile.Length-4;
				}
				else
				{
					data = new byte[tgaFile.Length-18-tgaFile[0]+54];
					last = tgaFile.Length;
				}

				byte[] bmpHeader = new byte[]{0x42, 0x4D, (byte)data.Length, (byte)(data.Length>>8), (byte)(data.Length>>16), (byte)(data.Length>>24), 0x00, 0x00, 0x00, 0x00, 0x36, 0x00, 0x00, 0x00, 0x28, 0x00, 0x00, 0x00, tgaFile[12], tgaFile[13], 0x00, 0x00, tgaFile[14], tgaFile[15], 0x00, 0x00, 0x01, 0x00, 0x18, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x12, 0x0B, 0x00, 0x00, 0x12, 0x0B, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};
				bmpHeader.CopyTo(data, 0);

				int pos = 54;
				double alpha;

				if (hasAlpha)
				{
					for (int i = 18+tgaFile[0]; i<last; i=i+4)
					{
						alpha = tgaFile[i+3]/255.0;
						data[pos] = (byte)((tgaFile[i] * alpha) + (colour.B * (1-alpha)));
						pos++;
						data[pos] = (byte)((tgaFile[i+1] * alpha) + (colour.G * (1-alpha)));
						pos++;
						data[pos] = (byte)((tgaFile[i+2] * alpha) + (colour.R * (1-alpha)));
						pos++;
					}
				}
				else
				{
					for (int i = 18+tgaFile[0]; i<last; i++)
					{
						data[pos++] = tgaFile[i];
					}
				}

				MemoryStream stream = new MemoryStream(data);

				return new Bitmap(stream);
			}
			finally
			{
				if (fs!=null)
				{
					fs.Close();
				}
			}
		}

		public static void FlipTGAbytes(ref byte[] file)
		{
			int idLength, width, height, byteWidth, halfHeight, topIndex, bottomIndex;
			byte temp;

			idLength = file[0];
			width = file[12]+(file[13]<<8);
			height = file[14]+(file[15]<<8);
			
			byteWidth = width*(file[16]/8);
			halfHeight = height/2;

			for (int row = height-1; row>=halfHeight; row--)
			{
				for (int col = 0; col<byteWidth; col++)
				{
					topIndex = (height - row -1)*byteWidth + col + 18 + idLength;
					bottomIndex = row*byteWidth + col + 18 + idLength;
					temp = file[topIndex];
					file[topIndex] = file[bottomIndex];
					file[bottomIndex] = temp;
				}
			}
		}

		public static byte[] ColourMapToGreyscale(byte[] file)
		{
			int idLength, width, height, cmapLength, cmapStart, dataLength, offset, pos;
			byte colourBytes, cmapDepth;
			byte[] colourMap;
			byte[] data;

			idLength = file[0];
			width = file[12]+(file[13]<<8);
			height = file[14]+(file[15]<<8);

			cmapStart = file[3]+(file[4]<<8);
			cmapLength = file[5]+(file[6]<<8);
			cmapDepth = file[7];
			colourBytes = (byte)(cmapDepth/8);

			colourMap = new byte[cmapLength];

			dataLength = height*width;

			data = new byte[18+dataLength];

			offset = idLength+cmapLength*colourBytes;

			TGA_Greyscale_Header.CopyTo(data, 0);

			data[12] = file[12];
			data[13] = file[13];
			data[14] = file[14];
			data[15] = file[15];

			pos = idLength+18;

			for (int i = 0; i<cmapLength; i++)
			{
				for (int j = 1; j<colourBytes; j++)
				{
					if (file[pos]!=file[pos+j])
					{
						throw new InvalidOperationException("Conversion to greyscale failed - image must use a greyscale colour map.");
					}
				}

				colourMap[i] = file[pos];
				pos+=colourBytes;
			}
			
			for (int i = 18; i<dataLength; i++)
			{
				data[i] = colourMap[file[i+offset]];
			}

			return data;
		}

		public static byte[] TGAto32bitTGA(byte[] file)
		{
			if (file[2]!=2)
			{
				throw new UnsupportedFileTypeException("non-true colour, unencoded, unmapped TGA", "TGAto32BitTGA");
			}

			if (file[16]==32)
			{
				return file;
			}
			else if (file[16]==24)
			{
				int imagesize = (file[12]+(file[13]<<8))*(file[14]+(file[15]<<8));
				int imagebytes = imagesize*3;
				byte[] converted = new byte[imagesize*4+18];

				//leave index 0 as 0 so it has 0 length ID
				for (int i = 1; i<18; i++)
				{
					converted[i] = file[i];
				}

				converted[16] = 32;

				int idLength = file[0];
				int lastIndex = imagebytes+18;

				for (int i = 18, pos = 18; i<lastIndex; i+=3, pos+=4)
				{
					converted[pos] = file[i+idLength];
					converted[pos+1] = file[i+1+idLength];
					converted[pos+2] = file[i+2+idLength];
					converted[pos+3] = byte.MaxValue;
				}

				return converted;
			}
			else
			{
				throw new UnsupportedFileTypeException("true colour, encoded or mapped TGA", "TGAto32BitTGA");
			}
		}
	}
}
