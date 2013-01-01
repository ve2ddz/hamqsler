﻿/*
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
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;
using System.Xml.Serialization;

namespace hamqsler
{
	/// <summary>
	/// TextItem class - displays text on a QSL card
	/// </summary>
	[Serializable]
	public class TextItem : CardItem
	{
		private static readonly DependencyProperty TextFontFaceProperty =
			DependencyProperty.Register("TextFontFace", typeof(string), typeof(TextItem),
			                            new PropertyMetadata("Arial"));
		public string TextFontFace
		{
			get {return (string)GetValue(TextFontFaceProperty);}
			set {SetValue(TextFontFaceProperty, value);}
		}
		
		private static readonly DependencyProperty TextFontWeightProperty =
			DependencyProperty.Register("TextFontWeight", typeof(FontWeight), typeof(TextItem),
			                            new PropertyMetadata(FontWeights.Normal));
		[XmlIgnore]
		public FontWeight TextFontWeight
		{
			get {return (FontWeight)GetValue(TextFontWeightProperty);}
			set {SetValue(TextFontWeightProperty, value);}
		}
		
		public int FontWeightAsInt
		{
			get {return TextFontWeight.ToOpenTypeWeight();}
			set {TextFontWeight = FontWeight.FromOpenTypeWeight(value);}
		}
		
		private static readonly DependencyProperty IsItalicProperty =
			DependencyProperty.Register("IsItalic", typeof(bool), typeof(TextItem),
			                            new PropertyMetadata(false));
		public bool IsItalic
		{
			get {return (bool)GetValue(IsItalicProperty);}
			set {SetValue(IsItalicProperty, value);}
		}
		
		private static readonly DependencyProperty FontSizeProperty =
			DependencyProperty.Register("FontSize", typeof(double), typeof(TextItem),
			                            new PropertyMetadata(12.0));
		public double FontSize
		{
			get {return (double)GetValue(FontSizeProperty);}
			set {SetValue(FontSizeProperty, value);}
		}
		
		private static readonly DependencyProperty TextBrushProperty =
			DependencyProperty.Register("TextBrush", typeof(Brush), typeof(TextItem),
			                            new PropertyMetadata(Brushes.Black));
		public Brush TextBrush
		{
			get {return (Brush)GetValue(TextBrushProperty);}
			set {SetValue(TextBrushProperty, value);}
		}
		
		private static readonly DependencyPropertyKey TextPropertyKey =
			DependencyProperty.RegisterReadOnly("Text", typeof(TextParts), typeof(TextItem),
			                                    new PropertyMetadata(null));
		private static readonly DependencyProperty TextProperty =
			TextPropertyKey.DependencyProperty;
		public TextParts Text
		{
			get {return (TextParts)GetValue(TextProperty);}
			set {SetValue(TextProperty, value);}
		}
		
		private static readonly DependencyProperty CheckboxBeforeProperty =
			DependencyProperty.Register("CheckboxBefore", typeof(bool), typeof(TextItem),
			                            new PropertyMetadata(false));
		public bool CheckboxBefore
		{
			get {return (bool)GetValue(CheckboxBeforeProperty);}
			set {SetValue(CheckboxBeforeProperty, value);}
		}
		
		private static readonly DependencyProperty CheckboxAfterProperty =
			DependencyProperty.Register("CheckboxAfter", typeof(bool), typeof(TextItem),
			                            new PropertyMetadata(false));
		public bool CheckboxAfter
		{
			get {return (bool)GetValue(CheckboxAfterProperty);}
			set {SetValue(CheckboxAfterProperty, value);}
		}
		
		private static readonly DependencyProperty CheckboxLineThicknessProperty =
			DependencyProperty.Register("CheckboxLineThickness", typeof(double),
			                            typeof(TextItem), new PropertyMetadata(2.0));
		public double CheckboxLineThickness
		{
			get {return (double)GetValue(CheckboxLineThicknessProperty);}
			set {SetValue(CheckboxLineThicknessProperty, value);}
		}
		
		private static readonly DependencyProperty CheckBoxRelativeSizeProperty =
			DependencyProperty.Register("CheckBoxRelativeSize", typeof(double),
			                            typeof(TextItem), new PropertyMetadata(0.6));
		public double CheckBoxRelativeSize
		{
			get {return (double)GetValue(CheckBoxRelativeSizeProperty);}
			set {SetValue(CheckBoxRelativeSizeProperty, value);}
		}
				
		/// <summary>
		/// Default constructor
		/// </summary>
		public TextItem() : base(true)
		{
			SetValue(TextPropertyKey, new TextParts());
		}
		
		/// <summary>
		/// TextItem constructor
		/// </summary>
        /// <param name="isInDesignMode">Boolean to indicate if this image is to be displayed
        /// in design mode</param>
		public TextItem(bool isInDesignMode) : base(isInDesignMode)
		{
			SetValue(TextPropertyKey, new TextParts());
		}
		
		/// <summary>
		/// Sets DisplayRectangle to a size that contains all of the text plus space for checkboxes
		/// before and after the text
		/// </summary>
		public void CalculateRectangle()
		{
			if(Text != null && Text.Count > 0 && CardItemView != null)
			{
				FormattedText forText = ((TextItemView)CardItemView).FormattedTextItem;
				bool isModified = false;
				if(QslCard != null)		// various properties that result in CalculateRectangle being
										// called may be set before QslCard is set
				{
					isModified = QslCard.IsDirty;
				}
				if(DisplayX == 0 && DisplayY == 0 && DisplayWidth == 0 && DisplayHeight == 0)
				{
					DisplayX = (QslCard.DisplayWidth - forText.Width) / 2;
					DisplayY = (QslCard.DisplayHeight - forText.Height) / 2;
				}
				DisplayWidth = forText.Width + 2 * forText.Height + 6;
				DisplayHeight = forText.Height;
				((TextItemView)CardItemView).SetCheckBoxOffsets();
				
				if(QslCard != null)		// various properties that result in CalculateRectangle being
										// called may be set before QslCard is set
	
				{
					QslCard.IsDirty = isModified;
				}
			}
		}
		
		/// <summary>
		/// Handler for PropertyChanged event
		/// </summary>
		/// <param name="e">DependencyPropertyChangedEventArgs object. Used to determine
		/// which property changed</param>
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if(e.Property == TextFontFaceProperty ||
			   e.Property == TextFontWeightProperty ||
			   e.Property == IsItalicProperty ||
			   e.Property == FontSizeProperty ||
			   e.Property == TextBrushProperty ||
			   e.Property == CheckboxBeforeProperty ||
			   e.Property == CheckboxAfterProperty ||
			   e.Property == CheckboxLineThicknessProperty ||
			   e.Property == CheckBoxRelativeSizeProperty)
			{
				CalculateRectangle();
				if(QslCard != null)		// properties may be set before QslCard is set
				{
					QslCard.IsDirty = true;
				}
			}
			else if(e.Property == IsInDesignModeProperty &&
			       CardItemView != null && QslCard.QsosBox != null && QslCard.QsosBox.CardItemView != null)
			{
				((TextItemView)CardItemView).SetDisplayText(((QsosBoxView)QslCard.QsosBox.CardItemView).Qsos);
			}
			if((e.Property == FontSizeProperty ||
			   e.Property == CheckBoxRelativeSizeProperty) &&
			   CardItemView != null)
			{
				((TextItemView)CardItemView).SetCheckBoxSizeAndMargin();
				// no need to set QslCard.IsDirty because this is done above for these properties
			}
		}
		
		/// <summary>
		/// Load values for this TextItem from QslDnP card file contents
		/// </summary>
		/// <param name="itemNode">The TextItem node</param>
		/// <param name="culture">CultureInfo that the card was created in</param>
		public override void Load(XmlNode itemNode, CultureInfo culture)
		{
			XmlNode node = XmlProcs.GetFirstChildElement(itemNode);
			while(node != null)
			{
				XmlText text = XmlProcs.GetTextNode(node);
				switch(node.Name)
				{
					case "CardItem":
						base.Load(node, culture);
						break;
					case "FaceName":
						TextFontFace = text.Value;
						break;
					case "TextFontWeight":
						FontWeight weight = FontWeights.Normal;
						switch(text.Value)
						{
							case "Normal":
								break;
							case "Bold":
								weight = FontWeights.Bold;
								break;
							case "Black":
								weight = FontWeights.Black;
								break;
						}
						TextFontWeight = weight;
						break;
					case "IsItalic":
						if(text.Value == "true")
						{
							IsItalic = true;
						}
						else
						{
							IsItalic = false;
						}
						break;
					case "FontSize":
						double value = 0;
						if(!Double.TryParse(text.Value, NumberStyles.Float, culture, out value))
						{
							if(!Double.TryParse(text.Value, NumberStyles.Float,
							                    CultureInfo.InvariantCulture, out value))
							{
								throw new XmlException("Bad TextItem property value");
							}
						}
						FontSize = value;
						break;
					case "TextBrush":
						TextBrush = XmlProcs.ConvertXmlToBrush(XmlProcs.GetFirstChildElement(node), culture);
						break;
					case "Text":
						StaticText sText = new StaticText();
						sText.Text = text.Value;
						Text.Add(sText);
						break;
					case "CheckboxBefore":
						if(text.Value == "true")
						{
							CheckboxBefore = true;
						}
						else
						{
							CheckboxBefore = false;
						}
						CheckBoxRelativeSize = 1.0;
						break;
					case "CheckboxAfter":
						if(text.Value == "true")
						{
							CheckboxAfter = true;
						}
						else
						{
							CheckboxAfter = false;
						}
						CheckBoxRelativeSize = 1.0;
						break;
				}
				node = XmlProcs.GetNextSiblingElement(node);
			}
		}
		
		/// <summary>
		/// Deep copy properties from one TextItem object to this one
		/// </summary>
		/// <param name="card">Card object that this TextItem belongs to</param>
		/// <param name="ti">TextItem object whose properties are to be copied</param>
		public void CopyTextItem(Card card, TextItem ti)
		{
			CopyBaseProperties(card, ti);
			TextFontFace = ti.TextFontFace;
//			TextFontWeight = ti.TextFontWeight;
			IsItalic = ti.IsItalic;
			FontSize = ti.FontSize;
			TextBrush = ti.TextBrush.Clone();
			foreach(TextPart part in ti.Text)
			{
				Text.Add(part.Clone());
			}
			CheckboxBefore = ti.CheckboxBefore;
			CheckboxAfter = ti.CheckboxAfter;
			CheckboxLineThickness = ti.CheckboxLineThickness;
			CheckBoxRelativeSize = ti.CheckBoxRelativeSize;
		}
	}
}
