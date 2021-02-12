// This file is a part of the Relic Tools and is copyright 2006-2018 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU GPL license, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.

using System;
using System.IO;
using System.Collections;

namespace IBBoard.Relic.RelicTools
{
	/// <summary>
	/// Summary description for SgaArchive.
	/// </summary>
	public class SgaArchive
	{
		private string path = "";
		private readonly Hashtable attrib;
		private SgaReader sr = null;
		private SgaFolder root = null;
		private FileInfo file = null;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pathIn"></param>
		public SgaArchive(string pathIn)
		{
			path = pathIn;
			file = new FileInfo(this.path);
			sr = new SgaReader(path);
			sr.Archive = this;
			attrib = sr.ReadHeaders();
			root = sr.ReadFolders((long)attrib["DirOffset"], (long)attrib["ItemOffset"], attrib["TocAlias"].ToString());
			root.ParentArchive = this;
		}

		/// <summary>
		/// 
		/// </summary>
		public SgaReader ArchiveReader
		{
			get{ return sr; }
		}

		public string Name
		{
			get{ return file.Name; }
		}

		public SgaFolder Root
		{
			get{return root;}
		}

		public int Version
		{
			get { return (int)(long)attrib["Version"]; }
		}

		/// <summary>
		/// 
		/// </summary>
		public Hashtable Attributes
		{
			get{ return attrib; }
		}

		public string DefaultPath
		{
			get{ return file.DirectoryName+Path.DirectorySeparatorChar+attrib["TocAlias"]; }
		}

		public string BasePath
		{
			get{ return file.DirectoryName; }
		}

