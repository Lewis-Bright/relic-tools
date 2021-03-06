//  This file (CompositeMinimumLimit.cs) is a part of the IBBoard project and is copyright 2010 IBBoard
// 
//  The file and the library/program it is in are licensed and distributed, without warranty, under the GNU LGPL, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.
using System;
using System.Collections.Generic;
namespace IBBoard.Limits
{
	/// <summary>
	/// A composite limit that returns the minimum of all contained limits
	/// </summary>
	public class CompositeMinimumLimit : AbstractCompositeLimit
	{
		
		/// <summary>
		/// Creates a composite limit based on one initial limit
		/// </summary>
		/// <param name="initialLimit">
		/// The initial <see cref="ILimit"/>
		/// </param>
		public CompositeMinimumLimit(ILimit initialLimit) : base(initialLimit)
		{
			//Do nothing extra
		}
		
		/// <summary>
		/// Creates a composite limit based on a collection of initial limits
		/// </summary>
		/// <param name="initialLimit">
		/// The initial <see cref="ILimit"/>
		/// </param>
		public CompositeMinimumLimit(ICollection<ILimit> initialLimits) : base(initialLimits)
		{
			//Do nothing extra
		}

		protected override int CalculateLimit(int size)
		{
			int limitedNumber = int.MaxValue;
			
			foreach (ILimit limit in Limits)
			{
				limitedNumber = Math.Min(limitedNumber, limit.GetLimit(size));
			}
			
			return limitedNumber;
		}
	}
}

