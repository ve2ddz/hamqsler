﻿/*
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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace hamqsler
{
	/// <summary>
	/// Paginator for QslCards
	/// </summary>
	public class HamqslerPaginator : DocumentPaginator
	{
		private int cardsWide;
		private int cardsHigh;
		private int cardsPerPage;
		private int pageCount;
		private Card card;
		private Size pageSize;
		private List<List<DispQso>> dispQsos;
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="qslCard">Card to be printed</param>
		/// <param name="qsos">Collection of Qsos to be displayed on all cards</param>
		/// <param name="size">Size of the paper the cards are to be printed on</param>
		public HamqslerPaginator(Card qslCard, DisplayQsos qsos, Size size)
		{
			card = qslCard;
			dispQsos = qsos.GetDispQsosList(card);
			pageSize = size;
			CalculatePageCount();
		}
		
		public override bool IsPageCountValid {
			get {return true;}
		}
		
		public override int PageCount {
			get {return pageCount;}
		}
		
		public override IDocumentPaginatorSource Source {
			get {return null;}
		}
		
		public override Size PageSize {
			get {return pageSize;}
			set {pageSize = value;}
		}
		
		/// <summary>
		/// Creates a page for printing
		/// </summary>
		/// <param name="pageNumber">Number of the page to print</param>
		/// <returns>DocumentPage obejct to be printed</returns>
		public override DocumentPage GetPage(int pageNumber)
		{
			// for now, we assume Letter sized paper and 4 cards per page
			int cardNumber = pageNumber * cardsPerPage;
			Canvas canvas = new Canvas();
			canvas.Measure(PageSize);
			canvas.Arrange(new Rect(PageSize));
			CardView cView = new CardView(card, cardNumber > dispQsos.Count);
			if(cardNumber < dispQsos.Count)
			{
				card.QsosBox.QBoxView.Qsos = dispQsos[cardNumber];
			}
			cardNumber++;
			Canvas.SetLeft(cView, 0);
			Canvas.SetTop(cView, 0);
			canvas.Children.Add(cView);
			CardView cView2 = new CardView(card, cardNumber > dispQsos.Count);
			if(cardNumber < dispQsos.Count)
			{
				card.QsosBox.QBoxView.Qsos = dispQsos[cardNumber];
			}
			cardNumber++;
			Canvas.SetLeft(cView2, card.DisplayWidth);
			Canvas.SetTop(cView2, 0);
			canvas.Children.Add(cView2);
			CardView cView3 = new CardView(card, cardNumber > dispQsos.Count);
			if(cardNumber < dispQsos.Count)
			{
				card.QsosBox.QBoxView.Qsos = dispQsos[cardNumber];
			}
			cardNumber++;
			Canvas.SetLeft(cView3, 0);
			Canvas.SetTop(cView3, card.DisplayHeight);
			canvas.Children.Add(cView3);
			CardView cView4 = new CardView(card, cardNumber > dispQsos.Count);
			if(cardNumber < dispQsos.Count)
			{
				card.QsosBox.QBoxView.Qsos = dispQsos[cardNumber];
			}
			cardNumber++;
			Canvas.SetLeft(cView4, card.DisplayWidth);
			Canvas.SetTop(cView4, card.DisplayHeight);
			canvas.Children.Add(cView4);
			canvas.UpdateLayout();
			return new DocumentPage(canvas);
		}
		
		/// <summary>
		/// Calculate number of pages to be printed, along with a number of
		/// other properties used in the calculation that are needed in GetPage
		/// </summary>
		private void CalculatePageCount()
		{
			// NOTE: currently assumes landscape mode
			double pageWidth = PageSize.Height;
			double pageHeight = PageSize.Width;
			cardsWide = ((int)(pageWidth / card.DisplayWidth));
			cardsHigh = ((int)(pageHeight / card.DisplayHeight));
			cardsPerPage = cardsWide * cardsHigh;
			pageCount = (dispQsos.Count + cardsPerPage - 1) / cardsPerPage;
			if(pageCount == 0)
			{
				pageCount = 1;
			}
		}
	}
}
