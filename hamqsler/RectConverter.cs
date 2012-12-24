/*
 *  Author:
 *       Jim Orcheson <jimorcheson@gmail.com>
 * 
 *  Copyright (c) 2012 Jim Orcheson
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
using System.Windows;
using System.Windows.Data;

namespace hamqsler
{
	/// <summary>
	/// Creates a Rect structure from 4 input values
	/// </summary>
	public class RectConverter : IMultiValueConverter
	{
		/// <summary>
		/// Convert four input values to a Rect structure
		/// </summary>
		/// <param name="values">The four input values</param>
		/// <param name="targetType">not used</param>
		/// <param name="parameter">not used</param>
		/// <param name="culture">not used</param>
		/// <returns></returns>
		public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
		{
			if(values[2] == DependencyProperty.UnsetValue)
			{
				values[2] = 0.0;
			}
			if(values[3] == DependencyProperty.UnsetValue)
			{
				values[3] = 0.0;
			}
			return new Rect((double)values[0], (double)values[1], (double)values[2], (double)values[3]);
		}
		
		/// <summary>
		/// Not implemented (not needed)
		/// </summary>
		/// <param name="value">not used</param>
		/// <param name="targetTypes">not used</param>
		/// <param name="parameter">not used</param>
		/// <param name="culture">not used</param>
		/// <returns></returns>
		public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
	}
}
