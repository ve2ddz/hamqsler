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
	/// Interaction logic for CardPropertiesGroupBox.xaml
	/// </summary>
	public partial class CardPropertiesGroupBox : UserControl
	{
		private static readonly DependencyProperty QslCardProperty =
			DependencyProperty.Register("QslCard", typeof(CardWF), typeof(CardPropertiesGroupBox),
			                            new PropertyMetadata(null));
		public CardWF QslCard
		{
			get {return GetValue(QslCardProperty) as CardWF;}
			set {SetValue(QslCardProperty, value);}
		}
		
		private bool initializing = true;
		
		/// <summary>
		/// Constructor
		/// </summary>
		public CardPropertiesGroupBox()
		{
			InitializeComponent();
			printPropsPanel.printerPropertiesGroupBox.Header = "Default Printer Properties";
			printPropsPanel.printPropertiesGroupBox.Header = "Default Print Properties";
			printPropsPanel.cardsLayoutGroupBox.Header = "Default Cards Layout";
			printPropsPanel.PrintPropertiesChanged += OnPrintPropertiesChanged;

		}
		
		/// <summary>
		/// Event handler for DependencyProperty Changed event
		/// </summary>
		/// <param name="e">DependencyPropertyChangedEventArgs object</param>
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if(e.Property == QslCardProperty)
			{
				// pass the print properties on to printPropsPanel
				initializing = true;
				printPropsPanel.PrinterName = QslCard.CardPrintProperties.PrinterName;
				printPropsPanel.PrinterPaperSize = QslCard.CardPrintProperties.PrinterPaperSize;
				printPropsPanel.Resolution = QslCard.CardPrintProperties.Resolution;
				printPropsPanel.Source = QslCard.CardPrintProperties.Source;
				printPropsPanel.InsideMargins = QslCard.CardPrintProperties.InsideMargins;
				printPropsPanel.PrintCardOutlines = QslCard.CardPrintProperties.PrintCardOutlines;
				printPropsPanel.FillLastPage = QslCard.CardPrintProperties.FillLastPage;
				printPropsPanel.SetCardMargins = QslCard.CardPrintProperties.SetCardMargins;
				printPropsPanel.Layout = QslCard.CardPrintProperties.Layout;
				printPropsPanel.CardWidth = QslCard.Width;
				printPropsPanel.CardHeight = QslCard.Height;
				initializing = false;
			}
		}
		
		/// <summary>
		/// Handler for printPropsPanel PrintProperties changed event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void OnPrintPropertiesChanged(object sender, EventArgs e)
		{
			if(!initializing)
			{
				QslCard.CardPrintProperties.PrinterName = printPropsPanel.PrinterName;
				QslCard.CardPrintProperties.PrinterPaperSize = printPropsPanel.PrinterPaperSize;
				QslCard.CardPrintProperties.Resolution = printPropsPanel.Resolution;
				QslCard.CardPrintProperties.Source = printPropsPanel.Source;
				QslCard.CardPrintProperties.InsideMargins = printPropsPanel.InsideMargins;
				QslCard.CardPrintProperties.PrintCardOutlines = printPropsPanel.PrintCardOutlines;
				QslCard.CardPrintProperties.FillLastPage = printPropsPanel.FillLastPage;
				QslCard.CardPrintProperties.SetCardMargins = printPropsPanel.SetCardMargins;
				QslCard.CardPrintProperties.Layout = printPropsPanel.Layout;
			}
		}
		
	}
}