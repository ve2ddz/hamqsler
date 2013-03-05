/*
 *  Author:
 *       Jim Orcheson <jimorcheson@gmail.com>
 * 
 *  Copyright (c) 2012, 2013 Jim Orcheson
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
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml;
using System.Xml.Serialization;

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
			                            typeof(QsosBox), new PropertyMetadata(true));
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
		
        /// <summary>
        /// Default constructor - called when deserializing a card file
        /// </summary>
        public QsosBox() : base(true)
        {
			UserPreferences userPrefs = ((App)Application.Current).UserPreferences;
			InitializeDisplayProperties(userPrefs);
			ConfirmingText.Clear();		// must clear or text is entered twice
        }
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="isInDesignMode">Boolean that indicates if the Card is being built
		/// in display or print mode.</param>
        public QsosBox(bool isInDesignMode = true) : base(isInDesignMode)
		{
			UserPreferences userPrefs = ((App)Application.Current).UserPreferences;
			InitializeDisplayProperties(userPrefs);
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
		public void CalculateRectangle()
		{
			if(QslCard != null)
			{
				if(DisplayX == 0 && DisplayY == 0 && DisplayWidth == 0 && DisplayHeight == 0)
				{
					DisplayX = QslCard.DisplayWidth / 20;
					DisplayY = QslCard.DisplayHeight / 2;
				}
				DisplayWidth = QslCard.DisplayWidth * 18 / 20;
				Typeface typeface = new Typeface(new FontFamily(FontName), FontStyles.Normal,
				                                 FontWeights.Normal, FontStretches.Normal);
				CultureInfo culture = CultureInfo.CurrentCulture;
				FormattedText fText = new FormattedText("SampleText", culture,
				                                        culture.TextInfo.IsRightToLeft ?
				                                        FlowDirection.RightToLeft : FlowDirection.LeftToRight,
				                                        typeface, FontSize, LineTextBrush);
				int qCount = MaximumQsos;
				if(CardItemView != null)
				{
					qCount = ((QsosBoxView)CardItemView).Qsos.Count;
				}
				DisplayHeight = (fText.Height + 4) * (2 + qCount);
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
			QsosBoxView view = CardItemView as QsosBoxView;
			if(view != null)
			{
				CalculateRectangle();
				view.CalculateColumnWidths();
				view.BuildColumnHeaders();
			}
			else if(e.Property == QslCardProperty)
			{
				CalculateRectangle();
			}
			if(e.Property == MaximumQsosProperty && view != null)
			{
				view.BuildQsos();
				CalculateRectangle();
			}
			if(QslCard != null && e.Property != IsHighlightedProperty)		// properties may be set before QslCard is set
			{
				QslCard.IsDirty = true;
			}
			if(QslCard != null && QslCard.CardItemView != null)
			{
				QslCard.CardItemView.InvalidateVisual();
			}
		}
		
		/// <summary>
		/// Set properties of this QsosBox from properties loaded from a QslDesignAndPrint card file
		/// </summary>
		/// <param name="itemNode">The QsosBox node</param>
		/// <param name="culture">Culture that the card was written in</param>
		public override void Load(XmlNode itemNode, CultureInfo culture)
		{
			XmlNode node = XmlProcs.GetFirstChildElement(itemNode);
			while(node != null)
			{
				switch(node.Name)
				{
					case "QsosBoxBase":
						LoadQsosBoxBase(node, culture);
						break;
				}
				node = XmlProcs.GetNextSiblingElement(node);
			}
		}
		
		/// <summary>
		/// Load values for this QsosBox from QslDnP card file contents
		/// </summary>
		/// <param name="itemNode">The QsosBox node</param>
		/// <param name="culture">CultureInfo that the card was created in</param>
		private void LoadQsosBoxBase(XmlNode itemNode, CultureInfo culture)
		{
			CountMacro cMacro = new CountMacro();
			cMacro.Count = 1;
			cMacro.CountEquals = true;
			ConfirmingText.Clear();
			ConfirmingText.Add(cMacro);
			StaticText cText = new StaticText();
			cText.Text = "Confirming 2-Way QSO<S> with";
			cMacro.DesignText.Add(cText);
			ConfirmingText.Add(cMacro);
			XmlNode node = XmlProcs.GetFirstChildElement(itemNode);
			XmlNode child;
			while(node != null)
			{
				XmlText text = XmlProcs.GetTextNode(node);
				switch(node.Name)
				{
					case "CardItem":
						base.Load(node, culture);
						break;
					case "ShowManager":
						if(text.Value == "true")
						{
							ShowManager = true;
						}
						else
						{
							ShowManager = false;
						}
						break;
					case "ShowFrequency":
						if(text.Value == "true")
						{
							ShowFrequency = true;
						}
						else
						{
							ShowFrequency = false;
						}
						break;
					case "ShowPseTnx":
						if(text.Value == "true")
						{
							ShowPseTnx = true;
						}
						else
						{
							ShowPseTnx = false;
						}
						break;
					case "MaximumQsos":
						MaximumQsos = Int32.Parse(text.Value, culture);
						break;
					case "DateFormat":
						DateFormat = text.Value;
						break;
					case "LineTextBrush":
						child = XmlProcs.GetFirstChildElement(node);
						LineTextBrush = XmlProcs.ConvertXmlToBrush(child, culture);
						break;
					case "CallsignBrush":
						child = XmlProcs.GetFirstChildElement(node);
						CallsignBrush = XmlProcs.ConvertXmlToBrush(child, culture);
						break;
					case "ManagerBrush":
						child = XmlProcs.GetFirstChildElement(node);
						ManagerBrush = XmlProcs.ConvertXmlToBrush(child, culture);
						break;
					case "FaceName":
						FontName = text.Value;
						break;
					case "BackgroundBrush":
						child = XmlProcs.GetFirstChildElement(node);
						BackgroundBrush = XmlProcs.ConvertXmlToBrush(child, culture);
						break;
					case "BackgroundOpacity":
						double value = 0;
						if(!Double.TryParse(text.Value, NumberStyles.Float, culture, out value))
						{
							if(!Double.TryParse(text.Value, NumberStyles.Float,
							                    CultureInfo.InvariantCulture, out value))
							{
								throw new XmlException("Bad QsosBox property value");
							}
						}
						BackgroundOpacity = value;
						break;
					case "ConfirmingMultiText":
						StaticText cMText = new StaticText();
						cMText.Text = text.Value;
						cMacro.FalseText.Add(cMText);
						break;
					case "Confirming1Text":
						StaticText c1Text = new StaticText();
						c1Text.Text = text.Value;
						cMacro.TrueText.Add(c1Text);
						break;
					case "ViaText":
						ViaText = text.Value;
						break;
					case "YYYYMMDDText":
						YYYYMMDDText = text.Value;
						break;
					case "DDMMMYYText":
						DDMMMYYText = text.Value;
						break;
					case "DDMMYYText":
						DDMMYYText = text.Value;
						break;
					case "TimeText":
						TimeText = text.Value;
						break;
					case "BandText":
						BandText = text.Value;
						break;
					case "FrequencyText":
						FreqText = text.Value;
						break;
					case "RSTText":
						RSTText = text.Value;
						break;
					case "QslText":
						QSLText = text.Value;
						break;
					case "PseText":
						PseText = text.Value;
						break;
					case "TnxText":
						TnxText = text.Value;
						break;
				}
				node = XmlProcs.GetNextSiblingElement(node);
			}
		}
		
		/// <summary>
		/// Make a deep copy of QsosBox properties
		/// </summary>
		/// <param name="card">Card object that this QsosBox belongs to</param>
		/// <param name="box">QsosBox whose properties are to be copied</param>
		public void CopyQsosBoxProperties(Card card, QsosBox box)
		{
			CopyBaseProperties(card, box);
			ShowManager = box.ShowManager;
			ShowFrequency = box.ShowFrequency;
			ShowPseTnx = box.ShowPseTnx;
			MaximumQsos = box.MaximumQsos;
			DateFormat = box.DateFormat;
			LineTextBrush = box.LineTextBrush.Clone();
			CallsignBrush = box.CallsignBrush.Clone();
			ManagerBrush = box.ManagerBrush.Clone();
			FontSize = box.FontSize;
			FontName = box.FontName;
			BackgroundBrush = box.BackgroundBrush.Clone();
			BackgroundOpacity = box.BackgroundOpacity;
			foreach(TextPart part in box.ConfirmingText)
			{
				ConfirmingText.Add(part.Clone());
			}
			ViaText = box.ViaText;
			YYYYMMDDText = box.YYYYMMDDText;
			DDMMMYYText = box.DDMMMYYText;
			DDMMYYText = box.DDMMYYText;
			TimeText = box.TimeText;
			ModeText = box.ModeText;
			BandText = box.BandText;
			FreqText = box.FreqText;
			RSTText = box.RSTText;
			QSLText = box.QSLText;
			PseText = box.PseText;
			TnxText = box.TnxText;
			
		}
	}
	
	
}
