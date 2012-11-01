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
	/// Interaction logic for CardTabItem.xaml
	/// </summary>
	public partial class CardTabItem : TabItem
	{
		private Card card = null;
		
		/// <summary>
		/// CardTabItem constructor
		/// </summary>
		/// <param name="cardWidth">Width of the card in graphics independent units</param>
		/// <param name="cardHeight">Height of the card in graphics independent units</param>
		public CardTabItem(double cardWidth, double cardHeight)
		{
			InitializeComponent();
			// create a card and position it in the middle of the CardCanvas
			card = new Card(cardWidth, cardHeight);
			double left = (CardCanvas.Width - cardWidth) / 2;
			double top = (CardCanvas.Height - cardHeight) / 2;
			Canvas.SetLeft(card, left);
			Canvas.SetTop(card, top);
			this.CardCanvas.Children.Add(card);
		}
		
	}
}