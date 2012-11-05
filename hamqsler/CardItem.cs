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
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace hamqsler
{
	/// <summary>
	/// Abstract class that is the base for Card and all card items.
	/// </summary>
	abstract public class CardItem : UIElement
	{
		protected static readonly DependencyProperty DisplayRectangleProperty =
		            DependencyProperty.Register("DisplayRectangle", typeof(Rect),
		        typeof(CardItem), new PropertyMetadata(new Rect(0, 0, 0, 0)));

        public virtual Rect DisplayRectangle
        {
            get { return (Rect)GetValue(DisplayRectangleProperty); }
            set { SetValue(DisplayRectangleProperty, value); }
        }

        [NonSerialized]
		private Card qslCard;
		public Card QslCard
		{
			get {return qslCard;}
			set {qslCard = value;}
		}
		
		private bool isHighlighted = false;
		public bool IsHighlighted
		{
			get {return isHighlighted;}
			set {isHighlighted = value;}
		}
		
		private bool isSelected = false;
		public bool IsSelected
		{
			get {return isSelected;}
			set {isSelected = value;}
		}
		
		private bool isLeftMouseButtonDown = false;
		protected bool IsLeftMouseButtonDown
		{
			get {return isLeftMouseButtonDown;}
			set {isLeftMouseButtonDown = value;}
		}
		
		protected Point leftMouseDownPoint = new Point(0, 0);
		protected Rect originalDisplayRectangle;
		
		protected static Pen hightlightPen = CreateHighlightPen();
		protected static Pen selectPen = CreateSelectPen();
		
		public enum CursorLocation
		{
			None,
			Outside,
			Inside,
			NW,
			NE,
			SW,
			SE
		};
		
		protected CursorLocation cursorLoc = CursorLocation.Outside;
		
		protected const int cornerSize = 5;
		
				
        // default constructor
		public CardItem() : base()
		{
			InitializeCardItem();
		}
		
		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="r">Rect object that describes the location and size of the card item</param>
		public CardItem(Rect r)
		{
			InitializeCardItem();
			DisplayRectangle = r;
		}
		
		/// <summary>
		/// Perform actions common to all constructors
		/// </summary>
        private void InitializeCardItem()
        {
            this.SnapsToDevicePixels = true;
        }
		
		/// <summary>
		/// Handle MouseLeftButtonDown event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">MosueButtonEventArgs object</param>
        // Must be public because called from CardCanvas
        public virtual void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.IsSelected)
            {
                Point pt = e.GetPosition(QslCard);
                if(GetCursorLocation(pt.X, pt.Y) != CursorLocation.Outside)
                {
               		// only want to capture mouse if cursor within this card item
	                IsLeftMouseButtonDown = true;
	                originalDisplayRectangle = DisplayRectangle;
	                leftMouseDownPoint = pt;
	                this.CaptureMouse();
                }
            }
            e.Handled = true;
        }

		/// <summary>
		/// Handle LeftMouseButtonUp event 
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
        // Must be public because called from CardCanvas
        public virtual void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (this.IsSelected && IsLeftMouseButtonDown)
            {
                IsLeftMouseButtonDown = false;
                this.ReleaseMouseCapture();
            }
//            e.Handled = true;
        }

		/// <summary>
		/// Handles MouseMove events when mouse is over this CardItem
		/// </summary>
		/// <param name="e">MouseEventArgs object</param>
		public virtual void MoveMouse(MouseEventArgs e) 
		{
			if(!this.IsSelected)
			{
				if(!this.IsHighlighted)
				{
					QslCard.ClearHighlighted();
					this.IsHighlighted = true;
				}
			}		
		}

		/// <summary>
		/// Gets the location within this CardItem
		/// </summary>
		/// <param>
		/// X coordinate
		/// </param>
		/// <param>
		/// Y coordinate
		/// </param>
		/// <returns>
		/// CursorLocation of the coordinates
		/// </returns>
		public CursorLocation GetCursorLocation(double x, double y)
		{
			Rect nw = new Rect(DisplayRectangle.Left - cornerSize,
			                   DisplayRectangle.Top - cornerSize,
			                   2 * cornerSize, 2 * cornerSize);
			Rect ne = new Rect(DisplayRectangle.Left + DisplayRectangle.Width - cornerSize,
			                   DisplayRectangle.Top - cornerSize,
			                   2 * cornerSize, 2 * cornerSize);
			Rect se = new Rect(DisplayRectangle.Left + DisplayRectangle.Width - cornerSize,
			                   DisplayRectangle.Top + DisplayRectangle.Height - cornerSize,
			                   2 * cornerSize, 2 * cornerSize);
			Rect sw = new Rect(DisplayRectangle.Left - cornerSize,
			                   DisplayRectangle.Top + DisplayRectangle.Height - cornerSize,
			                   2 * cornerSize, 2 * cornerSize);
			if(WithinRectangle(nw, x, y))
 		    {
			   	return CursorLocation.NW;
		    }
			else if(WithinRectangle(ne, x, y))
			{
				return CursorLocation.NE;
			}
			else if(WithinRectangle(se, x, y))
			{
				return CursorLocation.SE;
			}
			else if(WithinRectangle(sw, x, y))
			{
				return CursorLocation.SW;
			}
			else if(WithinRectangle(DisplayRectangle, x, y))
			{
				return CursorLocation.Inside;
			}
			else
			{
				return CursorLocation.Outside;
			}
		}
		
		/// <summary>
		///  Helper method to determine if x, y is within the rectangle
		/// </summary>
		/// <param>The rectangle to test
		/// <param>
		/// X coordinate to test
		/// </param>
		/// <param>
		/// Y coordinate to test
		/// </param>
		/// <returns>
		/// true if x, y within the rectangle
		/// false if x, y not within the rectangle
		/// </returns>
		public static bool WithinRectangle(Rect r, double x, double y)
		{
			return (x >= r.X) && (x <= r.X + r.Width) && (y >= r.Y) && (y <= r.Y + r.Height);
		}
		
		/// <summary>
		/// Static method that creates the highlight pen
		/// </summary>
		/// <returns>the highlight pen</returns>
		private static Pen CreateHighlightPen()
		{
			Pen hPen = new Pen(Brushes.OrangeRed, 3);
			double[] dashes = {8, 5, 5, 4};
			hPen.DashStyle = new DashStyle(dashes, 0);
			return hPen;
		}
		
		/// <summary>
		/// Static method that creates the select pen
		/// </summary>
		/// <returns>the select pen</returns>
		private static Pen CreateSelectPen()
		{
			Pen sPen = new Pen(Brushes.Blue, 3);
			double[] dashes = {8, 5, 5, 4};
			sPen.DashStyle = new DashStyle(dashes, 5);
			return sPen;
		}
		
		/// <summary>
		/// Call the OnRender method for this CardItem. This method is necessary because OnRender
		/// is not called directly, but rather through Card.OnRender and OnRender is a protected
		/// method inherited from UIElement
		/// </summary>
		/// <param name="dc">DrawingContext to render this CardItem on</param>
		public void Render(DrawingContext dc)
		{
			this.OnRender(dc);
		}
				
	}
}
