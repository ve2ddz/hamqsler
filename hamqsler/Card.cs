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
		
		private List<SecondaryImage> secondaryImages = new List<SecondaryImage>();
		public List<SecondaryImage> SecondaryImages
		{
			get {return secondaryImages;}
		}

		private List<TextItem> textItems = new List<TextItem>();
		public List<TextItem> TextItems
		{
			get {return textItems;}
		}
		
		private QsosBox qsosBox = null;
		public QsosBox QsosBox
		{
			get {return qsosBox;}
			set {qsosBox = value;}
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
			DisplayX = 0;
			DisplayY = 0;
			DisplayWidth = 0;
			DisplayHeight = 0;
			QslCard = this;
		}
		
		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="width">Width of the card in device independent units</param>
		/// <param name="height">Height of the card in device independent units</param>
        /// <param name="isInDesignMode">Boolean to indicate if this image is to be displayed
        /// in design mode</param>
		public Card(double width, double height, bool isInDesignMode)
			: base(new Rect(0, 0, width, height), isInDesignMode)
		{
			userPreferences = ((App)Application.Current).UserPreferences;
			// card properties
			PrintCardOutlines = userPreferences.PrintCardOutlines;
			FillLastPageWithBlankCards = userPreferences.PrintCardOutlines;
			SetCardMarginsToPrinterPageMargins = userPreferences.SetCardMarginsToPrinterPageMargins;
			// background image
			BackImage = new BackgroundImage(isInDesignMode);
			BackImage.QslCard = this;
			// call text item
			TextItem call = new TextItem(isInDesignMode);
			call.QslCard = this;
			foreach(TextPart part in userPreferences.Callsign)
			{
				call.Text.Add(part);
			}
			call.SetDisplayText();
			call.TextFontFace = new FontFamily(userPreferences.DefaultTextItemsFontFace);
			call.TextFontWeight = FontWeights.Black;
			call.FontSize = 72.0;
			call.DisplayX = -70;
			call.DisplayY = 0;
			call.DisplayWidth = 0;
			call.DisplayHeight = 0;
			call.SetDisplayText();
			TextItems.Add(call);
			// name Qth text item
			TextItem nameQth = new TextItem(isInDesignMode);
			nameQth.QslCard = this;
			foreach(TextPart part in userPreferences.NameQth)
			{
				nameQth.Text.Add(part);
			}
			nameQth.SetDisplayText();
			nameQth.TextFontFace = new FontFamily(userPreferences.DefaultTextItemsFontFace);
			nameQth.TextFontWeight = FontWeights.Normal;
			nameQth.FontSize = 10.0;
			nameQth.DisplayX = DisplayWidth / 2;
			nameQth.DisplayY = 15;
			nameQth.DisplayWidth = 0;
			nameQth.DisplayHeight = 0;
			nameQth.SetDisplayText();
			TextItems.Add(nameQth);
			// salutation
			TextItem salutation = new TextItem(isInDesignMode);
			salutation.QslCard = this;
			foreach(TextPart part in userPreferences.Salutation)
			{
				salutation.Text.Add(part);
			}
			salutation.SetDisplayText();
			salutation.TextFontFace = new FontFamily(userPreferences.DefaultTextItemsFontFace);
			salutation.FontSize = 10.0;
			salutation.DisplayX = 5;
			salutation.DisplayY = DisplayHeight - 25;
			salutation.DisplayWidth = 0;
			salutation.DisplayHeight = 0;
			salutation.SetDisplayText();
			TextItems.Add(salutation);
			
			// QsosBox
			qsosBox = new QsosBox(isInDesignMode);
			qsosBox.QslCard = this;
			// more card properties
			QslCard = this;
			isDirty = false;
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
			if(qsosBox != null && qsosBox.IsHighlighted)
			{
				return qsosBox;
			}
			foreach(TextItem ti in TextItems)
			{
				if(ti.IsHighlighted)
				{
					return ti;
				}
			}
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
			if(qsosBox != null)
			{
				qsosBox.IsHighlighted = false;
			}
			foreach(TextItem ti in TextItems)
			{
				ti.IsHighlighted = false;
			}
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
			if(qsosBox != null && CardItem.WithinRectangle(new Rect(qsosBox.DisplayX, qsosBox.DisplayY,
			                                                        qsosBox.DisplayWidth, 
			                                                        qsosBox.DisplayHeight),
			                                                        x, y))
			{
				return qsosBox;
			}
				
			// reverse items in list so that latest items checked first
			List<TextItem> revTextList = new List<TextItem>();
			foreach(TextItem ti in TextItems)
			{
				revTextList.Add(ti);
			}
			revTextList.Reverse();
			foreach(TextItem ti in revTextList)
			{
				if(CardItem.WithinRectangle(new Rect(ti.DisplayX, ti.DisplayY, ti.DisplayWidth,
				                                     ti.DisplayHeight), x, y))
				{
					return ti;
				}
			}
			
			// reverse items in list so that latest items checked first
			List<SecondaryImage> revImageList = new List<SecondaryImage>();
			foreach(SecondaryImage si in SecondaryImages)
			{
				revImageList.Add(si);
			}
			revImageList.Reverse();
			foreach(SecondaryImage si in revImageList)
			{
				if(CardItem.WithinRectangle(new Rect(si.DisplayX, si.DisplayY, si.DisplayWidth,
				                                     si.DisplayHeight), x, y))
				{
					return si;
				}
			}
			
			if(BackImage != null && CardItem.WithinRectangle(new Rect(BackImage.DisplayX,
			                                                          BackImage.DisplayY,
			                                                          BackImage.DisplayWidth,
			                                                          BackImage.DisplayHeight),
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
			if(qsosBox != null && qsosBox.IsSelected)
			{
				return qsosBox;
			}
			foreach(TextItem ti in TextItems)
			{
				if(ti.IsSelected)
					return ti;
			}
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
		}
				
		/// <summary>
		/// Adds a TextItem to the card. 
		/// </summary>
		public void AddText()
		{
			TextItem ti = new TextItem();
			ti.QslCard = this;
			TextItems.Add(ti);
			StaticText sText = new StaticText();
			sText.Text = "Text Item";
			ti.Text.Add(sText);
			ti.SetDisplayText();
			CardItem ci = GetHighlighted();
			if(ci != null)
			{
				ci.IsHighlighted = false;
			}
			ti.IsSelected = true;
		}
		
		/// <summary>
		/// Adds a QsosBox to the card
		/// </summary>
		public void AddQsosBox()
		{
			if(QsosBox == null)
			{
				QsosBox qBox = new QsosBox(IsInDesignMode);
				qBox.QslCard = this;
				this.QsosBox = qBox;
				qBox.IsSelected = true;
			}
		}
		
		/// <summary>
		/// Deletes selected CardItem
		/// </summary>
		public void DeleteItem()
		{
			CardItem ci = QslCard.GetSelected();
			if(ci != null)
			{
				if(ci.GetType() == typeof(SecondaryImage))
				{
					SecondaryImages.Remove((SecondaryImage)ci);
				}
				else if(ci.GetType() == typeof(TextItem))
				{
					TextItems.Remove((TextItem)ci);
				}
				else if(ci.GetType() == typeof(QsosBox))
				{
					QsosBox = null;
				}
			}
		}
				
	}
	
}
