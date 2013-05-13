/*
 *  Author:
 *       Jim Orcheson <jimorcheson@gmail.com>
 * 
 *  Copyright © 2013 Jim Orcheson
 * 
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 * 
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 * 
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */
using System;
using System.Globalization;
using System.Windows.Data;

namespace hamqsler
{
	/// <summary>
	/// Converter for converting a float to a percent value (also a float)
	/// </summary>
	[ValueConversion(typeof(float), typeof(float))]
	public class FloatToPercentConverter : IValueConverter
	{
		/// <summary>
		/// Convert from a float to a percent value
		/// </summary>
		/// <param name="value">value to convert</param>
		/// <param name="targetType">not used</param>
		/// <param name="parameter">not used</param>
		/// <param name="culture">not used</param>
		/// <returns></returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			float pc = (float)value * 100 + 0.5F;
			int intPC = (int)pc;
			return (float)intPC;
		}
		
		/// <summary>
		/// Convert from a percent value to a float
		/// </summary>
		/// <param name="value">value to convert</param>
		/// <param name="targetType">not used</param>
		/// <param name="parameter">not used</param>
		/// <param name="culture">not used</param>
		/// <returns></returns>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if(value.GetType() == typeof(double))
		    {
				return ((double)value) / 100;
		    }
			else
			{
				return ((float)value) / 100;
			}
		}
	}
}
