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
	/// MacroExpander class
	/// </summary>
	public partial class MacroExpander : Expander
	{
		private static readonly DependencyProperty PartItemsProperty =
			DependencyProperty.Register("PartItems", typeof(TextParts), typeof(MacroExpander),
			                            new PropertyMetadata(null));
		public TextParts PartItems
		{
			get {return (TextParts)GetValue(PartItemsProperty);}
			set {SetValue(PartItemsProperty, value);}
		}
		
		private static readonly DependencyProperty PartItemProperty =
			DependencyProperty.Register("PartItem", typeof(TextPart), typeof(MacroExpander),
			                            new PropertyMetadata(null));
		public TextPart PartItem
		{
			get {return (TextPart)GetValue(PartItemProperty);}
			set {SetValue(PartItemProperty, value);}
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		public MacroExpander()
		{
			InitializeComponent();
			
		}

		/// <summary>
		/// Create a StaticTextViewExpander
		/// </summary>
		/// <param name="d">TextMacrosDialog object that the created expander will be added to</param>
		/// <param name="box">MacroExpanderBox object that will contain this expander</param>
		/// <param name="label">Text to display in the expander's header</param>
		/// <param name="text"StaticText object whose text will be displayed in this expander></param>
		/// <returns></returns>
		public static Expander CreateStaticTextViewExpander(TextMacrosDialog d, MacroExpanderBox box,
                                                   			string label, StaticText text)
		{
			MacroExpander expander = new MacroExpander();
			expander.Header = label;
			StaticTextViewer viewer = new StaticTextViewer(d, box, text);
			expander.ContentPanel.Children.Add(viewer);
			return expander;
		}
	}
}