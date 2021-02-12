// This file is a part of the Relic Tools and is copyright 2006-2018 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU GPL license, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.
using System;

namespace IBBoard.Relic.RelicTools
{
	/// <summary>
	/// Summary description for ChunkyDataLayer.
	/// </summary>
	public abstract class ChunkyDataLayer : ChunkyData
	{
		private ChunkyDataINFOTPAT attr;

		public ChunkyDataLayer(string id, int version_in, string name_in):base(id, version_in, name_in)
		{
		}

		public ChunkyDataINFOTPAT Info
		{
			get{ return attr; }
			set{ attr = value; }
		}
	}
}
