//  This file (EqualityChecker.cs) is a part of the IBBoard project and is copyright 2010 IBBoard
// 
//  The file and the library/program it is in are licensed and distributed, without warranty, under the GNU LGPL, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.
using System;
namespace IBBoard
{
	/// <summary>
	/// Null-safe equality checker
	/// </summary>
	public class EqualityChecker
	{
		public static bool AreEqual(object obj1, object obj2)
		{
			bool areEqual = false;
			
			if ((obj1 == null && obj2 == null) || (obj1 != null && obj1.Equals(obj2)))
			{
				areEqual = true;
			}
			
			return areEqual;
		}
	}
}

