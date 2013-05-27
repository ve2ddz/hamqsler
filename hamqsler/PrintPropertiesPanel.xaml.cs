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
using System.Windows.Shapes;

namespace hamqsler
{
	/// <summary>
	/// Interaction logic for PrintPropertiesPanel.xaml
	/// </summary>
	public partial class PrintPropertiesPanel : UserControl
	{
		private static readonly DependencyProperty PrinterNameProperty =
			DependencyProperty.Register("PrinterName", typeof(string), 
			                            typeof(CardPropertiesGroupBox),
			                            new PropertyMetadata(string.Empty));
		public string PrinterName
		{
			get {return GetValue(PrinterNameProperty) as string;}
			set {SetValue(PrinterNameProperty, value);}
		}
		
		private static readonly DependencyProperty PrinterPaperSizeProperty =
			DependencyProperty.Register("PrinterPaperSize", typeof(PaperSize),
			                            typeof(PrintPropertiesPanel),
			                            new PropertyMetadata(null));
		public PaperSize PrinterPaperSize
		{
			get {return GetValue(PrinterPaperSizeProperty) as PaperSize;}
			set {SetValue(PrinterPaperSizeProperty, value);}
		}
		
		private static readonly DependencyProperty PrinterResolutionProperty =
			DependencyProperty.Register("Resolution", typeof(PrinterResolution),
			                            typeof(PrintPropertiesPanel),
			                            new PropertyMetadata(null));
		public PrinterResolution Resolution
		{
			get {return GetValue(PrinterResolutionProperty) as PrinterResolution;}
			set {SetValue(PrinterResolutionProperty, value);}
		}
		
		private static readonly DependencyProperty InsideMarginsProperty =
			DependencyProperty.Register("InsideMargins", typeof(bool),
			                            typeof(PrintPropertiesPanel),
			                            new PropertyMetadata(false));
		public bool InsideMargins
		{
			get {return (bool)GetValue(InsideMarginsProperty);}
			set {SetValue(InsideMarginsProperty, value);}
		}
		
		private static readonly DependencyProperty PrintCardOutlinesProperty =
			DependencyProperty.Register("PrintCardOutlines", typeof(bool),
			                            typeof(PrintPropertiesPanel),
			                            new PropertyMetadata(false));
		public bool PrintCardOutlines
		{
			get {return (bool)GetValue(PrintCardOutlinesProperty);}
			set {SetValue(PrintCardOutlinesProperty, value);}
		}
		
		private static readonly DependencyProperty FillLastPageProperty =
			DependencyProperty.Register("FillLastPage", typeof(bool),
			                            typeof(PrintPropertiesPanel),
			                            new PropertyMetadata(false));
		public bool FillLastPage
		{
			get {return (bool)GetValue(FillLastPageProperty);}
			set {SetValue(FillLastPageProperty, value);}
		}
		
		private static readonly DependencyProperty SetCardMarginsProperty =
			DependencyProperty.Register("SetCardMargins", typeof(bool),
			                            typeof(PrintPropertiesPanel),
			                            new PropertyMetadata(false));
		public bool SetCardMargins
		{
			get {return(bool) GetValue(SetCardMarginsProperty);}
			set {SetValue(SetCardMarginsProperty, value);}
		}
		
		private static readonly DependencyProperty LayoutProperty =
			DependencyProperty.Register("Layout", typeof(PrintProperties.CardLayouts),
			                            typeof(PrintPropertiesPanel),
			                            new PropertyMetadata(PrintProperties.CardLayouts.None));
		public PrintProperties.CardLayouts Layout
		{
			get {return (PrintProperties.CardLayouts)GetValue(LayoutProperty);}
			set {SetValue(LayoutProperty, value);}
		}
		
		private static readonly DependencyProperty CardWidthProperty =
			DependencyProperty.Register("CardWidth", typeof(int), 
			                            typeof(PrintPropertiesPanel),
			                            new PropertyMetadata(1));
		public int CardWidth
		{
			get {return (int)GetValue(CardWidthProperty);}
			set {SetValue(CardWidthProperty, value);}
		}
		
