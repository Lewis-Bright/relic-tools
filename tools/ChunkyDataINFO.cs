// This file is a part of the Relic Tools and is copyright 2006-2018 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU GPL license, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.
using System;

namespace IBBoard.Relic.RelicTools
{
	/// <summary>
	/// Summary description for ChunkyChunkINFO.
	/// </summary>
	public abstract class ChunkyDataINFO : ChunkyData
	{
		public ChunkyDataINFO(int version_in, string name_in):base("INFO", version_in, name_in){}
	}
}
