//  This file (StreamUtil.cs) is a part of the IBBoard project and is copyright 2011 IBBoard
// 
//  The file and the library/program it is in are licensed and distributed, without warranty, under the GNU LGPL, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.
using System;
using System.IO;

namespace IBBoard.IO
{
	public class StreamUtil
	{
		public static void CopyStream(Stream fromStream, Stream toStream)
		{
			byte[] bytes = new byte[8096];
		    int read;
		    while ((read = fromStream.Read(bytes, 0, bytes.Length)) > 0)
			{
		        toStream.Write(bytes, 0, read);
			}
		}
		
		public static byte[] ToBytes(Stream fromStream)
		{
			MemoryStream toStream = new MemoryStream();
			CopyStream(fromStream, toStream);
			return toStream.ToArray();
		}
	}
}