		private static readonly DependencyProperty CardHeightProperty =
			DependencyProperty.Register("CardHeight", typeof(int),
			                            typeof(PrintPropertiesPanel),
			                            new PropertyMetadata(1));
		public int CardHeight
		{
			get {return (int)GetValue(CardHeightProperty);}
			set {SetValue(CardHeightProperty, value);}
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
		
		private PrinterSettings settings;		// delegate and event handler for Properties changed
		public delegate void PrintPropertiesChangedEventHandler(
			object sender, EventArgs e);
		public event PrintPropertiesChangedEventHandler PrintPropertiesChanged;
		
		/// <summary>
		/// Constructor
		/// </summary>
		public PrintPropertiesPanel()
		{
			InitializeComponent();
			DataContext = this;
			LoadInstalledPrinterNames();
		}
		
		/// <summary>
		/// Load printerComboBox with the list of installed printers
		/// </summary>
		private void LoadInstalledPrinterNames()
		{
			// initialize printerComboBox with the list of installed printers
			foreach(string printer in PrinterSettings.InstalledPrinters)
			{
				printerComboBox.Items.Add(printer);
			}
		}
		
		/// <summary>
		/// Processes DependencyProperty changes
		/// </summary>
		/// <param name="e">DependencyPropertyChangedEventArgs object</param>
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if(e.Property == PrinterNameProperty)
			{
				settings = new PrinterSettings();
				settings.PrinterName = PrinterName;
				SetPaperSizes(settings);
				SetResolutions(settings);
				settings.DefaultPageSettings.PaperSize = PrinterPaperSize;
				settings.DefaultPageSettings.PrinterResolution = Resolution;
				insideMarginsButton.IsEnabled = true;
				setCardMarginsButton.IsEnabled = true;
				// if the page margins are greater than ¼ inch, then force InsideMargins
				// to be set and set card margins to be cleared
				RectangleF area = settings.DefaultPageSettings.PrintableArea;
				bool insideMargins = InsideMargins;
				if(App.Logger.DebugPrinting)
				{
					string info = 
						string.Format("PrintPropertiesPanel.SetCardsLayouts:" +
					              Environment.NewLine +
					              "\tPapersize = {0} x {1}" +
					              Environment.NewLine +
					              "\tPrintableArea = {2}, {3}: {4} x {5}" +
					              Environment.NewLine +
					              "\tInsideMargins original value = {6}" +
					              Environment.NewLine,
					              PrinterPaperSize.Width,
					              PrinterPaperSize.Height,
					              area.Left, area.Top, area.Width, area.Height,
					             insideMargins);
					App.Logger.Log(info);
				}
				InsideMargins = InsideMargins ||
				area.X > MAXMARGIN ||
				settings.DefaultPageSettings.PaperSize.Width - area.X -
				area.Width > MAXMARGIN ||
				area.Y > MAXMARGIN ||
				settings.DefaultPageSettings.PaperSize.Height - area.Y -
				area.Height > MAXMARGIN;
				if(insideMargins != InsideMargins)
				{
					insideMarginsButton.IsEnabled = false;
					setCardMarginsButton.IsChecked = false;
					setCardMarginsButton.IsEnabled = false;
					if(App.Logger.DebugPrinting)
					{
						App.Logger.Log(string.Format("PrintPropertiesPanel.SetCardsLayouts:" +
						              Environment.NewLine +
						              "\tInsideMargins changed to {0}" +
						              Environment.NewLine,
						              InsideMargins));
					}
				}
			}
			else if(e.Property == PrinterPaperSizeProperty)
			{
				paperSizeComboBox.SelectedItem = PrinterPaperSize.PaperName;
				if(paperSizeComboBox.SelectedIndex == -1)
				{
					paperSizeComboBox.SelectedItem = settings.DefaultPageSettings.PaperSize.PaperName;
				}
				SetCardsLayouts();
			}
			else if(e.Property == PrinterResolutionProperty)
			{
				qualityComboBox.SelectedItem = ResolutionString(Resolution);
				if(qualityComboBox.SelectedIndex == -1)
				{
					PrinterSettings settings = new PrinterSettings();
					settings.PrinterName = PrinterName;
					qualityComboBox.SelectedItem = 
						ResolutionString(settings.DefaultPageSettings.PrinterResolution);
				}
			}
			else if(e.Property == InsideMarginsProperty)
			{
				SetCardsLayouts();
			}
			else if(e.Property == CardWidthProperty)
			{
				SetCardsLayouts();
			}
			else if(e.Property == CardHeightProperty)
			{
				SetCardsLayouts();
			}
			else if(e.Property == LayoutProperty)
			{
				SetCardLayout();
			}
			if(e.Property == PrinterNameProperty ||
			   e.Property == PrinterPaperSizeProperty ||
			   e.Property == PrinterResolutionProperty ||
			   e.Property == InsideMarginsProperty ||
			   e.Property == PrintCardOutlinesProperty ||
			   e.Property == FillLastPageProperty ||
			   e.Property == SetCardMarginsProperty ||
			   e.Property == LayoutProperty)
			{
				if(PrintPropertiesChanged != null)
				{
					PrintPropertiesChanged(this, new EventArgs());
				}
			}
		}
		
