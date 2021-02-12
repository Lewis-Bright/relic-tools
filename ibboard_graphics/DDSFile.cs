// This file (DDSFile.cs) is a part of the IBBoard.Graphics library and is copyright 2009 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU LGPL license, either version 3 of the License or (at your option) any later version. Please see COPYING.LGPL for more information and the full license.

using System;
using System.Drawing;
using IBBoard.Graphics.OpenILPort;
using IBBoard.Graphics.SquishWrapper;

namespace IBBoard.Graphics
{
	/// <summary>
	/// Summary description for DDSFile.
	/// </summary>
	public class DDSFile
	{
		private byte[] data;
        private Converter.DXTType type;
		private int width, height;
		private byte blockwidth;
		private byte alphawidth;

		public DDSFile(byte[] file)
		{
			data = file;
			height = data[12]+(data[13]<<8)+(data[14]<<16)+(data[15]<<24);
			width = data[16]+(data[17]<<8)+(data[18]<<16)+(data[19]<<24);

			switch(data[87])
			{
				case 0x31:
					type = Converter.DXTType.DXT1;
					blockwidth=8;
					alphawidth = 0;
					break;
				case 0x33:
                    type = Converter.DXTType.DXT3;
					blockwidth = 16;
					alphawidth = 8;
					break;
				case 0x35:
                    type = Converter.DXTType.DXT5;
					blockwidth = 16;
					alphawidth = 8;
					break;
				default:
					throw new InvalidOperationException("Unsupported DDS type - only DXT1, DXT3 and DXT5 are currently supported");
			}
		}

        private DDSFile(byte[] ddsData, int imgWidth, int imgHeight, Converter.DXTType compression)
		{
			data = new byte[128+ddsData.Length];
			type = compression;
			width = imgWidth;
			height= imgHeight;
			
			switch(type)
			{
                case Converter.DXTType.DXT1:
					blockwidth=8;
					alphawidth = 0;
                    Converter.DXT1_Header.CopyTo(data, 0);
					break;
                case Converter.DXTType.DXT3:
					blockwidth = 16;
					alphawidth = 8;
                    Converter.DXT3_Header.CopyTo(data, 0);
					break;
                case Converter.DXTType.DXT5:
					blockwidth = 16;
					alphawidth = 8;
                    Converter.DXT5_Header.CopyTo(data, 0);
					break;
				default:
					throw new InvalidOperationException("Unsupported DDS type - only DXT1, DXT3 and DXT5 are currently supported");
			}

			data[12] = (byte)height;
			data[13] = (byte)(height >> 8);
			data[14] = (byte)(height >> 16);
			data[15] = (byte)(height >> 24);
			data[16] = (byte)width;
			data[17] = (byte)(width >> 8);
			data[18] = (byte)(width >> 16);
			data[19] = (byte)(width >> 24);

			int mips = CalcMips(width, height);
			data[28] = (byte)mips;
			data[29] = (byte)(mips >> 8);
			data[30] = (byte)(mips >> 16);
			data[31] = (byte)(mips >> 24);

			ddsData.CopyTo(data, 128);
		}

		public int Width
		{
			get{return width;}
		}

		public int Height
		{
			get{return height;}
		}

        public Converter.DXTType Type
		{
			get{ return type;}
		}
		
		private static int CalcMips(int width, int height)
		{
			return (int)Math.Ceiling(Math.Log(Math.Max(width, height))/Math.Log(2))+1;
		}

        private static int CalcDataSize(Converter.DXTType type, int width, int height, int mipmaps)
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

        private static int BlockSize(Converter.DXTType type)
		{
            if (type == Converter.DXTType.DXT1)
			{
				return 8;
			}
            else if (type == Converter.DXTType.DXT5 || type == Converter.DXTType.DXT3)
			{
				return 16;
			}
			else
			{
				throw new InvalidOperationException("Unsupported DDS type - only DXT1, DXT3 and DXT5 are currently supported");
			}
		}

		private ushort GetColour0(int pos)
		{
			int start = 128+(pos*blockwidth) + alphawidth;
			return (ushort)(data[start]+((ushort)data[start+1]<<8));
		}

		private ushort GetColour1(int pos)
		{
			int start = 128+(pos*blockwidth) + alphawidth;
			return (ushort)(data[start+2]+((ushort)data[start+3]<<8));
		}

		private byte GetPixelColour(int pos, byte pixel)
		{
			int start = 128+(pos*blockwidth) + alphawidth;
			return data[start+4+pixel];
		}

