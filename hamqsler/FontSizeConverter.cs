/*
 *  Author:
 *       Jim Orcheson <jimorcheson@gmail.com>
 * 
 *  Copyright (c) 2012, 2013 Jim Orcheson
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
	/// Converts double to string values for FontSize
	/// </summary>
	[ValueConversion(typeof(object), typeof(string))]
	public class FontSizeConverter : IValueConverter
	{
		/// <summary>
		/// Convert a FontSize value to string representation.
		/// </summary>
		/// <param name="value">FontSize to convert</param>
		/// <param name="targetType">not used</param>
		/// <param name="parameter">not used</param>
		/// <param name="culture">not used</param>
		/// <returns>string representation of the input FontWeight</returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			float size = (float)value;
			return string.Format(CultureInfo.InvariantCulture, "{0:F1}", size);
		}
		
		/// <summary>
		/// Converts string representation of FontSize back into a double. 
		/// </summary>
		/// <param name="value">string to convert</param>
		/// <param name="targetType">not used</param>
		/// <param name="parameter">not used</param>
		/// <param name="culture">not used</param>
		/// <returns>FontWeight that corresponds to the input string</returns>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string size = (string)value;
			size = size.Replace(",", ".");
			if(value.Equals(string.Empty))
			{
				return 1;
			}
			float val;
			if(Single.TryParse(size, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture,
			                   out val))
			{
				return val;
			}
			else
			{
				return 1;
			}
			   
		}
	}
}
