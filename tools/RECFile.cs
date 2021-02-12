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
	/// Summary description for WTPFile.
	/// </summary>
	public class RECFile:RelicChunkyFile
	{
		int version;
		string recType;

		public RECFile(string filename, ChunkyStructureCollection chunkyStructs, int ver, string type): base(filename, chunkyStructs)
		{	
			version = ver;
			recType = type;
		}

		public string RecordingType
		{
			get { return recType; }
		}

		public int RecordingVersion
		{
			get { return version; }
		}
	}
}
