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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

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
		
		// RoutedCommands
		public static RoutedCommand DateTimeRadioButtonClickCommand = new RoutedCommand();
		public static RoutedCommand CallRadioButtonClickCommand = new RoutedCommand();
		public static RoutedCommand BureauRadioButtonClickCommand = new RoutedCommand();
		
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
		/// <param name="e">not used</param>
		private void DateTimeRadioButtonClick_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = DisplayQsos.Count > 0;
		}
		
		/// <summary>
		/// CanExecute for CallRadioButton
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void CallRadioButtonClick_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = DisplayQsos.Count > 0;
		}
		
		/// <summary>
		/// CanExecute for BureauRadioButton
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BureauRadioButtonClick_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = DisplayQsos.Count > 0;
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
			SetModes();
			InvalidateVisual();
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
			int numCheckBoxes = 0;
			foreach(string mode in DisplayQsos.GetModes())
			{
				CheckBox bcb = new CheckBox();
				bcb.Content = mode;
				bcb.IsChecked = true; //AllModes.IsChecked;
				bcb.Margin=new Thickness(20, 5, 20, 5);
				bcb.Checked += OnModeCheckBoxChecked;
				bcb.Unchecked += OnModeCheckBoxChecked;
				ModeGrid.Children.Add(bcb);
				int column = 0;
				if(numCheckBoxes%2 != 0)
					column = 2;
				Grid.SetColumn(bcb, column);
				Grid.SetRow(bcb, numCheckBoxes/2);
				numCheckBoxes++;
			}
		}
		
		/// <summary>
		/// Handler for Checked and Unchecked bands checkboxes (not including AllBands)
		/// </summary>
		/// <param name="sender">checkbox being checked or unchecked</param>
		/// <param name="e">not used</param>
		private void OnBandCheckBoxChecked(object sender, RoutedEventArgs e)
		{
			CheckBox cb = sender as CheckBox;
			if(!((bool)(cb.IsChecked)))
			{
//				AllBands.IsChecked = false;
			}
			else
			{
				bool allChecked = true;
				foreach(UIElement element in BandGrid.Children)
				{
					CheckBox cbInGrid = element as CheckBox;
					if(cbInGrid != null && cbInGrid.Name != "AllBands")
					{
						allChecked = allChecked && (bool)cbInGrid.IsChecked;
					}
				}
//				AllBands.IsChecked = allChecked;
			}
			SetIncludes();
		}
		
		/// <summary>
		/// Handler for Checked and Unchecked modes checkboxes (not including AllModes)
		/// </summary>
		/// <param name="sender">checkbox being checked or unchecked</param>
		/// <param name="e">not used</param>
		private void OnModeCheckBoxChecked(object sender, RoutedEventArgs e)
		{
			CheckBox cb = sender as CheckBox;
			if(!((bool)(cb.IsChecked)))
			{
//				AllModes.IsChecked = false;
			}
			else
			{
				bool allChecked = true;
				foreach(UIElement element in ModeGrid.Children)
				{
					CheckBox cbInGrid = element as CheckBox;
					if(cbInGrid != null && cbInGrid.Name != "AllModes")
					{
						allChecked = allChecked && (bool)cbInGrid.IsChecked;
					}
				}
//				AllModes.IsChecked = allChecked;
			}
			SetIncludes();
		}
		
		/// <summary>
		/// Sets Include for each QSO based on bands, modes, qsl statuses and date/time settings
		/// </summary>
		private void SetIncludes()
		{
			Dictionary<string, bool>bandDict = CreateBandDictionary();
			Dictionary<string, bool>modeDict = CreateModeDictionary();
			DisplayQsos.SetIncludes(ref bandDict, ref modeDict);
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

	}
}