		/// <summary>
		/// Populate paperSizeComboBox with the list of PaperSizes for the selected printer
		/// and select the PrinterPaperSize, or the default paper size for the printer if
		/// PrinterPaperSize is not valid for the selected printer
		/// </summary>
		/// <param name="settings">PrinterSettings object for the selected printer</param>
		private void SetPaperSizes(PrinterSettings settings)
		{
			paperSizeComboBox.Items.Clear();
			foreach(PaperSize size in settings.PaperSizes)
			{
				paperSizeComboBox.Items.Add(size.PaperName);
			}
			if(PrinterPaperSize != null)
			{
				paperSizeComboBox.SelectedItem = PrinterPaperSize.PaperName;
			}
			if(paperSizeComboBox.SelectedIndex == -1)
			{
				paperSizeComboBox.SelectedItem = settings.DefaultPageSettings.PaperSize.PaperName;
			}
		}
		
		/// <summary>
		/// Handles paperSizeComboBox SelectionChanged event processing.
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e"SelectionChangedEventArgs object</param>
		void PaperSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			PrinterSettings settings = new PrinterSettings();
			settings.PrinterName = PrinterName;
			if(e.AddedItems.Count > 0)
			{
				foreach(PaperSize size in settings.PaperSizes)
				{
					if(size.PaperName.Equals(e.AddedItems[0]))
					{
						PrinterPaperSize = size;
						if(App.Logger.DebugPrinting)
						{
							App.Logger.Log("PaperSizeComboBox_SelectionChanged:" +
							               Environment.NewLine +
							               "PaperSize changed to " + PrinterPaperSize +
							               Environment.NewLine);
						}
						break;
					}
				}
			}
		}
		
		/// <summary>
		/// Populate qualityComboBox with list of PrinterResolutions that are valid for the selected
		/// printer and select the PrinterResolution, or default resolution  if PrinterResolution 
		/// is not valid for the selected printer.
		/// </summary>
		/// <param name="settings">PrinterSettings object for the selected printer</param>
		private void SetResolutions(PrinterSettings settings)
		{
			qualityComboBox.Items.Clear();
			foreach(PrinterResolution res in settings.PrinterResolutions)
			{
				qualityComboBox.Items.Add(ResolutionString(res));
			}
			if(Resolution != null)
			{
				qualityComboBox.SelectedItem = ResolutionString(Resolution);
			}
			if(qualityComboBox.SelectedIndex == -1)
			{
				qualityComboBox.SelectedItem = 
					ResolutionString(settings.DefaultPageSettings.PrinterResolution);
			}
		}

		/// <summary>
		/// Helper method that creates string representation of the PrinterResolution
		/// </summary>
		/// <param name="res">PrinterResolution object to create string representation of</param>
		/// <returns>String containing resolution</returns>
		private string ResolutionString(PrinterResolution res)
		{
			string resolution = res.Kind.ToString();
			if (resolution.Equals("Custom")) 
			{
				resolution = string.Format("{0} ({1} x {2})", res.Kind, res.X, res.Y);
			}
			return resolution;
		}
		
