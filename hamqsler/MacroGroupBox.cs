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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
namespace hamqsler
{
	/// <summary>
	/// MacroGroupBox class = base class for other MacroGroupBoxes (e.g. StaticTextGroupBox and
	/// AdifMacroGroupBox)
	/// </summary>
	public class MacroGroupBox : GroupBox
	{
		public static RoutedCommand InsertAdifMacroBeforeCommand = new RoutedCommand();
		public static RoutedCommand InsertAdifMacroAfterCommand = new RoutedCommand();
		
		private static readonly DependencyProperty HeaderTextProperty =
			DependencyProperty.Register("HeaderText", typeof(string), typeof(MacroGroupBox),
			                            new PropertyMetadata(string.Empty));
		public string HeaderText
		{
			get {return (string)GetValue(HeaderTextProperty);}
			set {SetValue(HeaderTextProperty, value);}
		}
		
		private static readonly DependencyPropertyKey PartItemsPropertyKey =
			DependencyProperty.RegisterReadOnly("PartItems", typeof(TextParts), typeof(MacroGroupBox),
			                                    new PropertyMetadata(null));
		private static readonly DependencyProperty PartItemsProperty =
			PartItemsPropertyKey.DependencyProperty;
		public TextParts PartItems
		{
			get {return (TextParts)GetValue(PartItemsProperty);}
		}
		
		private static readonly DependencyProperty PartItemProperty =
			DependencyProperty.Register("PartItem", typeof(TextPart), typeof(MacroGroupBox),
			                            new PropertyMetadata(null));
		public TextPart PartItem
		{
			get {return (TextPart)GetValue(PartItemProperty);}
			set {SetValue(PartItemProperty, value);}
		}
		
		// use these values rather than true or false when you want to include the context menu
		public static bool INCLUDECONTENTMENU = true;
		public static bool DONOTINCLUDECONTENTMENU = false;
		
		public static Thickness INSETMARGIN = new Thickness(20, 1, 1, 1);
		
		public MacroGroupBox()
		{
			InitializeGroupBox(DONOTINCLUDECONTENTMENU);
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="parts">TextParts object that the specified TextPart is part of</param>
		/// <param name="part">TextPart object to display</param>
		/// <param name="includeContextMenu">Indicator for whether to include the
		/// context menu. Values are INCLUDECONTEXTMENU and DONOTINCLUDECONTEXTMENU</param>
		public MacroGroupBox(TextParts parts, TextPart part, bool includeContextMenu)
		{
			SetValue(PartItemsPropertyKey, parts);
			PartItem = part;
			InitializeGroupBox(includeContextMenu);
		}
		
		/// <summary>
		/// Initialize the contents of the group box
		/// </summary>
		/// <param name="includeContextMenu">Indicator for whether to include the
		/// context menu. Values are INCLUDECONTEXTMENU and DONOTINCLUDECONTEXTMENU</param>
		private void InitializeGroupBox(bool includeContextMenu)
		{
			// create a Label object to use as the expander Header
			Label header = new Label();
			header.Height = 30;
			header.Content = HeaderText;
			// If ContentMenu wanted, create it and set on Header only!
			// Otherwise, the context menu will be active for all of the group box, not just the
			// header, overriding context menus used for any groupboxes inside this group box.
			if(includeContextMenu == INCLUDECONTENTMENU)
			{
				header.ContextMenu = CreateContextMenu();
				header.Foreground = Brushes.Green;
			}
			this.Header = header;
		}
		
		/// <summary>
		/// Create the context menu for the group box
		/// </summary>
		/// <returns>created context menu object</returns>
		private ContextMenu CreateContextMenu()
		{
			ContextMenu cm = new ContextMenu();
			cm.Items.Add(CreateInsertAdifMacroBeforeMenuItem());
			cm.Items.Add(CreateInsertAdifMacroAfterMenuItem());
			return cm;
		}
		
		/// <summary>
		/// Create the InsertAdifMacroBefore menu item
		/// </summary>
		/// <returns>The menu item</returns>
		private MenuItem CreateInsertAdifMacroBeforeMenuItem()
		{
			MenuItem mi = new MenuItem();
			mi.Header = "Insert Adif Macro Before";
			CommandBinding cb = new CommandBinding(InsertAdifMacroBeforeCommand,
			                                       OnInsertAdifMacroBeforeCommand_Executed,
			                                       OnInsertAdifMacroBeforeCommand_CanExecute);
			this.CommandBindings.Add(cb);
			mi.Command = InsertAdifMacroBeforeCommand;
			mi.CommandTarget = this;
			return mi;			
		}
		
		/// <summary>
		/// Create the InsertAdifMacroAfter menu item
		/// </summary>
		/// <returns>The menu item</returns>
		private MenuItem CreateInsertAdifMacroAfterMenuItem()
		{
			MenuItem mi = new MenuItem();
			mi.Header = "Insert Adif Macro After";
			CommandBinding cb = new CommandBinding(InsertAdifMacroAfterCommand,
			                                       OnInsertAdifMacroAfterCommand_Executed,
			                                       OnInsertAdifMacroAfterCommand_CanExecute);
			this.CommandBindings.Add(cb);
			mi.Command = InsertAdifMacroAfterCommand;
			mi.CommandTarget = this;
			return mi;			
		}
		
		/// <summary>
		/// DependencyPropertyChanged event handler
		/// </summary>
		/// <param name="e">DependencyPropertyChangedEventArgs objecgt for this event</param>
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if(e.Property == HeaderTextProperty)
			{
				// change the text in the expander's header
				Label header = this.Header as Label;
				if(header != null)	// no header when macroExpander is being created
				{
					header.Content = HeaderText;
					header.InvalidateVisual();
				}
			}
		}
		
