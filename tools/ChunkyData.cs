// This file is a part of the Relic Tools and is copyright 2006-2018 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU GPL license, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.
using System;
using System.IO;

namespace IBBoard.Relic.RelicTools
{
	/// <summary>
	/// Summary description for ChunkyData.
	/// </summary>
	public abstract class ChunkyData : ChunkyChunk
	{
		public static readonly byte[] TGA_Greyscale_Header_a = new byte[]{0x00, 0x00, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};
		public static readonly byte[] TGA_Greyscale_Header_b = new byte[]{0x08, 0x00}; //Adobe Photoshop sets the second byte (the ImageDescriptor) as 0x08 but that means alpha transparency, which only 16/32-bit images should have, which confuses The GIMP and is technically against the standards
		public static readonly byte[] TGA_Colour_Header_a = new byte[]{0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00};
		public static readonly byte[] TGA_Colour_Header_b = new byte[]{0x20, 0x08};
		public static readonly byte[] DTX_Header_a = new byte[]{0x44, 0x44, 0x53, 0x20,//"DDS "
																   0x7C, 0x00, 0x00, 0x00,//size - fixed to 124
																   //0x07, 0x10, 0x02, 0x00};//valid field flags - DDSD_CAPS, DDSD_PIXELFORMAT, DDSD_WIDTH, DDSD_HEIGHT
																   0x07, 0x10, 0x0A, 0x00};
		public static readonly byte[] DXT1_Header_b = new byte[]{0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
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

		public static readonly byte[] DXT3_Header_b = new byte[]{0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
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
		
		public static readonly byte[] DXT5_Header_b = new byte[]{0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
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

		public ChunkyData(string ID_in, int version_in, string name_in):base(ChunkyChunkType.Data, ID_in, version_in, name_in)
		{}

		protected string GetBaseDisplayDetails()
		{
			return base.GetDisplayDetails();
		}

		public override byte[] GetBytes()
		{
			
			byte[] file = new byte[Length];
			byte[] temp;
			int pos = 0;
			System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
			
			enc.GetBytes("DATA"+id).CopyTo(file,0);
			pos=id.Length+4;

			BitConverter.GetBytes(version).CopyTo(file, pos);
			pos+=4;
			BitConverter.GetBytes(DataLength).CopyTo(file, pos);
			pos+=4;
			BitConverter.GetBytes(NameDataLength).CopyTo(file, pos);
			pos+=4;
			temp = GetNameBytes();
			temp.CopyTo(file, pos);
			pos+=temp.Length;
			GetDataBytes().CopyTo(file, pos);

			return file;
		}

		public abstract byte[] GetDataBytes();

		protected byte[] GetStartBytes()
		{
			byte[] file = new byte[NameDataLength+20];
			int pos = 0;
			System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
			
			enc.GetBytes("DATA"+id).CopyTo(file,0);
			pos=id.Length+4;

			BitConverter.GetBytes(version).CopyTo(file, pos);
			pos+=4;
			BitConverter.GetBytes(DataLength).CopyTo(file, pos);
			pos+=4;
			BitConverter.GetBytes(NameDataLength).CopyTo(file, pos);
			pos+=4;
			GetNameBytes().CopyTo(file, pos);

			return file;
		}

		public virtual bool Save(string path)
		{			
			string name = this.ParentFile.Name.Substring(0, this.ParentFile.Name.LastIndexOf('.'));
			this.Save(new DirectoryInfo(path), name);
			return true;
		}

		public virtual void Save(DirectoryInfo dir, string fileBaseName)
		{
		}

		public override bool Savable
		{
			get{return false;}
		}

		public override string GetValidationString()
		{
			return "DATA"+ID;
		}
	}
}
