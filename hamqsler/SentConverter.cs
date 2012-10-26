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
using System.Windows.Data;

namespace hamqsler
{
	/// <summary>
	/// Converter for Sent property
	/// </summary>
	[ValueConversion(typeof(string), typeof(string))]
	public class SentConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter,
		                      CultureInfo culture)
		{
			string sentValue = (string)value;
			string retValue = string.Empty;
			switch(sentValue)
			{
				case "Y":
					retValue = "Yes";
					break;
				case "N":
					retValue = "No";
					break;
				case "R":
					retValue = "Requested";
					break;
				case "Q":
					retValue = "Queued";
					break;
				case "I":
					retValue = "Ignore";
					break;
				case "":
					retValue = string.Empty;
					break;
				default:
					retValue = "***";
					break;
			}
			return retValue;
		}

		public object ConvertBack(object value, Type targetType, object parameter,
		                      CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
