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
			                            typeof(QsosBox), new PropertyMetadata(new SolidColorBrush(Colors.Black)));
		public Brush LineTextBrush
		{
			get { return (Brush)GetValue(LineTextBrushProperty); }
			set { SetValue(LineTextBrushProperty, (Brush)value); }
		}

		// Callsign brush for QSO box
		private static readonly DependencyProperty CallsignBrushProperty =
			DependencyProperty.Register("CallsignBrush", typeof(Brush),
			                            typeof(QsosBox), new PropertyMetadata(new SolidColorBrush(Colors.Red)));
		public Brush CallsignBrush
		{
			get { return (Brush)GetValue(CallsignBrushProperty); }
			set { SetValue(CallsignBrushProperty, (Brush)value); }
		}

		// Manager brush for QSO box
		private static readonly DependencyProperty ManagerBrushProperty =
			DependencyProperty.Register("ManagerBrush", typeof(Brush),
			                            typeof(QsosBox), new PropertyMetadata(new SolidColorBrush(Colors.Gray)));
		public Brush ManagerBrush
		{
			get { return (Brush)GetValue(ManagerBrushProperty); }
			set { SetValue(ManagerBrushProperty, (Brush)value); }
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
			                            typeof(QsosBox), new PropertyMetadata(new SolidColorBrush(Colors.Black)));
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
		private static readonly DependencyProperty ConfirmingTextProperty =
			DependencyProperty.Register("ConfirmingText", typeof(string),
			                            typeof(QsosBox), new PropertyMetadata("Confirming 2-Way QSO with"));
		public string ConfirmingText
		{
			get { return (string)GetValue(ConfirmingTextProperty); }
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
		/// <summary>
		/// Retrieves the number of QSOs that will be printed
		/// </summary>
		public int QsosCount
		{
			get { return qsos.Count; }
		}
		
		private static int cornerRounding = 3;

		
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
		/// Helper method that initializes header texts and calculates the size of the box
		/// </summary>
		/// <param name="userPrefs">UserPreferences object used to initialize QsosBox properties</param>
		private void InitializeDisplayProperties(UserPreferences userPrefs)
		{
			double x = QslCard.DisplayRectangle.Width / 20;
			double y = QslCard.DisplayRectangle.Height / 2;
			CalculateRectangle();
			
			this.FontName = userPrefs.DefaultQsosBoxFontFace;
			this.ConfirmingText = userPrefs.ConfirmingText;
			this.ViaText = userPrefs.ViaText;
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
			DisplayRectangle = new Rect(QslCard.DisplayRectangle.Width / 20, 
			                            QslCard.DisplayRectangle.Height / 2,
			                            QslCard.DisplayRectangle.Width * 18 / 20,
			                            height);
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
			                                        FlowDirection.LeftToRight, typeface, 12.0, brush);
			return fText;
		}

        /// <summary>
        /// Handles rendering of the QSOsBox on the card
        /// </summary>
        /// <param name="drawingContext">drawing context to render to</param>
        protected override void OnRender(DrawingContext dc)
        {
            bool isMod = QslCard.IsDirty;
            CalculateRectangle();
            QslCard.IsDirty = isMod;
            Brush brush = Brushes.Transparent;
            Pen pen = new Pen(LineTextBrush, 1);
            // draw the box background
            dc.PushOpacity(BackgroundOpacity);
            dc.DrawRoundedRectangle(brush, pen, DisplayRectangle,
                                   	cornerRounding, cornerRounding);
            dc.Pop();
            if (IsSelected)
            {
                dc.DrawRoundedRectangle(brush, selectPen, DisplayRectangle,
            	                       cornerRounding, cornerRounding);
            }
            else if(IsHighlighted)
            {
            	dc.DrawRoundedRectangle(brush, hightlightPen, DisplayRectangle,
            	                       cornerRounding, cornerRounding);
            }
        }

	}
	
	
}
