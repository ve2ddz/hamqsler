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
		public static RoutedCommand SelectCardItemCommand = new RoutedCommand();
		public static RoutedCommand DeselectCardItemCommand = new RoutedCommand();
		
		private Card qslCard = null;
		public Card QslCard
		{
			get {return qslCard;}
			set {qslCard = value;}
		}
		
		/// <summary>
		/// Default constructor
		/// </summary>
		public CardCanvas()
		{
			InitializeComponent();
		}
		
		/// <summary>
		/// Create a QSL card and place it in the middle of the card canvas
		/// </summary>
		/// <param name="cardWidth">Width of card in device independent units</param>
		/// <param name="cardHeight">Height of card in device independent units</param>
		public void CreateCard(double cardWidth, double cardHeight)
		{
			QslCard = new Card(cardWidth, cardHeight);
			double left = (this.Width - cardWidth) / 2;
			double top = (this.Height - cardHeight) / 2;
			Canvas.SetLeft(QslCard, left);
			Canvas.SetTop(QslCard, top);
			this.Children.Add(QslCard);
			this.MouseMove += OnMouseMove;
		}
		
		/// <summary>
		/// CanExecute handler for SelectCardItem menu item
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">CanExecuteRoutedEventArgs object</param>
		private void SelectCardItemCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			CardItem ci = QslCard.GetHighlighted();
			e.CanExecute = ci != null;
		}
		
		/// <summary>
		/// CanExecute handler for DeselectCardItem menu item
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void DeselectCardItemCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			CardItem ci = QslCard.GetSelected();
			e.CanExecute = ci != null;
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
			if(ci != null)
			{
				if(!ci.IsSelected)
				{
					QslCard.ClearHighlighted();
					// for now we need to highlight the CardItem
					if(!ci.IsHighlighted)
					{
						QslCard.ClearHighlighted();
						ci.IsHighlighted = true;
					}
				}
				// delegate MouseMove event to the CardItem
				ci.MoveMouse(e);
			}
			else
			{
				QslCard.ClearHighlighted();
			}
			QslCard.InvalidateVisual();
		}
		
		/// <summary>
		/// Handler for SelectCardItem menu item Executed (Clicked) event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void SelectCardItemCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			CardItem ci = QslCard.GetHighlighted();
			if(ci != null)
			{
				QslCard.ClearHighlighted();
				ci.IsSelected = true;
				QslCard.InvalidateVisual();
				SetPropertiesPanelVisibility();
			}
		}
		
		/// <summary>
		/// Handler for DeselectCardItem menu item Executed (Clicked) event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void DeselectCardItemCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			CardItem ci = QslCard.GetSelected();
			if(ci != null)
			{
				ci.IsSelected = false;
				QslCard.InvalidateVisual();
				SetPropertiesPanelVisibility();
			}
		}
		
		/// <summary>
		/// Helper method that calls SetPropertiesVisibility in CardTabItem to set visibility
		/// of a properties panel based on selected carditem
		/// </summary>
		private void SetPropertiesPanelVisibility()
		{
			FrameworkElement ctrl = (FrameworkElement)this.Parent;
			while(ctrl.GetType() != typeof(CardTabItem))
				ctrl = (FrameworkElement)ctrl.Parent;
			((CardTabItem)ctrl).SetPropertiesVisibility(QslCard);
		}
	}
}