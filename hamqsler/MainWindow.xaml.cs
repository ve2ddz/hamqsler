/*
 *  Author:
 *       Jim Orcheson <jimorcheson@gmail.com>
 * 
 *  Copyright © 2012, 2013 Jim Orcheson
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
using System.IO.Packaging;
using System.Printing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Xps;
using System.Windows.Xps.Packaging;
using System.Windows.Xps.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace hamqsler
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private PrintDialog printDialog = null;
		private PageMediaSize pageSize = null;
		
		public delegate string AddOrImportDelegate(string fName, QSOsView.OrderOfSort so);

		public static RoutedCommand CardOpenCommand = new RoutedCommand();
		public static RoutedCommand CardSaveCommand = new RoutedCommand();
		public static RoutedCommand CardSaveAsCommand = new RoutedCommand();
		public static RoutedCommand CloseCardCommand = new RoutedCommand();
		public static RoutedCommand PrintCardsCommand = new RoutedCommand();
		public static RoutedCommand ExitCommand = new RoutedCommand();
		
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
		/// Handler for Card Save CanExecute event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">CanExecuteRoutedEventArgs object</param>
		private void CardSaveCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			CardTabItem cti = mainTabControl.SelectedItem as CardTabItem;
			e.CanExecute = cti != null && cti.cardCanvas.QslCard.FileName != null &&
				cti.cardCanvas.QslCard.IsDirty;
		}
		
		/// <summary>
		/// CanExecute for File->SaveCardAs
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">CanExecuteRoutedEventArgs object</param>
		private void CardSaveAsCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			CardTabItem cti = mainTabControl.SelectedItem as CardTabItem;
			e.CanExecute = cti != null;
		}
		
		/// <summary>
		/// CanExecute for File->Card Close
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">CanExecuteRoutedEventArgs object</param>
		private void CloseCardCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			CardTabItem cti = mainTabControl.SelectedItem as CardTabItem;
			e.CanExecute = cti != null;
		}
		
		/// <summary>
		/// CanExecute handler for Print Cards... menu item
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">CanExecuteRoutedEventArgs object</param>
		private void PrintCardsCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			CardTabItem cti = mainTabControl.SelectedItem as CardTabItem;
			e.CanExecute = cti != null;
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
				return;
			}
			CardItem ci = null;
			if(ti.cardCanvas != null && ti.cardCanvas.QslCard != null)
			{
				e.CanExecute = ti.cardCanvas.QslCard.GetSelected() == null;
				return;
			}
			e.CanExecute = ci == null;
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
		/// Handler for Window Closing event.
		/// Check each Card to see if it should be saved.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Window_Closing(object sender, EventArgs e)
		{
			TabItem[] tabItems = new TabItem[mainTabControl.Items.Count];
			mainTabControl.Items.CopyTo(tabItems, 0);
			foreach(TabItem ti in tabItems)
			{
				CardTabItem cti = ti as CardTabItem;
				if(cti != null)
				{
					CloseCardTab(cti, false);
				}
			}
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
		/// Handler for window Loaded event.
		/// Loads Adif and card files that were loaded last time the program was terminated
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void Window_Loaded(object sender, EventArgs e)
		{
			UserPreferences prefs = ((App)App.Current).UserPreferences;
			// load Adif files
			if(prefs.AdifReloadOnStartup)
			{
				string[] adifFiles = prefs.AdifFiles.ToArray();
				prefs.AdifFiles.Clear();
				if(adifFiles.Length > 0)
				{
					// import the QSOs from the ADIF file
					for(int i = 0; i < adifFiles.Length; i++)
					{
						try
						{
							string error = qsosView.DisplayQsos.AddQsos(adifFiles[i], qsosView.SortOrder);
							if(error != null)
							{
								MessageBox.Show(error, "Import Error", MessageBoxButton.OK,
							                MessageBoxImage.Warning);
							}
						}
						catch(Exception ex)
						{
							App.Logger.Log(ex);
						}
					}
				}
			}
			// load card files
			if(prefs.CardsReloadOnStartup)
			{
				string[] fileNames = prefs.CardFiles.ToArray();
				prefs.CardFiles.Clear();
				foreach(string fileName in fileNames)
				{
					Card card = Card.DeserializeCard(fileName);
					card.FileName = fileName;
					card.IsDirty = false;
					CardTabItem cti = new CardTabItem(card);
					mainTabControl.Items.Add(cti);
					cti.IsSelected = true;		// select the new tab
					cti.SetTabLabel();
					// need to call SetTitle here because mainTabControl SelectionChanged event is not fired.
					SetTitle(card.FileName, card.IsDirty);
					prefs.CardFiles.Add(fileName);
				}
			}
		}
		
		/// <summary>
		/// Handler for CardOpen Executed event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void CardOpenCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			OpenFileDialog oDialog = new OpenFileDialog();
			oDialog.Filter = "QSL Card(*.qslx)|*.qslx|QslDnP Card(*xqsl)|*.xqsl";
			if(oDialog.ShowDialog(this) == true)
			{
				string fileName = oDialog.FileName;
				string fileExt = fileName.Substring(fileName.Length - 4);
				Card card;
				if(fileExt.Equals("qslx"))		// HamQSLer card file
				{
					card = Card.DeserializeCard(fileName);
					card.FileName = fileName;
					card.IsDirty = false;
					UserPreferences prefs = ((App)App.Current).UserPreferences;
					if(prefs.CardsReloadOnStartup)
					{
						prefs.CardFiles.Add(fileName);
						prefs.SerializeAsXml();
					}
				}
				else if(fileExt.Equals("xqsl"))  // QslDnP card file
				{
					try
					{
						card = Card.DeserializeQslDnPCard(fileName);
					}
					catch(XmlException)
					{
	            		string msg = string.Format("The file '{0}' does not contain a valid QslDnP card description.",
	            		                           fileName);
	            		MessageBox.Show(msg, "File Content Error", MessageBoxButton.OK,
	            		                MessageBoxImage.Error);
						return;						
					}
					card.FileName = fileName.Substring(0, fileName.Length - 4) + "qslx";
					card.IsDirty = true;
				}
				else			// neither file type - this really is a programming error
				{
					return;
				}
				CardTabItem cti = new CardTabItem(card);
				mainTabControl.Items.Add(cti);
				cti.IsSelected = true;		// select the new tab
				cti.SetTabLabel();
				// need to call SetTitle here because mainTabControl SelectionChanged event is not fired.
				SetTitle(card.FileName, card.IsDirty);
			}
				
		}
		
		/// <summary>
		/// Handler for Card Save Executed event
		/// </summary>
		/// <param name="sender">Object that is the source of this event</param>
		/// <param name="e">ExecutedRoutedEventArgs object</param>
		private void CardSaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			SaveCard();
		}
		
		/// <summary>
		/// Handler for Save Card As menu item
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void CardSaveAsCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			SaveCardAs();
		}
		
		/// <summary>
		/// Handler for Close Card Executed event
		/// </summary>
		/// <param name="sender">object that generated the event</param>
		/// <param name="e">ExecutedRoutedEventArgs object</param>
		private void CloseCardCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			CardTabItem cti = mainTabControl.SelectedItem as CardTabItem;
			CloseCardTab(cti, true);
		}
		
		/// <summary>
		/// Handler for Print Cards menu item Executed event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void PrintCardsCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			const double GRAPHICSPIXELSPERINCH = 96;
			MessageBoxResult result = MessageBoxResult.No;
			CardTabItem cti = mainTabControl.SelectedItem as CardTabItem;
			PrintTicket ticket = new PrintTicket();
			if(printDialog == null)
			{
				printDialog = new PrintDialog();
			}
			else
			{
				printDialog.PrintTicket.PageMediaSize = pageSize;
			}
			do
			{
				if(printDialog.ShowDialog() == true)
				{
					ticket = printDialog.PrintTicket.Clone();
					pageSize =ticket.PageMediaSize;
					// make sure at least one card can be printed on the page
					double pageHeight = (double)ticket.PageMediaSize.Height;
					double pageWidth = (double)ticket.PageMediaSize.Width;
					double cardWidth = cti.cardCanvas.QslCard.DisplayWidth;
					double cardHeight = cti.cardCanvas.QslCard.DisplayHeight;
					// check that at least one card can be printed
					if(Math.Min(cardWidth, cardHeight) > Math.Min(pageWidth, pageHeight) ||
						Math.Max(cardWidth, cardHeight) > Math.Max(pageWidth, pageHeight))
					{
                        string msg = string.Format("The size of the cards to be printed is larger than the selected paper size.\r\n"
                                        + "Card size is {0} by {1} inches and paper size is {2} by {3} inches.\r\n"
                                        + "You must choose another paper size.", 
                                        cardWidth / GRAPHICSPIXELSPERINCH, 
                                        cardHeight / GRAPHICSPIXELSPERINCH,
                                        pageHeight / GRAPHICSPIXELSPERINCH, 
                                        pageWidth / GRAPHICSPIXELSPERINCH);
						MessageBox.Show(msg, "Paper Size Too Small", MessageBoxButton.OK, MessageBoxImage.Error);
						result = MessageBoxResult.No;
						continue;
					}
					result = MessageBoxResult.Yes;
				}
				else
				{
					return;
				}
			} while(result == MessageBoxResult.No);
			ticket = printDialog.PrintTicket;
			// paginate and print
			Card card = cti.cardCanvas.QslCard;
			PrintSettingsDialog psDialog = PrintSettingsDialog.CreatePrintSettingsDialog(ticket, card);
			if(psDialog.ShowDialog() == true)
			{
				if(psDialog.PrintType == PrintSettingsDialog.PrintButtonTypes.Print)
				{
					HamqslerPaginator paginator = 
						new HamqslerPaginator(psDialog.CardsLayout, card, qsosView.DisplayQsos,
						                      new Size((double)ticket.PageMediaSize.Width,
						                               (double)ticket.PageMediaSize.Height));
					// Kludge: to get printer to print to near bottom of page
					// add 1/2 inch to page height
					PageMediaSize pms = new PageMediaSize((double)ticket.PageMediaSize.Width,
					                                      (double)ticket.PageMediaSize.Height + 
					                                      GRAPHICSPIXELSPERINCH / 2);
					ticket.PageMediaSize = pms;
					// force Landscape orientation
					ticket.PageOrientation = PageOrientation.Portrait;
					if(psDialog.CardsLayout == PrintSettingsDialog.CardLayout.LandscapeEdge ||
					   psDialog.CardsLayout == PrintSettingsDialog.CardLayout.LandscapeCentre ||
					   psDialog.CardsLayout == PrintSettingsDialog.CardLayout.LandscapeTopCentre)
					{
						ticket.PageOrientation = PageOrientation.Landscape;
					}
					printDialog.PrintTicket = ticket;
					printDialog.PrintDocument(paginator, "QSL Cards");
				}
				else if(psDialog.PrintType == PrintSettingsDialog.PrintButtonTypes.Preview)
				{
					MemoryStream ms = new MemoryStream();
					Package pkg = Package.Open(ms, FileMode.Create, FileAccess.ReadWrite);
					string pack = "pack://temp.xps";
					Uri uri = new Uri(pack);
					PackageStore.AddPackage(uri, pkg);
					XpsDocument doc = new XpsDocument(pkg, CompressionOption.NotCompressed, pack);
					XpsSerializationManager rsm =
						new XpsSerializationManager(new XpsPackagingPolicy(doc), false);
					HamqslerPaginator paginator = 
						new HamqslerPaginator(psDialog.CardsLayout, card, qsosView.DisplayQsos,
						                      new Size((double)ticket.PageMediaSize.Width,
						                               (double)ticket.PageMediaSize.Height));
					rsm.SaveAsXaml(paginator);

					XpsDocumentWindow docWindow = new XpsDocumentWindow();
					docWindow.docViewer.Document = doc.GetFixedDocumentSequence();
					docWindow.ShowDialog();
					PackageStore.RemovePackage(uri);
				}
			}
		}
		
		/// <summary>
		/// Handler for Exit menu item Executed event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void ExitCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			// just close main window; Actual work done in Window_Closing and Window_Closed,
			// which are called as result of Close call.
			this.Close();
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
			UserPreferences prefs = ((App)App.Current).UserPreferences;
			prefs.AdifFiles.Clear();
			prefs.SerializeAsXml();
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
				UserPreferences prefs = ((App)Application.Current).UserPreferences;
				prefs.AdifFiles.Clear();
				prefs.AdifFiles.Add(fileName);
				prefs.SerializeAsXml();
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
			cardTab.SetTabLabel();
			// need to call SetTitle here because mainTabControl SelectionChanged event is not fired.
			SetTitle(cardTab.cardCanvas.QslCard.FileName, cardTab.cardCanvas.QslCard.IsDirty);
		}
		
		/// <summary>
		/// Handles New -> 4 1/4 by 5 1/2 inch Card menu item processing
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void Card45MenuItem_Click(object sender, RoutedEventArgs e)
		{
			// create a CardTabItem and add it to the mainTabControl
			CardTabItem cardTab = new CardTabItem(5.5 * 96, 4.25 * 96);
			mainTabControl.Items.Add(cardTab);
			cardTab.IsSelected = true;		// select the new tab
			cardTab.SetTabLabel();
			SetTitle(cardTab.cardCanvas.QslCard.FileName, cardTab.cardCanvas.QslCard.IsDirty);
		}

		/// <summary>
		/// Handles New -> 4 by 6 inch Card menu item processing
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void Card46MenuItem_Click(object sender, RoutedEventArgs e)
		{
			// create a CardTabItem and add it to the mainTabControl
			CardTabItem cardTab = new CardTabItem(6 * 96, 4 * 96);
			mainTabControl.Items.Add(cardTab);
			cardTab.IsSelected = true;		// select the new tab
			cardTab.SetTabLabel();
			SetTitle(cardTab.cardCanvas.QslCard.FileName, cardTab.cardCanvas.QslCard.IsDirty);
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
				string text = parts.GetText(null, null, true);
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
				}
			}
		}

		/// <summary>
		/// Save card as XML
		/// </summary>
		/// <param name="card">the card to save</param>
		/// <param name="fileName">name of file to save the card in</param>
		private void SaveCard(Card card, string fileName)
		{
			XmlSerializer xmlFormat = new XmlSerializer(typeof(Card),
			                                            new Type[]{typeof(BackgroundImage),
			                                            	typeof(SecondaryImage),
			                                            	typeof(CardImageBase),
			                                            	typeof(TextItem),
			                                            	typeof(QsosBox),
			                                            	typeof(TextParts),
			                                            	typeof(StaticText),
			                                            	typeof(AdifMacro),
			                                            	typeof(AdifExistsMacro),
			                                            	typeof(CountMacro),
			                                            	typeof(ManagerMacro),
			                                            	typeof(ManagerExistsMacro),
			                                            	typeof(SolidColorBrush),
			                                            	typeof(MatrixTransform)});
			using (Stream fStream = new FileStream(fileName, FileMode.Create,
			                                       FileAccess.Write, FileShare.Read))
			{
				xmlFormat.Serialize(fStream, card);
				card.FileName = fileName;
				card.IsDirty = false;
				SetTitle(fileName, card.IsDirty);
			}
		}
		
		/// <summary>
		/// Set tab text and window title
		/// </summary>
		/// <param name="fileName">name of the card file</param>
		/// <param name="isDirty">Boolean indicating whether any of the card properties has changed</param>
		public void SetTitle(string fileName, bool isDirty)
		{
			CardTabItem cti = mainTabControl.SelectedItem as CardTabItem;
			if(cti != null)
			{
				this.Title = "HamQSLer - ";
				if(fileName != null)
				{
					FileInfo fileInfo = new FileInfo(fileName);
					string fName = fileInfo.Name;
					this.Title += fileName + (isDirty ? " - Modified" : string.Empty);
				}
				else
				{
					this.Title += "New Card" + (isDirty ? "- Modified" : string.Empty);
				}
			}
		}
		
		/// <summary>
		/// Handler for mainTabControl SelectionChanged event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void MainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			// all we do is set the window title based on tab selected
			CardTabItem ti = mainTabControl.SelectedItem as CardTabItem;
			if(ti != null)
			{
				SetTitle(ti.cardCanvas.QslCard.FileName, ti.cardCanvas.QslCard.IsDirty);
			}
			else
			{
				this.Title = "HamQSLer";
			}
			e.Handled = true;
		}
		
		/// <summary>
		/// Close the CardTabItem
		/// </summary>
		/// <param name="cti">CardTabItem to close</param>
		public void CloseCardTab(CardTabItem cti, bool doNotReloadCard)
		{
			if(cti.cardCanvas.QslCard.IsDirty)
			{
				MessageBoxResult result = MessageBox.Show("The design of this card has been modified.\n"
				                + "Do you want to save the card before closing?", "Card Modified", 
				                MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
				if(result == MessageBoxResult.Yes)
				{
					SaveCard();
				}
			}
			mainTabControl.Items.Remove(cti);
			UserPreferences prefs = ((App)App.Current).UserPreferences;
			if(doNotReloadCard && prefs.CardsReloadOnStartup)
			{
				if(cti.cardCanvas.QslCard.FileName != null)
				{
					prefs.CardFiles.Remove(cti.cardCanvas.QslCard.FileName);
					prefs.SerializeAsXml();
				}
			}
			cti = null;
		}
		
		/// <summary>
		/// Save card to file named in Card.FileName
		/// </summary>
		private void SaveCard()
		{
			CardTabItem cti = mainTabControl.SelectedItem as CardTabItem;
			Card qslCard = cti.cardCanvas.QslCard;
			if(qslCard.FileName != null)
			{
				qslCard.SaveAsXml(qslCard.FileName);
				cti.SetTabLabel();
				SetTitle(qslCard.FileName, qslCard.IsDirty);
				// If the last property control changed was a color button, then the background
				// color of the button will cycle from 0 to 100% opacity. By moving focus to a
				// different control, this will not happen
				cti.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
			}
			else
			{
				SaveCardAs();
			}

		}
		
		/// <summary>
		/// Save card to new file
		/// </summary>
		private void SaveCardAs()
		{
			CardTabItem cti = mainTabControl.SelectedItem as CardTabItem;
			Card qslCard = cti.cardCanvas.QslCard;
			string fileName = qslCard.FileName;
			SaveFileDialog sDialog = new SaveFileDialog();
			sDialog.Filter = "QSL Card(*.qslx)|*.qslx";
			if(fileName != null)
			{
				sDialog.FileName = fileName;
			}
			if(sDialog.ShowDialog() == true)
			{
				qslCard.SaveAsXml(sDialog.FileName);
				cti.SetTabLabel();
				SetTitle(sDialog.FileName, qslCard.IsDirty);
				UserPreferences prefs = ((App)App.Current).UserPreferences;
				if(prefs.CardsReloadOnStartup)
				{
					if(fileName != sDialog.FileName)
					{
						prefs.CardFiles.Remove(fileName);
						prefs.CardFiles.Add(sDialog.FileName);
						prefs.SerializeAsXml();
					}
				}
				// If the last property control changed was a color button, then the background
				// color of the button will cycle from 0 to 100% opacity. By moving focus to a
				// different control, this will not happen
				cti.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
			}

		}
	}
}