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
using System.Windows;
using System.Windows.Controls;

namespace hamqsler
{
	/// <summary>
	/// MacroExpander is the base class for all other MacroExpanders (e.g. AdifMacroExpander,
	/// CountMacroExpander, etc.)
	/// </summary>
	public class MacroExpander : Expander
	{
		private static readonly DependencyProperty HeaderTextProperty =
			DependencyProperty.Register("HeaderText", typeof(string), typeof(MacroExpander),
			                            new PropertyMetadata(string.Empty));
		public string HeaderText
		{
			get {return (string)GetValue(HeaderTextProperty);}
			set {SetValue(HeaderTextProperty, value);}
		}
		
		// use these values rather than true or false when you want to include the context menu
		public static bool INCLUDECONTENTMENU = true;
		public static bool DONOTINCLUDECONTENTMENU = false;
		
		/// <summary>
		/// Default constructor
		/// </summary>
		public MacroExpander()
		{
			InitializeExpander(DONOTINCLUDECONTENTMENU);
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="headerText">Text to place in the expander's header</param>
		/// <param name="includeContentMenu">Indicator for whether to include the
		/// context menu. Values are INCLUDECONTEXTMENU and DONOTINCLUDECONTEXTMENU</param>
		public MacroExpander(string headerText, bool includeContextMenu)
		{
			HeaderText = headerText;
			InitializeExpander(includeContextMenu);
		}
		
		/// <summary>
		/// Initialize the contents of the expander
		/// </summary>
		/// <param name="includeContextMenu">Indicator for whether to include the
		/// context menu. Values are INCLUDECONTEXTMENU and DONOTINCLUDECONTEXTMENU</param>
		private void InitializeExpander(bool includeContextMenu)
		{
			// add StackPanel to the contents of the expander so that various controls may
			// be added to the expander
			StackPanel panel = new StackPanel();
			this.Content = panel;
			// create a Label object to use as the expander Header
			Label header = new Label();
			header.Content = HeaderText;
			// If ContentMenu wanted, create it and set on Header only!
			// Otherwise, the context menu will be active for all of the expander, not just the
			// header, overriding context menus used for any expanders inside this expander.
			if(includeContextMenu == INCLUDECONTENTMENU)
			{
				header.ContextMenu = CreateContextMenu();
			}
			this.Header = header;
		}
		
		/// <summary>
		/// Create the context menu for the expander
		/// </summary>
		/// <returns>created context menu object</returns>
		private ContextMenu CreateContextMenu()
		{
			ContextMenu cm = new ContextMenu();
			MenuItem mi = new MenuItem();
			mi.Header = "Dummy Menu Item";
			cm.Items.Add(mi);
			return cm;
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
	}
}
