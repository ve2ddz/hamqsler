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
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace hamqsler
{
	/// <summary>
	/// QSL card
	/// </summary>
	[Serializable]
	public class Card : CardItem
	{
		private static readonly DependencyProperty PrintCardOutlinesProperty =
			DependencyProperty.Register("PrintCardOutlines", typeof(bool), typeof(Card),
			                            new PropertyMetadata(true));
		public bool PrintCardOutlines
		{
			get {return (bool)GetValue(PrintCardOutlinesProperty);}
			set {SetValue(PrintCardOutlinesProperty, value);}
		}
		
		private static readonly DependencyProperty FillLastPageWithBlankCardsProperty =
			DependencyProperty.Register("FillLastPageWithBlankCards", typeof(bool), typeof(Card),
			                            new PropertyMetadata(true));
		public bool FillLastPageWithBlankCards
		{
			get {return (bool)GetValue(FillLastPageWithBlankCardsProperty);}
			set {SetValue(FillLastPageWithBlankCardsProperty, value);}
		}
		
		private static readonly DependencyProperty SetCardMarginsToPrinterPageMarginsProperty =
			DependencyProperty.Register("SetCardMarginsToPrinterPageMargins", typeof(bool),
			                            typeof(Card), new PropertyMetadata(true));
		public bool SetCardMarginsToPrinterPageMargins
		{
			get {return (bool)GetValue(SetCardMarginsToPrinterPageMarginsProperty);}
			set {SetValue(SetCardMarginsToPrinterPageMarginsProperty, value);}
		}
		
		private static readonly DependencyProperty BackgroundImageProperty = 
			DependencyProperty.Register("BackImage", typeof(BackgroundImage), typeof(Card),
			                            new PropertyMetadata(null));
		public BackgroundImage BackImage
		{
			get {return (BackgroundImage)GetValue(BackgroundImageProperty);}
			set {SetValue(BackgroundImageProperty, value);}
		}
		
		private static readonly DependencyProperty SecondaryImagesProperty =
			DependencyProperty.Register("SecondaryImages", typeof(List<SecondaryImage>),
			                            typeof(Card), new PropertyMetadata(new List<SecondaryImage>()));
		public List<SecondaryImage> SecondaryImages
		{
			get {return (List<SecondaryImage>)GetValue(SecondaryImagesProperty);}
		}
			
		[NonSerialized]
		private UserPreferences userPreferences;
		
		private bool isDirty = false;
		public bool IsDirty
		{
			get {return isDirty;}
			set {isDirty = value;}
		}
		
		/// <summary>
		/// default constructor (called when loading Card from XML)
		/// </summary>
		public Card()
		{
			userPreferences = ((App)Application.Current).UserPreferences;
			DisplayRectangle = new Rect(0, 0, 0, 0);
			QslCard = this;
		}
		
		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="width">Width of the card in device independent units</param>
		/// <param name="height">Height of the card in device independent units</param>
		public Card(double width, double height)
		{
			userPreferences = ((App)Application.Current).UserPreferences;
			PrintCardOutlines = userPreferences.PrintCardOutlines;
			FillLastPageWithBlankCards = userPreferences.PrintCardOutlines;
			SetCardMarginsToPrinterPageMargins = userPreferences.SetCardMarginsToPrinterPageMargins;
			DisplayRectangle = new Rect(0, 0, width, height);
			BackImage = new BackgroundImage();
			BackImage.QslCard = this;
			QslCard = this;
		}
		
		/// <summary>
		/// OnRender event handler - draws the card
		/// </summary>
		/// <param name="dc">Context to draw the card on</param>
		protected override void OnRender(DrawingContext dc)
		{
			base.OnRender(dc);
			Brush brush = Brushes.White;
			Pen pen = new Pen(Brushes.Transparent, 1);
			if(PrintCardOutlines)
			{
				pen = new Pen(Brushes.Black, 1);
			}
			dc.DrawRectangle(brush, pen, DisplayRectangle);
			if(BackImage != null)
			{
				BackImage.Render(dc);
			}
			foreach(SecondaryImage si in SecondaryImages)
			{
				si.Render(dc);
			}
		}
		
		/// <summary>
		/// Sets the CardItem that the cursor is over as highlighted
		/// </summary>
		/// <param name="x">X position of the cursor relative to the upper left corner of this Card</param>
		/// <param name="y">Y position of the cursor relative to the upper left corner of this card</param>
		/// <returns></returns>
		public CardItem SetHighlighted(double x, double y)
		{
			CardItem ci;
			ClearHighlighted();
			ci = CursorOver(x, y);
			if(ci != null)
			{
				ci.IsHighlighted = true;
			}
			return ci;
		}
		
		/// <summary>
		/// Gets the CardItem that is highlighted
		/// </summary>
		/// <returns>Highlighted card item or null if no card item is highlighted</returns>
		public CardItem GetHighlighted()
		{
			foreach(SecondaryImage si in SecondaryImages)
			{
				if(si.IsHighlighted)
				{
					return si;
				}
			}
			if(BackImage.IsHighlighted)
			{
				return BackImage;
			}
			return null;
		}
		
		/// <summary>
		/// Clears the IsHighlighted property of every CardItem child of this Card
		/// </summary>
		public void ClearHighlighted()
		{
			foreach(SecondaryImage si in SecondaryImages)
			{
				si.IsHighlighted = false;
			}
			BackImage.IsHighlighted = false;
		}
		
		/// <summary>
		/// Get the CardItem that the cursor is over
		/// </summary>
		/// <param name="x">X position of the cursor relative to the top left corner of this Card</param>
		/// <param name="y">Y poistion of the cursor relative to the top left corner of this Card</param>
		/// <returns>CardItem the cursor is over, or null if not over a child CardItem</returns>
		public CardItem CursorOver(double x, double y)
		{
			foreach(SecondaryImage si in SecondaryImages)
			{
				if(CardItem.WithinRectangle(si.DisplayRectangle, x, y))
				{
					return si;
				}
			}
			if(BackImage != null && CardItem.WithinRectangle(BackImage.DisplayRectangle,
			                                                 x, y))
			{
				return BackImage;
			}
			return null;
		}
		
		/// <summary>
		/// Get the CardItem that is selected
		/// </summary>
		/// <returns>Selected card item, or null if none selected</returns>
		public CardItem GetSelected()
		{
			foreach(SecondaryImage si in SecondaryImages)
			{
				if(si.IsSelected)
					return si;
			}
			if(BackImage.IsSelected)
			{
				return BackImage;
			}
			return null;
		}
		
		/// <summary>
		/// Adds a SecondaryImage to the card. No image is actually loaded; this must be done
		/// from the SecondaryImageProperties panel.
		/// </summary>
		public void AddImage()
		{
			SecondaryImage si = new SecondaryImage();
			SecondaryImages.Add(si);
			si.QslCard = this;
			CardItem ci = GetHighlighted();
			if(ci != null)
			{
				ci.IsHighlighted = false;
			}
			si.IsSelected = true;
			InvalidateVisual();
		}
				
	}
	
}
