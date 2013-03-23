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
	/// TextMacrosDialog class - used to display and interact with TextParts
	/// </summary>
	public partial class TextMacrosDialog : Window
	{
		public bool IsDirty {get; set;}
		
		private TextParts partItems;
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="parts">TextParts object that this dialog will display and interact with</param>
		public TextMacrosDialog(TextParts parts)
		{
			InitializeComponent();
			partItems = parts;
			IsDirty = false;
		}
		/// <summary>
		/// Handler for the CloseButton Clicked event 
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void CloseButton_Clicked(object sender, RoutedEventArgs e)
		{
			// all we do is close this dialog
			this.Close();
		}
		
		/// <summary>
		/// Handler for Render event.
		/// </summary>
		/// <param name="drawingContext">DrawingContext (just passed to base OnRender method</param>
		protected override void OnRender(DrawingContext drawingContext)
		{
			// Rebuild all of the Macro displays. This simplifies processing as there is no need to
			// attempt to add expanders in the correct locations in the dialog.
			ContentPanel.Children.RemoveRange(0, ContentPanel.Children.Count);
			bool isDirty = IsDirty;
			foreach(TextPart part in partItems)
			{
				MacroGroupBox box = part.BuildGroupBox(partItems, MacroGroupBox.INCLUDECONTENTMENU);
				ContentPanel.Children.Add(box);
			}
			IsDirty = isDirty;
			// have the base class perform any other rendering needed.
			base.OnRender(drawingContext);
		}
		
		void OnPreviewKeyDown(object sender, KeyEventArgs e)
		{
			IsDirty = true;
		}
		
		/// <summary>
		/// Handles Window Loaded event
		/// All we need to do is set the cursor to Arrow
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void Window_Loaded(object sender, RoutedEventArgs e)
		{
			Mouse.OverrideCursor = Cursors.Arrow;
		}
	}
}