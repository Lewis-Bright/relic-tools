// This file (Converter.cs) is a part of the IBBoard.Graphics.OpenIL library and is copyright 2009 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU LGPL license, either version 3 of the License or (at your option) any later version. Please see COPYING.LGPL for more information and the full license.

using System;
using System.Drawing;

namespace IBBoard.Graphics.OpenILPort
{
    /// <summary>
    /// This class converts a 32-bit BRGA file (TGA) to a DXT1/3/5 DDS file.
    /// Conversion is currently incomplete, MipMapping isn't using the correct
    ///		filters and other file formats are not supported.
    /// This port was originally made as a partial port to allow IBBoard's .Net
    ///		apps for the Dawn of War computer game textures to convert TGAs to DDS
    ///     files without using an external app, and without using Managed DirectX.
    ///     It has since been replaced by a wrapper for Squish and is only used for scaling.
    /// </summary>
	public class Converter
	{
		public enum DXTType { None, DXT1, DXT3, DXT5 }

		public enum Filter {Triangle}

		private const byte BLACK_PIXEL = 0;
		private const byte WHITE_PIXEL = 255;

		public static readonly byte[] DXT1_Header = new byte[]{0x44, 0x44, 0x53, 0x20,//"DDS "
																  0x7C, 0x00, 0x00, 0x00,//size - fixed to 124
																  //0x07, 0x10, 0x02, 0x00};//valid field flags - DDSD_CAPS, DDSD_PIXELFORMAT, DDSD_WIDTH, DDSD_HEIGHT
																  0x07, 0x10, 0x0A, 0x00,//Use Photoshop's output as a guide rather than the OpenIL line above
																  0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,//leave space for width and height
																  0x00, 0x00, 0x00, 0x00,//and data length
																  0x00, 0x00, 0x00, 0x00,//zero the volume depths
																  0x00, 0x00, 0x00, 0x00,//and leave space for the mip maps
																  0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
																  0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
																  0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
																  0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,//11 DWords of unspecified 'reserved'
																  0x20, 0x00, 0x00, 0x00, //size - apparently fixed to 32
																  0x04, 0x00, 0x00, 0x00,//DDPF_FOURCC
																  0x44, 0x58, 0x54, 0x31,//DTX1
																  0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, //RGB and Alpha bit masks - not used in DoW DDS files
																  0x08, 0x10, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, //DDS Caps - always the same in DoW
																  0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,//two more reserved DWords
																  0x00, 0x00, 0x00, 0x00};//and a final reserved DWord for good measure!

		public static readonly byte[] DXT5_Header = new byte[]{0x44, 0x44, 0x53, 0x20,//"DDS "
																  0x7C, 0x00, 0x00, 0x00,//size - fixed to 124
																  //0x07, 0x10, 0x02, 0x00};//valid field flags - DDSD_CAPS, DDSD_PIXELFORMAT, DDSD_WIDTH, DDSD_HEIGHT
																  0x07, 0x10, 0x0A, 0x00,//Use Photoshop's output as a guide rather than the OpenIL line above
																  0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,//leave space for width and height
																  0x00, 0x00, 0x00, 0x00,//and data length
																  0x00, 0x00, 0x00, 0x00,//zero the volume depths
																  0x00, 0x00, 0x00, 0x00,//and leave space for the mip maps
																  0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
																  0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
																  0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
																  0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,//11 DWords of unspecified 'reserved'
																  0x20, 0x00, 0x00, 0x00, //size - apparently fixed to 32
																  0x04, 0x00, 0x00, 0x00,//DDPF_FOURCC
																  0x44, 0x58, 0x54, 0x35,//DTX5
																  0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, //RGB and Alpha bit masks - not used in DoW DDS files
																  0x08, 0x10, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, //DDS Caps - always the same in DoW
																  0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,//two more reserved DWords
																  0x00, 0x00, 0x00, 0x00};//and a final reserved DWord for good measure!

		public static readonly byte[] DXT3_Header = new byte[]{0x44, 0x44, 0x53, 0x20,//"DDS "
																  0x7C, 0x00, 0x00, 0x00,//size - fixed to 124
																  //0x07, 0x10, 0x02, 0x00};//valid field flags - DDSD_CAPS, DDSD_PIXELFORMAT, DDSD_WIDTH, DDSD_HEIGHT
																  0x07, 0x10, 0x0A, 0x00,//Use Photoshop's output as a guide rather than the OpenIL line above
																  0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,//leave space for width and height
																  0x00, 0x00, 0x00, 0x00,//and data length
																  0x00, 0x00, 0x00, 0x00,//zero the volume depths
																  0x00, 0x00, 0x00, 0x00,//and leave space for the mip maps
																  0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
																  0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
																  0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
																  0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,//11 DWords of unspecified 'reserved'
																  0x20, 0x00, 0x00, 0x00, //size - apparently fixed to 32
																  0x04, 0x00, 0x00, 0x00,//DDPF_FOURCC
																  0x44, 0x58, 0x54, 0x33,//DTX5
																  0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, //RGB and Alpha bit masks - not used in DoW DDS files
																  0x08, 0x10, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, //DDS Caps - always the same in DoW
																  0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,//two more reserved DWords
																  0x00, 0x00, 0x00, 0x00};//and a final reserved DWord for good measure!

		private static int NextPower2(int val)
		{
			return (int)Math.Pow(2, Math.Ceiling(Math.Log(val)/Math.Log(2.0)));
		}

		private static int CalcMips(int width, int height)
		{
			return (int)Math.Ceiling(Math.Log(Math.Max(width, height))/Math.Log(2))+1;
		}

