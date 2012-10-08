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
	/// Interaction logic for UserPreferencesDialog.xaml
	/// </summary>
	public partial class UserPreferencesDialog : Window
	{
		private UserPreferences userPrefs;
		
		public UserPreferencesDialog()
		{
			InitializeComponent();

			// create a clone of the UserPreferences object
			userPrefs = new UserPreferences(((App)Application.Current).UserPreferences);
			propertiesDisplay.DataContext = userPrefs;
		}
		
		void OkButton_Click(object sender, RoutedEventArgs e)
		{
			userPrefs.SerializeAsXml();
			((App)Application.Current).UserPreferences = userPrefs;
			e.Handled = true;
			this.Close();
		}
		
		void ApplyButton_Click(object sender, RoutedEventArgs e)
		{
			((App)Application.Current).UserPreferences = userPrefs;
			e.Handled = true;
		}
		
		void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
		
		/// <summary>
		/// Handler for DefaultAdifFilesFolderButton click events.
		/// Allows user to select a default folder to contain the ADIF files
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">RoutedEventArgs object</param>
		void DefaultAdifFilesFolderButton_Click(object sender, RoutedEventArgs e)
		{
			//open and display FolderBrowserDialog
			System.Windows.Forms.FolderBrowserDialog folderDialog = 
					new System.Windows.Forms.FolderBrowserDialog();
			folderDialog.Description = "Select the default folder for ADIF files";
			folderDialog.SelectedPath = userPrefs.DefaultAdifFilesFolder;
			
			System.Windows.Forms.DialogResult res = folderDialog.ShowDialog();
			if(res == System.Windows.Forms.DialogResult.OK)
			{
				// new folder selected, so update UserPreferences object
				userPrefs.DefaultAdifFilesFolder = folderDialog.SelectedPath;
			}
			e.Handled = true;
		}
		
		/// <summary>
		/// Handler for DefaultCardFilesFolderButton click events.
		/// Allows user to select a default folder to contain the card files
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">RoutedEventArgs object</param>
		void DefaultCardFilesFolderButton_Click(object sender, RoutedEventArgs e)
		{
			//open and display FolderBrowserDialog
			System.Windows.Forms.FolderBrowserDialog folderDialog = 
					new System.Windows.Forms.FolderBrowserDialog();
			folderDialog.Description = "Select the default folder for card files";
			folderDialog.SelectedPath = userPrefs.DefaultCardFilesFolder;
			
			System.Windows.Forms.DialogResult res = folderDialog.ShowDialog();
			if(res == System.Windows.Forms.DialogResult.OK)
			{
				// new folder selected, so update UserPreferences object
				userPrefs.DefaultCardFilesFolder = folderDialog.SelectedPath;
			}
			e.Handled = true;
		}
		
	}
}