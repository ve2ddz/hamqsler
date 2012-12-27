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
		private Card card;
		private Size pageSize;
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="qslCard">Card to be printed</param>
		/// <param name="size"></param>
		public HamqslerPaginator(Card qslCard, Size size)
		{
			card = qslCard;
			pageSize = size;
		}
		
		public override bool IsPageCountValid {
			get {return true;}
		}
		
		public override int PageCount {
			get {return 1;}
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
			Canvas canvas = new Canvas();
			canvas.Measure(PageSize);
			canvas.Arrange(new Rect(PageSize));
			CardView cView = new CardView(card, true);
			Canvas.SetLeft(cView, 0);
			Canvas.SetTop(cView, 0);
			canvas.Children.Add(cView);
			CardView cView2 = new CardView(card, true);
			cView2.UpdateLayout();
			Canvas.SetLeft(cView2, card.DisplayWidth);
			Canvas.SetTop(cView2, 0);
			canvas.Children.Add(cView2);
			CardView cView3 = new CardView(card, true);
			Canvas.SetLeft(cView3, 0);
			Canvas.SetTop(cView3, card.DisplayHeight);
			canvas.Children.Add(cView3);
			CardView cView4 = new CardView(card, true);
			Canvas.SetLeft(cView4, card.DisplayWidth);
			Canvas.SetTop(cView4, card.DisplayHeight);
			canvas.Children.Add(cView4);
			canvas.UpdateLayout();
			return new DocumentPage(canvas);
		}
	}
}
