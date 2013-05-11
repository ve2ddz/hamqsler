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
using System.Drawing;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace hamqsler
{
	/// <summary>
	/// FormsBrushToWPFSolidColorBrushConverter converts between System.Drawing.Brush and
	/// System.Windows.Media.SolidColorBrush.
	/// </summary>
	[ValueConversion(typeof(System.Drawing.Brush), typeof(System.Windows.Media.SolidColorBrush))]
	public class FormsBrushToWPFSolidColorBrushConverter : IValueConverter
	{
		/// <summary>
		/// Convert a System.Drawing.Brush value to System.Windows.Media.SolidColorBrush.
		/// </summary>
		/// <param name="value">Brushto convert</param>
		/// <param name="targetType">not used</param>
		/// <param name="parameter">not used</param>
		/// <param name="culture">not used</param>
		/// <returns>System.Windows.Media.SolidColorBrush object</returns>
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			System.Drawing.SolidBrush winFormsBrush = value as System.Drawing.SolidBrush;
			System.Drawing.Color color = winFormsBrush.Color;
			System.Windows.Media.SolidColorBrush brush = 
				new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(
					color.A, color.R, color.G, color.B));
			return brush;
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
			System.Windows.Media.SolidColorBrush wpfBrush = 
				value as System.Windows.Media.SolidColorBrush;
			System.Windows.Media.Color wpfColor = wpfBrush.Color;
			return new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(
				255, wpfColor.R, wpfColor.G, wpfColor.B));
		}

	}
}
