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
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace hamqsler
{
	/// <summary>
	/// Converts date in Adif format to date in 'DD-MMM-YY' format
	/// </summary>
	[ValueConversion(typeof(string), typeof(string))]
	public class AdifToDDMMMYYDateConverter : IValueConverter
	{
		/// <summary>
		/// Converts Adif date to 'DD-MM-YY' date
		/// </summary>
		/// <param name="value">Adif date to convert</param>
		/// <param name="targetType">not used</param>
		/// <param name="parameter">not used</param>
		/// <param name="culture">not used</param>
		/// <returns>Formatted date</returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string date = (string)value;
			if(date.Equals(string.Empty))
			{
				return date;
			}
			if(date.Length == 8)
			{
				Dictionary<string, string> Months = new Dictionary<string, string>();
				BuildMonths(Months);
				return string.Format("{0}-{1}-{2}", date.Substring(6, 2), Months[date.Substring(4, 2)],
				                     date.Substring(2, 2));
			}
			else
			{
				return "Data Error";
			}
		}
		
		/// <summary>
		/// Helper method that builds a dictionary of month strings based on the month number
		/// </summary>
		/// <param name="Months">The Dictionary to store month values in</param>
		private void BuildMonths(Dictionary<string, string> Months)
		{
			UserPreferences prefs = ((App)Application.Current).UserPreferences;
			Months["01"] = prefs.JanuaryText;
			Months["02"] = prefs.FebruaryText;
			Months["03"] = prefs.MarchText;
			Months["04"] = prefs.AprilText;
			Months["05"] = prefs.MayText;
			Months["06"] = prefs.JuneText;
			Months["07"] = prefs.JulyText;
			Months["08"] = prefs.AugustText;
			Months["09"] = prefs.SeptemberText;
			Months["10"] = prefs.OctoberText;
			Months["11"] = prefs.NovemberText;
			Months["12"] = prefs.DecemberText;
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