		/// <summary>
		/// Handler for qualityComboBox SelectionChanged event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">SelectionChangedEventArgs object</param>
		void QualityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			PrinterSettings settings = new PrinterSettings();
			settings.PrinterName = PrinterName;
			if(e.AddedItems.Count > 0)
			{
				foreach(PrinterResolution res in settings.PrinterResolutions)
				{
					if(e.AddedItems[0].Equals(ResolutionString(res)))
					{
						Resolution = res;
						if(App.Logger.DebugPrinting)
						{
							App.Logger.Log("QualityComboBox_SelectionChanged:" +
							               Environment.NewLine +
							               "Resolution changed to " + ResolutionString(Resolution) +
							               Environment.NewLine);
						}
						break;
					}
				}
			}
		}
		
		/// <summary>
		/// Helper method that selects the alignment radio box based on Layout setting
		/// of QslCard CardPrintProperties
		/// </summary>
		private void SetCardLayout()
		{
			portraitTopLeftButton.IsChecked = 
				(Layout ==  PrintProperties.CardLayouts.PortraitTopLeft);
			portraitTopCenterButton.IsChecked = 
				(Layout == PrintProperties.CardLayouts.PortraitTopCenter);
			portraitCenterButton.IsChecked = 
				(Layout == PrintProperties.CardLayouts.PortraitCenter);
			landscapeTopLeftButton.IsChecked = 
				(Layout == PrintProperties.CardLayouts.LandscapeTopLeft);
			landscapeTopCenterButton.IsChecked = 
				(Layout == PrintProperties.CardLayouts.LandscapeTopCenter);
			landscapeCenterButton.IsChecked = 
				(Layout == PrintProperties.CardLayouts.LandscapeCenter) ||
				(Layout == PrintProperties.CardLayouts.None);
		}
		
		/// <summary>
		/// Creates and displays the images for the layout radio buttons
		/// </summary>
		public void SetCardsLayouts()
		{
			if(PrinterPaperSize != null && CardWidth != 1 && CardHeight != 1)
			{
				CalculateScaledPaperAndCardSizes();
				// determine the number of cards that can be printed on portrait
				// and landscape pages
				CalculateCardsPerPortraitPage();
				CalculateCardsPerLandscapePage();
				// create the images
				CreatePortraitButtonImages();
				CreateLandscapeButtonImages();
				bool portraitButtonsVisible = portraitCardsWide > 0 && portraitCardsHigh > 0;
				bool landscapeButtonsVisible = landscapeCardsWide > 0 && landscapeCardsHigh > 0;
				if(!portraitButtonsVisible)
				{
					switch(Layout)
					{
						case PrintProperties.CardLayouts.PortraitTopLeft:
						case PrintProperties.CardLayouts.PortraitTopCenter:
						case PrintProperties.CardLayouts.PortraitCenter:
							Layout = PrintProperties.CardLayouts.LandscapeTopLeft;
							break;
					}
				}
				if(!landscapeButtonsVisible)
				{
					switch(Layout)
					{
						case PrintProperties.CardLayouts.LandscapeTopLeft:
						case PrintProperties.CardLayouts.LandscapeTopCenter:
						case PrintProperties.CardLayouts.LandscapeCenter:
							if(portraitButtonsVisible)
							{
								Layout = PrintProperties.CardLayouts.PortraitTopLeft;
							}
							else
							{
								Layout = PrintProperties.CardLayouts.None;
							}
							break;
					}
				}
			}
		}
		
		/// <summary>
		/// Helper method that calculatees the scaled paper and card sizes.
		/// Paper is scaled so that the longer side is 100 pixels long
		/// </summary>
		private void CalculateScaledPaperAndCardSizes()
		{
			int pageWidth = PrinterPaperSize.Width;
			int pageHeight = PrinterPaperSize.Height;
			scaleFactor = SCALEFACTOR / pageHeight;
			scaledPageWidth = (pageWidth * scaleFactor) / SCALEDPAGESIZE;
			scaledPageHeight = (pageHeight * scaleFactor) / SCALEDPAGESIZE;
			scaledCardWidth = (CardWidth * scaleFactor) / SCALEDPAGESIZE;
			scaledCardHeight = (CardHeight * scaleFactor) / SCALEDPAGESIZE;
			if(App.Logger.DebugPrinting)
			{
				string info =
					string.Format("PrintPropertiesPanel.CalculateScaledPaperAndCardSizes:" +
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
			int pageWidth = PrinterPaperSize.Width;
			int pageHeight = PrinterPaperSize.Height;
			if(InsideMargins)
			{
				settings.DefaultPageSettings.PaperSize = PrinterPaperSize;
				RectangleF area = settings.DefaultPageSettings.PrintableArea;
				pageWidth = (int)(area.Width - area.Left);
				pageHeight = (int)(area.Height - area.Top);
			}
			portraitCardsWide = pageWidth / CardWidth;
			portraitCardsHigh = pageHeight / CardHeight;
			if(App.Logger.DebugPrinting)
			{
				string info = 
					string.Format("PrintPropertiesPanel.CalculateCardsPerPortraitPage:" +
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
					              pageWidth, pageHeight, CardWidth, CardHeight,
					              portraitCardsWide, portraitCardsHigh,
					              InsideMargins);
				App.Logger.Log(info);
			}
		}
		
		/// <summary>
		/// Helper method that calculates the number of cards wide and high that can
		/// be printed on the selected page size in landscape orientation
		/// </summary>
		private void CalculateCardsPerLandscapePage()
		{
			int pageWidth = PrinterPaperSize.Height;
			int pageHeight = PrinterPaperSize.Width;
			if(InsideMargins)
			{
				RectangleF area = settings.DefaultPageSettings.PrintableArea;
				pageWidth = (int)(area.Height - area.Top);
				pageHeight = (int)(area.Width - area.Left);
			}
			landscapeCardsWide = pageWidth / CardWidth;
			landscapeCardsHigh = pageHeight / CardHeight;
			if(App.Logger.DebugPrinting)
			{
				string info = 
					string.Format("PrintPropertiesPanel.CalculateCardsPerLandscapePage:" +
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
					              pageWidth, pageHeight, CardWidth, CardHeight,
					              landscapeCardsWide, landscapeCardsHigh,
					              InsideMargins);
				App.Logger.Log(info);
			}
		}
		
		/// <summary>
		/// Create the images for the portrait radio buttons
		/// </summary>
		private void CreatePortraitButtonImages()
		{
			Visibility buttonsVisibility = portraitCardsWide > 0 && portraitCardsHigh > 0 ?
				Visibility.Visible : Visibility.Collapsed;
			portraitTopLeftButton.Visibility = buttonsVisibility;
			portraitTopCenterButton.Visibility = buttonsVisibility;
			portraitCenterButton.Visibility = buttonsVisibility;
			if(buttonsVisibility == Visibility.Visible)
			{
				RectangleF area = settings.DefaultPageSettings.PrintableArea;
				int leftOffset = 0;
				int topOffset = 0;
				if(InsideMargins)
				{
					leftOffset = (int)area.Left / scaleFactor;
					topOffset = (int)area.Top / scaleFactor;				
				}
				CreateButtonImage(portraitTopLeftButtonCanvas, scaledPageWidth, scaledPageHeight,
			 	                  leftOffset, topOffset, portraitCardsWide, portraitCardsHigh);
				leftOffset = (scaledPageWidth - scaledCardWidth * portraitCardsWide) / 2;
				if(InsideMargins &&
				   leftOffset < (MAXMARGIN / scaleFactor))
				{
					leftOffset = (MAXMARGIN / scaleFactor);
				}
				CreateButtonImage(portraitTopCenterButtonCanvas, scaledPageWidth, scaledPageHeight,
			 	                  leftOffset, topOffset, portraitCardsWide, portraitCardsHigh);
				topOffset = (scaledPageHeight - scaledCardHeight * portraitCardsHigh) / 2;
				if(InsideMargins &&
				   topOffset < (MAXMARGIN / scaleFactor))
				{
					topOffset = (MAXMARGIN / scaleFactor);
				}
				CreateButtonImage(portraitCenterButtonCanvas, scaledPageWidth, scaledPageHeight,
			 	                  leftOffset, topOffset, portraitCardsWide, portraitCardsHigh);
			}
		}
		
		/// <summary>
		/// Create the images for the landscape radio buttons
		/// </summary>
		private void CreateLandscapeButtonImages()
		{
			Visibility buttonsVisibility = landscapeCardsWide > 0 && landscapeCardsHigh > 0
				? Visibility.Visible : Visibility.Collapsed;
			landscapeTopLeftButton.Visibility = buttonsVisibility;
			landscapeTopCenterButton.Visibility = buttonsVisibility;
			landscapeCenterButton.Visibility = buttonsVisibility;
			if(buttonsVisibility == Visibility.Visible)
			{
				RectangleF area = settings.DefaultPageSettings.PrintableArea;
				int leftOffset = 0;
				int topOffset = 0;
				if(InsideMargins)
				{
					leftOffset = (int)area.Top / scaleFactor;
					topOffset = (int)area.Left / scaleFactor;
				}
				CreateButtonImage(landscapeTopLeftButtonCanvas, scaledPageHeight, scaledPageWidth,
			 	                  leftOffset, topOffset, landscapeCardsWide, landscapeCardsHigh);
				leftOffset = (scaledPageHeight - scaledCardWidth * landscapeCardsWide) / 2;
				if(InsideMargins &&
				   leftOffset < MAXMARGIN / scaleFactor)
				{
					leftOffset = MAXMARGIN / scaleFactor;
				}
				CreateButtonImage(landscapeTopCenterButtonCanvas, scaledPageHeight, scaledPageWidth,
			 	                  leftOffset, topOffset, landscapeCardsWide, landscapeCardsHigh);
				topOffset = (scaledPageWidth - scaledCardHeight * landscapeCardsHigh) / 2;
				if(InsideMargins &&
				   topOffset < MAXMARGIN / scaleFactor)
				{
					topOffset = MAXMARGIN / scaleFactor;
				}
				CreateButtonImage(landscapeCenterButtonCanvas, scaledPageHeight, scaledPageWidth,
			 	                  leftOffset, topOffset, landscapeCardsWide, landscapeCardsHigh);
			}
		}
		
		/// <summary>
		/// Create an individual image for a radio button
		/// </summary>
		/// <param name="sPageWidth">Scaled page width to create the image for</param>
		/// <param name="sPageHeight">Scaled page height to create the image for</param>
		/// <param name="leftOffset">Scaled card left offset</param>
		/// <param name="topOffset">Scaled card top offset</param>
		/// <returns>Canvas object containing the image</returns>
		private void CreateButtonImage(Canvas canvas, int sPageWidth, int sPageHeight, 
		                               int leftOffset, int topOffset, int cardsWide, int cardsHigh)
		{
			
			canvas.Children.Clear();
			// create a Path for the page
			int pageOffsetX = 0;
			int pageOffsetY = (int)((canvas.Height - sPageHeight) / 2);
			RectangleGeometry rect = new RectangleGeometry(
				new Rect(pageOffsetX, pageOffsetY, sPageWidth, sPageHeight));
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
						new Rect(hCard*scaledCardWidth + leftOffset + pageOffsetX, 
						         vCard*scaledCardHeight + topOffset + pageOffsetY,
						         scaledCardWidth, scaledCardHeight)));
				}
			}
			cardsPath.Data = geo;
			canvas.Children.Add(cardsPath);
		}

		/// <summary>
		/// Handler for portraitTopLeftButton Checked event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void PortraitTopLeftButton_Checked(object sender, RoutedEventArgs e)
		{
			Layout = PrintProperties.CardLayouts.PortraitTopLeft;
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
			Layout = PrintProperties.CardLayouts.PortraitTopCenter;
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
			Layout = PrintProperties.CardLayouts.PortraitCenter;
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
			Layout = PrintProperties.CardLayouts.LandscapeTopLeft;
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
			Layout = PrintProperties.CardLayouts.LandscapeTopCenter;
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
			Layout = PrintProperties.CardLayouts.LandscapeCenter;
			if(App.Logger.DebugPrinting)
			{
				App.Logger.Log("Cards layout set to LandscapeCenter" +
				               Environment.NewLine);
			}
		}
		
	}
}