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
	/// Converts date in Adif format to date in 'DD-MM-YY' format
	/// </summary>
	[ValueConversion(typeof(string), typeof(string))]
	public class AdifToDDMMYYDateConverter : IValueConverter
	{
		/// <summary>
		/// Converts Adif date to date in 'DD-MM-YY' format
		/// </summary>
		/// <param name="value">Date to convert</param>
		/// <param name="targetType">Not used</param>
		/// <param name="parameter">Not used</param>
		/// <param name="culture">Not used</param>
		/// <returns></returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string date = (string)value;
			if(date.Equals(string.Empty))
			{
				return date;
			}
			if(date.Length == 8)
			{
				return string.Format("{0}-{1}-{2}", date.Substring(6, 2), date.Substring(4, 2),
				                     date.Substring(2, 2));
			}
			else
			{
				return "Data Error";
			}
		}
		
		/// <summary>
		/// Convert display date back to Adif date.
		/// Not used.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="targetType"></param>
		/// <param name="parameter"></param>
		/// <param name="culture"></param>
		/// <returns></returns>
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
