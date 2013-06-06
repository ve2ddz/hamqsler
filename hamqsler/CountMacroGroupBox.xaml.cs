/*
 *  Author:
 *       Jim Orcheson <jimorcheson@gmail.com>
 * 
 *  Copyright (c) 2012, 2013 Jim Orcheson
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
using System.Windows;
using System.Windows.Controls;

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
		
		/// <summary>
		/// Handle click on one of the count condition radio buttons
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void Condition_Click(object sender, RoutedEventArgs e)
		{
			// all we do is set TextMacrosDialog.IsDirty
			SetIsDirty();		
		}

		/// <summary>
		/// Helper method that sets TextMacrosDialog.IsDirty
		/// </summary>
		private void SetIsDirty()
		{
			FrameworkElement elt = (FrameworkElement)this.Parent;
			// make sure that the TextMacrosDialog is displayed
			if (elt != null) {
				while (elt.GetType() != typeof(TextMacrosDialog)) {
					elt = (FrameworkElement)elt.Parent;
				}
				// now we have the TextMacrosDialog, so set IsDirty
				((TextMacrosDialog)elt).IsDirty = true;
			}
		}
		
		/// <summary>
		/// Handle Selection changed event for the CountMacroBox
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">SelectionChangedEventArgs object</param>
		private void CountComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			// e.RemovedItems empty when set during initial display
			// and contains old value when a new value is selected.
			// We want to set TextMacrosDialog.IsDirty only when user selects a new value
			if(e.RemovedItems.Count != 0)
			{
				SetIsDirty();
			}
		}
	}
}