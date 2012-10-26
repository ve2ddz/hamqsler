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
		
		// indicates whether already processing AllBands.Checked or AllBands.Unchecked event
		private bool allBandsUpdating = false;
		
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
			InvalidateVisual();
		}
		
		/// <summary>
		/// Creates ans shows checkboxes for each band in the QSOs
		/// </summary>
		private void SetBands()
		{
			// remove old checkboxes
			for(int i = BandGrid.Children.Count-1; i > 0; i--)
			{
				CheckBox cb = BandGrid.Children[i] as CheckBox;
				if(cb != null && cb.Name != "AllBands")
				{
					BandGrid.Children.Remove(cb);
				}
			}
			// add in new checkboxes
			int numCheckBoxes = 1;
			foreach(string band in DisplayQsos.GetBands())
			{
				CheckBox bcb = new CheckBox();
				bcb.Content = band.ToLower();
				bcb.IsChecked = AllBands.IsChecked;
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
		/// Handler for Checked and Unchecked bands checkboxes (not including AllBands)
		/// </summary>
		/// <param name="sender">checkbox being checked or unchecked</param>
		/// <param name="e">not used</param>
		private void OnBandCheckBoxChecked(object sender, RoutedEventArgs e)
		{
			CheckBox cb = sender as CheckBox;
			if(!((bool)(cb.IsChecked)))
			{
				AllBands.IsChecked = false;
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
				AllBands.IsChecked = allChecked;
			}
			DisplayQsos.SetIncludesByBand(cb.Content.ToString().ToLower(), (bool)cb.IsChecked);
		}
		
		/// <summary>
		/// AllBands Checked and UnChecked event handler
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void AllBands_Checked(object sender, RoutedEventArgs e)
		{
			// This code works in conjunction with the OnBandCheckBoxChecked event handler.
			// Every bands checkbox is set according to the AllBands.IsChecked property,
			// which causes the OnBandCheckBoxChecked being called for each checkbox.
			// If more than one checkbox was unchecked when the AllBands checkbox was checked,
			// OnBandCheckBoxChecked will reset AllBands.IsChecked to false, raising an
			// Unchecked event and firing this handler again. The result would be that the AllBands
			// checkbox and the first band checkbox would be unchecked.
			// allBandsUpdating is a flag used to prevent the processing of multiple Checked/Unchecked
			// events concurrently. By doing so, the Unchecked event caused by OnBandCheckBoxChecked
			// processing is ignored, and the stated problem does not occur.
			// Note that AllBands.IsChecked is set to false as long as there is still one checkbox
			// that is not checked, but once the last checkbox is checked, AllBands.IsChecked will
			// be set correctly.
			if(!allBandsUpdating)
			{
				allBandsUpdating = true;		// show that event is being handled
				bool check = (bool)AllBands.IsChecked;
				foreach(UIElement element in BandGrid.Children)
				{
					// check or uncheck every bands checkbox
					CheckBox cbInGrid = element as CheckBox;
					if(cbInGrid != null && cbInGrid.Name != "AllBands")
					{
						cbInGrid.IsChecked = check;
					}
				}
				// set the Include property of every QSO appropriately
				if(check)
				{
					DisplayQsos.IncludeAllQsos();
				}
				else
				{
					DisplayQsos.ExcludeAllQsos();
				}
				allBandsUpdating = false;		// done, so allow other events
			}
		}
	}
}