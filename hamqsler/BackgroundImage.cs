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
using System.Xml;

namespace hamqsler
{
	/// <summary>
	/// Description of BackgroundImage.
	/// </summary>
	public class BackgroundImage : CardImageBase
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public BackgroundImage() : base(true){}
		
		/// <summary>
		/// Constructor
		/// </summary>
        /// <param name="isInDesignMode">Boolean to indicate if this image is to be displayed
        /// in design mode</param>
		public BackgroundImage(bool isInDesignMode) : base(isInDesignMode)
		{}
		
		/// <summary>
		/// Calculate the DisplayRectangle for this BackgroundImage
		/// </summary>
		/// <param name="x">X coordinate of upper left corner</param>
		/// <param name="y">Y coordinate of upper left corner</param>
		/// <param name="w">Width</param>
		/// <param name="h">Height</param>
		protected override void CalculateRectangle(out double x, out double y, out double w,
		                                          out double h)
		{
			x = DisplayX;
			y = DisplayY;
			w = DisplayWidth;
			h = DisplayHeight;
			if(QslCard != null)
			{
				if(BitMapImage != EmptyImage)
				{
		            double dWidth = BitMapImage.Width;
		            double dHeight = BitMapImage.Height;
		            
		            w = QslCard.DisplayWidth;
		            h = QslCard.DisplayHeight;
		            double scaleX = w / dWidth;
		            double scaleY = h / dHeight;
		            double scale = scaleX > scaleY ? scaleX : scaleY;
		            w *= scale;
		            h *= scale;
		            if(x == 0 && y == 0)
		            {
			            if (dWidth > w)
			            {
			                x = (w - dWidth) / 2;
			                w = dWidth;
			            }
			            if (dHeight > h)
			            {
			                y = (h - dHeight) / 2;
			                h = dHeight;
			            }
		            }
				}
			}
		}
		
		/// <summary>
		/// Set display rectangle to default size (only used when there is no image file)
		/// </summary>
		protected override void ResetRectangle()
		{
			DisplayX = 0;
			DisplayY = 0;
			DisplayWidth = QslCard.DisplayWidth;
			DisplayHeight = QslCard.DisplayHeight;
		}
		
		/// <summary>
		/// Load values for this BackgroundImage from QslDnP card file contents
		/// </summary>
		/// <param name="itemNode">The BackgroundImage node</param>
		/// <param name="culture">CultureInfo that the card was created in</param>
		public override void Load(System.Xml.XmlNode itemNode, System.Globalization.CultureInfo culture)
		{
			XmlNode node = XmlProcs.GetFirstChildElement(itemNode);
			while(node != null)
			{
				switch(node.Name)
				{
					case "CardImageBase":
						base.Load(node, culture);
						break;
				}
				node = XmlProcs.GetNextSiblingElement(node);
			}
		}
	}
}
