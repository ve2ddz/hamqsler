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
using System.Windows.Media;

namespace hamqsler
{
	/// <summary>
	/// Abstract class that is the base for Card and all card items.
	/// </summary>
	abstract public class CardItem : UIElement
	{
public static readonly DependencyProperty DisplayRectangleProperty =
            DependencyProperty.Register("DisplayRectangle", typeof(Rect),
        typeof(CardItem), new PropertyMetadata(new Rect(0, 0, 0, 0)));

        public Rect DisplayRectangle
        {
            get { return (Rect)GetValue(DisplayRectangleProperty); }
            set { SetValue(DisplayRectangleProperty, value); }
        }

        // default constructor
		public CardItem() : base()
		{
			this.SnapsToDevicePixels = true;
		}
		
		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="r">Rect object that describes the location and size of the card item</param>
		public CardItem(Rect r)
		{
			this.SnapsToDevicePixels = true;
			DisplayRectangle = r;
		}
	}
}
