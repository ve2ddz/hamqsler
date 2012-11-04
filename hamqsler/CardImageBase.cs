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
				DisplayRectangle = QslCard.DisplayRectangle;
				dc.DrawRectangle(brush, new Pen(brush, 0), DisplayRectangle);
			}
			if(IsSelected)
			{
				dc.DrawRectangle(brush, selectPen, DisplayRectangle);
			}
			else if(IsHighlighted)
			{
				dc.DrawRectangle(brush, hightlightPen, DisplayRectangle);
			}
		}

	}
}
