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
	/// PrinterMarginsDialog displays printer margins for the selected printer
	/// </summary>
	public partial class PrinterMarginsDialog : Window
	{
		private static readonly DependencyProperty LeftMarginInchesProperty =
			DependencyProperty.Register("LeftMarginInches", typeof(float),
			                            typeof(PrinterMarginsDialog),
			                            new PropertyMetadata(0F));
		public float LeftMarginInches
		{
			get {return (float)GetValue(LeftMarginInchesProperty);}
			set {SetValue(LeftMarginInchesProperty, value);}
		}
		
		private static readonly DependencyProperty LeftMarginMMProperty =
			DependencyProperty.Register("LeftMarginMM", typeof(float),
			                            typeof(PrinterMarginsDialog),
			                            new PropertyMetadata(0F));
		public float LeftMarginMM
		{
			get {return (float)GetValue(LeftMarginMMProperty);}
			set {SetValue(LeftMarginMMProperty, value);}
		}
		
		private static readonly DependencyProperty TopMarginInchesProperty =
			DependencyProperty.Register("TopMarginInches", typeof(float),
			                            typeof(PrinterMarginsDialog),
			                            new PropertyMetadata(0F));
		public float TopMarginInches
		{
			get {return (float)GetValue(TopMarginInchesProperty);}
			set {SetValue(TopMarginInchesProperty, value);}
		}
		
		private static readonly DependencyProperty TopMarginMMProperty =
			DependencyProperty.Register("TopMarginMM", typeof(float),
			                            typeof(PrinterMarginsDialog),
			                            new PropertyMetadata(0F));
		public float TopMarginMM
		{
			get {return (float)GetValue(TopMarginMMProperty);}
			set {SetValue(TopMarginMMProperty, value);}
		}
		
		private static readonly DependencyProperty RightMarginInchesProperty =
			DependencyProperty.Register("RightMarginInches", typeof(float),
			                            typeof(PrinterMarginsDialog),
			                            new PropertyMetadata(0F));
		public float RightMarginInches
		{
			get {return (float)GetValue(RightMarginInchesProperty);}
			set {SetValue(RightMarginInchesProperty, value);}
		}
		
		private static readonly DependencyProperty RightMarginMMProperty =
			DependencyProperty.Register("RightMarginMM", typeof(float),
			                            typeof(PrinterMarginsDialog),
			                            new PropertyMetadata(0F));
		public float RightMarginMM
		{
			get {return (float)GetValue(RightMarginMMProperty);}
			set {SetValue(RightMarginMMProperty, value);}
		}
		
		private static readonly DependencyProperty BottomMarginInchesProperty =
			DependencyProperty.Register("BottomMarginInches", typeof(float),
			                            typeof(PrinterMarginsDialog),
			                            new PropertyMetadata(0F));
		public float BottomMarginInches
		{
			get {return (float)GetValue(BottomMarginInchesProperty);}
			set {SetValue(BottomMarginInchesProperty, value);}
		}
		
		private static readonly DependencyProperty BottomMarginMMProperty =
			DependencyProperty.Register("BottomMarginMM", typeof(float),
			                            typeof(PrinterMarginsDialog),
			                            new PropertyMetadata(0F));
		public float BottomMarginMM
		{
			get {return (float)GetValue(BottomMarginMMProperty);}
			set {SetValue(BottomMarginMMProperty, value);}
		}
		
		private static readonly DependencyProperty LeftMarginBrushProperty =
			DependencyProperty.Register("LeftMarginBrush", typeof(SolidColorBrush),
			                            typeof(PrinterMarginsDialog),
			                            new PropertyMetadata(Brushes.DarkGreen));
		
		public Brush LeftMarginBrush
		{
			get {return GetValue(LeftMarginBrushProperty) as SolidColorBrush;}
			set {SetValue(LeftMarginBrushProperty, value);}
		}
		
		private static readonly DependencyProperty TopMarginBrushProperty =
			DependencyProperty.Register("TopMarginBrush", typeof(SolidColorBrush),
			                            typeof(PrinterMarginsDialog),
			                            new PropertyMetadata(Brushes.DarkGreen));
		public Brush TopMarginBrush
		{
			get {return GetValue(TopMarginBrushProperty) as SolidColorBrush;}
			set {SetValue(TopMarginBrushProperty, value);}
		}
		
		private static readonly DependencyProperty RightMarginBrushProperty =
			DependencyProperty.Register("RightMarginBrush", typeof(SolidColorBrush),
			                            typeof(PrinterMarginsDialog),
			                            new PropertyMetadata(Brushes.DarkGreen));
		public Brush RightMarginBrush
		{
			get {return GetValue(RightMarginBrushProperty) as SolidColorBrush;}
			set {SetValue(RightMarginBrushProperty, value);}
		}
		
		private static readonly DependencyProperty BottomMarginBrushProperty =
			DependencyProperty.Register("BottomMarginBrush", typeof(SolidColorBrush),
			                            typeof(PrinterMarginsDialog),
			                            new PropertyMetadata(Brushes.DarkGreen));
		public Brush BottomMarginBrush
		{
			get {return GetValue(BottomMarginBrushProperty) as SolidColorBrush;}
			set {SetValue(BottomMarginBrushProperty, value);}
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		public PrinterMarginsDialog()
		{
			InitializeComponent();
			foreach(string printer in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
			{
				printersComboBox.Items.Add(printer);
			}
			string defaultPrinter = ((App)App.Current).UserPreferences.DefaultPrinterName;
			printersComboBox.SelectedItem = defaultPrinter;
		}
		
		/// <summary>
		/// Handler for the printersComboBox selection changed event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void PrintersComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			// get printer PrintableArea and PaperSize
			string printer = printersComboBox.SelectedItem as string;
			if(printer != null && !printer.Equals(string.Empty))
			{
				System.Drawing.Printing.PrinterSettings settings
					= new System.Drawing.Printing.PrinterSettings();
				settings.PrinterName = printer;
				System.Drawing.RectangleF area = settings.DefaultPageSettings.PrintableArea;
				float paperHeight = settings.DefaultPageSettings.PaperSize.Height;
				float paperWidth = settings.DefaultPageSettings.PaperSize.Width;
				// convert margins to inches and mm
				LeftMarginInches = area.Left / 100F;
				LeftMarginMM = LeftMarginInches * 25.4F;
				LeftMarginBrush = LeftMarginInches > 0.25F ? Brushes.Red :
					Brushes.DarkGreen;
				TopMarginInches = area.Top / 100F;
				TopMarginMM = TopMarginInches * 25.4F;
				TopMarginBrush = TopMarginInches > 0.25F ? Brushes.Red :
					Brushes.DarkGreen;
				RightMarginInches = (paperWidth - area.Right) / 100F;
				RightMarginMM = RightMarginInches * 25.4F;
				RightMarginBrush = RightMarginInches > 0.25F ? Brushes.Red :
					Brushes.DarkGreen;
				BottomMarginInches = (paperHeight - area.Bottom) / 100F;
				BottomMarginMM = BottomMarginInches * 25.4F;
				BottomMarginBrush = BottomMarginInches > 0.25F ? Brushes.Red :
					Brushes.DarkGreen;
				// if any margin is greater than ¼ inch, warn user
				marginWarningText.Visibility = LeftMarginInches > 0.25 ||
					TopMarginInches > 0.25 || RightMarginInches > 0.25 ||
					BottomMarginInches > 0.25 ?
					Visibility.Visible : Visibility.Collapsed;
			}
		}
		
		// handler for the OK button click event
		void OKButton_Click(object sender, RoutedEventArgs e)
		{
			// just close the dialog
			DialogResult = true;
			this.Close();
		}
	}
}