// This file is a part of the Relic Tools and is copyright 2006-2018 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU GPL license, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.
using System;
using System.IO;
using System.Text;
using IBBoard.Relic.RelicTools;
using IBBoard.Relic.RelicTools.Collections;

namespace IBBoard.Relic.RelicTools
{
	/// <summary>
	/// Summary description for RelicChunkyStructure.
	/// </summary>
	public class RelicChunkyStructure
	{
		protected ChunkyCollection chunkCol;
		int unknownInt1;
		int unknownInt2;
		int unknownInt3;
		
		public static readonly byte[] ChunkyHeader = new byte[]{0x52, 0x65, 0x6C, 0x69, 0x63, 0x20, 0x43, 0x68, 0x75, 0x6E, 0x6B, 0x79, 0x0D, 0x0A, 0x1A, 0x00, 0x01, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00};

		public RelicChunkyStructure(ChunkyFolder chunkyRoot):this(chunkyRoot, 0, 0, 0){}

		public RelicChunkyStructure(ChunkyFolder chunkyRoot, int unknown1, int unknown2, int unknown3)
		{
			chunkCol = new ChunkyCollection();
			chunkCol.Add(chunkyRoot);
			unknownInt1 = unknown1;
			unknownInt2 = unknown2;
			unknownInt3 = unknown3;
		}

		public RelicChunkyStructure(ChunkyCollection col):this(col, 0, 0, 0){}

		public RelicChunkyStructure(ChunkyCollection col, int unknown1, int unknown2, int unknown3)
		{
			chunkCol = col;
			unknownInt1 = unknown1;
			unknownInt2 = unknown2;
			unknownInt3 = unknown3;
		}

		public RelicChunkyFile ParentFile
		{
			get{ return (chunkCol.Count>0)?chunkCol[0].ParentFile:null;  }
			set{
				for (int i = 0; i<chunkCol.Count; i++)
				{
					chunkCol[i].ParentFile = value;
				}
			}
		}

		public ChunkyCollection RootChunks
		{
			get{ return chunkCol; }
		}

		public void Save(BinaryWriter bw)
		{			
			bw.Write(ChunkyHeader);

			foreach(ChunkyChunk chunk in RootChunks)
			{
				bw.Write(chunk.GetBytes());
			}
		}
		
		public string GetValidationString()
		{
			int childCount = this.RootChunks.Count;

			if (childCount>1)
			{
				StringBuilder sb = new StringBuilder();
				childCount--;

				for (int i = 0; i<childCount; i++)
				{
					sb.Append(RootChunks[i].GetValidationString()+" ");
				}

				sb.Append(RootChunks[childCount].GetValidationString());

				return sb.ToString();
			}
			else
			{
				return RootChunks[0].GetValidationString();
			}
		}
	}
}
