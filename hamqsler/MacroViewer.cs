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
	/// Abstract MacroViewer class = this is used as the parent for all macro viewers
	/// </summary>
	public abstract class MacroViewer : UserControl
	{
		protected TextMacrosDialog dialog;
		protected MacroExpanderBox expanderBox;
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="d">TextMacrosDialog object that will contain this MacroExpanderBox
		/// containing this MacroViewer</param>
		/// <param name="box">MacroExpanderBox object that will contain this MacroViewer</param>
		public MacroViewer(TextMacrosDialog d, MacroExpanderBox box)
		{
			dialog = d;
			expanderBox = box;
		}
		
	}
}