		private byte[] GetInterpolatedAlpha(int pos)
		{
			int start = 128+(pos*blockwidth);
			return new byte[]{data[start], data[start+1], data[start+2], data[start+3], data[start+4], data[start+5], data[start+6], data[start+7]};
		}

		private ushort[] GetLinearAlpha(int pos)
		{
			int start = 128+(pos*blockwidth);
			return new ushort[]{(ushort)(data[start]+ (data[start+1]<<8)), (ushort)(data[start+2]+ (ushort)(data[start+3]<<8)), (ushort)(data[start+4] + (data[start+5]<<8)), (ushort)(data[start+6] + (data[start+7]<<8))};
		}

		public byte[] Bytes
		{
			get{return data;}
		}

		public byte[] GetTGAData()
		{
			byte[] tgaFile = new byte[width*height*4+18];
			
			ImageConverter.TGA_Colour_Header.CopyTo(tgaFile, 0);
			tgaFile[12] = (byte)width;
			tgaFile[13] = (byte)(width>>8);			
			tgaFile[14] = (byte)height;
			tgaFile[15] = (byte)(height>>8);

			int i, j;
			int texels_per_row = Math.Max(width >> 2, 1);
			int texel_rows = Math.Max(height >> 2, 1);
			int stride = width;

			byte[] interpolated_alpha;
			ushort[] linear_alpha;
			uint[] color_map = new uint[4];
			byte[] alpha_map = new byte[8];
			uint[,] alphadata = new uint[4,4];
			uint rgba;
			int k, l;
			uint pixel_alpha_01, pixel_alpha_23;
			ushort colour0, colour1;
			int texelnum, tgacounter, pixelColour;
			byte index;

            if (this.Type == Converter.DXTType.DXT1)
			{
				for (i = 0; i < texel_rows; i++) 
				{
					for (j = 0; j < texels_per_row; j++) 
					{
						for (k=0; k<4; k++) 
						{
							for (l=0; l<4; l++) 
							{
								alphadata[k,l] = 0xffffffff;
							}
						}

						texelnum = i*texels_per_row + j;

						colour0 = GetColour0(texelnum);
						colour1 = GetColour1(texelnum);

						color_map[0] = ImageConverter.rgb565_to_rgba(colour0);
						color_map[1] = ImageConverter.rgb565_to_rgba(colour1);

						if (colour0 <= colour1) 
						{
							color_map[2] = (uint)(((byte)color_map[1] + (byte)color_map[0]) / 2) + 
								((uint)(((byte)(color_map[1]>>8) + (byte)(color_map[0]>>8)) / 2)<<8) + 
								((uint)(((byte)(color_map[1]>>16) + (byte)(color_map[0]>>16)) / 2)<<16) + 
								((uint)(((byte)(color_map[1]>>24) + (byte)(color_map[0]>>24)) / 2)<<24);
							color_map[3] = 0x00000000;//colour map3 = transparent

							for (k=0; k<4; k++) 
							{
								pixelColour = GetPixelColour(texelnum, (byte)k);
								tgacounter = (i*4*texels_per_row + j + (k * texels_per_row))*4;

								for (l=0; l<4; l++) 
								{
									index = (byte)((pixelColour >> (l*2)) & 0x03);
									rgba = color_map[index];
									tgaFile[(tgacounter + l)*4 + 18] = (byte)rgba;
									tgaFile[(tgacounter + l)*4 + 19] = (byte)(rgba>>8);
									tgaFile[(tgacounter + l)*4 + 20] = (byte)(rgba>>16);
									tgaFile[(tgacounter + l)*4 + 21] = (byte)((index==3)?0x00:0xff);
								}
							}
						} 
						else 
						{
							color_map[2] = (uint)((2 * (byte)color_map[0] + (byte)color_map[1] + 1) / 3) + 
								((uint)((2 * (byte)(color_map[0]>>8) + (byte)(color_map[1]>>8) + 1) / 3)<<8) + 
								((uint)((2 * (byte)(color_map[0]>>16) + (byte)(color_map[1]>>16) + 1) / 3)<<16) + 
								((uint)((2 * (byte)(color_map[0]>>24) + (byte)(color_map[1]>>24) + 1) / 3)<<24);
							color_map[3] = (uint)((2 * (byte)color_map[1] + (byte)color_map[0] + 1) / 3) + 
								((uint)((2 * (byte)(color_map[1]>>8) + (byte)(color_map[0]>>8) + 1) / 3)<<8) + 
								((uint)((2 * (byte)(color_map[1]>>16) + (byte)(color_map[0]>>16) + 1) / 3)<<16) + 
								((uint)((2 * (byte)(color_map[1]>>24) + (byte)(color_map[0]>>24) + 1) / 3)<<24);

							for (k=0; k<4; k++) 
							{
								pixelColour = GetPixelColour(texelnum, (byte)k);
								tgacounter = (i*4*texels_per_row + j + (k * texels_per_row))*4;

								for (l=0; l<4; l++) 
								{
									index = (byte)((pixelColour >> (l*2)) & 0x03);
									rgba = color_map[index] & alphadata[k,l];
									tgaFile[(tgacounter + l)*4 + 18] = (byte)rgba;
									tgaFile[(tgacounter + l)*4 + 19] = (byte)(rgba>>8);
									tgaFile[(tgacounter + l)*4 + 20] = (byte)(rgba>>16);
									tgaFile[(tgacounter + l)*4 + 21] = 0xff;
								}
							}
						}
					}
				}
			}
            else if (this.Type == Converter.DXTType.DXT5) 
			{				
				for (i = 0; i < texel_rows; i++) 
				{
					for (j = 0; j < texels_per_row; j++) 
					{
						interpolated_alpha = GetInterpolatedAlpha(i*texels_per_row + j);
						alpha_map[0] = interpolated_alpha[0];
						alpha_map[1] = interpolated_alpha[1];
						if (alpha_map[0] <= alpha_map[1]) 
						{
							alpha_map[2] = (byte)((4*alpha_map[0] + 1*alpha_map[1] + 2) / 5); // Bit code 010
							alpha_map[3] = (byte)((3*alpha_map[0] + 2*alpha_map[1] + 2) / 5); // Bit code 011
							alpha_map[4] = (byte)((2*alpha_map[0] + 3*alpha_map[1] + 2) / 5); // Bit code 100
							alpha_map[5] = (byte)((1*alpha_map[0] + 4*alpha_map[1] + 2) / 5); // Bit code 101
							alpha_map[6] = 0;                                         // Bit code 110
							alpha_map[7] = 255;                                       // Bit code 111
						} 
						else 
						{
							alpha_map[2] = (byte)((6*alpha_map[0] + 1*alpha_map[1] + 3) / 7); // bit code 010
							alpha_map[3] = (byte)((5*alpha_map[0] + 2*alpha_map[1] + 3) / 7); // bit code 011
							alpha_map[4] = (byte)((4*alpha_map[0] + 3*alpha_map[1] + 3) / 7); // bit code 100
							alpha_map[5] = (byte)((3*alpha_map[0] + 4*alpha_map[1] + 3) / 7); // bit code 101
							alpha_map[6] = (byte)((2*alpha_map[0] + 5*alpha_map[1] + 3) / 7); // bit code 110
							alpha_map[7] = (byte)((1*alpha_map[0] + 6*alpha_map[1] + 3) / 7); // bit code 111
						}

						pixel_alpha_01 = (uint)(((uint)interpolated_alpha[4] << 16) |
							((uint)interpolated_alpha[3] << 8) |
							((uint)interpolated_alpha[2]));
						pixel_alpha_23 = (uint)(((uint)interpolated_alpha[7] << 16) |
							((uint)interpolated_alpha[6] << 8) |
							((uint)interpolated_alpha[5]));

						alphadata[0,0] = (uint)(alpha_map[(pixel_alpha_01 >>  0) & 0x07] << 24) | 0x00ffffff;
						alphadata[0,1] = (uint)(alpha_map[(pixel_alpha_01 >>  3) & 0x07] << 24) | 0x00ffffff;
						alphadata[0,2] = (uint)(alpha_map[(pixel_alpha_01 >>  6) & 0x07] << 24) | 0x00ffffff;
						alphadata[0,3] = (uint)(alpha_map[(pixel_alpha_01 >>  9) & 0x07] << 24) | 0x00ffffff;
						alphadata[1,0] = (uint)(alpha_map[(pixel_alpha_01 >> 12) & 0x07] << 24) | 0x00ffffff;
						alphadata[1,1] = (uint)(alpha_map[(pixel_alpha_01 >> 15) & 0x07] << 24) | 0x00ffffff;
						alphadata[1,2] = (uint)(alpha_map[(pixel_alpha_01 >> 18) & 0x07] << 24) | 0x00ffffff;
						alphadata[1,3] = (uint)(alpha_map[(pixel_alpha_01 >> 21) & 0x07] << 24) | 0x00ffffff;
						alphadata[2,0] = (uint)(alpha_map[(pixel_alpha_23 >>  0) & 0x07] << 24) | 0x00ffffff;
						alphadata[2,1] = (uint)(alpha_map[(pixel_alpha_23 >>  3) & 0x07] << 24) | 0x00ffffff;
						alphadata[2,2] = (uint)(alpha_map[(pixel_alpha_23 >>  6) & 0x07] << 24) | 0x00ffffff;
						alphadata[2,3] = (uint)(alpha_map[(pixel_alpha_23 >>  9) & 0x07] << 24) | 0x00ffffff;
						alphadata[3,0] = (uint)(alpha_map[(pixel_alpha_23 >> 12) & 0x07] << 24) | 0x00ffffff;
						alphadata[3,1] = (uint)(alpha_map[(pixel_alpha_23 >> 15) & 0x07] << 24) | 0x00ffffff;
						alphadata[3,2] = (uint)(alpha_map[(pixel_alpha_23 >> 18) & 0x07] << 24) | 0x00ffffff;
						alphadata[3,3] = (uint)(alpha_map[(pixel_alpha_23 >> 21) & 0x07] << 24) | 0x00ffffff;
						
						texelnum = i*texels_per_row + j;

						color_map[0] = ImageConverter.rgb565_to_rgba(GetColour0(texelnum));
						color_map[1] = ImageConverter.rgb565_to_rgba(GetColour1(texelnum));

						color_map[2] = (uint)((2 * (byte)color_map[0] + (byte)color_map[1] + 1) / 3) + 
							((uint)((2 * (byte)(color_map[0]>>8) + (byte)(color_map[1]>>8) + 1) / 3)<<8) + 
							((uint)((2 * (byte)(color_map[0]>>16) + (byte)(color_map[1]>>16) + 1) / 3)<<16) + 
							((uint)((2 * (byte)(color_map[0]>>24) + (byte)(color_map[1]>>24) + 1) / 3)<<24);
						color_map[3] = (uint)((2 * (byte)color_map[1] + (byte)color_map[0] + 1) / 3) + 
							((uint)((2 * (byte)(color_map[1]>>8) + (byte)(color_map[0]>>8) + 1) / 3)<<8) + 
							((uint)((2 * (byte)(color_map[1]>>16) + (byte)(color_map[0]>>16) + 1) / 3)<<16) + 
							((uint)((2 * (byte)(color_map[1]>>24) + (byte)(color_map[0]>>24) + 1) / 3)<<24);

						for (k=0; k<4; k++) 
						{
							pixelColour = GetPixelColour(texelnum, (byte)k);
							tgacounter = (i*4*texels_per_row + j + (k * texels_per_row))*4;

							for (l=0; l<4; l++) 
							{
								index = (byte)((pixelColour >> (l*2)) & 0x03);
								rgba = color_map[index] & alphadata[k,l];
								tgaFile[(tgacounter + l)*4 + 18] = (byte)rgba;
								tgaFile[(tgacounter + l)*4 + 19] = (byte)(rgba>>8);
								tgaFile[(tgacounter + l)*4 + 20] = (byte)(rgba>>16);
								tgaFile[(tgacounter + l)*4 + 21] = (byte)(rgba>>24);
							}
						}
					}
				}
			}
            else if (this.Type == Converter.DXTType.DXT3) 
			{
				for (i = 0; i < texel_rows; i++) 
				{
					for (j = 0; j < texels_per_row; j++) 
					{
						linear_alpha = GetLinearAlpha(i*texels_per_row + j);

						for (k=0; k<4; k++) 
						{
							alphadata[k,0] = (uint)((((uint)linear_alpha[k] << 4) & 0xf0) << 24) | 0x00ffffff;
							alphadata[k,1] = (uint)((((uint)linear_alpha[k] << 0) & 0xf0) << 24) | 0x00ffffff;
							alphadata[k,2] = (uint)((((uint)linear_alpha[k] >> 4) & 0xf0) << 24) | 0x00ffffff;
							alphadata[k,3] = (uint)((((uint)linear_alpha[k] >> 8) & 0xf0) << 24) | 0x00ffffff;
						}
						
						texelnum = i*texels_per_row + j;

						color_map[0] = ImageConverter.rgb565_to_rgba(GetColour0(texelnum));
						color_map[1] = ImageConverter.rgb565_to_rgba(GetColour1(texelnum));

						color_map[2] = (uint)((2 * (byte)color_map[0] + (byte)color_map[1] + 1) / 3) + 
							((uint)((2 * (byte)(color_map[0]>>8) + (byte)(color_map[1]>>8) + 1) / 3)<<8) + 
							((uint)((2 * (byte)(color_map[0]>>16) + (byte)(color_map[1]>>16) + 1) / 3)<<16) + 
							((uint)((2 * (byte)(color_map[0]>>24) + (byte)(color_map[1]>>24) + 1) / 3)<<24);
						color_map[3] = (uint)((2 * (byte)color_map[1] + (byte)color_map[0] + 1) / 3) + 
							((uint)((2 * (byte)(color_map[1]>>8) + (byte)(color_map[0]>>8) + 1) / 3)<<8) + 
							((uint)((2 * (byte)(color_map[1]>>16) + (byte)(color_map[0]>>16) + 1) / 3)<<16) + 
							((uint)((2 * (byte)(color_map[1]>>24) + (byte)(color_map[0]>>24) + 1) / 3)<<24);

						for (k=0; k<4; k++) 
						{
							pixelColour = GetPixelColour(texelnum, (byte)k);
							tgacounter = (i*4*texels_per_row + j + (k * texels_per_row))*4;

							for (l=0; l<4; l++) 
							{
								index = (byte)((pixelColour >> (l*2)) & 0x03);
								rgba = color_map[index] & alphadata[k,l];
								tgaFile[(tgacounter + l)*4 + 18] = (byte)rgba;
								tgaFile[(tgacounter + l)*4 + 19] = (byte)(rgba>>8);
								tgaFile[(tgacounter + l)*4 + 20] = (byte)(rgba>>16);
								tgaFile[(tgacounter + l)*4 + 21] = (byte)(rgba>>24);
							}
						}
					}
				}
			}
			else 
			{
				throw new InvalidOperationException("DDS file was not a DXT1, DXT3 or DXT5 file");
			}

			return tgaFile;
		}