		private static int CalcFileSize(DXTType type, int width, int height, int mipmaps)
		{
			int val = 0;
			byte bytes = (byte)BlockSize(type);

			width = width/4;
			height = height/4;

			for (int i = 0; i<mipmaps; i++)
			{
				val+= width*height*bytes;
				width = (width>1)?width/2:1;
				height = (height>1)?height/2:1;
			}

			return val;
		}

		private static int BlockSize(DXTType type)
		{
			if (type==DXTType.DXT1)
			{
				return 8;
			}
			else if (type == DXTType.DXT5 || type == DXTType.DXT3)
			{
				return 16;
			}
			else
			{
				return 32;
			}
		}

		public static byte[] BGRAtoDDS(byte[] rawData, DXTType DXTCFormat, int width, int height)
		{
			ushort ex0 = 0, ex1 = 0;
			ushort[] Data;
			ushort[] Block = new ushort[16]; 
			byte[] file = null;
			int x, y, i, pos;
			uint BitMask;
			byte a0 = 0, a1 = 0;
			byte[] Alpha;
			byte[] AlphaBlock = new byte[16];
			byte[] AlphaBitMask = new byte[6];
			byte[] AlphaOut = new byte[16];
			bool HasAlpha = false;
			int blocksize = BlockSize(DXTCFormat);

			if (NextPower2(width) != width ||
				NextPower2(height) != height) 
			{
				throw new InvalidOperationException("Dimensions of the image must be a power of two");
			}

			int mips = CalcMips(width, height);
			int size = CalcFileSize(DXTCFormat, width, height, mips);
			int mainSize = width*height/(16/blocksize);

			rawData = CreateMipMaps(rawData, width, height, mips);

			Data = CompressTo565(rawData, width, height);
			if (Data == null)
				return null;

			Alpha = GetAlpha(rawData);
			if (Alpha == null) 
			{
				return null;
			}

			file = new byte[size+128];

			switch(DXTCFormat)
			{
				case DXTType.DXT1: DXT1_Header.CopyTo(file, 0);
					break;
				case DXTType.DXT5: DXT5_Header.CopyTo(file, 0);
					break;
				case DXTType.DXT3: DXT3_Header.CopyTo(file, 0);
					break;
				default:
					throw new InvalidOperationException("Unsupported DXT format");
			}

			file[12] = (byte)height;
			file[13] = (byte)(height>>8);
			file[14] = (byte)(height>>16);
			file[15] = (byte)(height>>24);
			file[16] = (byte)width;
			file[17] = (byte)(width>>8);
			file[18] = (byte)(width>>16);
			file[19] = (byte)(width>>24);
			file[20] = (byte)mainSize;
			file[21] = (byte)(mainSize>>8);
			file[22] = (byte)(mainSize>>16);
			file[23] = (byte)(mainSize>>24);
			file[28] = (byte)mips;
			file[29] = (byte)(mips>>8);
			file[30] = (byte)(mips>>16);
			file[31] = (byte)(mips>>24);
			pos = 128;

			int start = 0;
			int startAlpha = 0;

			switch (DXTCFormat)
			{
				case DXTType.DXT1:			
					for (int m = 0; m<mips; m++)
					{
						for (y = 0; y < height; y += 4) 
						{
							for (x = 0; x < width; x += 4) 
							{
								AlphaBlock = GetAlphaBlock(Alpha, width, x, y, startAlpha, ref HasAlpha);

								//Remove the old 'has alpha' check and replace is with a Ref'd bool above
								//HasAlpha = false;

								//for (i = 0 ; i < 16; i++) 
								//{
								//if (AlphaBlock[i] < 128) 
								//{
								//HasAlpha = true;
								//break;
								//}
								//}

								Block = GetBlock(Data, width, x, y, start);
								ChooseEndpoints(Block, ref ex0, ref ex1);
								CorrectEndDXT1(ref ex0, ref ex1, HasAlpha);
								file[pos] = (byte)ex0;
								file[pos+1] = (byte)(ex0>>8);
								pos+=2;
								file[pos] = (byte)ex1;
								file[pos+1] = (byte)(ex1>>8);
								pos+=2;

								if (HasAlpha)
									BitMask = GenBitMask(ex0, ex1, 3, Block, AlphaBlock);
								else
									BitMask = GenBitMask(ex0, ex1, 4, Block, null);

								file[pos] = (byte)BitMask;
								file[pos+1] = (byte)(BitMask>>8);
								file[pos+2] = (byte)(BitMask>>16);
								file[pos+3] = (byte)(BitMask>>24);
								pos+=4;
							}		
						}

						start = start+(width*height);
						startAlpha = startAlpha+(width*height/2);

						width /=2;
						if (width<1) width = 1;
						height /=2;
						if (height<1) height = 1;
					}
					break;
				case DXTType.DXT3:					
					for (int m = 0; m<mips; m++)
					{
						for (y = 0; y < height; y += 4) 
						{
							for (x = 0; x < width; x += 4) 
							{
								AlphaBlock = GetAlphaBlock(Alpha, width, x, y, startAlpha);
								for (i = 0; i < 16; i += 2) 
								{
									file[pos++] = (byte)(((AlphaBlock[i+1] >> 4) << 4) | (AlphaBlock[i] >> 4));
								}

								Block = GetBlock(Data, width, x, y, start);
								ChooseEndpoints(Block, ref ex0, ref ex1);
								CorrectEndDXT1(ref ex0, ref ex1, HasAlpha);//correct DXT3 colour ordering, because otherwise Photoshop treats it as DXT1 and makes index 3 (0x11) black
								file[pos] = (byte)ex0;
								file[pos+1] = (byte)(ex0>>8);
								pos+=2;
								file[pos] = (byte)ex1;
								file[pos+1] = (byte)(ex1>>8);
								pos+=2;
								BitMask = GenBitMask(ex0, ex1, 4, Block, null);
								file[pos] = (byte)BitMask;
								file[pos+1] = (byte)(BitMask>>8);
								file[pos+2] = (byte)(BitMask>>16);
								file[pos+3] = (byte)(BitMask>>24);
								pos+=4;
							}		
						}

						start = start+(width*height);
						startAlpha = startAlpha+(width*height/2);

						width /=2;
						if (width<1) width = 1;
						height /=2;
						if (height<1) height = 1;
					}
					break;

				case DXTType.DXT5:
					for (int m = 0; m<mips; m++)
					{
						for (y = 0; y < height; y += 4) 
						{
							for (x = 0; x < width; x += 4) 
							{
								AlphaBlock = GetAlphaBlock(Alpha, width, x, y, startAlpha);
								ChooseAlphaEndpoints(AlphaBlock, ref a0, ref a1);
								AlphaOut = GenAlphaBitMask(a0, a1, 8, AlphaBlock, AlphaBitMask);
								file[pos++] = a0;
								file[pos++] = a1;
								file[pos++] = (byte)AlphaBitMask[0];
								file[pos++] = (byte)AlphaBitMask[1];
								file[pos++] = (byte)AlphaBitMask[2];
								file[pos++] = (byte)AlphaBitMask[3];
								file[pos++] = (byte)AlphaBitMask[4];
								file[pos++] = (byte)AlphaBitMask[5];

								Block = GetBlock(Data, width, x, y, start);
								ChooseEndpoints(Block, ref ex0, ref ex1);
								CorrectEndDXT1(ref ex0, ref ex1, HasAlpha);//correct DXT5 colour ordering, because otherwise Photoshop treats it as DXT1 and makes index 3 (0x11) black
								file[pos] = (byte)ex0;
								file[pos+1] = (byte)(ex0>>8);
								pos+=2;
								file[pos] = (byte)ex1;
								file[pos+1] = (byte)(ex1>>8);
								pos+=2;
								BitMask = GenBitMask(ex0, ex1, 4, Block, null);
								file[pos] = (byte)BitMask;
								file[pos+1] = (byte)(BitMask>>8);
								file[pos+2] = (byte)(BitMask>>16);
								file[pos+3] = (byte)(BitMask>>24);
								pos+=4;
							}
						}

						start = start+(width*height);
						startAlpha = startAlpha+(width*height/2);

						width /=2;
						if (width<1) width = 1;
						height /=2;
						if (height<1) height = 1;
					}
					break;
			}

			return file;
		}

