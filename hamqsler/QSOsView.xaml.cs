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
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace hamqsler
{
	/// <summary>
	/// Interaction logic for QSOsView.xaml
	/// </summary>
	public partial class QSOsView : UserControl
	{
		public enum OrderOfSort 
		{
			DATETIME=1, 
			CALL, 
			BUREAU
		
		}
		private OrderOfSort sortOrder = OrderOfSort.DATETIME;
		public OrderOfSort SortOrder
		{
			get {return sortOrder;}
		}
		
		private StartEndDateTime startEndDateTime = new StartEndDateTime();
		public StartEndDateTime StartEndDateTime
		{
			get {return startEndDateTime;}
		}
		
		// RoutedCommands
		public static RoutedCommand DateTimeRadioButtonClickCommand = new RoutedCommand();
		public static RoutedCommand CallRadioButtonClickCommand = new RoutedCommand();
		public static RoutedCommand BureauRadioButtonClickCommand = new RoutedCommand();
		public static RoutedCommand ResortButtonClickCommand = new RoutedCommand();
		
		private static DependencyProperty DisplayQsosProperty = DependencyProperty.Register(
			"DisplayQsos", typeof(DisplayQsos), typeof(MainWindow), 
			new PropertyMetadata(new DisplayQsos()));
		public DisplayQsos DisplayQsos
		{
			get {return (DisplayQsos)GetValue(DisplayQsosProperty);}
			set {SetValue(DisplayQsosProperty, value);}
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		public QSOsView()
		{
			InitializeComponent();
		}
		
		/// <summary>
		/// CanExecute for DateTimeRadioButton
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">CanExecuteRoutedEventArgs object</param>
		private void DateTimeRadioButtonClick_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = DisplayQsos.Count > 0;
		}
		
		/// <summary>
		/// CanExecute for CallRadioButton
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">CanExecuteRoutedEventArgs object</param>
		private void CallRadioButtonClick_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = DisplayQsos.Count > 0;
		}
		
		/// <summary>
		/// CanExecute for BureauRadioButton
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">CanExecuteRoutedEventArgs object</param>
		private void BureauRadioButtonClick_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = DisplayQsos.Count > 0;
		}
		
		/// <summary>
		/// CanExecute for ResortButton
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">CanExecuteRoutedEventArgs object</param>
		private void ResortButtonClickCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = !(SortOrder == OrderOfSort.DATETIME) && DisplayQsos.NeedsSorting;
		}
		
		/// <summary>
		/// Handles DateTimeRadioButton click events
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void DateTimeRadioButtonClick_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			sortOrder = OrderOfSort.DATETIME;
			DateTimeComparer dt = new DateTimeComparer();
			DisplayQsos.SortQSOs(dt);
		}
		
		/// <summary>
		/// Handles CallRadioButton click events
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void CallRadioButtonClick_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			sortOrder = OrderOfSort.CALL;
			CallComparer cc = new CallComparer();
			DisplayQsos.SortQSOs(cc);
		}
		
		/// <summary>
		/// Handles BureauRadioButton click events
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void BureauRadioButtonClick_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			sortOrder = OrderOfSort.BUREAU;
			BureauComparer bc = new BureauComparer();
			DisplayQsos.SortQSOs(bc);
		}
		
		/// <summary>
		/// Method for creating a Comparer based on QSO sort order
		/// </summary>
		/// <returns>comparer object for sorting</returns>
		public Comparer<QsoWithInclude> GetComparer()
		{
			Comparer<QsoWithInclude> comparer = null;
			if(SortOrder == OrderOfSort.DATETIME)
				comparer = new DateTimeComparer();
			else if(SortOrder == OrderOfSort.CALL)
				comparer = new CallComparer();
			else if(SortOrder == OrderOfSort.BUREAU)
				comparer = new BureauComparer();
			return comparer;
		}
		
		/// <summary>
		/// Create and show the various include selectors (bands, modes, dates/times, and qsl statuses)
		/// </summary>
		public void ShowIncludeSelectors()
		{
			SetBands();					// create and show bands checkboxes
			SetModes();					// create and show modes checkboxes
			SetRcvdStatuses();			// create and show rcvd statuses checkboxes
			SetSentStatuses();			// create and show sent statuses checkboxes
			SetSentViaStatuses();		// create and show sent via statuses checkboxes
			SetDatesTimes();			// set and show start and end dates/times
		}
		
		/// <summary>
		/// Creates and shows checkboxes for each band in the QSOs
		/// </summary>
		private void SetBands()
		{
			// remove old checkboxes
			BandGrid.Children.RemoveRange(0, BandGrid.Children.Count);
			// add in new checkboxes
			int numCheckBoxes = 0;
			foreach(string band in DisplayQsos.GetBands())
			{
				CheckBox bcb = new CheckBox();
				bcb.Content = band.ToLower();
				bcb.IsChecked = true; //AllBands.IsChecked;
				bcb.Margin=new Thickness(20, 5, 20, 5);
				bcb.Checked += OnBandCheckBoxChecked;
				bcb.Unchecked += OnBandCheckBoxChecked;
				BandGrid.Children.Add(bcb);
				int column = 0;
				if(numCheckBoxes%2 != 0)
					column = 2;
				Grid.SetColumn(bcb, column);
				Grid.SetRow(bcb, numCheckBoxes/2);
				numCheckBoxes++;
			}
		}
		
		/// <summary>
		/// Creates and shows checkboxes for each mode in the QSOs
		/// </summary>
		private void SetModes()
		{
			// remove old checkboxes
			ModeGrid.Children.RemoveRange(0, ModeGrid.Children.Count);
			// add in new checkboxes
			int row = 0;
			foreach(string mode in DisplayQsos.GetModes())
			{
				CheckBox bcb = new CheckBox();
				bcb.Content = mode;
				bcb.IsChecked = true; //AllModes.IsChecked;
				bcb.Margin=new Thickness(20, 5, 20, 5);
				bcb.Checked += OnModeCheckBoxChecked;
				bcb.Unchecked += OnModeCheckBoxChecked;
				ModeGrid.Children.Add(bcb);
				Grid.SetRow(bcb, row++);
			}
		}
		
		/// <summary>
		/// Gets and sets the start and end dates/times based on contents of DisplayQsos
		/// </summary>
		private void SetDatesTimes()
		{
			string startDate;
			string startTime;
			string endDate;
			string endTime;
			DisplayQsos.GetStartEndDatesTimes(out startDate, out startTime,
			                                 out endDate, out endTime);
			startEndDateTime.StartDate = startDate;
			startEndDateTime.StartTime = startTime;
			// must set EndTime before EndDate or will get message about end date/time
			// before start date/time
			startEndDateTime.EndTime = endTime;
			startEndDateTime.EndDate = endDate;
		}
		
		/// <summary>
		/// Creates and shows checkboxes for each QSL sent status
		/// </summary>
		private void SetSentStatuses()
		{
			// remove old checkboxes
			SentPanel.Children.RemoveRange(0, SentPanel.Children.Count);
			// create button for each status
			foreach(string status in DisplayQsos.GetSentStatuses())
			{
				CheckBox bcb = new CheckBox();
				bcb.Content = status;
				bcb.IsChecked = true;
				bcb.Margin=new Thickness(20, 5, 20, 5);
				bcb.Checked += OnSentCheckBoxChecked;
				bcb.Unchecked += OnSentCheckBoxChecked;
				SentPanel.Children.Add(bcb);
			}
		}
		
		/// <summary>
		/// Creates and shows checkboxes for each QSL sent via status
		/// </summary>
		private void SetSentViaStatuses()
		{
			// remove old checkboxes
			SendViaPanel.Children.RemoveRange(0, SendViaPanel.Children.Count);
			// create button for each status
			foreach(string status in DisplayQsos.GetSentViaStatuses())
			{
				CheckBox bcb = new CheckBox();
				bcb.Content = status;
				bcb.IsChecked = true;
				bcb.Margin=new Thickness(20, 5, 20, 5);
				bcb.Checked += OnSentViaCheckBoxChecked;
				bcb.Unchecked += OnSentViaCheckBoxChecked;
				SendViaPanel.Children.Add(bcb);
			}
		}
		
		/// <summary>
		/// Creates and shows checkboxes for each QSL rcvd status
		/// </summary>
		private void SetRcvdStatuses()
		{
			// remove old checkboxes
			RcvdPanel.Children.RemoveRange(0, RcvdPanel.Children.Count);
			// create button for each status
			foreach(string status in DisplayQsos.GetRcvdStatuses())
			{
				CheckBox bcb = new CheckBox();
				bcb.Content = status;
				bcb.IsChecked = true;
				bcb.Margin=new Thickness(20, 5, 20, 5);
				bcb.Checked += OnRcvdCheckBoxChecked;
				bcb.Unchecked += OnRcvdCheckBoxChecked;
				RcvdPanel.Children.Add(bcb);
			}
		}
		
		/// <summary>
		/// Handler for Checked and Unchecked bands checkboxes (not including AllBands)
		/// </summary>
		/// <param name="sender">checkbox being checked or unchecked</param>
		/// <param name="e">not used</param>
		private void OnBandCheckBoxChecked(object sender, RoutedEventArgs e)
		{
			SetIncludes();
		}
		
		/// <summary>
		/// Handler for Checked and Unchecked modes checkboxes (not including AllModes)
		/// </summary>
		/// <param name="sender">checkbox being checked or unchecked</param>
		/// <param name="e">not used</param>
		private void OnModeCheckBoxChecked(object sender, RoutedEventArgs e)
		{
			SetIncludes();
		}
		
		/// <summary>
		/// Handler for Checked and Unchecked events on the QSL Sent checkboxes
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void OnSentCheckBoxChecked(object sender, RoutedEventArgs e)
		{
			SetIncludes();
		}
		
		/// <summary>
		/// Handler for Checked and Unchecked events on the QSL sent via checkboxes
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnSentViaCheckBoxChecked(object sender, RoutedEventArgs e)
		{
			SetIncludes();
		}
		
		/// <summary>
		/// Handler for Checked and Unchecked events on the QSL rcvd checkboxes
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void OnRcvdCheckBoxChecked(object sender, RoutedEventArgs e)
		{
			SetIncludes();
		}
		
		/// <summary>
		/// Sets Include for each QSO based on bands, modes, qsl statuses and date/time settings
		/// </summary>
		private void SetIncludes()
		{
			Dictionary<string, bool>bandDict = CreateBandDictionary();
			Dictionary<string, bool>modeDict = CreateModeDictionary();
			Dictionary<string, bool>rcvdDict = CreateRcvdDictionary();
			Dictionary<string, bool>sentDict = CreateSentDictionary();
			Dictionary<string, bool>sentViaDict = CreateSentViaDictionary();
			DisplayQsos.SetIncludes(ref bandDict, ref modeDict,
			                        ref startEndDateTime,
			                        ref rcvdDict,
			                        ref sentDict,
			                        ref sentViaDict);
		}
		
		/// <summary>
		/// Helper method that creates a Dictionary containing the bands as keys and IsChecked
		/// value of corresponding checkbox as value
		/// </summary>
		/// <returns>the dictionary</returns>
		private Dictionary<string, bool> CreateBandDictionary()
		{
			Dictionary<string, bool> bands = new Dictionary<string, bool>();
			foreach(UIElement element in BandGrid.Children)
			{
				CheckBox cb = element as CheckBox;
				if(cb != null && cb.Name != "AllBands")
				{
					bands.Add(cb.Content.ToString(), (bool)cb.IsChecked);
				}
			}
			return bands;
		}

		/// <summary>
		/// Helper method that creates a Dictionary containing the mdoes as keys and IsChecked
		/// value of the corresponding checkbox as value
		/// </summary>
		/// <returns></returns>
		private Dictionary<string, bool> CreateModeDictionary()
		{
			Dictionary<string, bool> modes = new Dictionary<string, bool>();
			foreach(UIElement element in ModeGrid.Children)
			{
				CheckBox cb = element as CheckBox;
				if(cb != null && cb.Name != "AllModes")
				{
					modes.Add(cb.Content.ToString(), (bool)cb.IsChecked);
				}
			}
			return modes;
		}
		
		/// <summary>
		/// Helper method that creates a Dictionary containing the sent statuses and IsChecked
		/// value of the corresponding checkbox as value;
		/// </summary>
		/// <returns></returns>
		private Dictionary<string, bool> CreateSentDictionary()
		{
			Dictionary<string, bool> sents = new Dictionary<string, bool>();
			foreach(UIElement element in SentPanel.Children)
			{
				CheckBox cb = element as CheckBox;
				if(cb != null)
				{
					sents.Add(cb.Content.ToString(), (bool)cb.IsChecked);
				}
			}
			return sents;
		}

		/// <summary>
		/// Helper method that creates a Dictionary containing the sent via statuses and IsChecked
		/// value of the corresponding checkbox as value;
		/// </summary>
		/// <returns></returns>
		private Dictionary<string, bool> CreateSentViaDictionary()
		{
			Dictionary<string, bool> sents = new Dictionary<string, bool>();
			foreach(UIElement element in SendViaPanel.Children)
			{
				CheckBox cb = element as CheckBox;
				if(cb != null)
				{
					sents.Add(cb.Content.ToString(), (bool)cb.IsChecked);
				}
			}
			return sents;
		}

		/// <summary>
		/// Helper method that creates a Dictionary containing the rcvd statuses and IsChecked
		/// value of the corresponding checkbox as value;
		/// </summary>
		/// <returns></returns>
		private Dictionary<string, bool> CreateRcvdDictionary()
		{
			Dictionary<string, bool> rcvds = new Dictionary<string, bool>();
			foreach(UIElement element in RcvdPanel.Children)
			{
				CheckBox cb = element as CheckBox;
				if(cb != null)
				{
					rcvds.Add(cb.Content.ToString(), (bool)cb.IsChecked);
				}
			}
			return rcvds;
		}
		
		/// <summary>
		/// Handler for TextChanged events on the date and time textboxes
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void DatesTimes_TextChanged(object sender, TextChangedEventArgs e)
		{
			string startDateTime = startEndDateTime.ValidStartDate + 
				startEndDateTime.ValidStartTime;
			string endDateTime = startEndDateTime.ValidEndDate + 
				startEndDateTime.ValidEndTime;
			// if start later than end, show error message to indicate that no QSOs will be included
			if(String.Compare(startDateTime, endDateTime, true) > 0 &&
			   !startEndDateTime.EndDate.Equals(string.Empty) &&
 			   !startEndDateTime.EndTime.Equals(string.Empty) &&
			   !startEndDateTime.StartTime.Equals(string.Empty) &&
			   !startEndDateTime.StartDate.Equals(string.Empty))
			{
				MessageBox.Show("Start date and time later than end date and time.\n\r" +
				                "No QSOs are included.", "Date/Time Error",
				                MessageBoxButton.OK, MessageBoxImage.Warning);
			}
			// now go set Include for each QSO
			SetIncludes();
		}
		
		
		/// <summary>
		/// Handler for Reset button click event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void ResetButton_Click(object sender, RoutedEventArgs e)
		{
			// reset start date and time to that of earliest QSO, and end date and time
			// to that of latest QSO
			SetDatesTimes();
		}
		
		/// <summary>
		/// Handler for TextChanged event on Manager boxes
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void ManagerBoxes_TextChanged(object sender, TextChangedEventArgs e)
		{
			// Text property of the TextBox is bound to the appropriate QSO
			// so that the property is changed. All we need to do is
			// set IsDirty to true to show that a change to at least one of
			// the QSOs took place
			DisplayQsos.IsDirty = true;
			if(SortOrder != OrderOfSort.DATETIME)
			{
				DisplayQsos.NeedsSorting = true;
			}
		}
		
		/// <summary>
		/// Handler for ResortButton click event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void ResortButtonClickCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			Comparer<QsoWithInclude> comparer = GetComparer();
			DisplayQsos.SortQSOs(comparer);
		}
		/// <summary>
		/// Handler for click events on the Include checkboxes in qsosListView
		/// </summary>
		/// <param name="sender">Include checkbox that was clicked</param>
		/// <param name="e">not used</param>
		void IncludeClicked(object sender, RoutedEventArgs e)
		{
			CheckBox cb = sender as CheckBox;
			// we must allow for selection of multiple rows in the listview
			System.Collections.IList list = qsosListView.SelectedItems;
			DisplayQsos.SetIncludesChecked(list, (bool)cb.IsChecked);
		}
		
		/// <summary>
		/// Select all loaded QSOs
		/// </summary>
		public void SelectAllQsos()
		{
			qsosListView.SelectedItems.Clear();
			foreach(object o in qsosListView.Items)
			{
				qsosListView.SelectedItems.Add(o);
			}
		}
		
		/// <summary>
		/// Deselect all loaded QSOs
		/// </summary>
		public void DeselectAllQsos()
		{
			qsosListView.SelectedItems.Clear();
		}
	}
}