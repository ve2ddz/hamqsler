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
	/// Description of SecondaryImage.
	/// </summary>
	public class SecondaryImage : CardImageBase
	{
		private static double imageStartDimension = 96;	// 1 inch
		
		public SecondaryImage() : base()
		{
		}
		
		/// <summary>
		/// Calculate the DisplayRectangle for this SecondaryImage
		/// </summary>
		/// <returns>DisplayRectangle for the image, or 1 inch by 1 inch square if no image</returns>
		protected override Rect CalculateRectangle()
		{
			// must get DisplayRectangle directly from DisplayRectangleProperty to prevent
			// circular reference
			Rect r = (Rect)GetValue(DisplayRectangleProperty);
			if(bImage == null)
			{
				Rect cardRect = QslCard.DisplayRectangle;
				r = new Rect((cardRect.Width - imageStartDimension) / 2,
				             (cardRect.Height - imageStartDimension) / 2,
				             imageStartDimension, imageStartDimension);
			}
			else
			{
				if(r == new Rect(0, 0, 0, 0))
				{
					Rect cardRect = QslCard.DisplayRectangle;
					r = new Rect((cardRect.Width - imageStartDimension) / 2,
					             (cardRect.Height - imageStartDimension) / 2,
					             imageStartDimension, imageStartDimension);					
				}
	            double upperLeftX = r.X;
	            double upperLeftY = r.Y;
	            double dWidth = bImage.Width;
	            double dHeight = bImage.Height;
	            
	            double width = r.Width;
	            double height = r.Height;
	            double scaleX = width / dWidth;
	            double scaleY = height / dHeight;
	            double scale = scaleX > scaleY ? scaleX : scaleY;
	            dWidth *= scale;
	            dHeight *= scale;
	            if (dWidth > width)
	            {
	                upperLeftX += (width - dWidth) / 2;
	            }
	            if (dHeight > height)
	            {
	                upperLeftY += (height - dHeight) / 2;
	            }
	           r = new Rect(upperLeftX, upperLeftY, dWidth, dHeight);
					
			}
			return r;
		}
	}
}
