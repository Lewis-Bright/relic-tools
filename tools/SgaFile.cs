// This file is a part of the Relic Tools and is copyright 2006-2018 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU GPL license, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.
using System;
using System.Collections;
using System.IO;
using System.Text;
using ICSharpCode.SharpZipLib.GZip;
using IBBoard.Relic.RelicTools.Exceptions;

namespace IBBoard.Relic.RelicTools
{
	/// <summary>
	/// Summary description for SgaFile.
	/// </summary>
	public class SgaFile
	{
		public enum FileFormat {Unknown, LUA, WTP, RSH, RTX, WHM, WHE, RGD, EVENTS, AI, SCAR, NIL, TGA, DDS, SCREEN, NIS, RAT, SGB, CAMP, TURN, FDA, CON, TEAMCOLOUR, JPG, JPEG, BMP, SGM, STYLES, COLOURS, FNT, TTF, TXT}
		public enum CompressionType{None, ZLibLarge, ZLibSmall, ZLib, UnknownCompression}

		private Hashtable attrib = null;
		private long id;
		private SgaFolder parent = null;
		private string name;
		private string extension;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="idNum"></param>
		/// <param name="nameIn"></param>
		/// <param name="attr"></param>
		public SgaFile(long idNum, string nameIn, Hashtable attr)
		{
			attrib = attr;
			id = idNum;
			parent = null;
			name = nameIn;
			extension = name.Substring(name.LastIndexOf('.')+1).ToLower();
		}

