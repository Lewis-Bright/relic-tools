//  This file (AbstractCompositeLimit.cs) is a part of the IBBoard project and is copyright 2010 IBBoard
// 
//  The file and the library/program it is in are licensed and distributed, without warranty, under the GNU LGPL, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.
using System.Collections.Generic;
using IBBoard.Collections;

namespace IBBoard.Limits
{
	/// <summary>
	/// Abstract implementation of a limit that is a composition of one or more other limits
	/// </summary>
	public abstract class AbstractCompositeLimit : ILimit
	{
		private SimpleSet<ILimit> limits;
		
		private AbstractCompositeLimit()
		{
			limits = new SimpleSet<ILimit>();
		}
		
		/// <summary>
		/// Creates a composite limit based on one initial limit
		/// </summary>
		/// <param name="initialLimit">
		/// The initial <see cref="ILimit"/>
		/// </param>
		public AbstractCompositeLimit(ILimit initialLimit) : this()
		{
			AddLimit(initialLimit);
		}
		
		/// <summary>
		/// Creates a composite limit based on a collection of initial limits
		/// </summary>
		/// <param name="initialLimit">
		/// The initial <see cref="ILimit"/>
		/// </param>
		public AbstractCompositeLimit(ICollection<ILimit> initialLimits) : this()
		{
			AddLimits(initialLimits);
		}
		
		/// <summary>
		/// Adds an <see cref="ILimit"/> to the set of limits included in the composition
		/// </summary>
		/// <param name="limit">
		/// The <see cref="ILimit"/> to add
		/// </param>
		public void AddLimit(ILimit limit)
		{
			limits.Add(limit);
		}
		
		/// <summary>
		/// Adds a collection of <see cref="ILimit"/> to the set of limits included in the composition
		/// </summary>
		/// <param name="limit">
		/// The <see cref="ILimit"/>s to add
		/// </param>
		public void AddLimits(ICollection<ILimit> newLimits)
		{
			limits.AddRange(newLimits);
		}
		
		protected SimpleSet<ILimit> Limits
		{
			get { return limits; }
		}
		
		public int GetLimit(int size)
		{
			int limit = 0;
			
			if (limits.Count > 0)
			{
				limit = CalculateLimit(size);
			}
			
			return limit;
		}

		protected abstract int CalculateLimit(int size);


		public override int GetHashCode()
		{
			int hash = GetType().GetHashCode();
			hash += limits.GetHashCode();
			return hash;
		}

		public override bool Equals(object obj)
		{
			bool equal = false;
			
			if (obj != null && GetType().Equals(obj.GetType()))
			{
				equal = Limits.Equals(((AbstractCompositeLimit)obj).Limits);
			}
			
			return equal;
		}
		
	}
}

