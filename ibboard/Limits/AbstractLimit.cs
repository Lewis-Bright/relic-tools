//  This file (AbstractLimit.cs) is a part of the IBBoard project and is copyright 2009 IBBoard
// 
//  The file and the library/program it is in are licensed under the GNU LGPL license, either version 3 of the License or (at your option) any later version. Please see COPYING.LGPL for more information and the full license.

using System;

namespace IBBoard.Limits
{
	/// <summary>
	/// The abstract base class for all limits
	/// </summary>
	public abstract class AbstractLimit : ILimit
	{
		private double limit;

		public AbstractLimit (double limitNum)
		{
			limit = limitNum;
		}
		
		protected double Limit
		{
			get
			{
				return limit;
			}
		}
		
		public override int GetHashCode ()
		{
			int hash = GetType().GetHashCode();
			hash+= Limit.GetHashCode();
			return hash;
		}
				
		public abstract int GetLimit(int size);

		public override bool Equals (object obj)
		{
			bool equal = false;
			
			if (obj != null && GetType().Equals(obj.GetType()))
			{
				equal = (Limit == ((AbstractLimit)obj).Limit);
			}
			
			return equal;
		}

	}
}
