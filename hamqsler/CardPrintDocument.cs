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
using System.Drawing;
using System.Drawing.Printing;
using System.Windows;

namespace hamqsler
{
	/// <summary>
	/// CardPrintDocument class - Control class for printing Qsl Cards
	/// </summary>
	public class CardPrintDocument : PrintDocument
	{
		private PrintProperties printProperties = null;
		public PrintProperties PrintProperties
		{
			get {return printProperties;}
			set {printProperties = value;}
		}
		
		private CardWF qslCard = null;
		public CardWF QslCard
		{
			get {return qslCard;}
			set {qslCard = value;}
		}
		
		private int cardsWide = 0;
		private int cardsHigh = 0;
		private float leftOffset = 0;
		private float topOffset = 0;
		private int cardsPrinted = 0;
		
		private DisplayQsos qsos = null;
		private List<List<DispQso>> dispQsos;
		
		/// <summary>
		/// Constructor
		/// </summary>
		public CardPrintDocument(DisplayQsos dQsos)
		{
			qsos = dQsos;
		}
		
		/// <summary>
		/// BeginPrint event handler
		/// </summary>
		/// <param name="e">PrintEventArgs object</param>
		protected override void OnBeginPrint(PrintEventArgs e)
		{
			base.OnBeginPrint(e);
			cardsPrinted = 0;
			dispQsos = qsos.GetDispQsosList(QslCard);
			if(PrinterSettings == null || QslCard == null)
			{
				App.Logger.Log("Programming error: Attempting to print cards before " +
				               "CardPrintDocument.PrintProperties or CardPrintDocument" +
				               ".QslCard is initialized.");
				MessageBox.Show("Programming Error: Please log a bug report and include " +
				                "the contents of the log file", "Programming Error",
				                MessageBoxButton.OK, MessageBoxImage.Error);
				e.Cancel = true;
				return;
			}
			this.DocumentName = "Qsl Cards";
			PrinterSettings settings = new PrinterSettings();
			settings.PrinterName = PrintProperties.PrinterName;
			settings.DefaultPageSettings.PaperSize = PrintProperties.PrinterPaperSize;
			settings.DefaultPageSettings.PrinterResolution = PrintProperties.Resolution;
			if(PrintProperties.Layout == PrintProperties.CardLayouts.LandscapeTopLeft ||
			   PrintProperties.Layout == PrintProperties.CardLayouts.LandscapeTopCenter ||
			   PrintProperties.Layout == PrintProperties.CardLayouts.LandscapeCenter)
			{
				settings.DefaultPageSettings.Landscape = true;
			}
			this.PrinterSettings = settings;

			CalculateCardsPerPage();
			CalculateOffsets();
		}
		
		/// <summary>
		/// Helper method that calculates the number of cards that can be printed
		/// on a page
		/// </summary>
		private void CalculateCardsPerPage()
		{
			int pageWidth = 0;
			int pageHeight = 0;
			if(PrintProperties.InsideMargins)
			{
				RectangleF area = this.PrinterSettings.DefaultPageSettings.PrintableArea;
				pageWidth = (int)area.Width;
				pageHeight = (int)area.Height;
				if(PrintProperties.Layout == PrintProperties.CardLayouts.LandscapeTopLeft ||
				   PrintProperties.Layout == PrintProperties.CardLayouts.LandscapeTopCenter ||
				   PrintProperties.Layout == PrintProperties.CardLayouts.LandscapeCenter)
				{
					pageWidth = (int)area.Height;
					pageHeight = (int)area.Width;
				}
			}
			else
			{
				pageWidth = this.PrinterSettings.DefaultPageSettings.PaperSize.Width;
				pageHeight = this.PrinterSettings.DefaultPageSettings.PaperSize.Height;
				if(PrintProperties.Layout == PrintProperties.CardLayouts.LandscapeTopLeft ||
				   PrintProperties.Layout == PrintProperties.CardLayouts.LandscapeTopCenter ||
				   PrintProperties.Layout == PrintProperties.CardLayouts.LandscapeCenter)
				{
					pageWidth = this.PrinterSettings.DefaultPageSettings.PaperSize.Height;
					pageHeight = this.PrinterSettings.DefaultPageSettings.PaperSize.Width;
				}
			}
			cardsWide = pageWidth / QslCard.Width;
			cardsHigh = pageHeight / QslCard.Height;
			if(App.Logger.DebugPrinting)
			{
				string info = string.Format("CardPrintDocument.CalculateCardsPerPage:" +
				                            Environment.NewLine +
				                            "Cards printed per page = {0} x {1}" +
				                            Environment.NewLine,
				                            cardsWide, cardsHigh);
				App.Logger.Log(info);
			}
		}
		
