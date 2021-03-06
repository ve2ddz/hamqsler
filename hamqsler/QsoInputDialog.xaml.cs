﻿/*
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
using System.Windows.Input;

namespace hamqsler
{
	/// <summary>
	/// Interaction logic for QsoInputDialog.xaml
	/// </summary>
	public partial class QsoInputDialog : Window
	{
		// routed commands
		public static RoutedCommand AddButtonCommand = new RoutedCommand();
		public static RoutedCommand OkButtonCommand = new RoutedCommand();
		public static RoutedCommand CancelButtonCommand = new RoutedCommand();
		
		private static int BEEPFREQUENCY = 800;		// Hz
		private static int BEEPDURATION = 200;		// ms
		private DisplayQsos dispQsos;
		private QsoInputData QsoData = new QsoInputData();
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="qsos">DisplayQsos object to add Qsos to</param>
		public QsoInputDialog(DisplayQsos qsos)
		{
			dispQsos = qsos;
			InitializeComponent();
			LoadModes();
			this.DataContext = QsoData;
		}
		
		/// <summary>
		/// CanExecute handler for Add+ button
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">CanExecuteRoutedEventArgs object</param>
		private void AddButtonCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute =  QsoData["Callsign"] == null && QsoData["Manager"] == null &&
			                 QsoData["StartDate"] == null && QsoData["StartTime"] == null &&
			                 !QsoData.Mode.Equals(string.Empty) &&
			                 QsoData["Band"] == null && QsoData["Frequency"] == null &&
							 (!QsoData.Band.Equals(string.Empty) ||
				              !QsoData.Frequency.Equals(string.Empty));
		}
		
		/// <summary>
		/// CanExecute handler for OK button
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">CanExecuteRoutedEventArgs object</param>
		private void OkButtonCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute =  QsoData["Callsign"] == null && QsoData["Manager"] == null &&
			                 QsoData["StartDate"] == null && QsoData["StartTime"] == null &&
			                 !QsoData.Mode.Equals(string.Empty) &&
			                 QsoData["Band"] == null && QsoData["Frequency"] == null &&
							 (!QsoData.Band.Equals(string.Empty) ||
				              !QsoData.Frequency.Equals(string.Empty));
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
			if(CallsignBox.Text.Length >= CallsignBox.MaxLength)
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
		/// Preview text input to ManagerBox and consume any non-valid characters.
		/// Note that only A-Z, a-z, 0-9 are allowed in manager.
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">TextCompositionEventArgs object. This object has a
		/// property that contains the character that was entered.</param>
		void ManagerBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			Regex mgrReg = new Regex("[A-Za-z0-9]");
			if(ManagerBox.Text.Length >= ManagerBox.MaxLength)
			{
				// too long
				Console.Beep(BEEPFREQUENCY, BEEPDURATION);		// alert user
				e.Handled = true;			// consume the event so that the character is not processed
			}
			if(!mgrReg.IsMatch(e.Text))	// check valid character
			{
				// not valid
				Console.Beep(BEEPFREQUENCY, BEEPDURATION);		// alert user
				e.Handled = true;			// consume the event so that the character is not processed
			}
		}
		
		/// <summary>
		/// Preview text input to StartDateBox and consume any non-valid characters.
		/// Note that only 0-9 are allowed in dates.
		/// Dates are validated when keyboard focus leaves the TextBox.
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">TextCompositionEventArgs object. This object has a
		/// property that contains the character that was entered.</param>
		void StartDateBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			Regex dateReg = new Regex("[0-9]");
			if(StartDateBox.Text.Length >= StartDateBox.MaxLength)
			{
				// too much input
				Console.Beep(BEEPFREQUENCY, BEEPDURATION);
				e.Handled = true;
			}
			if(!dateReg.IsMatch(e.Text))	// check valid character
			{
				// not valid
				Console.Beep(BEEPFREQUENCY, BEEPDURATION);		// alert user
				e.Handled = true;			// consume the event so that the character is not processed
			}
		}
		
		/// <summary>
		/// Preview text input to StartTimeBox and consume any non-valid characters.
		/// Note that only 0-9 are allowed in times.
		/// Times are validated when keyboard focus leaves the TextBox.
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">TextCompositionEventArgs object. This object has a
		/// property that contains the character that was entered.</param>
		void StartTimeBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			Regex timeReg = new Regex("[0-9]");
			if(StartTimeBox.Text.Length >= StartTimeBox.MaxLength)
			{
				// too long
				Console.Beep(BEEPFREQUENCY, BEEPDURATION);		// alert user
				e.Handled = true;			// consume the event so that the character is not processed
			}
			if(!timeReg.IsMatch(e.Text))	// check valid character
			{
				// not valid
				Console.Beep(BEEPFREQUENCY, BEEPDURATION);		// alert user
				e.Handled = true;			// consume the event so that the character is not processed
			}
		}
		
		/// <summary>
		/// Preview text input to FrequencyBox and consume any non-valid characters.
		/// Note that only 0-9, ',', and '.' are allowed in frequencies.
		/// Frequencies are validated when keyboard focus leaves the TextBox.
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">TextCompositionEventArgs object. This object has a
		/// property that contains the character that was entered.</param>
		void FrequencyBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			Regex freqReg = new Regex("[0-9,\\.]");
			if(FrequencyBox.Text.Length >= FrequencyBox.MaxLength)
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
		/// Executed handler for Add+ button. Saves the QSO info
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void AddButtonCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			SaveQso();		
		}

		/// <summary>
		/// Helper method that saves a QSO, then clears the fields in QsoInputDialog
		/// </summary>
		/// <returns>false if there is an error in the QSO data, true otherwiser</returns>
		private bool SaveQso()
		{
			Qso2 qso = BuildQsoFromInput();
			QsoWithInclude qwi = new QsoWithInclude(qso);
			dispQsos.AddQso(qwi);
			QsoData.ClearQsoData();
			return true;
		}
		
		/// <summary>
		/// Executed handler for OK button. Saves QSO info and closes dialog
		/// </summary>
		/// <param name="sender">source for the event</param>
		/// <param name="e">ExecutedRoutedEventArgs</param>
		void OkButtonCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			if(SaveQso())
			{
				this.Close();
			}
		}
		
		/// <summary>
		/// Executed handler for Cancel button. Just closes the dialog without saving QSO info
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void CancelButtonCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			this.Close();
		}
		
		/// <summary>
		/// Helper method that builds a QSO object from the info input in dialog
		/// </summary>
		/// <returns>QSO object built from input</returns>
		private Qso2 BuildQsoFromInput()
		{
			string err = string.Empty;
			Qso2 qso = new Qso2(string.Empty, App.AdifEnums, ref err, null);
			qso["call"] = QsoData.Callsign;
			if(QsoData.Manager != string.Empty)
			{
				qso["qsl_via"] = QsoData.Manager;
			}
			qso["qso_date"] = QsoData.StartDate;
			qso["time_on"] = QsoData.StartTime;
			qso["mode"] = QsoData.Mode;
			qso["submode"] = QsoData.Submode;
			qso["rst_sent"] = QsoData.RST;
			QsoData.Frequency = QsoData.Frequency.Replace(",", ".");
			if(QsoData.Band != string.Empty)
			{
				qso["band"] = QsoData.Band;
			}
			else
			{
				string band = null;
				App.AdifEnums.GetBandFromFrequency(QsoData.Frequency, out band);
					qso["band"] = band;
			}
			if(QsoData.Frequency != string.Empty)
			{
				qso["freq"] = QsoData.Frequency;
			}
			if(QsoData.Rcvd != string.Empty)
			{
				qso["qsl_rcvd"] = QsoData.Rcvd.Substring(0,1);
			}
			if(QsoData.Sent != string.Empty)
			{
				qso["qsl_sent"] = QsoData.Sent.Substring(0,1);
			}
			if(QsoData.SentVia != string.Empty)
			{
				qso["qsl_sent_via"] = QsoData.SentVia.Substring(0,1);
			}
			return qso;
		}
		
		/// <summary>
		/// Handler for RstBox PreviewTextInput event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void RstBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			if(RstBox.Text.Length >= RstBox.MaxLength)
			{
				// too long
				Console.Beep(BEEPFREQUENCY, BEEPDURATION);
				e.Handled = true;
			}
		}
		
		/// <summary>
		/// Load modes defined in AdifEnumerations into ModeComboBox
		/// </summary>
		private void LoadModes()
		{
			AdifEnumerations aEnums = App.AdifEnums;
			string[] modes = aEnums.GetEnumeratedValues("Mode");
			foreach(string mode in modes)
			{
				if(!aEnums.IsDeprecated("Mode", mode) &&
				   !aEnums.IsDeleted("Mode", mode))
				{
					ModeComboBox.Items.Add(mode);
				}
			}
		}
		
		/// <summary>
		/// Handler for ModeComboBox SelectionChanged event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">SelectionChangedEvent object</param>
		void ModeComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
		{
			SubmodeComboBox.Items.Clear();
			if(e.AddedItems.Count > 0)
			{
				string mode = e.AddedItems[0] as string;
				if(!mode.Equals(string.Empty))
				{
					AdifEnumerations aEnums = App.AdifEnums;
					string[] submodes = aEnums.GetSubmodesFromMode(mode);
					foreach(string submode in submodes)
					{
						SubmodeComboBox.Items.Add(submode);
					}
				}
			}
		}
	}
}