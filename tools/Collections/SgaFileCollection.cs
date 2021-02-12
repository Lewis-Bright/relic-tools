// This file is a part of the Relic Tools and is copyright 2006-2018 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU GPL license, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.
using System;
using System.Collections;
using IBBoard.Relic.RelicTools.Exceptions;

namespace IBBoard.Relic.RelicTools.Collections
{
	/// <summary>
	/// Summary description for SgaFileCollection.
	/// </summary>
	public class SgaFileCollection: IEnumerable, ICollection
	{
		private SortedList col = null;

		/// <summary>
		/// 
		/// </summary>
		public SgaFileCollection()
		{
			col = new SortedList();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="toAdd"></param>
		public void Add(SgaFile toAdd)
		{
			if (!col.ContainsKey(toAdd.Name))
			{
				col.Add(toAdd.Name, toAdd);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public SgaFile this[string name]
		{
			get
			{
				if (col.ContainsKey(name))
				{
					return (SgaFile)col[name];
				}
				else
				{
					throw new FileNotFoundException();
				}
			}
		}

		#region ICollection Members

		public bool IsSynchronized
		{
			get
			{
				return col.IsSynchronized;
			}
		}

		public int Count
		{
			get
			{
				return col.Count;
			}
		}

		public void CopyTo(Array array, int index)
		{
			col.CopyTo(array, index);
		}

		public object SyncRoot
		{
			get
			{
				return col.SyncRoot;
			}
		}

		#endregion

		/// <summary>
		/// 
		/// </summary>
		public ICollection Values
		{
			get{ return col.Values;}
		}

		#region IEnumerable Members

		public IEnumerator GetEnumerator()
		{
			return col.GetEnumerator();
		}

		#endregion
	}
}
