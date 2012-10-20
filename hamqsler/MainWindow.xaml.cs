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
using Microsoft.Win32;
using Qsos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace hamqsler
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		
		public delegate string AddOrImportDelegate(string fName, QSOsView.OrderOfSort so);

		public static RoutedCommand ImportQsosCommand = new RoutedCommand();
		public static RoutedCommand AddQsosCommand = new RoutedCommand();
		public static RoutedCommand UserPreferencesCommand = new RoutedCommand();
		
		public MainWindow()
		{
			InitializeComponent();
		}
		
		
		private void AddQsosCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = qsosView.DisplayQsos.Count > 0;
		}
		
		/// <summary>
		/// Shutdown the program when MainWindow closes
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void Window_Closed(object sender, EventArgs e)
		{
			// Force program shutdown (required because App shutdown mode set to OnExplicitShutdown)
			Application.Current.Shutdown();
		}
		
		private void UserPreferencesCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			UserPreferencesDialog userPrefsDialog = new UserPreferencesDialog();
			userPrefsDialog.ShowDialog();
		}
		
		/// <summary>
		/// Handles Import Qsos menu item processing
		/// </summary>
		/// <param name="sender">not used </param>
		/// <param name="e">not used</param>
		public void ImportQsosCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ImportOrAddQsos(qsosView.DisplayQsos.ImportQsos);		
		}
		
		
		/// <summary>
		/// Handles Add Qsos menu item processing
		/// </summary>
		/// <param name="sender">not used </param>
		/// <param name="e">not used</param>
		public void AddQsosCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ImportOrAddQsos(qsosView.DisplayQsos.AddQsos);
		}
		
		/// <summary>
		/// Handles importing or adding QSOs from an ADIF file. This method eliminates
		/// the duplication of code in ImportQsosCommand_Executed and AddQsosCommand_Executed
		/// whose only difference is the DisplayQsos method that is called.
		/// </summary>
		/// <param name="importOrAdd">delegated method to call (in DisplayQsos)</param>
		private void ImportOrAddQsos(AddOrImportDelegate importOrAdd)
		{
			// create and show an OpenFileDialog to get the ADIF file to load
			OpenFileDialog openDialog = new OpenFileDialog();
			openDialog.InitialDirectory = ((App)Application.Current).UserPreferences.DefaultAdifFilesFolder;
			openDialog.Multiselect = false;
			openDialog.Filter = "Adif Files (*.adi)|*.adi";
			openDialog.CheckFileExists = true;
			if(openDialog.ShowDialog() == true)
			{
				// get the file name
				string adifFileName = openDialog.FileName;
				try
				{
					// import the QSOs from the ADIF file
					string error = importOrAdd(adifFileName, qsosView.SortOrder);
					if(error != null)
					{
						MessageBox.Show(error, "Import Error", MessageBoxButton.OK,
						                MessageBoxImage.Warning);
					}
				}
				catch(Exception ex)
				{
					App.Logger.Log(ex);
					return;
				}
			}
		}

	}
}