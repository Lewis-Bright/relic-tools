// This file (SimpleSet.cs) is a part of the IBBoard library and is copyright 2009 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU LGPL license, either version 3 of the License or (at your option) any later version. Please see COPYING.LGPL for more information and the full license.
using System;
using System.Collections;
using System.Collections.Generic;

namespace IBBoard.Collections
{
	public class SimpleSet<TYPE> : ICollection<TYPE>
	{		
		private Dictionary<int, TYPE> dictionary;
		
		public SimpleSet()
		{
			dictionary = new Dictionary<int,TYPE>();
		}
		
		public void Add(TYPE val)
		{
			dictionary[val.GetHashCode()] = val;
		}
		
		public bool AddRange(ICollection<TYPE> vals)
		{
			bool added = false;
			
			foreach (TYPE val in vals)
			{
				Add(val);
				added = true;
			}
			
			return added;
		}
		
		public bool Remove(TYPE val)
		{
			return dictionary.Remove(val.GetHashCode());
		}
		
		public bool Contains(TYPE item)
		{
			return dictionary.ContainsKey(item.GetHashCode());
		}

		public void Clear()
		{
			dictionary.Clear();
		}
		
		public int Count
		{
			get { return dictionary.Count; }
		}
		
		public bool IsReadOnly
		{
			get { return false; }
		}
		
		public IEnumerator GetEnumerator()
		{
			return dictionary.Values.GetEnumerator();
		}

		public void CopyTo(TYPE[] array, int arrayIndex)
		{
			if (arrayIndex + dictionary.Count > array.Length)
			{
				throw new ArgumentOutOfRangeException("arrayIndex", "Insufficient space in array");
			}
			if (arrayIndex < 0)
			{
				throw new ArgumentOutOfRangeException("arrayIndex", "Value must be > 0");
			}

			int i = arrayIndex;
			foreach (TYPE val in dictionary.Values)
			{
				array[i++] = val;
			}
		}

		IEnumerator<TYPE> IEnumerable<TYPE>.GetEnumerator()
		{
			return dictionary.Values.GetEnumerator();
		}
	}
}
