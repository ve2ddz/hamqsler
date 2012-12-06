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
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace hamqsler
{
	/// <summary>
	/// Description of QsosBox.
	/// </summary>
	[Serializable]
	public class QsosBox : CardItem
	{
		// determines whether to show Manager ("via (Manager)")
		private static readonly DependencyProperty ShowManagerProperty =
			DependencyProperty.Register("ShowManager", typeof(bool),
			                            typeof(QsosBox), new PropertyMetadata(true));
		public bool ShowManager
		{
			get { return (bool)GetValue(ShowManagerProperty); }
			set { SetValue(ShowManagerProperty, value); }
		}

		// determines whether to show Band or Frequency
		private static readonly DependencyProperty ShowFrequencyProperty =
			DependencyProperty.Register("ShowFrequency",typeof(bool),
			                            typeof(QsosBox), new PropertyMetadata(false));
		public bool ShowFrequency
		{
			get { return (bool)GetValue(ShowFrequencyProperty); }
			set { SetValue(ShowFrequencyProperty, value); }
		}

		// determines whether to include QSL column
		private static readonly DependencyProperty ShowPseTnxProperty =
			DependencyProperty.Register("ShowPseTnx", typeof(bool),
			                            typeof(QsosBox), new PropertyMetadata(false));
		public bool ShowPseTnx
		{
			get { return (bool)GetValue(ShowPseTnxProperty); }
			set { SetValue(ShowPseTnxProperty, (bool)value); }
		}

		// maximum QSOs that can be displayed on the card
		private static readonly DependencyProperty MaximumQsosProperty =
			DependencyProperty.Register("MaximumQsos", typeof(int),
			                            typeof(QsosBox), new PropertyMetadata(4));
		public int MaximumQsos
		{
			get { return (int)GetValue(MaximumQsosProperty); }
			set { SetValue(MaximumQsosProperty, (int)value); }
		}

		// Format for dates displayed
		private static readonly DependencyProperty DateFormatProperty =
			DependencyProperty.Register("DateFormat", typeof(string),
			                            typeof(QsosBox), new PropertyMetadata("YYYY-MM-DD"));
		public string DateFormat
		{
			get { return (string)GetValue(DateFormatProperty); }
			set { SetValue(DateFormatProperty, (string)value); }
		}

		// Line and text brush for QSO box
		private static readonly DependencyProperty LineTextBrushProperty =
			DependencyProperty.Register("LineTextBrush", typeof(Brush),
			                            typeof(QsosBox), new PropertyMetadata(Brushes.Black));
		public Brush LineTextBrush
		{
			get { return (Brush)GetValue(LineTextBrushProperty); }
			set { SetValue(LineTextBrushProperty, (Brush)value); }
		}

		// Callsign brush for QSO box
		private static readonly DependencyProperty CallsignBrushProperty =
			DependencyProperty.Register("CallsignBrush", typeof(Brush),
			                            typeof(QsosBox), new PropertyMetadata(Brushes.Red));
		public Brush CallsignBrush
		{
			get { return (Brush)GetValue(CallsignBrushProperty); }
			set { SetValue(CallsignBrushProperty, (Brush)value); }
		}

		// Manager brush for QSO box
		private static readonly DependencyProperty ManagerBrushProperty =
			DependencyProperty.Register("ManagerBrush", typeof(Brush),
			                            typeof(QsosBox), new PropertyMetadata(Brushes.Gray));
		public Brush ManagerBrush
		{
			get { return (Brush)GetValue(ManagerBrushProperty); }
			set { SetValue(ManagerBrushProperty, (Brush)value); }
		}
		
		// QsosBox text font size
		private static readonly DependencyProperty FontSizeProperty =
			DependencyProperty.Register("FontSize", typeof(double), typeof(QsosBox),
			                            new PropertyMetadata(12.0));
		public double FontSize
		{
			get {return (double)GetValue(FontSizeProperty);}
			set {SetValue(FontSizeProperty, value);}
		}

		// Font name for text in QSO box
		private static readonly DependencyProperty FontNameProperty =
			DependencyProperty.Register("FontName", typeof(string),
			                            typeof(QsosBox), new PropertyMetadata("Arial"));
		public string FontName
		{
			get { return (string)GetValue(FontNameProperty); }
			set { SetValue(FontNameProperty, (string)value); }
		}


		// Background brush for QSO box
		private static readonly DependencyProperty BackgroundBrushProperty =
			DependencyProperty.Register("BackgroundBrush", typeof(Brush),
			                            typeof(QsosBox), new PropertyMetadata(Brushes.Transparent));
		public Brush BackgroundBrush
		{
			get { return (Brush)GetValue(BackgroundBrushProperty); }
			set { SetValue(BackgroundBrushProperty, (Brush)value); }
		}

		// Background opacity for QSO box
		private static readonly DependencyProperty BackgroundOpacityProperty =
			DependencyProperty.Register("BackgroundOpacity", typeof(double),
			                            typeof(QsosBox), new PropertyMetadata(1.0));
		public double BackgroundOpacity
		{
			get { return (double)GetValue(BackgroundOpacityProperty); }
			set { SetValue(BackgroundOpacityProperty, (double)value); }
		}

		// Text for confirming QSO string for single QSO
		private static readonly DependencyPropertyKey ConfirmingTextPropertyKey =
			DependencyProperty.RegisterReadOnly("ConfirmingText", typeof(TextParts),
			                            typeof(QsosBox), new PropertyMetadata(null));
		private static readonly DependencyProperty ConfirmingTextProperty =
			ConfirmingTextPropertyKey.DependencyProperty;
		public TextParts ConfirmingText
		{
			get { return (TextParts)GetValue(ConfirmingTextProperty); }
			set { SetValue(ConfirmingTextProperty, value); }
		}

		// Text preceeding Manager callsign
		private static readonly DependencyProperty ViaTextProperty =
			DependencyProperty.Register("ViaText", typeof(string),
			                            typeof(QsosBox), new PropertyMetadata("via"));
		public string ViaText
		{
			get { return (string)GetValue(ViaTextProperty); }
			set { SetValue(ViaTextProperty, value); }
		}

		/// <summary>
		/// Date column header text for the card when date format is YYYY-MM-DD
		/// </summary>
		private static readonly DependencyProperty YYYYMMDDTextProperty =
			DependencyProperty.Register("YYYYMMDDText", typeof(string), typeof(QsosBox),
			                            new PropertyMetadata("YYYY-MM-DD"));
		public string YYYYMMDDText
		{
			get { return (string)GetValue(YYYYMMDDTextProperty); }
			set { SetValue(YYYYMMDDTextProperty, value); }
		}

		/// <summary>
		/// Date column header text for the card when date format is DD-MMM-YY
		/// </summary>
		private static readonly DependencyProperty DDMMMYYTextProperty =
			DependencyProperty.Register("DDMMMYYText", typeof(string), typeof(QsosBox),
			                            new PropertyMetadata("DD-MMM-YY"));
		public string DDMMMYYText
		{
			get { return (string)GetValue(DDMMMYYTextProperty); }
			set { SetValue(DDMMMYYTextProperty, value); }
		}

		/// <summary>
		/// Date column header text for the card when date format is DD-MM-YY
		/// </summary>
		private static readonly DependencyProperty DDMMYYTextProperty =
			DependencyProperty.Register("DDMMYYText", typeof(string), typeof(QsosBox),
			                            new PropertyMetadata("DD-MM-YY"));
		public string DDMMYYText
		{
			get { return (string)GetValue(DDMMYYTextProperty); }
			set { SetValue(DDMMYYTextProperty, value); }
		}

		/// <summary>
		/// Time column header text for the card
		/// </summary>
		private static readonly DependencyProperty TimeTextProperty =
			DependencyProperty.Register("TimeText", typeof(string), typeof(QsosBox),
			                            new PropertyMetadata("Time"));
		public string TimeText
		{
			get { return (string)GetValue(TimeTextProperty); }
			set { SetValue(TimeTextProperty, value); }
		}

		/// <summary>
		/// Mode column header text for the card
		/// </summary>
		private static readonly DependencyProperty ModeTextProperty =
			DependencyProperty.Register("ModeText", typeof(string), typeof(QsosBox),
			                            new PropertyMetadata("Mode"));
		public string ModeText
		{
			get { return (string)GetValue(ModeTextProperty); }
			set { SetValue(ModeTextProperty, value); }
		}

		/// <summary>
		/// Band column text for the card
		/// </summary>
		private static readonly DependencyProperty BandTextProperty =
			DependencyProperty.Register("BandText", typeof(string), typeof(QsosBox),
			                            new PropertyMetadata("Band"));
		public string BandText
		{
			get { return (string)GetValue(BandTextProperty); }
			set { SetValue(BandTextProperty, value); }
		}

		/// <summary>
		/// Frequency column text for the card
		/// </summary>
		private static readonly DependencyProperty FreqTextProperty =
			DependencyProperty.Register("BFreqText", typeof(string), typeof(QsosBox),
			                            new PropertyMetadata("MHz"));
		public string FreqText
		{
			get { return (string)GetValue(FreqTextProperty); }
			set { SetValue(FreqTextProperty, value); }
		}

		/// <summary>
		/// RST column header text for the card
		/// </summary>
		private static readonly DependencyProperty RSTTextProperty =
			DependencyProperty.Register("RSTText", typeof(string), typeof(QsosBox),
			                            new PropertyMetadata("RST"));
		public string RSTText
		{
			get { return (string)GetValue(RSTTextProperty); }
			set { SetValue(RSTTextProperty, value); }
		}

		/// <summary>
		/// QSL column header text for the card
		/// </summary>
		private static readonly DependencyProperty QSLTextProperty =
			DependencyProperty.Register("QSLText", typeof(string), typeof(QsosBox),
			                            new PropertyMetadata("QSL"));
		public string QSLText
		{
			get { return (string)GetValue(QSLTextProperty); }
			set { SetValue(QSLTextProperty, value); }
		}

		/// <summary>
		/// Pse text displayed in QSL column
		/// </summary>
		private static readonly DependencyProperty PseTextProperty =
			DependencyProperty.Register("PseText", typeof(string), typeof(QsosBox),
			                            new PropertyMetadata("Pse"));
		public string PseText
		{
			get { return (string)GetValue(PseTextProperty); }
			set { SetValue(PseTextProperty, value); }
		}

		/// <summary>
		/// Tnx text displayed in QSL column
		/// </summary>
		private static readonly DependencyProperty TnxTextProperty =
			DependencyProperty.Register("TnxText", typeof(string), typeof(QsosBox),
			                            new PropertyMetadata("Tnx"));
		public string TnxText
		{
			get { return (string)GetValue(TnxTextProperty); }
			set { SetValue(TnxTextProperty, value); }
		}
		
		[NonSerialized]
		private Dictionary<string, string> months = new Dictionary<string, string>();
		
		/// <summary>
		/// Holds QSOs to print on the cards
		/// </summary>
		internal List<QsoWithInclude> qsos = new List<QsoWithInclude>();

		/// <summary>
		/// Indexer for the QSOs
		/// </summary>
		/// <param name="key">Key to the QSO</param>
		/// <returns>QSO specified by the key</returns>
		internal QsoWithInclude this[int key]
		{
			get
			{
				QsoWithInclude qso = qsos[key];
				return qso;
			}
		}

		/// <summary>
		/// <summary>
		/// Retrieves the number of QSOs that will be printed
		/// </summary>
		public int QsosCount
		{
			get { return qsos.Count; }
		}
		
		private enum Columns
		{
			Date = 0,
			Time,
			BandFreq,
			Mode,
			RST,
			QSL
		};
		
		// attributes used for table headers
		private double[] colWidths = { 0, 0, 0, 0, 0, 0 };
		private FormattedText[] colHeadersText = {null, null, null, null, null, null};
		private double[] colHeaderX = { 0, 0, 0, 0, 0, 0 };
		private double[] colHeadersTextWidth = { 0, 0, 0, 0, 0, 0 };

		
		private const int cornerRounding = 3;
		private const double confirmingXOffset = 5;
		private const double confirmingYOffset = 2;
		private const double confirmingCallXOffset = 3;
		private const double viaXOffset = 15;
        private const double firstLineYOffset = 2;
        private const double lineYOffset = firstLineYOffset + 2;

		/// <summary>
		/// Constructor
		/// </summary>
		public QsosBox()
		{
			UserPreferences userPrefs = ((App)Application.Current).UserPreferences;
			InitializeDisplayProperties(userPrefs);
			InitializeMonthsDictionary(userPrefs);
		}
		
		/// <summary>
		/// Helper method that initializes the text for three letter month abreviations
		/// </summary>
		/// <param name="userPrefs">UserPreferences object used to initialize months text</param>
		private void InitializeMonthsDictionary(UserPreferences userPrefs)
		{
			months["01"] = userPrefs.JanuaryText;
			months["02"] = userPrefs.FebruaryText;
			months["03"] = userPrefs.MarchText;
			months["04"] = userPrefs.AprilText;
			months["05"] = userPrefs.MayText;
			months["06"] = userPrefs.JuneText;
			months["07"] = userPrefs.JulyText;
			months["08"] = userPrefs.AugustText;
			months["09"] = userPrefs.SeptemberText;
			months["10"] = userPrefs.OctoberText;
			months["11"] = userPrefs.NovemberText;
			months["12"] = userPrefs.DecemberText;
		}
		
		/// <summary>
		/// Helper method that initializes header texts
		/// </summary>
		/// <param name="userPrefs">UserPreferences object used to initialize QsosBox properties</param>
		private void InitializeDisplayProperties(UserPreferences userPrefs)
		{
			DisplayX = 0;
			DisplayY = 0;
			DisplayWidth = 0;
			DisplayHeight = 0;
			this.FontName = userPrefs.DefaultQsosBoxFontFace;
			SetValue(ConfirmingTextPropertyKey, new TextParts());
			foreach(TextPart part in userPrefs.ConfirmingText)
			{
				this.ConfirmingText.Add(part);
			}
			this.ViaText = userPrefs.ViaText;
			this.DateFormat = userPrefs.DefaultDateFormat;
			this.YYYYMMDDText = userPrefs.YYYYMMDDText;
			this.DDMMMYYText = userPrefs.DDMMMYYText;
			this.DDMMYYText = userPrefs.DDMMYYText;
			this.TimeText = userPrefs.TimeText;
			this.ModeText = userPrefs.ModeText;
			this.BandText = userPrefs.BandText;
			this.FreqText = userPrefs.FrequencyText;
			this.RSTText = userPrefs.RSTText;
			this.QSLText = userPrefs.QSLText;
			this.PseText = userPrefs.PseText;
			this.TnxText = userPrefs.TnxText;

		}
		
		/// <summary>
		/// Calculate the size of the QsosBox based on text size and maxQsos
		/// </summary>
		private void CalculateRectangle()
		{
			FormattedText fText = GenerateFormattedText("Sample Text", LineTextBrush,
			                                            FontWeights.Normal);
			double height = (fText.Height + 4) * (2 + ((QsosCount > 0) ? QsosCount : MaximumQsos));
			if(DisplayX == 0 && DisplayY == 0 && DisplayWidth == 0 && DisplayHeight == 0)
			{
				DisplayX = QslCard.DisplayWidth / 20;
				DisplayY = QslCard.DisplayHeight / 2;
				DisplayWidth = QslCard.DisplayWidth * 18 / 20;
				DisplayHeight = height;
			}
			else
			{
				DisplayWidth = QslCard.DisplayWidth * 18 / 20;
				DisplayHeight = height;
			}
		}
		
		/// <summary>
		/// Generates formatted text based on the text, brush, and weight
		/// </summary>
		/// <param name="text">Text to be formatted</param>
		/// <param name="brush">Brush to use for formatting</param>
		/// <param name="weight">Text weight to use for formatting</param>
		/// <returns>FormattedText object representing the text</returns>
		protected FormattedText GenerateFormattedText(string text, Brush brush, FontWeight weight)
		{
			
			Typeface typeface = new Typeface(new FontFamily(FontName), FontStyles.Normal,
			                                 weight, FontStretches.Normal);
			FormattedText fText = new FormattedText(text,
			                                        System.Globalization.CultureInfo.CurrentUICulture,
			                                        FlowDirection.LeftToRight, typeface, FontSize, brush);
			return fText;
		}
		
		
       /// <summary>
        /// Calculates the width of QSOsBox columns base on column and column header text
        /// // as well as typeface, style, and weight
        /// </summary>
        private void CalculateColumns()
        {
            Typeface typeface = new Typeface(new FontFamily(FontName), FontStyles.Normal,
                FontWeights.Normal, FontStretches.Normal);
        	string dateFmt = YYYYMMDDText;
        	if(DateFormat == "DD-MMM-YY")
        		dateFmt = DDMMMYYText;
        	else if(DateFormat == "DD-MM-YY")
        		dateFmt = DDMMYYText;
            FormattedText fT = new FormattedText(dateFmt, System.Globalization.CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight, typeface, 12.0, LineTextBrush);
        	colWidths[(int)Columns.Date] = fT.Width;
            colHeadersText[(int)Columns.Date] = fT;
            colHeadersTextWidth[(int)Columns.Date] = fT.Width;

            fT = new FormattedText("8888", System.Globalization.CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight, typeface, 12.0, LineTextBrush);
            FormattedText fTH = new FormattedText(TimeText, System.Globalization.CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight, typeface, 12.0, LineTextBrush);
            colWidths[(int)Columns.Time] = (fT.Width > fTH.Width) ? fT.Width : fTH.Width;
            colHeadersText[(int)Columns.Time] = fTH;
            colHeadersTextWidth[(int)Columns.Time] = fTH.Width;

            string band = ShowFrequency ? "888.888" : "WWWWW";
            fT = new FormattedText(band, System.Globalization.CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight, typeface, 12.0, LineTextBrush);
            fTH = new FormattedText((ShowFrequency ? FreqText : BandText),
                System.Globalization.CultureInfo.CurrentUICulture, FlowDirection.LeftToRight,
                typeface, 12.0, LineTextBrush);
            colWidths[(int)Columns.BandFreq] = (fT.Width > fTH.Width) ? fT.Width : fTH.Width;
            colHeadersText[(int)Columns.BandFreq] = fTH;
            colHeadersTextWidth[(int)Columns.BandFreq] = fTH.Width;

            fT = new FormattedText("WWWWWW", System.Globalization.CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight, typeface, 12.0, LineTextBrush);
            fTH = new FormattedText(ModeText, System.Globalization.CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight, typeface, 12.0, LineTextBrush);
            colWidths[(int)Columns.Mode] = (fT.Width > fTH.Width) ? fT.Width : fTH.Width;
            colHeadersText[(int)Columns.Mode] = fTH;
            colHeadersTextWidth[(int)Columns.Mode] = fTH.Width;

            fT = new FormattedText("888", System.Globalization.CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight, typeface, 12.0, LineTextBrush);
            fTH = new FormattedText(RSTText, System.Globalization.CultureInfo.CurrentUICulture,
                FlowDirection.LeftToRight, typeface, 12.0, LineTextBrush);
            colWidths[(int)Columns.RST] = (fT.Width > fTH.Width) ? fT.Width : fTH.Width;
            colHeadersText[(int)Columns.RST] = fTH;
            colHeadersTextWidth[(int)Columns.RST] = fTH.Width;

            if(ShowPseTnx == true)
            {
                fT = new FormattedText(PseText, System.Globalization.CultureInfo.CurrentUICulture,
                    FlowDirection.LeftToRight, typeface, 12.0, LineTextBrush);
                FormattedText fT2 = new FormattedText(TnxText, System.Globalization.CultureInfo.CurrentUICulture,
                    FlowDirection.LeftToRight, typeface, 12.0, LineTextBrush);
                fTH = new FormattedText(QSLText, System.Globalization.CultureInfo.CurrentUICulture,
                    FlowDirection.LeftToRight, typeface, 12.0, LineTextBrush);
                double max = Math.Max(fT.Width, fT2.Width);
                max = Math.Max(max, fTH.Width);
                colWidths[(int)Columns.QSL] = max;
                colHeadersText[(int)Columns.QSL] = fTH;
                colHeadersTextWidth[(int)Columns.QSL] = fTH.Width;
            }
            else
            {
                colWidths[(int)Columns.QSL] = 0;
                colHeadersText[(int)Columns.QSL] = null;
                colHeadersTextWidth[(int)Columns.QSL] = 0;
            }
            double totalColWidths = 0;
            for (int i = 0; i < colWidths.Length; i++)
            {
                totalColWidths += colWidths[i];
            }
            int totalCols = (ShowPseTnx) ? colHeadersText.Length : colHeadersText.Length - 1;
            double columnExpansion = (DisplayWidth - totalColWidths) / totalCols;
            for (int i = 0; i < colWidths.Length - 1; i++)
            {
                colWidths[i] += columnExpansion;
                colHeaderX[i] = (colWidths[i] - colHeadersTextWidth[i]) / 2;
            }
            if (ShowPseTnx)
            {
            	colWidths[(int)Columns.QSL] += columnExpansion;
            	colHeaderX[(int)Columns.QSL] = (colWidths[(int)Columns.QSL] - 
            	                                colHeadersTextWidth[(int)Columns.QSL]) / 2;
            }
        }
        
 
        /// <summary>
        /// Handles rendering of the QSOsBox on the card
        /// </summary>
        /// <param name="drawingContext">drawing context to render to</param>
        protected override void OnRender(DrawingContext dc)
        {
            bool isMod = QslCard.IsDirty;
            CalculateRectangle();
            // headers cannot be set up until after QslCard is set
            if(QslCard != null)
            {
            	CalculateColumns();
            }
            QslCard.IsDirty = isMod;
            Brush brush = Brushes.Transparent;
            Pen pen = new Pen(LineTextBrush, 1);
            Pen transparentPen = new Pen(Brushes.Transparent, 1);
            // draw the box background
            dc.PushOpacity(BackgroundOpacity);
            dc.DrawRoundedRectangle(BackgroundBrush, transparentPen,
                                    new Rect(DisplayX, DisplayY, DisplayWidth, DisplayHeight),
                                   	cornerRounding, cornerRounding);
            
            dc.Pop();
            dc.DrawRoundedRectangle(Brushes.Transparent, pen, 
                                    new Rect(DisplayX, DisplayY, DisplayWidth, DisplayHeight),
                                   	cornerRounding, cornerRounding);
            int qsoCount = (QsosCount > 0) ? QsosCount : MaximumQsos;
            FormattedText formattedText = GenerateFormattedText(ConfirmingText.GetText(true),
                                                                LineTextBrush,
                                                                FontWeights.Normal);
            double textHeight = formattedText.Height;
            double x = DisplayX + confirmingXOffset;
            double y = DisplayY + confirmingYOffset;
            dc.DrawText(formattedText, new Point(x, y));
            x += formattedText.Width + confirmingCallXOffset;
            string text = "        ";
            if(QsosCount > 0)
            {
            	text = ((QsoWithInclude)this[0]).Callsign;
            }
            else if(QsosCount == 0)
            {
            	text = "XXXXXX";
	            formattedText = GenerateFormattedText(text, CallsignBrush, FontWeights.Black);
	            dc.DrawText(formattedText, new Point(x, y));
	            x += formattedText.Width + viaXOffset;
            }
            if(ShowManager)
            {
            	string manager = (QsosCount > 0) ? ((QsoWithInclude)this[0]).Manager : string.Empty;
            	text = string.Empty;
            	if(QsosCount > 0 && manager != string.Empty)
            	{
            		text = ViaText + " " + manager;
            	}
            	else if(QsosCount == 0)
            	{
            		text = ViaText + " ZZZZZZ";
            	}
            	formattedText = GenerateFormattedText(text, ManagerBrush, FontWeights.Bold);
            	dc.DrawText(formattedText, new Point(x, y));
            }
            // font sizes used in headerYOffset calculation below were determined empirically for
            // best text fit
            double headerYOffset = FontSize < 9.5 ? 0 : (FontSize < 11.0 ? 1 : 2);
            y += textHeight + firstLineYOffset;
            dc.DrawLine(pen, new Point(DisplayX, y),
                        new Point(DisplayX  + DisplayWidth, y));
            x = DisplayX;
            for(int i = 0; i < qsoCount; i++)
            {
            	y += textHeight + lineYOffset;
            	dc.DrawLine(pen, new Point(x, y), new Point(x + DisplayWidth, y));
            }
            x = DisplayX;
            y = DisplayY + textHeight + lineYOffset;
            int lastColumn = 0;
            for(int i=0; i < ((ShowPseTnx) ? colWidths.Length - 1 : colWidths.Length - 2); i++)
            {
            	dc.DrawText(colHeadersText[i], new Point(x + colHeaderX[i], y + headerYOffset));
            	x += colWidths[i];
            	dc.DrawLine(pen, new Point(x, y), new Point(x, DisplayY + DisplayHeight));
            	lastColumn++;
            }
            dc.DrawText(colHeadersText[lastColumn], new Point(x + colHeaderX[lastColumn],
                                                              y + headerYOffset));
            if (IsSelected)
            {
                dc.DrawRoundedRectangle(brush, selectPen, 
            	                        new Rect(DisplayX, DisplayY, DisplayWidth, DisplayHeight),
            	                       cornerRounding, cornerRounding);
            }
            else if(IsHighlighted)
            {
            	dc.DrawRoundedRectangle(brush, hightlightPen, 
            	                        new Rect(DisplayX, DisplayY, DisplayWidth, DisplayHeight),
            	                       cornerRounding, cornerRounding);
            }
        }

		/// <summary>
		/// Handles MouseMove events
		/// </summary>
		/// <param name="e">MouseEventArgs object</param>
		public override void MoveMouse(MouseEventArgs e)
		{
			base.MoveMouse(e);
			if (this.IsSelected)
			{
				if (this.IsLeftMouseButtonDown)
				{
					System.Windows.Point pt = e.GetPosition(QslCard);
					if (cursorLoc == CursorLocation.Inside)
					{
						DisplayX = originalDisplayRectangle.X;
						DisplayY = originalDisplayRectangle.Y + pt.Y - leftMouseDownPoint.Y;
						DisplayWidth = originalDisplayRectangle.Width;
						DisplayHeight = originalDisplayRectangle.Height;
					}
				}
				else
				{
					Cursor cursor = Cursors.Arrow;
					cursorLoc = CursorLocation.Outside;
					System.Windows.Point pt = e.GetPosition(QslCard);
					if ((new Rect(DisplayX, DisplayY, DisplayWidth, DisplayHeight)).Contains(pt))
					{
						cursorLoc = CursorLocation.Inside;
						cursor = Cursors.SizeNS;
					}
					Mouse.OverrideCursor = cursor;
					this.CaptureMouse();
				}
			}
			else
			{
				Mouse.OverrideCursor = Cursors.Arrow;
			}

		}

		/// <summary>
		/// Handler for PropertyChanged event
		/// </summary>
		/// <param name="e">DependencyPropertyChangedEventArgs object. Used to determine
		/// which property changed</param>
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if(e.Property == ShowManagerProperty ||
			   e.Property == ShowFrequencyProperty ||
			   e.Property == ShowPseTnxProperty ||
			   e.Property == MaximumQsosProperty ||
			   e.Property == DateFormatProperty ||
			   e.Property == LineTextBrushProperty ||
			   e.Property == CallsignBrushProperty ||
			   e.Property == ManagerBrushProperty ||
			   e.Property == FontSizeProperty ||
			   e.Property == FontNameProperty ||
			   e.Property == BackgroundBrushProperty ||
			   e.Property == BackgroundOpacityProperty)
			{
				CalculateRectangle();
				if(QslCard != null)		// properties may be set before QslCard is set
				{
					QslCard.IsDirty = true;
				}
				QslCard.InvalidateVisual();
			}
		}
	}
	
	
}
