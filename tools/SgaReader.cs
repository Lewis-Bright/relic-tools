// This file is a part of the Relic Tools and is copyright 2006-2018 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU GPL license, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.
using System;
using System.Collections;
using System.IO;
using System.Text;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;
using IBBoard.Relic.RelicTools.Exceptions;

/*
 * struct _SgaHeader
	{
		char* sIdentifier; // 8 bytes
		long iVersion;
		long* iToolMD5; // 16 bytes
		wchar_t* sArchiveType; // unicode (128 bytes - 64 * wchar)
		long* iMD5; // MD5
		long iDataHeaderSize;
		long iDataOffset;

		long iToCOffset;
		short int iToCCount;
		long iDirOffset;
		short int iDirCount;
		long iFileOffset;
		short int iFileCount;
		long iItemOffset;
		short int iItemCount;
	};

	struct _SgaToC
	{
		char* sAlias; // 64 bytes
		char* sBaseDirName; // 64 bytes
		short int iStartDir;
		short int iEndDir;
		short int iStartFile;
		short int iEndFile;
		long iFolderOffset;
	};

	struct _SgaDirInfo
	{
		long iNameOffset;
		short int iSubDirBegin;
		short int iSubDirEnd;
		short int iFileBegin;
		short int iFileEnd;
	};

	struct _SgaFileInfo
	{
		long iNameOffset;
		long iFlags; // 0x00 = uncompressed, 0x10 = zlib large file, 0x20 = zlib small file (< 4kb)
		long iDataOffset;
		long iDataLengthCompressed; //actually uncompressed size
		long iDataLength;//actually compressed size
	};
	*/
namespace IBBoard.Relic.RelicTools
{
	/// <summary>
	/// Summary description for SgaReader.
	/// </summary>
	public class SgaReader
	{
		private BinaryReader br = null;
		private string path = "";
		private FileInfo sga = null;
		
		public static readonly int FolderInfoLength = 12;
		public static int FileInfoLength
		{
			get{ return fileInfoLength; }
		}
		public static int BaseOffset
		{
			get{ return baseOffset; }
		}

		private static int baseOffset;
		private static int fileInfoLength;
		private static SgaArchive archive;


		/// <summary>
		/// 
		/// </summary>
		/// <param name="pathIn"></param>
		public SgaReader(string pathIn)
		{
			path = pathIn;

			if (!File.Exists(path))
			{
				throw new RelicTools.Exceptions.FileNotFoundException();
			}

			sga = new FileInfo(path);
		}

