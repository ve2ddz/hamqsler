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

		public static RoutedCommand InputQsosCommand = new RoutedCommand();
		public static RoutedCommand ImportQsosCommand = new RoutedCommand();
		public static RoutedCommand AddQsosCommand = new RoutedCommand();
		public static RoutedCommand ClearQsosCommand = new RoutedCommand();
		public static RoutedCommand ExportQsosCommand = new RoutedCommand();
		public static RoutedCommand IncludeAllQsosCommand = new RoutedCommand();
		public static RoutedCommand ExcludeAllQsosCommand = new RoutedCommand();
		public static RoutedCommand UserPreferencesCommand = new RoutedCommand();
		
		public MainWindow()
		{
			InitializeComponent();
		}
		
		/// <summary>
		/// CanExecute routine for Add Qsos menu item
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void AddQsosCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = qsosView.DisplayQsos.Count > 0;
		}
		
		/// <summary>
		/// CanExecute routine for Clear Qsos menu item
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void ClearQsosCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = qsosView.DisplayQsos.Count > 0;
		}
		
		/// <summary>
		/// CanExecute routine for Export Qsos menu item
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void ExportQsosCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = qsosView.DisplayQsos.Count > 0;
		}
		
		/// <summary>
		/// CanExecute routine for Include All Qsos menu item
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void IncludeAllQsosCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = qsosView.DisplayQsos.Count > 0;
		}
		
		/// <summary>
		/// CanExecute routine for Exclude All Qsos menu item
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void ExcludeAllQsosCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
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
		
		/// <summary>
		/// Handles Clear Qsos menu item processing
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ClearQsosCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			qsosView.DisplayQsos.Clear();
		}
		
		/// <summary>
		/// Handles Export Qsos menu item processing
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void ExportQsosCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			// get QSOs as Adif stream in bytes
			Byte[] asciiAdif = qsosView.DisplayQsos.GetQsosAsAdif2();
			// determine file to store in
			SaveFileDialog saveDialog = new SaveFileDialog();
			saveDialog.InitialDirectory = 
				((App)Application.Current).UserPreferences.DefaultAdifFilesFolder;
			saveDialog.Filter = "Adif files (*.adi) | *.adi";
			saveDialog.Title = "Select File to Save QSOs to";
			if(saveDialog.ShowDialog() == true)
			{
				string fileName = saveDialog.FileName;
				// make sure filename ends in .adi
				if(fileName.Substring(fileName.Length-4).ToLower() != ".adi")
				{
					fileName += ".adi";
				}
				if(File.Exists(fileName))
				{
					File.Delete(fileName);
				}
				// write stream to file
				StreamWriter writer = File.CreateText(fileName);
				foreach(byte b in asciiAdif)
				{
					writer.Write(Convert.ToChar(b).ToString());
				}
				writer.Close();
			}
		}
		
		/// <summary>
		/// Handles Include All Qsos menu item processing
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void IncludeAllQsosCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			qsosView.DisplayQsos.IncludeAllQsos();
		}
		
		/// <summary>
		/// Handles Exclude All Qsos menu item processing
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void ExcludeAllQsosCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			qsosView.DisplayQsos.ExcludeAllQsos();
		}
		
		/// <summary>
		/// Handles Input Qsos menu item processing
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void InputQsosCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			QsoInputDialog qsoDialog = new QsoInputDialog(qsosView.DisplayQsos);
			qsoDialog.ShowDialog();
			Comparer<QsoWithInclude> comparer = qsosView.GetComparer();
			qsosView.DisplayQsos.SortQSOs(comparer);
		}
		
	}
}