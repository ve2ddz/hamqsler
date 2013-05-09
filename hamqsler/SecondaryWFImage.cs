/*
 *  Author:
 *       Jim Orcheson <jimorcheson@gmail.com>
 * 
 *  Copyright © 2013 Jim Orcheson
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

namespace hamqsler
{
	/// <summary>
	/// SecondaryWFImage class - describes secondary images.
	/// </summary>
	public class SecondaryWFImage : ImageWFBase
	{
		private int initialSize = 100;
		
		/// <summary>
		/// Default constructor
		/// </summary>
		public SecondaryWFImage()
		{
		}
		
		/// <summary>
		/// Calculate the DisplayRectangle for this SecondaryImage
		/// </summary>

		protected override void CalculateRectangle()
		{
			double x = this.X;
			double y = this.Y;
			double w = this.Width;
			double h = this.Height;
			if(QslCard != null)
			{
				if(Image != EmptyImage)
				{
		            double dWidth = Image.Width;
		            double  dHeight = Image.Height;
		            
		            w = initialSize;
		            h = initialSize;
		            double scaleX = w / dWidth;
		            double scaleY = h / dHeight;
		            double scale = scaleX > scaleY ? scaleX : scaleY;
		            dWidth *= scale;
		            dHeight *= scale;
		            this.X = (int)x;
		            this.Y = (int)y;
		            this.Width = (int)dWidth;
		            this.Height = (int)dHeight;
				}
			}
		}
		
		/// <summary>
		/// Set display rectangle to default size (only used when there is no image file)
		/// </summary>
		protected override void ResetRectangle()
		{
			this.X = (QslCard.Width - initialSize) / 2;
			this.Y = (QslCard.Height - initialSize) / 2;
			this.Width = initialSize;
			this.Height = initialSize;
		}
	}
}
