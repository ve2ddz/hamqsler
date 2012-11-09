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
	/// Converter for FontWeight to a string representation of same
	/// </summary>
	[ValueConversion(typeof(FontWeight), typeof(string))]
	public class FontWeightConverter : IValueConverter
	{
		/// <summary>
		/// Convert a FontWeight to string representation. Only Bold, Black, and Normal are supported
		/// </summary>
		/// <param name="value">FontWeight to convert</param>
		/// <param name="targetType">not used</param>
		/// <param name="parameter">not used</param>
		/// <param name="culture">not used</param>
		/// <returns>string representation of the input FontWeight</returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			FontWeight fw = (FontWeight)value;
			if(fw == FontWeights.Bold)
				return "Bold";
			else if(fw == FontWeights.Black)
				return "Black";
			else
				return "Normal";
		}
		
		/// <summary>
		/// Convert s string representation of FontWeight back into a FontWeight. Only Bold,
		/// Black and Normal are supported.
		/// </summary>
		/// <param name="value">string to convert</param>
		/// <param name="targetType">not used</param>
		/// <param name="parameter">not used</param>
		/// <param name="culture">not used</param>
		/// <returns>FontWeight that corresponds to the input string</returns>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string weight = (string)value;
			if(weight.Equals("Bold"))
				return FontWeights.Bold;
			else if(weight.Equals("Black"))
				return FontWeights.Black;
			else
				return FontWeights.Normal;
		}
	}
}
