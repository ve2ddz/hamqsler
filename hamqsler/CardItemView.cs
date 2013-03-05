/*
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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace hamqsler
{
	/// <summary>
	/// Base class for all views of CardItem objects
	/// </summary>
	public abstract class CardItemView : UserControl
	{
		// Reference to the CardItem that this view displays
		protected static readonly DependencyProperty ItemDataProperty =
			DependencyProperty.Register("ItemData", typeof(CardItem), typeof(CardItemView),
			                            new PropertyMetadata(null));
		public CardItem ItemData
		{
			get {return (CardItem)GetValue(ItemDataProperty);}
			set {SetValue(ItemDataProperty, value);}
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
		
		protected static Pen selectedPen = null;
		protected static Pen highlightedPen = null;
		
		static CardItemView()
		{
			CreateSelectedPen();
			CreateHighlightedPen();
		}
		
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
			ItemData = item;
		}
		
		/// <summary>
		/// Helper method that creates the pen used to draw rectangles around a carditem
		/// when that carditem is selected
		/// </summary>
		private static void CreateSelectedPen()
		{
			selectedPen = new Pen(Brushes.DarkBlue, 4.0);
			selectedPen.DashStyle = DashStyles.DashDot;
			selectedPen.StartLineCap = PenLineCap.Square;
			selectedPen.EndLineCap = PenLineCap.Square;
			selectedPen.DashCap = PenLineCap.Square;
		}
		
		/// <summary>
		/// Helper method that creates the pen used to draw rectangles around a carditem
		/// when that carditem is highlighted
		/// </summary>
		private static void CreateHighlightedPen()
		{
			highlightedPen = new Pen(Brushes.Orange, 4.0);
			highlightedPen.DashStyle = DashStyles.DashDot;
			highlightedPen.StartLineCap = PenLineCap.Square;
			highlightedPen.EndLineCap = PenLineCap.Square;
			highlightedPen.DashCap = PenLineCap.Square;			
		}
		
		/// <summary>
		/// Draw the carditemview.
		/// This method provides a public interface to OnRender
		/// </summary>
		/// <param name="dc">DrawingContext to draw the carditem on</param>
		public void Render(DrawingContext dc)
		{
			OnRender(dc);
		}
		
		/// <summary>
		/// Determine if the mouse cursor is over this CardItemView
		/// </summary>
		/// <param name="x">X coordinate to check (is relative to enclosing CardView)</param>
		/// <param name="y">Y coordinate to check (is relative to enclosing CardView</param>
		/// <returns>True if (x, y) is over this CardItemView</returns>
		public bool CursorIsOverThisView(double x, double y)
		{
			// Get the rectangle that surrounds this CardItemView
			Rect r = new Rect(ItemData.DisplayX, ItemData.DisplayY,
			                  ItemData.DisplayWidth, ItemData.DisplayHeight);
			return CardItemView.WithinRectangle(r, x, y);
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
					HandleMouseMoveWithLeftMouseButtonUp(view, e);
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
                Point pt = e.GetPosition(view);
                if(GetCursorLocation(pt.X, pt.Y) != CursorLocation.Outside)
                {
	                IsLeftMouseButtonDown = true;
	                originalDisplayRectangle = new Rect(ItemData.DisplayX, ItemData.DisplayY,
	                                                    ItemData.DisplayWidth,
	                                                    ItemData.DisplayHeight);
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
            Point pt = e.GetPosition(ItemData.QslCard.CardItemView);
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
		protected virtual void HandleMouseMoveWithLeftMouseButtonUp(CardView view, MouseEventArgs e)
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
			Rect nw = new Rect(ItemData.DisplayX - cornerSize, 
			                   ItemData.DisplayY - cornerSize,
			                   2 * cornerSize, 2 * cornerSize);
			Rect ne = new Rect(ItemData.DisplayX + ItemData.DisplayWidth - cornerSize, 
			                   ItemData.DisplayY - cornerSize,
			                   2 * cornerSize, 2 * cornerSize);
			Rect se = new Rect(ItemData.DisplayX + ItemData.DisplayWidth - cornerSize, 
			                   ItemData.DisplayY + ItemData.DisplayHeight- cornerSize,
			                   2 * cornerSize, 2 * cornerSize);
			Rect sw = new Rect(ItemData.DisplayX - cornerSize, 
			                   ItemData.DisplayY + ItemData.DisplayHeight - cornerSize,
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
			else if(WithinRectangle(new Rect(ItemData.DisplayX, ItemData.DisplayY,
			                                 ItemData.DisplayWidth,
			                                 ItemData.DisplayHeight), x, y))
			{
				return CursorLocation.Inside;
			}
			else
			{
				return CursorLocation.Outside;
			}
		}
		
		/// <summary>
		/// Render the CardItemView - this just draws a rectangle around a CardItem if it is
		/// selected or highlighted
		/// </summary>
		/// <param name="drawingContext">DrawingContext on which to draw the rectangle</param>
		protected override void OnRender(DrawingContext drawingContext)
		{
			base.OnRender(drawingContext);
			if(ItemData.IsSelected)
			{
				drawingContext.DrawRectangle(Brushes.Transparent, selectedPen,
				                             new Rect(ItemData.DisplayX, ItemData.DisplayY,
				                                      ItemData.DisplayWidth, ItemData.DisplayHeight));
			}
			else if(ItemData.IsHighlighted)
			{
				drawingContext.DrawRectangle(Brushes.Transparent, highlightedPen,
				                             new Rect(ItemData.DisplayX, ItemData.DisplayY,
				                                      ItemData.DisplayWidth, ItemData.DisplayHeight));
				
			}
		}
	}
}
