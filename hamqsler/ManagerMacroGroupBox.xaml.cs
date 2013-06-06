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

namespace hamqsler
{
	/// <summary>
	/// Interaction logic for ManagerMacroGroupBox.xaml
	/// </summary>
	public partial class ManagerMacroGroupBox : MacroGroupBox
	{
		/// <summary>
		/// Default Constructor
		/// </summary>
		public ManagerMacroGroupBox()
		{
			InitializeComponent();
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="parts">TextParts that the ManagerMacro is contained in</param>
		/// <param name="part">ManagerMacro object displayed in this group box</param>
		/// <param name="includeContextMenu">Indicator for whether to include the
		/// context menu. Values are INCLUDECONTEXTMENU and DONOTINCLUDECONTEXTMENU</param>
		public ManagerMacroGroupBox(TextParts parts, TextPart part, bool includeContextMenu)
			: base(parts, part, includeContextMenu)
		{
			InitializeComponent();
			ManagerMacro macro = part as ManagerMacro;
			foreach(TextPart p  in macro.DesignText)
			{
				MacroGroupBox box = p.BuildGroupBox(macro.DesignText, INCLUDECONTENTMENU);
				box.Margin = INSETMARGIN;
				DesignTextPanel.Children.Add(box);
			}
			foreach(TextPart p in macro.NoManagerText)
			{
				MacroGroupBox box = p.BuildGroupBox(macro.NoManagerText, INCLUDECONTENTMENU);
				box.Margin = INSETMARGIN;
				NoManagerTextPanel.Children.Add(box);
			}
		}
	}
}