		/// <summary>
		/// Gets a 4x4 pixel block of RGB565 colours from the image
		/// </summary>
		/// <param name="Data">The image (and its MipMaps) as an array of RGB565 ushorts</param>
		/// <param name="width">Pixel width of the image</param>
		/// <param name="XPos">Horizontal position at which to start</param>
		/// <param name="YPos">Vertical position at which to start</param>
		/// <param name="start">Start of the MipMaps layer within the array</param>
		/// <returns>Array of RGB565 ushorts for each pixel in the block</returns>
		static ushort[] GetBlock(ushort[] Data, int width, int XPos, int YPos, int start)
		{
			int x, y, i = 0, Offset;
			ushort[] Block = new ushort[16];

			for (y = 0; y < 4; y++) 
			{
				Offset = (YPos + y) * width + XPos + start;

				for (x = 0; x < 4; x++) 
				{
					Block[i++] = Data[Offset+x];
				}
			}

			return Block;
		}

		/// <summary>
		/// Returns a block of Alpha Channel data, without information as to whether there is transparency or not
		/// </summary>
		/// <param name="Data">The 32-bit image (and its MipMaps) as an array of bytes</param>
		/// <param name="width">Pixel width of the image</param>
		/// <param name="XPos">Horizontal position at which to start</param>
		/// <param name="YPos">Vertical position at which to start</param>
		/// <param name="start">Start of the MipMaps layer within the array</param>
		/// <returns>Array of alpha bytes at the specified position in the specified MipMap</returns>
		static byte[] GetAlphaBlock(byte[] Data, int width, int XPos, int YPos, int start)
		{
			bool junk = false;
			return GetAlphaBlock(Data, width, XPos, YPos, start, ref junk);
		}

		/// <summary>
		/// Returns a block of Alpha Channel data, with information as to whether there is transparency or not within the block
		/// </summary>
		/// <param name="Data">The 32-bit image (and its MipMaps) as an array of bytes</param>
		/// <param name="width">Pixel width of the image</param>
		/// <param name="XPos">Horizontal position at which to start</param>
		/// <param name="YPos">Vertical position at which to start</param>
		/// <param name="start">Start of the MipMaps layer within the array</param>
		/// <param name="HasAlpha">Referenced boolean to indicate whether the block contains transparency</param>
		/// <returns>Array of alpha bytes at the specified position in the specified MipMap</returns>
		static byte[] GetAlphaBlock(byte[] Data, int width, int XPos, int YPos, int start, ref bool HasAlpha)
		{
			int x, y, i = 0, Offset;
			byte[] Block = new byte[16];
			HasAlpha = false;

			for (y = 0; y < 4; y++) 
			{
				Offset = (YPos + y) * width + XPos + start;

				for (x = 0; x < 4; x++) 
				{
					Block[i++] = Data[Offset+x];

					if (Data[Offset+x]<128)
						HasAlpha = true;
				}
			}

			return Block;
		}


