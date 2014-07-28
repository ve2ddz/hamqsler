/*
 *  Author:
 *       Jim Orcheson <jimorcheson@gmail.com>
 * 
 *  Copyright (c) 2012-2014 Jim Orcheson
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
using System.ComponentModel;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security;
using System.Text.RegularExpressions;
using System.Windows;
using System.Xml.Serialization;

namespace hamqsler
{
	/// <summary>
    /// UserPrefs contains user preference information
    /// It is serialized to file .hamqsler in the hamqsler folder
	/// </summary>
	[Serializable]
	[XmlInclude(typeof(TextParts))]
	[XmlInclude(typeof(TextPart))]
	[XmlInclude(typeof(StaticText))]
	[XmlInclude(typeof(AdifMacro))]
	[XmlInclude(typeof(AdifExistsMacro))]
	[XmlInclude(typeof(CountMacro))]
	[XmlInclude(typeof(ManagerMacro))]
	[XmlInclude(typeof(ManagerExistsMacro))]
	public class UserPreferences : DependencyObject, IDataErrorInfo
	{
		// check for new versions on startup?
		private static readonly DependencyProperty CheckForNewVersionsProperty = 
			DependencyProperty.Register("CheckForNewVersions", typeof(bool),
			                            typeof(UserPreferences), new PropertyMetadata(true));
		public bool CheckForNewVersions
		{
			get {return (bool)GetValue(CheckForNewVersionsProperty);}
			set {SetValue(CheckForNewVersionsProperty, value);}
		}
		// check for new development versions on startup?
		private static readonly DependencyProperty CheckForDevelopmentVersionsProperty =
			DependencyProperty.Register("CheckForDevelopmentVersions", typeof(bool),
			                            typeof(UserPreferences), new PropertyMetadata(false));
		public bool CheckForDevelopmentVersions
		{
			get {return (bool)GetValue(CheckForDevelopmentVersionsProperty);}
			set {SetValue(CheckForDevelopmentVersionsProperty, value);}
		}
		// HTTP proxy server
		private static readonly DependencyProperty HttpProxyServerProperty =
			DependencyProperty.Register("HttpProxyServer", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata(string.Empty));
		public string HttpProxyServer
		{
			get {return (string)GetValue(HttpProxyServerProperty);}
			set {SetValue(HttpProxyServerProperty, value);}
		}
		// HTTP proxy server port number
		private static readonly DependencyProperty HttpProxyServerPortNumberProperty =
			DependencyProperty.Register("HttpProxyServerPortNumber", typeof(int),
			                            typeof(UserPreferences), new PropertyMetadata(80));
		public int HttpProxyServerPortNumber
		{
			get {return (int)GetValue(HttpProxyServerPortNumberProperty);}
			set {SetValue(HttpProxyServerPortNumberProperty, value);}
		}
		// default folder for ADIF files
		// used in FileChooser dialogs when opening or saving ADIF files
		private static readonly DependencyProperty DefaultAdifFilesFolderProperty = 
			DependencyProperty.Register("DefaultAdifFilesFolder", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata(
			                            	((App)Application.Current).HamqslerFolder));
		public string DefaultAdifFilesFolder
		{
			get {return (string)GetValue(DefaultAdifFilesFolderProperty);}
			set {SetValue(DefaultAdifFilesFolderProperty, value);}
		}
		
		// reload ADIF files on startup?
		private static readonly DependencyProperty AdifReloadOnStartupProperty =
			DependencyProperty.Register("AdifReloadOnStartup", typeof(bool),
			                            typeof(UserPreferences), new PropertyMetadata(false));
		public bool AdifReloadOnStartup
		{
			get {return (bool)GetValue(AdifReloadOnStartupProperty);}
			set {SetValue(AdifReloadOnStartupProperty, value);}
		}
		
		// ADIF files to reload
		private static readonly DependencyProperty AdifFilesProperty = 
			DependencyProperty.Register("AdifFiles", typeof(List<string>),
			                            typeof(UserPreferences), new PropertyMetadata(new List<string>()));
		public List<string> AdifFiles
		{
			get {return (List<string>)GetValue(AdifFilesProperty);}
			set {SetValue(AdifFilesProperty, value);}
		}
		
		private static readonly DependencyProperty DefaultImagesFolderProperty = 
			DependencyProperty.Register("DefaultImagesFolder", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata(
			                            	((App)Application.Current).HamqslerFolder));
		public string DefaultImagesFolder
		{
			get {return (string)GetValue(DefaultImagesFolderProperty);}
			set {SetValue(DefaultImagesFolderProperty, value);}
		}
		
		// reload Card files on startup?
		private static readonly DependencyProperty CardsReloadOnStartupProperty =
			DependencyProperty.Register("CardsReloadOnStartup", typeof(bool),
			                            typeof(UserPreferences), new PropertyMetadata(false));
		public bool CardsReloadOnStartup
		{
			get {return (bool)GetValue(CardsReloadOnStartupProperty);}
			set {SetValue(CardsReloadOnStartupProperty, value);}
		}
		
		// card files to reload
		private static readonly DependencyProperty CardFilesProperty = 
			DependencyProperty.Register("CardFiles", typeof(List<string>),
			                            typeof(UserPreferences), new PropertyMetadata(new List<string>()));
		public List<string> CardFiles
		{
			get {return (List<string>)GetValue(CardFilesProperty);}
			set {SetValue(CardFilesProperty, value);}
		}
		
		// default folder for Card files
		private static readonly DependencyProperty DefaultCardFilesFolderProperty =
			DependencyProperty.Register("DefaultCardFilesFolder", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata(
			                            	((App)Application.Current).HamqslerFolder));
		public string DefaultCardFilesFolder
		{
			get {return (string)GetValue(DefaultCardFilesFolderProperty);}
			set {SetValue(DefaultCardFilesFolderProperty, value);}
		}
		
		private static readonly DependencyProperty DefaultPrinterNameProperty =
			DependencyProperty.Register("DefaultPrinterName", typeof(string),
			                            typeof(UserPreferences), 
			                            new PropertyMetadata(null));
		public string DefaultPrinterName
		{
			get {return GetValue(DefaultPrinterNameProperty) as string;}
			set {SetValue(DefaultPrinterNameProperty, value);}
		}
		
		private static readonly DependencyProperty DefaultPaperSizeProperty =
			DependencyProperty.Register("DefaultPaperSize", typeof(PaperSize),
			                            typeof(UserPreferences),
			                            new PropertyMetadata(null));
		public PaperSize DefaultPaperSize
		{
			get {return GetValue(DefaultPaperSizeProperty) as PaperSize;}
			set {SetValue(DefaultPaperSizeProperty, value);}
		}
		
		private static readonly DependencyProperty DefaultPrinterResolutionProperty =
			DependencyProperty.Register("DefaultPrinterResolution", typeof(PrinterResolution),
			                            typeof(UserPreferences),
			                            new PropertyMetadata(null));
		public PrinterResolution DefaultPrinterResolution
		{
			get {return GetValue(DefaultPrinterResolutionProperty) as PrinterResolution;}
			set {SetValue(DefaultPrinterResolutionProperty, value);}
		}
		
		private static readonly DependencyProperty DefaultPaperSourceProperty =
			DependencyProperty.Register("DefaultPaperSource", typeof(PaperSource),
			                            typeof(UserPreferences),
			                            new PropertyMetadata(null));
		public PaperSource DefaultPaperSource
		{
			get {return GetValue(DefaultPaperSourceProperty) as PaperSource;}
			set {SetValue(DefaultPaperSourceProperty, value);}
		}
		
		private static readonly DependencyProperty InsideMarginsProperty =
			DependencyProperty.Register("InsideMargins", typeof(bool),
			                            typeof(UserPreferences),
			                            new PropertyMetadata(false));
		public bool InsideMargins
		{
			get {return (bool)GetValue(InsideMarginsProperty);}
			set {SetValue(InsideMarginsProperty, value);}
		}
		
		private static readonly DependencyProperty PrintCardOutlinesProperty =
			DependencyProperty.Register("PrintCardOutlines", typeof(bool),
			                            typeof(UserPreferences),
			                            new PropertyMetadata(false));
		public bool PrintCardOutlines
		{
			get {return (bool)GetValue(PrintCardOutlinesProperty);}
			set {SetValue(PrintCardOutlinesProperty, value);}
		}
		
		private static readonly DependencyProperty FillLastPageProperty =
			DependencyProperty.Register("FillLastPage", typeof(bool),
			                            typeof(UserPreferences),
			                            new PropertyMetadata(false));
		public bool FillLastPage
		{
			get {return (bool)GetValue(FillLastPageProperty);}
			set {SetValue(FillLastPageProperty, value);}
		}
		
		private static readonly DependencyProperty SetCardMarginsProperty =
			DependencyProperty.Register("SetCardMargins", typeof(bool),
			                            typeof(UserPreferences),
			                            new PropertyMetadata(false));
		public bool SetCardMargins
		{
			get {return (bool)GetValue(SetCardMarginsProperty);}
			set {SetValue(SetCardMarginsProperty, value);}
		}
		
		private static readonly DependencyProperty PrintCardsVerticalProperty =
			DependencyProperty.Register("PrintCardsVertical", typeof(bool),
			                            typeof(UserPreferences),
			                            new PropertyMetadata(false));
		public bool PrintCardsVertical
		{
			get {return (bool)GetValue(PrintCardsVerticalProperty);}
			set {SetValue(PrintCardsVerticalProperty, value);}
		}
		
		private static readonly DependencyProperty CustomPaperSizesProperty =
			DependencyProperty.Register("CustomPaperSizes", 
			                            typeof(List<System.Drawing.Printing.PaperSize>),
			                            typeof(UserPreferences),
			                            new PropertyMetadata(new List<System.Drawing.Printing.PaperSize>()
			                                                ));
		public List<System.Drawing.Printing.PaperSize> CustomPaperSizes
		{
			get {return (List<System.Drawing.Printing.PaperSize>)GetValue(CustomPaperSizesProperty);}
		}
		
		// Default font face to display text items in
		private static readonly DependencyProperty DefaultTextItemsFontFaceProperty =
			DependencyProperty.Register("DefaultTextItemsFontFace", typeof(string),
			                            typeof(UserPreferences), 
			                            new PropertyMetadata("Arial"));
		public string DefaultTextItemsFontFace
		{
			get {return (string)GetValue(DefaultTextItemsFontFaceProperty);}
			set {SetValue(DefaultTextItemsFontFaceProperty, value);}
		}
		
		// Default text items
        /// <summary>
        /// Default callsign for the card
        /// </summary>
        private static readonly DependencyPropertyKey CallsignPropertyKey =
            DependencyProperty.RegisterReadOnly("Callsign", typeof(TextParts), 
        	                                    typeof(UserPreferences),
        	                                    new PropertyMetadata(null));
        private static readonly DependencyProperty CallsignProperty =
        	CallsignPropertyKey.DependencyProperty;
        public TextParts Callsign
        {
            get { return (TextParts)GetValue(CallsignProperty); }
        }
        
        /// <summary>
        /// Default Name and QTH text for the card
        /// </summary>
        private static readonly DependencyPropertyKey NameQthPropertyKey =
            DependencyProperty.RegisterReadOnly("NameQth", typeof(TextParts), typeof(UserPreferences),
            new PropertyMetadata(null));
        public static readonly DependencyProperty NameQthProperty =
        	NameQthPropertyKey.DependencyProperty;
        public TextParts NameQth
        {
            get { return (TextParts)GetValue(NameQthProperty); }
         }
        
		// Default salutation for the card
		private static readonly DependencyPropertyKey SalutationPropertyKey =
			DependencyProperty.RegisterReadOnly("Salutation", typeof(TextParts), typeof(UserPreferences),
			                            new PropertyMetadata(null));
		private static readonly DependencyProperty SalutationProperty =
			SalutationPropertyKey.DependencyProperty;
		public TextParts Salutation
		{
			get {return (TextParts)GetValue(SalutationProperty);}
		}
		
		// Default font face to display QSOs box text in
		private static readonly DependencyProperty DefaultQsosBoxFontFaceProperty =
			DependencyProperty.Register("DefaultQsosBoxFontFace", typeof(string),
			                            typeof(UserPreferences), 
			                            new PropertyMetadata("Arial"));
		public string DefaultQsosBoxFontFace
		{
			get {return (string)GetValue(DefaultQsosBoxFontFaceProperty);}
			set {SetValue(DefaultQsosBoxFontFaceProperty, value);}
		}
		
		// Default confirming text displayed in the QSOs Box
		private static readonly DependencyPropertyKey ConfirmingTextPropertyKey =
			DependencyProperty.RegisterReadOnly("ConfirmingText", typeof(TextParts), 
			                                    typeof(UserPreferences),
			                            		new PropertyMetadata(null));
		private static readonly DependencyProperty ConfirmingTextProperty =
			ConfirmingTextPropertyKey.DependencyProperty;
		public TextParts ConfirmingText
		{
			get {return (TextParts)GetValue(ConfirmingTextProperty);}
		}
		
		// Default confirming via text
		private static readonly DependencyProperty ViaTextProperty =
			DependencyProperty.Register("ViaText", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("via"));
		public string ViaText
		{
			get {return (string)GetValue(ViaTextProperty);}
			set {SetValue(ViaTextProperty, value);}
		}
		
		private static readonly DependencyProperty QsoColumn1Property =
			DependencyProperty.Register("QsoColumn1", typeof(string),
			                            typeof(UserPreferences), 
			                            new PropertyMetadata("Date"));
		public string QsoColumn1
		{
			get {return GetValue(QsoColumn1Property) as string;}
			set {SetValue(QsoColumn1Property, value);}
		}
		
		private static readonly DependencyProperty QsoColumn2Property =
			DependencyProperty.Register("QsoColumn2", typeof(string),
			                            typeof(UserPreferences), 
			                            new PropertyMetadata("Time"));
		public string QsoColumn2
		{
			get {return GetValue(QsoColumn2Property) as string;}
			set {SetValue(QsoColumn2Property, value);}
		}
		
		private static readonly DependencyProperty QsoColumn3Property =
			DependencyProperty.Register("QsoColumn3", typeof(string),
			                            typeof(UserPreferences), 
			                            new PropertyMetadata("Band or Frequency"));
		public string QsoColumn3
		{
			get {return GetValue(QsoColumn3Property) as string;}
			set {SetValue(QsoColumn3Property, value);}
		}
		
		private static readonly DependencyProperty QsoColumn4Property =
			DependencyProperty.Register("QsoColumn4", typeof(string),
			                            typeof(UserPreferences), 
			                            new PropertyMetadata("Mode"));
		public string QsoColumn4
		{
			get {return GetValue(QsoColumn4Property) as string;}
			set {SetValue(QsoColumn4Property, value);}
		}
		
		private static readonly DependencyProperty QsoColumn5Property =
			DependencyProperty.Register("QsoColumn5", typeof(string),
			                            typeof(UserPreferences), 
			                            new PropertyMetadata("RST"));
		public string QsoColumn5
		{
			get {return GetValue(QsoColumn5Property) as string;}
			set {SetValue(QsoColumn5Property, value);}
		}
		
		private static readonly DependencyProperty QsoColumn6Property =
			DependencyProperty.Register("QsoColumn6", typeof(string),
			                            typeof(UserPreferences), 
			                            new PropertyMetadata("QSL"));
		public string QsoColumn6
		{
			get {return GetValue(QsoColumn6Property) as string;}
			set {SetValue(QsoColumn6Property, value);}
		}
		
		// Date Text Headers
		private static readonly DependencyProperty YYYYMMDDTextProperty =
			DependencyProperty.Register("YYYYMMDDText", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("YYYY-MM-DD"));
		public string YYYYMMDDText
		{
			get {return (string)GetValue(YYYYMMDDTextProperty);}
			set {SetValue(YYYYMMDDTextProperty, value);}
		}
		
		private static readonly DependencyProperty DDMMMYYTextProperty = 
			DependencyProperty.Register("DDMMMYYText", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("DD-MMM-YY"));
		public string DDMMMYYText
		{
			get {return (string)GetValue(DDMMMYYTextProperty);}
			set {SetValue(DDMMMYYTextProperty, value);}
		}
		
		private static readonly DependencyProperty DDMMYYTextProperty =
			DependencyProperty.Register("DDMMYYText", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("DD-MM-YY"));
		public string DDMMYYText
		{
			get {return (string)GetValue(DDMMYYTextProperty);}
			set {SetValue(DDMMYYTextProperty, value);}
		}
		
		// Default date format
		private static readonly DependencyProperty DefaultDateFormatProperty =
			DependencyProperty.Register("DefaultDateFormat", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("YYYY-MM-DD"));
		public string DefaultDateFormat
		{
			get {return (string)GetValue(DefaultDateFormatProperty);}
			set {SetValue(DefaultDateFormatProperty, value);}
		}
		
		// Time header text
		private static readonly DependencyProperty TimeTextProperty = 
			DependencyProperty.Register("TimeText", typeof(string),
			                             typeof(UserPreferences), new PropertyMetadata("Time"));
		public string TimeText
		{
			get {return (string)GetValue(TimeTextProperty);}
			set {SetValue(TimeTextProperty, value);}
		}
		
		// Mode header text
		private static readonly DependencyProperty ModeTextProperty = 
			DependencyProperty.Register("ModeText", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("Mode"));
		public string ModeText
		{
			get {return (string)GetValue(ModeTextProperty);}
			set {SetValue(ModeTextProperty, value);}
		}
		
		// Band header text
		private static readonly DependencyProperty BandTextProperty = 
			DependencyProperty.Register("BandText", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("Band"));
		public string BandText
		{
			get {return (string)GetValue(BandTextProperty);}
			set {SetValue(BandTextProperty, value);}
		}
		
		// Frequency header text
		private static readonly DependencyProperty FrequencyTextProperty = 
			DependencyProperty.Register("FrequencyText", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("MHz"));
		public string FrequencyText
		{
			get {return (string)GetValue(FrequencyTextProperty);}
			set {SetValue(FrequencyTextProperty, value);}
		}
		
		// Report header by report type per mode
		private static readonly DependencyProperty ReportTypeByModeProperty =
			DependencyProperty.Register("ReportTypeByMode", typeof(bool),
			                            typeof(UserPreferences), new PropertyMetadata(false));
		public bool ReportTypeByMode
		{
			get {return (bool)GetValue(ReportTypeByModeProperty);}
			set {SetValue(ReportTypeByModeProperty, value);}
		}
		
		// RST header text
		private static readonly DependencyProperty RSTTextProperty = 
			DependencyProperty.Register("RSTText", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("RST"));
		public string RSTText
		{
			get {return (string)GetValue(RSTTextProperty);}
			set {SetValue(RSTTextProperty, value);}
		}
		
		// RST header text by report type
		private static readonly DependencyProperty RSTTextByReportTypeProperty = 
			DependencyProperty.Register("RSTTextByReportType", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("RST"));
		public string RSTTextByReportType
		{
			get {return (string)GetValue(RSTTextByReportTypeProperty);}
			set {SetValue(RSTTextByReportTypeProperty, value);}
		}
		
		// RS header text by report type
		private static readonly DependencyProperty RSTextByReportTypeProperty = 
			DependencyProperty.Register("RSTextByReportType", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("RS"));
		public string RSTextByReportType
		{
			get {return (string)GetValue(RSTextByReportTypeProperty);}
			set {SetValue(RSTextByReportTypeProperty, value);}
		}
		
		// RSQ header text
		private static readonly DependencyProperty RSQTextByReportTypeProperty = 
			DependencyProperty.Register("RSQTextByReportType", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("RSQ"));
		public string RSQTextByReportType
		{
			get {return (string)GetValue(RSQTextByReportTypeProperty);}
			set {SetValue(RSQTextByReportTypeProperty, value);}
		}
		
		// dB header text
		private static readonly DependencyProperty DBTextByReportTypeProperty = 
			DependencyProperty.Register("DBTextByReportType", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("dB"));
		public string DBTextByReportType
		{
			get {return (string)GetValue(DBTextByReportTypeProperty);}
			set {SetValue(DBTextByReportTypeProperty, value);}
		}
		
		// Report type by mode
		private static readonly DependencyPropertyKey ReportByModePropertyKey =
			DependencyProperty.RegisterReadOnly("ReportByMode", typeof(List<DataItem>), 
			                                    typeof(UserPreferences),
			                                    new PropertyMetadata(new List<DataItem>()));
		private static readonly DependencyProperty ReportByModeProperty =
			ReportByModePropertyKey.DependencyProperty;
		public List<DataItem> ReportByMode
		{
			get {return (List<DataItem>)GetValue(ReportByModeProperty);}
		}
		
		// QSL header text
		private static readonly DependencyProperty QSLTextProperty = 
			DependencyProperty.Register("QSLText", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("QSL"));
		public string QSLText
		{
			get {return (string)GetValue(QSLTextProperty);}
			set {SetValue(QSLTextProperty, value);}
		}
		
		// QSL Please text
		private static readonly DependencyProperty PseTextProperty = 
			DependencyProperty.Register("PseText", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("Pse"));
		public string PseText
		{
			get {return (string)GetValue(PseTextProperty);}
			set {SetValue(PseTextProperty, value);}
		}
		
		// QSL Thanks text
		private static readonly DependencyProperty TnxTextProperty = 
			DependencyProperty.Register("TnxText", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("Tnx"));
		public string TnxText
		{
			get {return (string)GetValue(TnxTextProperty);}
			set {SetValue(TnxTextProperty, value);}
		}
		
		// January text
		private static readonly DependencyProperty JanuaryTextProperty = 
			DependencyProperty.Register("JanuaryText", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("Jan"));
		public string JanuaryText
		{
			get {return (string)GetValue(JanuaryTextProperty);}
			set {SetValue(JanuaryTextProperty, value);}
		}
		
		// February text
		private static readonly DependencyProperty FebruaryTextProperty = 
			DependencyProperty.Register("FebruaryText", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("Feb"));
		public string FebruaryText
		{
			get {return (string)GetValue(FebruaryTextProperty);}
			set {SetValue(FebruaryTextProperty, value);}
		}
		
		// March text
		private static readonly DependencyProperty MarchTextProperty = 
			DependencyProperty.Register("MarchText", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("Mar"));
		public string MarchText
		{
			get {return (string)GetValue(MarchTextProperty);}
			set {SetValue(MarchTextProperty, value);}
		}
		
		// April text
		private static readonly DependencyProperty AprilTextProperty = 
			DependencyProperty.Register("AprilText", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("Apr"));
		public string AprilText
		{
			get {return (string)GetValue(AprilTextProperty);}
			set {SetValue(AprilTextProperty, value);}
		}
		
		// May text
		private static readonly DependencyProperty MayTextProperty = 
			DependencyProperty.Register("MayText", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("May"));
		public string MayText
		{
			get {return (string)GetValue(MayTextProperty);}
			set {SetValue(MayTextProperty, value);}
		}
		
		// June text
		private static readonly DependencyProperty JuneTextProperty = 
			DependencyProperty.Register("JuneText", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("Jun"));
		public string JuneText
		{
			get {return (string)GetValue(JuneTextProperty);}
			set {SetValue(JuneTextProperty, value);}
		}
		
		// July text
		private static readonly DependencyProperty JulyTextProperty = 
			DependencyProperty.Register("JulyText", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("Jul"));
		public string JulyText
		{
			get {return (string)GetValue(JulyTextProperty);}
			set {SetValue(JulyTextProperty, value);}
		}
		
		// August text
		private static readonly DependencyProperty AugustTextProperty = 
			DependencyProperty.Register("AugustText", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("Aug"));
		public string AugustText
		{
			get {return (string)GetValue(AugustTextProperty);}
			set {SetValue(AugustTextProperty, value);}
		}
		
		// September text
		private static readonly DependencyProperty SeptemberTextProperty = 
			DependencyProperty.Register("SeptemberText", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("Sep"));
		public string SeptemberText
		{
			get {return (string)GetValue(SeptemberTextProperty);}
			set {SetValue(SeptemberTextProperty, value);}
		}
		
		// October text
		private static readonly DependencyProperty OctoberTextProperty = 
			DependencyProperty.Register("OctoberText", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("Oct"));
		public string OctoberText
		{
			get {return (string)GetValue(OctoberTextProperty);}
			set {SetValue(OctoberTextProperty, value);}
		}
		
		// November text
		private static readonly DependencyProperty NovemberTextProperty = 
			DependencyProperty.Register("NovemberText", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("Nov"));
		public string NovemberText
		{
			get {return (string)GetValue(NovemberTextProperty);}
			set {SetValue(NovemberTextProperty, value);}
		}
		
		// December text
		private static readonly DependencyProperty DecemberTextProperty = 
			DependencyProperty.Register("DecemberText", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("Dec"));
		public string DecemberText
		{
			get {return (string)GetValue(DecemberTextProperty);}
			set {SetValue(DecemberTextProperty, value);}
		}
		
		// Frequency substitution for 2190m
		private static readonly DependencyProperty Frequency2190mProperty = 
			DependencyProperty.Register("Frequency2190m", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("0.136"));
		public string Frequency2190m
		{
			get {return (string)GetValue(Frequency2190mProperty);}
			set {SetValue(Frequency2190mProperty, value);}
		}
		
		// Frequency substitution for 630m
		private static readonly DependencyProperty Frequency630mProperty = 
			DependencyProperty.Register("Frequency630m", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("0.472"));
		public string Frequency630m
		{
			get {return (string)GetValue(Frequency630mProperty);}
			set {SetValue(Frequency630mProperty, value);}
		}
		
		// Frequency substitution for 560m
		private static readonly	DependencyProperty Frequency560mProperty = 
			DependencyProperty.Register("Frequency560m", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("0.501"));
		public string Frequency560m
		{
			get {return (string)GetValue(Frequency560mProperty);}
			set {SetValue(Frequency560mProperty, value);}
		}
		
		// Frequency substitution for 160m
		private static readonly DependencyProperty Frequency160mProperty = 
			DependencyProperty.Register("Frequency160m", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("1.8"));
		public string Frequency160m
		{
			get {return (string)GetValue(Frequency160mProperty);}
			set {SetValue(Frequency160mProperty, value);}
		}
		
		// Frequency substitution for 80m
		private static readonly DependencyProperty Frequency80mProperty = 
			DependencyProperty.Register("Frequency80m", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("3.5"));
		public string Frequency80m
		{
			get {return (string)GetValue(Frequency80mProperty);}
			set {SetValue(Frequency80mProperty, value);}
		}
		
		// Frequency substitution for 60m
		private static readonly DependencyProperty Frequency60mProperty = 
			DependencyProperty.Register("Frequency60m", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("5.2"));
		public string Frequency60m
		{
			get {return (string)GetValue(Frequency60mProperty);}
			set {SetValue(Frequency60mProperty, value);}
		}
		
		// Frequency substitution for 40m
		private static readonly DependencyProperty Frequency40mProperty = 
			DependencyProperty.Register("Frequency40m", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("7"));
		public string Frequency40m
		{
			get {return (string)GetValue(Frequency40mProperty);}
			set {SetValue(Frequency40mProperty, value);}
		}
		
		// Frequency substitution for 30m
		private static readonly DependencyProperty Frequency30mProperty = 
			DependencyProperty.Register("Frequency30m", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("10.1"));
		public string Frequency30m
		{
			get {return (string)GetValue(Frequency30mProperty);}
			set {SetValue(Frequency30mProperty, value);}
		}
		
		// Frequency substituion for 20m
		private static readonly DependencyProperty Frequency20mProperty = 
			DependencyProperty.Register("Frequency20m", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("14"));
		public string Frequency20m
		{
			get {return (string)GetValue(Frequency20mProperty);}
			set {SetValue(Frequency20mProperty, value);}
		}
		
		// Frequency substitution for 17m
		private static readonly DependencyProperty Frequency17mProperty = 
			DependencyProperty.Register("Frequency17m", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("18.1"));
		public string Frequency17m
		{
			get {return (string)GetValue(Frequency17mProperty);}
			set {SetValue(Frequency17mProperty, value);}
		}
		
		// Frequency substitution for 15m
		private static readonly DependencyProperty Frequency15mProperty = 
			DependencyProperty.Register("Frequency15m", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("21"));
		public string Frequency15m
		{
			get {return (string)GetValue(Frequency15mProperty);}
			set {SetValue(Frequency15mProperty, value);}
		}
		
		// Frequency substitution for 12m
		private static readonly DependencyProperty Frequency12mProperty = 
			DependencyProperty.Register("Frequency12m", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("24.9"));
		public string Frequency12m
		{
			get {return (string)GetValue(Frequency12mProperty);}
			set {SetValue(Frequency12mProperty, value);}
		}
		
		// Frequency substitution for 10m
		private static readonly DependencyProperty Frequency10mProperty = 
			DependencyProperty.Register("Frequency10m", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("28"));
		public string Frequency10m
		{
			get {return (string)GetValue(Frequency10mProperty);}
			set {SetValue(Frequency10mProperty, value);}
		}
		
		// Frequency substitution for 6m
		private static readonly DependencyProperty Frequency6mProperty = 
			DependencyProperty.Register("Frequency6m", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("50"));
		public string Frequency6m
		{
			get {return (string)GetValue(Frequency6mProperty);}
			set {SetValue(Frequency6mProperty, value);}
		}
		
		// Frequency substitution for 4m
		private static readonly DependencyProperty Frequency4mProperty = 
			DependencyProperty.Register("Frequency4m", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("70"));
		public string Frequency4m
		{
			get {return (string)GetValue(Frequency4mProperty);}
			set {SetValue(Frequency4mProperty, value);}
		}
		
		// Frequency substitution for 2m
		private static readonly DependencyProperty Frequency2mProperty = 
			DependencyProperty.Register("Frequency2m", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("144"));
		public string Frequency2m
		{
			get {return (string)GetValue(Frequency2mProperty);}
			set {SetValue(Frequency2mProperty, value);}
		}
		
		// Frequency substitution for 1.25m
		private static readonly DependencyProperty Frequency1p25mProperty = 
			DependencyProperty.Register("Frequency1p25m", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("222"));
		public string Frequency1p25m
		{
			get {return (string)GetValue(Frequency1p25mProperty);}
			set {SetValue(Frequency1p25mProperty, value);}
		}
		
		// Frequency substitution for 70cm
		private static readonly DependencyProperty Frequency70cmProperty = 
			DependencyProperty.Register("Frequency70cm", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("420"));
		public string Frequency70cm
		{
			get {return (string)GetValue(Frequency70cmProperty);}
			set {SetValue(Frequency70cmProperty, value);}
		}
		
		// Frequency substitution for 33cm
		private static readonly DependencyProperty Frequency33cmProperty = 
			DependencyProperty.Register("Frequency33cm", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("902"));
		public string Frequency33cm
		{
			get {return (string)GetValue(Frequency33cmProperty);}
			set {SetValue(Frequency33cmProperty, value);}
		}
		
		// Frequency substitution for 23cm
		private static readonly DependencyProperty Frequency23cmProperty = 
			DependencyProperty.Register("Frequency23cm", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("1240"));
		public string Frequency23cm
		{
			get {return (string)GetValue(Frequency23cmProperty);}
			set {SetValue(Frequency23cmProperty, value);}
		}
		
		// Frequency substitution for 13cm
		private static readonly DependencyProperty Frequency13cmProperty = 
			DependencyProperty.Register("Frequency13cm", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("2300"));
		public string Frequency13cm
		{
			get {return (string)GetValue(Frequency13cmProperty);}
			set {SetValue(Frequency13cmProperty, value);}
		}
		
		// Frequency substitution for 9cm
		private static readonly DependencyProperty Frequency9cmProperty	= 
			DependencyProperty.Register("Frequency9cm", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("3300"));
		public string Frequency9cm
		{
			get {return (string)GetValue(Frequency9cmProperty);}
			set {SetValue(Frequency9cmProperty, value);}
		}
		
		// Frequency substitution for 6cm
		private static readonly DependencyProperty Frequency6cmProperty = 
			DependencyProperty.Register("Frequency6cm", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("5650"));
		public string Frequency6cm
		{
			get {return (string)GetValue(Frequency6cmProperty);}
			set {SetValue(Frequency6cmProperty, value);}
		}
		
		// Frequency substitution for 3cm
		private static readonly DependencyProperty Frequency3cmProperty = 
			DependencyProperty.Register("Frequency3cm", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("10000"));
		public string Frequency3cm
		{
			get {return (string)GetValue(Frequency3cmProperty);}
			set {SetValue(Frequency3cmProperty, value);}
		}
		
		// Frequency substitution for 1.25cm
		private static readonly DependencyProperty Frequency1p25cmProperty = 
			DependencyProperty.Register("Frequency1p25cm", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("24000"));
		public string Frequency1p25cm
		{
			get {return (string)GetValue(Frequency1p25cmProperty);}
			set {SetValue(Frequency1p25cmProperty, value);}
		}
		
		// Frequency substitution for 6mm
		private static readonly DependencyProperty Frequency6mmProperty = 
			DependencyProperty.Register("Frequency6mm", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("47000"));
		public string Frequency6mm
		{
			get {return (string)GetValue(Frequency6mmProperty);}
			set {SetValue(Frequency6mmProperty, value);}
		}
		
		// Frequency substitution for 4mm
		private static readonly DependencyProperty Frequency4mmProperty = 
			DependencyProperty.Register("Frequency4mm", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("75500"));
		public string Frequency4mm
		{
			get {return (string)GetValue(Frequency4mmProperty);}
			set {SetValue(Frequency4mmProperty, value);}
		}
		
		// Frequency substitution for 2.5mm
		private static readonly DependencyProperty Frequency2p5mmProperty = 
			DependencyProperty.Register("Frequency2p5mmProperty", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("119980"));
		public string Frequency2p5mm
		{
			get {return (string)GetValue(Frequency2p5mmProperty);}
			set {SetValue(Frequency2p5mmProperty, value);}
		}
		
		// Frequency substitution for 2mm
		private static readonly DependencyProperty Frequency2mmProperty =
			DependencyProperty.Register("Frequency2mm", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("142000"));
		public string Frequency2mm
		{
			get {return (string)GetValue(Frequency2mmProperty);}
			set {SetValue(Frequency2mmProperty, value);}
		}
		
		// Frequency substitution for 1mm
		private static readonly DependencyProperty Frequency1mmProperty = 
			DependencyProperty.Register("Frequency1mm", typeof(string),
			                            typeof(UserPreferences), new PropertyMetadata("241000"));
		public string Frequency1mm
		{
			get {return (string)GetValue(Frequency1mmProperty);}
			set {SetValue(Frequency1mmProperty, value);}
		}
		
		// Debug printing
		private static readonly DependencyProperty DebugPrintingProperty =
			DependencyProperty.Register("DebugPrinting", typeof(bool),
			                            typeof(UserPreferences),
			                            new PropertyMetadata(false));
		public bool DebugPrinting
		{
			get {return (bool)GetValue(DebugPrintingProperty);}
			set {SetValue(DebugPrintingProperty, value);}
		}
		
		// Debug Logging
		private static readonly DependencyProperty DebugLoggingProperty =
			DependencyProperty.Register("DebugLogging", typeof(bool),
			                            typeof(UserPreferences),
			                            new PropertyMetadata(false));
		public bool DebugLogging
		{
			get {return (bool)GetValue(DebugLoggingProperty);}
			set {SetValue(DebugLoggingProperty, value);}
		}
		
        [NonSerialized]
        private static string userPreferencesFilename =  ((App)Application.Current).HamqslerFolder
        	+ ".hamqsler";
        public static string UserPreferencesFilename
        {
        	get {return userPreferencesFilename;}
        }
        
        // property required by IDataErrorInfo interface, but not used by WPF
        public string Error
        {
        	get {return null;}
        }

        /// <summary>
        ///  Default constructor used by the deserialization process.
        /// Do not call this directly, use the static CreateUserPreferences method instead
        /// </summary>
		public UserPreferences()
		{
			SetValue(CallsignPropertyKey, new TextParts());
			SetValue(NameQthPropertyKey, new TextParts());
			SetValue(SalutationPropertyKey, new TextParts());
			SetValue(ConfirmingTextPropertyKey, new TextParts());
			SetValue(ReportByModePropertyKey, new List<DataItem>());
			if(DefaultPrinterName == null)
			{
				PrinterSettings settings = new PrinterSettings();
				DefaultPrinterName = settings.PrinterName;
				DefaultPaperSize = settings.DefaultPageSettings.PaperSize;
				DefaultPrinterResolution = settings.DefaultPageSettings.PrinterResolution;
				DefaultPaperSource = settings.DefaultPageSettings.PaperSource;
			}
		}
		
				
		/// <summary>
		/// Error handling for IDataErrorInfo interface
		/// </summary>
		public string this[string propertyName]
		{
			get 
			{
				string result = null;
				if(propertyName == "Callsign")
				{
					result = null;
				}
				else		// a frequency*/
				{
					Regex regexData = new Regex("^[0-9\\.,]*$");
					Regex regexDecimalSep = new Regex("[\\.,][0-9]*[\\.,]");
					string band = null;
					string lower = null;
					string upper = null;
					string strFreq = GetBandProperties(propertyName, out band, out lower, out upper);
					if(strFreq != null)
					{
						if(regexDecimalSep.IsMatch(strFreq))
					   {
						   	result = "Only a single decimal separator is allowed.";
						   	return result;
					   }
						if(!regexData.IsMatch(strFreq))
						{
							result = "Only numbers and a single decimal separator are allowed.";
							return result;
						}
						string f = strFreq.Replace(",", ".");
						float freq = float.Parse(f, NumberStyles.AllowDecimalPoint,
						                    CultureInfo.InvariantCulture);
						float fLower = float.Parse(lower, NumberStyles.AllowDecimalPoint,
						                    CultureInfo.InvariantCulture);
						float fUpper = float.Parse(upper, NumberStyles.AllowDecimalPoint,
						                    CultureInfo.InvariantCulture);
						if(freq < fLower || freq > fUpper)
							result = string.Format("String must be between {0} and {1} MHz", 
							                       lower, upper);
					}
				}
				return result;
			}
		}

		/// <summary>
		/// Constructor used to create a clone of a UserPrefs object
		/// </summary>
		/// <param name="userPrefs">UserPreferences object to clone</param>
		public UserPreferences(UserPreferences prefs)
		{
			CheckForNewVersions = prefs.CheckForNewVersions;
			CheckForDevelopmentVersions = prefs.CheckForDevelopmentVersions;
			HttpProxyServer = prefs.HttpProxyServer;
			HttpProxyServerPortNumber = prefs.HttpProxyServerPortNumber;
			DefaultAdifFilesFolder = prefs.DefaultAdifFilesFolder;
			AdifReloadOnStartup = prefs.AdifReloadOnStartup;
			AdifFiles = prefs.AdifFiles;
			DefaultImagesFolder = prefs.DefaultImagesFolder;
			CardsReloadOnStartup = prefs.CardsReloadOnStartup;
			CardFiles = prefs.CardFiles;
			DefaultCardFilesFolder = prefs.DefaultCardFilesFolder;
			
			DefaultPrinterName = prefs.DefaultPrinterName;
			DefaultPaperSize = prefs.DefaultPaperSize;
			DefaultPrinterResolution = prefs.DefaultPrinterResolution;
			DefaultPaperSource = prefs.DefaultPaperSource;
			InsideMargins = prefs.InsideMargins;
			PrintCardOutlines = prefs.PrintCardOutlines;
			FillLastPage = prefs.FillLastPage;
			SetCardMargins = prefs.SetCardMargins;
			PrintCardsVertical = prefs.PrintCardsVertical;

			DefaultTextItemsFontFace = prefs.DefaultTextItemsFontFace;
			SetValue(CallsignPropertyKey, new TextParts());
			foreach(TextPart part in prefs.Callsign)
			{
				Callsign.Add(part);
			}
			SetValue(NameQthPropertyKey, new TextParts());
			foreach(TextPart part in prefs.NameQth)
			{
				NameQth.Add(part);
			}
			SetValue(SalutationPropertyKey, new TextParts());
			foreach(TextPart part in prefs.Salutation)
			{
				Salutation.Add(part);
			}
			QsoColumn1 = prefs.QsoColumn1;
			QsoColumn2 = prefs.QsoColumn2;
			QsoColumn3 = prefs.QsoColumn3;
			QsoColumn4 = prefs.QsoColumn4;
			QsoColumn5 = prefs.QsoColumn5;
			QsoColumn6 = prefs.QsoColumn6;
			DefaultQsosBoxFontFace = prefs.DefaultQsosBoxFontFace;
			SetValue(ConfirmingTextPropertyKey, new TextParts());
			foreach(TextPart part in prefs.ConfirmingText)
			{
				ConfirmingText.Add(part);
			}
			SetValue(ConfirmingTextPropertyKey, new TextParts());
			foreach(TextPart part in prefs.ConfirmingText)
			{
				ConfirmingText.Add(part);
			}
			ViaText = prefs.ViaText;
			YYYYMMDDText = prefs.YYYYMMDDText;
			DDMMMYYText = prefs.DDMMMYYText;
			DDMMYYText = prefs.DDMMYYText;
			DefaultDateFormat = prefs.DefaultDateFormat;
			TimeText = prefs.TimeText;
			ModeText = prefs.ModeText;
			BandText = prefs.BandText;
			FrequencyText = prefs.FrequencyText;
			ReportTypeByMode = prefs.ReportTypeByMode;
			RSTText = prefs.RSTText;
			RSTextByReportType = prefs.RSTextByReportType;
			RSTTextByReportType = prefs.RSTTextByReportType;
			RSQTextByReportType = prefs.RSQTextByReportType;
			DBTextByReportType = prefs.DBTextByReportType;
			ReportByMode.Clear();
			foreach(DataItem item in prefs.ReportByMode)
			{
				ReportByMode.Add(item);
			}
			QSLText = prefs.QSLText;
			PseText = prefs.PseText;
			TnxText = prefs.TnxText;
			JanuaryText = prefs.JanuaryText;
			FebruaryText = prefs.FebruaryText;
			MarchText = prefs.MarchText;
			AprilText = prefs.AprilText;
			MayText = prefs.MayText;
			JuneText = prefs.JuneText;
			JulyText = prefs.JulyText;
			AugustText = prefs.AugustText;
			SeptemberText = prefs.SeptemberText;
			OctoberText = prefs.OctoberText;
			NovemberText = prefs.NovemberText;
			DecemberText = prefs.DecemberText;
			Frequency2190m = prefs.Frequency2190m;
			Frequency630m = prefs.Frequency630m;
			Frequency560m = prefs.Frequency560m;
			Frequency160m = prefs.Frequency160m;
			Frequency80m = prefs.Frequency80m;
			Frequency60m = prefs.Frequency60m;
			Frequency40m = prefs.Frequency40m;
			Frequency30m = prefs.Frequency30m;
			Frequency20m = prefs.Frequency20m;
			Frequency17m = prefs.Frequency17m;
			Frequency15m = prefs.Frequency15m;
			Frequency12m = prefs.Frequency12m;
			Frequency10m = prefs.Frequency10m;
			Frequency6m = prefs.Frequency6m;
			Frequency4m = prefs.Frequency4m;
			Frequency2m = prefs.Frequency2m;
			Frequency1p25m = prefs.Frequency1p25m;
			Frequency70cm = prefs.Frequency70cm;
			Frequency33cm = prefs.Frequency33cm;
			Frequency23cm = prefs.Frequency23cm;
			Frequency13cm = prefs.Frequency13cm;
			Frequency9cm = prefs.Frequency9cm;
			Frequency6cm = prefs.Frequency6cm;
			Frequency3cm = prefs.Frequency3cm;
			Frequency1p25cm = prefs.Frequency1p25cm;
			Frequency6mm = prefs.Frequency6mm;
			Frequency4mm = prefs.Frequency4mm;
			Frequency2p5mm = prefs.Frequency2p5mm;
			Frequency2mm = prefs.Frequency2mm;
			Frequency1mm = prefs.Frequency1mm;
			DebugPrinting = prefs.DebugPrinting;
			DebugLogging = prefs.DebugLogging;
		}

        /// <summary>
        ///  Factory for UserPrefs objects.
        ///  Deserializes UserPrefs object stored in .hamqsler if file exists.
        ///  Otherwise creates a default UserPrefs object
        /// </summary>
        /// <param name="showMessages">boolean indicating whether an error message should be
        /// displayed if an error occurs.</param>
        /// <param name="prefsInitilized">boolean indicating whether the prefs object was initialized
        /// or loaded. Value of true indicates that the object was initialized, not loaded</param>
        /// <param name="prefsError">boolean indicating if an error occurred while trying
        /// to deserialize the prefs file.</param>
        /// <returns>Deserialized or default UserPrefs object</returns>
        public static UserPreferences CreateUserPreferences(bool showMessages,
                                                            out bool prefsInitialized,
                                                            out bool prefsError)
        {
            // filename of the user prefs file
            
            FileStream fStream = null;
            // default user preferences. Created here in case there are new preferences that
            // were not saved in the preferences file
            UserPreferences prefs = CreateDefaultUserPreferences(out prefsInitialized);
            prefsError = true;		// set to false if prefs file is read
            try
            {
                // get file info
 				FileInfo prefsInfo = new FileInfo(userPreferencesFilename);
                if (prefsInfo.Exists)
                {
                    // file exists, so read it
                    fStream = new FileStream(userPreferencesFilename, FileMode.Open,
                        FileAccess.Read);
                    XmlSerializer xmlFormat = new XmlSerializer(typeof(UserPreferences));
                    prefs = (UserPreferences)xmlFormat.Deserialize(fStream);
                    prefsInitialized = false;	// prefs file has been loaded
                    prefsError = false;			// no error loading file
                }
                else 
                {
                	prefsError = false;
                	if(showMessages)
	                {
	                    // no user prefs file, so inform user, and create a default
	                    MessageBox.Show("User Preferences file does not exist.\r\n"
	                        + "User Preferences are being initialized.\r\n"
	                        + "Preferences may be set from the Edit dropdown menu",
	                        "No User Preferences Found",
	                        MessageBoxButton.OK, MessageBoxImage.Information);
	                }
                }
            }
            catch (SecurityException e)
            {
                Exception ex = new Exception("You do not have permission to access the User Preferences File.\r\n"
                    + "Preferences will be initialized.",
                    e);
                App.Logger.Log(ex);
            }
            catch (UnauthorizedAccessException e)
            {
                Exception ex = new Exception("You do not have permission to access the User Preferences File.\r\n"
                    + "Preferences will be initialized.",
                    e);
                App.Logger.Log(ex);
             }
            catch (PathTooLongException e)
            {
                Exception ex = new Exception("Path to User Preferences File exceeds the system defined maximum length.\r\n"
                    + "Preferences will be initialized.",
                    e);
                App.Logger.Log(ex);
            }
            catch (FileNotFoundException e)
            {
            	prefsInitialized = true;
            	prefsError = false;
                Exception ex = new Exception("There appears to be a problem with your file system.\r\n"
                    + "This program has found the file, but it cannot be opened.\r\n"
                    + "Preferences will be initialized.",
                    e);
                App.Logger.Log(ex);
            }
            catch (IOException e)
            {
                Exception ex = new Exception("An IO error occurred while attempting to access the User Preferences file:\r\n"
                    + e.Message + "\r\n"
                    + "Preferences will be initialized.",
                    e);
                App.Logger.Log(ex);
            }
            catch (Exception e)
            {
                Exception ex = new Exception("Programming Error: Bad User Preferences filename or mode:\r\n"
                    + userPreferencesFilename + "\r\n"
                    + "Please do the following when you have finished using HamQSLer:\r\n"
                    + "1. Make a copy of the log file.\r\n"
                    + "2. Post a bug report, including the content of this message./r/n"
                    + "Preferences will be initialized. You may continue to use the program.",
                    e);
                App.Logger.Log(ex);
            }
            finally
            {
                // be sure to close the file
                if(fStream != null)
                    fStream.Close();
            }
            // check if there is a callsign from the file
            // and create default if not
            if(prefs.Callsign.Count == 0)
            {
            	StaticText sText = new StaticText();
            	sText.Text = "MyCall";
            	prefs.Callsign.Add(sText);
            }
            else
            {
            	foreach(TextPart part in prefs.Callsign)
            	{
            		part.RemoveExtraneousStaticTextMacros();
            	}
            }
            // check if there is a Name and QTH from the file
            // and create default if not
            if(prefs.NameQth.Count == 0)
            {
            	StaticText nText = new StaticText();
            	nText.Text = "MyQth";
            	prefs.NameQth.Add(nText);
            }
            else
            {
            	foreach(TextPart part in prefs.NameQth)
            	{
            		part.RemoveExtraneousStaticTextMacros();
            	}
            }
            // check if there is a salutation from the file
            // and create default if not
            if(prefs.Salutation.Count == 0)
            {
            	StaticText salText = new StaticText();
            	salText.Text = "Thanks for the QSO";
            	prefs.Salutation.Add(salText);
            	CountMacro macro = new CountMacro();
            	macro.CountEquals = true;
            	macro.Count = 1;
				((StaticText)macro.DesignText[0]).Text = "$";
				((StaticText)macro.TrueText[0]).Text = string.Empty;
				((StaticText)macro.FalseText[0]).Text = "s";
            	prefs.Salutation.Add(macro);
            	StaticText text73 = new StaticText();
            	text73.Text = ". 73,";
            	prefs.Salutation.Add(text73);
            }
            else
            {
            	foreach(TextPart part in prefs.Salutation)
            	{
            		part.RemoveExtraneousStaticTextMacros();
            	}
            }
            if(prefs.ConfirmingText.Count == 0)
            {
            	StaticText cText = new StaticText();
            	cText.Text = "Confirming 2-Way QSO";
            	prefs.ConfirmingText.Add(cText);
            	CountMacro macro = new CountMacro();
            	macro.CountEquals = true;
            	macro.Count = 1;
            	((StaticText)macro.DesignText[0]).Text = "$";
            	((StaticText)macro.TrueText[0]).Text = string.Empty;
            	((StaticText)macro.FalseText[0]).Text = "s";
            	prefs.ConfirmingText.Add(macro);
            	StaticText withText = new StaticText();
            	withText.Text = " with ";
            	prefs.ConfirmingText.Add(withText);
            }
            else
            {
            	foreach(TextPart part in prefs.ConfirmingText)
            	{
            		part.RemoveExtraneousStaticTextMacros();
            	}
            }
            // if prefs have been initialized, it is necessary to save the file
            // return the UserPrefs object
            if(prefsInitialized)
            {
            	// note: we do not want to suppress error Message box displays when saving the prefs file
            	prefs.SerializeAsXml();
            }
            return prefs;                
        }

        /// <summary>
        /// Helper method that creates a default UserPrefs object
        /// </summary>
        /// <param name="prefsInitialized">boolean indicating that the prefs object was created</param>
        /// <returns></returns>
        private static UserPreferences CreateDefaultUserPreferences(
                                              out bool prefsInitialized)
        {
            UserPreferences prefs = new UserPreferences();
            prefsInitialized = true;
            App.Logger.Log("User Preferences have been initialized");
            return prefs;
        }

       /// <summary>
        /// Saves the User Preferences into /hamqsler/.hamqsler
        /// </summary>
        internal void SerializeAsXml()
        {
            // get path to prefs file
            XmlSerializer xmlFormat = new XmlSerializer(typeof(UserPreferences));
            Stream fStream = null;
            try
            {
                // open the file and serialize the user prefs to the file
                fStream = new FileStream(userPreferencesFilename, FileMode.Create,
                        FileAccess.Write, FileShare.None);
                xmlFormat.Serialize(fStream, this);
            }
            catch (ArgumentOutOfRangeException e)
                // invalid file access mode
            {
                // wrap the exception in another message and log it
                Exception ex = new Exception("Programmer Error: Invalid File Access Mode"
                    + "\r\nUnable to save preferences file."
                    + "\r\n\r\nPlease do the following:\r\n"
                    + "1. Terminate this program.\r\n"
                    + "2. Post a bug report and include the contents of the log file.", e);
                App.Logger.Log(ex);
            }
            catch (ArgumentNullException e)
                // path to the file is null
            {
                // wrap exception in another and log it
                Exception ex = new Exception("Programmer Error: Null User Preferences file name."
                    + "\r\nUnable to save preferences file."
                    + "\r\n\r\nPlease do the following:\r\n"
                    + "1. Terminate this program.\r\n"
                    + "2. Post a bug report and include the contents of the log file.", e);
                App.Logger.Log(ex);
            }
            catch (ArgumentException e)
                // path is empty, contains only whitespace or illegal characters
                // or refers to a non-file device
            {
                Exception ex = new Exception("Programmer Error: Invalid User Preferences file name: "
                    + userPreferencesFilename
                    + "\r\nUnable to save preferences file."
                    + "\r\n\r\nPlease do the following:\r\n"
                    + "1. Terminate this program.\r\n"
                    + "2. Post a bug report and include the contents of the log file.", e);
                App.Logger.Log(ex);
            }
            catch (NotSupportedException e)
                // path refers to a non-file device
            {
                // wrap exception in another and log it
                Exception ex = new Exception("Programmer Error: Invalid User Preferences file name: "
                    + userPreferencesFilename
                    + "\r\nUnable to save preferences file."
                    + "\r\n\r\nPlease do the following:\r\n"
                    + "1. Terminate this program.\r\n"
                    + "2. Post a bug report and include the contents of the log file."
                    + "3. Restart HamQSLer and proceed without modifying preferences.",
                    e);
                App.Logger.Log(ex);
            }
            catch (PathTooLongException e)
                // the path, filename, or both exceeds system defined max length
            {
                // wrap exception in another and log it
                Exception ex = new Exception("The path or filename of the User Preferences file is too long:\r\n"
                        + userPreferencesFilename + "\r\n"
                        + "The most likely cause is a path to the hamqsler directory being more than 248 characters.\r\n"
                        + "The log file may contain more information."
                        , e);
                App.Logger.Log(ex);
            }
            catch (IOException e)
                // IO error occurred
            {
                // wrap exception in another and log it
                Exception ex = new Exception("IO Error While Attempting to Open or Write to the Preferences File."
                    + "Some possible causes include:\r\n"
                    + "1. You do not have write permission on the file or directory\r\n"
                    + "2. Disk error.\r\n\r\n"
                    + "See the log file for more information on the error", e);
                App.Logger.Log(ex);
            }
            catch (SecurityException e)
                // caller does not have required permission
            {
                // wrap exception in another, then log it
                Exception ex = new Exception("You do not have the required permissions to open or write the Preferences File\r\n"
                        , e);
                App.Logger.Log(ex);
            }
            catch (UnauthorizedAccessException e)
                // write access not allowed to the file (probably read-only)
            {
                // wrap in another exception and log it
                Exception ex = new Exception("Write access to the Preferences File is not allowed by the operating system.\r\n"
                    + "Check to see if the hamqsler directory or .hamqsler file is set to read-only.",
                    e);
                App.Logger.Log(ex);
            }
            catch (InvalidOperationException e)
                // error occurred during serialization
            {
                // wrap error in another exception and log it
                Exception ex = new Exception("An error occurred during serializing or saving the User Preferences File:"
                    + "\r\n" + e.InnerException.Message
                    + "\r\nIf you do not understand the error message, post a message using"
                    + "\r\nthe Contact Us form at www.va3hj.ca and include the contents of this message",
                    e);
                App.Logger.Log(ex);
            }
            finally
            {
                // make sure that the file is closed.
                if (fStream != null)
                    fStream.Close();
            }
        }	
        
        /// <summary>
        /// Retrieves both the text to display in the frequency column for a band when no freq 
        /// is specified, and the upper and lower limits for the band
        /// </summary>
        /// <param name="property">Name of the UserPreferences property that contains the text</param>
        /// <param name="band">HamBand object that corresponds to the property</param>
        /// <returns>String that contains the text to display</returns>
        private string GetBandProperties(string property, out string band, out string lower, 
                                         out string upper)
        {
        	string freq = null;

			PropertyInfo prefsPropInfo = this.GetType().GetProperty(property);
        	freq = prefsPropInfo.GetValue(this, null) as string;
			band = property.Substring("Frequency".Length);
			band = band.Replace('p', '.');
        	App.AdifEnums.GetBandLimits(band, out lower, out upper);
			return freq;
        }
        
        /// <summary>
        /// Build a list of modes and their report types.
        /// This UserPreferences object is created before the AdifEnumerations object is created,
        /// but this method calls AdifEnumerations.GetEnumeratedValues, so this method
        /// must be called after the AdifEnumerations object is created.
        /// </summary>
        public void BuildReportByModeList()
        {
        	List<DataItem> list = ReportByMode;
        	List<string> itemKeys = new List<string>(list.Count);
        	List<DataItem> newList = new List<DataItem>(list.Count);
        	bool newMode = false;
        	foreach(DataItem item in list)
        	{
        		newList.Add(item);
        		if(!App.AdifEnums.IsDeprecated("Mode", item.Key))
        		{
        			itemKeys.Add(item.Key);
        		}
        	}
        	string[] modes = App.AdifEnums.GetEnumeratedValues("Mode");
        	foreach(string mode in modes)
        	{
        		bool found = false;
        		foreach(string key in itemKeys)
        		{
        			if(mode.Equals(key))
        			{
        				found = true;
        				break;
        			}
        		}
        		if(!found)
        		{
        			string modeType = string.Empty;
        			if(App.AdifEnums.GetDefaultReportType(mode, out modeType))
        			{
        				newList.Add(new DataItem(mode, modeType));
        				newMode = true;
        			}
        		}
        	}
        	// if a new mode has been defined, save the user preferences.
        	if(newMode)
        	{
        		ReportByMode.Clear();
        		ReportByMode.AddRange(newList);
        		SerializeAsXml();
        	}
        }
        
	}
}