        public static DDSFile MakeFrom32bitBGRA(byte[] rawData, Converter.DXTType DXTCFormat, int width, int height)
		{
			byte[] data = new byte[rawData.Length];
			rawData.CopyTo(data, 0);

			for (int i = 0; i < data.Length; i+= 4)
			{
				byte temp = data[i];
				data[i] = data[i+2];
				data[i+2] = temp;
			}

			return MakeFrom32bitRGBA(data, DXTCFormat, width, height);
		}

        public static DDSFile MakeFrom32bitRGBA(byte[] rawData, Converter.DXTType DXTCFormat, int width, int height)
		{
			SquishFlags format;

			switch(DXTCFormat)
			{
                case Converter.DXTType.DXT1: format = SquishFlags.kDxt1;
					break;
                case Converter.DXTType.DXT3: format = SquishFlags.kDxt3;
					break;
                case Converter.DXTType.DXT5: format = SquishFlags.kDxt5;
					break;
				default: throw new InvalidOperationException("Invalid DXT Type supplied: "+DXTCFormat.ToString());
			}

			return MakeWithMipMapsFrom32bitRGBA(rawData, DXTCFormat, width, height, format);
		}

        private static DDSFile MakeWithMipMapsFrom32bitRGBA(byte[] rawData, Converter.DXTType DXTCFormat, int width, int height, SquishFlags format)
		{
			int mips = CalcMips(width, height);
			byte[] data = new byte[CalcDataSize(DXTCFormat, width, height, mips)];
			int lastLocation = 0;
			byte[] temp;

			byte[] map = SquishWrapper.SquishWrapper.CompressImage(rawData, width, height, (int)format);
			map.CopyTo(data, 0);
			lastLocation+= map.Length;

			if (mips > 0)
			{
				for (int i = 1; i < mips; i++)
				{
					double reduction = (double)Math.Pow(2, i);
					int redWidth = (int)Math.Round(width / reduction);
					int redHeight = (int)Math.Round(height / reduction);
					map = new byte[redWidth * redHeight * 4];
					OpenILPort.Converter.Zoom(map, rawData, width, height, redWidth, redHeight, OpenILPort.Converter.Filter.Triangle, 1.0);
					temp = SquishWrapper.SquishWrapper.CompressImage(map, redWidth, redHeight, (int)format);
					temp.CopyTo(data, lastLocation);
					lastLocation+= temp.Length;
				}
			}

			return MakeFromDDSCompressedImage(data, DXTCFormat, width, height);
		}

        public static DDSFile MakeFromDDSCompressedImage(byte[] rawData, Converter.DXTType DXTCFormat, int width, int height)
		{
			return new DDSFile(rawData, width, height, DXTCFormat);
		}
	}
}