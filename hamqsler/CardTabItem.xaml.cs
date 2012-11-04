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
		/// <summary>
		/// CardTabItem constructor
		/// </summary>
		/// <param name="cardWidth">Width of the card in graphics independent units</param>
		/// <param name="cardHeight">Height of the card in graphics independent units</param>
		public CardTabItem(double cardWidth, double cardHeight)
		{
			InitializeComponent();
			// create a card and position it in the middle of the CardCanvas
			cardCanvas.CreateCard(cardWidth, cardHeight);
			this.DataContext = cardCanvas.QslCard;
			
		}
		
		/// <summary>
		/// Handles PrintCardOutlines checkbox clicked event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void PrintCardOutlines_Clicked(object sender, RoutedEventArgs e)
		{
			// force redisplay of the card
			cardCanvas.QslCard.InvalidateVisual();
		}
		
		/// <summary>
		/// Set visibility of properties groupboxes based on the CardItem selected in the card
		/// </summary>
		/// <param name="card">Card that contains the CardItems</param>
		public void SetPropertiesVisibility(Card card)
		{
			bool displayCardProps = true;
			if(card.BackImage.IsSelected)
			{
				displayCardProps = false;
			}
			backgroundImageProperties.Visibility = (card.BackImage.IsSelected) ?
				Visibility.Visible : Visibility.Collapsed;
			cardProperties.Visibility = displayCardProps ? Visibility.Visible :
				Visibility.Collapsed;
		}
	}
}