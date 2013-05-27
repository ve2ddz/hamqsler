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
using System.Drawing.Printing;
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
	/// Interaction logic for HamQSLerPrintDialog.xaml
	/// </summary>
	public partial class HamQSLerPrintDialog : Window
	{
		private bool preview = true;
		public bool Preview
		{
			get {return preview;}
		}
		
		public string PrinterName
		{
			get {return printPropsPanel.PrinterName;}
			set {printPropsPanel.PrinterName = value;}
		}
		
		public PaperSize PrinterPaperSize
		{
			get {return printPropsPanel.PrinterPaperSize;}
			set {printPropsPanel.PrinterPaperSize = value;}
		}
		
		public PrinterResolution Resolution
		{
			get {return printPropsPanel.Resolution;}
			set {printPropsPanel.Resolution = value;}
		}
		
		public PaperSource Source
		{
			get {return printPropsPanel.Source;}
			set {printPropsPanel.Source = value;}
		}
		
		public bool InsideMargins
		{
			get {return printPropsPanel.InsideMargins;}
			set {printPropsPanel.InsideMargins = value;}
		}
		
		public bool PrintCardOutlines
		{
			get {return printPropsPanel.PrintCardOutlines;}
			set {printPropsPanel.PrintCardOutlines = value;}
		}
		
		public bool FillLastPage
		{
			get {return printPropsPanel.FillLastPage;}
			set {printPropsPanel.FillLastPage = value;}
		}
		
		public bool SetCardMargins
		{
			get {return printPropsPanel.SetCardMargins;}
			set {printPropsPanel.SetCardMargins = value;}
		}
		
		public PrintProperties.CardLayouts Layout
		{
			get {return printPropsPanel.Layout;}
			set {printPropsPanel.Layout = value;}
		}
		
		public int CardWidth
		{
			get {return printPropsPanel.CardWidth;}
			set {printPropsPanel.CardWidth = value;}
		}
		
		public int CardHeight
		{
			get {return printPropsPanel.CardHeight;}
			set {printPropsPanel.CardHeight = value;}
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		public HamQSLerPrintDialog()
		{
			InitializeComponent();
		}
		
		/// <summary>
		/// Handler for PreviewButton Clicked event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void PreviewButton_Click(object sender, RoutedEventArgs e)
		{
			preview = true;
			this.DialogResult = true;
		}
		
		/// <summary>
		/// Handler for CancelButton Clicked event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void CancelButton_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = false;
		}
		
		/// <summary>
		/// Handler for PrintButton Clicked event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void PrintButton_Click(object sender, RoutedEventArgs e)
		{
			preview = false;
			this.DialogResult = true;
		}
	}
}