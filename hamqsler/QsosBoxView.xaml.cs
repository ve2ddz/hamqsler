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
			set {SetValue(QsosProperty, value);}
		}
		
		/// <summary>
		/// Date column width property
		/// </summary>
		private static readonly DependencyProperty DateColumnWidthProperty =
			DependencyProperty.Register("DateColumnWidth", typeof(double), typeof(QsosBoxView),
			                            new PropertyMetadata(0.0));
		public double DateColumnWidth
		{
			get {return (double)GetValue(DateColumnWidthProperty);}
			set {SetValue(DateColumnWidthProperty, value);}
		}
		
		/// <summary>
		/// Time column width property
		/// </summary>
		private static readonly DependencyProperty TimeColumnWidthProperty =
			DependencyProperty.Register("TimeColumnWidth", typeof(double), typeof(QsosBoxView),
			                            new PropertyMetadata(0.0));
		public double TimeColumnWidth
		{
			get {return (double)GetValue(TimeColumnWidthProperty);}
			set {SetValue(TimeColumnWidthProperty, value);}
		}
		
		/// <summary>
		/// Band column width property
		/// </summary>
		private static readonly DependencyProperty BandColumnWidthProperty =
			DependencyProperty.Register("BandColumnWidth", typeof(double), typeof(QsosBoxView),
			                            new PropertyMetadata(0.0));
		public double BandColumnWidth
		{
			get {return (double)GetValue(BandColumnWidthProperty);}
			set {SetValue(BandColumnWidthProperty, value);}
		}
		
		/// <summary>
		/// Frequency column width property
		/// </summary>
		private static readonly DependencyProperty FrequencyColumnWidthProperty =
			DependencyProperty.Register("FrequencyColumnWidth", typeof(double), typeof(QsosBoxView),
			                            new PropertyMetadata(0.0));
		public double FrequencyColumnWidth
		{
			get {return (double)GetValue(FrequencyColumnWidthProperty);}
			set {SetValue(FrequencyColumnWidthProperty, value);}
		}
		
		/// <summary>
		/// Mode column width property
		/// </summary>
		private static readonly DependencyProperty ModeColumnWidthProperty =
			DependencyProperty.Register("ModeColumnWidth", typeof(double), typeof(QsosBoxView),
			                            new PropertyMetadata(0.0));
		public double ModeColumnWidth
		{
			get {return (double)GetValue(ModeColumnWidthProperty);}
			set {SetValue(ModeColumnWidthProperty, value);}
		}
		
		/// <summary>
		/// RST column width property
		/// </summary>
		private static readonly DependencyProperty RstColumnWidthProperty =
			DependencyProperty.Register("RstColumnWidth", typeof(double), typeof(QsosBoxView),
			                            new PropertyMetadata(0.0));
		public double RstColumnWidth
		{
			get {return (double)GetValue(RstColumnWidthProperty);}
			set {SetValue(RstColumnWidthProperty, value);}
		}
		
		/// <summary>
		/// Qsl column width property
		/// </summary>
		private static readonly DependencyProperty QslColumnWidthProperty =
			DependencyProperty.Register("QslColumnWidth", typeof(double), typeof(QsosBoxView),
			                            new PropertyMetadata(0.0));
		public double QslColumnWidth
		{
			get {return (double)GetValue(QslColumnWidthProperty);}
			set {SetValue(QslColumnWidthProperty, value);}
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
		public double ZeroOffset
		{
			get {return zeroOffset;}
		}
		
		// Maximum number of characters for display in each column
		private static double MaxTimeLength = 6;
		private static double MaxBandLength = 6;
		private static double MaxFrequencyLength = 8;
		private static double MaxModeLength = 6;
		private static double MaxRstLength = 3;
		private static double MaxQslLength = 3;
		
		// Enumeration for the columns in the QsosBox
		public enum Columns
		{
			DateColumn = 0,
			TimeColumn,
			BandColumn,
			FrequencyColumn,
			ModeColumn,
			RstColumn,
			QslColumn,
			End
		};

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="qsosBox">QsosBox to display</param>
		public QsosBoxView(QsosBox qsosBox) : base(qsosBox)
		{
			SetValue(QsosPropertyKey, new List<DispQso>());
			DataContext = this;
			qsosBox.QBoxView = this;
			InitializeComponent();
			CalculateColumnWidthsAndSetVisibilities();
			if(((QsosBox)ItemData).IsInDesignMode)
			{
				BuildQsos();
			}
		}
		
		/// <summary>
		/// Helper method that builds DispQso objects for use when in design mode.
		/// Adds QsosBox.MaximumQsos Qsos for display 
		/// </summary>
		private void BuildQsos()
		{
			Qsos.Clear();
			for(int i = 0; i < ((QsosBox)ItemData).MaximumQsos; i++)
			{
				Qsos.Add(new DispQso());
			}
		}
		
		/// <summary>
		/// Calculates the width of each column.
		/// Sets the visibility of the Band and Frequency columns based on setting of QsosBox.ShowFrequency.
		/// Sets the visibility of the Qsl column based on setting of QsosBox.ShowPseTnx.
		/// </summary>
		public void CalculateColumnWidthsAndSetVisibilities()
		{
			
			double[] colWidths = new Double[(int)Columns.End];
			QsosBox qBox = (QsosBox)ItemData;
			colWidths[(int)Columns.DateColumn] = qBox.YYYYMMDDText.Length;
			if(qBox.DateFormat == "DD-MMM-YY")
			{
				colWidths[(int)Columns.DateColumn] = qBox.DDMMMYYText.Length;
			}
			else if(qBox.DateFormat == "DD-MM-YY")
			{
				colWidths[(int)Columns.DateColumn] = qBox.DDMMYYText.Length;
			}
			colWidths[(int)Columns.TimeColumn] = MaxTimeLength;
			if(qBox.ShowFrequency)
			{
				colWidths[(int)Columns.BandColumn] = 0;
				colWidths[(int)Columns.FrequencyColumn] = MaxFrequencyLength;
				BandColumn.Visibility = Visibility.Collapsed;
				FrequencyColumn.Visibility = Visibility.Visible;
				DesignBandColumn.Visibility = Visibility.Collapsed;
				DesignFrequencyColumn.Visibility = Visibility.Visible;
			}
			else
			{
				colWidths[(int)Columns.BandColumn] = MaxBandLength;
				colWidths[(int)Columns.FrequencyColumn] = 0;
				BandColumn.Visibility = Visibility.Visible;
				FrequencyColumn.Visibility = Visibility.Collapsed;
				DesignBandColumn.Visibility = Visibility.Visible;
				DesignFrequencyColumn.Visibility = Visibility.Collapsed;
			}
			colWidths[(int)Columns.ModeColumn] = MaxModeLength;
			colWidths[(int)Columns.RstColumn] = MaxRstLength;
			if(qBox.ShowPseTnx)
			{
				colWidths[(int)Columns.QslColumn] = MaxQslLength;
				QslColumn.Visibility = Visibility.Visible;
				DesignQslColumn.Visibility = Visibility.Visible;
			}
			else
			{
				colWidths[(int)Columns.QslColumn] = 0;
				QslColumn.Visibility = Visibility.Collapsed;
				DesignQslColumn.Visibility = Visibility.Collapsed;
			}
			
			double columnsWidth = 0.0;
			for(int i = 0; i < (int)Columns.End; i++)
			{
				columnsWidth += colWidths[i];
			}
			double columnWidthAdjustment = qBox.DisplayWidth / columnsWidth;
			DateColumnWidth = colWidths[(int)Columns.DateColumn] * columnWidthAdjustment;
			TimeColumnWidth = colWidths[(int)Columns.TimeColumn] * columnWidthAdjustment;
			BandColumnWidth = colWidths[(int)Columns.BandColumn] * columnWidthAdjustment;
			FrequencyColumnWidth = colWidths[(int)Columns.FrequencyColumn] * columnWidthAdjustment;
			ModeColumnWidth = colWidths[(int)Columns.ModeColumn] * columnWidthAdjustment;
			RstColumnWidth = colWidths[(int)Columns.RstColumn] * columnWidthAdjustment;
			QslColumnWidth = colWidths[(int)Columns.QslColumn] * columnWidthAdjustment;
		}
		
		/// <summary>
		/// Helper method to handle MouseMove events when the related QsosBox is selected
		/// and the left mouse button is down
		/// </summary>
		/// <param name="e">MouseEventArgs object</param>
		protected override void HandleMouseMoveWithLeftMouseButtonDown(MouseEventArgs e)
		{
			throw new NotImplementedException();
		}
		
		/// <summary>
		/// Helper method to handle MouseMove events when the related QsosBox is selected
		/// but the left mouse button is not down.
		/// </summary>
		/// <param name="e">MouseEventArgs object</param>
		protected override void HandleMouseMoveWithLeftMouseButtonUp(MouseEventArgs e)
		{
//			throw new NotImplementedException();
		}
		
	}
}