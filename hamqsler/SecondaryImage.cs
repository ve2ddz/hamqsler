/*
 *  Author:
 *       Jim Orcheson <jimorcheson@gmail.com>
 * 
 *  Copyright © 2012, 2013 Jim Orcheson
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
using System.Globalization;
using System.Windows;
using System.Xml;

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
			x = DisplayX;
			y = DisplayY;
            w = imageStartDimension;
            h = imageStartDimension;
            bool imageFound = false;
			if(BitMapImage != EmptyImage)
			{
				imageFound = true;
	            double dWidth = BitMapImage.Width;
	            double dHeight = BitMapImage.Height;
	            double scaleX = w / dWidth;
	            double scaleY = h / dHeight;
	            double scale = scaleX > scaleY ? scaleX : scaleY;
	            w = dWidth * scale;
	            h = dHeight * scale;
			}
			if(QslCard != null && !imageFound)
			{
		        x = (QslCard.DisplayWidth - w) / 2;
		        y = (QslCard.DisplayHeight - h) / 2;
			}
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

		/// <summary>
		/// Load values for this SecondaryImage from QslDnP card file contents
		/// </summary>
		/// <param name="itemNode">The SecondaryImage node</param>
		/// <param name="culture">CultureInfo that the card was created in</param>
		public override void Load(XmlNode itemNode, CultureInfo culture)
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
		
		/// <summary>Make a deep copy of image properties
		/// </summary>
		/// <param name="card">Card object that this image belongs to</param>
		/// <param name="image">Image whose properties are to be copied</param>
		public override void CopyImageProperties(Card card, CardImageBase image)
		{
			base.CopyImageProperties(card, image);
			CopyBaseProperties(card, image);
		}
	}
}
