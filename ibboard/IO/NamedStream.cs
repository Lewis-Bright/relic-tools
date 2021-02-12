//  This file (NamedStream.cs) is a part of the IBBoard project and is copyright 2012 IBBoard
//
//  The file and the library/program it is in are licensed and distributed, without warranty, under the GNU LGPL, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.
using System;
using System.IO;

namespace IBBoard.IO
{
	/// <summary>
	/// A wrapper class to allow naming of unnamed streams (e.g. memory streams). All Stream functions
	/// are invoked on the provided stream.
	/// </summary>
	public class NamedStream : Stream
	{
		private Stream stream;

		public NamedStream(string name, Stream stream)
		{
			this.Name = name;
			this.stream = stream;
		}

		public string Name { get; set; }

		#region implemented abstract members of Stream
		public override void Flush()
		{
			stream.Flush();
		}
		public override int Read(byte[] buffer, int offset, int count)
		{
			return stream.Read(buffer, offset, count);
		}
		public override long Seek(long offset, SeekOrigin origin)
		{
			return stream.Seek(offset, origin);
		}
		public override void SetLength(long value)
		{
			stream.SetLength(value);
		}
		public override void Write(byte[] buffer, int offset, int count)
		{
			stream.Write(buffer, offset, count);
		}
		public override bool CanRead
		{
			get
			{
				return stream.CanRead;
			}
		}
		public override bool CanSeek
		{
			get
			{
				return stream.CanSeek;
			}
		}
		public override bool CanWrite
		{
			get
			{
				return stream.CanWrite;
			}
		}
		public override long Length
		{
			get
			{
				return stream.Length;
			}
		}
		public override long Position
		{
			get
			{
				return stream.Position;
			}
			set
			{
				stream.Position = value;
			}
		}
		#endregion
	}
}

