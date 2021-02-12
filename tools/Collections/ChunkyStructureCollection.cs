// This file is a part of the Relic Tools and is copyright 2006-2018 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU GPL license, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.
using System;
using System.Collections;

namespace IBBoard.Relic.RelicTools.Collections
{
	/// <summary>
	/// Summary description for ChunkyStructureCollection.
	/// </summary>
	public class ChunkyStructureCollection
	{		
		private ArrayList arr;
		private RelicChunkyFile parent;

		public ChunkyStructureCollection()
		{
			arr = new ArrayList();
		}

		public ChunkyStructureCollection(RelicChunkyFile parentFile):this()
		{			
			parent = parentFile;
		}

		public ChunkyStructureCollection(RelicChunkyStructure strct):this()
		{
			arr.Add(strct);
		}

		public void Add(RelicChunkyStructure chunk)
		{
			if (chunk!=null)
			{
				arr.Add(chunk);
				chunk.ParentFile = parent;
			}
		}

		public RelicChunkyStructure this[int key]
		{
			get{return (RelicChunkyStructure)arr[key];}
			set{ if (value!=null){arr[key] = value;}}
		}

		public void Remove(RelicChunkyStructure chunk)
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
