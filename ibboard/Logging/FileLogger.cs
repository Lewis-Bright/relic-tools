// This file (FileLogger.cs) is a part of the IBBoard library and is copyright 2009 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU LGPL license, either version 3 of the License or (at your option) any later version. Please see COPYING.LGPL for more information and the full license.

using System;
using System.IO;

namespace IBBoard.Logging
{	
	public abstract class FileLogger : Logger
	{			
		public FileLogger() : this(CreateDefaultLogFileStream())
		{
		}
		
		public FileLogger(string path) : this(CreateLogFileStream(path))
		{
		}
		
		public FileLogger(FileInfo file) : this(CreateLogFileStream(file))
		{
		}
		
		public FileLogger(FileStream stream) : base(stream)
		{
		}
		
		public static string MakeDefaultLogFilePath()
		{
			return MakeLogFilePath(Constants.UserDataPath);
		}
		
		public static string MakeLogFilePath(string path)
		{
			return Path.Combine(Path.Combine(path, "logs"), String.Format("{0:yyyy-MM-dd-HHmmss}.log", DateTime.Now));
		}
		
		public static FileStream CreateDefaultLogFileStream()
		{
			return CreateLogFileStream(MakeDefaultLogFilePath());
		}
		
		public static FileStream CreateLogFileStream(string path)
		{
			return CreateLogFileStream(new FileInfo(path));
		}
		
		public static FileStream CreateLogFileStream(FileInfo file)
		{
			if (!file.Directory.Exists)
			{
				file.Directory.Create();
			}
			
			return new FileStream(file.FullName, FileMode.OpenOrCreate, FileAccess.Write);
		}
	}
}
