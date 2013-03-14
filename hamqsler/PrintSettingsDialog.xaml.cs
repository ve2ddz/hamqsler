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
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Printing;

namespace hamqsler
{
	/// <summary>
	/// Interaction logic for PrintSettingsDialog.xaml
	/// </summary>
	public partial class PrintSettingsDialog : Window
	{
		
		private bool portraitCardMargins;
		private bool landscapeCardMargins;

		public enum PrintButtonTypes { Print, Preview };

		private PrintButtonTypes printType = PrintButtonTypes.Print;

		public PrintButtonTypes PrintType
		{
			get { return printType; }
			set { printType = value; }
		}

		public enum CardLayout { PortraitEdge, PortraitTopCentre, PortraitCentre, LandscapeEdge, LandscapeTopCentre, LandscapeCentre, None };

		private static DependencyProperty CardsLayoutProperty =
			DependencyProperty.Register("CardsLayout", typeof(CardLayout),
			                            typeof(PrintSettingsDialog), new PropertyMetadata(CardLayout.None));

		public CardLayout CardsLayout
		{
			get { return (CardLayout)GetValue(CardsLayoutProperty); }
			set { SetValue(CardsLayoutProperty, value); }
		}

		private static DependencyProperty CardOutlineProperty =
			DependencyProperty.Register("CardOutline", typeof(bool),
			                            typeof(PrintSettingsDialog), new PropertyMetadata(true));

		public bool CardOutline
		{
			get { return (bool)GetValue(CardOutlineProperty); }
			set { SetValue(CardOutlineProperty, value); }
		}

		private static DependencyProperty FillLastPageProperty =
			DependencyProperty.Register("FillLastPage", typeof(bool),
			                            typeof(PrintSettingsDialog), new PropertyMetadata(true));

		public bool FillLastPage
		{
			get { return (bool)GetValue(FillLastPageProperty); }
			set { SetValue(FillLastPageProperty, value); }
		}

		private static DependencyProperty CardMarginsProperty =
			DependencyProperty.Register("CardMargins", typeof(bool),
			                            typeof(PrintSettingsDialog), new PropertyMetadata(false));

		public bool CardMargins
		{
			get { return (bool)GetValue(CardMarginsProperty); }
			set { SetValue(CardMarginsProperty, value); }
		}

		/// <summary>
		/// Private constructor - use the factory method:CreatePrintSettingsDialog instead
		/// </summary>
		private PrintSettingsDialog()
		{
			InitializeComponent();
			portraitCardMargins = false;
			landscapeCardMargins = false;
		}
		