		/// <summary>
		/// Helper method that calculates the offsets for the top left corner of
		/// the first card to be printed on the page based on the card layout that
		/// the user selected
		/// </summary>
		private void CalculateOffsets()
		{
			float hardX = this.PrinterSettings.DefaultPageSettings.HardMarginX;
			float hardY = this.PrinterSettings.DefaultPageSettings.HardMarginY;
			switch(PrintProperties.Layout)
			{
				case PrintProperties.CardLayouts.PortraitTopLeft:
					leftOffset = PrintProperties.InsideMargins ? hardX : 0;
					topOffset = PrintProperties.InsideMargins ? hardY : 0;
					break;
				case PrintProperties.CardLayouts.PortraitTopCenter:
					leftOffset = (this.PrinterSettings.DefaultPageSettings.PaperSize.Width -
					              cardsWide * QslCard.Width) / 2;
					topOffset = PrintProperties.InsideMargins ? hardY : 0;
					break;
				case PrintProperties.CardLayouts.PortraitCenter:
					leftOffset = (this.PrinterSettings.DefaultPageSettings.PaperSize.Width -
					              cardsWide * QslCard.Width) / 2;
					topOffset = (this.PrinterSettings.DefaultPageSettings.PaperSize.Height -
					             cardsHigh * QslCard.Height) / 2;
					break;
				case PrintProperties.CardLayouts.LandscapeTopLeft:
					leftOffset = PrintProperties.InsideMargins ? hardX : 0;
					topOffset = PrintProperties.InsideMargins ? hardY : 0;
					break;
				case PrintProperties.CardLayouts.LandscapeTopCenter:
					leftOffset = (this.PrinterSettings.DefaultPageSettings.PaperSize.Height -
					             cardsWide * QslCard.Width) / 2;
					topOffset = PrintProperties.InsideMargins ? hardY : 0;
					break;
				case PrintProperties.CardLayouts.LandscapeCenter:
					leftOffset = (this.PrinterSettings.DefaultPageSettings.PaperSize.Height -
					             cardsWide * QslCard.Width) / 2;
					topOffset = (this.PrinterSettings.DefaultPageSettings.PaperSize.Width -
					              cardsHigh * QslCard.Height) / 2;
					break;
			}
		}
		
		/// <summary>
		/// Handler for PrintPage event
		/// </summary>
		/// <param name="e">PrintPageEventArgs</param>
		protected override void OnPrintPage(PrintPageEventArgs e)
		{
			base.OnPrintPage(e);
			Graphics g = e.Graphics;
			float hardX = this.PrinterSettings.DefaultPageSettings.HardMarginX;
			float hardY = this.PrinterSettings.DefaultPageSettings.HardMarginY;
			int cardsPerPage = cardsWide * cardsHigh;
			for(int hCards = 0; hCards < cardsWide; hCards++)
			{
				for(int vCards = 0; vCards < cardsHigh; vCards++)
				{
					if(cardsPrinted < dispQsos.Count || PrintProperties.FillLastPage)
					{
						CardWF card = QslCard.Clone();
						card.CardPrintProperties = this.PrintProperties;
						FormsCardView view = new FormsCardView(card);
						System.Drawing.Drawing2D.GraphicsState gState = g.Save();
						float x = leftOffset + hCards * card.Width - hardX;
						float y = topOffset + vCards * card.Height - hardY;
						if(App.Logger.DebugPrinting)
						{
							App.Logger.Log("Printing card at " + x + ", " + y +
							               Environment.NewLine);
						}
						g.TranslateTransform(x, y);
						List<DispQso> qs = null;
						if(cardsPrinted < dispQsos.Count)
						{
							qs = dispQsos[cardsPrinted];
						}
						view.PaintCard(g, qs);
						g.Restore(gState);
						cardsPrinted++;
					}
				}
			}
			e.HasMorePages = cardsPrinted < dispQsos.Count;
		}
	}
}
