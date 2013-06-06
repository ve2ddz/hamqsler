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
	/// Interaction logic for AdifExistsMacroGroupBox.xaml
	/// </summary>
	public partial class AdifExistsMacroGroupBox : MacroGroupBox
	{
		public AdifExistsMacroGroupBox() : base()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="parts">TextParts that the AdifExistsMacro is contained in</param>
		/// <param name="part">AdifExistsMacro object displayed in this group box</param>
		/// <param name="includeContextMenu">Indicator for whether to include the
		/// context menu. Values are INCLUDECONTEXTMENU and DONOTINCLUDECONTEXTMENU</param>
		public AdifExistsMacroGroupBox(TextParts parts, TextPart part, bool includeContextMenu)
			: base(parts, part, includeContextMenu)
		{
			InitializeComponent();
			AdifExistsMacro macro = part as AdifExistsMacro;
			SeparateCheckBox.DataContext = macro;
			AdifField.DataContext = macro.AdifField;
			foreach(TextPart p  in macro.DesignText)
			{
				MacroGroupBox box = p.BuildGroupBox(macro.DesignText, INCLUDECONTENTMENU);
				box.Margin = INSETMARGIN;
				DesignTextPanel.Children.Add(box);
			}
			foreach(TextPart p in macro.ExistsText)
			{
				MacroGroupBox box = p.BuildGroupBox(macro.ExistsText, INCLUDECONTENTMENU);
				box.Margin = INSETMARGIN;
				ExistsTextPanel.Children.Add(box);
			}
			foreach(TextPart p in macro.DoesntExistText)
			{
				MacroGroupBox box = p.BuildGroupBox(macro.DoesntExistText, INCLUDECONTENTMENU);
				box.Margin = INSETMARGIN;
				DoesntExistTextPanel.Children.Add(box);
			}
		}
	}
}