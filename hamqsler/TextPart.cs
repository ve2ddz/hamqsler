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
	/// Interface for all TextPart objects
	/// </summary>
	public abstract class TextPart : UIElement
	{
		/// <summary>
		/// Retrieve the text associated with this TextPart
		/// </summary>
		/// <param name="screen">Indicates whether the text will be displayed on screen, or printed</param>
		/// <returns>the text</returns>
		public abstract string GetText(bool screen);
		
		/// <summary>
		/// Gets the adif fields for sorting.
		/// </summary>
		/// <param name='fields'>
		/// HashSet of fieldnames for sorting
		/// </param>
		/// <param name='existFields'>
		/// HashSet of exists fieldnames for sorting (sorting done on whether these ADIF fields exist or not
		/// </param>
		public abstract void GetAdifFieldsForSorting(ref HashSet<string>fields, 
											 ref HashSet<string> existFields);

		/// <summary>
		/// Create the group box that will display this TextPart
		/// </summary>
		/// <param name="parts">TextParts object containing this TextPart</param>
		/// <param name="includeContentMenu">Indicator for whether to include the
		/// context menu. Values are INCLUDECONTEXTMENU and DONOTINCLUDECONTEXTMENU</param>
		/// <returns></returns>
		public abstract MacroGroupBox BuildGroupBox(TextParts parts, bool includeContentMenu);
	}
}
