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
        	get { return CalculateRectangle(); }
            set { SetValue(DisplayRectangleProperty, value); }
        }
        
        /// <summary>
        /// CalculateRectangle determines the DisplayRectangle size for the image when no image
        /// is loaded
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
		}
		
		/// <summary>
		/// OnRender event handler - draws the image
		/// </summary>
		/// <param name="dc">Context to draw the card on</param>
		protected override void OnRender(DrawingContext dc)
		{
			Rect rect = QslCard.DisplayRectangle;
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
					CalculateRectangle();
				}
				else
				{
					// reset bImage
					bImage = null;
				}
				// Image has changed, so let QslCard know it has changed
				// and redisplay card
				QslCard.IsDirty = true;
				QslCard.InvalidateVisual();
			}
		}
	}
}
