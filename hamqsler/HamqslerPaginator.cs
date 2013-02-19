/*
 *  Author:
 *       Jim Orcheson <jimorcheson@gmail.com>
 * 
 *  Copyright © 2012, 2013 Jim Orcheson
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
			int cardNumber = pageNumber * cardsPerPage;
			Canvas canvas = new Canvas();
			for(int j = 0; j < cardsHigh; j++)
			{
				for(int i = 0; i < cardsWide; i++)
				{
					CardView cView = BuildCardViewForPrinting(ref cardNumber);
					Canvas.SetLeft(cView, i * card.DisplayWidth);
					Canvas.SetTop(cView, j * card.DisplayHeight);
					canvas.Children.Add(cView);
				}
			}
			canvas.Measure(PageSize);
			canvas.Arrange(new Rect(PageSize));
			canvas.UpdateLayout();
			return new DocumentPage(canvas);
		}

		/// <summary>
		/// Helper method that creates a CardView object for printing
		/// </summary>
		/// <param name="cardNumber">Number of the card that is being printed</param>
		/// <returns>CardView object to be printed</returns>
		private CardView BuildCardViewForPrinting(ref int cardNumber)
		{
			Card vCard = card.Clone();
			vCard.IsInDesignMode = false;
			CardView cView = new CardView(vCard, vCard.IsInDesignMode);
			if (cardNumber < dispQsos.Count) {
				((QsosBoxView)vCard.QsosBox.CardItemView).Qsos = dispQsos[cardNumber];
			}
			else
			{
				((QsosBoxView)vCard.QsosBox.CardItemView).BuildQsos();
			}
			foreach (FrameworkElement elt in cView.CanvasForCard.Children) 
			{
				TextItemView tiv = elt as TextItemView;
				if (tiv != null) 
				{
					tiv.SetDisplayText(((QsosBoxView)vCard.QsosBox.CardItemView).Qsos);
				}
			}
			cardNumber++;
			return cView;
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
