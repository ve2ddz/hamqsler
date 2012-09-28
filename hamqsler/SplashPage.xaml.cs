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
	/// Interaction logic for SplashPage.xaml
	/// SplashPage is displayed when App is started.
	/// It performs many initialization tasks, such as loading and
	/// checking the UserPreferences,
	/// and checking if a new version of the program is available.
	/// </summary>
	public partial class SplashPage : Window
	{
		public SplashPage()
		{
			InitializeComponent();
		}
		
		/// <summary>
		/// Called when SplashPage is closing to display MainWindow
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void Window_Closing(object sender, EventArgs e)
		{
			App app = (App)Application.Current;
			app.ShowMainWindow();
		}
		
		/// <summary>
		/// Perform all startup functions such as:
		/// Load or create UserPreferences, and
		/// check if new version of program is available.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Window_ContentRendered(object sender, EventArgs e)
		{
			// TODO: Add program startup functions here
		}
	}
}