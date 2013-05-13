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
using System.Windows;

namespace hamqsler
{
	/// <summary>
	/// Description of QsosWFBox.
	/// </summary>
	public class QsosWFBox : CardWFItem
	{
		// determines whether to show Manager ("via (Manager)")
		private static readonly DependencyProperty ShowManagerProperty =
			DependencyProperty.Register("ShowManager", typeof(bool),
			                            typeof(QsosWFBox), new PropertyMetadata(true));
		public bool ShowManager
		{
			get { return (bool)GetValue(ShowManagerProperty); }
			set { SetValue(ShowManagerProperty, value); }
		}

		// determines whether to show Band or Frequency
		private static readonly DependencyProperty ShowFrequencyProperty =
			DependencyProperty.Register("ShowFrequency",typeof(bool),
			                            typeof(QsosWFBox), new PropertyMetadata(false));
		public bool ShowFrequency
		{
			get { return (bool)GetValue(ShowFrequencyProperty); }
			set { SetValue(ShowFrequencyProperty, value); }
		}

		// determines whether to include QSL column
		private static readonly DependencyProperty ShowPseTnxProperty =
			DependencyProperty.Register("ShowPseTnx", typeof(bool),
			                            typeof(QsosWFBox), new PropertyMetadata(true));
		public bool ShowPseTnx
		{
			get { return (bool)GetValue(ShowPseTnxProperty); }
			set { SetValue(ShowPseTnxProperty, (bool)value); }
		}

		// maximum QSOs that can be displayed on the card
		private static readonly DependencyProperty MaximumQsosProperty =
			DependencyProperty.Register("MaximumQsos", typeof(int),
			                            typeof(QsosWFBox), new PropertyMetadata(4));
		public int MaximumQsos
		{
			get { return (int)GetValue(MaximumQsosProperty); }
			set { SetValue(MaximumQsosProperty, (int)value); }
		}

		// Format for dates displayed
		private static readonly DependencyProperty DateFormatProperty =
			DependencyProperty.Register("DateFormat", typeof(string),
			                            typeof(QsosWFBox), new PropertyMetadata("DD-MMM-YY"));
		public string DateFormat
		{
			get { return (string)GetValue(DateFormatProperty); }
			set { SetValue(DateFormatProperty, value); }
		}

		// Line and text brush for QSO box
		private static readonly DependencyProperty LineTextBrushProperty =
			DependencyProperty.Register("LineTextBrush", typeof(System.Drawing.Brush),
			                            typeof(QsosWFBox), 
			                            new PropertyMetadata(System.Drawing.Brushes.Black));
		public System.Drawing.Brush LineTextBrush
		{
			get { return (System.Drawing.Brush)GetValue(LineTextBrushProperty); }
			set { SetValue(LineTextBrushProperty, value); }
		}

		// Callsign brush for QSO box
		private static readonly DependencyProperty CallsignBrushProperty =
			DependencyProperty.Register("CallsignBrush", typeof(System.Drawing.Brush),
			                            typeof(QsosWFBox), 
			                            new PropertyMetadata(System.Drawing.Brushes.Red));
		public System.Drawing.Brush CallsignBrush
		{
			get { return (System.Drawing.Brush)GetValue(CallsignBrushProperty); }
			set { SetValue(CallsignBrushProperty, value); }
		}

		// Manager brush for QSO box
		private static readonly DependencyProperty ManagerBrushProperty =
			DependencyProperty.Register("ManagerBrush", typeof(System.Drawing.Brush),
			                            typeof(QsosWFBox), 
			                            new PropertyMetadata(System.Drawing.Brushes.Gray));
		public System.Drawing.Brush ManagerBrush
		{
			get { return (System.Drawing.Brush)GetValue(ManagerBrushProperty); }
			set { SetValue(ManagerBrushProperty, value); }
		}
		
		// QsosBox text font size
		private static readonly DependencyProperty FontSizeProperty =
			DependencyProperty.Register("FontSize", typeof(float), typeof(QsosWFBox),
			                            new PropertyMetadata(10.0F));
		public float FontSize
		{
			get {return (float)GetValue(FontSizeProperty);}
			set {SetValue(FontSizeProperty, value);}
		}

		// Font name for text in QSO box
		private static readonly DependencyProperty FontNameProperty =
			DependencyProperty.Register("FontName", typeof(string),
			                            typeof(QsosWFBox), new PropertyMetadata("Arial"));
		public string FontName
		{
			get { return (string)GetValue(FontNameProperty); }
			set { SetValue(FontNameProperty, (string)value); }
		}


		// Background brush for QSO box
		private static readonly DependencyProperty BackgroundBrushProperty =
			DependencyProperty.Register("BackgroundBrush", typeof(System.Drawing.Brush),
			                            typeof(QsosWFBox),
			                            new PropertyMetadata(System.Drawing.Brushes.White));
		public System.Drawing.Brush BackgroundBrush
		{
			get { return (System.Drawing.Brush)GetValue(BackgroundBrushProperty); }
			set { SetValue(BackgroundBrushProperty, value); }
		}

		// Background opacity for QSO box
		private static readonly DependencyProperty BackgroundOpacityProperty =
			DependencyProperty.Register("BackgroundOpacity", typeof(float),
			                            typeof(QsosWFBox), new PropertyMetadata(1.0F));
		public float BackgroundOpacity
		{
			get { return (float)GetValue(BackgroundOpacityProperty); }
			set { SetValue(BackgroundOpacityProperty, value); }
		}

