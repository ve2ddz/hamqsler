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
using System.Windows.Shapes;

namespace hamqsler
{
	/// <summary>
	/// Base class for all views of CardItem objects
	/// </summary>
	public abstract class CardItemView : UserControl
	{
		private CardItem itemData = null;
		public CardItem ItemData
		{
			get {return itemData;}
		}
		
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
	}
}
