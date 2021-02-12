// This file is a part of the Relic Tools and is copyright 2006-2018 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU GPL license, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.
using System;
using System.Text;
using System.IO;

namespace IBBoard.Relic.RelicTools
{
	public enum ChunkyChunkType {Data, Folder, RawData}
	/// <summary>
	/// Summary description for ChunkyChunk.
	/// </summary>
	public abstract class ChunkyChunk
	{
		protected string id;
		protected int version;
		protected string name;
		//protected int dataLength;
		protected ChunkyChunkType type;
		protected ChunkyFolder parent;
		private RelicChunkyFile parentFile;

		public ChunkyChunk(ChunkyChunkType type_in, string ID_in, int version_in, string name_in)
		{
			type = type_in;
			id = ID_in;
			version = version_in;
			name = name_in;
			parentFile = null;
		}

		public ChunkyFolder Parent
		{
			get{return parent;}
			set{parent = value;}
		}

		public RelicChunkyFile ParentFile
		{
			get
			{
				if (parentFile!=null)
				{
					return parentFile;
				}
				else if (parent!=null)
				{
					return parent.ParentFile;
				}
				else
				{
					return null;
				}
			}
			set
			{
				if (parent==null)
				{
					parentFile = value;
				}
				else
				{
					throw new InvalidOperationException("Unable to set parent file - parent file must only be set on root nodes.");
				}
			}
		}

		public virtual int Length
		{
			get {
				return 20 + NameDataLength + DataLength;
			}
		}

		public int NameLength
		{
			get
			{
				return name.Length;
			}
		}

		public int NameDataLength
		{
			get
			{
				if (name=="")
				{
					return 0;
				}
				else
				{
					return NameLength+1; //names are padded with a single null if they exist
				}
			}
		}

		public abstract int DataLength{get;}

		public string ID
		{
			get{return id;}
		}

		public string Name
		{
			get{return name;}
			set
			{
				if (name!=null)
				{
					name = value;
				}
				else
				{
					name = ""; 
				}
			}
		}

		public abstract string GetValidationString();

		public virtual string GetDisplayDetails()
		{
			return "Type:\t\t"+ type.ToString() + Environment.NewLine +
					"ID:\t\t"+ id + Environment.NewLine +
					"Version:\t"+version+Environment.NewLine +
					"Data Size:\t"+DataLength+Environment.NewLine +
					"Chunk Size:\t"+Length+Environment.NewLine +
					"Name Length:\t"+name.Length+Environment.NewLine +
					"Name:\t\t"+name;
		}

		public static string ByteArrayToString(byte[] arr)
		{
			StringBuilder sb = new StringBuilder(arr.Length);

			for (int i = 0; i<arr.Length && i<512; i++)
			{
				if ((i%4)==0 && i!=0)
				{
					sb.Append(" ");
				}

				sb.Append(String.Format("{0:x2}", arr[i])+" ");
			}

			if (arr.Length>511)
			{
				sb.Append(Environment.NewLine+"...Truncated...");
			}

			return sb.ToString();
		}

		public static string ByteArrayToString(byte[] arr, int start)
		{

			return ByteArrayToString(arr, start, (arr.Length-start));
		}

		public static string ByteArrayToString(byte[] arr, int start, int length)
		{
			byte[] temp = new byte[length];

			for (int i = 0; i<length; i++)
			{
				temp[i] = arr[start+i];
			}

			return ByteArrayToString(temp);
		}

		public static float ByteArrayToSingle(byte[] arr)
		{
			return BitConverter.ToSingle(arr, 0);;
		}

		public static float ByteArrayToSingle(byte[] arr, int start)
		{
			return BitConverter.ToSingle(arr, start);
		}

		public static string ByteArrayToTextString(byte[] arr)
		{
			StringBuilder sb = new StringBuilder(arr.Length);

			for (int i = 0; i<arr.Length; i++)
			{
				sb.Append((char)arr[i]);
			}

			return sb.ToString();
		}

		public static string ByteArrayToTextString(byte[] arr, int start)
		{

			return ByteArrayToTextString(arr, start, (arr.Length-start));
		}

		public static string ByteArrayToTextString(byte[] arr, int start, int length)
		{
			byte[] temp = new byte[length];

			for (int i = 0; i<length; i++)
			{
				temp[i] = arr[start+i];
			}

			return ByteArrayToTextString(temp);
		}

		protected byte[] GetNameBytes()
		{
			if (name=="")
			{
				return new byte[0];
			}
			else
			{
				byte[] bytes = new byte[name.Length+1];
				System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
				enc.GetBytes(name).CopyTo(bytes,0);
				return bytes;
			}
		}

		public bool Export(String destination)
		{
			FileStream str = new FileStream(destination, FileMode.Create);
			BinaryWriter bw = new BinaryWriter(str);
			bw.Write(this.GetBytes());
			bw.Flush();
			bw.Close();
			return true;
		}

		public abstract byte [] GetBytes();
		public abstract bool Savable { get; }
	}
}
