// This file (ILimit.cs) is a part of the IBBoard project and is copyright 2009 IBBoard
// 
// The file and the library/program it is in are licensed and distributed, without warranty, under the GNU LGPL, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.

using System;

namespace IBBoard.Limits
{
	/// <summary>
	/// The interface for all limits
	/// </summary>
	public interface ILimit
	{
		/// <summary>
		/// Gets the limited number, based on the limit that this <code>Limit</code> represents and the number it is limiting (<code>size</code>).
		/// </summary>
		/// <param name="size">
		/// The number to be limited
		/// </param>
		/// <returns>
		/// the limited number, as defined by the implementation of the limit
		/// </returns>
		int GetLimit(int size);
	}
}
