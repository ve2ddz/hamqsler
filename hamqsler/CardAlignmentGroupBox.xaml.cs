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
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using RadioButtonImage = System.Windows.Controls.Image;

namespace hamqsler
{
	/// <summary>
	/// CardAlignmentGroupBox class - displays and interacts with QslCard cards layout property
	/// </summary>
	public partial class CardAlignmentGroupBox : GroupBox
	{
		private CardWF qslCard = null;
		public CardWF QslCard
		{
			get {return qslCard;}
			set
			{
				qslCard = value;
				SetCardsLayouts();
				SetCardLayout();
			}
		}
		
		private const int MAXMARGIN = 25;
		// size of the paper in the radio button images is SCALEFACTOR/SCALEDPAGESZIE
		private const int SCALEFACTOR = 10000;
		private const int SCALEDPAGESIZE = 100;
		
		private int scaledPageWidth = 0;
		private int scaledPageHeight = 0;
		private int scaledCardWidth = 0;
		private int scaledCardHeight = 0;
		private int portraitCardsWide = 0;
		private int portraitCardsHigh = 0;
		private int landscapeCardsWide = 0;
		private int landscapeCardsHigh = 0;
		private int scaleFactor = 0;
		
		private PrinterSettings settings;

		/// <summary>
		/// Constructor
		/// </summary>
		public CardAlignmentGroupBox()
		{
			InitializeComponent();
		}
		
		/// <summary>
		/// Helper method that selects the alignment radio box based on Layout setting
		/// of QslCard CardPrintProperties
		/// </summary>
		private void SetCardLayout()
		{
			switch(QslCard.CardPrintProperties.Layout)
			{
				case PrintProperties.CardLayouts.PortraitTopLeft:
					portraitTopLeftButton.IsChecked = true;
					break;
			}
		}
		
		/// <summary>
		/// Creates and displays the images for the layout radio buttons
		/// </summary>
		public void SetCardsLayouts()
		{
			if(QslCard != null)
			{
				CalculateScaledPaperAndCardSizes();
				settings = new PrinterSettings();
				settings.PrinterName = QslCard.CardPrintProperties.PrinterName;
				settings.DefaultPageSettings.PaperSize =
					QslCard.CardPrintProperties.PrinterPaperSize;
				settings.DefaultPageSettings.PrinterResolution =
					QslCard.CardPrintProperties.Resolution;
				// if the page margins are greater than ¼ inch, then force InsideMargins
				// to be set
				RectangleF area = settings.DefaultPageSettings.PrintableArea;
				QslCard.CardPrintProperties.InsideMargins = 
					QslCard.CardPrintProperties.InsideMargins ||
					area.X > MAXMARGIN ||
					settings.DefaultPageSettings.PaperSize.Width - area.X -
					area.Width > MAXMARGIN ||
					area.Y > MAXMARGIN ||
					settings.DefaultPageSettings.PaperSize.Height - area.Y -
					area.Height > MAXMARGIN;
				// determine the number of cards that can be printed on portrait
				// and landscape pages
				CalculateCardsPerPortraitPage();
				CalculateCardsPerLandscapePage();
				// create the image
				portraitTopLeftButton.Content = CreatePortraitTopLeftButtonImage();
			}
		}
		
		/// <summary>
		/// Helper method that calculatees the scaled paper and card sizes.
		/// Paper is scaled so that the longer side is 100 pixels long
		/// </summary>
		private void CalculateScaledPaperAndCardSizes()
		{
			int pageWidth = QslCard.CardPrintProperties.PrinterPaperSize.Width;
			int pageHeight = QslCard.CardPrintProperties.PrinterPaperSize.Height;
			scaleFactor = SCALEFACTOR / pageHeight;
			scaledPageWidth = (pageWidth * scaleFactor) / SCALEDPAGESIZE;
			scaledPageHeight = (pageHeight * scaleFactor) / SCALEDPAGESIZE;
			scaledCardWidth = (QslCard.Width * scaleFactor) / SCALEDPAGESIZE;
			scaledCardHeight = (QslCard.Height * scaleFactor) / SCALEDPAGESIZE;			
		}
		
		/// <summary>
		/// Helper method that calculate the number of cards wide and high that can
		/// be printed on the selected page size in portrait orientation.
		/// </summary>
		private void CalculateCardsPerPortraitPage()
		{
			int pageWidth = QslCard.CardPrintProperties.PrinterPaperSize.Width;
			int pageHeight = QslCard.CardPrintProperties.PrinterPaperSize.Height;
			if(QslCard.CardPrintProperties.InsideMargins)
			{
				RectangleF area = settings.DefaultPageSettings.PrintableArea;
				pageWidth = (int)(area.Width - area.Left);
				pageHeight = (int)(area.Height - area.Top);
			}
			portraitCardsWide = pageWidth / QslCard.Width;
			portraitCardsHigh = pageHeight / QslCard.Height; 
		}
		
		/// <summary>
		/// Helper method that calculates the number of cards wide and high that can
		/// be printed on the selected page size in landscape orientation
		/// </summary>
		private void CalculateCardsPerLandscapePage()
		{
			int pageWidth = QslCard.CardPrintProperties.PrinterPaperSize.Height;
			int pageHeight = QslCard.CardPrintProperties.PrinterPaperSize.Width;
			if(QslCard.CardPrintProperties.InsideMargins)
			{
				RectangleF area = settings.DefaultPageSettings.PrintableArea;
				pageWidth = (int)(area.Height = area.Top);
				pageHeight = (int)(area.Width = area.Left);
			}
			landscapeCardsWide = pageWidth / QslCard.Width;
			landscapeCardsHigh = pageHeight / QslCard.Height;
		}
		
		/// <summary>
		/// Create image to be displayed in the portraitTopLeftButton
		/// </summary>
		/// <returns>Canvas object containing the generated image</returns>
		private Canvas CreatePortraitTopLeftButtonImage()
		{
			Canvas canvas = new Canvas();
			// create a Path for the page
			RectangleGeometry rect = new RectangleGeometry(
				new Rect(0, 0, scaledPageWidth, scaledPageHeight));
			Path pagePath = new Path();
			pagePath.Stroke = System.Windows.Media.Brushes.LightPink;
			pagePath.Fill = System.Windows.Media.Brushes.LightPink;
			pagePath.Data = rect;
			canvas.Children.Add(pagePath);
			
			// create a path for all of the cards
			Path cardsPath = new Path();
			cardsPath.Stroke = System.Windows.Media.Brushes.Blue;
			cardsPath.Fill = System.Windows.Media.Brushes.White;
			GeometryGroup geo = new GeometryGroup();
			int leftMargin = 0;
			int topMargin = 0;
			if(QslCard.CardPrintProperties.InsideMargins)
			{
				RectangleF area = settings.DefaultPageSettings.PrintableArea;
				leftMargin = (int)area.Left / scaleFactor;
				topMargin = (int)area.Top / scaleFactor;
			}
			for(int hCard = 0; hCard < portraitCardsWide; hCard++)
			{
				for(int vCard = 0; vCard < portraitCardsHigh; vCard++)
				{
					geo.Children.Add(new RectangleGeometry(
						new Rect(hCard*scaledCardWidth + leftMargin, 
						         vCard*scaledCardHeight + topMargin,
						         scaledCardWidth, scaledCardHeight)));
				}
			}
			cardsPath.Data = geo;
			canvas.Children.Add(cardsPath);
			return canvas;
		}
	}
}