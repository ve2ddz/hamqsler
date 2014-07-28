/*
 *  Author:
 *       Jim Orcheson <jimorcheson@gmail.com>
 * 
 *  Copyright (c) 2012 - 2014 Jim Orcheson
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
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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
		
		private static int NOSELECTION = -1;
		private int qsoColumnSelected = NOSELECTION;
		
		private static int NUMMODEREPORTCOLUMNS = 3;

		private static readonly DependencyProperty UserPrefsProperty =
			DependencyProperty.Register("UserPrefs", typeof(UserPreferences),
			                                typeof(UserPreferencesDialog),
			                                new PropertyMetadata(null));
		private UserPreferences UserPrefs
		{
			get {return GetValue(UserPrefsProperty) as UserPreferences;}
			set {SetValue(UserPrefsProperty, value);}
		}
		
		private static readonly DependencyProperty ShowTypeByModeProperty =
			DependencyProperty.Register("ShowTypeByMode", typeof(bool),
			                            typeof(UserPreferencesDialog),
			                            new PropertyMetadata(false));
		
		public UserPreferencesDialog()
		{
			InitializeComponent();
			printPropertiesPanel.printerPropertiesGroupBox.Header = "Default Printer Properties";
			printPropertiesPanel.printPropertiesGroupBox.Header = "Default Print Properties";
			printPropertiesPanel.printerPropertiesGroupBox.Header = "Default Card Print Order";
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
			BuildReportTypeByModeGrid();
			SetSameReportTypeHeaderRadioButtons();
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
			   Validation.GetHasError(F630m) ||
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
//			((App)Application.Current).UserPreferences = new UserPreferences(UserPrefs);
			bool prefsInit;
			bool prefsError;
			((App)Application.Current).UserPreferences = UserPreferences.CreateUserPreferences(
				false, out prefsInit, out prefsError);
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
			Regex freqReg = new Regex("[0-9\\.,]");
			TextBox box = sender as TextBox;
			if(box != null && box.Text.Length >= box.MaxLength)
			{
				// too long
				Console.Beep(BEEPFREQUENCY, BEEPDURATION);		// alert user
				e.Handled = true;			// consume the event so that the character is not processed				
			}
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
			if(CallsignTextBox.Text.Length >= CallsignTextBox.MaxLength)
			{
				// too long
				Console.Beep(BEEPFREQUENCY, BEEPDURATION);		// alert user
				e.Handled = true;			// consume the event so that the character is not processed				
			}
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
			UserPrefs.PrintCardsVertical = printPropertiesPanel.PrintCardsVertical;
		}
		
		/// <summary>
		/// Event handler for DependencyProperty Changed event
		/// </summary>
		/// <param name="e">DependencyPropertyChangedEventArgs object</param>
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if(e.Property == UserPrefsProperty)
			{
				// pass the print properties on to printPropertiesPanel
				printPropertiesPanel.PrinterName = UserPrefs.DefaultPrinterName;
				printPropertiesPanel.PrinterPaperSize = UserPrefs.DefaultPaperSize;
				printPropertiesPanel.Resolution = UserPrefs.DefaultPrinterResolution;
				printPropertiesPanel.Source = UserPrefs.DefaultPaperSource;
				printPropertiesPanel.InsideMargins = UserPrefs.InsideMargins;
				printPropertiesPanel.PrintCardOutlines = UserPrefs.PrintCardOutlines;
				printPropertiesPanel.FillLastPage = UserPrefs.FillLastPage;
				printPropertiesPanel.SetCardMargins = UserPrefs.SetCardMargins;
				printPropertiesPanel.PrintCardsVertical = UserPrefs.PrintCardsVertical;
			}
		}
		
		/// <summary>
		/// Handler for PreviewTextInput events on TextBoxes - checks for length of input
		/// </summary>
		/// <param name="sender">Textbox being checked</param>
		/// <param name="e">TextCompositionEventArgs object</param>
		private void CheckLength(object sender, TextCompositionEventArgs e)
		{
			TextBox box = sender as TextBox;
			if(box != null)
			{
				if(box.Text.Length >= box.MaxLength)
				{
					Console.Beep(BEEPFREQUENCY, BEEPDURATION);
					e.Handled = true;
				}
			}
			
		}
		
		/// <summary>
		/// Handler for ProgramUpdatesCheckBox unchecked event - display warning when unchecking
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void ProgramUpdatesCheckBox_Unchecked(object sender, RoutedEventArgs e)
		{
			MessageBox.Show("You are disabling checks for new program versions as well as" +
			                Environment.NewLine +
			                "checks for new data files. You will not be informed that new" +
			                Environment.NewLine +
			                "versions of HamQSLer are available, and will not receive" +
			                Environment.NewLine +
			                "updates to files containing ADIF enumeration values, non-standard" +
			                Environment.NewLine +
			                "callsigns, and QSL Bureau changes.",
			                "Updates Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
		}
		
		/// <summary>
		/// Handler for QsoColumnsOrderListBox KeyUp event.
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">KeyEventArgs object</param>
		void QsoColumnsOrderListBox_KeyUp(object sender, KeyEventArgs e)
		{
			switch(e.Key)
			{
				case Key.Up:	// move selected column up in order
					if(qsoColumnSelected != 0)
					{
						string prevSel = ((TextBlock)qsoColumnsOrderListBox.Items[qsoColumnSelected]).Text;
						string newSel = ((TextBlock)qsoColumnsOrderListBox.Items[qsoColumnSelected - 1]).Text;
						((TextBlock)qsoColumnsOrderListBox.Items[qsoColumnSelected - 1]).Text = prevSel;
						((TextBlock)qsoColumnsOrderListBox.Items[qsoColumnSelected]).Text = newSel;
						qsoColumnSelected = qsoColumnsOrderListBox.SelectedIndex;
					}
					else
					{
						Console.Beep(BEEPFREQUENCY, BEEPDURATION);					
					}
					break;
				case Key.Down:	// move selected column down in order
					if(qsoColumnSelected != 5)
					{
						string prevSel = ((TextBlock)qsoColumnsOrderListBox.Items[qsoColumnSelected]).Text;
						string newSel = ((TextBlock)qsoColumnsOrderListBox.Items[qsoColumnSelected + 1]).Text;
						((TextBlock)qsoColumnsOrderListBox.Items[qsoColumnSelected + 1]).Text = prevSel;
						((TextBlock)qsoColumnsOrderListBox.Items[qsoColumnSelected]).Text = newSel;
						qsoColumnSelected = qsoColumnsOrderListBox.SelectedIndex;
					}
					else
					{
						Console.Beep(BEEPFREQUENCY, BEEPDURATION);					
					}
					break;
				case Key.Tab:	// select first column if none selected
					if(qsoColumnSelected == NOSELECTION)
					{
						qsoColumnsOrderListBox.SelectedIndex = 0;
						qsoColumnSelected = 0;
					}
					break;
			}
		}
		
		/// <summary>
		/// Handler for qsoColumnsOrderListBox MouseDown event
		/// </summary>
		/// <param name="sender">TextBlock item that was clicked</param>
		/// <param name="e">not used</param>
		void QsoColumnsOrderListBox_TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
		{
			// set the selected column to the one that was clicked
			qsoColumnSelected = qsoColumnsOrderListBox.Items.IndexOf(sender);
		}
		
		/// <summary>
		/// Helper method that builds report type by mode panel contents
		/// </summary>
		private void BuildReportTypeByModeGrid()
		{
			List<DataItem> modeItems = UserPrefs.ReportByMode;
			int itemsPerColumn = (modeItems.Count + NUMMODEREPORTCOLUMNS - 1) / NUMMODEREPORTCOLUMNS;
			CreateRowDefinitionsForReportTypeByModeGrid(itemsPerColumn);
			int item = 0;
			while(item < modeItems.Count)
			{
				CreateColumnItem(modeItems[item], item, itemsPerColumn);
				item++;
			}
		}
		
		/// <summary>
		/// Helper method that creates the row definitions within the report type
		/// by mode panel
		/// </summary>
		/// <param name="rows">number of rows to create RowDefinitions for</param>
		private void CreateRowDefinitionsForReportTypeByModeGrid(int rows)
		{
			for(int row = 0; row < rows; row++)
			{
				RowDefinition rowDef = new RowDefinition();
				reportTypeByModeGrid.RowDefinitions.Add(rowDef);
			}
		}
		
		/// <summary>
		/// Helper method that creates a single mode item and its radio buttons in
		/// the report type by mode panel
		/// </summary>
		/// <param name="modeItem">DataItem containing the mode</param>
		/// <param name="itemNumber">The number of the mode item within the panel</param>
		/// <param name="itemsPerColumn">Number of items contained in each column in
		/// the panel</param>
		private void CreateColumnItem(DataItem modeItem, int itemNumber, int itemsPerColumn)
		{
			TextBlock mode = new TextBlock();
			mode.Text = modeItem.Key;
			mode.Margin = new Thickness(2);
			mode.VerticalAlignment = VerticalAlignment.Center;
			int column = (itemNumber / itemsPerColumn) * 6;
			int row = (itemNumber % itemsPerColumn) + 1;
			Grid.SetRow(mode, row);
			Grid.SetColumn(mode, column);
			reportTypeByModeGrid.Children.Add(mode);
			CreateAndAddRadioButton(modeItem, "RS", column + 1, row);
			CreateAndAddRadioButton(modeItem, "RST", column + 2, row);
			CreateAndAddRadioButton(modeItem, "RSQ", column + 3, row);
			CreateAndAddRadioButton(modeItem, "dB", column + 4, row);
		}
		
		/// <summary>
		/// Helper method that adds a radio button for the mode in the report type
		/// by mode panel.
		/// </summary>
		/// <param name="item">DataItem containing the mode</param>
		/// <param name="modeType">Indicator of the radio button to check</param>
		/// <param name="column">Column number to place the radio button in</param>
		/// <param name="row">Row number to place the radio button in</param>
		private void CreateAndAddRadioButton(DataItem item, string modeType, int column, int row)
		{
			RadioButton rb = new RadioButton();
			rb.Tag = modeType;
			rb.GroupName = item.Key;
			rb.Margin = new Thickness(2);
			rb.VerticalAlignment = VerticalAlignment.Center;
			rb.HorizontalAlignment = HorizontalAlignment.Center;
			Grid.SetRow(rb, row);
			Grid.SetColumn(rb, column);
			rb.IsChecked = item.Value.Equals(modeType);
			rb.Checked += ModeRadioButton_Checked;	// place after rb.IsChecked set so that Checked
													// not called when first creating the table
			reportTypeByModeGrid.Children.Add(rb);
		}
		
		/// <summary>
		/// Helper method that sets AllModesRadioButton and ByModeRadioButton IsChecked
		/// property based on setting of ReportTypeByMode value in UserPreferences.
		/// This method should be called only from the UserPreferencesDialog constructor.
		/// </summary>
		private void SetSameReportTypeHeaderRadioButtons()
		{
			AllModesRadioButton.IsChecked = !UserPrefs.ReportTypeByMode;
			ByModeRadioButton.IsChecked = UserPrefs.ReportTypeByMode;
		}
		
		/// <summary>
		/// Sets UserPreferences.ReportTypeByMode whenever there is a change to ByModeRadioButton
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void ByModeRadioButton_Checked(object sender, RoutedEventArgs e)
		{
			UserPrefs.ReportTypeByMode = (bool)ByModeRadioButton.IsChecked;
		}
		
		/// <summary>
		/// Handler for Checked event for any report by mode change.
		/// </summary>
		/// <param name="sender">RadioButton object that was checked</param>
		/// <param name="e">not used</param>
		private void ModeRadioButton_Checked(object sender, RoutedEventArgs e)
		{
			RadioButton button = sender as RadioButton;
			foreach(DataItem item in UserPrefs.ReportByMode)
			{
				if(item.Key.Equals(button.GroupName))
				{
					item.Value = button.Tag as string;
					break;
				}
			}
		}
	}
}