		/// <summary>
		/// Factory method for creating a PrintSettingsDialog object. Creates the PrintSettingsDialog
		/// and draws representations of the placements of cards on the paper
		/// </summary>
		/// <param name="ticket">PrintTicket object containing paper and printer settings</param>
		/// <param name="card">Card object describing the cards to be printed</param>
		public static PrintSettingsDialog CreatePrintSettingsDialog(PrintTicket ticket, PrintQueue queue,
		                                                            Card card)
		{
			PrintSettingsDialog psD = new PrintSettingsDialog();
			// Some printers (some Brother printers at least), do not report a PageImageableArea,
			// so it is not possible to determine card margins from printer margins.
			// In this case, show a message indicating this, and do not allow user
			// to set the SetCardMarginsCheckBox.
			PrintCapabilities caps = queue.GetPrintCapabilities();
			PageImageableArea imageableArea = caps.PageImageableArea;
			if(imageableArea == null)
			{
				UserPreferences prefs = ((App)Application.Current).UserPreferences;
				psD.CardMargins = false;
				psD.SetCardMarginsCheckBox.IsEnabled = false;
				// check to see if printer is in list of "Do not show again for this printer"
				// and show or don't show message
				string printer = queue.Name;
				bool printerFound = false;
				foreach(string pr in prefs.DoNotShowNullImageableAreaMessagePrinters)
				{
					if(printer == pr)
					{
						printerFound = true;
						break;
					}
				}
				// printer not in do not show list, so show the dialog
				if(!printerFound)
				{
					PrinterImageableAreaErrorDialog pDialog = new PrinterImageableAreaErrorDialog();
					pDialog.ShowDialog();
					// if do not show again checkbox checked, add printer to the do not show list
					if(pDialog.DoNotShowForThisPrinter)
					{
						prefs.DoNotShowNullImageableAreaMessagePrinters.Add(printer);
						prefs.SerializeAsXml();
					}
				}
			}
			// cards aligned to edge on portrait orientation
			Grid grid = psD.CreatePortraitButtonContent(ticket, card, CardLayout.PortraitEdge);
			SetButtonContentAndVisibility(ref psD.portraitEdgeButton, ref grid);
			
			// cards top-centred on portrait orientation
			grid = psD.CreatePortraitButtonContent(ticket, card, CardLayout.PortraitTopCentre);
			SetButtonContentAndVisibility(ref psD.portraitTopCentreButton, ref grid);
			
			// cards centred on portrait orientation
			grid = psD.CreatePortraitButtonContent(ticket, card, CardLayout.PortraitCentre);
			SetButtonContentAndVisibility(ref psD.portraitCentreButton, ref grid);
			
			// cards aligned to edge on landscape orientation
			grid = psD.CreateLandscapeButtonContent(ticket, card, CardLayout.LandscapeEdge);
			SetButtonContentAndVisibility(ref psD.landscapeEdgeButton, ref grid);
			
			// cards top centred on landscape orientation
			grid = psD.CreateLandscapeButtonContent(ticket, card, CardLayout.LandscapeTopCentre);
			SetButtonContentAndVisibility(ref psD.landscapeTopCentreButton, ref grid);
			
			// cards centred on landscape orientation
			grid = psD.CreateLandscapeButtonContent(ticket, card, CardLayout.LandscapeCentre);
			SetButtonContentAndVisibility(ref psD.landscapeCentreButton, ref grid);
			
			// determine which button to check
			psD.CheckLastVisibleButton();
			
			// generate cardstock tooltips for the radio buttons
			try
			{
				PageMediaSizeName pmsName = (PageMediaSizeName)ticket.PageMediaSize.PageMediaSizeName;
				GenerateToolTips(psD, pmsName, card.GetType().Name);
			}
			catch(InvalidOperationException) {} // throw if PageMediaSizeName not set
			
			return psD;
		}

		/// <summary>
		/// Check the last visible radio button on the PrintSettingsDialog
		/// </summary>
		private void CheckLastVisibleButton()
		{
			if (landscapeCentreButton.Visibility == Visibility.Visible) {
				landscapeCentreButton.IsChecked = true;
			} else if (landscapeTopCentreButton.Visibility == Visibility.Visible) {
				landscapeEdgeButton.IsChecked = true;
			} else if (landscapeEdgeButton.Visibility == Visibility.Visible) {
				landscapeEdgeButton.IsChecked = true;
			} else if (portraitCentreButton.Visibility == Visibility.Visible) {
				portraitCentreButton.IsChecked = true;
			} else if (portraitTopCentreButton.Visibility == Visibility.Visible) {
				portraitCentreButton.IsChecked = true;
			} else {
				portraitEdgeButton.IsChecked = true;
			}
		}


		/// <summary>
		/// sets the button content and visibility
		/// </summary>
		/// <param name="button">button to operate one</param>
		/// <param name="path">Path object for the content of the button</param>
		private static void SetButtonContentAndVisibility(ref RadioButton button, ref Grid grid)
		{
			if (grid != null) {
				button.Content = grid;
				button.Visibility = Visibility.Visible;
			} else {
				button.Visibility = Visibility.Collapsed;
			}
		}
		
		/// <summary>
		/// Handler for checking one of the layout radio buttons
		/// </summary>
		/// <param name="sender">Radio button that was checked</param>
		/// <param name="e">not used</param>
		
