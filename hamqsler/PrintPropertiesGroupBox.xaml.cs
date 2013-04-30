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
	/// PrintPropertiesGroupBox class - displays and interacts with QslCard print properties
	/// </summary>
	public partial class PrintPropertiesGroupBox : GroupBox
	{
		private CardWF qslCard = null;
		public CardWF QslCard
		{
			get {return qslCard;}
			set
			{
				qslCard = value;
				Initializing = true;
				insideMarginsButton.IsChecked = QslCard.CardPrintProperties.InsideMargins;
				printCardOutlinesButton.IsChecked = QslCard.CardPrintProperties.PrintCardOutlines;
				fillLastPageButton.IsChecked = QslCard.CardPrintProperties.FillLastPage;
				setCardMarginsButton.IsChecked = QslCard.CardPrintProperties.SetCardMargins;
				Initializing = false;
			}
		}
		
		// delegate and event handler for Properties changed
		public delegate void PrintPropertiesChangedEventHandler(
			object sender, EventArgs e);
		public event PrintPropertiesChangedEventHandler PrintPropertiesChanged;
		
		private bool initializing = true;
		public bool Initializing
		{
			get {return initializing;}
			set {initializing = value;}
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		public PrintPropertiesGroupBox()
		{
			InitializeComponent();
		}
		
		/// <summary>
		/// Handler for insideMarginsButton Checked and Unchecked events
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void InsideMarginsButton_CheckedChanged(object sender, RoutedEventArgs e)
		{
			// set InsideMargins property
			QslCard.CardPrintProperties.InsideMargins = 
				(bool)insideMarginsButton.IsChecked;
			// raise PrintPropertiesChanged event
			if(PrintPropertiesChanged != null && !Initializing)
			{
				PrintPropertiesChanged(this, new EventArgs());
			}
		}
		
		/// <summary>
		/// Handler for printCardOutlinesButton Checked and Unchecked events
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void PrintCardOutlinesButton_CheckedChanged(object sender, RoutedEventArgs e)
		{
			// set PrintCardOutlines property
			QslCard.CardPrintProperties.PrintCardOutlines = 
				(bool)printCardOutlinesButton.IsChecked;
			// raise PrintPropertiesChanged event
			if(PrintPropertiesChanged != null && !Initializing)
			{
				PrintPropertiesChanged(this, new EventArgs());
			}
		}
		
		/// <summary>
		/// Handler for fillLastPageButton Checked and Unchecked events
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void FillLastPageButton_CheckedChanged(object sender, RoutedEventArgs e)
		{
			// set FillLastPage property
			QslCard.CardPrintProperties.FillLastPage = 
				(bool)fillLastPageButton.IsChecked;
			// raise PrintPropertiesChanged event
			if(PrintPropertiesChanged != null && !Initializing)
			{
				PrintPropertiesChanged(this, new EventArgs());
			}
		}
		
		/// <summary>
		/// Handler for setCardMarginsButton Checked and Unchecked events
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void SetCardMarginsButton_CheckedChanged(object sender, RoutedEventArgs e)
		{
			// set SetCardMargins property
			QslCard.CardPrintProperties.SetCardMargins = 
				(bool)setCardMarginsButton.IsChecked;
			// raise PrintPropertiesChanged event
			if(PrintPropertiesChanged != null && !Initializing)
			{
				PrintPropertiesChanged(this, new EventArgs());
			}
		}
		
	}
}