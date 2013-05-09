﻿/*
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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace hamqsler
{
	/// <summary>
	/// Control that displays a CardWF object (QslCard).
	/// </summary>
	public class FormsCardView : Panel
	{
		// Reference to the CardWF that this view displays
		private CardWF qslCard = null;
		public CardWF QslCard
		{
			get {return qslCard;}
			set {qslCard = value;}
		}
		
		private Point cardLocation = new Point(0, 0);
		public Point CardLocation
		{
			get {return cardLocation;}
			set {cardLocation = value;}
		}
		
		private static float[] dashPattern =  {5, 2, 3, 2};
		protected static Pen selectedPen = CreateSelectedPen();
		protected static Pen highlighedPen = CreateHighlightedPen();
		protected static ImageAttributes outsideCardAttrs = CreateOutsideCardImageAttributes();
		
		private System.Windows.Controls.ContextMenu contextMenu = 
			new System.Windows.Controls.ContextMenu();
		private CardWFItem highlightedCardItem;
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="card">QslCard to display</param>
		public FormsCardView(CardWF card)
		{
			this.DoubleBuffered = true;
			QslCard = card;
			QslCard.DispPropertyChanged += OnDispPropertyChanged;
			BuildContextMenu();
		}
		
		/// <summary>
		/// Creates the pen used to draw a rectangle around the selected CardWFItem
		/// </summary>
		/// <returns>Pen that highlights a selected CardWFItem</returns>
		public static Pen CreateSelectedPen()
		{
			Pen pen = new Pen(Color.Blue, 4);
			pen.DashPattern = dashPattern;
			return pen;
		}
		
		/// <summary>
		/// Creates the pen used to draw a rectangle around the highlighted CardWFItem
		/// </summary>
		/// <returns>Pen that highlights the selected CardWFItem</returns>
		public static Pen CreateHighlightedPen()
		{
			Pen pen = new Pen(Color.Orange, 4);
			pen.DashPattern = dashPattern;
			return pen;
		}
		
		/// <summary>
		/// Creates the ImageAttributes needed to display card items outside the card
		/// at 40% opacity
		/// </summary>
		/// <returns>ImageAttributes object with 40% opacity</returns>
		public static ImageAttributes CreateOutsideCardImageAttributes()
		{
		    // Create a color matrix that is 40% opaque
		    float[][] matrixItems ={ 
		                               new float[] {1, 0, 0, 0, 0},
		                               new float[] {0, 1, 0, 0, 0},
		                               new float[] {0, 0, 1, 0, 0},
		                               new float[] {0, 0, 0, 0.4f, 0}, 
		                               new float[] {0, 0, 0, 0, 1}}; 
		    ColorMatrix colorMatrix = new ColorMatrix(matrixItems);
		    // Create an ImageAttributes object and set its color matrix.
		    ImageAttributes imageAttrs = new ImageAttributes();
		    imageAttrs.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default,
		        ColorAdjustType.Bitmap);
		    return imageAttrs;
		}
		
		/// Draw the card
		/// </summary>
		/// <param name="e">PaintEventArgs object</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			PaintCard(e.Graphics);
		}
		
		/// <summary>
		/// Draw the card
		/// </summary>
		/// <param name="g">Graphics object on which to draw the card</param>
		public void PaintCard(Graphics g)
		{
			g.TranslateTransform(CardLocation.X, CardLocation.Y);
			g.FillRectangle(Brushes.White, new Rectangle(0, 0, QslCard.Width, QslCard.Height));
			PaintImage(g, QslCard.BackgroundImage);
			if(QslCard.CardPrintProperties.PrintCardOutlines)
			{
				g.DrawRectangle(Pens.Black, new Rectangle(
					0, 0, QslCard.Width - 1, QslCard.Height - 1));
			}
		}
		
		/// <summary>
		/// Handles CardWF property change events
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void OnDispPropertyChanged(object sender, EventArgs e)
		{
			this.Invalidate();
		}
		
		/// <summary>
		/// Paint the background image on the card.
		/// </summary>
		/// <param name="g">Graphics object on which to do the drawing</param>
		/// <param name="image">BackgorundImage object to draw</param>
		private void PaintImage(Graphics g, ImageWFBase image)
		{
			if(image.ImageFileName != null && image.ImageFileName != string.Empty)
			{
				GraphicsState state = g.Save();
				RectangleF clipRect = new RectangleF(0, 0, QslCard.Width, QslCard.Height);
				g.Clip = new Region(clipRect);
				g.DrawImage(image.Image, 
				            new Rectangle(image.X, image.Y, image.Width, image.Height));
				g.Restore(state);
				if(QslCard.IsInDesignMode)
				{
					g.DrawImage(image.Image, new Rectangle(
						image.X, image.Y, image.Width, image.Height), 0, 0, image.Image.Width,
						image.Image.Height, GraphicsUnit.Pixel, outsideCardAttrs);
				}
			}
			if(QslCard.IsInDesignMode)
			{
				if(image.IsHighlighted)
				{
					g.DrawRectangle(highlighedPen, new Rectangle(
						image.X, image.Y, image.Width, image.Height));
				}
				else if(image.IsSelected)
				{
					g.DrawRectangle(selectedPen, new Rectangle(
						image.X, image.Y, image.Width, image.Height));
				}
			}
		}
		
		/// <summary>
		/// Handler for MouseMove events
		/// </summary>
		/// <param name="e">MouseEventArgs object</param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if(e.Button == MouseButtons.None)
			{
				CardWFItem ci = QslCard.GetSelectedItem();
				if(ci == null)
				{
					ClearHighlights();
					if(QslCard.BackgroundImage.Contains(e.X - CardLocation.X, 
					                                    e.Y - CardLocation.Y))
					{
						QslCard.BackgroundImage.IsHighlighted = true;
					}
				}
			}
		}
		
		/// <summary>
		/// Handler for MouseLeave events
		/// </summary>
		/// <param name="e">EventArgs object</param>
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			// clear IsHighlighted property for every card item on the card.
			// This call is necessary because if the mouse is moved quickly away from the
			// highlighted card item, the card item IsHighlighted property might not be
			// unset.
			ClearHighlights();
		}
		
		/// <summary>
		/// Handler for MouseUp events
		/// </summary>
		/// <param name="e">MouseEventArgs object</param>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if(e.Button == MouseButtons.Right)
			{
				contextMenu.IsOpen = true;
			}
		}
		
		/// <summary>
		/// Helper function that sets the IsHighlighted property of every card item
		/// on the displayed card to false;
		/// </summary>
		protected void ClearHighlights()
		{
			 QslCard.BackgroundImage.IsHighlighted = false;
		}
		
		/// <summary>
		/// Helper method that builds the card view's context menu
		/// </summary>
		private void BuildContextMenu()
		{
			contextMenu.Opened += OnContextMenuOpen;
			System.Windows.Controls.MenuItem select = 
				new System.Windows.Controls.MenuItem();
			select.Header = "Select Item";
			select.Name = "SelectItem";
			select.Click += OnSelectItemClicked;
			contextMenu.Items.Add(select);
			
			System.Windows.Controls.MenuItem  deselect = 
				new System.Windows.Controls.MenuItem();
			deselect.Header = "Deselect Item";
			deselect.Name = "DeselectItem";
			deselect.Click += OnDeselectItemClicked;
			contextMenu.Items.Add(deselect);
			
			System.Windows.Controls.MenuItem clearBackground =
				new System.Windows.Controls.MenuItem();
			clearBackground.Header = "Clear Background Image";
			clearBackground.Name = "ClearBackgroundItem";
			clearBackground.Click += OnClearBackgroundClicked;
			contextMenu.Items.Add(clearBackground);
		}
		
		/// <summary>
		/// Handler for context menu's opened event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void OnContextMenuOpen(object sender, System.Windows.RoutedEventArgs e)
		{
			highlightedCardItem = QslCard.GetHighlightedItem();
			CardWFItem selectedCardItem = QslCard.GetSelectedItem();
			// determine the IsEnabled property for each menu item
			foreach(System.Windows.Controls.MenuItem mi in contextMenu.Items)
			{
				switch(mi.Name)
				{
					case "SelectItem":
						mi.IsEnabled = highlightedCardItem != null;
						break;
					case "DeselectItem":
						mi.IsEnabled = selectedCardItem != null;
						break;
					case "ClearBackgroundItem":
						mi.IsEnabled = QslCard.BackgroundImage.ImageFileName != null &&
							QslCard.BackgroundImage.ImageFileName != string.Empty &&
							(selectedCardItem == null || 
							 selectedCardItem == QslCard.BackgroundImage);
						break;
				}
			}
		}
		
		/// <summary>
		/// Handler for context menu's Select Item menu item clicked event
		/// </summary>
		/// <param name="sender">menu item that was clicked (Select Item)</param>
		/// <param name="e">RoutedEventArgs objecgt</param>
		private void OnSelectItemClicked(object sender, System.Windows.RoutedEventArgs e)
		{
			// pass processing to MainWindow.OnSelectItem_Clicked
			System.Windows.Controls.MenuItem mi = sender as System.Windows.Controls.MenuItem;
			mi.Tag = highlightedCardItem;
			((MainWindow)App.Current.MainWindow).OnSelectItem_Clicked(sender, e);
		}
		
		private void OnDeselectItemClicked(object sender, System.Windows.RoutedEventArgs e)
		{
			// pass processing to MainWindow.OnNone_Clicked
			((MainWindow)App.Current.MainWindow).OnNone_Clicked(sender, e);
		}
		
		private void OnClearBackgroundClicked(object sender, System.Windows.RoutedEventArgs e)
		{
			// pass processing to MainWindow.ClearBackgroundCommand_Executed
			((MainWindow)App.Current.MainWindow).ClearBackgroundCommand_Executed(
				sender, null);
		}
		
	}
}
