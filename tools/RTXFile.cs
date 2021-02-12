// This file is a part of the Relic Tools and is copyright 2006-2018 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU GPL license, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.
using System;
using System.IO;
using IBBoard.Relic.RelicTools.Exceptions;
using IBBoard.Relic.RelicTools.Collections;


namespace IBBoard.Relic.RelicTools
{
	/// <summary>
	/// Summary description for WTPFile.
	/// </summary>
	public class RTXFile:RelicChunkyFile
	{
		int width = 0;
		int height = 0;
		int id = 0;

		public RTXFile(string filename, ChunkyFolder folder)
			:this(filename, new ChunkyStructureCollection(new RelicChunkyStructure(folder))){}
		
		public RTXFile(string filename, ChunkyCollection col)
			:this(filename, new ChunkyStructureCollection(new RelicChunkyStructure(col))){}

		public RTXFile(string filename, ChunkyStructureCollection col): base(filename, col)
		{	
			int lastIndex = filename.LastIndexOf('_');
			
			if (lastIndex == -1 ||  filename.IndexOf('_')==lastIndex)
			{
				throw new InvalidFileException("RTX file name specified must end with a skin name and number e.g. _default_0.rtx");
			}

			try
			{
				int dot = filename.LastIndexOf('.');
				lastIndex++;
				id = int.Parse(filename.Substring(lastIndex, dot - lastIndex));
			}
			catch(FormatException)
			{
				throw new InvalidFileException("RTX file name specified must end with a skin name and number e.g. _default_0.rtx");
			}

			FindAttributes((ChunkyFolder)this.ChunkyStructures[0].RootChunks[0]);			
		}

		private void FindAttributes(ChunkyFolder folder)
		{
			int children = folder.Children.Count;
			ChunkyChunk chunk = null;
			ChunkyDataATTR attr = null;

			for (int i = 0; i<children; i++)
			{
				chunk = folder.Children[i];

				if (chunk is ChunkyDataATTR)
				{
					attr = (ChunkyDataATTR)chunk;
					width = attr.Width;
					height = attr.Height;
					break;
				}
				else if (chunk is ChunkyFolder)
				{
					FindAttributes((ChunkyFolder)chunk);
				}
			}
		}

		public void SaveParts(DirectoryInfo destination)
		{
			string filenamebase = filename.Substring(0, filename.LastIndexOf('.'));

			saveFolder((ChunkyFolder)this.ChunkyStructures[0].RootChunks[0], destination, filenamebase);
		}

		private void saveFolder(ChunkyFolder folder, DirectoryInfo destination, string filenamebase)
		{
			int children = folder.Children.Count;
			ChunkyChunk chunk = null;

			for (int i = 0; i<children; i++)
			{
				chunk = folder.Children[i];

				if (chunk is ChunkyFolder)
				{
					this.saveFolder((ChunkyFolder)chunk, destination, filenamebase);
				}
				/*else if (chunk is ChunkyDataATTR)
				{
					attr = (ChunkyDataATTR)chunk;
					imagetype = attr.ImageType;
					mipmaps = attr.MipMaps;
				}*/
				else if (chunk is ChunkyDataDATAIMAG)
				{
					((ChunkyDataDATAIMAG)chunk).Save(destination, filenamebase);
				}
			}
		}

		public static RTXFile Create(string filepath)
		{
			string baseFileName = filepath.Substring(filepath.LastIndexOf(Path.DirectorySeparatorChar)+1);
			string baseFileNameLower = baseFileName.ToLower();

			string directory = "";

			if (filepath.IndexOf(Path.DirectorySeparatorChar)!=-1)
			{
				directory = filepath.Substring(0, filepath.LastIndexOf(Path.DirectorySeparatorChar))+Path.DirectorySeparatorChar;
			}
			
			if(baseFileNameLower.EndsWith(".dds"))
			{
				baseFileName = baseFileName.Substring(0, baseFileName.Length-4);
			}
			else
			{
				throw new InvalidFileException("File path specified is not valid for a RTX file");
			}

			string unit_name = "";
			int lastUnderscore = baseFileName.LastIndexOf('_');

			if (lastUnderscore!=-1)
			{
				unit_name = baseFileName.Substring(0, lastUnderscore);//trim the number

				try
				{
					int id = int.Parse(baseFileName.Substring(lastUnderscore+1));
				}
				catch
				{
					throw new InvalidFileException("DDS file name specified must end with a number to identify RTX e.g. _default_0.dds");
				}

				lastUnderscore = unit_name.LastIndexOf('_');
				if (lastUnderscore!=-1)
				{
					unit_name = unit_name.Substring(0, lastUnderscore);
				}
				else
				{
					throw new InvalidFileException("DDS file name specified must end with a skin name followed by a number e.g. _default_0.dds");
				}
			}
			else
			{
				throw new InvalidFileException("DDS file name specified must end with a skin name and number e.g. _default_0.dds");
			}

			ChunkyDataDATA defaultData = null;
			ChunkyData attr = null;
			byte [] data;

			int width = 0;
			int height = 0;
			int mipmaps = 0;

			if (File.Exists(directory+baseFileName+".dds"))
			{
				CompilationEvent("Reading "+baseFileName+".dds");
				FileInfo file = new FileInfo(directory+baseFileName+".dds");
				BinaryReader br = new BinaryReader(file.OpenRead());
				br.BaseStream.Seek(0, SeekOrigin.Begin);
				data = br.ReadBytes((int)file.Length);

				defaultData = ChunkyDataDATAIMAG.CreateFromDDS(2, "", data);
				
				br.BaseStream.Seek(12,SeekOrigin.Begin);
				height = br.ReadInt32();
				width = br.ReadInt32();
				int size = br.ReadInt32();
				br.BaseStream.Seek(4, SeekOrigin.Current);
				mipmaps = br.ReadInt32();
				br.Close();

				byte type = 0x8;

				if (size==width*height)
				{
					type = 0xb;
				}

				data = new byte[]{type, 0x0, 0x0, 0x0, (byte)width, (byte)(width>>8), (byte)(width>>16), (byte)(width>>24), (byte)(height), (byte)(height>>8), (byte)(height>>16), (byte)(height>>24), (byte)mipmaps, (byte)(mipmaps>>8), (byte)(mipmaps>>16), (byte)(mipmaps>>24)};
				attr = new ChunkyDataUnknown("ATTR", 2, "", data);
			}
			else
			{
				throw new RelicTools.Exceptions.FileNotFoundException("RTX files must be made from a DDS file");
			}

			ChunkyFolder defaultFolder = new ChunkyFolder("IMAG", 1, "");

			defaultFolder.Children.Add(attr);
			defaultFolder.Children.Add(defaultData);

			ChunkyData head = new ChunkyDataUnknown("HEAD", 1, "", new byte[]{0x05, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x00});


			CompilationEvent("Compiling RTX File");
			ChunkyFolder txtr = new ChunkyFolder("TXTR", 1, unit_name);
			txtr.Children.Add(head);
			txtr.Children.Add(defaultFolder);
			return new RTXFile(baseFileName+".rtx", txtr);
			//RelicChunkyFile.SaveChunky(directory+baseFileName+".rtx", txtr.GetBytes());
		}
	}
}