		/// <summary>
		/// Converts a ushort RGB565 colour back to an RGB888 colour in the form of a Color object
		/// </summary>
		/// <param name="Pixel">The RGB565 ushort colour of a pixel</param>
		/// <returns>A Color object of the equivalent RGB888 colour</returns>
		static Color ShortToColor888(ushort Pixel)
		{
			Color col = Color.Black;

			//we could do it the fancy and balanced way, but that seems to give colour hue issues of greys turning green/purple!
			/*red = ((int)(Pixel & 0xf800))>>11;
				red = (red * 255) / 31;
				green = ((int)(Pixel & 0x07e0))>>5;
				green = (green * 255) / 63;
				blue = ((int)(Pixel & 0x001f) * 255) / 31;*/
			col = Color.FromArgb(((Pixel & 0xF800) >> 11) << 3,
				((Pixel & 0x07E0) >> 5)  << 2,
				((Pixel & 0x001F))       << 3);

			return col;
		}

		/// <summary>
		/// Converts an RGB888 Color object to a ushort RGB565 colour
		/// </summary>
		/// <param name="Colour">The Color object to convert</param>
		/// <returns>The equivalent RGB565 colour as a ushort</returns>
		static ushort Color888ToShort(Color Colour)
		{
			return (ushort)(((Colour.R >> 3) << 11) | ((Colour.G >> 2) << 5) | (Colour.B >> 3));
		}

		/// <summary>
		/// Generates the appropriate colour bitmask for the 4px x 4px DDS block
		/// </summary>
		/// <param name="ex0">Colour 0 as a ushort RGB565</param>
		/// <param name="ex1">Colour 1 as a ushort RGB565</param>
		/// <param name="NumCols">Number of colours to compress to. Value 3 for using 1-bit transparency in DXT1 images, otherwise 4</param>
		/// <param name="In">Array of ushort RGB565 values holding the colours of the pixels in the block</param>
		/// <param name="Alpha">Byte array of the alpha channel bytes for the block</param>
		/// <returns></returns>
		static uint GenBitMask(ushort ex0, ushort ex1, uint NumCols, ushort[] In, byte[] Alpha)
		{
			uint i, j, Closest, Dist, BitMask = 0;
			byte[] Mask = new byte[16];
			Color c;
			Color[] Colours = new Color[4];

			Colours[0] = ShortToColor888(ex0);
			Colours[1] = ShortToColor888(ex1);

			if (NumCols == 3) 
			{
				Colours[2] = Color.FromArgb((Colours[0].R + Colours[1].R) / 2, 
					(Colours[0].G + Colours[1].G) / 2,
					(Colours[0].B + Colours[1].B) / 2);
				Colours[3] = Color.Black;
			}
			else 
			{  // NumCols == 4
				Colours[2] = Color.FromArgb((2 * Colours[0].R + Colours[1].R + 1) / 3,
					(2 * Colours[0].G + Colours[1].G + 1) / 3,
					(2 * Colours[0].B + Colours[1].B + 1) / 3);
				Colours[3] = Color.FromArgb((Colours[0].R + 2 * Colours[1].R + 1) / 3,
					(Colours[0].G + 2 * Colours[1].G + 1) / 3,
					(Colours[0].B + 2 * Colours[1].B + 1) / 3);
			}

			for (i = 0; i < 16; i++) 
			{
				if (Alpha!=null) 
				{  // Test to see if we have 1-bit transparency
					if (Alpha[i] < 128) 
					{
						Mask[i] = 3;  // Transparent
						continue;
					}
				}

				// If no transparency, try to find which colour is the closest.
				Closest = UInt16.MaxValue;
				c = ShortToColor888(In[i]);
				for (j = 0; j < NumCols; j++) 
				{
					Dist = Distance(c, Colours[j]);
					//Dist = HSLDistance(c, Colours[j]);
					if (Dist < Closest) 
					{
						Closest = Dist;
						Mask[i] = (byte)j;
					}
				}
			}

			for (i = 0; i < 16; i++) 
			{
				BitMask |= (uint)((int)Mask[i] << (int)(i*2));
			}

			return BitMask;
		}

		/// <summary>
		/// Compresses an BGRA8888 image (32-bit TGA) to an RGB565 image with MipMaps
		/// </summary>
		/// <param name="rawData">Raw 32-bit BRGA8888 image data without headers as an array of bytes</param>
		/// <param name="width">Width of the raw image</param>
		/// <param name="height">Height of the raw image</param>
		/// <returns>RGB565 image with MipMaps as an array of ushorts</returns>
		static ushort[] CompressTo565(byte[] rawData, int width, int height)
		{
			int mips = CalcMips(width, height);
			ushort[] data = new ushort[CalcFileSize(DXTType.None, width, height, mips)/2];
			uint i, j = 0;

			for (i = 0; i < rawData.Length; i += 4, j++) 
			{
				data[j]  = (ushort)((rawData[i+2]>> 3) << 11);
				data[j] |= (ushort)((rawData[i+1] >> 2) << 5);
				data[j] |=  (ushort)(rawData[i] >> 3);
			}

			return data;
		}

		/// <summary>
		/// Adds the MipMaps to a BGRA8888 image
		/// </summary>
		/// <param name="data">Raw 32-bit BRGA8888 image data without headers as an array of bytes</param>
		/// <param name="width">Width of the raw image</param>
		/// <param name="height">Height of the raw image</param>
		/// <returns>Raw 32-bit BRGA8888 image data with MipMaps as an array of bytes</returns>
		public static byte[] CreateMipMaps(byte[] data, int width, int height)
		{
			return CreateMipMaps(data, width, height, CalcMips(width, height));
		}

