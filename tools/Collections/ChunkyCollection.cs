// This file is a part of the Relic Tools and is copyright 2006-2018 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU GPL license, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.
using System;
using System.Collections;

namespace IBBoard.Relic.RelicTools.Collections
{
	/// <summary>
	/// Summary description for ChunkyCollection.
	/// </summary>
	public class ChunkyCollection
	{
		private ArrayList arr;
		private ChunkyFolder parent;

		public ChunkyCollection()
		{
			arr = new ArrayList();
		}

		public ChunkyCollection(ChunkyFolder parentFolder):this()
		{			
			parent = parentFolder;
		}

		public void Add(ChunkyChunk chunk)
		{
			if (chunk!=null)
			{
				arr.Add(chunk);
				chunk.Parent = parent;
			}
		}

		public ChunkyChunk this[int key]
		{
			get{return (ChunkyChunk)arr[key];}
			set{ if (value!=null){arr[key] = value;}}
		}

		public void Remove(ChunkyChunk chunk)
		{
			arr.Remove(chunk);
		}

		public int Count
		{
			get{return arr.Count;}
		}

		public IEnumerator GetEnumerator()
		{
			return arr.GetEnumerator();
		}
	}
}
