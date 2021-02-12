//  This file (IBBMath.cs) is a part of the IBBoard project and is copyright 2009 IBBoard
// 
//  The file and the library/program it is in are licensed under the GNU LGPL license, either version 3 of the License or (at your option) any later version. Please see COPYING.LGPL for more information and the full license.
// 

using System;

namespace IBBoard.CustomMath
{
	/// <summary>
	/// RoundType defines how a number should be rounded. "Up" always rounds up (Ceiling), "Down" always rounds down (Floor), "Banker" rounds
	/// up or down as appropriate with a balanced bias (rounding towards even). Each rounding method also has a "ToHalf" version that performs
	/// the same type of rounding, but to the closes half number instead of the closest whole number.
	/// </summary>
	public enum RoundType
	{
		Up,
		Down,
		Banker,
		UpToHalf,
		DownToHalf,
		BankerToHalf
	}

	/// <summary>
	/// IBBMath provides a number of custom Maths functions based on the core Math classes.
	/// </summary>
	public class IBBMath
	{
		/// <summary>
		/// Rounds a number to the closest half, with a bias towards whole numbers. This is equivalent to 'round-to-even' in that
		/// 0.25 is rounded down to 0.0 and 0.75 is rounded up to 1.0 so that a bias isn't introduced by rounding.
		/// </summary>
		/// <param name="number">
		/// The <see cref="System.Double"/> to round to the nearest 0.5
		/// </param>
		/// <returns>
		/// <code>param</code> rounded to the nearest 0.5
		/// </returns>
		public static double RoundToHalf(double number)
		{
			return Math.Round(number * 2) / 2;
		}
		
		/// <summary>
		/// Returns the largest whole or half number that is less than or equal to the specified number.
		/// </summary>
		/// <param name="number">
		/// The <see cref="System.Double"/> to round to the nearest 0.5
		/// </param>
		/// <returns>
		/// <code>param</code> rounded to the nearest 0.5 that is less than or equal to <code>param</code>
		/// </returns>
		public static double FloorToHalf(double number)
		{
			return Math.Floor(number * 2) / 2;
		}
		
		/// <summary>
		/// Returns the smallest whole or half number that is greater than or equal to the specified number.
		/// </summary>
		/// <param name="number">
		/// The <see cref="System.Double"/> to round to the nearest 0.5
		/// </param>
		/// <returns>
		/// <code>param</code> rounded to the nearest 0.5 that is greater than or equal to <code>param</code>
		/// </returns>
		public static double CeilToHalf(double number)
		{
			return Math.Ceiling(number * 2) / 2;
		}

		/// <summary>
		/// Returns the number rounded as defined by the <code>roundType</code>
		/// </summary>
		/// <param name="number">
		/// The <see cref="System.Double"/> to round
		/// </param>
		/// <param name="roundType">
		/// The way in which <code>number</code> should be rounded
		/// </param>
		/// <returns>
		/// The rounded <see cref="System.Double"/>
		/// </returns>
		public static double Round(double number, RoundType roundType)
		{
			double val;
			
			switch (roundType)
			{
			case RoundType.Up:
				val = Math.Ceiling(number);
				break;
			case RoundType.Down:
				val = Math.Floor(number);
				break;
			case RoundType.Banker:
				val = Math.Round(number);
				break;
			case RoundType.UpToHalf:
				val = CeilToHalf(number);
				break;
			case RoundType.DownToHalf:
				val = FloorToHalf(number);
				break;
			case RoundType.BankerToHalf:
				val = RoundToHalf(number);
				break;
			default:
				throw new InvalidOperationException("Unhandled round type: "+roundType);
			}

			return val;
		}
		
		/// <summary>
		/// Returns the number rounded up or down to the closest whole number.
		/// </summary>
		/// <param name="number">
		/// The <see cref="System.Double"/> to round
		/// </param>
		/// <param name="roundUp">
		/// <code>true</code> to round up, else rounds down
		/// </param>
		/// <returns>
		/// The rounded <see cref="System.Double"/>
		/// </returns>
		public static double Round(double number, bool roundUp)
		{
			return (roundUp ? Math.Ceiling(number) : Math.Floor(number));
		}
		
		/// <summary>
		/// Takes two integers and gets the percentage of the latter that the former represents. If the numerator is less than
		/// or equal to the denominator then this will be a number between 0.0 and 100.0.
		/// </summary>
		/// <param name="numerator">
		/// The numerator of the percentage (the top number)
		/// </param>
		/// <param name="denominator">
		/// The denominator of the percentage (the bottom number - the divisor)
		/// </param>
		/// <returns>
		/// The percentage, where 100% is the double value 100.0, or positive/negative infinity where numerator is non-zero and denominator is zero, or NaN where numerator and denominator are zero
		/// </returns>
		public static double Percentage(int numerator, int denominator)
		{
			return (numerator / (double) denominator) * 100;
		}
	}
}
