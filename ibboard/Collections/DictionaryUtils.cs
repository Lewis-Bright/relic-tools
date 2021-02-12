// This file (DictionaryUtils.cs) is a part of the IBBoard library and is copyright 2009 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU LGPL license, either version 3 of the License or (at your option) any later version. Please see COPYING.LGPL for more information and the full license.

using System;
using System.Collections.Generic;

namespace IBBoard
{	
	public class DictionaryUtils
	{
		/// <summary>
		/// Takes the set of values in a dictionary and returns them as an array of typed objects.
		/// </summary>
		/// <param name="dictionary">
		/// A <see cref="IDictionary"/> to extract an array of values from
		/// </param>
		/// <returns>
		/// An array of <see cref="VALUE_TYPE"/> objects taken from the Values property of the dictionary, or NULL if the dictionary is NULL
		/// </returns>
		public static  VALUE_TYPE[] ToArray<KEY_TYPE, VALUE_TYPE>(IDictionary<KEY_TYPE, VALUE_TYPE> dictionary)
		{
			if (dictionary == null)
			{
				return null;
			}
			
			int entryCount = dictionary.Count;
			VALUE_TYPE[] col = new VALUE_TYPE[entryCount];
			
			if (entryCount > 0)
			{
				dictionary.Values.CopyTo(col, 0);
			}
			
			return col;
		}
		
		/// <summary>
		/// Takes the set of keys in a dictionary and returns them as an array of typed objects.
		/// </summary>
		/// <param name="dictionary">
		/// A <see cref="IDictionary"/> to extract an array of keys from
		/// </param>
		/// <returns>
		/// An array of <see cref="KEY_TYPE"/> objects taken from the Keys property of the dictionary, or NULL if the dictionary is NULL
		/// </returns>
		public static  KEY_TYPE[] ToKeyArray<KEY_TYPE, VALUE_TYPE>(IDictionary<KEY_TYPE, VALUE_TYPE> dictionary)
		{
			if (dictionary == null)
			{
				return null;
			}
			
			int entryCount = dictionary.Count;
			KEY_TYPE[] col = new KEY_TYPE[entryCount];
			
			if (entryCount > 0)
			{
				dictionary.Keys.CopyTo(col, 0);
			}
			
			return col;
		}

		/// <summary>
		/// Takes two dictionaries and merges them. If a key exists in both dictionaries then the value in <code>firstDictionary</code> takes priority.
		/// </summary>
		/// <param name="firstDictionary">
		/// The <see cref="IDictionary"/> to merge values in to
		/// </param>
		/// <param name="secondDictionary">
		/// The <see cref="IDictionary"/> of values to merge in
		/// </param>
		/// <returns>
		/// A <see cref="IDictionary"/> made by adding values from <code>secondDictionary</code> where the key didn't exist in <code>firstDictionary</code>
		/// </returns>
		public static IDictionary<KEY_TYPE, VALUE_TYPE> Merge<KEY_TYPE, VALUE_TYPE>(IDictionary<KEY_TYPE, VALUE_TYPE> firstDictionary, IDictionary<KEY_TYPE, VALUE_TYPE> secondDictionary)
		{
			Dictionary<KEY_TYPE, VALUE_TYPE> mergedDictionary = new Dictionary<KEY_TYPE, VALUE_TYPE>(firstDictionary);

			foreach (KEY_TYPE key in secondDictionary.Keys)
			{
				if (!mergedDictionary.ContainsKey(key))
				{
					mergedDictionary.Add(key, secondDictionary[key]);
				}
			}

			return mergedDictionary;
		}
		
		/// <summary>
		/// Convenience method to get a value from a dictionary for a given key. This method wraps a TryGetValue call in a single function.
		/// </summary>
		/// <param name="dictionary">
		/// The <see cref="IDictionary"/> to get a value from
		/// </param>
		/// <param name="key">
		/// The key to get the value for
		/// </param>
		/// <returns>
		/// The value for <code>key</code>, or <code>null</code> if there was no value for the key
		/// </returns>
		public static VALUE_TYPE GetValue<KEY_TYPE, VALUE_TYPE>(IDictionary<KEY_TYPE, VALUE_TYPE> dictionary, KEY_TYPE key)
		{
			if (dictionary == null)
			{
				return default(VALUE_TYPE);
			}
			
			VALUE_TYPE val = default(VALUE_TYPE);
			dictionary.TryGetValue(key, out val);
			return val;
		}
	}
}