		// Text for confirming QSO string for single QSO
		private static readonly DependencyPropertyKey ConfirmingTextPropertyKey =
			DependencyProperty.RegisterReadOnly("ConfirmingText", typeof(TextParts),
			                            typeof(QsosWFBox), new PropertyMetadata(null));
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
			                            typeof(QsosWFBox), new PropertyMetadata("via"));
		public string ViaText
		{
			get { return (string)GetValue(ViaTextProperty); }
			set { SetValue(ViaTextProperty, value); }
		}

		/// <summary>
		/// Date column header text for the card when date format is YYYY-MM-DD
		/// </summary>
		private static readonly DependencyProperty YYYYMMDDTextProperty =
			DependencyProperty.Register("YYYYMMDDText", typeof(string), typeof(QsosWFBox),
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
			DependencyProperty.Register("DDMMMYYText", typeof(string), typeof(QsosWFBox),
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
			DependencyProperty.Register("DDMMYYText", typeof(string), typeof(QsosWFBox),
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
			DependencyProperty.Register("TimeText", typeof(string), typeof(QsosWFBox),
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
			DependencyProperty.Register("ModeText", typeof(string), typeof(QsosWFBox),
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
			DependencyProperty.Register("BandText", typeof(string), typeof(QsosWFBox),
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
			DependencyProperty.Register("BFreqText", typeof(string), typeof(QsosWFBox),
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
			DependencyProperty.Register("RSTText", typeof(string), typeof(QsosWFBox),
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
			DependencyProperty.Register("QSLText", typeof(string), typeof(QsosWFBox),
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
			DependencyProperty.Register("PseText", typeof(string), typeof(QsosWFBox),
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
			DependencyProperty.Register("TnxText", typeof(string), typeof(QsosWFBox),
			                            new PropertyMetadata("Tnx"));
		public string TnxText
		{
			get { return (string)GetValue(TnxTextProperty); }
			set { SetValue(TnxTextProperty, value); }
		}
		
        /// <summary>
        /// Default constructor
        /// </summary>
		public QsosWFBox()
		{
			InitializeDisplayProperties();
			CalculateRectangle();
		}
		
		/// <summary>
		/// Get the relative position of the input coordinates within this qsos box
		/// </summary>
		/// <param name="x">Card relative X coordinate</param>
		/// <param name="y">Card relative Y coordinate</param>
		/// <returns>RelativeLocations value</returns>
		public override CardWFItem.RelativeLocations GetRelativeLocation(int x, int y)
		{
			RelativeLocations location = RelativeLocations.Outside;
			if(E.Contains(x, y))
		    {
		   		location = RelativeLocations.E;
		    }
			else if(W.Contains(x, y))
			{
				location = RelativeLocations.W;
			}
			else if(this.Contains(x, y))
			{
				location = RelativeLocations.Inside;
			}
			return location;
		}

		/// <summary>
		/// Calculate the size of the QsosBox based on text size and number of QSOs
		/// </summary>
		public void CalculateRectangle()
		{
			if(QslCard != null)
			{
				if(this.X == 0 && this.Y == 0 && this.Width == 0 && this.Height == 0)
				{
					this.X = QslCard.Width / 20;
					this.Y = QslCard.Height / 2;
				}
				this.Width = QslCard.Width * 18 / 20;
				System.Drawing.Font font = 
					new System.Drawing.Font(new System.Drawing.FontFamily(
						this.FontName), this.FontSize,
			        	System.Drawing.FontStyle.Regular, 
			        	System.Drawing.GraphicsUnit.Point);
				System.Drawing.Size size = System.Windows.Forms.TextRenderer.MeasureText(
					"SampleText", font);
				int qCount = MaximumQsos;
				if(!QslCard.IsInDesignMode)
				{
//					qCount = ((QsosBoxView)CardItemView).Qsos.Count;
				}
				this.Height = (size.Height + 4) * (2 + qCount);
			}
		}
		
		/// <summary>
		/// Helper method that initializes header texts
		/// </summary>
		private void InitializeDisplayProperties()
		{
			UserPreferences userPrefs = ((App)App.Current).UserPreferences;
			this.X = 0;
			this.Y = 0;
			this.Width = 0;
			this.Height = 0;
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
		/// Handler for PropertyChanged event
		/// </summary>
		/// <param name="e">DependencyPropertyChangedEventArgs</param>
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if(QslCard != null)
			{
				if(e.Property == QslCardProperty ||
				   e.Property == FontNameProperty ||
				   e.Property == FontSizeProperty ||
		           e.Property == DateFormatProperty ||
				   e.Property == ShowFrequencyProperty ||
				   e.Property == ShowPseTnxProperty ||
				   e.Property == MaximumQsosProperty)
				{
					CalculateRectangle();
				}
				// FontName and ShowPseTnx changes do not necessarily generate a change in the
				// size of the QsosBox, so we must raise the DispPropertyChanged event
				// to be sure that the font on the display changes.
				if(e.Property == FontNameProperty ||
		           e.Property == ShowPseTnxProperty |
				   e.Property == ShowManagerProperty ||
		           e.Property == ShowFrequencyProperty ||
		           e.Property == DateFormatProperty ||
		           e.Property == LineTextBrushProperty ||
		           e.Property == CallsignBrushProperty ||
		           e.Property == ManagerBrushProperty ||
		           e.Property == BackgroundBrushProperty ||
		           e.Property == BackgroundOpacityProperty)
				{
					if(QslCard != null)
					{
						QslCard.RaiseDispPropertyChangedEvent();
					}
				}
			}
		}
		
	}
}
