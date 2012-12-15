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
	/// Converts Adif Qsl_Rcvd values to Pse, Tnx, or empty string for display in the Qsl
	/// column in QsosBoxView.
	/// </summary>
	[ValueConversion(typeof(string), typeof(string))]
	public class QslRcvdToPseTnxConverter : IValueConverter
	{
		/// <summary>
		/// Converts Adif Qsl_Rcvd status to display string
		/// </summary>
		/// <param name="value">Status to convert</param>
		/// <param name="targetType">Not used</param>
		/// <param name="parameter">Not used</param>
		/// <param name="culture">Not used</param>
		/// <returns></returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string qslRcvd = (string)value;
			switch(qslRcvd)
			{
				case "Y":
					return "Tnx";
				case "N":
				case "R":
					return "Pse";
				default:
					return string.Empty;
			}
		}
		
		/// <summary>
		/// Converts Qsl string back to Adif Qsl_Rcvd status.
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