		/// <summary>
		/// Adds the MipMaps to a BGRA8888 image
		/// </summary>
		/// <param name="data">Raw 32-bit BRGA8888 image data without headers as an array of bytes</param>
		/// <param name="width">Width of the raw image</param>
		/// <param name="height">Height of the raw image</param>
		/// <param name="mips">Number of MipMaps to create</param>
		/// <returns>Raw 32-bit BRGA8888 image data with MipMaps as an array of bytes</returns>
		public static byte[] CreateMipMaps(byte[] data, int width, int height, int mips)
		{
			byte[] image = new byte[CalcFileSize(DXTType.None, width, height, mips)*2];
			byte[] temp;
			byte[] prev = data;
			int start = width*height*4;
			int reduction;
			int redWidth = width;
			int redHeight = height;

			data.CopyTo(image, 0);
			
			for (int m = 1; m<mips; m++)
			{
				reduction = (int)Math.Pow(2, m);
				reduction*=reduction;
				temp = HalveSize(prev, redWidth, redHeight);
				redWidth = redWidth/2;
				redHeight = redHeight/2;
				prev = temp;
				temp.CopyTo(image, start); 
				start = start+width*height*4/reduction;
			}

			return image;
		}

		/// <summary>
		/// Reduces the size of a given image by half to create the MipMap
		/// </summary>
		/// <param name="data">32-bit image to reduce in size</param>
		/// <param name="width">Width of original image</param>
		/// <param name="height">Height of original image</param>
		/// <returns>Half-sized 32-bit image</returns>
		public static byte[] HalveSize(byte[] data, int width, int height)
		{
			byte[] map = new byte[data.Length/4];
			Zoom(map, data, width, height, width/2, height/2, Filter.Triangle, 1.0);
			return map;

			
			/*
			int reduction = 2;
			int step = reduction*4;
			int byteWidth = 4*width;
			int pos = 0;
			int oldPos = 0;
			double divisor = reduction*reduction;
			byte[] map = new byte[data.Length/(reduction*reduction)];
			double[] tempMap = new double[map.Length];
			int reducedByteWidth = (int)Math.Round((double)byteWidth/reduction);
			int penultByteWidth = byteWidth-4;
			int penultRow = height-1;
			double fraction = 1.0/reduction;
			double proportionBL = (divisor-1)/(divisor*divisor);
			double proportionBR = 1.0/(divisor*divisor);
			double proportionTL = ((divisor-1)*(divisor-1))/(divisor*divisor);
			double proportionTR = (divisor-1)/(divisor*divisor);
			double start = fraction/2 - 0.5;

			for (int i = 0; i<height; i+=2)
			{
				for (int j = 0; j<byteWidth; j+=8)
				{
					oldPos = i*byteWidth+j;

					pos = (i/2)*reducedByteWidth+j/2;
					tempMap[pos]+=(byte)(data[oldPos]/divisor+data[oldPos+4]/divisor+data[oldPos+byteWidth]/divisor+data[oldPos+byteWidth+4]/divisor);
					oldPos++;
					tempMap[pos+1]+= (byte)(data[oldPos]/divisor+data[oldPos+4]/divisor+data[oldPos+byteWidth]/divisor+data[oldPos+byteWidth+4]/divisor);
					oldPos++;
					tempMap[pos+2]+= (byte)(data[oldPos]/divisor+data[oldPos+4]/divisor+data[oldPos+byteWidth]/divisor+data[oldPos+byteWidth+4]/divisor);
					oldPos++;
					tempMap[pos+3]+= (byte)(data[oldPos]/divisor+data[oldPos+4]/divisor+data[oldPos+byteWidth]/divisor+data[oldPos+byteWidth+4]/divisor);
				}
			}

			for (int i = 0; i<map.Length; i++)
			{
				map[i] = (byte)tempMap[i];
			}

			return map;*/
		}

		/// <summary>
		/// Extracts the alpha channel from a raw 32-bit image with no headers
		/// </summary>
		/// <param name="rawData">The raw 32-bit image as a byte array</param>
		/// <returns>The alpha channel from the image as a byte array</returns>
		static byte[] GetAlpha(byte[] rawData)
		{
			byte[] data = new byte[rawData.Length/4];

			for (int i = 0; i<data.Length; i++)
			{
				data[i] = rawData[i*4 + 3];
			}

			return data;
		}

