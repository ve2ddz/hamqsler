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
using System.Windows;
using System.Windows.Media;

namespace hamqsler
{
	/// <summary>
	/// QSL card
	/// </summary>
	public class Card : CardItem
	{
		private UserPreferences userPreferences;
		
		private static readonly DependencyProperty PrintCardOutlinesProperty =
			DependencyProperty.Register("PrintCardOutlines", typeof(bool), typeof(Card),
			                            new PropertyMetadata(true));
		public bool PrintCardOutlines
		{
			get {return (bool)GetValue(PrintCardOutlinesProperty);}
			set {SetValue(PrintCardOutlinesProperty, value);}
		}
		
		private static readonly DependencyProperty FillLastPageWithBlankCardsProperty =
			DependencyProperty.Register("FillLastPageWithBlankCards", typeof(bool), typeof(Card),
			                            new PropertyMetadata(true));
		public bool FillLastPageWithBlankCards
		{
			get {return (bool)GetValue(FillLastPageWithBlankCardsProperty);}
			set {SetValue(FillLastPageWithBlankCardsProperty, value);}
		}
		
		private static readonly DependencyProperty SetCardMarginsToPrinterPageMarginsProperty =
			DependencyProperty.Register("SetCardMarginsToPrinterPageMargins", typeof(bool),
			                            typeof(Card), new PropertyMetadata(true));
		public bool SetCardMarginsToPrinterPageMargins
		{
			get {return (bool)GetValue(SetCardMarginsToPrinterPageMarginsProperty);}
			set {SetValue(SetCardMarginsToPrinterPageMarginsProperty, value);}
		}
			                     
		
		/// <summary>
		/// default constructor
		/// </summary>
		public Card()
		{
			userPreferences = ((App)Application.Current).UserPreferences;
			DisplayRectangle = new Rect(0, 0, 0, 0);
			QslCard = this;
		}
		
		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="width">Width of the card in device independent units</param>
		/// <param name="height">Height of the card in device independent units</param>
		public Card(double width, double height)
		{
			userPreferences = ((App)Application.Current).UserPreferences;
			PrintCardOutlines = userPreferences.PrintCardOutlines;
			FillLastPageWithBlankCards = userPreferences.PrintCardOutlines;
			SetCardMarginsToPrinterPageMargins = userPreferences.SetCardMarginsToPrinterPageMargins;
			DisplayRectangle = new Rect(0, 0, width, height);
			QslCard = this;
		}
		
		/// <summary>
		/// OnRender event handler - draws the card
		/// </summary>
		/// <param name="dc">Context to draw the card on</param>
		protected override void OnRender(DrawingContext dc)
		{
			base.OnRender(dc);
			Brush brush = Brushes.White;
			Pen pen = new Pen(Brushes.Transparent, 1);
			if(PrintCardOutlines)
			{
				pen = new Pen(Brushes.Black, 1);
			}
			dc.DrawRectangle(brush, pen, DisplayRectangle);
		}
		
	}
	
}
