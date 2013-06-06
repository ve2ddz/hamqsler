/*
 *  Author:
 *       Jim Orcheson <jimorcheson@gmail.com>
 * 
 *  Copyright © 2013 Jim Orcheson
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
using System.Windows;

namespace hamqsler
{
	/// <summary>
	/// Interaction logic for PrinterImageableAreaErrorDialog.xaml
	/// </summary>
	public partial class PrinterImageableAreaErrorDialog : Window
	{
		private static readonly DependencyProperty DoNotShowForThisPrinterProperty =
			DependencyProperty.Register("DoNotShowForThisPrinter", typeof(bool),
			                            typeof(PrinterImageableAreaErrorDialog),
			                            new PropertyMetadata(false));
		public bool DoNotShowForThisPrinter
		{
			get {return (bool)GetValue(DoNotShowForThisPrinterProperty);}
			set {SetValue(DoNotShowForThisPrinterProperty, value);}
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		public PrinterImageableAreaErrorDialog()
		{
			InitializeComponent();
		}
		
		/// <summary>
		/// Handler for OK button click event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void OkButton_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
			this.Close();
		}
	}
}