		/// <summary>
		/// Generates an alpha bit mask for a block in a DDS image
		/// </summary>
		/// <param name="a0">Alpha level 0</param>
		/// <param name="a1">Alpha level 1</param>
		/// <param name="Num">Number of graded alpha levels between a0 and a1 inclusive - either 6 or 8</param>
		/// <param name="In">Alpha channel levels for the block</param>
		/// <param name="Mask">Alpha bit mask to be stored in DXT3/DXT5 images</param>
		/// <returns>Alpha bitmask for DXT1 images</returns>
		static byte[] GenAlphaBitMask(byte a0, byte a1, uint Num, byte[] In, byte[] Mask)
		{
			byte[] Alphas = new byte[8];
			byte[] M = new byte[16];
			byte[] Out = new byte[16];
			uint	i, j, Closest, Dist;

			Alphas[0] = a0;
			Alphas[1] = a1;

			// 8-alpha or 6-alpha block?    
			if (Num == 8) 
			{    
				// 8-alpha block:  derive the other six alphas.    
				// Bit code 000 = alpha_0, 001 = alpha_1, others are interpolated.
				Alphas[2] = (byte)((6 * Alphas[0] + 1 * Alphas[1] + 3) / 7);	// bit code 010
				Alphas[3] = (byte)((5 * Alphas[0] + 2 * Alphas[1] + 3) / 7);	// bit code 011
				Alphas[4] = (byte)((4 * Alphas[0] + 3 * Alphas[1] + 3) / 7);	// bit code 100
				Alphas[5] = (byte)((3 * Alphas[0] + 4 * Alphas[1] + 3) / 7);	// bit code 101
				Alphas[6] = (byte)((2 * Alphas[0] + 5 * Alphas[1] + 3) / 7);	// bit code 110
				Alphas[7] = (byte)((1 * Alphas[0] + 6 * Alphas[1] + 3) / 7);	// bit code 111  
			}    
			else 
			{  
				// 6-alpha block.    
				// Bit code 000 = alpha_0, 001 = alpha_1, others are interpolated.
				Alphas[2] = (byte)((4 * Alphas[0] + 1 * Alphas[1] + 2) / 5);	// Bit code 010
				Alphas[3] = (byte)((3 * Alphas[0] + 2 * Alphas[1] + 2) / 5);	// Bit code 011
				Alphas[4] = (byte)((2 * Alphas[0] + 3 * Alphas[1] + 2) / 5);	// Bit code 100
				Alphas[5] = (byte)((1 * Alphas[0] + 4 * Alphas[1] + 2) / 5);	// Bit code 101
				Alphas[6] = 0x00;										// Bit code 110
				Alphas[7] = 0xFF;										// Bit code 111
			}

			for (i = 0; i < 16; i++) 
			{
				Closest = uint.MaxValue;
				for (j = 0; j < 8; j++) 
				{
					Dist = (uint)(In[i] - Alphas[j]);
					if (Dist < Closest) 
					{
						Closest = Dist;
						M[i] = (byte)j;
					}
				}
			}

			if (Out!=null) 
			{
				for (i = 0; i < 16; i++) 
				{
					Out[i] = Alphas[M[i]];
				}
			}

			// First three bytes.
			Mask[0] = (byte)((M[0]) | (M[1] << 3) | ((M[2] & 0x03) << 6));
			Mask[1] = (byte)((M[2] & 0x04)>>2 | (M[3] << 1) | (M[4] << 4) | ((M[5] & 0x01) << 7));
			Mask[2] = (byte)((M[5] & 0x06)>>1 | (M[6] << 2) | (M[7] << 5));

			// Second three bytes.
			Mask[3] = (byte)((M[8]) | (M[9] << 3) | ((M[10] & 0x03) << 6));
			Mask[4] = (byte)((M[10] & 0x04)>>2 | (M[11] << 1) | (M[12] << 4) | ((M[13] & 0x01) << 7));
			Mask[5] = (byte)((M[13] & 0x06)>>1 | (M[14] << 2) | (M[15] << 5));

			return Out;
		}

		/// <summary>
		/// Calculates the difference between two colours
		/// </summary>
		/// <param name="c1">First colour to difference</param>
		/// <param name="c2">Colour to difference it from</param>
		/// <returns>Distance between the two colours</returns>
		static uint Distance(Color c1, Color c2)
		{
			return  (uint)((c1.R - c2.R) * (c1.R - c2.R)) +
				(uint)((c1.G - c2.G) * (c1.G - c2.G)) +
				(uint)((c1.B - c2.B) * (c1.B - c2.B));
			/*return  (uint)Math.Abs(c1.R - c2.R) +
				(uint)Math.Abs(c1.G - c2.G) +
				(uint)Math.Abs(c1.B - c2.B);*/
		}

		static uint HSLDistance(Color c1, Color c2)
		{
			double hue1, hue2;
			double delta;

			hue1 = hue2 = 0;

			if (c1.R>c1.G && c1.R>c1.B)
			{
				delta = (c1.R - Math.Min(c1.G, c1.B));

				if (delta!=0)
				{
					hue1 = (c1.G - c1.B) / delta * 60;
				}

				if (hue1<0)
				{
					hue1+= 360;
				}
			}
			else if (c1.G>c1.B)
			{
				delta = (c1.G - Math.Min(c1.R, c1.B));

				if (delta!=0)
				{
					hue1 = (c1.B - c1.R) / delta * 60 + 120;
				}
			}
			else
			{
				delta = (c1.B - Math.Min(c1.G, c1.R));

				if (delta!=0)
				{
					hue1 = (c1.R - c1.G) / delta * 60 + 240;
				}
			}

			if (c2.R>c2.G && c2.R>c2.B)
			{
				delta = (c2.R - Math.Min(c2.G, c2.B));

				if (delta!=0)
				{
					hue2 = (c2.G - c2.B) / delta * 60;
				}

				if (hue2<0)
				{
					hue2+= 360;
				}
			}
			else if (c1.G>c1.B)
			{
				delta = (c2.G - Math.Min(c2.R, c2.B));

				if (delta!=0)
				{
					hue2 = (c2.B - c2.R) / delta * 60 + 120;
				}
			}
			else
			{
				delta = (c2.B - Math.Min(c2.G, c2.R));
				if (delta!=0)
				{
					hue2 = (c2.R - c2.G) / delta * 60 + 240;
				}
			}

			return (uint)Math.Abs(hue1 - hue2);
			/*return  (uint)Math.Abs(c1.R - c2.R) +
				(uint)Math.Abs(c1.G - c2.G) +
				(uint)Math.Abs(c1.B - c2.B);*/
		}

