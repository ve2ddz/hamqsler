﻿/*
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
	/// Interaction logic for AdifMacroExpander.xaml
	/// </summary>
	public partial class AdifMacroExpander : MacroExpander
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public AdifMacroExpander() : base()
		{
			InitializeComponent();
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="parts">TextParts that the AdifMacro is contained in</param>
		/// <param name="part">AdifMacro object displayed in this expander</param>
		/// <param name="includeContextMenu">Indicator for whether to include the
		/// context menu. Values are INCLUDECONTEXTMENU and DONOTINCLUDECONTEXTMENU</param>
		public AdifMacroExpander(TextParts parts, TextPart part, bool includeContextMenu)
			: base(parts, part, includeContextMenu)
		{
			InitializeComponent();
			AdifMacro macro = part as AdifMacro;
			SeparateCheckBox.DataContext = macro;
			AdifField.DataContext = macro.AdifField;
			foreach(TextPart p  in macro.DesignText)
			{
				MacroExpander expander = p.BuildExpander(macro.DesignText, INCLUDECONTENTMENU);
				expander.Margin = INSETMARGIN;
				DesignTextPanel.Children.Add(expander);
			}
			foreach(TextPart p in macro.NoFieldText)
			{
				MacroExpander expander = p.BuildExpander(macro.NoFieldText, INCLUDECONTENTMENU);
				expander.Margin = INSETMARGIN;
				NoFieldTextPanel.Children.Add(expander);
			}
		}
	}
}