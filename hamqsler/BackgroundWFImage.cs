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
	/// Description of BackgroundWFImage.
	/// </summary>
	public class BackgroundWFImage : ImageWFBase
	{
		public BackgroundWFImage()
		{
		}

		/// <summary>
		/// Calculate the DisplayRectangle for this BackgroundImage
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
		            
		            w = QslCard.Width;
		            h = QslCard.Height;
		            double scaleX = w / dWidth;
		            double scaleY = h / dHeight;
		            double scale = scaleX > scaleY ? scaleX : scaleY;
		            dWidth *= scale;
		            dHeight *= scale;
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
		            this.X = (int)x;
		            this.Y = (int)y;
		            this.Width = (int)w;
		            this.Height = (int)h;
				}
			}
		}
		
		/// <summary>
		/// Set display rectangle to default size (only used when there is no image file)
		/// </summary>
		protected override void ResetRectangle()
		{
			this.X = 0;
			this.Y = 0;
			this.Width = QslCard.Width;
			this.Height = QslCard.Height;
		}
		
		/// <summary>
		/// Make a deep copy of this BackgroundWFImage object
		/// </summary>
		/// <returns>BackgroundWFImage object that is a deep copy of this one</returns>
		public BackgroundWFImage Clone()
		{
			BackgroundWFImage image = new BackgroundWFImage();
			image.CopyImageBaseProperties(this);
			return image;
		}
	}
}