		/// <summary>
		/// Finds two endpoint RGB565 colours that are the most different from each other within a DDS image block
		/// </summary>
		/// <param name="Block">The 4x4 pixel block of RGB565 colours</param>
		/// <param name="ex0">Reference to the first endpoint colour</param>
		/// <param name="ex1">Reference to the secont endpoint colour that is most different from the first</param>
		static void ChooseEndpoints(ushort[] Block, ref ushort ex0, ref ushort ex1)
		{
			int i, j;
			Color[] Colours = new Color[16];
			int Farthest = -1;//, nextFarthest = -1;
			uint d;
			//short idx_i = -1, idx_j = -1;

			for (i = 0; i < 16; i++) 
			{
				Colours[i] = ShortToColor888(Block[i]);
			}

			for (i = 0; i < 16; i++) 
			{
				for (j = i+1; j < 16; j++) 
				{
					d = Distance(Colours[i], Colours[j]);

					if (d > Farthest) 
					{
						Farthest = (int)d;
						ex0 = Block[i];
						ex1 = Block[j];
					}
				}
			}

			/*
			//IBBoard custom version follows:
			//Find the two most distant colours and ignore them
			for (i = 0; i < 16; i++) 
			{
				for (j = i+1; j < 16; j++) 
				{
					d = Distance(Colours[i], Colours[j]);
					if (d > Farthest) 
					{
						nextFarthest = Farthest;
						Farthest = (int)d;
						//ex0_next = ex0;
						//ex1_next = ex1;
						//ex0 = Block[i];
						//ex1 = Block[j];
						idx_i = (short)i;
						idx_j = (short)j;
					}
				}
			}

			//Then find the most distant of all the colours excluding the two most distant ones
			Farthest = -1;
			nextFarthest = -1;

			for (i = 0; i < 16; i++) 
			{
				if (i!=idx_i)
				{
					for (j = i+1; j < 16; j++) 
					{
						if (idx_j != j)
						{
							d = Distance(Colours[i], Colours[j]);

							if (d > Farthest) 
							{
								nextFarthest = Farthest;
								Farthest = (int)d;
								//ex0_next = ex0;
								//ex1_next = ex1;
								ex0 = Block[i];
								ex1 = Block[j];
							}
						}
					}
				}
			}*/

			return;
		}

		/// <summary>
		/// Finds two endpoints within the alpha channel of a DDS image block that are most different from each other
		/// </summary>
		/// <param name="Block">The 4x4 pixel block of alpha channel data</param>
		/// <param name="a0">Reference to the first endpoint alpha value</param>
		/// <param name="a1">Reference to the second endpoint alpha value that is most distant from the first</param>
		static void ChooseAlphaEndpoints(byte[] Block, ref byte a0, ref byte a1)
		{
			uint	i;
			uint	Lowest = 0xFF, Highest = 0;
			a0 = (byte)Highest;
			a1 = (byte)Lowest;

			for (i = 0; i < 16; i++) 
			{
				if (Block[i] < Lowest) 
				{
					a1 = Block[i];  // a1 is the lower of the two.
					Lowest = Block[i];
				}
				
				if (Block[i] > Highest) 
				{
					a0 = Block[i];  // a0 is the higher of the two.
					Highest = Block[i];
				}
			}

			return;
		}


		/// <summary>
		/// Corrects the colour endpoints in accordance with the DDS algorithm so that ex0 < ex1 
		/// only when the block has a 1-bit alpha channel with transparency
		/// </summary>
		/// <param name="ex0">Colour endpoint 0 as an RGB565 ushort</param>
		/// <param name="ex1">Colour endpoint 1 as an RGB565 ushort</param>
		/// <param name="HasAlpha">Whether the block has transparent bits</param>
		/// <remarks>This function should only be used with DXT1 textures, however tests with Photoshop's
		/// DDS plugin show that DXT3 and DXT5 images will show black for pixels given the value 0x11 if
		/// the colour endpoints are not also corrected.</remarks>
		static void CorrectEndDXT1(ref ushort ex0, ref ushort ex1, bool HasAlpha)
		{
			ushort Temp;

			if (HasAlpha) 
			{
				if (ex0 > ex1) 
				{
					Temp = ex0;
					ex0 = ex1;
					ex1 = Temp;
				}
			}
			else 
			{
				if (ex0 < ex1) 
				{
					Temp = ex0;
					ex0 = ex1;
					ex1 = Temp;
				}
			}
		}

		#region New 'zoom' code for Triangular filter

		struct Contribution
		{
			public int n;
			public ContribPixel[] p;
		}

		struct ContribPixel
		{
			public int pixel;
			public double weight;
		}

