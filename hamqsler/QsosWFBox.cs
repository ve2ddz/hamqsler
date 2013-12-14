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
using System.Xml.Serialization;

namespace hamqsler
{
	/// <summary>
	/// Description of QsosWFBox.
	/// </summary>
	[Serializable]
	public class QsosWFBox : CardWFItem
	{
		// determines horizontal anchor position for QsosBox when the number of QSOs being printed
		// is less than the maximum number of QSOs
		private static readonly DependencyProperty VerticalAnchorPointProperty =
			DependencyProperty.Register("VerticalAnchorPoint", typeof(string),
			                            typeof(QsosWFBox), new PropertyMetadata("Top"));
		public string VerticalAnchorPoint
		{
			get {return GetValue(VerticalAnchorPointProperty) as string;}
			set {SetValue(VerticalAnchorPointProperty, value);}
		}
			
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
		private static readonly DependencyProperty LineTextColorProperty =
			DependencyProperty.Register("LineTextColor", typeof(System.Drawing.Color),
			                            typeof(QsosWFBox), 
			                            new PropertyMetadata(System.Drawing.Color.Black));
		[XmlIgnore]
		public System.Drawing.Color LineTextColor
		{
			get { return (System.Drawing.Color)GetValue(LineTextColorProperty); }
			set { SetValue(LineTextColorProperty, value); }
		}
		
		// the following 3 accessors are required for saving QsoWFBox in XML
		public int LineTextColorRed
		{
			get {return LineTextColor.R;}
			set {LineTextColor = System.Drawing.Color.FromArgb(255, value,
			                                                   LineTextColor.G,
			                                                   LineTextColor.B);}
		}
		
		public int LineTextColorGreen
		{
			get {return LineTextColor.G;}
			set {LineTextColor = System.Drawing.Color.FromArgb(255, LineTextColor.R,
			                                                   value,
			                                                   LineTextColor.B);}
		}
		
		public int LineTextColorBlue
		{
			get {return LineTextColor.B;}
			set {LineTextColor = System.Drawing.Color.FromArgb(255, LineTextColor.R,
			                                                   LineTextColor.G,
			                                                   value);}
		}
		
		[XmlIgnore]
		public System.Drawing.Brush LineTextBrush
		{
			get {return new System.Drawing.SolidBrush(LineTextColor);}
		}

		// Callsign brush for QSO box
		private static readonly DependencyProperty CallsignColorProperty =
			DependencyProperty.Register("CallsignColor", typeof(System.Drawing.Color),
			                            typeof(QsosWFBox), 
			                            new PropertyMetadata(System.Drawing.Color.Red));
		[XmlIgnore]
		public System.Drawing.Color CallsignColor
		{
			get { return (System.Drawing.Color)GetValue(CallsignColorProperty); }
			set { SetValue(CallsignColorProperty, value); }
		}
		
		// the following 3 accessors are required for saving QsoWFBox in XML
		public int CallsignColorRed
		{
			get {return CallsignColor.R;}
			set {CallsignColor = System.Drawing.Color.FromArgb(255, value,
			                                                   CallsignColor.G,
			                                                   CallsignColor.B);}
		}
		
		public int CallsignColorGreen
		{
			get {return CallsignColor.G;}
			set {CallsignColor = System.Drawing.Color.FromArgb(255, CallsignColor.R,
			                                                   value,
			                                                   CallsignColor.B);}
		}
		
		public int CallsignColorBlue
		{
			get {return CallsignColor.B;}
			set {CallsignColor = System.Drawing.Color.FromArgb(255, CallsignColor.R,
			                                                   CallsignColor.G,
			                                                   value);}
		}
		
		[XmlIgnore]
		public System.Drawing.Brush CallsignBrush
		{
			get {return new System.Drawing.SolidBrush(CallsignColor);}
		}

		// Manager brush for QSO box
		private static readonly DependencyProperty ManagerColorProperty =
			DependencyProperty.Register("ManagerColor", typeof(System.Drawing.Color),
			                            typeof(QsosWFBox), 
			                            new PropertyMetadata(System.Drawing.Color.Gray));
		[XmlIgnore]
		public System.Drawing.Color ManagerColor
		{
			get { return (System.Drawing.Color)GetValue(ManagerColorProperty); }
			set { SetValue(ManagerColorProperty, value); }
		}
		
		// the following 3 accessors are required for saving QsoWFBox in XML
		public int ManagerColorRed
		{
			get {return ManagerColor.R;}
			set {ManagerColor = System.Drawing.Color.FromArgb(255, value,
			                                                  ManagerColor.G,
			                                                  ManagerColor.B);}
		}
		
		public int ManagerColorGreen
		{
			get {return ManagerColor.G;}
			set {ManagerColor = System.Drawing.Color.FromArgb(255, ManagerColor.R,
			                                                  value,
			                                                  ManagerColor.B);}
		}
		
		public int ManagerColorBlue
		{
			get {return ManagerColor.B;}
			set {ManagerColor = System.Drawing.Color.FromArgb(255, ManagerColor.R,
			                                                  ManagerColor.G,
			                                                  value);}
		}
		
		[XmlIgnore]
		public System.Drawing.SolidBrush ManagerBrush
		{
			get {return new System.Drawing.SolidBrush(ManagerColor);}
		}
		
		// QsosBox text font size
		private static readonly DependencyProperty FontSizeProperty =
			DependencyProperty.Register("FontSize", typeof(float), typeof(QsosWFBox),
			                            new PropertyMetadata(9.0F));
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
		private static readonly DependencyProperty BackgroundColorProperty =
			DependencyProperty.Register("BackgroundColor", typeof(System.Drawing.Color),
			                            typeof(QsosWFBox),
			                            new PropertyMetadata(System.Drawing.Color.White));
		[XmlIgnore]
		public System.Drawing.Color BackgroundColor
		{
			get { return (System.Drawing.Color)GetValue(BackgroundColorProperty); }
			set { SetValue(BackgroundColorProperty, value); }
		}
		
