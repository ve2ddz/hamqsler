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
	/// StaticTextViewer class - displays and interacts with a StaticText object
	/// </summary>
	public partial class StaticTextViewer : MacroViewer
	{
		private StaticText sText;
		public StaticText StatText
		{
			get {return sText;}
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="d">TextMacrosDialog that will contain the MacroExpanderBox that will contain
		/// this viewer</param>
		/// <param name="box">MacroExpanderBox that will contain this viewer</param>
		/// <param name="text">StaticText object that this viewer will display and interact with</param>
		public StaticTextViewer(TextMacrosDialog d, MacroExpanderBox box, StaticText text)
			: base(d, box)
		{
			sText = text;
			InitializeComponent();
			this.DataContext = sText;
		}
	}
}