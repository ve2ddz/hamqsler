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
	/// Interaction logic for ImageView.xaml
	/// </summary>
	public partial class ImageView : CardItemView
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="image">CardImageBase that is the base class for the image to be displayed</param>
		public ImageView(CardImageBase image) : base(image)
		{
			DataContext	= this;
			image.CardItemView = this;
			InitializeComponent();
		}
				
		/// <summary>
		/// Helper method to handle MouseMove events when the related Image is selected
		/// and the left mouse button is down
		/// </summary>
		/// <param name="view">CardView object that contains this ImageView</param>
		/// <param name="e">MouseEventArgs object</param>

		protected override void HandleMouseMoveWithLeftMouseButtonDown(CardView view, MouseEventArgs e)
		{
			// handle drag actions
            double x = 0;
            double y= 0;
            double width= 0;
            double height = 0;
            CardImageBase cib = ItemData as CardImageBase;
            double imageAspectRatio = (cib.BitMapImage != CardImageBase.EmptyImage) ? 
            	cib.BitMapImage.Width / cib.BitMapImage.Height : 1.0;
            double minWidth = imageAspectRatio < 1.0 ? 9.6 : 9.6 / imageAspectRatio;
            double minHeight = imageAspectRatio >= 1.0 ? 9.6 : 9.6 / imageAspectRatio;
            Point pt = e.GetPosition(view);
            switch (cursorLoc)
            {
                case CursorLocation.Inside:
                    x = originalDisplayRectangle.X + pt.X - leftMouseDownPoint.X;
                    y = originalDisplayRectangle.Y + pt.Y - leftMouseDownPoint.Y;
                    width = originalDisplayRectangle.Width;
                    height = originalDisplayRectangle.Height; 
                    break;
                case CursorLocation.NW:
                    width = originalDisplayRectangle.Width -
                        (pt.X - leftMouseDownPoint.X);
                    width = width < minWidth ? minWidth : width;
                    height = originalDisplayRectangle.Height - 
                        (pt.Y - leftMouseDownPoint.Y);
                    height = height < minHeight ? minHeight : height;
                    if (imageAspectRatio < width / height)
                    {
                        width = height * imageAspectRatio;
                    }
                    else if (imageAspectRatio > width / height)
                    {
                        height = width / imageAspectRatio;
                    }
                    x = originalDisplayRectangle.X + originalDisplayRectangle.Width
                        - width;
                    y = originalDisplayRectangle.Y + originalDisplayRectangle.Height
                        - height;
                    break;
                case CursorLocation.NE:
                    width = originalDisplayRectangle.Width + 
                        (pt.X - leftMouseDownPoint.X);
                    width = width < minWidth ? minWidth : width;
                    height = originalDisplayRectangle.Height - 
                        (pt.Y - leftMouseDownPoint.Y);
                    height = height < minHeight ? minHeight : height;
                    if (imageAspectRatio < width / height)
                    {
                        width = height * imageAspectRatio;
                    }
                    else if (imageAspectRatio > width / height)
                    {
                        height = width / imageAspectRatio;
                    }

                    x = originalDisplayRectangle.X;
                    y = originalDisplayRectangle.Y + originalDisplayRectangle.Height
                        - height;
                    break;
                case CursorLocation.SE:
                    width = originalDisplayRectangle.Width +
                        (pt.X - leftMouseDownPoint.X);
                    width = width < minWidth ? minWidth : width;
                    height = originalDisplayRectangle.Height +
                        (pt.Y - leftMouseDownPoint.Y);
                    height = height < minHeight ? minHeight : height;
                    if (imageAspectRatio < width / height)
                    {
                        width = height * imageAspectRatio;
                    }
                    else if (imageAspectRatio > width / height)
                    {
                        height = width / imageAspectRatio;
                    }
                    x = originalDisplayRectangle.X;
                    y = originalDisplayRectangle.Y;
                    break;
                case CursorLocation.SW:
                    width = originalDisplayRectangle.Width -
                        (pt.X - leftMouseDownPoint.X);
                    width = width < minWidth ? minWidth : width;
                    height = originalDisplayRectangle.Height +
                        (pt.Y - leftMouseDownPoint.Y);
                    height = height < minHeight ? minHeight : height;
                    if (imageAspectRatio < width / height)
                    {
                        width = height * imageAspectRatio;
                    }
                    else if (imageAspectRatio > width / height)
                    {
                        height = width / imageAspectRatio;
                    }

                    x = originalDisplayRectangle.X + originalDisplayRectangle.Width
                        - width; ;
                    y = originalDisplayRectangle.Y;
                    break;
            }
            if (cursorLoc != CursorLocation.Outside)
            {
            	ItemData.DisplayX = x;
            	ItemData.DisplayY = y;
            	ItemData.DisplayWidth = width;
            	ItemData.DisplayHeight = height;
            }
		}
		
		/// <summary>
		/// Helper method to handle MouseMove events when the related Image is selected
		/// but the left mouse button is not down.
		/// </summary>
		/// <param name="e">MouseEventArgs object</param>
		protected override void HandleMouseMoveWithLeftMouseButtonUp(MouseEventArgs e)
		{
			// not dragging so set the mouse cursor based on where cursor is relative
			// to this image
            cursorLoc = CursorLocation.Outside;
            Point pt = e.GetPosition(this);
            Rect nw = new Rect(-cornerSize,- cornerSize,
                               2 * cornerSize, 2 * cornerSize);
            Rect ne = new Rect(ItemData.DisplayWidth - cornerSize, -cornerSize,
                               2 * cornerSize, 2 * cornerSize);
            Rect se = new Rect(ItemData.DisplayWidth - cornerSize, 
                               ItemData.DisplayHeight - cornerSize,
                               2 * cornerSize, 2 * cornerSize);
            Rect sw = new Rect(-cornerSize, ItemData.DisplayHeight - cornerSize,
                               2 * cornerSize, 2 * cornerSize);
            Cursor cursor = Cursors.Arrow;
            if (nw.Contains(pt))
            {
                cursorLoc = CursorLocation.NW;
                cursor = Cursors.SizeNWSE;
            }
            else if (ne.Contains(pt))
            {
                cursorLoc = CursorLocation.NE;
                cursor = Cursors.SizeNESW;
            }
            else if (se.Contains(pt))
            {
                cursorLoc = CursorLocation.SE;
                cursor = Cursors.SizeNWSE;
            }
            else if (sw.Contains(pt))
            {
                cursorLoc = CursorLocation.SW;
                cursor = Cursors.SizeNESW;
            }
            else if (new Rect(0, 0, GetWidth(), GetHeight()).Contains(pt))
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