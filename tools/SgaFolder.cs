// This file is a part of the Relic Tools and is copyright 2006-2018 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU GPL license, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.
using System;
using System.IO;
using System.Collections;

using IBBoard.Relic.RelicTools.Collections;
using IBBoard.Relic.RelicTools.Exceptions;

namespace IBBoard.Relic.RelicTools
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class SgaFolder
	{
		private Hashtable attrib = null;
		private long id;
		private SgaFolder parent;
		private SgaFolderCollection children;
		private string name = "";
		//private string path = "";
		private SgaFileCollection files = null;
		private SgaArchive archive = null;

		/// <summary>
		/// Initialises a new instance of a Folder within an SGA file with the specified ID and attributes
		/// </summary>
		/// <param name="idNum">A Long specifying the ID of the archive, as determined by the position in the Table of Contents</param>
		/// <param name="attr">A Hashtable of attributes of the folder</param>
		public SgaFolder(long idNum, Hashtable attr):this(idNum, "", attr){}

		/// <summary>
		/// Initialises a new instance of a Folder within an SGA file with the specified ID, path and attributes
		/// </summary>
		/// <param name="idNum">A Long specifying the ID of the archive, as determined by the position in the Table of Contents</param>
		/// <param name="nameIn">A String specifying the path of the folder below the archive root</param>
		/// <param name="attr">A Hashtable of attributes of the folder</param>
		public SgaFolder(long idNum, string nameIn, Hashtable attr)
		{
			attrib = attr;
			id = idNum;
			children = new SgaFolderCollection();
			files = null;
			name = nameIn;

			//path = pathIn;
			//if (path.IndexOf(Path.DirectorySeparatorChar)>0)
			//{
				//name = path.Substring(path.LastIndexOf(Path.DirectorySeparatorChar)+1);
			//}
			//else
			//{
				//name = path;
			//}
		}

		/// <summary>
		/// Gets or sets the parent SgaFolder of this folder. Setting the parent also adds this folder as the child of the parent.
		/// </summary>
		public SgaFolder Parent
		{
			get{ return parent; }
			set{
				parent = value;
				value.SubFolders.Add(this);
			}
		}

		/// <summary>
		/// Gets an SgaFolderCollection containing all of the SgaFolder objects that are children of this folder
		/// </summary>
		public SgaFolderCollection SubFolders
		{
			get{ return children; }
		}

		/// <summary>
		/// Gets the ID of the folder, as specified by it's position in the Table of Contents of the archive
		/// </summary>
		public long ID 
		{
			get{ return id; }
		}

		/// <summary>
		/// Gets an SgaFileCollection of the files within this folder.
		/// </summary>
		public SgaFileCollection Files
		{
			get
			{
				//use some simple caching so we don't need to load all of the files if we don't want to use them
				if (files==null)
				{
					files = new SgaFileCollection();
					FillFiles();
				}
				
				return files;				
			}
		}

		public string PathDisplayString
		{
			get
			{
				if (parent==null)
				{
					return name;
				}
				else
				{
					return parent.PathDisplayString + ">" + name;
				}
			}
		}

			/// <summary>
			/// Gets or sets a reference to the SgaArchive that this folder is contained in. If no reference exists, then it is taken from the parent folder of this folder
			/// </summary>
		public SgaArchive ParentArchive
		{
			get
			{
				SgaArchive tempArchive;

				if (this.archive!=null)
				{
					return this.archive;
				}
				else if ((tempArchive = this.Parent.ParentArchive)!=null)
				{
					return tempArchive;
				}
				else
				{
					throw new InvalidUseException("No Parent Archive found. SgaFolders should only be instantiated automatically through the creation of an SgaArchive object");
				}
			}
			set
			{
				if (this.parent==null)
				{
					this.archive = value;

					//if (this.Name==this.archive.Attributes["TocAlias"].ToString())
					//{
						//this.path = "";
					//}
				}
			}
		}

		/// <summary>
		/// Gets the name of the folder
		/// </summary>
		public string Name
		{
			get{ return name; } 
		}

		/// <summary>
		/// Gets the full path of the folder within the archive
		/// </summary>
		public string InternalPath
		{
			get
			{
				if (this.Parent!=null)
				{
					return this.Parent.InternalPath+Name+"\\"; 
				}
				else
				{
					return "";
				}
			}
		}

		public string Path
		{
			get 
			{
				if (this.Parent!=null)
				{
					return "\\"+ParentArchive.Attributes["TocAlias"].ToString()+"\\"+this.InternalPath; 
				}
				else
				{
					return "\\"+ParentArchive.Attributes["TocAlias"].ToString()+"\\";
				}
			}
		}

		/// <summary>
		/// Works down the folder hierarchy of the archive to retrieve the SgaFolder that matches the path requested.
		/// </summary>
		/// <param name="digPath">A string of the path to dig for (not including the root folder e.g. 'data')</param>
		/// <returns>The SgaFolder object that represents the folder within the archive with the path specified</returns>
		public SgaFolder DigFor(string digPath)
		{
			if (digPath!="" && !digPath.EndsWith("\\"))
			{
				digPath = digPath + "\\";
			}

			string path = this.InternalPath;

			if (digPath == path)
			{
				return this;
			}
			else
			{
				if (SubFolders.Count>0)
				{
					foreach(SgaFolder subfolder in SubFolders.Values)
					{
						if (digPath==subfolder.InternalPath)
						{
							return subfolder;
						}
						else if (digPath.StartsWith(subfolder.InternalPath))
						{
							return subfolder.DigFor(digPath);
						}
					}

					return null;
				}
				else
				{
					return null;
				}
			}
		}

		/// <summary>
		/// Extracts a single file from within the folder to the destination specified
		/// </summary>
		/// <param name="fileName">A string representing the file name to extract</param>
		/// <param name="destination">A string representing the destination folder to save the file to</param>
		public bool Extract(string fileName, string destination)
		{
			return Extract(fileName, destination,"","");
		}

		/// <summary>
		/// Extracts a single file from within the folder to the destination specified, with the ability to force an overwrite of the file
		/// </summary>
		/// <param name="fileName">A string representing the file name to extract</param>
		/// <param name="destination">A string representing the destination folder to save the file to</param>
		/// <param name="overwrite">A boolean to specify whether to overwrite the file if it exists in the destination. This function will Exception if false is specified and the file exists.</param>
		public bool Extract(string fileName, string destination, bool overwrite)
		{
			return Extract(fileName, destination,"","", overwrite);
		}

		/// <summary>
		/// Extracts a single file from within the folder to the destination specified, with the ability to find and replace a string within the file.  This is useful for replacing race names when RSH or WHM files are being used for cloned races
		/// </summary>
		/// <param name="fileName">A string representing the file name to extract</param>
		/// <param name="destination">A string representing the destination folder to save the file to</param>
		/// <param name="find">A string representing the string to find in the code</param>
		/// <param name="replace">A string, which must be the same length as find, representing the string to replace the found matches with</param>
		public bool Extract(string fileName, string destination, string find, string replace)
		{
			return Extract(fileName, destination, find, replace, false);
		}

		public bool Extract(string fileName, string destination, string[] find, string[] replace)
		{
			return Extract(fileName, destination, find, replace, false);
		}

		/// <summary>
		/// Extracts a single file from within the folder to the destination specified, with the ability to find and replace a string within the file and force an overwrite of the file.  Find and Replace is useful for replacing race names when RSH or WHM files are being used for cloned races
		/// </summary>
		/// <param name="fileName">A string representing the file name to extract</param>
		/// <param name="destination">A string representing the destination folder to save the file to</param>
		/// <param name="find">A string representing the string to find in the code</param>
		/// <param name="replace">A string, which must be the same length as find, representing the string to replace the found matches with</param>
		/// <param name="overwrite">A boolean to specify whether to overwrite the file if it exists in the destination. This function will Exception if false is specified and the file exists.</param>
		public bool Extract(string fileName, string destination, string find, string replace, bool overwrite)
		{
			return Extract(fileName, destination, new string[]{find}, new string[]{replace}, overwrite);
		}

		public bool Extract(string fileName, string destination, string[] find, string[] replace, bool overwrite)
		{
			SgaFile file = this.Files[fileName];
			return file.Save(destination, find, replace, overwrite);
		}

		public bool ExtractType(string ext, string destination)
		{
			return ExtractType(ext, destination, false);
		}

		public bool ExtractType(string ext, string destination, bool recursive)
		{
			return ExtractType(ext, destination, recursive, false);
		}

		/// <summary>
		/// Extracts all of the files that have the specified extention within the folder to the destination specified. 
		/// </summary>
		/// <param name="ext">A string representing the extension to extract (does not include preceeding ".")</param>
		/// <param name="destination">A string representing the destination folder to save the file to</param>
		/// <param name="recursive">A boolean to define whether sub-folders should also be extracted</param>
		/// <param name="overwrite">A boolean to specify whether to overwrite the file if it exists in the destination. This function will Exception if false is specified and the file exists.</param>
		public bool ExtractType(string ext, string destination, bool recursive, bool overwrite)
		{
			return ExtractType(ext, destination, "","", recursive, overwrite);
		}

		public bool ExtractType(string ext, string destination, string find, string replace)
		{
			return ExtractType(ext, destination, find, replace, false);
		}

		public bool ExtractType(string ext, string destination, string find, string replace, bool recursive)
		{
			return ExtractType(ext, destination, find, replace, recursive, false);
		}

		public bool ExtractType(string ext, string destination, string[] find, string[] replace)
		{
			return ExtractType(ext, destination, find, replace, false);
		}

		public bool ExtractType(string ext, string destination, string[] find, string[] replace, bool recursive)
		{
			return ExtractType(ext, destination, find, replace, recursive, false);
		}

		public bool ExtractType(string ext, string destination, string find, string replace, bool recursive, bool overwrite)
		{
			return ExtractType(ext, destination, new string[]{find}, new string[]{replace}, recursive, overwrite);
		}

		public bool ExtractType(string ext, string destination, string[] find, string[] replace, bool recursive, bool overwrite)
		{
			if (destination.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
			{
				destination = destination.TrimEnd(System.IO.Path.DirectorySeparatorChar);
			}

			bool success = true;
			bool temp = false;

			if (recursive && this.SubFolders.Count>0)
			{
				foreach (SgaFolder subfolder in this.SubFolders.Values)
				{
					temp = subfolder.ExtractType(ext, destination+System.IO.Path.DirectorySeparatorChar+subfolder.Name, find, replace, recursive, overwrite);
					success = success && temp;
				}
			}

			ext = ext.ToLower();

			//trim the dot if someone ignored us and included it anyway
			if (ext[0]=='.')
			{
				ext = ext.Substring(1);
			}


			foreach (SgaFile file in this.Files.Values)
			{
				if (file.Extension==ext)
				{
					//separate the return and the anding out, because .Net doesn't go into the other files
					//once it hits the first "false" because it knows "false && anything" = false and so isn't
					//worth evaluating
					temp = file.Save(destination, find, replace, overwrite);
					success = success && temp;
				}
			}

			if (success)
			{
				this.ParentArchive.ExtractFolderSuccess(this);
			}
			else
			{
				this.ParentArchive.ExtractFolderFail(this, "one or more files failed to extract");
			}

			return success;
		}

		/// <summary>
		/// Extracts all of the files within the folder to the destination specified, but not from sub-folders. 
		/// </summary>
		/// <param name="destination">A string representing the path that the files should be extracted in to</param>
		public bool ExtractAll(string destination)
		{
			return ExtractAll(destination, false);
		}

		/// <summary>
		/// Extracts all of the files within the folder to the destination specified. When called recursively, it
		/// creates the folder structure of all folders below the current folder and extracts the files within them.
		/// </summary>
		/// <param name="destination">A string representing the path that the files should be extracted in to</param>
		/// <param name="recursive">A boolean to define whether sub-folders should also be extracted</param>
		public bool ExtractAll(string destination, bool recursive)
		{
			return ExtractAll(destination, recursive, false);
		}

		/// <summary>
		/// Extracts all of the files within the folder to the destination specified. When called recursively, it
		/// creates the folder structure of all folders below the current folder and extracts the files within them.
		/// </summary>
		/// <param name="destination"></param>
		/// <param name="recursive"></param>
		public bool ExtractAll(string destination, bool recursive, bool overwrite)
		{
			return ExtractAll(destination, "", "", recursive, overwrite);
		}

		public bool ExtractAll(string destination, string find, string replace)
		{
			return ExtractAll(destination, find, replace, false, false);
		}

		public bool ExtractAll(string destination, string[] find, string[] replace)
		{
			return ExtractAll(destination, find, replace, false, false);
		}

		public bool ExtractAll(string destination, string find, string replace, bool recursive)
		{
			return ExtractAll(destination, find, replace, recursive, false);
		}

		public bool ExtractAll(string destination, string[] find, string[] replace, bool recursive)
		{
			return ExtractAll(destination, find, replace, recursive, false);
		}

		public bool ExtractAll(string destination, string find, string replace, bool recursive, bool overwrite)
		{
			return ExtractAll(destination, new string[] {find}, new string[]{replace}, recursive, overwrite);
		}

		public bool ExtractAll(string destination, string[] find, string[] replace, bool recursive, bool overwrite)
		{
			if (destination.EndsWith(System.IO.Path.DirectorySeparatorChar.ToString()))
			{
				destination = destination.TrimEnd(System.IO.Path.DirectorySeparatorChar);
			}

			
			bool success = true;
			bool temp = false;

			if (recursive && this.SubFolders.Count>0)
			{
				foreach (SgaFolder subfolder in this.SubFolders.Values)
				{
					temp = subfolder.ExtractAll(destination+System.IO.Path.DirectorySeparatorChar+subfolder.Name, find, replace, recursive, overwrite);
					success = temp && success;
				}
			}


			foreach (SgaFile file in this.Files.Values)
			{
				//separate the return and the anding out, because .Net doesn't go into the other files
				//once it hits the first "false" because it knows "false && anything" = false and so isn't
				//worth evaluating
                temp = file.Save(destination, find, replace, overwrite);
				success = success && temp;
			}

			if (success)
			{
				this.ParentArchive.ExtractFolderSuccess(this);
			}
			else
			{
				this.ParentArchive.ExtractFolderFail(this, "one or more files failed to extract");
			}

			return success;
		}

		private void FillFiles()
		{
			long startID = (long)attrib["FileIDBegin"];
			long endID = (long)attrib["FileIDEnd"];
			SgaArchive parentArchive = this.ParentArchive;
			long itemOffset = (long)parentArchive.Attributes["ItemOffset"];
			long fileOffset = (long)parentArchive.Attributes["FileOffset"];
			SgaFile tempFile = null;

			for (long i = startID; i<endID; i++)
			{
				tempFile = parentArchive.ArchiveReader.ReadFile(fileOffset, itemOffset,i);
				tempFile.Parent = this;
			}
		}
	}
}
