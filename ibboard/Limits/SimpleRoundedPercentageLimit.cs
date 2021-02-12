//  This file (SimpleRoundedPercentageLimit.cs) is a part of the IBBoard project and is copyright 2009 IBBoard
// 
//  The file and the library/program it is in are licensed under the GNU LGPL license, either version 3 of the License or (at your option) any later version. Please see COPYING.LGPL for more information and the full license.

using System;
using IBBoard.CustomMath;

namespace IBBoard.Limits
{
	/// <summary>
	/// A percentage-based limit that always either rounds up or down to the closest integer
	/// </summary>
	public class SimpleRoundedPercentageLimit : AbstractLimit, IPercentageLimit
	{
		private bool roundUp;
		
		public SimpleRoundedPercentageLimit (double percentageLimit) : this(percentageLimit, true)
		{			
		}
		
		public SimpleRoundedPercentageLimit (double percentageLimit, bool roundFractionUp) : base(percentageLimit)
		{
			roundUp = roundFractionUp;
		}
		
		public double Percentage 
		{
			get 
			{
				return Limit;
			}
		}

		
		/// <summary>
		/// Gets the limited number, based on the percentage limit that this <code>Limit</code> represents and the rounding direction
		/// </summary>
		/// <param name="size">
		/// The maximum size
		/// </param>
		/// <returns>
		/// <code>size</code> or the numeric limit this object was created with, whichever is smaller.
		/// </returns>
		public override int GetLimit(int size)
		{
			return (int)IBBMath.Round(size * Limit / 100, roundUp);
		}
	}
}
