// This file (Array.cs) is a part of the IBBoard library and is copyright 2009 IBBoard.
//
// The file and the library/program it is in are licensed under the GNU LGPL license, either version 3 of the License or (at your option) any later version. Please see COPYING.LGPL for more information and the full license.

using System;
using System.Collections.Generic;

namespace IBBoard
{
	/// <summary>
	/// Helper methods for doing maths set-type operations on arrays.
	/// </summary>
	public class Arrays
	{
		/// <summary>
		/// Subtract one array of items from another array of items and return them in a new array.
		/// </summary>
		/// <param name='items'>
		/// The array of items to subtract from
		/// </param>
		/// <param name='subtract'>
		/// The array of items to subtract
		/// </param>
		/// <typeparam name='T'>
		/// The type of the items in the arrays
		/// </typeparam>
		public static T[] Subtract<T>(T[] items, T[] subtract)
		{
			List<T> arr = new List<T>();
			arr.AddRange(items);
			
			foreach (T obj in subtract)
			{
				arr.Remove(obj);
			}

			return arr.ToArray();
		}
		
		/// <summary>
		/// Gets the objects that are not common to both arrays and returns them in a new array
		/// </summary>
		/// <param name='items1'>
		/// The first array of objects
		/// </param>
		/// <param name='items2'>
		/// The second array of objects
		/// </param>
		/// <typeparam name='T'>
		/// The type of the items in the arrays
		/// </typeparam>
		public static T[] Difference<T>(T[] items1, T[] items2)
		{
			T[] diffObjs;


			//Difference with as few loops as possible, so see which is shortest			
			if (items1.Length >= items2.Length)
			{
				//add everything from the first list
				diffObjs = DoDifference(items1, items2);
			}
			else
			{
				diffObjs = DoDifference(items2, items1);
			}

			return diffObjs;
		}

		private static T[] DoDifference<T>(T[] longArray, T[] shortArray)
		{
			List<T> arr = new List<T>();
			arr.AddRange(longArray);

			foreach (T obj in shortArray)
			{
				if (arr.Contains(obj))
				{
					arr.Remove(obj);
				}
				else
				{
					arr.Add(obj);
				}
			}
			
			return arr.ToArray();
		}
		
		/// <summary>
		/// Gets the index of an item in an array, or <code>-1</code> if the item isn't in the array
		/// </summary>
		/// <returns>
		/// The index of the item, or <code>-1</code> if the item isn't in the array
		/// </returns>
		/// <param name='items'>
		/// The array of items to find the item in
		/// </param>
		/// <param name='item'>
		/// The item to find
		/// </param>
		/// <typeparam name='T'>
		/// The type of the items in the arrays
		/// </typeparam>
		public static int IndexOf<T>(T[] items, T item)
		{
			return Array.IndexOf(items, item);
		}
		
		/// <summary>
		/// Tests whether an array of items contains an item
		/// </summary>
		/// <param name='items'>
		/// The array of items to find the item in
		/// </param>
		/// <param name='item'>
		/// The item to find
		/// </param>
		/// <typeparam name='T'>
		/// The type of the items in the arrays
		/// </typeparam>
		public static bool Contains<T>(T[] items, T item)
		{
			return IndexOf(items, item) != -1;
		}
	}
}
