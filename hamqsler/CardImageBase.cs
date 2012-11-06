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
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace hamqsler
{
	/// <summary>
	/// Description of CardImageBase.
	/// </summary>
	[Serializable]
	abstract public class CardImageBase : CardItem
	{
		private static readonly DependencyProperty ImageFileNameProperty =
			DependencyProperty.Register("ImageFileName", typeof(string), typeof(CardImageBase),
			                            new PropertyMetadata(string.Empty));
		public string ImageFileName
		{
			get {return (string)GetValue(ImageFileNameProperty);}
			set {SetValue(ImageFileNameProperty, value);}
		}
		
		// overrides standard CardItem.DisplayRectangle accessor to account for the case
		// where there is no image loaded
        public override Rect DisplayRectangle
        {
        	get 
        	{
        		Rect r = (Rect)GetValue(DisplayRectangleProperty);
        		if(r.Width == 0 && r.Height == 0)
        			r = CalculateRectangle();
        		return r; 
        	}
            set { SetValue(DisplayRectangleProperty, value); }
        }
        
        /// <summary>
        /// CalculateRectangle determines the DisplayRectangle size for the image
        /// </summary>
        /// <returns>DisplayRectangle for this image.</returns>
        protected abstract Rect CalculateRectangle();

		[NonSerialized]
		protected BitmapImage bImage = null;
		
		/// <summary>
		/// Default constructor
		/// </summary>
		public CardImageBase()
		{
		}
		
		/// <summary>
		/// Handle MouseMove events for this image. This is called from CardTabItem.OnMouseMove.
		/// </summary>
		/// <param name="e">MouseEventArgs object</param>
		public override void MoveMouse(System.Windows.Input.MouseEventArgs e)
		{
			base.MoveMouse(e);
			if(this.IsSelected && bImage != null)
			{
				if(this.IsLeftMouseButtonDown)	
				{
					// handle drag actions
                    double x = 0;
                    double y= 0;
                    double width= 0;
                    double height = 0;
                    double imageAspectRatio = (bImage != null) ? bImage.Width / bImage.Height : 1.0;
                    double minWidth = imageAspectRatio < 1.0 ? 9.6 : 9.6 / imageAspectRatio;
                    double minHeight = imageAspectRatio >= 1.0 ? 9.6 : 9.6 / imageAspectRatio;
                    Point pt = e.GetPosition(QslCard);
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
                        DisplayRectangle = new Rect(x, y, width, height);
                        InvalidateVisual();
                    }
					
				}
				else
				{
					// not dragging so set the mouse cursor based on where cursor is relative
					// to this image
                    cursorLoc = CursorLocation.Outside;
                    Point pt = e.GetPosition(QslCard);
                    Rect nw = new Rect(DisplayRectangle.X - cornerSize,
                            DisplayRectangle.Y - cornerSize,
                            2 * cornerSize, 2 * cornerSize);
                    double x = DisplayRectangle.X + DisplayRectangle.Width;
                    double y = DisplayRectangle.Y;
                    Rect ne = new Rect(x - cornerSize, y - cornerSize,
                            2 * cornerSize, 2 * cornerSize);
                    y += DisplayRectangle.Height;
                    Rect se = new Rect(x - cornerSize, y - cornerSize,
                            2 * cornerSize, 2 * cornerSize);
                    x -= DisplayRectangle.Width;
                    Rect sw = new Rect(x - cornerSize, y - cornerSize,
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
                    else if (DisplayRectangle.Contains(pt))
                    {
                        cursorLoc = CursorLocation.Inside;
                        cursor = Cursors.SizeAll;
                    }

                    Mouse.OverrideCursor = cursor;
				}
			}
 		}
		
		/// <summary>
		/// OnRender event handler - draws the image
		/// </summary>
		/// <param name="dc">Context to draw the card on</param>
		protected override void OnRender(DrawingContext dc)
		{
			Rect rect = DisplayRectangle;
			if(this.GetType() == typeof(SecondaryImage))
		    {
				System.Diagnostics.Debug.WriteLine("{0}, {1} : {2}, {3}", 
				                                   rect.X, rect.Y, rect.Width, rect.Height);
		    }
			Brush brush = Brushes.Transparent;
			if(ImageFileName == string.Empty || ImageFileName == null)
			{
				dc.DrawRectangle(brush, new Pen(brush, 0), rect);
			}
			else
			{
				rect = DisplayRectangle;
				dc.PushClip(new RectangleGeometry(QslCard.DisplayRectangle));
				dc.DrawImage(bImage, rect);
				dc.Pop();
			}
			if(IsSelected)
			{
				if(bImage != null)
				{
					dc.PushOpacity(0.4);
					dc.DrawImage(bImage, DisplayRectangle);
					dc.Pop();
				}
				dc.DrawRectangle(brush, selectPen, rect);
			}
			else if(IsHighlighted)
			{
				if(bImage != null)
				{
					dc.PushOpacity(0.4);
					dc.DrawImage(bImage, DisplayRectangle);
					dc.Pop();					
				}
				dc.DrawRectangle(brush, hightlightPen, rect);
			}
		}

		/// <summary>
		/// Handles PropertyChanged event
		/// </summary>
		/// <param name="e">DependencyProperChangedEventArgs object</param>
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if(e.Property == ImageFileNameProperty)
			{
				// get the image file name
				string fName = (string)e.NewValue;
				if(fName != null && fName != string.Empty)
				{
					string hamqslerFolder = ((App)Application.Current).HamqslerFolder;
					// expand file name if using relative path
					if(fName.StartsWith("$hamqslerFolder$\\"))
					{
						fName = hamqslerFolder + fName.Substring("$hamqslerFolder$\\".Length);
					}
					// load and create the image
					bImage = new BitmapImage();
					bImage.BeginInit();
					bImage.UriSource = new Uri(fName, UriKind.RelativeOrAbsolute);
					bImage.EndInit();
					DisplayRectangle = CalculateRectangle();
				}
				else
				{
					// reset bImage
					bImage = null;
					this.DisplayRectangle = new Rect(0, 0, 0, 0);
				}
				// Image has changed, so let QslCard know it has changed
				// and redisplay card
				QslCard.IsDirty = true;
				QslCard.InvalidateVisual();
			}
		}
	}
}