		/// <summary>
		/// CanExecute event handler for the InsertAdifMacroBefore menu item
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">CanExecuteRoutedEventArgs object for this event</param>
		private void OnInsertAdifMacroBeforeCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
			e.Handled = true;
		}
		
		/// <summary>
		/// CanExecute event handler for the InsertAdifMacroAfter menu item
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">CanExecuteRoutedEventArgs object for this event</param>
		private void OnInsertAdifMacroAfterCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
			e.Handled = true;
		}
		
		/// <summary>
		/// Executed event handler for the InserAdifMacroBefore menu item
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void OnInsertAdifMacroBeforeCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			// create a new AdifMacro object and add it to the TextItems.
			AdifMacro aMacro = new AdifMacro();
			int position = GetPosition();
			if(position != -1)
			{
				PartItems.Insert(position, aMacro);
				UpdateDialog();		// redraw the Dialog contents
			}
			
		}
		
		/// <summary>
		/// Executed event handler for the InserAdifMacroAfter menu item
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void OnInsertAdifMacroAfterCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			// create a new AdifMacro object and add it to the TextItems.
			AdifMacro aMacro = new AdifMacro();
			int position = GetPosition();
			if(position != -1)
			{
				PartItems.Insert(position + 1, aMacro);
				UpdateDialog();		// redraw the Dialog contents
			}
			
		}
		
		/// <summary>
		/// Get the position of the PartItem displayed in this expander, within the PartItems
		/// </summary>
		/// <returns>Index representing the position, or -1 if not found (should always be found)</returns>
		private int GetPosition()
		{
			return PartItems.IndexOf(PartItem);
		}
		
		/// <summary>
		/// Force a redraw of the TextMacrosDialog containing this expander
		/// </summary>
		private void UpdateDialog()
		{
			FrameworkElement elt = (FrameworkElement)this.Parent;
			while (elt.GetType() != typeof(TextMacrosDialog))
			{
				elt = (FrameworkElement)elt.Parent;
			}
			((TextMacrosDialog) elt).InvalidateVisual();
		}
	}
}
