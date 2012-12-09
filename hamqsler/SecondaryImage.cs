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
		
		/// <summary>
		/// Constructor
		/// </summary>
		public SecondaryImage() : base()
		{
		}
		
		/// <summary>
		/// Calculate the DisplayRectangle for this SecondaryImage
		/// </summary>
		/// <param name="x">X coordinate of upper left corner</param>
		/// <param name="y">Y coordinate of upper left corner</param>
		/// <param name="w">Width</param>
		/// <param name="h">Height</param>
		protected override void CalculateRectangle(out double x, out double y, out double w,
		                                          out double h)
		{
            w = imageStartDimension;
            h = imageStartDimension;
			if(BitMapImage != EmptyImage)
			{
	            double dWidth = BitMapImage.Width;
	            double dHeight = BitMapImage.Height;
	            double scaleX = w / dWidth;
	            double scaleY = h / dHeight;
	            double scale = scaleX > scaleY ? scaleX : scaleY;
	            w = dWidth * scale;
	            h = dHeight * scale;
			}
	        x = (QslCard.DisplayWidth - w) / 2;
	        y = (QslCard.DisplayHeight - h) / 2;
		}
		
		/// <summary>
		/// Set display rectangle to default size (only used when there is no image file)
		/// </summary>
		protected override void ResetRectangle()
		{
			DisplayX = (QslCard.DisplayWidth - imageStartDimension) / 2;
			DisplayY = (QslCard.DisplayHeight - imageStartDimension) / 2;
			DisplayWidth = imageStartDimension;
			DisplayHeight = imageStartDimension;
		}
	}
}
