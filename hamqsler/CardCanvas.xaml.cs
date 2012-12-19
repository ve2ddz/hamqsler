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
		public static RoutedCommand AddImageCommand = new RoutedCommand();
		public static RoutedCommand AddTextCommand = new RoutedCommand();
		public static RoutedCommand AddQsosBoxCommand = new RoutedCommand();
		public static RoutedCommand DeleteItemCommand = new RoutedCommand();
		public static RoutedCommand ClearBackgroundCommand = new RoutedCommand();
		
		private Card qslCard = null;
		public Card QslCard
		{
			get {return qslCard;}
			set {qslCard = value;}
		}
		
		private CardView cardView = null;
		public CardView CardView
		{
			get {return cardView;}
			set {cardView = value;}
		}
		
		private Point highlightPoint;
		
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
			QslCard = new Card(cardWidth, cardHeight, true);
			double left = (this.Width - cardWidth) / 2;
			double top = (this.Height - cardHeight) / 2;
			cardView = new CardView(QslCard, true);
			Canvas.SetLeft(cardView, left);
			Canvas.SetTop(cardView, top);
			this.Children.Add(cardView);
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
		/// <param name="sender">not used</param>
		/// <param name="e">CanExecuteRoutedEventArgs object</param>
		private void DeselectCardItemCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			CardItem ci = QslCard.GetSelected();
			e.CanExecute = ci != null;
		}
		
		/// <summary>
		/// CanExecute handler for AddImage menu item
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void AddImageCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			CardItem ci = QslCard.GetSelected();
			e.CanExecute = ci == null;
		}
		
		/// <summary>
		/// CanExecute handler for AddText menu item
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void AddTextCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			CardItem ci = QslCard.GetSelected();
			e.CanExecute = ci == null;
		}
		
		/// <summary>
		/// CanExecute handler for Add Qsos Box menu item
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">CanExecuteRoutedEventArgs object</param>
		private void AddQsosBoxCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			e.CanExecute = QslCard.QsosBox == null;
		}
		
		/// <summary>
		/// CanExecute handler for Delete Item menu item
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">CanExecuteRoutedEventArgs object</param>
		private void DeleteItemCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			CardItem ci = QslCard.GetSelected();
			e.CanExecute = ci != null && ci.GetType() != typeof(BackgroundImage);
		}
		
		/// <summary>
		/// CanExecute handler for ClearBackground menu item
		/// </summary>
		/// <param name="sender">not ued</param>
		/// <param name="e">CanExecuteRoutedEventArgs object</param>
		private void ClearBackgroundCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
		{
			CardItem ci = QslCard.GetSelected();
			e.CanExecute = (ci == null || ci.GetType() == typeof(BackgroundImage)) &&
				QslCard.BackImage.ImageFileName != null &&
				QslCard.BackImage.ImageFileName != string.Empty;
		}

		/// <summary>
		/// Handler for MouseMove events. Simply calls MoveMouse method for the CardItem that the
		/// mouse is over.
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">MouseEventArgs object</param>
		private void OnMouseMove(object sender, MouseEventArgs e)
		{
			// Is the cursor over a CardItem?
			CardItemView civ = cardView.GetCardItemViewForSelectedCardItem();
			if(civ != null)
			{
				civ.MoveMouse(cardView, e);
			}
			else
			{
				Point location = e.GetPosition(cardView);
				civ = cardView.GetCardItemViewCursorIsOver(location.X, location.Y);
				if(civ != null)
				{
					// delegate MouseMove event to the CardItemView
					civ.MoveMouse(cardView, e);
				}
				else
				{
					QslCard.ClearHighlighted();
					Mouse.OverrideCursor = Cursors.Arrow;
				}
			}
		}
		
		/// <summary>
		/// Handle MouseLeftButtonDown event
		/// </summary>
		/// <param name="sender">Source of the event</param>
		/// <param name="e">MouseButtonEventArgs object</param>
        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        	CardItemView civ = cardView.GetCardItemViewForSelectedCardItem();
        	if(civ != null)
        	{
        		this.CaptureMouse();
        		civ.OnMouseLeftButtonDown(cardView, e);
        	}
        }
        
        /// <summary>
        /// Handle MouseLeftButtonUp event
        /// </summary>
        /// <param name="sender">Source of the event</param>
        /// <param name="e">MouseButtonEventArgs object</param>
        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
        	this.ReleaseMouseCapture();
			CardItemView civ = cardView.GetCardItemViewForSelectedCardItem();
        	if(civ != null)
        	{
        		civ.OnMouseLeftButtonUp(sender, e);
        	}
        }
        
		/// <summary>
		/// Handler for SelectCardItem menu item Executed (Clicked) event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void SelectCardItemCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			CardItemView civ = cardView.GetCardItemViewCursorIsOver(highlightPoint.X, highlightPoint.Y);
			if(civ != null)
			{
				QslCard.ClearHighlighted();
				civ.ItemData.IsSelected = true;
				SetPropertiesPanelVisibility(civ.ItemData);
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
				SetPropertiesPanelVisibility(null);
			}
		}
		
		/// <summary>
		/// Helper method that calls SetPropertiesVisibility in CardTabItem to set visibility
		/// of a properties panel based on selected carditem
		/// </summary>
		private void SetPropertiesPanelVisibility(CardItem ci)
		{
			GetCardTabItem().SetPropertiesVisibility(ci);
		}
		
		/// <summary>
		/// Handler for Delete Item menu item Executed (Clicked) event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		public void DeleteItemCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			QslCard.DeleteItem();
			GetCardTabItem().SetPropertiesVisibility(QslCard.GetSelected());
			cardView.RebuildCardView();
		}
		
		/// <summary>
		/// Handler for ClearBackground menu item Executed (Clicked) event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		public void ClearBackgroundCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			QslCard.BackImage.ImageFileName = string.Empty;
			// the following line ensures that if the background image was selected before
			// Clear Background was called, it remains selected.
			QslCard.BackImage.IsSelected = false;
			GetCardTabItem().SetPropertiesVisibility(null);
		}
		
		/// <summary>
		/// Helper method that gets the CardTabItem parent for this CardCanvas
		/// </summary>
		/// <returns>The parent CardTabItem</returns>
		private CardTabItem GetCardTabItem()
		{			
			FrameworkElement ctrl = (FrameworkElement)this.Parent;
			while(ctrl.GetType() != typeof(CardTabItem))
				ctrl = (FrameworkElement)ctrl.Parent;
			return (CardTabItem)ctrl;
		}
		
		/// <summary>
		/// Handler for Add Image menu item Executed (Clicked) event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		public void AddImageCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			QslCard.AddImage();
			GetCardTabItem().SetPropertiesVisibility(QslCard.GetSelected());
			cardView.RebuildCardView();
		}
		
		/// <summary>
		/// Handler for Add Text menu item Executed (Clicked) event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		public void AddTextCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			QslCard.AddText();
			GetCardTabItem().SetPropertiesVisibility(QslCard.GetSelected());
			cardView.RebuildCardView();
		}	
		
		/// <summary>
		/// Handler for Add QsosBox menu item Executed (Clicked) event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		public void AddQsosBoxCommand_Executed(object sender, ExecutedRoutedEventArgs e)
		{
			QslCard.AddQsosBox();
			GetCardTabItem().SetPropertiesVisibility(QslCard.GetSelected());
			cardView.RebuildCardView();
			
		}
		
		/// <summary>
		/// Handler for MouseRightButtonDown event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">MouseButtonEventArgs object</param>
		void OnMouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
			// A right mouse button click will display the context menu from which a menu item
			// may be selected. The location information passed to the menu item handlers is the 
			// location of the cursor when the menu item is clicked, not the location of the 
			// cursor when the right mouse button was pressed. We must save and use the location 
			// of the cursor here so that the correct CardItem will be selected.
			//
			highlightPoint = e.GetPosition(cardView);
			// now display the context menu
			base.OnMouseRightButtonDown(e);
		}
			}
}