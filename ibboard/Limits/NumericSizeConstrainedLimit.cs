//  This file (NumericSizeConstrainedLimit.cs) is a part of the IBBoard project and is copyright 2009 IBBoard
// 
//  The file and the library/program it is in are licensed under the GNU LGPL license, either version 3 of the License or (at your option) any later version. Please see COPYING.LGPL for more information and the full license.

using System;

namespace IBBoard.Limits
{
	/// <summary>
	/// A limit of a specified number, or the number it is limited against, whichever is smaller.
	/// </summary>
	public class NumericSizeConstrainedLimit : AbstractLimit, INumericLimit
	{
		/// <summary>
		/// Default constructor that will always return the constrained number, no matter what size it is
		/// </summary>
		public NumericSizeConstrainedLimit() : this(int.MaxValue)
		{			
		}
		
		public NumericSizeConstrainedLimit (int numericLimit) : base(numericLimit)
		{
		}
		
		/// <summary>
		/// Gets the limited number, based on the limit that this <code>Limit</code> represents and the maximum size.
		/// </summary>
		/// <param name="size">
		/// The maximum size
		/// </param>
		/// <returns>
		/// <code>size</code> or the numeric limit this object was created with, whichever is smaller.
		/// </returns>
		public override int GetLimit(int size)
		{
			return (int)Math.Min(size, Limit);
		}
	}
}
