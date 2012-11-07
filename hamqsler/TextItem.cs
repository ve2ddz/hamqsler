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
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace hamqsler
{
	/// <summary>
	/// TextItem class - displays text on a QSL card
	/// </summary>
	public class TextItem : CardItem
	{
		private static readonly DependencyProperty TextFontFaceProperty =
			DependencyProperty.Register("TextFontFace", typeof(FontFamily), typeof(TextItem),
			                            new PropertyMetadata(new FontFamily("Arial")));
		public FontFamily TextFontFace
		{
			get {return (FontFamily)GetValue(TextFontFaceProperty);}
			set {SetValue(TextFontFaceProperty, value);}
		}
		
		private static readonly DependencyProperty TextFontWeightProperty =
			DependencyProperty.Register("TextFontWeight", typeof(FontWeight), typeof(TextItem),
			                            new PropertyMetadata(FontWeights.Normal));
		public FontWeight TextFontWeight
		{
			get {return (FontWeight)GetValue(TextFontWeightProperty);}
			set {SetValue(TextFontWeightProperty, value);}
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
		
		private static readonly DependencyProperty TextProperty =
			DependencyProperty.Register("Text", typeof(string), typeof(TextItem),
			                            new PropertyMetadata("Text Item"));
		public string Text
		{
			get {return (string)GetValue(TextProperty);}
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
		
		public FormattedText FormattedTextItem
		{
			get
			{
				FontStyle style = (IsItalic==true) ? FontStyles.Italic : FontStyles.Normal;
				Typeface typeface = new Typeface(TextFontFace, style, TextFontWeight,
				                                 FontStretches.Normal);
				return new FormattedText(Text, CultureInfo.CurrentUICulture,
				                         FlowDirection.LeftToRight, typeface, FontSize, TextBrush);
			}
		}
		
			                                                 
		public TextItem() {}
		
		/// <summary>
		/// Sets DisplayRectangle to a size that contains all of the text plus space for checkboxes
		/// before and after the text
		/// </summary>
		protected void CalculateRectangle()
		{
			FormattedText forText = FormattedTextItem;
			bool isModified = false;
			if(QslCard != null)		// various properties that result in CalculateRectangle being
									// called may be set before QslCard is set
			{
				isModified = QslCard.IsDirty;
			}
			DisplayRectangle = new Rect(DisplayRectangle.X, DisplayRectangle.Y,
			                            forText.Width + 2 * forText.Height + 6,
			                            forText.Height);
			if(QslCard != null)		// various properties that result in CalculateRectangle being
									// called may be set before QslCard is set

			{
				QslCard.IsDirty = isModified;
			}
		}
		
		/// <summary>
		/// OnRender event handler - draws the image
		/// </summary>
		/// <param name="dc">Context to draw the card on</param>
		protected override void OnRender(DrawingContext dc)
		{
			base.OnRender(dc);
			CalculateRectangle();
			if(IsSelected)
			{
				dc.DrawRectangle(Brushes.Transparent, selectPen, DisplayRectangle);
				DrawCheckboxesAndText(dc);
			}
			else if(IsHighlighted)
			{
				dc.DrawRectangle(Brushes.Transparent, hightlightPen, DisplayRectangle);
				DrawCheckboxesAndText(dc);
			}
			else
			{
				// if not selected or highlighted, clip the textitem to fit on the card.
				dc.PushClip(new RectangleGeometry(QslCard.DisplayRectangle));
				DrawCheckboxesAndText(dc);
				dc.Pop();
			}
		}
			
		/// <summary>
		/// Helper method that draws the before and after checkboxes and the text
		/// </summary>
		/// <param name="dc">DrawingContext to draw on</param>
		private void DrawCheckboxesAndText(DrawingContext dc)
		{
			if(CheckboxBefore == true)
			{
				dc.DrawRectangle(Brushes.Transparent, new Pen(TextBrush, 1.5),
				                 new Rect(DisplayRectangle.X + FormattedTextItem.Height * 0.2, 
				                          DisplayRectangle.Y + FormattedTextItem.Height * 0.2,
				                          FormattedTextItem.Height * 0.6, 
				                          FormattedTextItem.Height * 0.6));
			}
			if(CheckboxAfter == true)
			{
				dc.DrawRectangle(Brushes.Transparent, new Pen(TextBrush, 1.5),
				                 new Rect(DisplayRectangle.X + DisplayRectangle.Width -
				                          FormattedTextItem.Height * 0.8,
				                          DisplayRectangle.Y + FormattedTextItem.Height * 0.2,
				                          FormattedTextItem.Height * 0.6, 
				                          FormattedTextItem.Height * 0.6));
			}
			dc.DrawText(FormattedTextItem, new Point(DisplayRectangle.X + FormattedTextItem.Height + 3,
			                                         DisplayRectangle.Y));
		}
		
		/// <summary>
		/// Handler for PropertyChanged event
		/// </summary>
		/// <param name="e">DependencyPropertyChangedEventArgs object. Used to determine
		/// which property changed</param>
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if(e.Property == TextProperty ||
			   e.Property == TextFontFaceProperty ||
			   e.Property == FontSizeProperty ||
			   e.Property == IsItalicProperty ||
			   e.Property == TextFontWeightProperty ||
			   e.Property == CheckboxBeforeProperty ||
			   e.Property == CheckboxAfterProperty)
			{
				CalculateRectangle();
				if(QslCard != null)		// properties may be set before QslCard is set
				{
					QslCard.IsDirty = true;
				}
			}
		}
	}
}