		/// <summary>
		/// 'Zooms' the image to resize it, using a filter such as Triangular
		/// </summary>
		/// <param name="dest">Byte array to copy the newly resized 32-bit image to</param>
		/// <param name="src">Byte array containing the existing 32-bit image</param>
		/// <param name="width">Current image width</param>
		/// <param name="height">Current image height</param>
		/// <param name="newWidth">Width to make the new image</param>
		/// <param name="newHeight">Height to make the new image</param>
		/// <param name="filter">Filter to use on resizing</param>
		/// <param name="fwidth">Filter width</param>
		/// <remarks>Currently only Triangular filter will be supported</remarks>
		public static void Zoom(byte[] dest, byte[] src, int width, int height, int newWidth, int newHeight, Filter filter, double fwidth)
		{
			double xscale, yscale;
			int xx;
			int i, j, k;
			int n;
			double center, left, right;
			double calcWidth, fscale, weight;
			byte pel = 0, pel2;
			bool bPelDelta;
			Contribution[] contribY = new Contribution[newHeight];
			Contribution contribX = new Contribution();
			int c = 0;


			xscale = newWidth/(double)width;
			yscale = newHeight/(double)height;

			
			/* create intermediate column to hold horizontal dst column zoom */
			byte[] tmp = new byte[height];

			for (c = 0; c<4; c++)
			{
				if(yscale < 1.0)
				{
					calcWidth = fwidth / yscale;
					fscale = 1.0 / yscale;
					for(i = 0; i < newHeight; ++i)
					{
						contribY[i].n = 0;
						contribY[i].p = new ContribPixel[(int)calcWidth * 2 + 1];

						center = (double) i / yscale;
						left = Math.Ceiling(center - calcWidth);
						right = Math.Floor(center + calcWidth);
						for(j = (int)left; j <= right; ++j) 
						{
							weight = center - (double) j;
							weight = triangle_filter(weight / fscale) / fscale;
							if(j < 0) 
							{
								n = -j;
							} 
							else if(j >= height) 
							{
								n = (height - j) + height - 1;
							} 
							else 
							{
								n = j;
							}
							k = contribY[i].n++;
							contribY[i].p[k].pixel = n;
							contribY[i].p[k].weight = weight;
						}
					}
				} 
				else 
				{
					for(i = 0; i < newHeight; ++i) 
					{
						contribY[i].n = 0;
						contribY[i].p = new ContribPixel[(int)(fwidth * 2 + 1)];
						if (contribY[i].p.Length == 0) 
						{
							return;
						}
						center = (double) i / yscale;
						left = Math.Ceiling(center - fwidth);
						right = Math.Floor(center + fwidth);
						for(j = (int)left; j <= right; ++j) 
						{
							weight = center - (double) j;
							weight = triangle_filter(weight);
							if(j < 0) 
							{
								n = -j;
							} 
							else if(j >= height) 
							{
								n = (height - j) + height - 1;
							} 
							else 
							{
								n = j;
							}
							k = contribY[i].n++;
							contribY[i].p[k].pixel = n;
							contribY[i].p[k].weight = weight;
						}
					}
				}
			
				for(xx = 0; xx < newWidth; xx++)
				{
					calc_x_contrib(ref contribX, xscale, fwidth, 
						newWidth, width, filter, xx);

					/* Apply horz filter to make dst column in tmp. */
					for(k = 0; k < height; ++k)
					{
						weight = 0.0;
						bPelDelta = false;
						// Denton:  Put get_pixel source here
						//pel = get_pixel(src, contribX.p[0].pixel, k);
						try
						{
							pel = src[k * 4 * width + contribX.p[0].pixel * 4 + c];
						}
						catch (IndexOutOfRangeException)
						{
							//FIXME: Exception when newWidth=1, k=width-1, contribX.p[0].pixel=width and c=0
							pel = 0;
						}

						for(j = 0; j < contribX.n; ++j)
						{
							// Denton:  Put get_pixel source here
							//pel2 = get_pixel(src, contribX.p[j].pixel, k);
							try
							{
								pel2 = src[k * 4 * width + contribX.p[j].pixel * 4 + c];
							}
							catch(IndexOutOfRangeException)
							{
								//FIXME: As above
								pel2 = 0;
							}

							if(pel2 != pel)
								bPelDelta = true;
							weight += pel2 * contribX.p[j].weight;
						}
						weight = bPelDelta ? roundcloser(weight) : pel;

						tmp[k] = CLAMP((byte)weight, BLACK_PIXEL, WHITE_PIXEL);
					} /* next row in temp column */

					/* The temp column has been built. Now stretch it 
						 vertically into dst column. */
					for(i = 0; i < newHeight; i++)
					{
						weight = 0.0;
						bPelDelta = false;

						try
						{
							pel = tmp[contribY[i].p[0].pixel];
						}
						catch (IndexOutOfRangeException)
						{
							//FIXME: As above
							pel = 0;
						}

						for(j = 0; j < contribY[i].n; ++j)
						{
							try
							{
								pel2 = tmp[contribY[i].p[j].pixel];
							}
							catch (IndexOutOfRangeException)
							{
								//FIXME: As above
								pel2 = 0;
							}							if(pel2 != pel)
								bPelDelta = true;
							weight += pel2 * contribY[i].p[j].weight;
						}
						weight = bPelDelta ? roundcloser(weight) : pel;

						try
						{
							dest[i * 4 * newWidth + xx * 4 + c] = CLAMP((byte)weight, BLACK_PIXEL, WHITE_PIXEL);
						}
						catch (IndexOutOfRangeException)
						{
							//FIXME: As above
						}
					} /* next dst row */
				} /* next dst column */
			}
		}

		private static void calc_x_contrib(ref Contribution contribX, double xscale, double fwidth, int dstwidth, int srcwidth, Filter filter, int i)
		{
			double width;
			double fscale;
			double center, left, right;
			double weight;
			int j, k, n;

			if(xscale < 1.0)
			{
				/* Shrinking image */
				width = fwidth / xscale;
				fscale = 1.0 / xscale;

				contribX.n = 0;
				contribX.p = new ContribPixel[(int)(width * 2 + 1)];

				center = (double) i / xscale;
				left = Math.Ceiling(center - width);
				right = Math.Floor(center + width);
				for(j = (int)left; j <= right; ++j)
				{
					weight = center - (double) j;
					weight = triangle_filter(weight / fscale) / fscale;

					if(j < 0)
						n = -j;
					else if(j >= srcwidth)
						n = (srcwidth - j) + srcwidth - 1;
					else
						n = j;
			
					k = contribX.n++;
					contribX.p[k].pixel = n;
					contribX.p[k].weight = weight;
				}
	
			}
			else
			{
				/* Expanding image */
				contribX.n = 0;
				contribX.p = new ContribPixel[(int)(fwidth * 2 + 1)];

				center = (double) i / xscale;
				left = Math.Ceiling(center - fwidth);
				right = Math.Floor(center + fwidth);

				for(j = (int)left; j <= right; ++j)
				{
					weight = center - (double) j;
					weight = triangle_filter(weight);
					if(j < 0) 
					{
						n = -j;
					} 
					else if(j >= srcwidth) 
					{
						n = (srcwidth - j) + srcwidth - 1;
					} 
					else 
					{
						n = j;
					}
					k = contribX.n++;
					contribX.p[k].pixel = n;
					contribX.p[k].weight = weight;
				}
			}
		}

		private static int roundcloser(double val)
		{
			return (int)Math.Round(val, 0);
		}

		private static byte CLAMP(byte v, byte l, byte h)
			//value, low, high
		{
			return (v<l) ? l : ((v>h) ? h : v);
		}

		private static double triangle_filter(double t)
		{
			if(t < 0.0) t = -t;
			if(t < 1.0) return 1.0 - t;
			return 0.0;
		}

		#endregion
	}
}