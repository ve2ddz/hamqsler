/*
 *  Author:
 *       Jim Orcheson <jimorcheson@gmail.com>
 * 
 *  Copyright (c) 2013 Jim Orcheson
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
	/// Interaction logic for NewOneSidedCardDialog.xaml
	/// </summary>
	public partial class NewOneSidedCardDialog : Window
	{
		public Size CardSize
		{
			get {return CalculateCardSize();}
		}
		
		public NewOneSidedCardDialog()
		{
			InitializeComponent();
		}
		
		/// <summary>
		/// Handler for OK button click event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void OK_Click(object sender, RoutedEventArgs e)
		{
            DialogResult = true;
            this.Close();
		}
		
		/// <summary>
		/// Determine card size based on card size and orientation radio buttons
		/// </summary>
		/// <returns>Size of the card</returns>
		private Size CalculateCardSize()
		{
			int width = 0;
			int height = 0;
			if((bool)BureauButton.IsChecked)
			{
				width = (bool)LandscapeOrientation.IsChecked ? CardWF.BureauCardWide : CardWF.BureauCardHigh;
				height = (bool)LandscapeOrientation.IsChecked ? CardWF.BureauCardHigh : CardWF.BureauCardWide;
			}
			else if((bool)FiveButton.IsChecked)
			{
				width = (bool)LandscapeOrientation.IsChecked ? CardWF.FiveCardWide : CardWF.FiveCardHigh;
				height = (bool)LandscapeOrientation.IsChecked ? CardWF.FiveCardHigh : CardWF.FiveCardWide;
			}
			else if((bool)SixButton.IsChecked)
			{
				width = (bool)LandscapeOrientation.IsChecked ? CardWF.SixCardWide : CardWF.SixCardHigh;
				height = (bool)LandscapeOrientation.IsChecked ? CardWF.SixCardHigh : CardWF.SixCardWide;
			}
			return new Size(width, height);
		}
	}
}