		public SgaArchive Archive
		{
			get{ return archive; }
			set{ archive = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public Hashtable ReadHeaders()
		{
			Hashtable ht = new Hashtable();

			try
			{
				
				FileStream input = sga.OpenRead();
				br = new BinaryReader(input);

				int pos = 0;

				ht["AsciiIdent"] = ReadString(0, 8);
				pos+=8;
				ht["Version"] = ReadLong(pos,4);
				pos+=4;
				ht["ToolMD5"] = ReadString(pos,16);
				pos+=16;
				ht["UnicodeString"] = ReadString(pos,128);
				pos+=128;
				ht["MD5"] = ReadString(pos,16);
				pos+=16;
				ht["DataHeaderSize"] = ReadLong(pos,4);
				pos+=4;
				ht["DataOffest"] =  ReadLong(pos,4);
				pos+=4;
				ht["TocOffset"] = ReadLong(pos,4);
				pos+=4;
				ht["TocCount"] = ReadLong(pos,2);
				pos+=2;

				if ((long)ht["Version"]==4)
				{
					ht["Unknown"] = ReadLong(pos, 4);
					pos+=4;
					baseOffset = 184;
					fileInfoLength = 22;
				}
				else
				{
					baseOffset = 180;
					fileInfoLength = 20;
				}

				ht["DirOffset"] = ReadLong(pos,4);
				pos+=4;
				ht["DirCount"] = ReadLong(pos,2);
				pos+=2;
				ht["FileOffset"] = ReadLong(pos,4);
				pos+=4;
				ht["FileCount"] = ReadLong(pos,2);
				pos+=2;
				ht["ItemOffset"] = ReadLong(pos,4);
				pos+=4;
				ht["ItemCount"] = ReadLong(pos,2);
				pos+=2;
				ht["TocAlias"] = ReadString(pos,64);
				pos+=64;
				ht["TocStartName"] = ReadString(pos,64);
				pos+=64;
				ht["TocStartDir"] = ReadLong(pos,2);
				pos+=2;
				ht["TocEndDir"] = ReadLong(pos,2);
				pos+=2;
				ht["TocStartFile"] = ReadLong(pos,2);
				pos+=2;
				ht["TocEndFile"] = ReadLong(pos,2);
				pos+=2;
				ht["TocFolderOffset"] = ReadLong(pos,2);
			}
			finally
			{
				if (br!=null)
				{
					br.Close();
				}
			}
			return ht;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dirOffset"></param>
		/// <param name="itemOffset"></param>
		/// <returns></returns>
		public SgaFolder ReadFolders(long dirOffset, long itemOffset, string rootName)
		{
			SgaFolder root = null;
			try
			{
				FileStream input = sga.OpenRead();
				br = new BinaryReader(input);
				Hashtable ht = new Hashtable();

				ht["FolderNameOffset"] = ReadLong(dirOffset+BaseOffset, 4);
				ht["SubDirIDBegin"] = ReadLong(dirOffset+BaseOffset+4, 2);
				ht["SubDirIDEnd"] = ReadLong(dirOffset+BaseOffset+6, 2);
				ht["FileIDBegin"] = ReadLong(dirOffset+BaseOffset+8, 2);
				ht["FileIDEnd"] = ReadLong(dirOffset+BaseOffset+10, 2);

				root = new SgaFolder(0, rootName, ht);
				long startID = (long)ht["SubDirIDBegin"];
				long endID = (long)ht["SubDirIDEnd"];

				if (startID<endID)
				{
					SgaFolder tempFolder = null;
					for (long i = startID; i < endID; i++)
					{
						tempFolder = ReadFolder(dirOffset, itemOffset, i);
						tempFolder.Parent = root;
					}
				}
			}
			finally
			{
				br.Close();
			}

			return root;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dirOffset"></param>
		/// <param name="itemOffset"></param>
		/// <param name="folderID"></param>
		/// <returns></returns>
		public SgaFolder ReadFolder(long dirOffset, long itemOffset, long folderID)
		{
			SgaFolder folder = null;

			Hashtable ht = new Hashtable();

			ht["FolderNameOffset"] = ReadLong(dirOffset+BaseOffset+(FolderInfoLength*folderID), 4);
			ht["SubDirIDBegin"] = ReadLong(dirOffset+BaseOffset+4+(FolderInfoLength*folderID), 2);
			ht["SubDirIDEnd"] = ReadLong(dirOffset+BaseOffset+6+(FolderInfoLength*folderID), 2);
			ht["FileIDBegin"] = ReadLong(dirOffset+BaseOffset+8+(FolderInfoLength*folderID), 2);
			ht["FileIDEnd"] = ReadLong(dirOffset+BaseOffset+10+(FolderInfoLength*folderID), 2);

			string tmpName = ReadString(itemOffset+BaseOffset+(long)ht["FolderNameOffset"], -1);
			tmpName = tmpName.Substring(tmpName.LastIndexOf('\\')+1);

			folder = new SgaFolder(folderID, tmpName, ht);
			long startID = (long)ht["SubDirIDBegin"];
			long endID = (long)ht["SubDirIDEnd"];

			if (startID<endID)
			{
				SgaFolder tempFolder = null;
				for (long i = startID; i < endID; i++)
				{
					tempFolder = ReadFolder(dirOffset, itemOffset, i);
					tempFolder.Parent = folder;
				}
			}

			return folder;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fileOffset"></param>
		/// <param name="itemOffset"></param>
		/// <param name="id"></param>
		/// <returns></returns>
		public SgaFile ReadFile(long fileOffset, long itemOffset, long id)
		{
			Hashtable ht = new Hashtable();
			SgaFile file = null;
			try
			{
				FileStream input = sga.OpenRead();
				br = new BinaryReader(input);
				long begin = fileOffset+BaseOffset+(id*FileInfoLength);
				int pos = 0;

				ht["FileNameOffset"] = ReadLong(begin,4);
				pos+=4;

				if (archive.Version==2)
				{
					ht["Compression"] = ReadLong(begin+pos,4);
					pos+=4;
				}
				else
				{
					ht["Compression"] = 0L;
				}

				ht["DataOffset"] = ReadLong(begin+pos,4);
				pos+=4;
				ht["DataLength"] = ReadLong(begin+pos,4);
				pos+=4;
				ht["DataLengthCompressed"] = ReadLong(begin+pos,4);
				pos+=4;

				if (archive.Version==4)
				{
					ht["CRC"] = ReadLong(begin+pos,4);
					pos+=4;
					ht["Unknown"] = ReadLong(begin+pos, 2);
					ht["Compression"] = ((long)ht["DataLength"] == (long)ht["DataLengthCompressed"] ? 0L : 0x30L);
				}
				else
				{
					ht["CRC"] = 0L;
					ht["Unknown"] = 0L;
				}

				string tmpName = ReadString(BaseOffset+itemOffset+(long)ht["FileNameOffset"], -1);
				file = new SgaFile(id, tmpName, ht);
			}
			finally
			{
				br.Close();
			}
			return file;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="start"></param>
		/// <param name="bytes"></param>
		/// <returns></returns>
		private string ReadString(long start, long bytes)
		{
			return ReadString(start, bytes, false);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="start"></param>
		/// <param name="bytes"></param>
		/// <param name="unicode"></param>
		/// <returns></returns>
		private string ReadString(long start, long bytes, bool unicode)
		{
			StringBuilder str = new StringBuilder();
			byte tempByte = 0;
			br.BaseStream.Seek(start, SeekOrigin.Begin);

			if (bytes>0)
			{
				for (long i = 0; i<bytes; i++)
				{
					tempByte = br.ReadByte();

					if (tempByte>=30)
						str.Append((char)tempByte);
				
					if (unicode)
						i++;
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

		/// <summary>
		/// 
		/// </summary>
		/// <param name="start"></param>
		/// <param name="bytes"></param>
		/// <returns></returns>
		private long ReadLong(long start, long bytes)
		{

			long tempLong = 0;
			byte tempByte = 0;			
			br.BaseStream.Seek(start, SeekOrigin.Begin);

			for (long i = 0; i<bytes; i++)
			{
				tempByte = br.ReadByte();

				tempLong = tempLong+(long)(tempByte*Math.Pow(256,i));
			}

			return tempLong;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dataOffset"></param>
		/// <param name="fileDataOffset"></param>
		/// <param name="bytes"></param>
		/// <returns></returns>
		public byte[] ReadFileData(long dataOffset, long fileDataOffset, long bytes)
		{
			try
			{
				FileStream input = sga.OpenRead();
				br = new BinaryReader(input);
				br.BaseStream.Seek(dataOffset+fileDataOffset, SeekOrigin.Begin);

				if (bytes<=int.MaxValue)
				{
					return br.ReadBytes((int)bytes);
				}
				else
				{
					throw new RelicTools.Exceptions.Exception("File larger than 2GB!");
				}
			}
			finally
			{
				br.Close();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="dataOffset"></param>
		/// <param name="fileDataOffset"></param>
		/// <param name="bytes"></param>
		/// <returns></returns>
		public byte[] ReadFileDataZipped(long dataOffset, long fileDataOffset, long bytes)
		{
			InflaterInputStream zipped = null;
			try
			{
				byte [] compressed = ReadFileData(dataOffset, fileDataOffset, bytes);
				MemoryStream ms = new MemoryStream(compressed, 0, compressed.Length);
				ms.Seek(0, SeekOrigin.Begin);
				zipped = new InflaterInputStream(ms);
				
				if (bytes<=int.MaxValue)
				{
					byte[] byteArr = new byte[bytes];
					int size = 0;
					int offset = 0;

					try
					{
						do
						{
							size = zipped.Read(byteArr, offset, byteArr.Length);
							offset+= size;
						} while (size > 0);
					}
					catch(RelicTools.Exceptions.Exception ex)
					{
						throw new FileNotZippedException(ex);
					}

					return byteArr;
				}
				else
				{
					throw new RelicTools.Exceptions.Exception("File larger than 2GB!");
				}
			}
			finally
			{
				zipped.Close();
			}
		}
	}
}
