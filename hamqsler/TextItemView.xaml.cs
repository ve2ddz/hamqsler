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
using System.Globalization;
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
	/// View for TextItems
	/// </summary>
	public partial class TextItemView : CardItemView
	{
		private static readonly DependencyProperty DisplayTextProperty =
			DependencyProperty.Register("DisplayText", typeof(string), typeof(TextItemView),
			                            new PropertyMetadata(string.Empty));
		public string DisplayText
		{
			get {return (string)GetValue(DisplayTextProperty);}
			set {SetValue(DisplayTextProperty, value);}
		}
		
		private static readonly DependencyProperty CheckBoxSizeProperty =
			DependencyProperty.Register("CheckBoxSize", typeof(double), typeof(TextItemView),
			                            new PropertyMetadata(0.0));
		public double CheckBoxSize
		{
			get {return (double)GetValue(CheckBoxSizeProperty);}
			set {SetValue(CheckBoxSizeProperty, value);}
		}
		
		private static readonly DependencyProperty CheckBoxMarginProperty =
			DependencyProperty.Register("CheckBoxMargin", typeof(Thickness), typeof(TextItemView),
			                            new PropertyMetadata(new Thickness(0)));
		public Thickness CheckBoxMargin
		{
			get {return (Thickness)GetValue(CheckBoxMarginProperty);}
			set {SetValue(CheckBoxMarginProperty, value);}
		}
		
		public FormattedText FormattedTextItem
		{
			get
			{
				TextItem ti = ItemData as TextItem;
				FontStyle style = (ti.IsItalic==true) ? FontStyles.Italic : FontStyles.Normal;
				Typeface typeface = new Typeface(new FontFamily(ti.TextFontFace), style, 
				                                 ti.TextFontWeight, FontStretches.Normal);
					return new FormattedText(DisplayText, CultureInfo.CurrentUICulture,
					                         FlowDirection.LeftToRight, typeface, ti.FontSize, 
					                         ti.TextBrush);
			}
		}
		

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="ti">TextItem that this view displays</param>
		public TextItemView(TextItem ti) : base(ti)
		{
			DataContext = this;
			ti.TItemView = this;
			InitializeComponent();
		}
		
		/// <summary>
		/// Helper method to handle MouseMove events when the related TextItem is selected
		/// and the left mouse button is down
		/// </summary>
		/// <param name="view">CardView object that contains this TextItemView</param>
		/// <param name="e">MouseEventArgs object</param>
		protected override void HandleMouseMoveWithLeftMouseButtonDown(CardView view, MouseEventArgs e)
		{
			// with left mouse button down, cursor shows where to move the text item
			if (cursorLoc == CursorLocation.Inside)
			{
				Point pt = e.GetPosition(view);
				ItemData.DisplayX = originalDisplayRectangle.X + pt.X - leftMouseDownPoint.X;
				ItemData.DisplayY = originalDisplayRectangle.Y + pt.Y - leftMouseDownPoint.Y;
			}
		}
		
		/// <summary>
		/// Helper method to handle MouseMove events when the related TextItem is selected
		/// but the left mouse button is not down.
		/// </summary>
		/// <param name="e">MouseEventArgs object</param>
		protected override void HandleMouseMoveWithLeftMouseButtonUp(MouseEventArgs e)
		{
			Cursor cursor = Cursors.Arrow;
			cursorLoc = CursorLocation.Outside;
			Point pt = e.GetPosition(this);
			if (new Rect(0, 0, GetWidth(), GetHeight()).Contains(pt))
			{
				cursorLoc = CursorLocation.Inside;
				cursor = Cursors.SizeAll;
			}
			Mouse.OverrideCursor = cursor;
		}
		
		/// <summary>
		/// Retrieve the actual width of this ImageView
		/// </summary>
		/// <returns>Width of this ImageView in device independent units</returns>
		protected override double GetWidth()
		{
			return SelectRectangle.ActualWidth;
		}
		
		/// <summary>
		/// Retrieve the actual height of this ImageView
		/// </summary>
		/// <returns>Height of this ImageView in device independent units</returns>
		protected override double GetHeight()
		{
			return SelectRectangle.ActualHeight;
		}
		
		/// <summary>
		/// Set DisplayText value
		/// </summary>
		public void SetDisplayText()
		{
			DisplayText = ((TextItem)ItemData).Text.GetText(ItemData.IsInDesignMode);
		}
		
		/// <summary>
		/// Handler for DependencyPropertyChanged event
		/// </summary>
		/// <param name="e">DependencyPropertyChangedEventArgs object</param>
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if(e.Property == DisplayTextProperty)
			{
				SetCheckBoxSizeAndMargin();
				TextItem ti = (TextItem)ItemData;
				ti.CalculateRectangle();
			}
		}
		
		/// <summary>
		/// Calculate CheckBox size and CheckBox margin properties
		/// </summary>
		public void SetCheckBoxSizeAndMargin()
		{
				TextItem ti = (TextItem)ItemData;
				// must use FormattedTextItem.Height here, not ti.DisplayHeight
				CheckBoxSize = FormattedTextItem.Height * ti.CheckBoxRelativeSize;
				double margin = (FormattedTextItem.Height - CheckBoxSize) / 2 + 2;
				CheckBoxMargin = new Thickness(margin, 0, margin, 0);
			
		}
	}
}