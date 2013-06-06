/*
 *  Author:
 *       Jim Orcheson <jimorcheson@gmail.com>
 * 
 *  Copyright (c) 2012, 2013 Jim Orcheson
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
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace hamqsler
{
	/// <summary>
	/// Interaction logic for UserPreferencesDialog.xaml
	/// </summary>
	public partial class UserPreferencesDialog : Window
	{
		// routed commands
		public static RoutedCommand ApplyButtonCommand = new RoutedCommand();
		public static RoutedCommand OkButtonCommand = new RoutedCommand();
		public static RoutedCommand CancelButtonCommand = new RoutedCommand();

		private static int BEEPFREQUENCY = 800;		// Hz
		private static int BEEPDURATION = 200;		// ms

		private static readonly DependencyProperty UserPrefsProperty =
			DependencyProperty.Register("UserPrefs", typeof(UserPreferences),
			                                typeof(UserPreferencesDialog),
			                                new PropertyMetadata(null));
		private UserPreferences UserPrefs
		{
			get {return GetValue(UserPrefsProperty) as UserPreferences;}
			set {SetValue(UserPrefsProperty, value);}
		}
		
		public UserPreferencesDialog()
		{
			InitializeComponent();
			printPropertiesPanel.printerPropertiesGroupBox.Header = "Default Printer Properties";
			printPropertiesPanel.printPropertiesGroupBox.Header = "Default Print Properties";
			printPropertiesPanel.cardsLayoutGroupBox.Visibility = Visibility.Collapsed;
			printPropertiesPanel.PrintPropertiesChanged += OnPrintPropertiesChanged;
			Mouse.OverrideCursor = Cursors.Arrow;
			// create a clone of the UserPreferences object
			UserPrefs = new UserPreferences(((App)Application.Current).UserPreferences);
			propertiesDisplay.DataContext = UserPrefs;
			if(UserPrefs.Callsign.Count == 1 && UserPrefs.Callsign[0].GetType() == typeof(StaticText))
			{
				CallsignTextBox.DataContext = (StaticText)UserPrefs.Callsign[0];
			}
			else
			{
				CallsignTextBox.Visibility = Visibility.Collapsed;
			}
			if(UserPrefs.NameQth.Count == 1 && UserPrefs.NameQth[0].GetType() == typeof(StaticText))
			{
				NameQthTextBox.DataContext = (StaticText)UserPrefs.NameQth[0];
			}
			else
			{
				NameQthTextBox.Visibility = Visibility.Collapsed;
			}
			if(UserPrefs.Salutation.Count == 1 && UserPrefs.Salutation[0].GetType() == typeof(StaticText))
			{
				SalutationTextBox.DataContext = (StaticText)UserPrefs.Salutation[0];
			}
			else
			{
				SalutationTextBox.Visibility = Visibility.Collapsed;
			}
			if(UserPrefs.ConfirmingText.Count == 1 && 
			   UserPrefs.ConfirmingText[0].GetType() == typeof(StaticText))
			{
				ConfirmingTextBox.DataContext = (StaticText)UserPrefs.ConfirmingText[0];
			}
			else
			{
				ConfirmingTextBox.Visibility = Visibility.Collapsed;
			}
			// load list of font names that are available to Windows Forms
			System.Drawing.Text.InstalledFontCollection fontCol =
				new System.Drawing.Text.InstalledFontCollection();
			foreach(System.Drawing.FontFamily family in fontCol.Families)
			{
				DefaultTextItemsFontFaceComboBox.Items.Add(family.Name);
				DefaultQsosBoxFontFaceComboBox.Items.Add(family.Name);
			}

		}
		
		/// <summary>
		/// Handles CanExecute command for OK and Cancel buttons
		/// </summary>
		/// <param name="sender">Not used</param>
		/// <param name="e">CanExecuteRoutedEventArgs object</param>
		private void OkAndApplyButtonCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = true;
			if(Validation.GetHasError(F2190m) ||
			  Validation.GetHasError(F560m)	||
			  Validation.GetHasError(F160m) ||
			  Validation.GetHasError(F80m)	||
			  Validation.GetHasError(F60m)	||
			  Validation.GetHasError(F40m)	||
			  Validation.GetHasError(F30m)	||
			  Validation.GetHasError(F20m)	||
			  Validation.GetHasError(F17m)	||
			  Validation.GetHasError(F15m)	||
			  Validation.GetHasError(F12m)	||
			  Validation.GetHasError(F10m)	||
			  Validation.GetHasError(F6m)	||
			  Validation.GetHasError(F4m)	||
			  Validation.GetHasError(F2m)	||
			  Validation.GetHasError(F1p25m)||
			  Validation.GetHasError(F70cm)	||
			  Validation.GetHasError(F33cm)	||
			  Validation.GetHasError(F23cm)	||
			  Validation.GetHasError(F13cm)	||
			  Validation.GetHasError(F9cm)	||
			  Validation.GetHasError(F6cm)	||
			  Validation.GetHasError(F3cm)	||
			  Validation.GetHasError(F1p25cm) ||
			  Validation.GetHasError(F6mm)	||
			  Validation.GetHasError(F4mm)	||
			  Validation.GetHasError(F2p5mm)||
			  Validation.GetHasError(F2mm)	||
			  Validation.GetHasError(F1mm))
			{
				e.CanExecute = false;
			}
		}
		
		/// <summary>
		/// Handles OK button clicked event
		/// </summary>
		/// <param name="sender">Button that was clicked</param>
		/// <param name="e">ExecutedRoutedEventArgs object</param>
		private void OkAndApplyButtonCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			UserPrefs.SerializeAsXml();
			((App)Application.Current).UserPreferences = UserPrefs;
			e.Handled = true;
			Button button = e.OriginalSource as Button;
			if(button != null && button.Name == "okButton")
			{
				this.Close();
			}
		}
		
		/// <summary>
		/// Handles Cancel button clicked event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void CancelButtonCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			this.Close();
		}
		
		/// <summary>
		/// Handler for DefaultAdifFilesFolderButton click events.
		/// Allows user to select a default folder to contain the ADIF files
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">RoutedEventArgs object</param>
		private void DefaultAdifFilesFolderButton_Click(object sender, RoutedEventArgs e)
		{
			//open and display FolderBrowserDialog
			System.Windows.Forms.FolderBrowserDialog folderDialog = 
					new System.Windows.Forms.FolderBrowserDialog();
			folderDialog.Description = "Select the default folder for ADIF files";
			folderDialog.SelectedPath = UserPrefs.DefaultAdifFilesFolder;
			
			System.Windows.Forms.DialogResult res = folderDialog.ShowDialog();
			if(res == System.Windows.Forms.DialogResult.OK)
			{
				// new folder selected, so update UserPreferences object
				UserPrefs.DefaultAdifFilesFolder = folderDialog.SelectedPath;
			}
			e.Handled = true;
		}
		
		/// <summary>
		/// Handler for DefaultCardFilesFolderButton click events.
		/// Allows user to select a default folder to contain the card files
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">RoutedEventArgs object</param>
		private void DefaultCardFilesFolderButton_Click(object sender, RoutedEventArgs e)
		{
			//open and display FolderBrowserDialog
			System.Windows.Forms.FolderBrowserDialog folderDialog = 
					new System.Windows.Forms.FolderBrowserDialog();
			folderDialog.Description = "Select the default folder for card files";
			folderDialog.SelectedPath = UserPrefs.DefaultCardFilesFolder;
			
			System.Windows.Forms.DialogResult res = folderDialog.ShowDialog();
			if(res == System.Windows.Forms.DialogResult.OK)
			{
				// new folder selected, so update UserPreferences object
				UserPrefs.DefaultCardFilesFolder = folderDialog.SelectedPath;
			}
			e.Handled = true;
		}
		
		/// <summary>
		/// Handler for DefaultImagesFolderButton click events.
		/// Allows user to select a default folder that contain image files
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">RoutedEventArgs object</param>
		private void DefaultImagesFolderButton_Click(object sender, RoutedEventArgs e)
		{
			//open and display FolderBrowserDialog
			System.Windows.Forms.FolderBrowserDialog folderDialog = 
					new System.Windows.Forms.FolderBrowserDialog();
			folderDialog.Description = "Select the default folder for image files";
			folderDialog.SelectedPath = UserPrefs.DefaultImagesFolder;
			
			System.Windows.Forms.DialogResult res = folderDialog.ShowDialog();
			if(res == System.Windows.Forms.DialogResult.OK)
			{
				// new folder selected, so update UserPreferences object
				UserPrefs.DefaultImagesFolder = folderDialog.SelectedPath;
			}
			e.Handled = true;
		}
		
		/// <summary>
		/// Preview text input to Band/Frequency TextBoxes and consume any non-valid characters.
		/// Note that only 0-9 and '.' are allowed in frequencies.
		/// Frequencies are validated when keyboard focus leaves the TextBox.
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">TextCompositionEventArgs object. This object has a
		/// property that contains the character that was entered.</param>
		private void BandFrequency_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			Regex freqReg = new Regex("[0-9\\.]");
			if(!freqReg.IsMatch(e.Text))	// check valid character
			{
				// not valid
				Console.Beep(BEEPFREQUENCY, BEEPDURATION);		// alert user
				e.Handled = true;			// consume the event so that the character is not processed
			}
		}
		
		/// <summary>
		/// Preview text input to CallsignBox and consume any non-valid characters.
		/// Note that only A-Z, a-z, 0-9, and / are allowed in callsigns
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">TextCompositionEventArgs object. This object has a
		/// property that contains the character that was entered.</param>
		void CallsignBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			Regex callReg = new Regex("[A-Za-z0-9/]");
			if(!callReg.IsMatch(e.Text))	// check valid character
			{
				// not valid
				Console.Beep(BEEPFREQUENCY, BEEPDURATION);		// alert user
				e.Handled = true;			// consume the event so that the character is not processed
			}
		}
		
		/// <summary>
		/// Handler for CallsignMacroButton Clicked event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void CallsignMacroButton_Click(object sender, RoutedEventArgs e)
		{
			TextMacrosDialog dialog = new TextMacrosDialog(UserPrefs.Callsign);
			dialog.ShowDialog();
			if(UserPrefs.Callsign.Count == 1 && 
			   (UserPrefs.Callsign[0].GetType() == typeof(StaticText)))
			{
				CallsignTextBox.Visibility = Visibility.Visible;
				CallsignTextBox.Text = ((StaticText)UserPrefs.Callsign[0]).Text;
			}
			else
			{
				CallsignTextBox.Visibility = Visibility.Collapsed;
			}
		}
		
		/// <summary>
		/// Handler for NameQTHMacroButton Clicked event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void NameQTHMacroButton_Click(object sender, RoutedEventArgs e)
		{
			TextMacrosDialog dialog = new TextMacrosDialog(UserPrefs.NameQth);
			dialog.ShowDialog();
			if(UserPrefs.NameQth.Count == 1 &&
			   (UserPrefs.NameQth[0].GetType() == typeof(StaticText)))
			{
				NameQthTextBox.Visibility = Visibility.Visible;
				NameQthTextBox.Text = ((StaticText)UserPrefs.NameQth[0]).Text;
			}
			else
			{
				NameQthTextBox.Visibility = Visibility.Collapsed;
			}
		}
		
		/// <summary>
		/// Handler for SalutationMacroButton Clicked event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void SalutationMacroButton_Click(object sender, RoutedEventArgs e)
		{
			TextMacrosDialog dialog = new TextMacrosDialog(UserPrefs.Salutation);
			dialog.ShowDialog();
			if(UserPrefs.Salutation.Count == 1 &&
			   (UserPrefs.Salutation[0].GetType() == typeof(StaticText)))
 		    {
		   		SalutationTextBox.Visibility = Visibility.Visible;
		   		SalutationTextBox.Text = ((StaticText)UserPrefs.Salutation[0]).Text;
		    }
			else
			{
				SalutationTextBox.Visibility = Visibility.Collapsed;
			}
		}
		
		/// <summary>
		/// Hander for ConfirmingTextMacroButton Clicked event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void ConfirmingTextMacroButton_Click(object sender, RoutedEventArgs e)
		{
			TextMacrosDialog dialog = new TextMacrosDialog(UserPrefs.ConfirmingText);
			dialog.ShowDialog();
			if(UserPrefs.ConfirmingText.Count == 1 &&
			   UserPrefs.ConfirmingText[0].GetType() == typeof(StaticText))
			{
				ConfirmingTextBox.Visibility = Visibility.Visible;
				ConfirmingTextBox.Text = ((StaticText)UserPrefs.ConfirmingText[0]).Text;
			}
			else
			{
				ConfirmingTextBox.Visibility = Visibility.Collapsed;
			}
		}
		
		/// <summary>
		/// Handler for printPropertiesPanel PrintProperties changed event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void OnPrintPropertiesChanged(object sender, EventArgs e)
		{
			UserPrefs.DefaultPrinterName = printPropertiesPanel.PrinterName;
			UserPrefs.DefaultPaperSize = printPropertiesPanel.PrinterPaperSize;
			UserPrefs.DefaultPrinterResolution = printPropertiesPanel.Resolution;
			UserPrefs.DefaultPaperSource = printPropertiesPanel.Source;
			UserPrefs.InsideMargins = printPropertiesPanel.InsideMargins;
			UserPrefs.PrintCardOutlines = printPropertiesPanel.PrintCardOutlines;
			UserPrefs.FillLastPage = printPropertiesPanel.FillLastPage;
			UserPrefs.SetCardMargins = printPropertiesPanel.SetCardMargins;
		}
		
		/// <summary>
		/// Event handler for DependencyProperty Changed event
		/// </summary>
		/// <param name="e">DependencyPropertyChangedEventArgs object</param>
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if(e.Property ==UserPrefsProperty)
			{
				// pass the print properties on to printPropertiesPanel
				UserPreferences prefs = new UserPreferences(UserPrefs);
				printPropertiesPanel.PrinterName = prefs.DefaultPrinterName;
				printPropertiesPanel.PrinterPaperSize = prefs.DefaultPaperSize;
				printPropertiesPanel.Resolution = prefs.DefaultPrinterResolution;
				printPropertiesPanel.Source = prefs.DefaultPaperSource;
				printPropertiesPanel.InsideMargins = prefs.InsideMargins;
				printPropertiesPanel.PrintCardOutlines = prefs.PrintCardOutlines;
				printPropertiesPanel.FillLastPage = prefs.FillLastPage;
				printPropertiesPanel.SetCardMargins = prefs.SetCardMargins;
			}
		}
		
	}
}