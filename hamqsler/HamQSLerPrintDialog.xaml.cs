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
using System.Drawing.Printing;
using System.Windows;
using System.Windows.Input;

namespace hamqsler
{
	/// <summary>
	/// Interaction logic for HamQSLerPrintDialog.xaml
	/// </summary>
	public partial class HamQSLerPrintDialog : Window
	{
		public static RoutedCommand PrintPreviewCommand = new RoutedCommand();
		public static RoutedCommand PrintCommand = new RoutedCommand();
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
		
		public bool PrintCardsVertical
		{
			get {return printPropsPanel.PrintCardsVertical;}
			set {printPropsPanel.PrintCardsVertical = value;}
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
			PrintCardsVertical = ((App)App.Current).UserPreferences.PrintCardsVertical;
		}
		
		/// <summary>
		/// Handler for PreviewButton CanExecute event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">CanExecuteRoutedEventArgs object for this event</param>
		private void PrintPreviewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = printPropsPanel != null && printPropsPanel.LayoutsVisible;
		}
		
		/// <summary>
		/// Handler for PreviewButton Executed event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void PrintPreviewCommand_Executed(object sender, RoutedEventArgs e)
		{
			preview = true;
			this.DialogResult = true;
		}
		
		/// <summary>
		/// Handler for PrintButton CanExecute event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">CanExecuteRoutedEventArgs object for this event</param>
		private void PrintCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = printPropsPanel != null && printPropsPanel.LayoutsVisible;
		}
		
		/// <summary>
		/// Handler for PrintButton Executed event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void PrintCommand_Executed(object sender, RoutedEventArgs e)
		{
			preview = false;
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
	}
}