		/// <summary>
		/// 
		/// </summary>
		public bool ExtractAll()
		{
			return ExtractAll(this.DefaultPath);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="overwrite"></param>
		public bool ExtractAll(bool overwrite)
		{
			return ExtractAll(this.DefaultPath, overwrite);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="destination"></param>
		public bool ExtractAll(string destination)
		{
			return ExtractAll(destination, false);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="desintation"></param>
		/// <param name="overwrite"></param>
		public bool ExtractAll(string desintation, bool overwrite)
		{
			return ExtractFolder("",desintation, true, overwrite);
		}

		/// <summary>
		/// Extracts the specified folder to it's default location
		/// </summary>
		/// <param name="folderPath">The path of the folder to extract</param>
		public bool ExtractFolder(string folderPath)
		{
			folderPath = this.TrimPath(folderPath);

			return ExtractFolder(folderPath, this.DefaultPath+"\\"+folderPath);
		}

		public bool ExtractFolder(string folderPath, string dest)
		{
			return ExtractFolder(folderPath, dest, false);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="folderPath"></param>
		/// <param name="recursive"></param>
		public bool ExtractFolder(string folderPath, bool recursive)
		{
			folderPath = this.TrimPath(folderPath);

			return ExtractFolder(folderPath, this.DefaultPath+"\\"+folderPath, recursive);
		}

		public bool ExtractFolder(string folderPath, bool recursive, bool overwrite)
		{
			folderPath = this.TrimPath(folderPath);

			return ExtractFolder(folderPath, this.DefaultPath+"\\"+folderPath, recursive, overwrite);
		}

		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="folderPath"></param>
		/// <param name="destination"></param>
		public bool ExtractFolder(string folderPath, string find, string replace)
		{
			folderPath = this.TrimPath(folderPath);

			return ExtractFolder(folderPath, this.DefaultPath+"\\"+folderPath, find, replace);
		}

		public bool ExtractFolder(string folderPath, string find, string replace, bool recursive)
		{
			folderPath = this.TrimPath(folderPath);

			return ExtractFolder(folderPath, this.DefaultPath+"\\"+folderPath, recursive, false);
		}

		public bool ExtractFolder(string folderPath, string find, string replace, bool recursive, bool overwrite)
		{
			folderPath = this.TrimPath(folderPath);

			return ExtractFolder(folderPath, this.DefaultPath+"\\"+folderPath, "", "", recursive, overwrite);
		}

		public bool ExtractFolder(string folderPath, string[] find, string[] replace)
		{
			folderPath = this.TrimPath(folderPath);

			return ExtractFolder(folderPath, this.DefaultPath+"\\"+folderPath, find, replace, false);
		}

		public bool ExtractFolder(string folderPath, string[] find, string[] replace, bool recursive)
		{
			folderPath = this.TrimPath(folderPath);

			return ExtractFolder(folderPath, this.DefaultPath+"\\"+folderPath, find, replace, recursive, false);
		}

		public bool ExtractFolder(string folderPath, string[] find, string[] replace, bool recursive, bool overwrite)
		{
			folderPath = this.TrimPath(folderPath);

			return ExtractFolder(folderPath, this.DefaultPath+"\\"+folderPath, find, replace, recursive, overwrite);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="folderPath"></param>
		/// <param name="destination"></param>
		/// <param name="recursive"></param>
		public bool ExtractFolder(string folderPath, string destination, bool recursive)
		{
			return ExtractFolder(folderPath, destination, recursive, false);
		}

		public bool ExtractFolder(string folderPath, string destination, bool recursive, bool overwrite)
		{
			return ExtractFolder(folderPath, destination, "", "", recursive, overwrite);
		}

		public bool ExtractFolder(string folderPath, string destination, string find, string replace)
		{
			return ExtractFolder(folderPath, destination, find, replace, false);
		}

		public bool ExtractFolder(string folderPath, string destination, string[] find, string[] replace)
		{
			return ExtractFolder(folderPath, destination, find, replace, false);
		}

		public bool ExtractFolder(string folderPath, string destination, string find, string replace, bool recursive)
		{
			return ExtractFolder(folderPath, destination, find, replace, recursive, false);
		}

		public bool ExtractFolder(string folderPath, string destination, string[] find, string[] replace, bool recursive)
		{
			return ExtractFolder(folderPath, destination, find, replace, recursive, false);
		}

		public bool ExtractFolder(string folderPath, string destination, string find, string replace, bool recursive, bool overwrite)
		{
			return ExtractFolder(folderPath, destination, new string[]{find}, new string[]{replace}, recursive, overwrite);
		}

		public bool ExtractFolder(string folderPath, string destination, string[] find, string[] replace, bool recursive, bool overwrite)
		{
			destination = destination.Replace('\\', Constants.DirectoryChar);
			folderPath = folderPath.TrimEnd(Constants.DirectoryChar);
			folderPath = TrimPath(folderPath);

			SgaFolder container = root.DigFor(folderPath);
			if (container!=null)
			{
				return container.ExtractAll(destination, find, replace, recursive, overwrite);
			}
			else
			{
				ExtractFolderFail(container, "path not found");
				return false;
			}
		}

		public bool ExtractType(string ext)
		{
			FileInfo file = new FileInfo(this.path);
			return ExtractType(ext, "", this.DefaultPath, true);
		}

		public bool ExtractType(string ext, bool overwrite)
		{
			FileInfo file = new FileInfo(this.path);
			return ExtractType(ext, "", this.DefaultPath, true, overwrite);
		}

		public bool ExtractType(string ext, string folderPath)
		{
			folderPath = this.TrimPath(folderPath);

			return ExtractType(ext, folderPath, this.DefaultPath+"\\"+folderPath);
		}

		public bool ExtractType(string ext, string folderPath, bool recursive)
		{
			folderPath = this.TrimPath(folderPath);
			
			return ExtractType(ext, folderPath, this.DefaultPath+"\\"+folderPath, recursive);
		}

		public bool ExtractType(string ext, string folderPath, bool recursive, bool overwrite)
		{
			folderPath = this.TrimPath(folderPath);

			return ExtractType(ext, folderPath, this.DefaultPath+"\\"+folderPath, recursive, overwrite);
		}

		public bool ExtractType(string ext, string folderPath, string destination)
		{
			return ExtractType(ext, folderPath, destination, false);
		}

		public bool ExtractType(string ext, string folderPath, string destination, bool recursive)
		{
			return ExtractType(ext, folderPath, destination, recursive, false);
		}

		public bool ExtractType(string ext, string folderPath, string destination, bool recursive, bool overwrite)
		{
			return ExtractType(ext, folderPath, destination, "", "", recursive, overwrite);
		}

		public bool ExtractType(string ext, string folderPath, string find, string replace)
		{
			folderPath = this.TrimPath(folderPath);

			return ExtractType(ext, folderPath, this.DefaultPath+"\\"+folderPath, find, replace,  false);
		}

		public bool ExtractType(string ext, string folderPath, string find, string replace, bool recursive)
		{
			folderPath = this.TrimPath(folderPath);

			return ExtractType(ext, folderPath, this.DefaultPath+"\\"+folderPath, find, replace, recursive, false);
		}

		public bool ExtractType(string ext, string folderPath, string find, string replace, bool recursive, bool overwrite)
		{
			folderPath = this.TrimPath(folderPath);

			return ExtractType(ext, folderPath, this.DefaultPath+"\\"+folderPath, find, replace, recursive, overwrite);
		}

		public bool ExtractType(string ext, string folderPath, string[] find, string[] replace)
		{
			folderPath = this.TrimPath(folderPath);

			return ExtractType(ext, folderPath, this.DefaultPath+"\\"+folderPath, find, replace,  false);
		}

		public bool ExtractType(string ext, string folderPath, string[] find, string[] replace, bool recursive)
		{
			folderPath = this.TrimPath(folderPath);

			return ExtractType(ext, folderPath, this.DefaultPath+"\\"+folderPath, find, replace, recursive, false);
		}

		public bool ExtractType(string ext, string folderPath, string[] find, string[] replace, bool recursive, bool overwrite)
		{
			folderPath = this.TrimPath(folderPath);

			return ExtractType(ext, folderPath, this.DefaultPath+"\\"+folderPath, find, replace, recursive, overwrite);
		}

		public bool ExtractType(string ext, string folderPath, string destination, string[] find, string[] replace)
		{
			return ExtractType(ext, folderPath, destination, find, replace, false, false);
		}

		public bool ExtractType(string ext, string folderPath, string destination, string find, string replace)
		{
			return ExtractType(ext, folderPath, destination, find, replace, false, false);
		}

		public bool ExtractType(string ext, string folderPath, string destination, string[] find, string[] replace, bool recursive)
		{
			return ExtractType(ext, folderPath, destination, find, replace, recursive, false);
		}

		public bool ExtractType(string ext, string folderPath, string destination, string find, string replace, bool recursive)
		{
			return ExtractType(ext, folderPath, destination, find, replace, recursive, false);
		}

		public bool ExtractType(string ext, string folderPath, string destination, string find, string replace, bool recursive, bool overwrite)
		{
			return ExtractType(ext, folderPath, destination, new string[]{find}, new string[]{replace}, recursive, overwrite);
		}

		public bool ExtractType(string ext, string folderPath, string destination, string[] find, string[] replace, bool recursive, bool overwrite)
		{
			folderPath = this.TrimPath(folderPath);

			SgaFolder container = root.DigFor(folderPath);
			
			if (container!=null)
			{
				return container.ExtractType(ext,destination, find, replace, recursive, overwrite);
			}
			else
			{
				ExtractFolderFail(null, "path not found");
				return false;
			}
		}


		public bool Extract(string filePath)
		{
			filePath = this.TrimPath(filePath);
			
			int lastSlash = filePath.LastIndexOf("\\");
			int lastDot = filePath.LastIndexOf('.');
			string folderPath = "";

			if (lastDot==-1)
			{
				//Someone stole our dot!
				ExtractFileFail(null, "Filename '"+filePath+"'did not contain an extension");
				return false;
			}
			else
			{
				folderPath = filePath.Substring(0,lastSlash);
			}

			return Extract(filePath, this.DefaultPath+"\\"+folderPath, false);
		}

		public bool Extract(string filePath, bool overwrite)
		{
			filePath = this.TrimPath(filePath);
			
			int lastSlash = filePath.LastIndexOf("\\");
			int lastDot = filePath.LastIndexOf('.');
			string folderPath = "";

			if (lastDot==-1)
			{
				//Someone stole our dot!
				ExtractFileFail(null, "Filename '"+filePath+"'did not contain an extension");
				return false;
			}
			else if (lastSlash>=0)
			{
				folderPath = filePath.Substring(0,lastSlash);
			}

			return Extract(filePath, this.DefaultPath+"\\"+folderPath, overwrite);
		}

		public bool Extract(string filePath, string find, string replace)
		{
			filePath = this.TrimPath(filePath);
			
			int lastSlash = filePath.LastIndexOf("\\");
			int lastDot = filePath.LastIndexOf('.');
			string folderPath = "";

			if (lastDot==-1)
			{
				//Someone stole our dot!
				ExtractFileFail(null, "Filename '"+filePath+"'did not contain an extension");
				return false;
			}
			else
			{
				folderPath = filePath.Substring(0,lastSlash);
			}

			return Extract(filePath, this.DefaultPath+"\\"+folderPath, find, replace);
		}

		public bool Extract(string filePath, string find, string replace, bool overwrite)
		{
			filePath = this.TrimPath(filePath);
			
			int lastSlash = filePath.LastIndexOf("\\");
			int lastDot = filePath.LastIndexOf('.');
			string folderPath = "";

			if (lastDot==-1)
			{
				//Someone stole our dot!
				ExtractFileFail(null, "Filename '"+filePath+"'did not contain an extension");
				return false;
			}
			else
			{
				folderPath = filePath.Substring(0,lastSlash);
			}

			return Extract(filePath, this.DefaultPath+"\\"+folderPath, find, replace, overwrite);
		}

		public bool Extract(string filePath, string[] find, string[] replace)
		{
			filePath = this.TrimPath(filePath);
			
			int lastSlash = filePath.LastIndexOf("\\");
			int lastDot = filePath.LastIndexOf('.');
			string folderPath = "";

			if (lastDot==-1)
			{
				//Someone stole our dot!
				ExtractFileFail(null, "Filename '"+filePath+"'did not contain an extension");
				return false;
			}
			else
			{
				folderPath = filePath.Substring(0,lastSlash);
			}

			return Extract(filePath, this.DefaultPath+"\\"+folderPath, find, replace, false);
		}

		public bool Extract(string filePath, string[] find, string[] replace, bool overwrite)
		{
			filePath = this.TrimPath(filePath);
			
			int lastSlash = filePath.LastIndexOf("\\");
			int lastDot = filePath.LastIndexOf('.');
			string folderPath = "";

			if (lastDot==-1)
			{
				//Someone stole our dot!
				ExtractFileFail(null, "Filename '"+filePath+"' did not contain an extension");
				return false;
			}
			else
			{
				folderPath = filePath.Substring(0,lastSlash);
			}

			return Extract(filePath, this.DefaultPath+"\\"+folderPath, find, replace, overwrite);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="destination"></param>
		public bool Extract(string filePath, string destination)
		{
			return Extract(filePath, destination,"","", false);
		}

		public bool Extract(string filePath, string destination, bool overwrite)
		{
			return Extract(filePath, destination,"","", overwrite);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="filePath"></param>
		/// <param name="destination"></param>
		/// <param name="find"></param>
		/// <param name="replace"></param>
		public bool Extract(string filePath, string destination, string find, string replace)
		{
			return Extract(filePath, destination, new string[]{find}, new string[]{replace});
		}

		public bool Extract(string filePath, string destination, string find, string replace, bool overwrite)
		{
			return Extract(filePath, destination, new string[]{find}, new string[]{replace}, overwrite);
		}

		public bool Extract(string filePath, string destination, string[] find, string[] replace)
		{
			return Extract(filePath, destination, find, replace, false);
		}
		public bool Extract(string filePath, string destination, string[] find, string[] replace, bool overwrite)
		{
			
			if (destination.EndsWith("\\"))
			{
				destination = destination.TrimEnd('\\');
			}

			filePath = this.TrimPath(filePath);

			int lastSlash = filePath.LastIndexOf('\\');
			string folderPath = "";

			if (lastSlash>-1)
			{
				folderPath = filePath.Substring(0, lastSlash);
			}
			
			destination = destination.Replace('\\', Constants.DirectoryChar);
			
			SgaFolder container = root.DigFor(folderPath);

			if (container!=null)
			{
				string fileName = filePath.Substring(filePath.LastIndexOf('\\')+1);
				return container.Extract(fileName, destination, find, replace, overwrite);
			}
			else
			{
				ExtractFileFail(null, "path not found");
				return false;
				//throw exception
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="folderPath"></param>
		/// <returns></returns>
		public string TrimPath(string folderPath)
		{
			string temp = folderPath.ToLower();
			string start = "\\"+(string)attrib["TocAlias"];

			if (temp==start)
			{
				return "";
			}
			else if (temp.StartsWith(start))
			{
				return folderPath.Substring(start.Length+1);
			}
			else
			{
				return folderPath;
			}
		}

		public delegate void ExtractionNotification(string type, string name, string message);

		public event ExtractionNotification OnExtractFileFail;
		public event ExtractionNotification OnExtractFileSuccess;
		public event ExtractionNotification OnExtractFolderFail;
		public event ExtractionNotification OnExtractFolderSuccess;
		
		public void ExtractFileFail(SgaFile file, string reason)
		{
			if (OnExtractFileFail!=null)
			{
				if (file==null)
				{
					OnExtractFileFail("File", "unknown", reason);
				}
				else
				{
					OnExtractFileFail("File", file.Name, reason);
				}
			}
		}

		public void ExtractFileSuccess(SgaFile file)
		{
			if (OnExtractFileSuccess!=null)
			{
				OnExtractFileSuccess("File", file.Name, "");
			}
		}

		public void ExtractFolderFail(SgaFolder folder, string reason)
		{
			if (OnExtractFolderFail!=null)
			{
				if (folder==null)
				{
					OnExtractFolderFail("Folder", "unknown", reason);
				}
				else
				{
					OnExtractFolderFail("Folder", folder.Path, reason);
				}
			}
		}

		public void ExtractFolderSuccess(SgaFolder folder)
		{
			if (OnExtractFolderSuccess!=null)
			{
				OnExtractFolderSuccess("Folder", folder.Path, "");
			}
		}
	}
}