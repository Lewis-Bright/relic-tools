// This file (IPercentageLimit.cs) is a part of the IBBoard project and is copyright 2009 IBBoard
// 
// The file and the library/program it is in are licensed and distributed, without warranty, under the GNU LGPL, either version 3 of the License or (at your option) any later version. Please see COPYING for more information and the full license.

using System;

namespace IBBoard.Limits
{
	/// <summary>
	/// A marker interface for limits that represent a percentage restriction
	/// </summary>
	public interface IPercentageLimit : ILimit
	{
		double Percentage { get; }
	}
}
