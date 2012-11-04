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
	/// Interaction logic for CardCanvas.xaml
	/// </summary>
	public partial class CardCanvas : Canvas
	{
		private Card qslCard = null;
		public Card QslCard
		{
			get {return qslCard;}
		}
		
		/// <summary>
		/// Default constructor
		/// </summary>
		public CardCanvas()
		{
			InitializeComponent();
		}
		
		public void CreateCard(double cardWidth, double cardHeight)
		{
			qslCard = new Card(cardWidth, cardHeight);
			double left = (this.Width - cardWidth) / 2;
			double top = (this.Height - cardHeight) / 2;
			Canvas.SetLeft(QslCard, left);
			Canvas.SetTop(QslCard, top);
			this.Children.Add(QslCard);
			this.MouseMove += OnMouseMove;
		}

		/// <summary>
		/// Handler for MouseMove events. Simply calls MoveMouse method for the CardItem that the
		/// mouse is over.
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">MouseEventArgs object</param>
		public void OnMouseMove(object sender, MouseEventArgs e)
		{
			// Is the cursor over a CardItem?
			Point location = e.GetPosition(QslCard);
			CardItem ci = QslCard.CursorOver(location.X, location.Y);
			QslCard.ClearHighlighted();
			if(ci != null)
			{
				// for now we need to highlight the CardItem
				if(!ci.IsHighlighted)
				{
					QslCard.ClearHighlighted();
					ci.IsHighlighted = true;
				}
				// delegate MouseMove event to the CardItem
				ci.MoveMouse(e);
			}
			QslCard.InvalidateVisual();
		}
		
	}
}