		/// <summary>
		/// 
		/// </summary>
		public SgaFolder Parent
		{
			get{ return parent; }
			set
			{
				parent = value;  
				parent.Files.Add(this);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public long ID 
		{
			get{ return id; }
		}

		public string Path
		{
			get
			{
				return parent.Path+this.Name;
			}
		}

		public long Size
		{
			get	{ return (long)attrib["DataLength"];}
		}

		public long SizeUncompressed
		{
			get{ return (long)attrib["DataLengthCompressed"];}
		}

		public string Type
		{
			get{return extension;}
		}

		public string TypeDesc
		{
			get{return FileFormats.FormatAsString(extension);}
		}

		public FileFormat Format
		{
			get
			{
				try
				{
					return (FileFormat)Enum.Parse(typeof(FileFormat), this.Extension, true);
				}
				catch
				{
					return FileFormat.Unknown;
				}
			}
		}

		public CompressionType Compression
		{
			get
			{
				switch((long)attrib["Compression"])
				{
					case 0x00: return CompressionType.None;
					case 0x10: return CompressionType.ZLibLarge;
					case 0x20: return CompressionType.ZLibSmall;
					case 0x30: return CompressionType.ZLib;
					default: return CompressionType.UnknownCompression; //HACK: temporarily stop the exception
						//throw new InvalidFileException("Invalid compression value found in SGA file");
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public string Name
		{
			get{ return name;}
		}

		public string Extension
		{
			get { return extension; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="destination"></param>
		public bool Save(string destination)
		{
			return Save(destination, false);
		}

		public bool Save(string destination, bool overwrite)
		{
			return Save(destination, "", "", overwrite);
		}

		public bool Save(string destination, string find, string replace)
		{
			return Save(destination, find, replace, false);
		}

		public bool Save(string destination, string[] find, string[] replace)
		{
			return Save(destination, find, replace, false);
		}

		public bool Save(string destination, string find, string replace, bool overwrite)
		{
			return Save(destination, new string[]{find}, new string[]{replace},overwrite);
		}

		public bool Save(string destination, string[] find, string[] replace, bool overwrite)
		{
			string outputFileName = "";

			//if there is a dot before the last slash (or no dot, which returns -1)
			//then we've been passed a folder to extract to, so the output file name is the current file name			
			int lastSlash = destination.LastIndexOf(System.IO.Path.DirectorySeparatorChar);
			
			if (destination.LastIndexOf('.') <= lastSlash)
			{
				outputFileName = name;
			}
			else
			{
				//if we can't find one of the slashes that Windows uses then look
				//for a real folder slash
				if (lastSlash==-1)
				{
					lastSlash = destination.LastIndexOf('/');
				}

				//if we didn't find one of those slashes either, then all we were given was a file name and no folder
				if (lastSlash==-1)
				{
					outputFileName = destination;
					destination = Environment.CurrentDirectory;
				}
				else
				{					
					outputFileName = destination.Substring(lastSlash+1);
					destination = destination.Substring(0, lastSlash);
				}
				//otherwise we've been passed a file name to save to
			}

			DirectoryInfo dir = null;

			if (!Directory.Exists(destination))
			{
				dir = Directory.CreateDirectory(destination);
			}
			else
			{
				dir = new DirectoryInfo(destination);

				if (File.Exists(dir.FullName+System.IO.Path.DirectorySeparatorChar+outputFileName) && !overwrite)
				{
					//throw new FileExistsException(dir.FullName+Path.DirectorySeparatorChar+outputFileName);
					//change the behaviour if a file exists so that it just fails and returns false instead of
					//exceptioning and stopping further execution
					this.Parent.ParentArchive.ExtractFileFail(this, "file already exists");
					return false;
				}
			}

			FileInfo file = new FileInfo(dir.FullName+System.IO.Path.DirectorySeparatorChar+outputFileName);
			long dataOffset = (long)parent.ParentArchive.Attributes["DataOffest"];
			long fileDataOffset = (long)attrib["DataOffset"];
			long dataLength = (long)attrib["DataLength"];
			long dataLengthCompressed = (long)attrib["DataLengthCompressed"];
			byte[] bytes = null;
			
			if (dataLengthCompressed!=dataLength)
			{
				bytes = parent.ParentArchive.ArchiveReader.ReadFileDataZipped(dataOffset, fileDataOffset, dataLengthCompressed);
			}
			else
			{
				bytes = parent.ParentArchive.ArchiveReader.ReadFileData(dataOffset, fileDataOffset, dataLength);
			}

			if (find!=null && replace!=null)
			{
				for (int i = 0; i<find.Length; i++)
				{
					if (replace[i]!=null && find[i]!=null)
					{
						if (find[i]!="" && replace[i]!="" && find[i].Length==replace[i].Length)
						{
							//string tempString = Encoding.UTF8.GetString(bytes);
							//tempString = tempString.Replace(find, replace);
							//bytes = Encoding.UTF8.GetBytes(tempString);
							bytes = Replace(bytes, find[i], replace[i]);
						}
						else if (find[i].Length!=replace[i].Length)
						{
//TODO: throw error here
						}
					}
				}
			}

			BinaryWriter bw = new BinaryWriter(file.OpenWrite());
			bw.Write(bytes);
			bw.Flush();
			bw.Close();		

			this.Parent.ParentArchive.ExtractFileSuccess(this);

			return true;
		}

		private byte[] Replace(byte[] data, string find, string replace)
		{
			if (replace!=null && find!=null && find!=replace)
			{
				byte[] findByte = new byte[find.Length];
				for (int i = 0; i<find.Length; i++)
				{
					findByte[i] = Convert.ToByte(find[i]);
				}

				byte[] replaceByte = new byte[replace.Length];
				for (int i = 0; i<replace.Length; i++)
				{
					replaceByte[i] = Convert.ToByte(replace[i]);
				}


				if (find!="" && replace!="" && find.Length==replace.Length)
				{
					int lastPos = data.Length - find.Length;
					int matchPos = -1;
					bool match = false;

					for (int i =0; i<data.Length; i++)
					{
						if (data[i] == findByte[0])
						{
							matchPos = i;
							match = true;

							int j = 1;

							for (j = 1; j < findByte.Length; j++)
							{
								if (data[i+j]==findByte[j])
								{
									continue;
								}
								else if (data[i+j]==findByte[0])
								{
									//if the data matches the start of the Find again
									//then reduce J by one to make sure that the next pass
									//doesn't skip over it
									j--;
									match = false;
									break;
								}
								else
								{
									match = false;
									break;
								}
							}

							if (match)
							{
								for (int k = 0; k < replaceByte.Length; k++)
								{
									data[matchPos+k] = replaceByte[k];
								}
							}

							i+= j;
						}
					}
				}
				else if (find.Length!=replace.Length)
				{
//TODO: throw error here
				}
			}

			return data;
		}
	}
}
