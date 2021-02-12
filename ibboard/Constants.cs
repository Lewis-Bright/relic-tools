// This file (Constants.cs) is a part of the IBBoard library and is copyright 2009 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU LGPL license, either version 3 of the License or (at your option) any later version. Please see COPYING.LGPL for more information and the full license.

using System;
using System.IO;

namespace IBBoard
{
	/// <summary>
	/// Summary description for Constants.
	/// </summary>
	public class Constants
	{
		public static readonly char DirectoryChar = Path.DirectorySeparatorChar;
		public static readonly string DirectoryString = Path.DirectorySeparatorChar.ToString();
		private static string executablePath;        
		private static string userDataPath;
		private static bool initialised;

		static Constants()
		{
			if (AppDomain.CurrentDomain.BaseDirectory != null && Environment.GetCommandLineArgs() != null)
			{
				string exe = Environment.GetCommandLineArgs()[0];
				Initialise(Path.GetFileNameWithoutExtension(exe), AppDomain.CurrentDomain.BaseDirectory);
			}
		}

		private static void Initialise(string appName, string baseDir)
		{
			string appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
			userDataPath = Path.Combine(Path.Combine(appDataDir, "IBBoard"), appName);
			executablePath = baseDir;
			initialised = true;
		}

		public static void Initialise(string appName)
		{
			if (initialised)
			{
				throw new InvalidOperationException("IBBoard.Constants have already been initialised");
			}

			Initialise(appName, "");
		}

		/// <summary>
		/// Gets the path of the directory that contains the executable.
		/// </summary>
		/// <value>
		/// The path that the executable is in, or an empty string if it cannot be determined
		/// </value>
		public static string ExecutablePath
		{
			get
			{ 
				if (!initialised)
				{
					throw new InvalidOperationException("IBBoard.Constants have not been initialised");
				}

				return executablePath; 
			}
		}

		/// <summary>
		/// Gets the standard user data path for this app. This follows a convention of using the app name for the folder
		/// and putting it in a folder called "IBBoard" to avoid collisions.
		/// </summary>
		/// <value>
		/// The user data path, following the IBBoard convention.
		/// </value>
		public static string UserDataPath
		{
			get
			{
				if (!initialised)
				{
					throw new InvalidOperationException("IBBoard.Constants have not been initialised");
				}

				return userDataPath;
			}
		}
	}
}