		private void LayoutRadioButton_Checked(object sender, RoutedEventArgs e)
		{
			// check the button and set the CardsLayout property accordingly
			if ((RadioButton)sender == portraitEdgeButton)
			{
				CardsLayout = CardLayout.PortraitEdge;
				if(SetCardMarginsCheckBox.IsEnabled)
				{
					CardMargins = true;
				}
			}
			else if ((RadioButton)sender == portraitTopCentreButton)
			{
				CardsLayout = CardLayout.PortraitTopCentre;
				if(SetCardMarginsCheckBox.IsEnabled)
				{
					CardMargins = portraitCardMargins;
				}
			}
			else if ((RadioButton)sender == portraitCentreButton)
			{
				CardsLayout = CardLayout.PortraitCentre;
				if(SetCardMarginsCheckBox.IsEnabled)
				{
					CardMargins = portraitCardMargins;
				}
			}
			else if ((RadioButton)sender == landscapeEdgeButton)
			{
				CardsLayout = CardLayout.LandscapeEdge;
				if(SetCardMarginsCheckBox.IsEnabled)
				{
					CardMargins = true;
				}
			}
			else if ((RadioButton)sender == landscapeTopCentreButton)
			{
				CardsLayout = CardLayout.LandscapeTopCentre;
				if(SetCardMarginsCheckBox.IsEnabled)
				{
					CardMargins = landscapeCardMargins;
				}
			}
			else if ((RadioButton)sender == landscapeCentreButton)
			{
				CardsLayout = CardLayout.LandscapeCentre;
				if(SetCardMarginsCheckBox.IsEnabled)
				{
					CardMargins = landscapeCardMargins;
				}
			}
		}

		private void PrintButton_Click(object sender, RoutedEventArgs e)
		{
			PrintType = PrintButtonTypes.Print;
			DialogResult = true;
		}

		private void PrintPreviewButton_Click(object sender, RoutedEventArgs e)
		{
			PrintType = PrintButtonTypes.Preview;
			DialogResult = true;
		}
		/// <summary>
		/// Create a Path object to be used as the content of a radio button illustrating
		/// the layout of cards on a page
		/// </summary>
		/// <param name="ticket">PrintTicket object containing paper and printer settings</param>
		/// <param name="card">Card object describing the card to be printed. Only its dimensions
		/// are used.</param>
		/// <param name="centreCards">Indicates whether the cards should be centred on the paper</param>
		/// <returns>Path object showing the layout of cards on the paper, or null if no cards fit on the paper</returns>
		private Grid CreatePortraitButtonContent(PrintTicket ticket, Card card, CardLayout layout)
		{
			// get card and paper width and height
			double cardWidth = card.DisplayWidth;
			double cardHeight = card.DisplayHeight;
			double pageWidth = (double)ticket.PageMediaSize.Width;
			double pageHeight = (double)ticket.PageMediaSize.Height;
			// if card and paper are same size, then only want to display one button.
			// createLandscapeButtonContent will handle this, so just return null to show no button
			if(((double)Math.Floor(pageWidth/cardWidth) == pageWidth/cardWidth) &&
			   ((double)Math.Floor(pageHeight/cardHeight) == pageHeight/cardHeight)  &&
			   (layout == CardLayout.PortraitCentre || layout == CardLayout.PortraitTopCentre))
			{
				return null;
			}
			if(((double)Math.Floor(pageWidth/cardWidth) == pageWidth/cardWidth ||
			    (double)Math.Floor(pageHeight/cardHeight) == pageHeight/cardHeight))
			{
				portraitCardMargins = true;
			}
			// calculate number of cards that will fit on paper (in both portrait and landscape mode)
			int portraitWidthCards = (int)(pageWidth/cardWidth);
			int portraitHeightCards = (int)(pageHeight/cardHeight);
			double ratio = 100.0/(Math.Max(pageWidth, pageHeight)); // factor to reduce dimensions for the path
			// determine offsets (for centering of cards)
			double centreXOffset = 0;
			double centreYOffset = 0;
			switch(layout)
			{
				case CardLayout.PortraitEdge:
					centreXOffset = 0;
					centreYOffset = 0;
					break;
				case CardLayout.PortraitTopCentre:
					centreXOffset = (pageWidth - cardWidth * portraitWidthCards) *ratio / 2;
					centreYOffset = 0;
					break;
				case CardLayout.PortraitCentre:
					centreXOffset = (pageWidth - cardWidth * portraitWidthCards) *ratio / 2;
					centreYOffset = (pageHeight - cardHeight * portraitHeightCards) * ratio / 2;
					break;
			}
			
			Grid grid = new Grid();
			// create path for the paper
			Path p1 = new Path();
			RectangleGeometry pageGeometry = new RectangleGeometry(new Rect(
				0, 0, pageWidth*ratio, pageHeight*ratio));
			p1.Fill = Brushes.LightPink;
			p1.Stroke = Brushes.LightPink;
			p1.StrokeThickness = 1;
			p1.Data = pageGeometry;
			grid.Children.Add(p1);
			// now create the geometry for the cards
			GeometryGroup geo = new GeometryGroup();
			
			if(portraitWidthCards > 0 && portraitHeightCards > 0) // check that there are cards
			{
				// add the cards
				for(int x = 0; x < portraitWidthCards; x++)
				{
					for(int y = 0; y < portraitHeightCards; y++)
					{
						geo.Children.Add(new RectangleGeometry(new Rect(x*cardWidth*ratio + centreXOffset,
						                                                y*cardHeight*ratio + centreYOffset,
						                                                cardWidth*ratio, cardHeight*ratio)));
					}
				}
				// create and return the Path object
				Path path = new Path();
				path.Stroke = Brushes.Blue;
				path.Fill = Brushes.White;
				path.Data = geo;
				grid.Children.Add(path);
				return grid;
			}
			else			// no cards, so return null
			{
				return null;
			}
			
		}
		
