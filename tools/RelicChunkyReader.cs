// This file is a part of the Relic Tools and is copyright 2006-2018 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU GPL license, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.
using System;
using System.Collections;
using System.IO;
using System.Text;
using IBBoard.Relic.RelicTools.Collections;
using IBBoard.Relic.RelicTools.Exceptions;

namespace IBBoard.Relic.RelicTools
{
	/// <summary>
	/// Summary description for RelicChunkyReader.
	/// </summary>
	public class RelicChunkyReader
	{
		public static RelicChunkyFile ReadChunkyFile(string filepath)
		{
			if (!File.Exists(filepath))
			{
				throw new RelicTools.Exceptions.FileNotFoundException("No 'RelicChunky' file found at "+filepath);
			}

			FileInfo chunkyFile = new FileInfo(filepath);
			BinaryReader br = new BinaryReader(chunkyFile.OpenRead());

			byte[] start = br.ReadBytes(4);
			int version = 0;
			string fileType = "";

			if (start[1] == 0 && start[2] == 0 && start[3] == 0)
			{
				version = start[0];
				fileType = ReadString(br, 4, 8);
			}
			else
			{
				br.BaseStream.Seek(0, SeekOrigin.Begin);
			}

			ChunkyStructureCollection structCol = ReadChunkyFileStructure(br);
			
			br.Close();

			string filename = filepath.Substring(filepath.LastIndexOf(Path.DirectorySeparatorChar)+1);

			if (filepath.EndsWith(".rec"))
			{
				return new RECFile(filename, structCol, version, fileType);
			}
			else if (structCol.Count==1 && structCol[0].RootChunks[0].ID=="TPAT")
			{
				return new WTPFile(filename, structCol);
			}
			else if (structCol.Count==1 && structCol[0].RootChunks[0].ID=="SHRF")
			{
				return new RSHFile(filename, structCol);
			}
			else if (structCol.Count==1 && structCol[0].RootChunks[0].ID=="TXTR")
			{
				return new RTXFile(filename, structCol);
			}
			else
			{
				return new RelicChunkyFile(filename, structCol);
			}
		}

		private static ChunkyStructureCollection ReadChunkyFileStructure(BinaryReader br)
		{			
			string name = "";

			name = ReadString(br, (int)br.BaseStream.Position, 12);

			if (name!="Relic Chunky")
			{
				/*if (name == "Relic Chunky")
				{
					//handle REC files which are chunkys with data at the start
					br.BaseStream.Seek(0, SeekOrigin.Begin);
					//version = br.ReadInt32();
					//fileType = ReadString(br, 4, 8);
					br.BaseStream.Seek(12, SeekOrigin.Current); //skip the name string we just read
					
				}
				else
				{*/
				throw new InvalidFileException("File was not recognised as a valid RelicChunky File");//"File '"+filepath+"' was not recognised as a valid RelicChunky File");
			}
			//}

			return ReadChunkyStructure(br, br.BaseStream.Length);
		}

		private static ChunkyStructureCollection ReadChunkyStructure(BinaryReader br, long fileLength)
		{

			//long end = fileLength - 20;
			ChunkyStructureCollection structCol = new ChunkyStructureCollection();
			int unknown1 = br.ReadInt32();
			int unknown2 = br.ReadInt32();
			int unknown3 = br.ReadInt32();

			byte[] bytes = br.ReadBytes((int)(fileLength-br.BaseStream.Position));

			structCol.Add(new RelicChunkyStructure(RelicChunkReader.ReadChunkyChunks(bytes), unknown1, unknown2, unknown3));
			
			if (RelicChunkReader.HasRemainingBytes())
			{
				byte[] remainingData = RelicChunkReader.GetRemainingBytes();

				if (remainingData[0]==0x52 && remainingData[1]==0x65 && remainingData[2]==0x6C && remainingData[3]==0x69)
				{
					MemoryStream ms = new MemoryStream(remainingData);
					BinaryReader br2 = new BinaryReader(ms);
					ChunkyStructureCollection col = ReadChunkyFileStructure(br2);

					foreach (RelicChunkyStructure strct in col)
					{
						structCol.Add(strct);
					}
				}
				else
				{
					ChunkyCollection col = new ChunkyCollection();
					col.Add(new ChunkyRawData(remainingData));
					structCol.Add(new RelicChunkyStructure(col));
				}
			}

			return structCol;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="start"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		private static string ReadString(BinaryReader br, int start, int length)
		{
			StringBuilder str = new StringBuilder();
			byte tempByte = 0;
			br.BaseStream.Seek(start, SeekOrigin.Begin);

			if (length>0)
			{
				for (int i = 0; i<length; i++)
				{
					tempByte = br.ReadByte();

					if (tempByte>=30)
						str.Append((char)tempByte);
				}
			}
			else
			{
				try
				{
					do 
					{
						tempByte = br.ReadByte();

						if (tempByte>=30)
							str.Append((char)tempByte);

					} while (tempByte!=0);
				}
				catch{}
			}

			return str.ToString();
		}
	}
}