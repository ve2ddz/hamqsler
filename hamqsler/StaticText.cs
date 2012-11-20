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
using System.Collections;
using System.Collections.Generic;
using System.Windows;

namespace hamqsler
{
	/// <summary>
	/// Static text class - used for text (non-macro) portions of text items and text in general
	/// </summary>
	[Serializable]
	public class StaticText : TextPart
	{
		private static readonly DependencyProperty TextProperty = 
			DependencyProperty.Register("Text", typeof(string), typeof(StaticText),
			                            new PropertyMetadata(string.Empty));
		public string Text
		{
			get {return (string)GetValue(TextProperty);}
			set {SetValue(TextProperty, value);}
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		public StaticText ()
		{
			this.Text = string.Empty;
		}
		
		/// <summary>
		/// Gets the text in this StaticText object.
		/// </summary>
		/// <returns>
		/// The text
		/// </returns>
		/// <param name='screen'>
		/// not used
		/// </param>
		public override string GetText(bool screen)
		{
			return Text;
		}

			/// <summary>
		/// Gets the adif fields for sorting.
		/// </summary>
		/// <param name='fields'>
		/// HashSet of fieldnames for sorting
		/// </param>
		/// <param name='existFields'>
		/// HashSet of exists fieldnames for sorting (sorting done on whether these ADIF fields exist or not
		/// </param>
		public override void GetAdifFieldsForSorting(ref HashSet<string> fields, ref HashSet<string> existFields)
		{
			// don't do anything
		}
	}
}