		/// <summary>
		/// Create a Path object to be used as the content of a radio button illustrating
		/// the layout of cards on a page
		/// </summary>
		/// <param name="ticket">PrintTicket object containing paper and printer settings</param>
		/// <param name="card">Card object describing the card to be printed. Only its dimensions
		/// are used.</param>
		/// <param name="centreCards">Indicates whether the cards should be centred on the paper</param>
		/// <returns>Path object showing the layout of cards on the paper, or null if no cards fit on the paper</returns>
		private Grid CreateLandscapeButtonContent(PrintTicket ticket, Card card, CardLayout layout)
		{
			// get card and paper width and height
			double cardWidth = card.DisplayWidth;
			double cardHeight = card.DisplayHeight;
			double pageWidth = (double)ticket.PageMediaSize.Width;
			double pageHeight = (double)ticket.PageMediaSize.Height;
			
			// if cardfit exactly on page, only want to display one button.
			// make that button the one for the edge card by returning null for the centre cards button
			   if(((double)Math.Floor(pageWidth/cardHeight) == pageWidth/cardHeight) &&
			   ((double)Math.Floor(pageHeight/cardWidth) == pageHeight/cardWidth) &&
			   (layout == CardLayout.LandscapeCentre || layout == CardLayout.LandscapeTopCentre))
			{
				return null;
			}
			// if cards print to edge of page, set cardmargins to true;
			if(((double)Math.Floor(pageWidth/cardHeight) == pageWidth/cardHeight) ||
			   ((double)Math.Floor(pageHeight/cardWidth) == pageHeight/cardWidth))
			{
				landscapeCardMargins = true;
			}
			// calculate number of cards that will fit on paper
			int landscapeWidthCards = (int)(pageHeight/cardWidth);
			int landscapeHeightCards = (int)(pageWidth/cardHeight);
			if(landscapeWidthCards > 0 && landscapeHeightCards > 0)
			{
				double ratio = 100.0/(Math.Max(pageWidth, pageHeight)); // factor to reduce dimensions for the path
				// determine offsets (for centering of cards)
				double centreXOffset = 0; //centreCards ?  : 0.0;
				double centreYOffset = 0;  // centreCards ? (pageHeight - landscapeWidthCards * cardWidth) * ratio / 2 : 0.0;
				switch(layout)
				{
					case CardLayout.LandscapeEdge:
						centreXOffset = 0;
						centreYOffset = 0;
						break;
					case CardLayout.LandscapeTopCentre:
						centreXOffset = (pageWidth - landscapeHeightCards * cardHeight) * ratio / 2;
						centreYOffset = 0;
						break;
					case CardLayout.LandscapeCentre:
						centreXOffset = (pageWidth - landscapeHeightCards * cardHeight) * ratio / 2;
						centreYOffset = (pageHeight - landscapeWidthCards * cardWidth) * ratio / 2;
						break;						
				}
				Grid grid = new Grid();
				// create path for the paper
				Path p1 = new Path();
				RectangleGeometry pageGeometry = new RectangleGeometry(new Rect(
					0, 0, pageHeight*ratio, pageWidth*ratio));
				p1.Fill = Brushes.LightPink;
				p1.Stroke = Brushes.LightPink;
				p1.StrokeThickness = 1;
				p1.Data = pageGeometry;
				grid.Children.Add(p1);
					//create geometry for paper and cards
				GeometryGroup geo = new GeometryGroup();
				for(int x = 0; x < landscapeWidthCards; x++)
				{
					for(int y = 0; y < landscapeHeightCards; y++)
					{
						geo.Children.Add(new RectangleGeometry(new Rect(x*cardWidth*ratio + centreYOffset,
						                                                y*cardHeight*ratio + centreXOffset,
						                                                cardWidth*ratio, cardHeight*ratio)));
					}
				}
				//create and return path object
				Path path = new Path();
				path.Stroke = Brushes.Blue;
				path.Fill = Brushes.White;
				path.Data = geo;
				grid.Children.Add(path);
				return grid;
			}
			else
			{
				return null;			// no cards, so return null
			}
		}
		
