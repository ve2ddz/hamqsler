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
using System.Windows.Shapes;
using System.Windows.Threading;

namespace hamqsler
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		
		public delegate string AddOrImportDelegate(string fName, QSOsView.OrderOfSort so);

		public static RoutedCommand QsosCommand = new RoutedCommand();
		public static RoutedCommand InputQsosCommand = new RoutedCommand();
		public static RoutedCommand ImportQsosCommand = new RoutedCommand();
		public static RoutedCommand AddQsosCommand = new RoutedCommand();
		public static RoutedCommand ClearQsosCommand = new RoutedCommand();
		public static RoutedCommand ExportQsosCommand = new RoutedCommand();
		public static RoutedCommand IncludeAllQsosCommand = new RoutedCommand();
		public static RoutedCommand ExcludeAllQsosCommand = new RoutedCommand();
		
		public static RoutedCommand AddImageCommand = new RoutedCommand();
		public static RoutedCommand AddTextCommand = new RoutedCommand();
		public static RoutedCommand AddQsosBoxCommand = new RoutedCommand();
		public static RoutedCommand DeleteItemCommand = new RoutedCommand();
		public static RoutedCommand ClearBackgroundCommand = new RoutedCommand();
		public static RoutedCommand UserPreferencesCommand = new RoutedCommand();
		
		public static RoutedCommand SelectCommand = new RoutedCommand();
		public static RoutedCommand SelectItemCommand = new RoutedCommand();
		public static RoutedCommand NoneCommand = new RoutedCommand();
		public MainWindow()
		{
			InitializeComponent();
		}
		
		/// <summary>
		/// CanExecute for QSOs dropdown menu
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">CanExecuteRoutedEventArgs object</param>
		private void QsosCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			if(qsosTab != null)
			{
				e.CanExecute = qsosTab.IsSelected;
			}
			else
			{
				e.CanExecute =false;
			}
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
			e.CanExecute = qsosView.DisplayQsos.Count > 0 && qsosView.DisplayQsos.IsDirty;
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
		/// CanExecute routine for Add Image menu item
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">CanExecuteRoutedEventArgs object</param>
		private void AddImageCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			CardTabItem ti = this.mainTabControl.SelectedItem as CardTabItem;
			if(ti == null)
			{
				e.CanExecute = false;
			}
			CardItem ci = ti.cardCanvas.QslCard.GetSelected();
			e.CanExecute = ti != null && ci == null;
		}
		
		/// <summary>
		/// CanExecute routine for Add Text Item menu item
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">CanExecuteRoutedEventArgs object</param>
		private void AddTextCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			CardTabItem ti = this.mainTabControl.SelectedItem as CardTabItem;
			if(ti == null)
			{
				e.CanExecute = false;
				return;
			}
			CardItem ci = ti.cardCanvas.QslCard.GetSelected();
			e.CanExecute = ci == null;
		}
		
		/// <summary>
		/// CanExecute routine for Add QsosBox menu item
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">CanExecuteRoutedEventArgs object</param>
		private void AddQsosBoxCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			CardTabItem ti = this.mainTabControl.SelectedItem as CardTabItem;
			if(ti == null)
			{
				e.CanExecute = false;
				return;
			}
			Card card = ti.cardCanvas.QslCard;
			CardItem ci = card.GetSelected();
			e.CanExecute = ci == null && card.QsosBox == null;
		}
		
		/// <summary>
		/// CanExecute routine for Delete Item menu item
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">CanExecuteRoutedEventArgs object</param>
		private void DeleteItemCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			CardTabItem ti = this.mainTabControl.SelectedItem as CardTabItem;
			if(ti == null)
			{
				e.CanExecute = false;
				return;
			}
			CardItem ci = ti.cardCanvas.QslCard.GetSelected();
			e.CanExecute = ci != null && ci.GetType() != typeof(BackgroundImage);
		}
		
		/// <summary>
		/// CanExecute routine for Clear Background menu item
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">CanExecuteRoutedEventArgs object</param>
		private void ClearBackgroundCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			CardTabItem ti = this.mainTabControl.SelectedItem as CardTabItem;
			if(ti == null)
			{
				e.CanExecute = false;
				return;
			}
			CardItem ci = ti.cardCanvas.QslCard.GetSelected();
			e.CanExecute = (ci == null || ci.GetType() == typeof(BackgroundImage)) &&
			                ti.cardCanvas.QslCard.BackImage != null;
		}
		
		/// <summary>
		/// CanExecute handler for Select dropdown menu
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">CanExecuteRoutedEventArgs object</param>
		private void SelectCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			if(this.mainTabControl != null)
			{
				CardTabItem ti = this.mainTabControl.SelectedItem as CardTabItem;
				e.CanExecute = ti != null;
			}
			else
			{
				e.CanExecute = false;
			}
		}
		
		/// <summary>
		/// CanExecute handler for SelectItem menu items
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">CanExecuteRoutedEventArgs object</param>
		private void SelectItemCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			CardTabItem ti = this.mainTabControl.SelectedItem as CardTabItem;
			e.CanExecute = ti != null && ti.cardCanvas.QslCard.GetSelected() == null;
		}
		
		/// <summary>
		/// CanExecute handler for None menu item
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">CanExecuteRoutedEventArgs object</param>
		private void NoneCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			CardTabItem ti = this.mainTabControl.SelectedItem as CardTabItem;
			e.CanExecute = ti != null && ti.cardCanvas.QslCard.GetSelected() != null;
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
		
		/// <summary>
		/// Handler for User Preferences menu item Executed (Clicked) command
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
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
			qsosView.ShowIncludeSelectors();
		}
		
		
		/// <summary>
		/// Handles Add Qsos menu item processing
		/// </summary>
		/// <param name="sender">not used </param>
		/// <param name="e">not used</param>
		public void AddQsosCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			ImportOrAddQsos(qsosView.DisplayQsos.AddQsos);
			qsosView.ShowIncludeSelectors();
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
			qsosView.ShowIncludeSelectors();
		}
		
		/// <summary>
		/// Handles Export Qsos menu item processing
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void ExportQsosCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			// The only change that can be made is to update one or more managers.
			// Transfer managers back to the actual Qsos
			qsosView.DisplayQsos.UpdateQSOsWithManager();
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
				qsosView.DisplayQsos.IsDirty = false;
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
			qsosView.ShowIncludeSelectors();
		}
		
		/// <summary>
		/// Handler for Add Image menu item Executed (Clicked) command
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void AddImageCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			CardTabItem cti = mainTabControl.SelectedItem as CardTabItem;
			if(cti != null)
			{
				cti.cardCanvas.AddImageCommand_Executed(sender, e);
			}
		}
		
		/// <summary>
		/// Handler for Add Text Item menu item Executed (Clicked) command
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void AddTextCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			CardTabItem cti = mainTabControl.SelectedItem as CardTabItem;
			if(cti != null)
			{
				cti.cardCanvas.AddTextCommand_Executed(sender, e);
			}
			
		}
		
		/// <summary>
		/// Handler for Add QsosBox menu item Executed (Clicked) command
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void AddQsosBoxCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			CardTabItem cti = mainTabControl.SelectedItem as CardTabItem;
			if(cti != null)
			{
				cti.cardCanvas.AddQsosBoxCommand_Executed(sender, e);
			}			
		}
		
		/// <summary>
		/// Handler for Delete Card Item menu item Executed (Clicked) command
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void DeleteItemCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			CardTabItem cti = mainTabControl.SelectedItem as CardTabItem;
			if(cti != null)
			{
				cti.cardCanvas.DeleteItemCommand_Executed(sender, e);
			}
		}
		
		/// <summary>
		/// Handler for Clear Background menu item Executed (Clicked) command
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void ClearBackgroundCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			CardTabItem cti = mainTabControl.SelectedItem as CardTabItem;
			if(cti != null)
			{
				cti.cardCanvas.ClearBackgroundCommand_Executed(sender, e);
			}
			cti.cardCanvas.QslCard.BackImage.IsSelected = false;
		}
		
		/// <summary>
		/// Handles New -> Bureau Card menu item processing
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void BureauCardMenuItem_Click(object sender, RoutedEventArgs e)
		{
			// create a CardTabItem and add it to the mainTabControl
			CardTabItem cardTab = new CardTabItem(5.5 * 96, 3.5 * 96);
			mainTabControl.Items.Add(cardTab);
			cardTab.IsSelected = true;		// select the new tab
		}
		
		/// <summary>
		/// Creates the Select menu MenuItems based on the CardItems in QslCard
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void SelectMenu_SubmenuOpened(object sender, RoutedEventArgs e)
		{
			SelectMenu.Items.Clear();
			CardTabItem cti = mainTabControl.SelectedItem as CardTabItem;
			if(cti != null)
			{
				Card card = cti.cardCanvas.QslCard;
				BuildBackgroundMenuItem();
				BuildSecondaryImagesMenuItems();
				BuildTextItemsMenuItems();
				BuildQsosBoxMenuItem();
				SelectMenu.Items.Add(new Separator());
				BuildNoneMenuItem();
			}
		}
		
		/// <summary>
		/// Helper method that builds the Background menu item
		/// </summary>
		private void BuildBackgroundMenuItem()
		{
			MenuItem mi = new MenuItem();
			CardTabItem cti = mainTabControl.SelectedItem as CardTabItem;
			string fName = cti.cardCanvas.QslCard.BackImage.ImageFileName;
			if(fName != string.Empty)
			{
				fName = System.IO.Path.GetFileName(fName);
			}
			else
			{
				fName = "Background";
			}
			mi.Header = fName;
			mi.Click += OnSelectItem_Clicked;
			mi.Command = SelectItemCommand;
			mi.Tag = cti.cardCanvas.QslCard.BackImage;
			SelectMenu.Items.Add(mi);
		}
		
		/// <summary>
		/// Helper method that builds a MenuItem for each SecondaryImage on the card
		/// </summary>
		private void BuildSecondaryImagesMenuItems()
		{
			CardTabItem cti = mainTabControl.SelectedItem as CardTabItem;
			foreach(SecondaryImage si in cti.cardCanvas.QslCard.SecondaryImages)
			{
				MenuItem mi = new MenuItem();
				string fName = si.ImageFileName;
				if(fName != string.Empty)
				{
					fName = System.IO.Path.GetFileName(fName);
				}
				else
				{
					fName = "Image";
				}
				mi.Header = fName;
				mi.Click += OnSelectItem_Clicked;
				mi.Command = SelectItemCommand;
				mi.Tag = si;
				SelectMenu.Items.Add(mi);
			}
		}
		
		/// <summary>
		/// Helper method that builds a MenuItem for every TextItem on the card
		/// </summary>
		private void BuildTextItemsMenuItems()
		{
			CardTabItem cti = mainTabControl.SelectedItem as CardTabItem;
			foreach(TextItem ti in cti.cardCanvas.QslCard.TextItems)
			{
				MenuItem mi = new MenuItem();
				TextParts parts = ti.Text;
				string text = parts.GetText(true);
				text = (text.Length > 10) ? text.Substring(0, 10) : text;
				mi.Header = text;
				mi.Click += OnSelectItem_Clicked;
				mi.Command = SelectItemCommand;
				mi.Tag = ti;
				SelectMenu.Items.Add(mi);
			}
		}
		
		/// <summary>
		/// Helper method that builds a MenuItem for the QsosBox
		/// </summary>
		private void BuildQsosBoxMenuItem()
		{
			CardTabItem cti = mainTabControl.SelectedItem as CardTabItem;
			if(cti.cardCanvas.QslCard.QsosBox != null)
			{
				MenuItem mi = new MenuItem();
				mi.Header = "Qsos Box";
				mi.Click += OnSelectItem_Clicked;
				mi.Command = SelectItemCommand;
				mi.Tag = cti.cardCanvas.QslCard.QsosBox;
				SelectMenu.Items.Add(mi);
			}
		}
		
		/// <summary>
		/// Helper method that builds the None menu item
		/// </summary>
		private void BuildNoneMenuItem()
		{
			MenuItem none = new MenuItem();
			none.Header = "None";
			none.Click += OnNone_Clicked;
			none.Command = NoneCommand;
			SelectMenu.Items.Add(none);
		}
		
		/// <summary>
		/// Handler for SelectItems clicked event
		/// </summary>
		/// <param name="sender">menu item that was clicked</param>
		/// <param name="e">Not used</param>
		private void OnSelectItem_Clicked(object sender, RoutedEventArgs e)
		{
			CardTabItem cti = mainTabControl.SelectedItem as CardTabItem;
			if(cti != null)
			{
				MenuItem mi = (MenuItem) sender;
				if(mi != null)
				{
					CardItem ci = ((MenuItem)sender).Tag as CardItem;
					if(ci != null)
					{
						ci.IsSelected = true;
						cti.SetPropertiesVisibility(ci);
						cti.cardCanvas.QslCard.InvalidateVisual();
					}
				}
			}
		}
		
		/// <summary>
		/// Handler for the None menu item clicked event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void OnNone_Clicked(object sender, RoutedEventArgs e)
		{
			CardTabItem cti = mainTabControl.SelectedItem as CardTabItem;
			if(cti != null)
			{
				CardItem ci = cti.cardCanvas.QslCard.GetSelected();
				if(ci != null)
				{
					ci.IsSelected = false;
					cti.SetPropertiesVisibility(null);
					cti.cardCanvas.QslCard.InvalidateVisual();
				}
			}
		}
	}
}