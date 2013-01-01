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
using System.Windows.Controls;

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
		/// <param name="card">Qsl card that is being displayed/printed</param>
		/// <param name="qsos">Qsos to print on the card</param>
		/// <param name='screen'>
		/// not used
		/// </param>
		public override string GetText(Card card, List<DispQso> qsos, bool screen)
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
		
		/// <summary>
		/// Builds a group box to display the StaticTextViewer for this StaticText object
		/// </summary>
		/// <param name="parts">TextParts object that this StaticText object is contained in</param>
		/// <param name="includeContentMenu">Boolean to indicate whether the ContextMenu should be
		/// displayable or not. Use the values defined in MacroExpander.cs rather than true/false.</param>
		/// <returns>MacroExpander object containing the StaticTextViewer for this StaticText object</returns>
		public override MacroGroupBox BuildGroupBox(TextParts parts, bool includeContextMenu)
		{
			StaticTextGroupBox box = new StaticTextGroupBox(parts, this, includeContextMenu);
			box.DataContext = this;
			return box;
		}
		
		/// <summary>
		/// Do nothing. This method is needed because TextPart.RemoveExtraneousStaticTextMacros
		/// is abstract.
		/// </summary>
		public override void RemoveExtraneousStaticTextMacros()
		{
			// do nothing for StaticText objects
		}
		
		/// <summary>
		/// Make a deep copy of this StaticText object
		/// </summary>
		/// <returns>StaticText object that is a deep copy of this one</returns>
		public override TextPart Clone()
		{
			StaticText sText = new StaticText();
			sText.Text = Text;
			return sText;
		}
	}
}