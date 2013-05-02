﻿/*
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
using System.Drawing.Printing;
using System.Windows;

namespace hamqsler
{
	/// <summary>
	/// PrintProperties class - holder for print and printer related properties
	/// </summary>
	public class PrintProperties : DependencyObject
	{
		public enum CardLayouts
		{
			None,
			PortraitTopLeft,
			PortraitTopCenter,
			PortraitCenter,
			LandscapeTopLeft,
			LandscapeTopCenter,
			LandscapeCenter

		};
		
		private static readonly DependencyProperty PrinterNameProperty =
			DependencyProperty.Register("PrinterName", typeof(string), 
			                            typeof(PrintProperties),
			                            new PropertyMetadata(string.Empty));
		public string PrinterName
		{
			get {return GetValue(PrinterNameProperty) as string;}
			set {SetValue(PrinterNameProperty, value);}
		}
		
		private static readonly DependencyProperty PrinterPaperSizeProperty =
			DependencyProperty.Register("PrinterPaperSize", typeof(PaperSize),
			                            typeof(PrintProperties),
			                            new PropertyMetadata(null));
		public PaperSize PrinterPaperSize
		{
			get {return GetValue(PrinterPaperSizeProperty) as PaperSize;}
			set {SetValue(PrinterPaperSizeProperty, value);}
		}
		
		private static readonly DependencyProperty PrinterResolutionProperty =
			DependencyProperty.Register("Resolution", typeof(PrinterResolution),
			                            typeof(PrintProperties),
			                            new PropertyMetadata(null));
		public PrinterResolution Resolution
		{
			get {return GetValue(PrinterResolutionProperty) as PrinterResolution;}
			set {SetValue(PrinterResolutionProperty, value);}
		}
		
		private static readonly DependencyProperty InsideMarginsProperty =
			DependencyProperty.Register("InsideMargins", typeof(bool),
			                            typeof(PrintProperties),
			                            new PropertyMetadata(false));
		public bool InsideMargins
		{
			get {return (bool)GetValue(InsideMarginsProperty);}
			set {SetValue(InsideMarginsProperty, value);}
		}
		
		private static readonly DependencyProperty PrintCardOutlinesProperty =
			DependencyProperty.Register("PrintCardOutlines", typeof(bool),
			                            typeof(PrintProperties),
			                            new PropertyMetadata(false));
		public bool PrintCardOutlines
		{
			get {return (bool)GetValue(PrintCardOutlinesProperty);}
			set {SetValue(PrintCardOutlinesProperty, value);}
		}
		
		private static readonly DependencyProperty FillLastPageProperty =
			DependencyProperty.Register("FillLastPage", typeof(bool),
			                            typeof(PrintProperties),
			                            new PropertyMetadata(false));
		public bool FillLastPage
		{
			get {return (bool)GetValue(FillLastPageProperty);}
			set {SetValue(FillLastPageProperty, value);}
		}
		
		private static readonly DependencyProperty SetCardMarginsProperty =
			DependencyProperty.Register("SetCardMargins", typeof(bool),
			                            typeof(PrintProperties),
			                            new PropertyMetadata(false));
		public bool SetCardMargins
		{
			get {return(bool) GetValue(SetCardMarginsProperty);}
			set {SetValue(SetCardMarginsProperty, value);}
		}
		
		private static readonly DependencyProperty LayoutProperty =
			DependencyProperty.Register("Layout", typeof(CardLayouts),
			                            typeof(PrintProperties),
			                            new PropertyMetadata(CardLayouts.None));
		public CardLayouts Layout
		{
			get {return (CardLayouts)GetValue(LayoutProperty);}
			set {SetValue(LayoutProperty, value);}
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		public PrintProperties()
		{
			UserPreferences prefs = ((App)Application.Current).UserPreferences;
			PrinterName = prefs.DefaultPrinterName;
			PrinterPaperSize = new PaperSize(prefs.DefaultPaperSize.PaperName,
			                                 prefs.DefaultPaperSize.Width,
			                                 prefs.DefaultPaperSize.Height);
			Resolution = new PrinterResolution();
			Resolution.Kind = prefs.DefaultPrinterResolution.Kind;
            Resolution.X = prefs.DefaultPrinterResolution.X;
            Resolution.Y = prefs.DefaultPrinterResolution.Y;
			InsideMargins = prefs.InsideMargins;
			PrintCardOutlines = prefs.PrintCardOutlines;
			FillLastPage = prefs.FillLastPage;
			SetCardMargins = prefs.SetCardMargins;
			Layout = prefs.CardsLayout;
		}
		
		/// <summary>
		/// Copy constructor
		/// </summary>
		/// <param name="props">PrintProperties object to make copy of</param>
		public PrintProperties(PrintProperties props)
		{
			this.PrinterName = props.PrinterName;
			this.PrinterPaperSize = 
				new PaperSize(props.PrinterPaperSize.PaperName,
				              props.PrinterPaperSize.Width, 
				              props.PrinterPaperSize.Height);
			this.Resolution = new PrinterResolution();
			this.Resolution.Kind = props.Resolution.Kind;
			this.Resolution.X = props.Resolution.X;
			this.Resolution.Y = props.Resolution.Y;
			
			this.InsideMargins = props.InsideMargins;
			this.PrintCardOutlines = props.PrintCardOutlines;
			this.FillLastPage = props.FillLastPage;
			this.SetCardMargins = props.SetCardMargins;
			this.Layout = props.Layout;
		}
		
		/// <summary>
		/// Overridden ToString method
		/// </summary>
		/// <returns>String representation of this PrintProperties object</returns>
		public override string ToString()
		{
			string newLine = Environment.NewLine;
			return string.Format("PrintProperties from Card:" + newLine +
				              "\tPrinter Name: {0}" + newLine +
				              "\tPaperSize: {1} {10} x {11}" + newLine +
				              "\tResolution: {2} {3} x {4}" + newLine +
				              "\tInsideMargins: {5}" + newLine +
				              "\tPrintCardOutlines: {6}" + newLine +
				              "\tFillLastPage: {7}" + newLine +
				              "\tSetCardMargins: {8}" + newLine +
				              "\tLayout: {9}",
				              PrinterName, PrinterPaperSize.PaperName,
				              Resolution.Kind, Resolution.X, Resolution.Y, 
				              InsideMargins, PrintCardOutlines,
				              FillLastPage, SetCardMargins, Layout,
				              PrinterPaperSize.Width, PrinterPaperSize.Height);
		}

	}
}