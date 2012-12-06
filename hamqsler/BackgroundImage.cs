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
		public BackgroundImage() : base()
		{
		}
		
		/// <summary>
		/// Calculate the DisplayRectangle for this BackgroundImage
		/// </summary>
		/// <returns>DisplayRectangle for the image, or QslCard.DisplayRectangle if no image</returns>
		protected override void CalculateRectangle(out double x, out double y, out double w,
		                                          out double h)
		{
			x = DisplayX;
			y = DisplayY;
			w = DisplayWidth;
			h = DisplayHeight;
			if(bImage == null)
			{
				x = QslCard.DisplayX;
				y = QslCard.DisplayY;
				w = QslCard.DisplayWidth;
				h = QslCard.DisplayHeight;
			}
			else
			{
	            x = 0;
	            y = 0;
	            double dWidth = bImage.Width;
	            double dHeight = bImage.Height;
	            
	            w = QslCard.DisplayWidth;
	            h = QslCard.DisplayHeight;
	            double scaleX = w / dWidth;
	            double scaleY = h / dHeight;
	            double scale = scaleX > scaleY ? scaleX : scaleY;
	            w *= scale;
	            h *= scale;
	            if (dWidth > w)
	            {
	                x = (w - dWidth) / 2;
	            }
	            if (dHeight > h)
	            {
	                y = (h - dHeight) / 2;
	            }
			}
		}
		
	}
}
