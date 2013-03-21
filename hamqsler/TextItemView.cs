﻿/*
 *  Author:
 *       Jim Orcheson <jimorcheson@gmail.com>
 * 
 *  Copyright (c) 2012, 2013 Jim Orcheson
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
		
		private static readonly DependencyProperty CheckBoxBeforeLeftOffsetProperty = 
			DependencyProperty.Register("CheckBoxBeforeLeftOffset", typeof(double), typeof(TextItemView),
			                            new PropertyMetadata(0.0));
		public double CheckBoxBeforeLeftOffset
		{
			get {return (double)GetValue(CheckBoxBeforeLeftOffsetProperty);}
			set {SetValue(CheckBoxBeforeLeftOffsetProperty, value);}
		}
		
		private static readonly DependencyProperty CheckBoxAfterRightOffsetProperty =
			DependencyProperty.Register("CheckBoxAfterRightOffset", typeof(double), typeof(TextItemView),
			                            new PropertyMetadata(0.0));
		public double CheckBoxAfterRightOffset
		{
			get {return (double)GetValue(CheckBoxAfterRightOffsetProperty);}
			set {SetValue(CheckBoxAfterRightOffsetProperty, value);}
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
			ti.CardItemView = this;
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
		protected override void HandleMouseMoveWithLeftMouseButtonUp(CardView view, MouseEventArgs e)
		{
			Cursor cursor = Cursors.Arrow;
			cursorLoc = CursorLocation.Outside;
			Point pt = e.GetPosition(view);
			if (new Rect(ItemData.DisplayX, ItemData.DisplayY, 
			             ItemData.DisplayWidth, ItemData.DisplayHeight).Contains(pt))
			{
				cursorLoc = CursorLocation.Inside;
				cursor = Cursors.SizeAll;
			}
			Mouse.OverrideCursor = cursor;
		}
		
		/// <summary>
		/// Set DisplayText value
		/// </summary>
		public void SetDisplayText(List<DispQso> qsos)
		{
			DisplayText = ((TextItem)ItemData).Text.GetText(ItemData.QslCard, qsos,
		                                                	ItemData.IsInDesignMode);
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
			if(ItemData != null && ItemData.QslCard != null && ItemData.QslCard.CardItemView != null)
			{
				ItemData.QslCard.CardItemView.InvalidateVisual();
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
		
		/// <summary>
		/// Calculate the CheckBox offset properties
		/// </summary>
		public void SetCheckBoxOffsets()
		{
			TextItem ti = (TextItem)ItemData;
			CheckBoxBeforeLeftOffset = ti.DisplayX + ti.DisplayHeight *
				                          (1 - ti.CheckBoxRelativeSize) / 2;
			CheckBoxAfterRightOffset = ti.DisplayX + ti.DisplayWidth -
										   (1 - ti.CheckBoxRelativeSize) / 2;
		}
		
		/// <summary>
		/// Render the TextItem on the card
		/// </summary>
		/// <param name="drawingContext">CrawingContext on which to render the text</param>
		protected override void OnRender(DrawingContext drawingContext)
		{
			TextItem tItem = ItemData as TextItem;
			CultureInfo culture = CultureInfo.CurrentCulture;
			FontStyle style = (tItem.IsItalic==true) ? FontStyles.Italic : FontStyles.Normal;
			FontWeight weight = tItem.TextFontWeight;
			FontFamily family = new FontFamily(tItem.TextFontFace);
			Typeface typeface = new Typeface(family, style, weight, FontStretches.Normal);
			FormattedText fText = new FormattedText(DisplayText, culture, FlowDirection.LeftToRight,
			                                        typeface, tItem.FontSize, tItem.TextBrush);
			drawingContext.DrawText(fText, new Point(tItem.DisplayX + 
			                                         CheckBoxSize + 2.0 * CheckBoxMargin.Left,
			                                         tItem.DisplayY));
			if(tItem.CheckboxBefore)
			{
				drawingContext.DrawRectangle(Brushes.Transparent, 
				                             new Pen(tItem.TextBrush, tItem.CheckboxLineThickness),
				                             new Rect(tItem.DisplayX + CheckBoxMargin.Left,
				                                      tItem.DisplayY + (fText.Height - CheckBoxSize) / 2,
				                                      CheckBoxSize, CheckBoxSize));
			}
			if(tItem.CheckboxAfter)
			{
				double boxX = tItem.DisplayX + tItem.DisplayWidth - tItem.CheckBoxRelativeSize *
					fText.Height - CheckBoxMargin.Right;
				drawingContext.DrawRectangle(Brushes.Transparent,
				                             new Pen(tItem.TextBrush, tItem.CheckboxLineThickness),
				                                     new Rect(boxX, 
				                                      tItem.DisplayY + (fText.Height - CheckBoxSize) / 2,
				                                      CheckBoxSize, CheckBoxSize));
				                                              
			}
			base.OnRender(drawingContext);
		}
	}
}