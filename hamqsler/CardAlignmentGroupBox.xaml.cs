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
			portraitTopLeftButton.IsChecked = 
				(QslCard.CardPrintProperties.Layout == 
				 PrintProperties.CardLayouts.PortraitTopLeft);
			portraitTopCenterButton.IsChecked = 
				(QslCard.CardPrintProperties.Layout == 
				 PrintProperties.CardLayouts.PortraitTopCenter);
			portraitCenterButton.IsChecked = 
				(QslCard.CardPrintProperties.Layout == 
				 PrintProperties.CardLayouts.PortraitCenter);
			landscapeTopLeftButton.IsChecked = 
				(QslCard.CardPrintProperties.Layout ==
				 PrintProperties.CardLayouts.LandscapeTopLeft);
			landscapeTopCenterButton.IsChecked = 
				(QslCard.CardPrintProperties.Layout ==
				 PrintProperties.CardLayouts.LandscapeTopCenter);
			landscapeCenterButton.IsChecked = 
				(QslCard.CardPrintProperties.Layout ==
				 PrintProperties.CardLayouts.LandscapeCenter) ||
				(QslCard.CardPrintProperties.Layout ==
				 PrintProperties.CardLayouts.None);
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
				bool insideMargins = QslCard.CardPrintProperties.InsideMargins;
				if(App.Logger.DebugPrinting)
				{
					string info = 
						string.Format("CardAlignmentGroupBox.SetCardsLayouts:" +
					              Environment.NewLine +
					              "\tPapersize = {0} x {1}" +
					              Environment.NewLine +
					              "\tPrintableArea = {2}, {3}: {4} x {5}" +
					              Environment.NewLine +
					              "\tInsideMargins original value = {6}" +
					              Environment.NewLine,
					              QslCard.CardPrintProperties.PrinterPaperSize.Width,
					              QslCard.CardPrintProperties.PrinterPaperSize.Height,
					              area.Left, area.Top, area.Width, area.Height,
					             insideMargins);
					App.Logger.Log(info);
				}
				QslCard.CardPrintProperties.InsideMargins = 
				QslCard.CardPrintProperties.InsideMargins ||
				area.X > MAXMARGIN ||
				settings.DefaultPageSettings.PaperSize.Width - area.X -
				area.Width > MAXMARGIN ||
				area.Y > MAXMARGIN ||
				settings.DefaultPageSettings.PaperSize.Height - area.Y -
				area.Height > MAXMARGIN;
				if(App.Logger.DebugPrinting && 
				   insideMargins != QslCard.CardPrintProperties.InsideMargins)
				{
					App.Logger.Log(string.Format("CardAlignmentGroupBox.SetCardsLayouts:" +
					              Environment.NewLine +
					              "\tInsideMargins changed to {0}" +
					              Environment.NewLine,
					              QslCard.CardPrintProperties.InsideMargins));
					              
				}
				// determine the number of cards that can be printed on portrait
				// and landscape pages
				CalculateCardsPerPortraitPage();
				CalculateCardsPerLandscapePage();
				// create the images
				CreatePortraitButtonImages();
				CreateLandscapeButtonImages();
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
			if(App.Logger.DebugPrinting)
			{
				string info =
					string.Format("CardAlignmentGroupBox.CalculateScaledPaperAndCardSizes:" +
					              Environment.NewLine +
					              "\tscaledPage size = {0} x {1}" +
					              Environment.NewLine +
					              "\tscaledCard size = {2} x {3}" +
					              Environment.NewLine +
					              "\tscaleFactor = {4}" +
					              Environment.NewLine,
					              scaledPageWidth, scaledPageHeight,
					              scaledCardWidth, scaledCardHeight, scaleFactor);
				App.Logger.Log(info);
			}
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
			if(App.Logger.DebugPrinting)
			{
				string info = 
					string.Format("CardAlignmentGroupBox.CalculateCardsPerPortraitPage:" +
					              Environment.NewLine +
					              "\tInside Margins = {6}" +
					              Environment.NewLine +
					              "\tPage Size = {0} x {1}" +
					              Environment.NewLine +
					              "\tCard Size = {2} x {3}" +
					              Environment.NewLine +
					              "\tPortrait cards wide = {4}" +
					              Environment.NewLine +
					              "\tPortrait cards high = {5}" +
					              Environment.NewLine,
					              pageWidth, pageHeight, QslCard.Width, QslCard.Height,
					              portraitCardsWide, portraitCardsHigh,
					              QslCard.CardPrintProperties.InsideMargins);
				App.Logger.Log(info);
			}
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
				pageWidth = (int)(area.Height - area.Top);
				pageHeight = (int)(area.Width - area.Left);
			}
			landscapeCardsWide = pageWidth / QslCard.Width;
			landscapeCardsHigh = pageHeight / QslCard.Height;
			if(App.Logger.DebugPrinting)
			{
				string info = 
					string.Format("CardAlignmentGroupBox.CalculateCardsPerLandscapePage:" +
					              Environment.NewLine +
					              "\tInside Margins = {6}" +
					              Environment.NewLine +
					              "\tPage Size = {0} x {1}" +
					              Environment.NewLine +
					              "\tCard Size = {2} x {3}" +
					              Environment.NewLine +
					              "\tLandscape cards wide = {4}" +
					              Environment.NewLine +
					              "\tLandscape cards high = {5}" +
					              Environment.NewLine,
					              pageWidth, pageHeight, QslCard.Width, QslCard.Height,
					              landscapeCardsWide, landscapeCardsHigh,
					              QslCard.CardPrintProperties.InsideMargins);
				App.Logger.Log(info);
			}
		}
		
		/// <summary>
		/// Create the images for the portrait radio buttons
		/// </summary>
		private void CreatePortraitButtonImages()
		{
			RectangleF area = settings.DefaultPageSettings.PrintableArea;
			int leftOffset = 0;
			int topOffset = 0;
			if(QslCard.CardPrintProperties.InsideMargins)
			{
				leftOffset = (int)area.Left / scaleFactor;
				topOffset = (int)area.Top / scaleFactor;				
			}
			portraitTopLeftButton.Content = 
				CreateButtonImage(scaledPageWidth, scaledPageHeight, leftOffset, topOffset,
				                 portraitCardsWide, portraitCardsHigh);
			
			leftOffset = (scaledPageWidth - scaledCardWidth * portraitCardsWide) / 2;
			if(QslCard.CardPrintProperties.InsideMargins &&
			   leftOffset < (MAXMARGIN / scaleFactor))
			{
				leftOffset = (MAXMARGIN / scaleFactor);
			}
			portraitTopCenterButton.Content =
				CreateButtonImage(scaledPageWidth, scaledPageHeight, leftOffset, topOffset,
				                 portraitCardsWide, portraitCardsHigh);
			topOffset = (scaledPageHeight - scaledCardHeight * portraitCardsHigh) / 2;
			if(QslCard.CardPrintProperties.InsideMargins &&
			   topOffset < (MAXMARGIN / scaleFactor))
			{
				topOffset = (MAXMARGIN / scaleFactor);
			}
			portraitCenterButton.Content =
				CreateButtonImage(scaledPageWidth, scaledPageHeight, leftOffset, topOffset,
				                 portraitCardsWide, portraitCardsHigh);
		}
		
		/// <summary>
		/// Create the images for the landscape radio buttons
		/// </summary>
		private void CreateLandscapeButtonImages()
		{
			RectangleF area = settings.DefaultPageSettings.PrintableArea;
			int leftOffset = 0;
			int topOffset = 0;
			if(QslCard.CardPrintProperties.InsideMargins)
			{
				leftOffset = (int)area.Top / scaleFactor;
				topOffset = (int)area.Left / scaleFactor;
			}
			landscapeTopLeftButton.Content =
				CreateButtonImage(scaledPageHeight, scaledPageWidth, leftOffset, topOffset,
				                 landscapeCardsWide, landscapeCardsHigh);
			leftOffset = (scaledPageHeight - scaledCardWidth * landscapeCardsWide) / 2;
			if(QslCard.CardPrintProperties.InsideMargins &&
			   leftOffset < MAXMARGIN / scaleFactor)
			{
				leftOffset = MAXMARGIN / scaleFactor;
			}
			landscapeTopCenterButton.Content =
				CreateButtonImage(scaledPageHeight, scaledPageWidth, leftOffset, topOffset,
				                 landscapeCardsWide, landscapeCardsHigh);
			topOffset = (scaledPageWidth - scaledCardHeight * landscapeCardsHigh) / 2;
			if(QslCard.CardPrintProperties.InsideMargins &&
			   topOffset < MAXMARGIN / scaleFactor)
			{
				topOffset = MAXMARGIN / scaleFactor;
			}
			landscapeCenterButton.Content =
				CreateButtonImage(scaledPageHeight, scaledPageWidth, leftOffset, topOffset,
				                 landscapeCardsWide, landscapeCardsHigh);
		}
		
		/// <summary>
		/// Create an individual image for a radio button
		/// </summary>
		/// <param name="sPageWidth">Scaled page width to create the image for</param>
		/// <param name="sPageHeight">Scaled page height to create the image for</param>
		/// <param name="leftOffset">Scaled card left offset</param>
		/// <param name="topOffset">Scaled card top offset</param>
		/// <returns>Canvas object containing the image</returns>
		private Canvas CreateButtonImage(int sPageWidth, int sPageHeight, int leftOffset, 
		                                 int topOffset, int cardsWide, int cardsHigh)
		{
			Canvas canvas = new Canvas();
			// create a Path for the page
			RectangleGeometry rect = new RectangleGeometry(
				new Rect(0, 0, sPageWidth, sPageHeight));
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
			for(int hCard = 0; hCard < cardsWide; hCard++)
			{
				for(int vCard = 0; vCard < cardsHigh; vCard++)
				{
					geo.Children.Add(new RectangleGeometry(
						new Rect(hCard*scaledCardWidth + leftOffset, 
						         vCard*scaledCardHeight + topOffset,
						         scaledCardWidth, scaledCardHeight)));
				}
			}
			cardsPath.Data = geo;
			canvas.Children.Add(cardsPath);
			return canvas;
		}
		
		/// <summary>
		/// Handler for portraitTopLeftButton Checked event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void PortraitTopLeftButton_Checked(object sender, RoutedEventArgs e)
		{
			QslCard.CardPrintProperties.Layout = PrintProperties.CardLayouts.PortraitTopLeft;
			if(App.Logger.DebugPrinting)
			{
				App.Logger.Log("Cards layout set to PortraitTopLeft" +
				               Environment.NewLine);
			}
		}
		
		/// <summary>
		/// Handler for portraitTopCenterButton Checked event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void PortraitTopCenterButton_Checked(object sender, RoutedEventArgs e)
		{
			QslCard.CardPrintProperties.Layout = PrintProperties.CardLayouts.PortraitTopCenter;
			if(App.Logger.DebugPrinting)
			{
				App.Logger.Log("Cards layout set to PortraitTopCenter" +
				               Environment.NewLine);
			}
		}
		
		/// <summary>
		/// Handler for portraitCenterButton Checked event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void PortraitCenterButton_Checked(object sender, RoutedEventArgs e)
		{
			QslCard.CardPrintProperties.Layout = PrintProperties.CardLayouts.PortraitCenter;
			if(App.Logger.DebugPrinting)
			{
				App.Logger.Log("Cards layout set to PortraitCenter" +
				               Environment.NewLine);
			}
		}
		
		/// <summary>
		/// Handler for landscapeTopLeftButton Checked event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void LandscapeTopLeftButton_Checked(object sender, RoutedEventArgs e)
		{
			QslCard.CardPrintProperties.Layout = PrintProperties.CardLayouts.LandscapeTopLeft;
			if(App.Logger.DebugPrinting)
			{
				App.Logger.Log("Cards layout set to LandscapeTopLeft" +
				               Environment.NewLine);
			}
		}
		
		/// <summary>
		/// Handler for landscapeTopCenter Checked event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void LandscapeTopCenterButton_Checked(object sender, RoutedEventArgs e)
		{
			QslCard.CardPrintProperties.Layout = PrintProperties.CardLayouts.LandscapeTopCenter;
			if(App.Logger.DebugPrinting)
			{
				App.Logger.Log("Cards layout set to LandscapeTopCenter" +
				               Environment.NewLine);
			}
		}
		
		/// <summary>
		/// Handler for landscapeCenterButton Checked event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void LandscapeCenterButton_Checked(object sender, RoutedEventArgs e)
		{
			QslCard.CardPrintProperties.Layout = PrintProperties.CardLayouts.LandscapeCenter;
			if(App.Logger.DebugPrinting)
			{
				App.Logger.Log("Cards layout set to LandscapeCenter" +
				               Environment.NewLine);
			}
		}
		
	}
}