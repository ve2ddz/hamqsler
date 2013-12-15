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
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

using Microsoft.Win32;

namespace hamqsler
{
	/// <summary>
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private static readonly DependencyProperty ShowQsosBoxVerticalAnchorProperty =
			DependencyProperty.Register("ShowQsosBoxVerticalAnchor", typeof(bool),
			                            typeof(MainWindow), new PropertyMetadata(false));
		public bool ShowQsosBoxVerticalAnchor
		{
			get {return (bool) GetValue(ShowQsosBoxVerticalAnchorProperty);}
			set {SetValue(ShowQsosBoxVerticalAnchorProperty, value);}
		}
		
		public static double PIXELSPERINCH = 100;
		
		public delegate string AddOrImportDelegate(string fName, QSOsView.OrderOfSort so,
		                                          AdifEnumerations aEnums);

		public static RoutedCommand NewBureauCardCommand = new RoutedCommand();
		public static RoutedCommand New45CardCommand = new RoutedCommand();
		public static RoutedCommand New46CardCommand = new RoutedCommand();
		public static RoutedCommand CardOpenCommand = new RoutedCommand();
		public static RoutedCommand CardSaveCommand = new RoutedCommand();
		public static RoutedCommand CardSaveAsCommand = new RoutedCommand();
		public static RoutedCommand CloseCardCommand = new RoutedCommand();
		public static RoutedCommand SaveCardAsJpegCommand = new RoutedCommand();
		public static RoutedCommand SaveCardsForPrintingCommand = new RoutedCommand();
		public static RoutedCommand Save4UpCardsForPrintingCommand = new RoutedCommand();
		public static RoutedCommand CalculateCardsToBePrintedCommand = new RoutedCommand();
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
		
		public static RoutedCommand UserManualA4Command = new RoutedCommand();
		public static RoutedCommand UserManualLetterCommand = new RoutedCommand();
		public static RoutedCommand AboutCommand = new RoutedCommand();
		public static RoutedCommand WebsiteCommand = new RoutedCommand();
		public static RoutedCommand ViewLogFileCommand = new RoutedCommand();
		
