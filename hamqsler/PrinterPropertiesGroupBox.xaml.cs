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
	/// PrinterPropertiesGroupBox class - displays and interacts with QslCard printer properties
	/// </summary>
	public partial class PrinterPropertiesGroupBox : GroupBox
	{
		private CardWF qslCard = null;
		public CardWF QslCard
		{
			get {return qslCard;}
			set
			{
				qslCard = value;
				SetDefaultPrinter();
			}
		}
		
		// delegate and event handler for paper size property changed
		public delegate void PaperSizePropertyChangedEventHandler(
			object sender, EventArgs e);
		public event PaperSizePropertyChangedEventHandler PaperSizePropertyChanged;
		
		/// <summary>
		/// Constructor
		/// </summary>
		public PrinterPropertiesGroupBox()
		{
			InitializeComponent();
			// initialize printerComboBox with the list of installed printers
			foreach(string printer in PrinterSettings.InstalledPrinters)
			{
				printerComboBox.Items.Add(printer);
			}
		}
		
		/// <summary>
		/// Helper method that sets the selected item in printerComboBox to the
		/// card default printer name, or the computer default printer if
		/// card default is not installed on this computer.
		/// </summary>
		private void SetDefaultPrinter()
		{
			printerComboBox.SelectedItem = QslCard.CardPrintProperties.PrinterName;
			if(printerComboBox.SelectedIndex == -1)
			{
				PrinterSettings settings = new PrinterSettings();
				printerComboBox.SelectedItem = settings.PrinterName;
			}
		}

		/// <summary>
		/// Handler for printerComboBox SelectionChanged event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void PrinterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if(QslCard != null) // if no QslCard, then can't save change or update
								// paper sizes and printer resolutions.
			{
				// save the selected printer name
				QslCard.CardPrintProperties.PrinterName = printerComboBox.SelectedItem.ToString();
				PrinterSettings settings = new PrinterSettings();
				settings.PrinterName = QslCard.CardPrintProperties.PrinterName;
				// get list of paper sizes for this printer and initialize
				// paperSizeComboBox list
				paperSizeComboBox.Items.Clear();
				foreach(PaperSize size in settings.PaperSizes)
				{
					paperSizeComboBox.Items.Add(size.PaperName);
				}
				// set selected paper size to card default, or printer default if card
				// default size is not available on this printer
				paperSizeComboBox.SelectedItem = QslCard.CardPrintProperties.PrinterPaperSize;
				if(paperSizeComboBox.SelectedIndex == -1)
				{
					paperSizeComboBox.SelectedItem =
						settings.DefaultPageSettings.PaperSize.PaperName;
				}
				// get list of printer resolutions for this printer and initialize
				// qualityComboBox list
				qualityComboBox.Items.Clear();
				foreach(PrinterResolution res in settings.PrinterResolutions)
				{
					qualityComboBox.Items.Add(ResolutionString(res));
				}
				// set selected resolution to card default, or printer default if card
				// default resolution is not available on this printer
				qualityComboBox.SelectedItem = ResolutionString(
					QslCard.CardPrintProperties.Resolution);
				if(qualityComboBox.SelectedIndex == -1)
				{
					qualityComboBox.SelectedItem = ResolutionString(
						settings.DefaultPageSettings.PrinterResolution);
				}
			}
		}

		/// <summary>
		/// Helper method that creates string representation of the PrinterResolution
		/// </summary>
		/// <param name="res">PrinterResolution object to create string representation of</param>
		/// <returns>String containing resolution</returns>
		private string ResolutionString(PrinterResolution res)
		{
			string resolution = res.Kind.ToString();
			if (resolution.Equals("Custom")) 
			{
				resolution = string.Format("{0} ({1} x {2})", res.Kind, res.X, res.Y);
			}
			return resolution;
		}
		
		/// <summary>
		/// Handler for paperSizeComboBox SelectionChanged event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">SelectionChangedEventArgs object</param>
		private void PaperSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if(e.AddedItems.Count != 0)
			{
				// find matching PaperSize object for the selected item
				PrinterSettings settings = new PrinterSettings();
				settings.PrinterName = QslCard.CardPrintProperties.PrinterName;
				foreach(PaperSize size in settings.PaperSizes)
				{
					if(size.PaperName == paperSizeComboBox.SelectedItem.ToString())
					{
						// found, so set PaperSize property
						QslCard.CardPrintProperties.PrinterPaperSize = size;
						break;
					}
				}
				// raise PaperSizePropertyChanged event
				if(PaperSizePropertyChanged != null)
				{
					PaperSizePropertyChanged(this, new EventArgs());
				}
			}
		}
		
		/// <summary>
		/// Handler for qualityComboBox SelectionChanged event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void QualityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if(e.AddedItems.Count != 0)
			{
				// find matching PrinterResolution object for the selected item
				PrinterSettings settings = new PrinterSettings();
				settings.PrinterName = QslCard.CardPrintProperties.PrinterName;
				foreach(PrinterResolution res in settings.PrinterResolutions)
				{
					if(ResolutionString(res).Equals(qualityComboBox.SelectedItem.ToString()))
				    {
				   		 // found match, so set Resolution property
				   		 QslCard.CardPrintProperties.Resolution = res;
				   		 break;
				    }
				}
			}
		}
	}
}