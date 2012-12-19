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
	/// View for TextItems
	/// </summary>
	public partial class TextItemView : CardItemView
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="ti">TextItem that this view displays</param>
		public TextItemView(TextItem ti) : base(ti)
		{
			DataContext = this;
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
	}
}