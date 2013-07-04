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
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace hamqsler
{
	/// <summary>
	/// Description of TextWFItem.
	/// </summary>
	[Serializable]
	public class TextWFItem : CardWFItem
	{
		private static readonly DependencyProperty TextFontFaceProperty =
			DependencyProperty.Register("TextFontFace", typeof(string), typeof(TextWFItem),
			                            new PropertyMetadata("Arial"));
		public string TextFontFace
		{
			get {return (string)GetValue(TextFontFaceProperty);}
			set {SetValue(TextFontFaceProperty, value);}
		}
		
		private static readonly DependencyProperty IsBoldProperty =
			DependencyProperty.Register("IsBold", typeof(bool), typeof(TextWFItem),
			                            new PropertyMetadata(false));
		public bool IsBold
		{
			get {return (bool)GetValue(IsBoldProperty);}
			set {SetValue(IsBoldProperty, value);}
		}
		
		private static readonly DependencyProperty IsItalicProperty =
			DependencyProperty.Register("IsItalic", typeof(bool), typeof(TextWFItem),
			                            new PropertyMetadata(false));
		public bool IsItalic
		{
			get {return (bool)GetValue(IsItalicProperty);}
			set {SetValue(IsItalicProperty, value);}
		}
		
		private static readonly DependencyProperty FontSizeProperty =
			DependencyProperty.Register("FontSize", typeof(float), typeof(TextWFItem),
			                            new PropertyMetadata(12.0F));
		public float FontSize
		{
			get {return (float)GetValue(FontSizeProperty);}
			set {SetValue(FontSizeProperty, value);}
		}
		
		private static readonly DependencyProperty TextColorProperty =
			DependencyProperty.Register("TextColor", typeof(Color), typeof(TextWFItem),
			                            new PropertyMetadata(Color.Black));
		[XmlIgnore]
		public Color TextColor
		{
			get {return (Color)GetValue(TextColorProperty);}
			set {SetValue(TextColorProperty, value);}
		}
		
		// the following 3 accessors are required for saving TextWFItem in XML
		public int TextColorRed
		{
			get {return TextColor.R;}
			set {TextColor = Color.FromArgb(255, value, TextColor.G, TextColor.B);}
		}
		
		public int TextColorGreen
		{
			get {return TextColor.G;}
			set {TextColor = Color.FromArgb(255, TextColor.R, value, TextColor.B);}
		}
		
		public int TextColorBlue
		{
			get {return TextColor.B;}
			set {TextColor = Color.FromArgb(255, TextColor.R, TextColor.G, value);}
		}
		
		[XmlIgnore]
		public Brush TextBrush
		{
			get {return new SolidBrush(TextColor);}
		}
		
		private static readonly DependencyPropertyKey TextPropertyKey =
			DependencyProperty.RegisterReadOnly("Text", typeof(TextParts), typeof(TextWFItem),
			                                    new PropertyMetadata(null));
		private static readonly DependencyProperty TextProperty =
			TextPropertyKey.DependencyProperty;
		public TextParts Text
		{
			get {return (TextParts)GetValue(TextProperty);}
			set {SetValue(TextProperty, value);}
		}
		
		private static readonly DependencyProperty CheckboxBeforeProperty =
			DependencyProperty.Register("CheckboxBefore", typeof(bool), typeof(TextWFItem),
			                            new PropertyMetadata(false));
		public bool CheckboxBefore
		{
			get {return (bool)GetValue(CheckboxBeforeProperty);}
			set {SetValue(CheckboxBeforeProperty, value);}
		}
		
		private static readonly DependencyProperty CheckboxAfterProperty =
			DependencyProperty.Register("CheckboxAfter", typeof(bool), typeof(TextWFItem),
			                            new PropertyMetadata(false));
		public bool CheckboxAfter
		{
			get {return (bool)GetValue(CheckboxAfterProperty);}
			set {SetValue(CheckboxAfterProperty, value);}
		}
		
		private static readonly DependencyProperty CheckboxLineThicknessProperty =
			DependencyProperty.Register("CheckboxLineThickness", typeof(float),
			                            typeof(TextWFItem), new PropertyMetadata(2.0F));
		public float CheckboxLineThickness
		{
			get {return (float)GetValue(CheckboxLineThicknessProperty);}
			set {SetValue(CheckboxLineThicknessProperty, value);}
		}
		
		private static readonly DependencyProperty CheckBoxRelativeSizeProperty =
			DependencyProperty.Register("CheckBoxRelativeSize", typeof(float),
			                            typeof(TextWFItem), new PropertyMetadata(0.6F));
		public float CheckBoxRelativeSize
		{
			get {return (float)GetValue(CheckBoxRelativeSizeProperty);}
			set {SetValue(CheckBoxRelativeSizeProperty, value);}
		}
				
		/// <summary>
		/// Default constructor
		/// </summary>
		public TextWFItem()
		{
			SetValue(TextPropertyKey, new TextParts());
		}
		
		/// <summary>
		/// Determine the relative location of the input coordinates within the text item 
		/// </summary>
		/// <param name="x">Card relative X coordinate</param>
		/// <param name="y">Card relative Y coordinate</param>
		/// <returns>Relative location</returns>
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
		/// Calculate the size of this TextWFItem based on its various properties
		/// </summary>
		public void CalculateRectangle()
		{
			if(QslCard != null)
			{
				System.Drawing.FontStyle style = System.Drawing.FontStyle.Regular;
				if(this.IsBold)
				{
					style = System.Drawing.FontStyle.Bold;
				}
				if(this.IsItalic)
				{
					style |= System.Drawing.FontStyle.Italic;
				}
				Font font = new Font(new FontFamily(this.TextFontFace), this.FontSize,
					         style, GraphicsUnit.Point);
				System.Drawing.Size size = TextRenderer.MeasureText(this.Text.GetText(
					QslCard, null, QslCard.IsInDesignMode), font);
				this.Height = size.Height;
				this.Width = size.Width + size.Height * 2 + 8;
			}
		}
		
		/// <summary>
		/// Handler for PropertyChanged event
		/// </summary>
		/// <param name="e">DependencyPropertyChangedEventArgs object</param>
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if(e.Property == TextFontFaceProperty ||
			   e.Property == IsBoldProperty ||
			   e.Property == IsItalicProperty ||
			   e.Property == FontSizeProperty ||
			   e.Property == TextProperty)
			{
				CalculateRectangle();
			}
			if(e.Property == TextColorProperty ||
	           e.Property == CheckboxBeforeProperty ||
	     	   e.Property == CheckboxAfterProperty ||
	     	   e.Property == CheckboxLineThicknessProperty ||
	     	   e.Property == CheckBoxRelativeSizeProperty ||
	     	   e.Property == IsItalicProperty ||
	     	   e.Property == IsBoldProperty ||
	     	   e.Property == TextProperty ||
	     	   e.Property == TextFontFaceProperty)
			{
				if(QslCard != null)
				{
					QslCard.IsDirty = true;
					QslCard.RaiseDispPropertyChangedEvent();
				}
			}
		}
		
		/// <summary>
		/// Create a TextWFItem object that is a deep copy of this one
		/// </summary>
		/// <returns>TextWFItem object that is a deep copy of this one</returns>
		public TextWFItem Clone()
		{
			TextWFItem tItem = new TextWFItem();
			tItem.CopyBaseProperties(this);
			tItem.TextFontFace = this.TextFontFace;
			tItem.IsBold = this.IsBold;
			tItem.IsItalic = this.IsItalic;
			tItem.FontSize = this.FontSize;
			tItem.TextColor = this.TextColor;
			tItem.Text.Clear();
			foreach(TextPart part in this.Text)
			{
				tItem.Text.Add(part.Clone());
			}
			tItem.CheckboxBefore = this.CheckboxBefore;
			tItem.CheckboxAfter = this.CheckboxAfter;
			tItem.CheckboxLineThickness = this.CheckboxLineThickness;
			tItem.CheckBoxRelativeSize = this.CheckBoxRelativeSize;
			return tItem;
		}
				
	}
}
