//  This file (FileStreamstream.cs) is a part of the IBBoard project and is copyright 2009 IBBoard
// 
//  The file and the library/program it is in are licensed under the GNU LGPL license, either version 3 of the License or (at your option) any later version. Please see COPYING.LGPL for more information and the full license.
// 

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace IBBoard
{
	/// <summary>
	/// A custom replacement for the BinaryReader that reads numbers as big-endian and also provides methods to read byte delimited strings.
	/// </summary>
	public class BinaryReaderBigEndian
	{
		private Stream stream;
		private Encoding encoding;
		
		public BinaryReaderBigEndian(Stream input) : this(input, Encoding.UTF8)
		{
		}
		
		public BinaryReaderBigEndian(Stream input, Encoding encoding)
		{
			stream = input;
			this.encoding = encoding;
		}

		public byte[] ReadBytes(int byteCount)
		{
			byte[] bytes = new byte[byteCount];
			stream.Read(bytes, 0, byteCount);
			return bytes;
		}

		private byte[] ReadBytesForNumber(int byteCount)
		{
			byte[] bytes = ReadBytes(byteCount);
			int halfCount = byteCount / 2;

			for (int i = 0, j = byteCount - 1; i < halfCount; i++, j--)
			{
				byte temp = bytes[i];
				bytes[i] = bytes[j];
				bytes[j] = temp;
			}

			return bytes;
		}

		public byte ReadByte()
		{
			return (byte)stream.ReadByte();
		}

		public short ReadShort()
		{
			return BitConverter.ToInt16(ReadBytesForNumber(2), 0);
		}

		public ushort ReadUShort()
		{
			return BitConverter.ToUInt16(ReadBytesForNumber(2), 0);
		}

		public int ReadInt()
		{
			return BitConverter.ToInt32(ReadBytesForNumber(4), 0);
		}

		public uint ReadUInt()
		{
			return BitConverter.ToUInt32(ReadBytesForNumber(4), 0);
		}

		public string ReadDelimitedString(byte delimiter)
		{
			List<byte> bytes = new List<byte>();

			while (!EndOfStream)
			{
				byte b = ReadByte();

				if (b!=delimiter)
				{
					bytes.Add(b);
				}
				else
				{
					break;
				}
			}

			return encoding.GetString(bytes.ToArray());
		}

		public string ReadString(int length)
		{
			byte[] bytes = ReadBytes(length);
			return encoding.GetString(bytes);
		}

		public string ReadUShortLengthString()
		{
			ushort length = ReadUShort();
			return ReadString(length);
		}
		
		public string ReadIntLengthString()
		{
			int length = ReadInt();
			return ReadString(length);
		}

		public void Seek(int distance)
		{
			stream.Seek(distance, SeekOrigin.Begin);
		}

		public void Move(int distance)
		{
			stream.Seek(distance, SeekOrigin.Current);
		}

		public void Close()
		{
			stream.Close();
		}

		public bool EndOfStream
		{
			get { return stream.Length - 1 == stream.Position; }
		}

		public long Position
		{
			get { return stream.Position; }
		}
	}
}