		// the following 3 accessors are required for saving QsoWFBox in XML
		public int BackgroundColorRed
		{
			get {return BackgroundColor.R;}
			set {BackgroundColor = System.Drawing.Color.FromArgb(255, value,
			                                                     BackgroundColor.G,
			                                                     BackgroundColor.B);}
		}
		
		public int BackgroundColorGreen
		{
			get {return BackgroundColor.G;}
			set {BackgroundColor = System.Drawing.Color.FromArgb(255, BackgroundColor.R,
			                                                     value,
			                                                     BackgroundColor.B);}
		}
		
		public int BackgroundColorBlue
		{
			get {return BackgroundColor.B;}
			set {BackgroundColor = System.Drawing.Color.FromArgb(255, BackgroundColor.R,
			                                                     BackgroundColor.G,
			                                                     value);}
		}
		
		[XmlIgnore]
		public System.Drawing.Brush BackgroundBrush
		{
			get {return new System.Drawing.SolidBrush(BackgroundColor);}
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

		// Text for confirming QSO string
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
        /// Default constructor - should not be called directly, only when deserializing a
        /// card when opening it.
        /// </summary>
		public QsosWFBox()
		{
			InitializeDisplayProperties();
			this.ConfirmingText.Clear();
		}
		
		/// <summary>
		/// Constructor to use exept when deserializing a card
		/// </summary>
		/// <param name="isCreating">used only to distinguish between constructors. Set
		/// this value to true.</param>
		public QsosWFBox(bool isCreating)
		{
			InitializeDisplayProperties();
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
		/// <param name="g">Graphics object the card is being drawn on</param>
		/// <param name="qsosCount">Number of QSOs to be displayed</param>
		/// <param name="fontAdjustmentFactor">amount to adjust font sizes by. This should be
		/// 1 except for images where it should be 96F/resolution</param>
		public void CalculateRectangle(System.Drawing.Graphics g, int qsosCount,
		                              float fontAdjustmentFactor)
		{
			if(QslCard != null)
			{
				int width = this.Width;
				if(this.X == 0 && this.Y == 0 && this.Width == 0 && this.Height == 0)
				{
					this.Location = new System.Drawing.Point(
						QslCard.Width / 20, QslCard.Height / 2);
					width = QslCard.Width * 18 / 20;
				}
				System.Drawing.Font font = 
					new System.Drawing.Font(new System.Drawing.FontFamily(
						this.FontName), this.FontSize * fontAdjustmentFactor,
			        	System.Drawing.FontStyle.Regular, 
			        	System.Drawing.GraphicsUnit.Point);
				System.Drawing.SizeF size =g.MeasureString("SampleText", font);
				if(qsosCount == 0)
				{
					qsosCount = MaximumQsos;
				}
				this.ItemSize = new System.Drawing.Size(width,
				                                        ((int)size.Height + 2) * (2 + qsosCount) + qsosCount + 2);
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
				   e.Property == VerticalAnchorPointProperty ||
				   e.Property == FontNameProperty ||
				   e.Property == FontSizeProperty ||
		           e.Property == ShowPseTnxProperty |
				   e.Property == ShowManagerProperty ||
		           e.Property == ShowFrequencyProperty ||
		           e.Property == DateFormatProperty ||
				   e.Property == MaximumQsosProperty ||
		           e.Property == LineTextColorProperty ||
		           e.Property == CallsignColorProperty ||
		           e.Property == ManagerColorProperty ||
		           e.Property == BackgroundColorProperty ||
		           e.Property == BackgroundOpacityProperty)
				{
					if(QslCard != null)
					{
						QslCard.IsDirty = true;
						QslCard.RaiseDispPropertyChangedEvent();
					}
				}
			}
		}
		
		/// <summary>
		/// Create a QsosWFBox that is a deep copy of this one
		/// </summary>
		/// <returns>QsosWFBox object that is a deep copy of this one</returns>
		public QsosWFBox Clone()
		{
			QsosWFBox box = new QsosWFBox(true);
			box.CopyBaseProperties(this);
			box.ShowManager = this.ShowManager;
			box.ShowFrequency = this.ShowFrequency;
			box.ShowPseTnx = this.ShowPseTnx;
			box.MaximumQsos = this.MaximumQsos;
			box.DateFormat = this.DateFormat;
			box.LineTextColor = this.LineTextColor;
			box.CallsignColor =  this.CallsignColor;
			box.ManagerColor = this.ManagerColor;
			box.FontSize = this.FontSize;
			box.FontName = this.FontName;
			box.BackgroundColor = this.BackgroundColor;
			box.BackgroundOpacity = this.BackgroundOpacity;
			box.ConfirmingText.Clear();
			foreach(TextPart part in this.ConfirmingText)
			{
				box.ConfirmingText.Add(part.Clone());
			}
			box.ViaText = this.ViaText;
			box.YYYYMMDDText = this.YYYYMMDDText;
			box.DDMMMYYText = this.DDMMMYYText;
			box.DDMMYYText = this.DDMMYYText;
			box.TimeText = this.TimeText;
			box.ModeText = this.ModeText;
			box.BandText = this.BandText;
			box.FreqText = this.FreqText;
			box.RSTText = this.RSTText;
			box.QSLText = this.QSLText;
			box.PseText = this.PseText;
			box.TnxText = this.TnxText;
			return box;
		}
	}
}
