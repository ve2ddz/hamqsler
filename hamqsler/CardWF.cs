/*
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
using System.Collections.Generic;
using System.Windows;

namespace hamqsler
{
	/// <summary>
	/// CardWF describes the QslCard.
	/// </summary>
	public class CardWF : CardWFItem
	{
		private static readonly DependencyProperty IsDirtyProperty =
			DependencyProperty.Register("IsDirty", typeof(bool),
			                            typeof(CardWF), new PropertyMetadata(false));
		public bool IsDirty
		{
			get {return (bool)GetValue(IsDirtyProperty);}
			set {SetValue(IsDirtyProperty, value);}
		}

		private static readonly DependencyProperty IsInDesignModeProperty =
			DependencyProperty.Register("IsInDesignMode", typeof(bool),
			                            typeof(CardWF), new PropertyMetadata(false));
		public bool IsInDesignMode
		{
			get {return (bool)GetValue(IsInDesignModeProperty);}
			set {SetValue(IsInDesignModeProperty, value);}
		}
		
		private static readonly DependencyProperty FileNameProperty =
			DependencyProperty.Register("FileName", typeof(string),
			                            typeof(CardWF), new PropertyMetadata(null));
		public string FileName
		{
			get {return GetValue(FileNameProperty) as string;}
			set {SetValue(FileNameProperty, value);}
		}
		
		private BackgroundWFImage backgroundImage = new BackgroundWFImage();
		public BackgroundWFImage BackgroundImage
		{
			get {return backgroundImage;}
			set {backgroundImage = value;}
		}
		
		private List<SecondaryWFImage> secondaryImages = new List<SecondaryWFImage>();
		public List<SecondaryWFImage> SecondaryImages
		{
			get {return secondaryImages;}
		}
		
		private List<TextWFItem> textItems = new List<TextWFItem>();
		public List<TextWFItem> TextItems
		{
			get {return textItems;}
		}
		
		private QsosWFBox qsosBox = new QsosWFBox();
		public QsosWFBox QsosBox
		{
			get {return qsosBox;}
			set {qsosBox = value;}
		}
		
		private static readonly DependencyProperty CardPrintPropertiesProperty =
			DependencyProperty.Register("CardPrintProperties", typeof(PrintProperties),
			                            typeof(CardWF),
			                            new PropertyMetadata(null));
		public PrintProperties CardPrintProperties
		{
			get {return GetValue(CardPrintPropertiesProperty) as PrintProperties;}
			set {SetValue(CardPrintPropertiesProperty, value);}
		}
		
		/// <summary>
		/// DispPropertyChanged event is called whenever a property that affects a
		/// CardWFItem display is changed
		/// </summary>
		public delegate void DisplayPropertyChanged(object sender, EventArgs e);
		public virtual event DisplayPropertyChanged DispPropertyChanged;
		
		/// <summary>
		/// Default constructor
		/// </summary>
		public CardWF() : base()
		{
			QslCard = this;
			App.Logger.Log("In CardWF default constructor:" + Environment.NewLine,
						   App.Logger.DebugPrinting);
			CardPrintProperties = new PrintProperties();
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="width">Width of the card (100 units/inch)</param>
		/// <param name="height">Height of the card (100 units/inch)</param>
		/// <param name="isInDesignMode">design mode indicator:
		/// true if in design mode, false otherwise</param>
		public CardWF(int width, int height, bool isInDesignMode) : base(width, height)
		{
			UserPreferences prefs = ((App)App.Current).UserPreferences;
			QslCard = this;
			App.Logger.Log("In CardWF constructor:" + Environment.NewLine,
						   App.Logger.DebugPrinting);
			CardPrintProperties = new PrintProperties();
			IsInDesignMode = isInDesignMode;
			ItemSize = new System.Drawing.Size(width, height);
			// background image
			BackgroundImage.QslCard = this;
			BackgroundImage.ImageFileName = @"$hamqslerFolder$\Samples\sample.jpg";
			// call text item
			TextWFItem call = new TextWFItem();
			call.QslCard = this;
			foreach(TextPart part in prefs.Callsign)
			{
				call.Text.Add(part);
			}
			call.TextFontFace = prefs.DefaultTextItemsFontFace;
			call.IsBold = true;
			call.FontSize = 48.0F;
			call.X = -70;
			call.Y = 0;
			TextItems.Add(call);
			QsosBox.QslCard = this;
		}
		
		/// <summary>
		/// Create a new CardWF object that is a hardcopy of this card
		/// </summary>
		/// <returns>CardWF object that is a hardcopy of this CardWF object</returns>
		public CardWF Clone()
		{
			CardWF card = new CardWF();
			card.ItemSize = new System.Drawing.Size(this.Width, this.Height);
			card.Location = new System.Drawing.Point(this.X, this.Y);
			return card;
		}
		
		/// <summary>
		/// Raise the DispPropertyChanged event
		/// </summary>
		public void RaiseDispPropertyChangedEvent()
		{
			if(DispPropertyChanged != null)
			{
				DispPropertyChanged(this, new EventArgs());
			}
		}
		
		/// <summary>
		/// Retrieve the card item that is selected on this card
		/// </summary>
		/// <returns>Selected card item, if any, or null if none selected</returns>
		public CardWFItem GetSelectedItem()
		{
			if(QsosBox != null && QsosBox.IsSelected)
			{
				return QsosBox;
			}
			foreach(TextWFItem tItem in TextItems)
			{
				if(tItem.IsSelected)
				{
					return tItem;
				}
			}
			foreach(SecondaryWFImage sImage in SecondaryImages)
			{
				if(sImage.IsSelected)
				{
					return sImage;
				}
			}
			if(BackgroundImage.IsSelected)
			{
				return BackgroundImage;
			}
			else return null;
		}
		
		/// <summary>
		/// Retrieve the highlighted card item on this card
		/// </summary>
		/// <returns>Highlighted card item, if any, or null if none highlighted</returns>
		public CardWFItem GetHighlightedItem()
		{
			if(QsosBox != null && QsosBox.IsHighlighted)
			{
				return QsosBox;
			}
			foreach(TextWFItem tItem in TextItems)
			{
				if(tItem.IsHighlighted)
				{
					return tItem;
				}
			}
			foreach(SecondaryWFImage sImage in SecondaryImages)
			{
				if(sImage.IsHighlighted)
				{
					return sImage;
				}
			}
			if(BackgroundImage.IsHighlighted)
			{
				return BackgroundImage;
			}
			else return null;
		}
		
		/// <summary>
		/// Get the relative position of the input coordinates relative to this card
		/// </summary>
		/// <param name="x">Card relative X coordinate</param>
		/// <param name="y">Card relative Y coordinate</param>
		/// <returns>RelativeLocations value</returns>
		public override CardWFItem.RelativeLocations GetRelativeLocation(int x, int y)
		{
			RelativeLocations location = RelativeLocations.Outside;
			if(this.Contains(x, y))
			{
				location = RelativeLocations.Inside;
			}
			return location;
		}
		
		/// <summary>
		/// Delete the selected card item
		/// </summary>
		public void DeleteSelectedItem()
		{
			CardWFItem ci = GetSelectedItem();
			if(ci != null)
			{
				if(ci.GetType() == typeof(SecondaryWFImage))
				{
					SecondaryImages.Remove((SecondaryWFImage)ci);
				}
				else if(ci.GetType() == typeof(TextWFItem))
				{
					TextItems.Remove((TextWFItem)ci);
				}
				else if(ci.GetType() == typeof(QsosWFBox))
				{
					QsosBox = null;
				}
				IsDirty = true;
				RaiseDispPropertyChangedEvent();
			}
		}
	}
}
