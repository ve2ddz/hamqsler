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
		

		public QSOsView()
		{
			InitializeComponent();
		}
		
		private void DateTimeRadioButtonClick_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = DisplayQsos.Count > 0;
		}
		
		private void CallRadioButtonClick_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = DisplayQsos.Count > 0;
		}
		
		private void BureauRadioButtonClick_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = DisplayQsos.Count > 0;
		}
		
		private void DateTimeRadioButtonClick_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			sortOrder = OrderOfSort.DATETIME;
			DateTimeComparer dt = new DateTimeComparer();
			DisplayQsos.SortQSOs(dt);
		}
		
		private void CallRadioButtonClick_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			sortOrder = OrderOfSort.CALL;
			CallComparer cc = new CallComparer();
			DisplayQsos.SortQSOs(cc);
		}
		
		private void BureauRadioButtonClick_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			sortOrder = OrderOfSort.BUREAU;
			BureauComparer bc = new BureauComparer();
			DisplayQsos.SortQSOs(bc);
		}
		
	}
}