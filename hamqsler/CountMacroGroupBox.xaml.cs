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
	/// Interaction logic for CountMacroGroupBox.xaml
	/// </summary>
	public partial class CountMacroGroupBox : MacroGroupBox
	{
		public CountMacroGroupBox()
		{
			InitializeComponent();
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="parts">TextParts that the CountMacro is contained in</param>
		/// <param name="part">CountMacro object displayed in this group box</param>
		/// <param name="includeContextMenu">Indicator for whether to include the
		/// context menu. Values are INCLUDECONTEXTMENU and DONOTINCLUDECONTEXTMENU</param>
		public CountMacroGroupBox(TextParts parts, TextPart part, bool includeContextMenu)
			: base(parts, part, includeContextMenu)
		{
			InitializeComponent();
			CountMacro macro = part as CountMacro;
			this.DataContext = macro;
			foreach(TextPart p  in macro.DesignText)
			{
				MacroGroupBox box = p.BuildGroupBox(macro.DesignText, INCLUDECONTENTMENU);
				box.Margin = INSETMARGIN;
				DesignTextPanel.Children.Add(box);
			}
			foreach(TextPart p in macro.TrueText)
			{
				MacroGroupBox box = p.BuildGroupBox(macro.TrueText, INCLUDECONTENTMENU);
				box.Margin = INSETMARGIN;
				TrueTextPanel.Children.Add(box);
			}
			foreach(TextPart p in macro.FalseText)
			{
				MacroGroupBox box = p.BuildGroupBox(macro.FalseText, INCLUDECONTENTMENU);
				box.Margin = INSETMARGIN;
				FalseTextPanel.Children.Add(box);
			}
		}
		
		private void Condition_Click(object sender, RoutedEventArgs e)
		{
			SetIsDirty();		
		}

		private void SetIsDirty()
		{
			FrameworkElement elt = (FrameworkElement)this.Parent;
			if (elt != null) {
				while (elt.GetType() != typeof(TextMacrosDialog)) {
					elt = (FrameworkElement)elt.Parent;
				}
				((TextMacrosDialog)elt).IsDirty = true;
			}
		}
		
		private void CountComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if(e.RemovedItems.Count != 0)
			{
				SetIsDirty();
			}
		}
	}
}