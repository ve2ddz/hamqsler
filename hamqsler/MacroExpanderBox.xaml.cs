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
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace hamqsler
{
	/// <summary>
	/// MacroExpanderBox class
	/// </summary>
	public partial class MacroExpanderBox : GroupBox
	{
		private TextMacrosDialog dialog;
		
		private const int MAXHEADERLENGTH = 15;
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="d">TextMacrosDialog object that will contain this MacroExpanderBox</param>
		/// <param name="parts">TextItems that will be displayed in this MacroExpanderBox</param>
		public MacroExpanderBox(TextMacrosDialog d, TextParts parts)
		{
			InitializeComponent();
			dialog = d;
			foreach(TextPart part in parts)
			{
				if(part.GetType() == typeof(StaticText))
				{
					CreateTextView((StaticText)part);
				}
			}
		}
		
		/// <summary>
		/// Helper method that creates a StaticTextViewExpander
		/// </summary>
		/// <param name="text">StaticText object that will be displayed in the expander</param>
		private void CreateTextView(StaticText text)
		{
			string label = text.GetText(true);
			Expander expander = MacroExpander.CreateStaticTextViewExpander(
				dialog, this, "Static Text", text);
			expander.IsExpanded = true;
			ContentPanel.Children.Add(expander);
		}
	}
}