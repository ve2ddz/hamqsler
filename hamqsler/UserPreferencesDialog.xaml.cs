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
using System.Text.RegularExpressions;
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
		// routed commands
		public static RoutedCommand ApplyButtonCommand = new RoutedCommand();
		public static RoutedCommand OkButtonCommand = new RoutedCommand();
		public static RoutedCommand CancelButtonCommand = new RoutedCommand();

		private static int BEEPFREQUENCY = 800;		// Hz
		private static int BEEPDURATION = 200;		// ms

		private UserPreferences userPrefs;
		
		public UserPreferencesDialog()
		{
			InitializeComponent();

			// create a clone of the UserPreferences object
			userPrefs = new UserPreferences(((App)Application.Current).UserPreferences);
			propertiesDisplay.DataContext = userPrefs;
			if(userPrefs.Callsign.Count == 1 && userPrefs.Callsign[0].GetType() == typeof(StaticText))
			{
				CallsignTextBox.DataContext = (StaticText)userPrefs.Callsign[0];
			}
			else
			{
				CallsignTextBox.Visibility = Visibility.Collapsed;
			}
			if(userPrefs.NameQth.Count == 1 && userPrefs.NameQth[0].GetType() == typeof(StaticText))
			{
				NameQthTextBox.DataContext = (StaticText)userPrefs.NameQth[0];
			}
			else
			{
				NameQthTextBox.Visibility = Visibility.Collapsed;
			}
			if(userPrefs.Salutation.Count == 1 && userPrefs.Salutation[0].GetType() == typeof(StaticText))
			{
				SalutationTextBox.DataContext = (StaticText)userPrefs.Salutation[0];
			}
			else
			{
				SalutationTextBox.Visibility = Visibility.Collapsed;
			}
			if(userPrefs.ConfirmingText.Count == 1 && 
			   userPrefs.ConfirmingText[0].GetType() == typeof(StaticText))
			{
				ConfirmingTextBox.DataContext = (StaticText)userPrefs.ConfirmingText[0];
			}
			else
			{
				ConfirmingTextBox.Visibility = Visibility.Collapsed;
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
			userPrefs.SerializeAsXml();
			((App)Application.Current).UserPreferences = userPrefs;
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
		private void DefaultCardFilesFolderButton_Click(object sender, RoutedEventArgs e)
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
			folderDialog.SelectedPath = userPrefs.DefaultImagesFolder;
			
			System.Windows.Forms.DialogResult res = folderDialog.ShowDialog();
			if(res == System.Windows.Forms.DialogResult.OK)
			{
				// new folder selected, so update UserPreferences object
				userPrefs.DefaultImagesFolder = folderDialog.SelectedPath;
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
			TextMacrosDialog dialog = new TextMacrosDialog(userPrefs.Callsign);
			dialog.ShowDialog();
			if(userPrefs.Callsign.Count == 1 && 
			   (userPrefs.Callsign[0].GetType() == typeof(StaticText)))
			{
				CallsignTextBox.Visibility = Visibility.Visible;
				CallsignTextBox.Text = ((StaticText)userPrefs.Callsign[0]).Text;
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
			TextMacrosDialog dialog = new TextMacrosDialog(userPrefs.NameQth);
			dialog.ShowDialog();
			if(userPrefs.NameQth.Count == 1 &&
			   (userPrefs.NameQth[0].GetType() == typeof(StaticText)))
			{
				NameQthTextBox.Visibility = Visibility.Visible;
				NameQthTextBox.Text = ((StaticText)userPrefs.NameQth[0]).Text;
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
			TextMacrosDialog dialog = new TextMacrosDialog(userPrefs.Salutation);
			dialog.ShowDialog();
			if(userPrefs.Salutation.Count == 1 &&
			   (userPrefs.Salutation[0].GetType() == typeof(StaticText)))
 		    {
		   		SalutationTextBox.Visibility = Visibility.Visible;
		   		SalutationTextBox.Text = ((StaticText)userPrefs.Salutation[0]).Text;
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
			TextMacrosDialog dialog = new TextMacrosDialog(userPrefs.ConfirmingText);
			dialog.ShowDialog();
			if(userPrefs.ConfirmingText.Count == 1 &&
			   userPrefs.ConfirmingText[0].GetType() == typeof(StaticText))
			{
				ConfirmingTextBox.Visibility = Visibility.Visible;
				ConfirmingTextBox.Text = ((StaticText)userPrefs.ConfirmingText[0]).Text;
			}
			else
			{
				ConfirmingTextBox.Visibility = Visibility.Collapsed;
			}
		}
	}
}