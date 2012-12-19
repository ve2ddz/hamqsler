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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;

namespace hamqsler
{
	/// <summary>
	/// Base class for all views of CardItem objects
	/// </summary>
	public abstract class CardItemView : UserControl
	{
		// Reference to the CardItem that this view displays
		private CardItem itemData = null;
		public CardItem ItemData
		{
			get {return itemData;}
		}
		
		private bool isLeftMouseButtonDown = false;
		protected bool IsLeftMouseButtonDown
		{
			get {return isLeftMouseButtonDown;}
			set {isLeftMouseButtonDown = value;}
		}

		protected Point leftMouseDownPoint = new Point(0, 0);
		protected Rect originalDisplayRectangle;
		protected static int cornerSize = 5;

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
		
		/// <summary>
		/// Default constructor
		/// </summary>
		public CardItemView()
		{}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="item">CardItem that this view displays</param>
		public CardItemView(CardItem item)
		{
			itemData = item;
		}
		
		/// <summary>
		/// Determine if the mouse cursor is over this CardItemView
		/// </summary>
		/// <param name="x">X coordinate to check (is relative to enclosing CardView)</param>
		/// <param name="y">Y coordinate to check (is relative to enclosing CardView</param>
		/// <returns>True if (x, y) is over this CardItemView</returns>
		public bool CursorIsOverThisView(double x, double y)
		{
			// Note: The method below is more accurate than using IsMouseDirectlyOver for each
			// UIElement that makes up the view because Margins are not considered in IsMouseDirectlyOver
			// The content of each CardItemView is always a Panel (e.g. StackPanel or Canvas)
			Panel panel = Content as Panel;
			// Get the rectangle that surrounds this CardItemView
			Rectangle r = new Rectangle();
			foreach(FrameworkElement elt in panel.Children)
			{
				r = elt as Rectangle;
				if(r != null)
				{
					break;
				}
			}
			// If the Rectangle is under the mouse, then we have the correct CardItemView
			return CardItemView.WithinRectangle(new Rect(Canvas.GetLeft(this), Canvas.GetTop(this),
					                            r.ActualWidth, r.ActualHeight), x, y);
		}
		
		/// <summary>
		/// Helper method that determines if the x and y coords are within the Rect
		/// </summary>
		/// <param name="rect">Rect to test</param>
		/// <param name="x">X coordinate to test</param>
		/// <param name="y">y coordinate to test</param>
		/// <returns>True if ( x, y) is within the Rect, False otherwise</returns>
		public static bool WithinRectangle(Rect rect, double x, double y)
		{
			return (x >= rect.X) && (x <= rect.X + rect.Width) && 
				(y >= rect.Y) && (y <= rect.Y + rect.Height);
		}
		
		/// <summary>
		/// Handler for MouseMove events related to this CardItem.
		/// Called from CardCanvas.
		/// </summary>
		/// <param name="e">Not used</param>
		public void MoveMouse(CardView view, MouseEventArgs e)
		{
			if(ItemData.IsSelected)
			{
				if(IsLeftMouseButtonDown)
				{
					HandleMouseMoveWithLeftMouseButtonDown(view, e);
				}
				else
				{
					HandleMouseMoveWithLeftMouseButtonUp(e);
				}
			}
			else
			{
				// not selected so highlight the CardItem.
				if(!ItemData.IsHighlighted)
				{
					ItemData.QslCard.ClearHighlighted();
					ItemData.IsHighlighted = true;
				}
			}		
		}
		
		/// <summary>
		/// Handle MouseLeftButtonDown event
		/// </summary>
		/// <param name="view">CardView object that contains this CardItemView</param>
		/// <param name="e">MouseButtonEventArgs object</param>
        // Must be public because called from CardCanvas
        public void OnMouseLeftButtonDown(CardView view, MouseButtonEventArgs e)
        {
                Point pt = e.GetPosition(this);
                if(GetCursorLocation(pt.X, pt.Y) != CursorLocation.Outside)
                {
	                IsLeftMouseButtonDown = true;
	                originalDisplayRectangle = new Rect(ItemData.DisplayX, ItemData.DisplayY,
	                                                    GetWidth(), GetHeight());
	                leftMouseDownPoint = e.GetPosition(view);
            }
            e.Handled = true;
        }

		/// <summary>
		/// Handle LeftMouseButtonUp event 
		/// </summary>
        // Must be public because called from CardCanvas
        public void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            IsLeftMouseButtonDown = false;
            Point pt = e.GetPosition(this);
            if(GetCursorLocation(pt.X, pt.Y) == CursorLocation.Outside)
            {
            	Mouse.OverrideCursor = Cursors.Arrow;
            }
        }
        
		/// <summary>
		/// Helper method to handle MouseMove events when the related CardItem is selected
		/// and the left mouse button is down. This method is overridden in ImageView,
		/// TextItemView, and QsosBoxView classes.
		/// </summary>
		/// <param name="view">CardView object that contains this CardItemView</param>
		/// <param name="e">MouseEventArgs object</param>
		protected virtual void HandleMouseMoveWithLeftMouseButtonDown(CardView view, MouseEventArgs e)
		{
			throw new NotImplementedException();
		}
		
		/// <summary>
		/// Helper method to handle MouseMove events when the related CardItem is selected
		/// but the left mouse button is not down. This method is overridden in ImageView,
		/// TextItemView, and QsosBoxView classes.
		/// </summary>
		/// <param name="e">MouseEventArgs object</param>
		protected virtual void HandleMouseMoveWithLeftMouseButtonUp(MouseEventArgs e)
		{
			throw new NotImplementedException();
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
			Rect nw = new Rect(-cornerSize, -cornerSize,
			                   2 * cornerSize, 2 * cornerSize);
			Rect ne = new Rect(GetWidth() - cornerSize, -cornerSize,
			                   2 * cornerSize, 2 * cornerSize);
			Rect se = new Rect(GetWidth() - cornerSize, GetHeight() - cornerSize,
			                   2 * cornerSize, 2 * cornerSize);
			Rect sw = new Rect(cornerSize, GetHeight() - cornerSize,
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
			else if(WithinRectangle(new Rect(0, 0, GetWidth(), GetHeight()), x, y))
			{
				return CursorLocation.Inside;
			}
			else
			{
				return CursorLocation.Outside;
			}
		}
		
		/// <summary>
		/// Helper method that returns the actual rendered width of this CardItemView
		/// </summary>
		/// <returns>Rendered width in device independent units</returns>
		protected virtual double GetWidth()
		{
			// nust be overridden in child views
			throw new NotImplementedException();
		}
		
		/// <summary>
		/// Helper method that returns the actual rendered height of this CardItemView
		/// </summary>
		/// <returns>Rendered height in device independent units</returns>
		protected virtual double GetHeight()
		{
			// must be overridden in child views
			throw new NotImplementedException();
		}
	}
}