		/// <summary>
		/// Generate tooltips for the layout buttons
		/// </summary>
		/// <param name="psD">PrintSettingsDialog object containing the buttons</param>
		/// <param name="mName">PageMediaSizeName for the cardstock</param>
		/// <param name="cType">Card type being printed</param>
		private static void GenerateToolTips(PrintSettingsDialog psD, PageMediaSizeName mName, string cType)
		{
			psD.portraitEdgeButton.ToolTip = null;
			psD.portraitCentreButton.ToolTip = null;
			psD.landscapeEdgeButton.ToolTip = null;
			psD.landscapeCentreButton.ToolTip = null;
			
			// for Bureau standard cards
			if(mName == PageMediaSizeName.NorthAmericaLetter &&
			   cType == "LandscapeCard55x35")
			{
				psD.portraitCentreButton.ToolTip = "Use for 3UPPCSTOCK micro-perfed cardstock from yourofficestop.com";
				psD.landscapeEdgeButton.ToolTip = "Use for The QSLKit micro-perfed card stock";
				psD.landscapeCentreButton.ToolTip = "Use for micro-perfed cardstock from QslPaper.com or Avery 3263, 3380, 5689, 8383, 8387, or equivalent";
			}
			// for 4¼ by 5½ inch cards
			else if(mName == PageMediaSizeName.NorthAmericaLetter &&
			        cType == "LandscapeCard55x425")
			{
				psD.landscapeEdgeButton.ToolTip = "Use for Avery postcard 3263, 3380, 5689, 8383, 8387, or equivalent";
			}
			// for 4 by 6 inch cards
			else if(mName == PageMediaSizeName.NorthAmericaLetter &&
			        cType == "LandscapeCard6x4")
				psD.portraitCentreButton.ToolTip = "Use for Avery 5389, 5889, 8386, or equivalent";
		}
	}
}