		/// <summary>
		/// Constructor
		/// </summary>
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
			e.CanExecute = cti != null && cti.cardPanel.QslCard.FileName != null &&
				cti.cardPanel.QslCard.IsDirty;
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
		/// CanExecute for File->Calculate Number of Cards to be Printed...
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">CanExecuteRoutedEventArgs object</param>
		private void CalculateCardsToBePrintedCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
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
		/// CanExecute for Cards->Save Card As Jpeg...
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">CanExecuteRoutedEventArgs object</param>
		private void SaveCardsForPrintingCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			bool canExecute = false;
			CardTabItem cti = mainTabControl.SelectedItem as CardTabItem;
			List<List<DispQso>> qList = null;
			if(cti != null)
			{
				qList = qsosView.DisplayQsos.GetDispQsosList(
						cti.cardPanel.QslCard);
			}
			if(cti != null && qList != null && qList.Count > 0)
			{
				CardWF card = cti.cardPanel.QslCard;
				if(card.Width <= 6 * PIXELSPERINCH && card.Height <= 4 * PIXELSPERINCH)
				{
					canExecute = true;
				}
			}
			e.CanExecute = canExecute;
		}
		
		/// <summary>
		/// CanExecute for Cards->Save Card As Jpeg...
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">CanExecuteRoutedEventArgs object</param>
		private void SaveCardAsJpegCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			CardTabItem cti = mainTabControl.SelectedItem as CardTabItem;
			e.CanExecute = cti != null;
		}
		
		/// <summary>
		/// CanExecute for Cards->Save 4 Up Card For Printing
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">CanExecuteRoutedEventArgs object</param>
		private void Save4UpCardsForPrintingCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			bool canExecute = false;
			CardTabItem cti = mainTabControl.SelectedItem as CardTabItem;
			if(cti != null)
			{
				CardWF card = cti.cardPanel.QslCard;
				if(card.Width == 5.5 * PIXELSPERINCH && 
				   card.Height == 3.5 * PIXELSPERINCH)
				{
					canExecute = true;
				}
			}
			e.CanExecute = canExecute;
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
			CardWFItem ci = null;
			if(ti.cardPanel != null && ti.cardPanel.QslCard != null)
			{
				e.CanExecute = ti.cardPanel.QslCard.GetSelectedItem() == null;
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
			CardWFItem ci = ti.cardPanel.QslCard.GetSelectedItem();
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
			CardWF card = ti.cardPanel.QslCard;
			CardWFItem ci = card.GetSelectedItem();
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
			CardWFItem ci = ti.cardPanel.QslCard.GetSelectedItem();
			e.CanExecute = ci != null && ci.GetType() != typeof(BackgroundWFImage);
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
			CardWFItem ci = ti.cardPanel.QslCard.GetSelectedItem();
			e.CanExecute = (ci == null || ci.GetType() == typeof(BackgroundWFImage)) &&
			                ti.cardPanel.QslCard.BackgroundImage != null;
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
				TabItem ti = this.mainTabControl.SelectedItem as TabItem;
				CardTabItem cti = this.mainTabControl.SelectedItem as CardTabItem;
				e.CanExecute = cti != null || (ti.Header.Equals("QSOs") &&
				                               qsosView.qsosListView.Items.Count != 0);
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
			CardTabItem cti = this.mainTabControl.SelectedItem as CardTabItem;
			e.CanExecute = cti != null && cti.cardPanel.QslCard.GetSelectedItem() == null;
		}
		
		/// <summary>
		/// CanExecute handler for None menu item
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">CanExecuteRoutedEventArgs object</param>
		private void NoneCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			CardTabItem ti = this.mainTabControl.SelectedItem as CardTabItem;
			e.CanExecute = ti != null && ti.cardPanel.QslCard.GetSelectedItem() != null;
		}
		
		/// <summary>
		/// CanExecute handler for User Manual - A4 menu item
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">CanExecuteRoutedEventArgs object</param>
		private void UserManualA4Command_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			FileInfo fi = new FileInfo("help\\pdf\\hamqsler-A4.pdf");
			e.CanExecute = fi.Exists;
		}
		
		/// <summary>
		/// CanExecute handler for User Manual - US Letter menu item
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">CanExecuteRoutedEventArgs object</param>
		private void UserManualLetterCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			FileInfo fi = new FileInfo("help\\pdf\\hamqsler-letter.pdf");
			e.CanExecute = fi.Exists;
		}
		
		/// <summary>
		/// Handler for Window Closing event.
		/// Check each Card to see if it should be saved.
		/// </summary>
		/// <param name="sender">Not used</param>
		/// <param name="e">Not used</param>
		void Window_Closing(object sender, EventArgs e)
		{
			TabItem[] tabItems = new TabItem[mainTabControl.Items.Count];
			mainTabControl.Items.CopyTo(tabItems, 0);
			foreach(TabItem ti in tabItems)
			{
				CardTabItem cti = ti as CardTabItem;
				if(cti != null)
				{
					cti.IsSelected = true;
					CloseCardTab(cti, false);
				}
			}
			CloseQsosTab();
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
			// load card files
			if(prefs.CardsReloadOnStartup)
			{
				string[] fileNames = prefs.CardFiles.ToArray();
				prefs.CardFiles.Clear();
				CardWF card;
				foreach(string fileName in fileNames)
				{
					try
					{
						card = CardWF.DeserializeCard(fileName);
					}
					catch(Exception ex)
					{
						App.Logger.Log(ex);
						continue;
					}
					card.FileName = fileName;
					card.IsInDesignMode = true;
					CardTabItem cti = new CardTabItem(card);
					mainTabControl.Items.Add(cti);
					cti.IsSelected = true;		// select the new tab
					card.IsDirty = false;
					cti.SetTabLabel();
					// need to call SetTitle here because mainTabControl SelectionChanged event is not fired.
					SetTitle(card.FileName, card.IsDirty);
					prefs.CardFiles.Add(fileName);
				}
			}
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
							string error = qsosView.DisplayQsos.AddQsos(adifFiles[i], qsosView.SortOrder,
							                                           App.AdifEnums);
							if(error != null)
							{
								MessageBox.Show("Either there are errors in the ADIF file loaded, or" +
								                Environment.NewLine +
								                "individual QSO fields were modified during load." +
								                Environment.NewLine +
								                "See log file for details.", "Import Error or Change", 
								                MessageBoxButton.OK, MessageBoxImage.Warning);
								App.Logger.Log(error);
							}
						}
						catch(Exception ex)
						{
							App.Logger.Log(ex);
							prefs.AdifFiles.Remove(adifFiles[i]);
							((App)Application.Current).UserPreferences = prefs;
							((App)Application.Current).UserPreferences.SerializeAsXml();
						}
					}
					qsosView.ShowIncludeSelectors();
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
			oDialog.Filter = "QSL Card(*.xq1)|*.xq1";
			oDialog.InitialDirectory = 
				((App)Application.Current).UserPreferences.DefaultCardFilesFolder;
			if(oDialog.ShowDialog(this) == true)
			{
				string fileName = oDialog.FileName;
				string fileExt = fileName.Substring(fileName.Length - 4);
				CardWF card = null;
				if(fileExt.Equals(".xq1"))		// HamQSLer card file
				{
					try
					{
						card = CardWF.DeserializeCard(fileName);
					}
					catch(InvalidOperationException ioe)
					{
						MessageBox.Show("An error occurred while opening "
						                + "the card. See the log file for details. The problem "
						                + "must be fixed before the card can be loaded.",
						                "Error Loading Card", MessageBoxButton.OK, 
						                MessageBoxImage.Error);
						App.Logger.Log(ioe, true, false);
						return;
						
					}
					catch(Exception ex)
					{
						App.Logger.Log(ex);
						return;
					}
					card.FileName = fileName;
					UserPreferences prefs = ((App)App.Current).UserPreferences;
					if(prefs.CardsReloadOnStartup)
					{
						prefs.CardFiles.Add(fileName);
						prefs.SerializeAsXml();
					}
				}
				else			// bad file type - this really is a programming error
				{
					return;
				}
				// Card.IsDirty is set to true in CardTabItem constructor so we must
				// save IsDirty and restore its value afterwards
				CardTabItem cti = new CardTabItem(card);
				card.IsInDesignMode = true;
				// if background or secondary image fileName changed, mark
				// card as dirty
				bool dirty = card.BackgroundImage.ModifiedDuringLoad;
				foreach(SecondaryWFImage image in card.SecondaryImages)
				{
					dirty = dirty || image.ModifiedDuringLoad;
				}
				card.IsDirty = dirty;
				// add the CardTabItem to the mainTabControl and set title and
				// tab label
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
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void CardSaveCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			CardTabItem cti = mainTabControl.SelectedItem as CardTabItem;
			SaveCard(cti);
		}
		
		/// <summary>
		/// Handler for Save Card As menu item
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void CardSaveAsCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			CardTabItem cti = mainTabControl.SelectedItem as CardTabItem;
			SaveCardAs(cti);
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
		/// Handler for Save Card As Jpeg Executed event
		/// </summary>
		/// <param name="sender">object that generated the event</param>
		/// <param name="e">ExecutedRoutedEventArgs object</param>
		private void SaveCardAsJpegCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			CardTabItem cti = mainTabControl.SelectedItem as CardTabItem;
			List<List<DispQso>> qList = null;
			if(cti != null)
			{
				qList = qsosView.DisplayQsos.GetDispQsosList(
						cti.cardPanel.QslCard);
			}
			JpegPropsDialog jpD = new JpegPropsDialog(qList != null && qList.Count > 0);
			if(jpD.ShowDialog() == true)
			{
				SaveFileDialog saveDialog = new SaveFileDialog();
				saveDialog.Filter = "Image (*.jpg)|*.jpg";
				saveDialog.InitialDirectory = ((App)Application.Current).HamqslerFolder;
				if(saveDialog.ShowDialog() == true)
				{
					string fileName = saveDialog.FileName;
					try
					{
						// save card as JPEG
						SaveCardAsJpeg(fileName, jpD.Resolution, jpD.Quality, jpD.ShowQsos);
						// tell user that file has been saved
						StatusText.Text = "Card image has been saved to " + fileName;
					}
					catch(Exception ex)
					{
						App.Logger.Log(ex);
					}
				}
					
			}
		}
		
		/// <summary>
		/// Save a card as JPEG file
		/// </summary>
		/// <param name="fileName">path to the file to save</param>
		/// <param name="resolution">image resolution</param>
		/// <param name="quality">image quality</param>
		/// <param name="showQsos">Boolean to indicate whether the card should include QSO info</param>
		private void SaveCardAsJpeg(string fileName, int resolution, int quality,
		                            bool showQsos)
		{
			// create visual of the card
			CardTabItem cti = mainTabControl.SelectedItem as CardTabItem;
			if(cti != null)
			{
				CardWF card = cti.cardPanel.QslCard.Clone();
				card.IsInDesignMode = false;
				FormsCardView cView = new FormsCardView(card);
				List<List<DispQso>> dispQsos = new List<List<DispQso>>();
				if(card.QsosBox != null)
				{
					if(qsosView.DisplayQsos.Count > 0 && showQsos)
					{
						dispQsos = qsosView.DisplayQsos.GetDispQsosList(card);
					}
				}
	
				float scale = (float)resolution / 100.0F;
				int bitmapWidth = (int)((float)card.Width * scale);
				int bitmapHeight = (int)((float)card.Height * scale);
				System.Drawing.Bitmap bitmap = 
					new System.Drawing.Bitmap(bitmapWidth, bitmapHeight);
				bitmap.SetResolution(resolution, resolution);
				System.Drawing.Graphics graphics = 
					System.Drawing.Graphics.FromImage(bitmap);
				graphics.ScaleTransform(scale, scale);
				cView.PaintCard(graphics, dispQsos.Count > 0 ? dispQsos[0] : null,
				               96F / resolution);
				graphics.Dispose();
				
				System.Drawing.Imaging.ImageCodecInfo jpgEncoder = 
					GetEncoder(System.Drawing.Imaging.ImageFormat.Jpeg);
				System.Drawing.Imaging.Encoder qualityEncoder = System.Drawing.Imaging.Encoder.Quality;
				System.Drawing.Imaging.EncoderParameter encoderParameter =
					new System.Drawing.Imaging.EncoderParameter(qualityEncoder, quality);
				System.Drawing.Imaging.EncoderParameters encoderParams =
					new System.Drawing.Imaging.EncoderParameters(1);
				encoderParams.Param[0] = encoderParameter;
				bitmap.Save(fileName, jpgEncoder, encoderParams);
				bitmap.Dispose();
			}
		}

		/// <summary>
		/// Helper method to get the image encoder for the specified format
		/// </summary>
		/// <param name="format">ImageFormat for the encoder</param>
		/// <returns>Encoder for the specified image format</returns>
		private System.Drawing.Imaging.ImageCodecInfo GetEncoder(System.Drawing.Imaging.ImageFormat format)
		{
		
		    System.Drawing.Imaging.ImageCodecInfo[] codecs = 
		    	System.Drawing.Imaging.ImageCodecInfo.GetImageDecoders();
		
		    foreach (System.Drawing.Imaging.ImageCodecInfo codec in codecs)
		    {
		        if (codec.FormatID == format.Guid)
		        {
		            return codec;
		        }
		    }
		    return null;
		}

		/// <summary>
		/// Handle processing for Cards->Save Cards for Printing menu item
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void SaveCardsForPrintingCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			// use FolderBrowserDialog for user to select directory that files are to be save in
			System.Windows.Forms.FolderBrowserDialog folderBrowser = 
				new System.Windows.Forms.FolderBrowserDialog();
			folderBrowser.RootFolder = System.Environment.SpecialFolder.MyDocuments;
			folderBrowser.Description = "Select folder to save card images in";
			folderBrowser.ShowNewFolderButton = true;
			if(folderBrowser.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				string directory = folderBrowser.SelectedPath;
				try
				{
					// check the directory for preexisting files and make sure that they are not overwritten
					DirectoryInfo dirInfo = new DirectoryInfo(directory);
					FileInfo[] files = dirInfo.GetFiles("Qsl*.jpg");
					int startNum = 0;
					Regex regex = new Regex("^[0-9]*$");
					foreach (FileInfo fInfo in files)
					{
						string fName = fInfo.Name;
						fName = fName.Substring(3);
						fName = fName.Substring(0, fName.Length - 4);
						if(regex.IsMatch(fName))
						{
							Int32 num = Int32.Parse(fName);
							if(num > startNum)
							{
								startNum = num;
							}
						}
					}
					// now create and save card images in the directory
					SaveCardsAsJpegsForPrinting(startNum, directory);
					
				}
				catch(Exception ex)
				{
					App.Logger.Log(ex);
				}
			}
		}

		/// <summary>
		/// Saves cards with QSO info as JPEGs for photo printing
		/// </summary>
		/// <param name="startNumber">largest number (nnn) in file names QSLnnn.jpeg in print directory before printing starts</param>
		/// <param name="directoryName">path to directory that JPEG files are to be written to</param>
		/// <exception>DirectoryNotFoundException when directory has been deleted between time directory was selected and attempt
		/// to create or write file.</exception>
		/// <exception>SecurityException when user does not permission to access the directory or file (e.g. write permission on directory.</exception>
		/// <exception>IOException for an IO error</exception>
		/// <exception>PathTooLongException when the directory is longer than 248 characters, or filename is longer than 260 characters.</exception>
		/// <exception>Several others that definitely should never occur.</exception>
		private void SaveCardsAsJpegsForPrinting(int startNumber, string directoryName)
		{
			CardTabItem cti = mainTabControl.SelectedItem as CardTabItem;
			if(cti != null)
			{
				//create a list of QSOs for each card
				List<List<DispQso>> qsos = qsosView.DisplayQsos.GetDispQsosList(cti.cardPanel.QslCard);
				// now create file for each card
				for(int cardNum = 0; cardNum < qsos.Count; cardNum++)
				{
					CardWF card = cti.cardPanel.QslCard.Clone();
					card.IsInDesignMode = false;
					FormsCardView cView = new FormsCardView(card);
					// inform user of progress
					StatusText.Text = "Creating card " + (cardNum+1) + " of " + qsos.Count;
					StatusText.InvalidateVisual();
					ForceUIUpdate();
					// create the card and render it
					float scale = (float)300F / 100.0F;
					int bitmapWidth = (int)((float)card.Width * scale);
					int bitmapHeight = (int)((float)card.Height * scale);
					System.Drawing.Bitmap bitmap = 
						new System.Drawing.Bitmap(bitmapWidth, bitmapHeight);
					bitmap.SetResolution(300F, 300F);
					System.Drawing.Graphics graphics = 
						System.Drawing.Graphics.FromImage(bitmap);
					graphics.ScaleTransform(scale, scale);
					cView.PaintCard(graphics, qsos[cardNum], 96F / 300F);
					graphics.Dispose();
					string fileName = directoryName + "\\Qsl" + 
						(startNumber + cardNum + 1) + ".jpg";
					bitmap.Save(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
					bitmap.Dispose();
					
					// force garbage collection to prevent Out of Memory exception
					if(cardNum % 50 == 0)
					{
						System.GC.WaitForPendingFinalizers();
						System.GC.Collect();
					}
				}
				// tell user that all cards are created
				StatusText.Text = qsos.Count + " of " + qsos.Count + " cards created in directory: "
					+ directoryName;
			}
		}

		/// <summary>
		/// Handler for Cards->Save 4-Up Cards for Printing menu item
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void Save4UpCardsForPrintingCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Print4UpDialog p4D = new Print4UpDialog();
			if(p4D.ShowDialog() == true)
			{
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Image(*.jpg)|*.jpg";
                if (saveDialog.ShowDialog() == true)
                {
                    string fileName = saveDialog.FileName;

                    try
                    {
                        // save the card as a JPEG
                        Save4UpAsJpeg(fileName, p4D.Resolution, p4D.IncludeCardOutlines);
		                // tell user that the file has been created
		                StatusText.Text = "File " + fileName + " has been created.";
                    }
                    catch (Exception ex)
                    {
                    	App.Logger.Log(ex);
                    }
                }
            }
        }

		/// <summary>
		/// Helper method that saves 4 Up Cards in a JPEG file
		/// </summary>
		/// <param name="fileName">name of file to store image in</param>
		/// <param name="resolution">Resolution to store the image at</param>
		/// <param name="incBorders">Boolean indicating whether card edges should be included</param>
        private void Save4UpAsJpeg(string fileName, float resolution, bool incBorders)
        {
            // create visual of the card
			// create the card and render it
            CardTabItem cti = mainTabControl.SelectedItem as CardTabItem;
            CardWF cardToSave = cti.cardPanel.QslCard;
			float scale = resolution / 100.0F;
			int bitmapWidth = (int)((float)cardToSave.Width * scale * 2);
			int bitmapHeight = (int)((float)cardToSave.Height * scale * 2);
			System.Drawing.Bitmap bitmap = 
				new System.Drawing.Bitmap(bitmapWidth, bitmapHeight);
			bitmap.SetResolution(resolution, resolution);
			System.Drawing.Graphics graphics = 
				System.Drawing.Graphics.FromImage(bitmap);
			graphics.ScaleTransform(scale, scale);
            for (int cardNo = 0; cardNo <= 3; cardNo++)
            {
				CardWF card = cardToSave.Clone();
				card.IsInDesignMode = false;
				card.CardPrintProperties.PrintCardOutlines = incBorders;
				FormsCardView cView = new FormsCardView(card);
				
				System.Drawing.Drawing2D.GraphicsState state = graphics.Save();
	            float offX = (cardNo % 2 == 0) ? 0 : card.Width;
                float offY = (cardNo < 2) ? 0 : card.Height;
                graphics.TranslateTransform(offX, offY);
                cView.PaintCard(graphics, null, 96F / resolution);
                graphics.Restore(state);
            }
            graphics.Dispose();
            bitmap.Save(fileName, System.Drawing.Imaging.ImageFormat.Jpeg);
            bitmap.Dispose();
        }


		/// <summary>
		/// Display MessageBox showing the number of cards that would be printed given the
		/// current selections
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void CalculateCardsToBePrintedCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			CardTabItem cti = mainTabControl.SelectedItem as CardTabItem;
			if(cti != null)
			{
				
				List<List<DispQso>> qList = qsosView.DisplayQsos.GetDispQsosList(
						cti.cardPanel.QslCard);
				int cards = qList.Count;
				MessageBox.Show("Given the currently loaded and included QSOs, and the selected card,\r\n"
                                 + cards + " cards will be printed.",
                                 " Number of Cards to be Printed", MessageBoxButton.OK,
                                 MessageBoxImage.Information);
			}
		}
		
		/// <summary>
		/// Handler for Print Cards menu item Executed event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void PrintCardsCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			CardTabItem cti = mainTabControl.SelectedItem as CardTabItem;
			PrintProperties props = new PrintProperties(
				cti.cardPanel.QslCard.CardPrintProperties);
			if(App.Logger.DebugPrinting)
			{
				App.Logger.Log("PrintCardsCommand_Executed:" +
				               Environment.NewLine +
				               "Card size = " + cti.cardPanel.QslCard.Width +
				               " x " + cti.cardPanel.QslCard.Height +
				               Environment.NewLine +
				               props.ToString());
			}
			HamQSLerPrintDialog printDialog = new HamQSLerPrintDialog();
			printDialog.PrinterName = props.PrinterName;
			printDialog.PrinterPaperSize = props.PrinterPaperSize;
			printDialog.Resolution = props.Resolution;
			printDialog.Source = props.Source;
			printDialog.InsideMargins = props.InsideMargins;
			printDialog.PrintCardOutlines = props.PrintCardOutlines;
			printDialog.FillLastPage = props.FillLastPage;
			printDialog.SetCardMargins = props.SetCardMargins;
			printDialog.Layout = props.Layout;
			printDialog.CardWidth = cti.cardPanel.QslCard.Width;
			printDialog.CardHeight = cti.cardPanel.QslCard.Height;
			if(printDialog.ShowDialog() == true)
			{
				props.PrinterName = printDialog.PrinterName;
				props.PrinterPaperSize = printDialog.PrinterPaperSize;
				props.Resolution = printDialog.Resolution;
				props.Source = printDialog.Source;
				props.InsideMargins = printDialog.InsideMargins;
				props.PrintCardOutlines = printDialog.PrintCardOutlines;
				props.FillLastPage = printDialog.FillLastPage;
				props.SetCardMargins = printDialog.SetCardMargins;
				props.Layout = printDialog.Layout;
				if(App.Logger.DebugPrinting)
				{
					App.Logger.Log("PrintCardsCommand after printDialog.ShowDialog:" +
					               Environment.NewLine +
					               "Card size = " + cti.cardPanel.QslCard.Width +
					               " x " + cti.cardPanel.QslCard.Height +
					               Environment.NewLine +
					               props.ToString());
				}
				CardPrintDocument document = 
					new CardPrintDocument(qsosView.DisplayQsos);
				document.PrintProperties = props;
				document.QslCard = cti.cardPanel.QslCard;
				if(printDialog.Preview)
				{
					System.Windows.Forms.PrintPreviewDialog ppDialog = 
						new System.Windows.Forms.PrintPreviewDialog();
					ppDialog.Document = document;
					ppDialog.WindowState = System.Windows.Forms.FormWindowState.Maximized;
					ppDialog.ShowDialog();
				}
				else
				{
					document.Print();
				}
			}
			else
			{
				if(App.Logger.DebugPrinting)
				{
					App.Logger.Log("Printing canceled");
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
			qsosView.ClearDatesTimes();
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
					string error = importOrAdd(adifFileName, qsosView.SortOrder, App.AdifEnums);
					if(error != null)
					{
						App.Logger.Log(error);
						MessageBox.Show("Either there are errors in the ADIF file loaded, or" +
						                Environment.NewLine +
						                "individual QSO fields were modified during load." +
						                Environment.NewLine +
						                "See log file for details.", "Import Error or Change", 
						                MessageBoxButton.OK, MessageBoxImage.Warning);
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
		/// <param name="sender">not  used</param>
		/// <param name="e">not used</param>
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
			ExportQsos();
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
		internal void AddImageCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			CardTabItem cti = mainTabControl.SelectedItem as CardTabItem;
			if(cti != null)
			{
				SecondaryWFImage sImage = cti.cardPanel.AddImage();
				cti.SetPropertiesVisibility(sImage);
			}
		}
		
		/// <summary>
		/// Handler for Add Text Item menu item Executed (Clicked) command
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		internal void AddTextCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			CardTabItem cti = mainTabControl.SelectedItem as CardTabItem;
			if(cti != null)
			{
				TextWFItem tItem = cti.cardPanel.AddTextItem();
				cti.SetPropertiesVisibility(tItem);
			}
			
		}
		
		/// <summary>
		/// Handler for Add QsosBox menu item Executed (Clicked) command
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		internal void AddQsosBoxCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			CardTabItem cti = mainTabControl.SelectedItem as CardTabItem;
			if(cti != null)
			{
				QsosWFBox box = cti.cardPanel.AddQsosBox();
				cti.SetPropertiesVisibility(box);
			}
		}
		
		/// <summary>
		/// Handler for Delete Card Item menu item Executed (Clicked) command
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		internal void DeleteItemCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			CardTabItem cti = mainTabControl.SelectedItem as CardTabItem;
			if(cti != null)
			{
				cti.cardPanel.DeleteItem();
				cti.SetPropertiesVisibility(null);
			}
		}
		
		/// <summary>
		/// Handler for Clear Background menu item Executed (Clicked) command
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		internal void ClearBackgroundCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			CardTabItem cti = mainTabControl.SelectedItem as CardTabItem;
			if(cti != null)
			{
				cti.cardPanel.ClearBackgroundImage();
			}
		}
		
		/// <summary>
		/// Handles New -> Bureau Card menu item processing
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void NewBureauCardCommand_Executed(object sender, RoutedEventArgs e)
		{
			// create a CardTabItem and add it to the mainTabControl
			CardTabItem cardTab = new CardTabItem(550, 350);
			mainTabControl.Items.Add(cardTab);
			cardTab.IsSelected = true;		// select the new tab
			cardTab.cardPanel.QslCard.IsDirty = false;
			cardTab.SetTabLabel();
			// need to call SetTitle here because mainTabControl SelectionChanged event is not fired.
			SetTitle(cardTab.cardPanel.QslCard.FileName, cardTab.cardPanel.QslCard.IsDirty);
		}
		
		/// <summary>
		/// Handles New -> 4 1/4 by 5 1/2 inch Card menu item processing
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void New45CardCommand_Executed(object sender, RoutedEventArgs e)
		{
			// create a CardTabItem and add it to the mainTabControl
			CardTabItem cardTab = new CardTabItem(550, 425);
			mainTabControl.Items.Add(cardTab);
			cardTab.IsSelected = true;		// select the new tab
			cardTab.cardPanel.QslCard.IsDirty = false;
			cardTab.SetTabLabel();
			SetTitle(cardTab.cardPanel.QslCard.FileName, cardTab.cardPanel.QslCard.IsDirty);
		}

		/// <summary>
		/// Handles New -> 4 by 6 inch Card menu item processing
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void New46CardCommand_Executed(object sender, RoutedEventArgs e)
		{
			// create a CardTabItem and add it to the mainTabControl
			CardTabItem cardTab = new CardTabItem(600, 400);
			mainTabControl.Items.Add(cardTab);
			cardTab.IsSelected = true;		// select the new tab
			cardTab.cardPanel.QslCard.IsDirty = false;
			cardTab.SetTabLabel();
			SetTitle(cardTab.cardPanel.QslCard.FileName, cardTab.cardPanel.QslCard.IsDirty);
		}

		/// <summary>
		/// Creates the Select menu MenuItems based on the CardItems in QslCard if a card
		/// tab item is selected, or the select, deselect menu items if the QSOs tab is selected.
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void SelectMenu_SubmenuOpened(object sender, RoutedEventArgs e)
		{
			SelectMenu.Items.Clear();
			TabItem ti = mainTabControl.SelectedItem  as TabItem;
			CardTabItem cti = mainTabControl.SelectedItem as CardTabItem;
			if(cti != null)
			{
				BuildBackgroundMenuItem();
				BuildSecondaryImagesMenuItems();
				BuildTextItemsMenuItems();
				BuildQsosBoxMenuItem();
				SelectMenu.Items.Add(new Separator());
				BuildNoneMenuItem();
			}
			else if(ti.Header.Equals("QSOs"))
			{
				BuildSelectAllQsosMenuItem();
				BuildDeselectAllQsosMenuItem();
			}
		}
		
		/// <summary>
		/// Helper method that builds the Background menu item
		/// </summary>
		private void BuildBackgroundMenuItem()
		{
			MenuItem mi = new MenuItem();
			CardTabItem cti = mainTabControl.SelectedItem as CardTabItem;
			string fName = cti.cardPanel.QslCard.BackgroundImage.ImageFileName;
			if(fName != null && fName != string.Empty)
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
			mi.Tag = cti.cardPanel.QslCard.BackgroundImage;
			SelectMenu.Items.Add(mi);
		}
		
		/// <summary>
		/// Helper method that builds a MenuItem for each SecondaryImage on the card
		/// </summary>
		private void BuildSecondaryImagesMenuItems()
		{
			CardTabItem cti = mainTabControl.SelectedItem as CardTabItem;
			foreach(SecondaryWFImage si in cti.cardPanel.QslCard.SecondaryImages)
			{
				MenuItem mi = new MenuItem();
				string fName = si.ImageFileName;
				if(fName != null && fName != string.Empty)
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
			foreach(TextWFItem ti in cti.cardPanel.QslCard.TextItems)
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
			if(cti.cardPanel.QslCard.QsosBox != null)
			{
				MenuItem mi = new MenuItem();
				mi.Header = "Qsos Box";
				mi.Click += OnSelectItem_Clicked;
				mi.Command = SelectItemCommand;
				mi.Tag = cti.cardPanel.QslCard.QsosBox;
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
			none.ToolTip = "Deselect the selected card item";
			SelectMenu.Items.Add(none);
		}
		
		/// <summary>
		/// Helper method that builds the Select All QSOs menu item
		/// </summary>
		private void BuildSelectAllQsosMenuItem()
		{
			MenuItem selectAll = new MenuItem();
			selectAll.Header = "Select All QSOs";
			selectAll.Click += OnSelectAllQsos_Clicked;
			selectAll.ToolTip = "Select all loaded QSOs";
			SelectMenu.Items.Add(selectAll);
		}
		
		/// <summary>
		/// Helper method that builds the Deselect All QSOs menu item
		/// </summary>
		private void BuildDeselectAllQsosMenuItem()
		{
			MenuItem deselectAll = new MenuItem();
			deselectAll.Header = "Deselect All QSOs";
			deselectAll.Click += OnDeselectAllQsos_Clicked;
			deselectAll.ToolTip = "Deselect all loaded QSOs";
			SelectMenu.Items.Add(deselectAll);
		}
		
		/// <summary>
		/// Handler for SelectItems clicked event
		/// </summary>
		/// <param name="sender">menu item that was clicked</param>
		/// <param name="e">Not used</param>
		internal void OnSelectItem_Clicked(object sender, RoutedEventArgs e)
		{
			CardTabItem cti = mainTabControl.SelectedItem as CardTabItem;
			if(cti != null)
			{
				bool cardIsDirty = cti.cardPanel.QslCard.IsDirty;
				MenuItem mi = (MenuItem) sender;
				if(mi != null)
				{
					CardWFItem ci = ((MenuItem)sender).Tag as CardWFItem;
					if(ci != null)
					{
						ci.IsSelected = true;
						cti.SetPropertiesVisibility(ci);
					}
				}
				cti.cardPanel.QslCard.IsDirty = cardIsDirty;
			}
		}
		
		/// <summary>
		/// Handler for the None menu item clicked event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		internal void OnNone_Clicked(object sender, RoutedEventArgs e)
		{
			CardTabItem cti = mainTabControl.SelectedItem as CardTabItem;
			if(cti != null)
			{
				CardWFItem ci = cti.cardPanel.QslCard.GetSelectedItem();
				if(ci != null)
				{
					ci.IsSelected = false;
					cti.SetPropertiesVisibility(null);
				}
			}
		}

		/// <summary>
		/// Handler for the Select All QSOs menu item clicked event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		internal void OnSelectAllQsos_Clicked(object sender, RoutedEventArgs e)
		{
			qsosView.SelectAllQsos();
		}

		/// <summary>
		/// Handler for the Select All QSOs menu item clicked event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		internal void OnDeselectAllQsos_Clicked(object sender, RoutedEventArgs e)
		{
			qsosView.DeselectAllQsos();
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
		/// <param name="sender">not used</param>
		/// <param name="e">SelectionChangedEventArgs object</param>
		void MainTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			// all we do is set the window title based on tab selected
			CardTabItem ti = mainTabControl.SelectedItem as CardTabItem;
			if(ti != null)
			{
				SetTitle(ti.cardPanel.QslCard.FileName, ti.cardPanel.QslCard.IsDirty);
				ti.SetTabLabel();
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
		/// <param name="doNotReloadCard">Boolean indicating that the card should not
		/// be reloaded next time program is started.</param>
		public void CloseCardTab(CardTabItem cti, bool doNotReloadCard)
		{
			if(cti.cardPanel.QslCard.IsDirty)
			{
				MessageBoxResult result = MessageBox.Show("The design of this card has been modified.\n"
				                + "Do you want to save the card before closing?", "Card Modified", 
				                MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
				if(result == MessageBoxResult.Yes)
				{
					SaveCard(cti);
				}
			}
			mainTabControl.Items.Remove(cti);
			UserPreferences prefs = ((App)App.Current).UserPreferences;
			if(doNotReloadCard && prefs.CardsReloadOnStartup)
			{
				if(cti.cardPanel.QslCard.FileName != null)
				{
					prefs.CardFiles.Remove(cti.cardPanel.QslCard.FileName);
					prefs.SerializeAsXml();
				}
			}
			cti = null;
		}
		
		/// <summary>
		/// Save card to file named in Card.FileName
		/// <param name="cti">CardTabItem containing card to save</param>
		/// </summary>
		private void SaveCard(CardTabItem cti)
		{
			CardWF qslCard = cti.cardPanel.QslCard;
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
				SaveCardAs(cti);
			}

		}
		
		/// <summary>
		/// Save card to new file
		/// <param name="cti">CardTabItem containing card to save</param>
		/// </summary>
		private void SaveCardAs(CardTabItem cti)
		{
			CardWF qslCard = cti.cardPanel.QslCard;
			string fileName = qslCard.FileName;
			SaveFileDialog sDialog = new SaveFileDialog();
			sDialog.Filter = "QSL Card(*.xq1)|*.xq1";
			sDialog.InitialDirectory = 
				((App)Application.Current).UserPreferences.DefaultCardFilesFolder;
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

		/// <summary>
		/// Routine to force update of UI
		/// </summary>
		private void ForceUIUpdate() 
		{
			DispatcherFrame frame = new DispatcherFrame();
			Dispatcher.CurrentDispatcher.BeginInvoke(DispatcherPriority.Render, 
			                                         new DispatcherOperationCallback(delegate(object parameter)
                                                            {
                                                            	frame.Continue = false;
                                                            	return null;
                                                            }), null);
			Dispatcher.PushFrame(frame);
		}
		
		/// <summary>
		/// Handler for Help->User Manual - A4... menu item selected event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void UserManualA4Command_Executed(object sender, RoutedEventArgs e)
		{
			OpenUserManual("hamqsler-A4.pdf");
		}
		
		/// <summary>
		/// Handler for Help->User Manual - US Letter... menu item selected event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void UserManualLetterCommand_Executed(object sender, RoutedEventArgs e)
		{
			OpenUserManual("hamqsler-letter.pdf");
		}
		
		/// <summary>
		/// Open the user manual
		/// </summary>
		/// <param name="fileName">Name of the user manual file</param>
		private void OpenUserManual(string fileName)
		{
			string path = null;
			try
			{
				string baseDir = System.IO.Path.GetDirectoryName(this.GetType().Assembly.CodeBase);
				string userManual = "help\\pdf\\" + fileName;
				path = System.IO.Path.Combine(baseDir, userManual);
				System.Diagnostics.Process.Start(path);
			}
			catch (Exception ex)
			{
				// wrap the exception inside another with an appropriate message
				Exception exc = new Exception("Error encountered while trying to display User Manual" +
				                              Environment.NewLine +
				                              "Please report this error on the bug list " +
				                              " and include the contents of the log file", ex);
				exc.Data.Add("Path:", path);
				// now throw the exception inside a try/catch so we can log with the message
				try
				{
					throw exc;
				}
				catch (Exception except)
				{
					App.Logger.Log(except);
				}
			}
						
		}
		
		/// <summary>
		/// Handle Help->About... menu item event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void AboutCommand_Executed(object sender, RoutedEventArgs e)
		{
			AboutDialog aboutD = new AboutDialog();
			aboutD.ShowDialog();
		}
		
		/// <summary>
		/// Handle Help->HamQSLer Website menu item event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void WebsiteCommand_Executed(object sender, RoutedEventArgs e)
		{
			System.Diagnostics.Process.Start("http://www.va3hj.ca");			
		}
		
		/// <summary>
		/// Handler for Help=>View Log File menu item event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ViewLogFileCommand_Executed(object sender, RoutedEventArgs e)
		{
			try
			{
				string baseDir = ((App)Application.Current).HamqslerFolder;
				string path = System.IO.Path.Combine(baseDir, "Logs/hamqsler.log");
				System.Diagnostics.Process.Start(path);
			}
			catch (Exception ex)
			{
				// create new exception with better error message
				Exception exc = new Exception("Error encountered while trying to display the log file", ex);
				// now throw from with try/catch so it can be logged
				try
				{
					throw exc;
				}
				catch (Exception except)
				{
					App.Logger.Log(except);
				}
			}
			
		}
		
		/// <summary>
		/// Move files from QslDnP folder to hamqsler folder and convert
		/// QslDnP card files to hamqsler card files.
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void ImportQslDnP_Click(object sender, RoutedEventArgs e)
		{
			System.Windows.Forms.FolderBrowserDialog folderDialog =
				new System.Windows.Forms.FolderBrowserDialog();
			folderDialog.RootFolder = Environment.SpecialFolder.MyComputer;
			folderDialog.Description = "Select the directory to copy and convert files from:";
			System.Windows.Forms.DialogResult res = folderDialog.ShowDialog();
			if(res == System.Windows.Forms.DialogResult.OK)
			{
				string fromFolder = folderDialog.SelectedPath;
				folderDialog.Description = "Select the directory to copy files to. Do not select " +
					"the source directory";
				res = folderDialog.ShowDialog();
				if(res == System.Windows.Forms.DialogResult.OK)
				{
					string toFolder = folderDialog.SelectedPath;
					int copyCount = QslDnPTasks.MoveFiles(fromFolder, toFolder);
					if(QslDnPTasks.CopyError)
					{
						MessageBox.Show("One or more files could not be copied from your selected folder" +
						                Environment.NewLine +
						                "to your hamqsler folder. See the log file for information.",
						                "File Copy Error", MessageBoxButton.OK, MessageBoxImage.Warning);
					}
					if(copyCount < 1)
					{
						MessageBox.Show("No files were copied.", "Copy Error", MessageBoxButton.OK,
						                MessageBoxImage.Warning);
					}
					int convertCount = QslDnPTasks.ConvertCardFiles(fromFolder, toFolder);
					if(convertCount < 1)
					{
						MessageBox.Show("No card files were converted.", "Convert Error",
						                MessageBoxButton.OK, MessageBoxImage.Warning);
					}
					if(copyCount > 0 || convertCount > 0)
					{
						StatusText.Text = "Files have been copied and card files converted. " +
							"See log file for details.";
					}
					else
					{
						StatusText.Text = "No files were copied or card files converted.";
					}
				}
			}
		}
		
		/// <summary>
		/// Display dialog to select a printer and show its minumum margins
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void ShowPrinterMargins_Click(object sender, RoutedEventArgs e)
		{
			PrinterMarginsDialog marginsDialog = new PrinterMarginsDialog();
			marginsDialog.WindowStartupLocation = 
				WindowStartupLocation.CenterOwner;
			marginsDialog.Owner = this;
			marginsDialog.ShowDialog();
		}
		
		/// <summary>
		/// Handler for defineCustomPaperSizes menu item click event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void DefineCustomPaperSizes_Click(object sender, RoutedEventArgs e)
		{
			CustomPaperSizesDialog psDialog = new CustomPaperSizesDialog();
			psDialog.ShowDialog();
		}
		
		/// <summary>
		/// Close the qsosTab at program termination. If a QSO has been added or modified,
		/// ask if user wants the QSOs saved in an ADIF file, and save.
		/// </summary>
		private void CloseQsosTab()
		{
			if(qsosView.DisplayQsos.IsDirty)
			{
				MessageBoxResult result = MessageBox.Show("One or more QSOs has been added or modified.\n"
				                + "Do you want to save the QSOs before closing?", "QSOs Modified", 
				                MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.Yes);
				if(result == MessageBoxResult.Yes)
				{
					ExportQsos();
				}
			}
			mainTabControl.Items.Remove(qsosTab);
		}
		
		/// <summary>
		/// Save the loaded QSOs.
		/// </summary>
		private void ExportQsos()
		{
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
		/// Handle PropertyChanged events
		/// </summary>
		/// <param name="e">DependencyPropertyChangedEventArgs object describing the event</param>
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if(e.Property == ShowQsosBoxVerticalAnchorProperty)
			{
				TabItem[] tabItems = new TabItem[mainTabControl.Items.Count];
				mainTabControl.Items.CopyTo(tabItems, 0);
				foreach(TabItem ti in tabItems)
				{
					CardTabItem cardTab = ti as CardTabItem;
					if(cardTab != null)
					{
						cardTab.cardPanel.QslCard.RaiseDispPropertyChangedEvent();
					}
				}
			}
		}
	}
}