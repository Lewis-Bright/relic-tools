// This file is a part of the Relic Tools and is copyright 2006-2018 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU GPL license, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.
using System;
using System.IO;
using System.Text;
using IBBoard.Relic.RelicTools.Collections;

namespace IBBoard.Relic.RelicTools
{
	/// <summary>
	/// Summary description for ChunkyFolder.
	/// </summary>
	public class ChunkyFolder : ChunkyChunk
	{
		ChunkyCollection children;

		public ChunkyFolder(string ID_in, int version_in, string name_in, byte[] innerData):base(ChunkyChunkType.Folder, ID_in, version_in, name_in)
		{
			children = new ChunkyCollection(this);
			MemoryStream ms = new MemoryStream(innerData, false);
			BinaryReader br = new BinaryReader(ms);
			
			ChunkyChunk chunk = null;
			ChunkyDataATTR attr = null;
			ChunkyDataINFOTPAT info = null;

			int pos = 0;

			while (pos<innerData.Length)
			{
				br.BaseStream.Seek(pos, SeekOrigin.Begin);
				chunk = RelicChunkReader.ReadChunkyChunk(br.ReadBytes(innerData.Length-pos), ID_in);
				children.Add(chunk);

				if (chunk is ChunkyDataATTR)
				{
					attr = (ChunkyDataATTR)chunk;
				}
				else if (chunk is ChunkyDataINFOTPAT)
				{
					info = (ChunkyDataINFOTPAT)chunk;
				}
				else if (attr!=null && chunk is ChunkyDataDATA)
				{
					((ChunkyDataDATA)chunk).Attributes = attr;
				}
				else if  (info!=null && chunk is ChunkyDataLayer)
				{
					((ChunkyDataLayer)chunk).Info = info;
				}

				pos+=chunk.Length;
			}

		}

		public ChunkyFolder(string ID_in, int version_in, string name_in):base(ChunkyChunkType.Folder, ID_in, version_in, name_in)
		{
			children = new ChunkyCollection(this);
		}

		public ChunkyCollection Children
		{
			get { return children; }
		}

		public override byte[] GetBytes()
		{
			byte[] file = new byte[this.Length];
			byte[] temp;
			int pos = 0;

			System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();

			enc.GetBytes("FOLD"+id).CopyTo(file,0);
			pos+= 8;

			BitConverter.GetBytes(version).CopyTo(file, pos);
			pos+=4;
			BitConverter.GetBytes(DataLength).CopyTo(file, pos);
			pos+=4;
			BitConverter.GetBytes(NameDataLength).CopyTo(file, pos);
			pos+=4;
			temp = this.GetNameBytes();
			temp.CopyTo(file,pos);
			pos+=temp.Length;

			int childCount = this.children.Count;

			for (int i = 0; i<childCount; i++)
			{
				temp = children[i].GetBytes();
				temp.CopyTo(file, pos);
				pos+= temp.Length;
			}

			return file;
		}

		public override int DataLength
		{
			get
			{
				int length = 0;

				int childCount = this.children.Count;

				for (int i = 0; i<childCount; i++)
				{
					length+= children[i].Length;
				}

				return length;
			}
		}

		public override int Length
		{
			get 
			{
				int length = 20;

				if (name!="")
				{
					 //names are padded with a single null if they exist
					length += name.Length + 1;
				}

				length+= DataLength;

				return length;
			}
		}

		public int Save(string path)
		{
			int saves = 0;
			ChunkyData data;

			foreach (ChunkyChunk child in children)
			{
				if (child is ChunkyFolder)
				{
					saves+= ((ChunkyFolder)child).Save(path);
				}
				else if (child is ChunkyData)
				{
					data = (ChunkyData)child;
					if (data.Savable && data.Save(path))
					{
						saves++;
					}
				}
			}

			return saves;
		}

		public override bool Savable
		{
			get
			{
				foreach (ChunkyChunk chunk in children)
				{
					if (chunk.Savable)
					{
						return true;
					}
				}

				return false;
			}
		}

		public override string GetValidationString()
		{
			int childCount = Children.Count;

			if (childCount>0)
			{
				StringBuilder sb = new StringBuilder("FOLD"+ID+"[ ");

				childCount--;

				for (int i = 0; i<childCount; i++)
				{
					sb.Append(Children[i].GetValidationString()+" ");
				}

				sb.Append(Children[childCount].GetValidationString());

				sb.Append(" ]");

				return sb.ToString();
			}
			else
			{
				return "FOLD"+ID+"[ ]";
			}
		}
	}
}
