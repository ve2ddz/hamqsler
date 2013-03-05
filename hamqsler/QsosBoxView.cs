/*
 *  Author:
 *       Jim Orcheson <jimorcheson@gmail.com>
 * 
 *  Copyright (c) 2012, 20113 Jim Orcheson
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
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace hamqsler
{
	/// <summary>
	/// Interaction logic for QsosBoxView.xaml
	/// </summary>
	public partial class QsosBoxView : CardItemView
	{
		/// <summary>
		/// Callsign property (displayed as part of ConfirmingText message)
		/// </summary>
		private static readonly DependencyProperty CallsignProperty =
			DependencyProperty.Register("Callsign", typeof(string), typeof(QsosBoxView),
			                            new PropertyMetadata("XXXXXX"));
		public string Callsign
		{
			get {return (string)GetValue(CallsignProperty);}
			set {SetValue(CallsignProperty, value);}
		}
		
		/// <summary>
		/// Manager property (displayed as part of ConfirmingText message)
		/// </summary>
		private static readonly DependencyProperty ManagerProperty =
			DependencyProperty.Register("Manager", typeof(string), typeof(QsosBoxView),
			                            new PropertyMetadata("ZZZZZZ"));
		public string Manager
		{
			get {return (string)GetValue(ManagerProperty);}
			set {SetValue(ManagerProperty, value);}
		}
		
		/// <summary>
		/// List of the DispQsos objects containing the data to display in the QsosBox
		/// </summary>
		private static readonly DependencyPropertyKey QsosPropertyKey =
			DependencyProperty.RegisterReadOnly("Qsos", typeof(List<DispQso>), typeof(QsosBoxView),
			                                    new PropertyMetadata(null));
		private static readonly DependencyProperty QsosProperty =
			QsosPropertyKey.DependencyProperty;
		public List<DispQso> Qsos
		{
			get {return (List<DispQso>)GetValue(QsosProperty);}
			set
			{
				Qsos.Clear();
				foreach(DispQso q in value)
				{
					Qsos.Add(q);
				}
				if(Qsos.Count > 0)
				{
					if(ItemData.IsInDesignMode)
					{
						Manager = "ZZZZZZ";
						Callsign = "XXXXXX";
					}
					else
					{
						Manager = Qsos[0].Manager;
						Callsign = Qsos[0].Callsign;
					}
				}
				((QsosBox)ItemData).CalculateRectangle();
			}
		}
		
		private static readonly DependencyProperty ConfirmingDisplayTextProperty =
			DependencyProperty.Register("ConfirmingDisplayText", typeof(string), typeof(QsosBoxView),
			                            new PropertyMetadata(string.Empty));
		public string ConfirmingDisplayText
		{
			get {return (string)GetValue(ConfirmingDisplayTextProperty);}
			set {SetValue(ConfirmingDisplayTextProperty, value);}
		}
		
		// Amount of rounding for the rectangle that surrounds the QsosBox on the card
		private const double cornerRounding = 3.0;
		public double CornerRounding
		{
			get {return cornerRounding;}
		}
		
		// Offset from left edge of the QsosBox for the start of Confirming text
		private static readonly double confirmingXOffset = 5.0;
		public double ConfirmingXOffset
		{
			get {return confirmingXOffset;}
		}
		
		// Offset from top edge of QsosBox for the confirming text message
		private static readonly  double confirmingTopOffset = 2.0;
		public double ConfirmingTopOffset
		{
			get {return confirmingTopOffset;}
		}
		
		// Offset of confirming text message from the top of the DataGrid
		private static readonly  double confirmingBottomOffset = 0.0;
		public double ConfirmingBottomOffset
		{
			get {return confirmingBottomOffset;}
		}
		
		// Zero offset for use in the confirming text message.
		private static readonly double zeroOffset = 0;
		private static readonly double boxRounding = 3;
		public double ZeroOffset
		{
			get {return zeroOffset;}
		}
		
		// Enumeration for the columns in the QsosBox
		public enum Columns
		{
			DateColumn = 0,
			TimeColumn,
			ModeColumn,
			BandColumn,
			FrequencyColumn,
			RstColumn,
			QslColumn,
			End
		};
		
		private double[] colWidths = new Double[(int)Columns.End];
		private string[] colHeaders = new String[(int)Columns.End];
		private static Dictionary<string, string> months = new Dictionary<string, string>();
		private static Dictionary<string, string> bandFreqs = new Dictionary<string, string>();
		
		/// <summary>
		/// Static constructor
		/// Populates dictionaries with values needed when printing QSOs
		/// </summary>
		static QsosBoxView()
		{
			UserPreferences prefs = ((App)Application.Current).UserPreferences;
			months["01"] = prefs.JanuaryText;
			months["02"] = prefs.FebruaryText;
			months["03"] = prefs.MarchText;
			months["04"] = prefs.AprilText;
			months["05"] = prefs.MayText;
			months["06"] = prefs.JuneText;
			months["07"] = prefs.JulyText;
			months["08"] = prefs.AugustText;
			months["09"] = prefs.SeptemberText;
			months["10"] = prefs.OctoberText;
			months["11"] = prefs.NovemberText;
			months["12"] = prefs.DecemberText;

			bandFreqs.Add("2190m", prefs.Frequency2190m);
			bandFreqs.Add("560m", prefs.Frequency560m);
			bandFreqs.Add("160m", prefs.Frequency160m);
			bandFreqs.Add("80m", prefs.Frequency80m);
			bandFreqs.Add("60m", prefs.Frequency60m);
			bandFreqs.Add("40m", prefs.Frequency40m);
			bandFreqs.Add("30m", prefs.Frequency30m);
			bandFreqs.Add("20m", prefs.Frequency20m);
			bandFreqs.Add("17m", prefs.Frequency17m);
			bandFreqs.Add("15m", prefs.Frequency15m);
			bandFreqs.Add("12m", prefs.Frequency12m);
			bandFreqs.Add("10m", prefs.Frequency10m);
			bandFreqs.Add("6m", prefs.Frequency6m);
			bandFreqs.Add("4m", prefs.Frequency4m);
			bandFreqs.Add("2m", prefs.Frequency2m);
			bandFreqs.Add("1.25m", prefs.Frequency1p25m);
			bandFreqs.Add("70cm", prefs.Frequency70cm);
			bandFreqs.Add("33cm", prefs.Frequency33cm);
			bandFreqs.Add("23cm", prefs.Frequency23cm);
			bandFreqs.Add("13cm", prefs.Frequency13cm);
			bandFreqs.Add("9cm", prefs.Frequency9cm);
			bandFreqs.Add("6cm", prefs.Frequency6cm);
			bandFreqs.Add("3cm", prefs.Frequency3cm);
			bandFreqs.Add("1.25cm", prefs.Frequency1p25cm);
			bandFreqs.Add("6mm", prefs.Frequency6mm);
			bandFreqs.Add("4mm", prefs.Frequency4mm);
			bandFreqs.Add("2.5mm", prefs.Frequency2p5mm);
			bandFreqs.Add("2mm", prefs.Frequency2mm);
			bandFreqs.Add("1mm", prefs.Frequency1mm);
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="qsosBox">QsosBox to display</param>
		public QsosBoxView(QsosBox qsosBox) : base(qsosBox)
		{
			SetValue(QsosPropertyKey, new List<DispQso>());
			qsosBox.CardItemView = this;
			ConfirmingDisplayText = qsosBox.ConfirmingText.GetText(qsosBox.QslCard, Qsos,
			                                                       qsosBox.IsInDesignMode);
			if(((QsosBox)ItemData).IsInDesignMode)
			{
				BuildQsos();
			}
		}
		
		/// <summary>
		/// Build DispQso objects for use when in design mode.
		/// Adds QsosBox.MaximumQsos Qsos for display
		/// </summary>
		public void BuildQsos()
		{
			List<DispQso> qsos = new List<DispQso>();
			for(int i = 0; i < ((QsosBox)ItemData).MaximumQsos; i++)
			{
				qsos.Add(new DispQso());
			}
			Qsos = qsos;
		}
		
		/// <summary>
		/// Get the column headers for the QsosBoxView
		/// </summary>
		public void BuildColumnHeaders()
		{
			QsosBox qsosBox = ItemData as QsosBox;
			switch(qsosBox.DateFormat)
			{
				case "YYYY-MM-DD":
					colHeaders[(int)Columns.DateColumn] = qsosBox.YYYYMMDDText;
					break;
				case "DD-MMM-YY":
					colHeaders[(int)Columns.DateColumn] = qsosBox.DDMMMYYText;
					break;
				case "DD-MM-YY":
					colHeaders[(int)Columns.DateColumn] = qsosBox.DDMMYYText;
					break;
				default:
					colHeaders[(int)Columns.DateColumn] = "Date";
					break;
			}
			colHeaders[(int)Columns.TimeColumn] = qsosBox.TimeText;
			colHeaders[(int)Columns.BandColumn] = qsosBox.BandText;
			colHeaders[(int)Columns.FrequencyColumn] = qsosBox.FreqText;
			colHeaders[(int)Columns.ModeColumn] = qsosBox.ModeText;
			colHeaders[(int)Columns.RstColumn] = qsosBox.RSTText;
			colHeaders[(int)Columns.QslColumn] = qsosBox.QSLText;
		}
		
		/// <summary>
		/// Calculate the width of each column.
		/// </summary>
		public void CalculateColumnWidths()
		{
			QsosBox qBox = (QsosBox)ItemData;
			string dateHeader = qBox.YYYYMMDDText;
			if(qBox.DateFormat == "DD-MMM-YY")
			{
				dateHeader = qBox.DDMMMYYText;
			}
			else if(qBox.DateFormat == "DD-MM-YY")
			{
				dateHeader = qBox.DDMMYYText;
			}
			FormattedText date = GenerateFormattedText(dateHeader, Brushes.Black, FontWeights.Normal);
			colWidths[(int)Columns.DateColumn] = date.Width;
			FormattedText time = GenerateFormattedText("TTTTTT", Brushes.Black, FontWeights.Normal);
			colWidths[(int)Columns.TimeColumn] = time.Width;
			if(qBox.ShowFrequency)
			{
				FormattedText freq = GenerateFormattedText("WWWWWWWW", Brushes.Black, FontWeights.Normal);
				colWidths[(int)Columns.BandColumn] = 0;
				colWidths[(int)Columns.FrequencyColumn] = freq.Width;
			}
			else
			{
				FormattedText band = GenerateFormattedText("XXXXXX", Brushes.Black, FontWeights.Normal);
				colWidths[(int)Columns.BandColumn] = band.Width;
				colWidths[(int)Columns.FrequencyColumn] = 0;
			}
			FormattedText mode = GenerateFormattedText("WWWWWW", Brushes.Black, FontWeights.Normal);
			colWidths[(int)Columns.ModeColumn] = mode.Width;
			FormattedText rst = GenerateFormattedText("WWW", Brushes.Black, FontWeights.Normal);
			colWidths[(int)Columns.RstColumn] =rst.Width;
			if(qBox.ShowPseTnx)
			{
				FormattedText qsl = GenerateFormattedText("WWW", Brushes.Black, FontWeights.Normal);
				colWidths[(int)Columns.QslColumn] = qsl.Width;
			}
			else
			{
				colWidths[(int)Columns.QslColumn] = 0;
			}
			
			double columnsWidth = 0.0;
			int nonZeroColumns = 0;
			for(int i = 0; i < (int)Columns.End; i++)
			{
				if(colWidths[i] != 0)
				{
					columnsWidth += colWidths[i];
					nonZeroColumns++;
				}
			}
			// columnWidthAdjustment is the amount to increase each column by to fill the QsosBox.
			double columnWidthAdjustment = (ItemData.DisplayWidth - columnsWidth) / nonZeroColumns;
			for (int column = 0; column < (int)Columns.End; column++)
			{
				if(colWidths[column] > 0)
				{
					colWidths[column] += columnWidthAdjustment;
				}
			}
		}
		
		/// <summary>
		/// Helper method to handle MouseMove events when the related QsosBox is selected
		/// and the left mouse button is down
		/// </summary>
		/// <param name="view">CardView object that contains this QsosBoxView</param>
		/// <param name="e">MouseEventArgs object</param>
		protected override void HandleMouseMoveWithLeftMouseButtonDown(CardView view, MouseEventArgs e)
		{
			if (cursorLoc == CursorLocation.Inside)
			{
				Point pt = e.GetPosition(view);
				ItemData.DisplayY = originalDisplayRectangle.Y + pt.Y - leftMouseDownPoint.Y;
			}
		}
		
		/// <summary>
		/// Helper method to handle MouseMove events when the related QsosBox is selected
		/// but the left mouse button is not down.
		/// </summary>
		/// <param name="e">MouseEventArgs object</param>
		protected override void HandleMouseMoveWithLeftMouseButtonUp(CardView view, MouseEventArgs e)
		{
			Cursor cursor = Cursors.Arrow;
			cursorLoc = CursorLocation.Outside;
			System.Windows.Point pt = e.GetPosition(view);
			if ((new Rect(ItemData.DisplayX, ItemData.DisplayY, 
			              ItemData.DisplayWidth, ItemData.DisplayHeight)).Contains(pt))
			{
				cursorLoc = CursorLocation.Inside;
				cursor = Cursors.SizeNS;
			}
			Mouse.OverrideCursor = cursor;
		}
		
		/// <summary>
		/// Generates formatted text based on the text

		/// </summary>
		/// <param name="text">Text to generate FormattedText for</param>
		/// <param name="brush">Foreground brush for the text</param>
		/// <param name="weight">FontWeight to create the formatted text in</param>
		/// <returns></returns>
		protected FormattedText GenerateFormattedText(string text, Brush brush, FontWeight weight)
		{
			QsosBox qBox = ItemData as QsosBox;
			Typeface typeface = new Typeface(new FontFamily(((QsosBox)ItemData).FontName),
			                                 FontStyles.Normal, weight, FontStretches.Normal);
			FormattedText fText = new FormattedText(text,
			                                        System.Globalization.CultureInfo.CurrentUICulture,
			                                        FlowDirection.LeftToRight, typeface,
			                                        qBox.FontSize, brush);
			return fText;
		}
		
		/// <summary>
		/// Override for Arrange method. This is necessary to force a call to
		/// CalculateColumnWidth; otherwise, the QsosBox columns
		/// will not be set to the proper widths when first displayed or when printing.
		/// Note: For display, Window.Loaded could be overridden to call
		/// CalculateColumnWidths, but Loaded is not called when a control
		/// is printed.
		/// </summary>
		/// <param name="arrangeBounds">Final area within the parent that this element should
		/// use to arrange itself and its children (just passed to base.ArrangeOverride
		/// in this case.</param>
		/// <returns>Actual size used. Note that this is the size calculated by the base class
		/// because we are not really interested in adjusting this size, just adjusting the
		/// column widths of the QsosBox.</returns>
		protected override Size ArrangeOverride(Size arrangeBounds)
		{
			Size size = base.ArrangeOverride(arrangeBounds);
			this.CalculateColumnWidths();
			return size;
		}
		
		/// <summary>
		/// Render the view for the QsosBox
		/// </summary>
		/// <param name="drawingContext">DrawingContext to render the QsosBoxView on</param>
		protected override void OnRender(DrawingContext drawingContext)
		{
			QsosBox qBox = ItemData as QsosBox;
			Pen pen = new Pen(qBox.LineTextBrush, 1);
			drawingContext.PushOpacity(qBox.BackgroundOpacity);
			drawingContext.DrawRoundedRectangle(qBox.BackgroundBrush, new Pen(qBox.BackgroundBrush, 1),
			                                    new Rect(ItemData.DisplayX, ItemData.DisplayY,
			                                             ItemData.DisplayWidth, ItemData.DisplayHeight),
			                                    boxRounding, boxRounding);
			drawingContext.Pop();
			base.OnRender(drawingContext);
			drawingContext.DrawRoundedRectangle(Brushes.Transparent, pen,
			                                    new Rect(ItemData.DisplayX, ItemData.DisplayY,
			                                             ItemData.DisplayWidth, ItemData.DisplayHeight),
			                                    boxRounding, boxRounding);
			FormattedText fText = GenerateFormattedText(ConfirmingDisplayText, qBox.LineTextBrush,
			                                            FontWeights.Normal);
			double x = ItemData.DisplayX + 5;
			double y = ItemData.DisplayY + 2;
			drawingContext.DrawText(fText, new Point(x, y));
			x += fText.Width + 3;
			string text = Callsign;
			FormattedText callText = GenerateFormattedText(text, qBox.CallsignBrush, FontWeights.Black);
			drawingContext.DrawText(callText, new Point(x, y));
			x += callText.Width + 15;
			if(qBox.ShowManager && Manager != string.Empty)
			{
				FormattedText managerText = GenerateFormattedText(qBox.ViaText + " " + Manager, qBox.ManagerBrush,
				                                                  FontWeights.Black);
				drawingContext.DrawText(managerText, new Point(x, y));
			}
			y += fText.Height + 2;
			drawingContext.DrawLine(pen, new Point(qBox.DisplayX, y),
			                        new Point(qBox.DisplayX + qBox.DisplayWidth, y));
			for(int i = 0; i < Qsos.Count; i++)
			{
				y += fText.Height + 4;
				drawingContext.DrawLine(pen, new Point(qBox.DisplayX, y),
				                        new Point(qBox.DisplayX + qBox.DisplayWidth, y));
			}
			x = qBox.DisplayX;
			y = qBox.DisplayY + fText.Height + 6;
			for(int column = 0; column < (int)Columns.End; column++)
			{
				if(colWidths[column] != 0)
				{
					fText = GenerateFormattedText(colHeaders[column], qBox.LineTextBrush,
					                              FontWeights.Normal);
					drawingContext.DrawText(fText, new Point(x + (colWidths[column] - fText.Width) / 2,
					                                         y));
					x += colWidths[column];
					if(column != (int)Columns.End - 1)
					{
						drawingContext.DrawLine(pen, new Point(x, y - 2),
						                        new Point(x, qBox.DisplayY + qBox.DisplayHeight));
					}
				}
			}
			
			for(int qsoNum = 0; qsoNum < Qsos.Count; qsoNum++)
			{
				y = qBox.DisplayY + (fText.Height + 4) * (qsoNum + 2) + 1;
				double xStart = qBox.DisplayX;
				string date = GenerateDateText(Qsos[qsoNum].Date);
				PrintQsoDataColumnAndAdjustToNextStartColumn(date, y, Columns.DateColumn, 
				                                             ref xStart, drawingContext);
				PrintQsoDataColumnAndAdjustToNextStartColumn(Qsos[qsoNum].Time, y, Columns.TimeColumn,
				                                             ref xStart, drawingContext);
			PrintQsoDataColumnAndAdjustToNextStartColumn(Qsos[qsoNum].Mode, y, Columns.ModeColumn, 
				                                             ref xStart, drawingContext);
				PrintQsoDataColumnAndAdjustToNextStartColumn(Qsos[qsoNum].Band, y, Columns.BandColumn,
				                                             ref xStart, drawingContext);
				if(Qsos[qsoNum].Band != string.Empty ||
				   Qsos[qsoNum].Frequency != string.Empty)
				{
					string freq = Qsos[qsoNum].Frequency;
					if(freq == string.Empty)
					{
						freq = bandFreqs[Qsos[qsoNum].Band];
					}
					PrintQsoDataColumnAndAdjustToNextStartColumn(freq, y, Columns.FrequencyColumn,
					                   ref xStart, drawingContext);
				}
				PrintQsoDataColumnAndAdjustToNextStartColumn(Qsos[qsoNum].RST, y, Columns.RstColumn,
				                                             ref xStart, drawingContext);
				PrintQsoDataColumnAndAdjustToNextStartColumn(Qsos[qsoNum].Qsl, y, Columns.QslColumn, 
				                                             ref xStart, drawingContext);
			}
		}

		/// <summary>
		/// Generate formatted date text
		/// </summary>
		/// <param name="qsoDate">ADIF style date to format for printing</param>
		/// <returns>formatted date</returns>
		private string GenerateDateText(string qsoDate)
		{
			string date = string.Empty;
			if(qsoDate != string.Empty)
			{
				QsosBox qBox = ItemData as QsosBox;
				switch (qBox.DateFormat) 
				{
					case "YYYY-MM-DD":
						date = qsoDate.Substring(0, 4) + "-" + qsoDate.Substring(4, 2) + "-" 
							+ qsoDate.Substring(6, 2);
						break;
					case "DD-MMM-YY":
						date = qsoDate.Substring(6, 2) + "-" + 
							months[qsoDate.Substring(4, 2)] + "-" + 
							qsoDate.Substring(2, 2);
						break;
					case "DD-MM-YY":
						date = qsoDate.Substring(6, 2) + "-" + qsoDate.Substring(4, 2) + "-" 
							+ qsoDate.Substring(2, 2);
						break;
				}
			}
			return date;
		}

		/// <summary>
		/// Print QSO data in specified column
		/// </summary>
		/// <param name="text">data to print</param>
		/// <param name="y">Y position to start printing</param>
		/// <param name="col">Column that data will be printed in</param>
		/// <param name="xStart">X position of the start of the column</param>
		/// <param name="drawingContext"></param>
		private void PrintQsoDataColumnAndAdjustToNextStartColumn(string text, double y, Columns col, 
		                                  ref double xStart, DrawingContext drawingContext)
		{
			if (text != string.Empty && colWidths[(int)col] != 0)
			{
				QsosBox qBox = ItemData as QsosBox;
				FormattedText fText = GenerateFormattedText(text, qBox.LineTextBrush,
				                                            FontWeights.Normal);
				double x = xStart + (colWidths[(int)col] - fText.Width) / 2;
				drawingContext.DrawText(fText, new Point(x, y));
			}
			xStart += colWidths[(int)col];
		}
	}
}