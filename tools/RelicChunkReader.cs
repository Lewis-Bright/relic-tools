// This file is a part of the Relic Tools and is copyright 2006-2018 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU GPL license, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.
using System;
using System.IO;
using IBBoard.Relic.RelicTools.Collections;
using IBBoard.Relic.RelicTools.Exceptions;

namespace IBBoard.Relic.RelicTools
{
	/// <summary>
	/// Summary description for RelicChunkReader.
	/// </summary>
	public class RelicChunkReader
	{
		static byte[] remaining;

		public static bool HasRemainingBytes()
		{
			return (remaining!=null && remaining.Length>20);
		}

		public static byte[] GetRemainingBytes()
		{
			return remaining;
		}

		public static ChunkyCollection ReadChunkyChunks(byte[] chunkBytes)
		{
			MemoryStream ms = new MemoryStream(chunkBytes, false);
			RelicBinaryReader br = new RelicBinaryReader(ms);

			remaining = new byte[0];

			int pos = 0;

			int fileLength = chunkBytes.Length;
			ChunkyCollection col = new ChunkyCollection();

			while (pos<fileLength && (fileLength-pos)>20)
				//check that there's a reasonable amount remaining so that the app doesn't choke on the extra bytes
				//added to the end of files like Relic's Chunky Viewer does on SpookyRAT extracted files
			{
				string type = br.ReadString(4);

				if (type == "FOLD" || type=="DATA")
				{
					string id = br.ReadString(4);
					int version = br.ReadInt32();
					int dataLength = br.ReadInt32();
					int nameLength = br.ReadInt32();
					string name = br.ReadString(nameLength);
					byte[] innerData = br.ReadBytes(dataLength);

					if (type == "FOLD")
					{
						col.Add(new ChunkyFolder(id, version, name, innerData));
					}
					else
					{
						col.Add(CreateChunkyChunk(id, "", version, name, innerData));
					}

					pos+= dataLength+nameLength+20;
				}
				else if (type == "Reli")
				{
					br.BaseStream.Seek(-4, SeekOrigin.Current);
					remaining = br.ReadBytes((int)(br.BaseStream.Length - br.BaseStream.Position));
					//we've hit another chunk, so stop the reading
					break;
				}
				else if (type=="")
				{
					//HACK: drop out with REC files rather than exceptioning, as the trailing info doesn't seem to be Chunkified
					remaining = br.ReadBytes((int)(br.BaseStream.Length - br.BaseStream.Position));
					break;
				}
				else
				{
					throw new InvalidChunkException("Chunk was not of type FOLD or DATA");
				}
			}

			return col;
		}

		public static ChunkyChunk ReadChunkyChunk(byte[] chunkBytes)
		{
			return ReadChunkyChunk(chunkBytes, "");
		}

		public static ChunkyChunk ReadChunkyChunk(byte[] chunkBytes, string parentID)
		{
			MemoryStream ms = new MemoryStream(chunkBytes, false);
			RelicBinaryReader br = new RelicBinaryReader(ms);

			string type = br.ReadString(0,4);
			string id = br.ReadString(4);
			int version = br.ReadInt32();
			int dataLength = br.ReadInt32();
			int nameLength = br.ReadInt32();
			string name = br.ReadString(nameLength);
			byte[] innerData = br.ReadBytes(dataLength);

			if (type=="FOLD")
			{
				return new ChunkyFolder(id, version, name, innerData);
			}
			else if (type=="DATA")
			{
				return CreateChunkyChunk(id, parentID, version, name, innerData);
			}
			else
			{
				throw new InvalidChunkException("Chunk was not of type FOLD or DATA");
			}
		}

		private static ChunkyData CreateChunkyChunk(string id, string parentID, int version, string name, byte[] innerData)
		{
			try
			{
				switch(id)
				{
					case "PTLD":
						return new ChunkyDataPTLD(version, name, innerData);
					case "DATA":
						if (parentID=="IMAG")
						{
							return new ChunkyDataDATAIMAG(version, name, innerData);
						}
						else
						{
							return new ChunkyDataDATA(version, name, innerData);
						}
					case "PTBN":
						return new ChunkyDataPTBN(version, name, innerData);
					case "PTBD":
						return new ChunkyDataPTBD(version, name, innerData);
					case "INFO":
						if (parentID == "")
						{
							return new ChunkyDataINFOGeneric(version, name, innerData);
						}
						else if (parentID == "SHDR")
						{
							return new ChunkyDataINFOSHDR(version, name, innerData);
						}
						else if (parentID == "TPAT")
						{						
							return new ChunkyDataINFOTPAT(version, name, innerData);
						}
						else if (parentID == "TXTR")
						{
							return new ChunkyDataINFOTXTR(version, name, innerData);
						}
						else
						{
							return new ChunkyDataINFOGeneric(version, name, innerData);
						}
					case "ATTR":
						return new ChunkyDataATTR(version, name, innerData);
					case "CHAN":
						return new ChunkyDataCHAN(version, name, innerData);
					case "HEAD":
						return new ChunkyDataHEAD(version, name, innerData);
					case "FBIF":
						return new ChunkyDataFBIF(version, name, innerData);
					case "SSHR":
						return new ChunkyDataSSHR(version, name, innerData);
					default:
						return new ChunkyDataUnknown(id, version, name, innerData);
				}
			}
			catch (IndexOutOfRangeException)
			{
				throw new InvalidChunkException("Data chunk of type "+id+" contained less data than expected");
			}
		}
	}
}
