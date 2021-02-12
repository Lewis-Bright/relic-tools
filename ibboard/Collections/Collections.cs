//  This file (Collections.cs) is a part of the IBBoard project and is copyright 2011 IBBoard
// 
//  The file and the library/program it is in are licensed and distributed, without warranty, under the GNU LGPL, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.
using System;
using System.Collections.Generic;

namespace IBBoard.Collections
{
	public class Collections
	{
		private Collections()
		{
		}

		public static bool AreEqual<T, U> (IList<T> list1, IList<U> list2)
		{
			bool equal = true;

			if (typeof(T) != typeof(U))
			{
				equal = false;
			}
			else if (!EqualityChecker.AreEqual(list1, list2))
			{
				if (list1.Count != list2.Count)
				{
					equal = false;
				}
				else
				{
					int length = list1.Count;

					for (int i = 0; i < length; i++)
					{
						if (!EqualityChecker.AreEqual(list1[i], list2[i]))
						{
							equal = false;
							break;
						}
					}
				}
			}

			return equal;
		}

		public static bool AreEqual<T,U,V,W>(IDictionary<T, U> dict1, IDictionary<V, W> dict2) where V : class
		{
			bool equal = true;

			if (typeof(T) != typeof(V) || typeof(U) != typeof(W))
			{
				equal = false;
			}
			else if (!EqualityChecker.AreEqual(dict1, dict2))
			{
				if (dict1.Count != dict2.Count)
				{
					equal = false;
				}
				else
				{
					foreach (KeyValuePair<T, U> pair in dict1)
					{
						V key = pair.Key as V;
						if (!dict2.ContainsKey(key) || !EqualityChecker.AreEqual(pair.Value, dict2[key]))
						{
							equal = false;
							break;
						}
					}
				}
			}

			return equal;
		}
	}
}

