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
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Drawing.Text;
using System.Windows.Forms;

namespace hamqsler
{
	/// <summary>
	/// Control that displays a CardWF object (QslCard).
	/// </summary>
	public class FormsCardView : Panel
	{
		/// <summary>
		/// Callsign property (displayed as part of ConfirmingText message)
		/// </summary>
		private string callsign = "XXXXXX";
		public string Callsign
		{
			get {return callsign;}
			set {callsign = value;}
		}
		
		/// <summary>
		/// Manager property (displayed as part of ConfirmingText message)
		/// </summary>
		private string manager = "ZZZZZZ";
		public string Manager
		{
			get {return manager;}
			set {manager = value;}
		}
		
		/// <summary>
		/// List of the DispQsos objects containing the data to display in the QsosBox
		/// </summary>
		private List<DispQso> qsos = new List<DispQso>();
		public List<DispQso> Qsos
		{
			get {return qsos;}
			set
			{
				Qsos.Clear();
				if(value != null)
				{
					foreach(DispQso q in value)
					{
						Qsos.Add(q);
					}
					if(Qsos.Count > 0)
					{
						if(QslCard.IsInDesignMode)
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
				}
			}
		}
		
		// Reference to the CardWF that this view displays
		private CardWF qslCard = null;
		public CardWF QslCard
		{
			get {return qslCard;}
			set {qslCard = value;}
		}
		
		private Point cardLocation = new Point(0, 0);
		public Point CardLocation
		{
			get {return cardLocation;}
			set {cardLocation = value;}
		}
		
		private int boxMinimumWidth = 250;
		public int BoxMinimumWidth
		{
			get {return boxMinimumWidth;}
			set {boxMinimumWidth = value;}
		}
		
		private static float[] dashPattern =  {5, 2, 3, 2};
		protected static Pen selectedPen = CreateSelectedPen();
		protected static Pen highlighedPen = CreateHighlightedPen();
		protected static ImageAttributes outsideCardAttrs = CreateOutsideCardImageAttributes();
		private const int boxRoundX = 5;
		private const int boxRoundY = 5;
		
		private System.Windows.Controls.ContextMenu contextMenu = 
			new System.Windows.Controls.ContextMenu();
		private CardWFItem highlightedCardItem;
		
		private static Dictionary<string, string> months = new Dictionary<string, string>();
		private static Dictionary<string, string> bandFreqs = new Dictionary<string, string>();

		/// <summary>
		/// Static constructor - initializes the months and bandFreqs dictionaries
		/// </summary>
		static FormsCardView()
		{
			UserPreferences prefs = ((App)App.Current).UserPreferences;
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
			bandFreqs.Add("630m", prefs.Frequency630m);
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
		/// <param name="card">QslCard to display</param>
		public FormsCardView(CardWF card)
		{
			this.DoubleBuffered = true;
			QslCard = card;
			QslCard.DispPropertyChanged += OnDispPropertyChanged;
			BuildContextMenu();
		}
		
		/// <summary>
		/// Creates the pen used to draw a rectangle around the selected CardWFItem
		/// </summary>
		/// <returns>Pen that highlights a selected CardWFItem</returns>
		public static Pen CreateSelectedPen()
		{
			Pen pen = new Pen(Color.Blue, 4);
			pen.DashPattern = dashPattern;
			return pen;
		}
		
		/// <summary>
		/// Creates the pen used to draw a rectangle around the highlighted CardWFItem
		/// </summary>
		/// <returns>Pen that highlights the selected CardWFItem</returns>
		public static Pen CreateHighlightedPen()
		{
			Pen pen = new Pen(Color.Orange, 4);
			pen.DashPattern = dashPattern;
			return pen;
		}
		
		private CardWFItem.RelativeLocations relativeLocation = 
			CardWFItem.RelativeLocations.Outside;
		private System.Drawing.Point cursorDownLocation = new Point(0, 0);
		private Rectangle originalItemRectangle;
		
		/// <summary>
		/// Creates the ImageAttributes needed to display card items outside the card
		/// at 40% opacity
		/// </summary>
		/// <returns>ImageAttributes object with 40% opacity</returns>
		public static ImageAttributes CreateOutsideCardImageAttributes()
		{
		    // Create a color matrix that is 40% opaque
		    float[][] matrixItems ={ 
		                               new float[] {1, 0, 0, 0, 0},
		                               new float[] {0, 1, 0, 0, 0},
		                               new float[] {0, 0, 1, 0, 0},
		                               new float[] {0, 0, 0, 0.4f, 0}, 
		                               new float[] {0, 0, 0, 0, 1}}; 
		    ColorMatrix colorMatrix = new ColorMatrix(matrixItems);
		    // Create an ImageAttributes object and set its color matrix.
		    ImageAttributes imageAttrs = new ImageAttributes();
		    imageAttrs.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default,
		        ColorAdjustType.Bitmap);
		    return imageAttrs;
		}
		
		/// Draw the card
		/// </summary>
		/// <param name="e">PaintEventArgs object</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			PaintCard(e.Graphics, null);
		}
		
		/// <summary>
		/// Draw the card and its constituent items
		/// </summary>
		/// <param name="g">Graphics object on which to draw the card</param>
		/// <param name="qsos">List of QSOs to paint on the card</param>
		/// <param name="fontAdjustmentFactor">Amount to adjust font size by to get
		/// properly sized text. This should be 1 except when creating card jpegs where
		/// it should be 96F/g.DpiX where g is the Graphics object</param>
		public void PaintCard(Graphics g, List<DispQso> dQsos, float fontAdjustmentFactor = 1)
		{
			Qsos = dQsos;
			g.TextRenderingHint = TextRenderingHint.AntiAlias;
			GraphicsState state;
			g.FillRectangle(Brushes.White, new Rectangle(
				CardLocation.X, CardLocation.Y, QslCard.Width, QslCard.Height));
			float cardX = CardLocation.X;
			float cardY = CardLocation.Y;
			float cardWidth = QslCard.Width;
			float cardHeight = QslCard.Height;
			if(QslCard.CardPrintProperties.SetCardMargins)
			{
				PrinterSettings settings = new PrinterSettings();
				settings.PrinterName = QslCard.CardPrintProperties.PrinterName;
				RectangleF area = settings.DefaultPageSettings.PrintableArea;
				PaperSize paperSize = settings.DefaultPageSettings.PaperSize;
				float margin = Math.Max(area.Left, area.Top);
				margin = Math.Max(margin, paperSize.Width - area.Right);
				margin = Math.Max(margin, paperSize.Height - area.Bottom);
				cardX += margin;
				cardY += margin;
				cardWidth -= 2 * margin;
				cardHeight -= 2 * margin;
			}
			RectangleF clipRect = new RectangleF(cardX, cardY, cardWidth, cardHeight);
			if(QslCard.IsInDesignMode)
			{
				// Create a bitmap containing the card and its items.
				//This is necessary because we want to display the parts of the card
				// items outside the card boundary at 40% opacity, and this can
				// only be done with Graphics.DrawImage, not Graphics.DrawString, etc.
				Bitmap designSurface = new Bitmap(this.Width, this.Height);
				Graphics bGraphics = Graphics.FromImage(designSurface);
				PaintCardItems(bGraphics, Qsos, fontAdjustmentFactor);
				// create a graphics path that excludes the card itself
				GraphicsPath path = new GraphicsPath();
				path.AddRectangle(new RectangleF(this.Location.X, this.Location.Y, 
				                                 this.Width, this.Height));
				path.AddRectangle(clipRect);
				state = g.Save();
				g.Clip = new Region(path);
				g.DrawImage(designSurface, 
				                     new Rectangle(0, 0, this.Width, this.Height),
				                     0, 0, this.Width, this.Height, GraphicsUnit.Pixel,
				                     outsideCardAttrs);
				g.Restore(state);
				bGraphics.Dispose();
				designSurface.Dispose();
			}
			// now draw the card items on the card.
			state = g.Save();
			g.Clip = new Region(clipRect);
			PaintCardItems(g, Qsos, fontAdjustmentFactor);
			g.Restore(state);
			// paint the card outline if requested
			if(QslCard.CardPrintProperties.PrintCardOutlines)
			{
				g.DrawRectangle(Pens.Black, new Rectangle(
					CardLocation.X, CardLocation.Y, QslCard.Width - 1, QslCard.Height - 1));
			}

		}
		
		/// <summary>
		/// Helper method that paints each card item on the card
		/// </summary>
		/// <param name="g">Graphics object on which to paint the card</param>
		/// <param name="qsos">List of QSOs to paint on the card</param>
		/// <param name="fontAdjustmentFactor">Amount to adjust font sizes by. This should
		/// be one except for creating jpegs where it should be 96F / resolution</param>
		private void PaintCardItems(Graphics g, List<DispQso> qsos, float fontAdjustmentFactor)
		{
			// paint the background image
			PaintImage(g, QslCard.BackgroundImage);
			// paint secondary images
			foreach(SecondaryWFImage sImage in QslCard.SecondaryImages)
			{
				PaintImage(g, sImage);
			}
			// paint text items
			foreach(TextWFItem tItem in QslCard.TextItems)
			{
				tItem.CalculateRectangle(g, qsos, fontAdjustmentFactor);
				PaintTextItem(g, qsos, tItem, fontAdjustmentFactor);
			}
			if(QslCard.QsosBox != null)
			{
				QslCard.QsosBox.CalculateRectangle(g, qsos.Count, fontAdjustmentFactor);
				PaintQsosBox(g, qsos, QslCard.QsosBox, fontAdjustmentFactor);
			}
		}
		
		/// <summary>
		/// Handles CardWF property change events
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void OnDispPropertyChanged(object sender, EventArgs e)
		{
			this.Invalidate();
		}
		
		/// <summary>
		/// Paint the background image on the card.
		/// </summary>
		/// <param name="g">Graphics object on which to do the drawing</param>
		/// <param name="image">BackgorundImage object to draw</param>
		private void PaintImage(Graphics g, ImageWFBase image)
		{
			if(image != null && 
			   image.ImageFileName != null && 
			   image.ImageFileName != string.Empty)
			{
				g.DrawImage(image.Image, 
				            new Rectangle(image.X + CardLocation.X,
				                          image.Y + cardLocation.Y, 
				                          image.Width, image.Height));
			}
			if(QslCard.IsInDesignMode)
			{
				if(image.IsHighlighted)
				{
					g.DrawRectangle(highlighedPen, new Rectangle(
						image.X + CardLocation.X, image.Y + CardLocation.Y, 
						image.Width, image.Height));
				}
				else if(image.IsSelected)
				{
					g.DrawRectangle(selectedPen, new Rectangle(
						image.X + CardLocation.X, image.Y + CardLocation.Y, 
						image.Width, image.Height));
				}
			}
		}
		
		/// <summary>
		/// Paint the text item on the card.
		/// </summary>
		/// <param name="g">Graphics object on which to do the drawing</param>
		/// <param name="qsos">List of QSOs that will be printed in the QSOsBox</param>
		/// <param name="tItem">TextWFItem object to draw</param>
		/// <param name="fontAdjustmentFactor">amount to adjust font sizes by. This should be
		/// 1 except for images where it should be 96F/resolution</param>
		private void PaintTextItem(Graphics g, List<DispQso> qsos, TextWFItem tItem,
		                          float fontAdjustmentFactor)
		{
			FontStyle style = FontStyle.Regular;
			if(tItem.IsBold)
			{
				style = FontStyle.Bold;
			}
			if(tItem.IsItalic)
			{
				style |= FontStyle.Italic;
			}
			Font font = new Font(new FontFamily(tItem.TextFontFace), 
			                     tItem.FontSize * fontAdjustmentFactor,
				         		 style, GraphicsUnit.Point);
			int startTextX = CardLocation.X + tItem.X + tItem.Height + 4;
			g.DrawString(tItem.Text.GetText(QslCard, qsos, QslCard.IsInDesignMode),
			             font, tItem.TextBrush, startTextX, CardLocation.Y + tItem.Y);
			float checkBoxSize = (float)tItem.Height * tItem.CheckBoxRelativeSize;
			float margin = (tItem.Height - checkBoxSize) / 2 + 2;
			if(tItem.CheckboxBefore)
			{
				Pen pen = new Pen(tItem.TextBrush, tItem.CheckboxLineThickness);
				g.DrawRectangle(pen, CardLocation.X + tItem.X + margin, 
				                CardLocation.Y +tItem.Y + (tItem.Height - checkBoxSize) / 2,
				                checkBoxSize, checkBoxSize);
				pen.Dispose();
			}
			if(tItem.CheckboxAfter)
			{
				Pen pen = new Pen(tItem.TextBrush, tItem.CheckboxLineThickness);
				g.DrawRectangle(pen, CardLocation.X + tItem.X + tItem.Width -
				                tItem.Height + margin - 2,
				                 CardLocation.Y + tItem.Y + (tItem.Height - checkBoxSize) / 2,
				                checkBoxSize, checkBoxSize);
				pen.Dispose();
			}
			if(QslCard.IsInDesignMode)
			{
				if(tItem.IsHighlighted)
				{
					g.DrawRectangle(highlighedPen, new Rectangle(
						CardLocation.X + tItem.X, CardLocation.Y + tItem.Y, 
						tItem.Width, tItem.Height));
				}
				else if(tItem.IsSelected)
				{
					g.DrawRectangle(selectedPen, new Rectangle(
						CardLocation.X + tItem.X, CardLocation.Y + tItem.Y, 
						tItem.Width, tItem.Height));
				}
			}
		}
		
		/// <summary>
		/// Paint the qsos box on the card.
		/// </summary>
		/// <param name="g">Graphics object on which to do the drawing</param>
		/// <param name="qsos">List of QSOs to paint on the card</param>
		/// <param name="qBox">QsosWFBox object to draw</param>
		/// <param name="fontAdjustmentFactor">amount to adjust font sizes by. This should be
		/// 1 except for images where it should be 96F/resolution</param>
		private void PaintQsosBox(Graphics g, List<DispQso> qsos, QsosWFBox qBox,
		                         float fontAdjustmentFactor)
		{
			Pen pen = new Pen(qBox.LineTextBrush, 1);
			g.SmoothingMode = SmoothingMode.AntiAlias;
			Font font = new Font(new System.Drawing.FontFamily(
				qBox.FontName), qBox.FontSize * fontAdjustmentFactor, 
				FontStyle.Regular, GraphicsUnit.Point);
			List<string> colHeaders = CreateColumnHeaders(qBox);
			List<float> colWidths = CreateColumnWidths(g, font, colHeaders, qBox);
			GraphicsPath path = CreateOutsideBoxPath(qBox);
			FillQsosBoxBackground(g, path, qBox);
			CreateInsideBoxPath(g, ref path, font, colHeaders, colWidths, qBox);
			g.DrawPath(pen, path);
			DrawConfirmingText(g, font, qsos, qBox, fontAdjustmentFactor);
			DrawHeaders(g, font, colHeaders, colWidths, qBox);
			DrawQsos(g, qsos, font, colWidths, qBox);
			pen.Dispose();
			if(QslCard.IsInDesignMode)
			{
				if(qBox.IsHighlighted)
				{
					g.DrawRectangle(highlighedPen, new Rectangle(
						CardLocation.X + qBox.X, CardLocation.Y + qBox.Y, 
						qBox.Width, qBox.Height));
				}
				else if(qBox.IsSelected)
				{
					g.DrawRectangle(selectedPen, new Rectangle(
						CardLocation.X + qBox.X, CardLocation.Y + qBox.Y, 
						qBox.Width, qBox.Height));
				}
			}
		}
		
		/// <summary>
		/// Create a Graphics path object for the outside of the QSOs Box
		/// </summary>
		/// <param name="box">Qsos Box that we are drawing</param>
		/// <returns>GraphicsPath object describing the outside of the Qsos Box</returns>
		private GraphicsPath CreateOutsideBoxPath(QsosWFBox box)
		{
			GraphicsPath path = new GraphicsPath();
			int left = CardLocation.X + box.X;
			int top = CardLocation.Y + box.Y;
			int right = left + box.Width;
			int bottom = top + box.Height;
			path.AddArc(left, top, 2 * boxRoundX, 2 * boxRoundY, 180, 90);
			path.AddLine(left + boxRoundX, top, right - boxRoundX, top);
			path.AddArc(right - 2 * boxRoundX, top, 2 * boxRoundX, 2 * boxRoundY, -90, 90);
			path.AddLine(right, top + boxRoundY, right, bottom -boxRoundY);
			path.AddArc(right - 2 * boxRoundX, bottom - 2 * boxRoundY, 2 * boxRoundX,
			            2 * boxRoundY, 0, 90);
			path.AddLine(right - boxRoundX, bottom, left + boxRoundX, bottom);
			path.AddArc(left , bottom - 2 * boxRoundY, 2 * boxRoundX,
			            2 * boxRoundY, 90, 90);
			path.AddLine(left, bottom - boxRoundY, left, top + boxRoundY);
			return path;
		}
		
		/// <summary>
		/// Fill the Qsos Box background with QsosBox.BackgroundBrush at the
		/// opacity of QsosBox.BackgroundOpacity
		/// </summary>
		/// <param name="g">Graphics object on which to draw the background</param>
		/// <param name="boxOutline">GraphicsPath object describing the outline
		/// of the Qsos box</param>
		/// <param name="box">Qsos box object </param>
		private void FillQsosBoxBackground(Graphics g, GraphicsPath boxOutline, 
		                                   QsosWFBox box)
		{
			// To set the opacity, we must first create a bitmap containing the
			// background at 100% opacity, then draw that on the final Graphics object
			// at the required opacity.
			Bitmap bitmap = new Bitmap(CardLocation.X + box.X + box.Width, 
			                           CardLocation.Y + box.Y + box.Height);
			Graphics bGraphics = Graphics.FromImage(bitmap);
			bGraphics.FillPath(box.BackgroundBrush, boxOutline);
		    // Create a color matrix that is partially opaque
		    float[][] matrixItems ={ 
		                               new float[] {1, 0, 0, 0, 0},
		                               new float[] {0, 1, 0, 0, 0},
		                               new float[] {0, 0, 1, 0, 0},
		                               new float[] {0, 0, 0, (float)box.BackgroundOpacity, 0},
		                               new float[] {0, 0, 0, 0, 1}}; 
		    ColorMatrix colorMatrix = new ColorMatrix(matrixItems);
		    // Create an ImageAttributes object and set its color matrix.
		    ImageAttributes imageAttrs = new ImageAttributes();
		    imageAttrs.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default,
		        ColorAdjustType.Bitmap);
			g.DrawImage(bitmap, new Rectangle(CardLocation.X + box.X,
			                                  CardLocation.Y + box.Y,
			                                  box.Width, box.Height),
			                                  CardLocation.X + box.X,
			                                  CardLocation.Y + box.Y,
			                                  box.Width, box.Height,
			                                  GraphicsUnit.Pixel, imageAttrs);
			bGraphics.Dispose();
			bitmap.Dispose();
		}
		
		/// <summary>
		/// Add the row and column lines to the already created outline of the Qsos box
		/// </summary>
		/// <param name="g">Graphics object that the card is being drawn on</param>
		/// <param name="path">Graphics path containing the outline of the Qsos box</param>
		/// <param name="font">Font that text in the Qsos box is drawn in</param>
		/// <param name="colHeaders">Header text for the columns in the Qsos box</param>
		/// <param name="colWidths">Width of each column in the Qsos box</param>
		/// <param name="box">Qsos box being dreawn</param>
		private void CreateInsideBoxPath(Graphics g, ref GraphicsPath path, Font font, 
		                                 List<string> colHeaders,
		                                 List<float> colWidths, QsosWFBox box)
		{
			int left = CardLocation.X + box.X;
			int top = CardLocation.Y + box.Y;
			int right = left + box.Width;
			int bottom = top + box.Height;
			System.Drawing.SizeF size = g.MeasureString("SampleText", font);
			int lineY = top + (int)size.Height + 3;
			while(lineY < bottom)
			{
				path.AddLine(left, lineY, right, lineY);
				// the following line is needed to prevent diagonal lines between the end
				// of the line just drawn and the start of the next line (if any)
				path.AddLine(right, lineY, left, lineY);
				lineY += (int)size.Height + 3;
			}
			int lineX = left;
			int topY = top + (int)size.Height + 3;
			path.AddLine(left, bottom - (int)size.Height + 3, left, topY);
			for(int col = 0; col < colWidths.Count - 1; col++)
			{
				if(col != colWidths.Count - 2 || box.ShowPseTnx)
				{
					lineX += (int)colWidths[col];
					path.AddLine(lineX, topY, lineX, bottom);
					// the following line is needed to prevent diagonal lines between the end
					// of the line just drawn and the strt of the next line (if any);
					path.AddLine(lineX, bottom, lineX, topY);
				}
			}
		}
		
		/// <summary>
		/// Create a list of Qsos box headers
		/// </summary>
		/// <param name="box">Qsos box for which we are creating the headers</param>
		/// <returns>List of column leaders</returns>
		protected List<string> CreateColumnHeaders(QsosWFBox box)
		{
			List<string> headers = new List<string>();
			switch(box.DateFormat)
			{
				case "YYYY-MM-DD":
					headers.Add(box.YYYYMMDDText.Equals(string.Empty) ? "YYYY-MM-DD" :
					            box.YYYYMMDDText);
					break;
				case "DD-MMM-YY":
					headers.Add(box.DDMMMYYText.Equals(string.Empty) ? "DD-MMM-YY" :
					            box.DDMMMYYText);
					break;
				case "DD-MM-YY":
					headers.Add(box.DDMMYYText.Equals(string.Empty) ? "DD-MM-YY" :
					            box.DDMMYYText);
					break;
			}
			headers.Add(box.TimeText.Equals(string.Empty) ? "Time" : box.TimeText);
			headers.Add(box.BandText.Equals(string.Empty) ? "Band" : box.BandText);
			headers.Add(box.FreqText.Equals(string.Empty) ? "MHz" : box.FreqText);
			headers.Add(box.ModeText.Equals(string.Empty) ? "Mode" : box.ModeText);
			headers.Add(box.RSTText.Equals(string.Empty) ? "RST" : box.RSTText);
			headers.Add(box.QSLText.Equals(string.Empty) ? "QSL" : box.QSLText);
			return headers;
		}
		
		/// <summary>
		/// Create list of column widths
		/// </summary>
		/// <param name="g">Graphics object the card is being drawn on</param>
		/// <param name="font">Font in which text in the Qsos Box will be displayed</param>
		/// <param name="headers">List of column headers</param>
		/// <param name="box">Qsos box for which the column widths are being created</param>
		/// <returns>List of column widths</returns>
		protected List<float> CreateColumnWidths(Graphics g, Font font, List<string> headers, 
		                                         QsosWFBox box)
		{
			List<float> colWidths = new List<float>();
			int width = 0;
			// date column width
			int dateLen = box.DateFormat.Length;
			string dateMeasure = string.Empty;
			for(int i = 0; i < dateLen; i++)
			{
				dateMeasure += "M";
			}
			SizeF size =g.MeasureString(dateMeasure, font);
			colWidths.Add((int)size.Width);
			// time column width
			size = g.MeasureString("000000", font);
			width = (int)size.Width;
			size = g.MeasureString(headers[1], font);
			colWidths.Add(width >= (int)size.Width ? width : (int)size.Width);
			if(box.ShowFrequency)
			{
				colWidths.Add(0);		// band column width
				// assuming frequency limited to 8 characters
				size = g.MeasureString("241000.0", font);
				width = (int)size.Width;
				size = g.MeasureString(headers[3], font);
				colWidths.Add(width >= (int)size.Width ? width : (int)size.Width);
			}
			else
			{
				// assuming 2190m is the largest band width
				size = g.MeasureString("2190m", font);
				width = (int)size.Width;
				size = g.MeasureString(headers[2], font);
				colWidths.Add(width >= (int)size.Width ? width : (int)size.Width);
				colWidths.Add(0);			// frequency column width
			}
			// mode column width
			// assuming mode is limited to 12 characters (e.g. OLIVIA-BEACON)
			size = g.MeasureString("MMMMMMMMMMMM", font);
			colWidths.Add((int)size.Width);
			// rst column width
			// assuming signal report limited to 3 chars
			size = g.MeasureString("599", font);
			width = (int)size.Width;
			size = g.MeasureString(headers[4], font);
			colWidths.Add(width >= (int)size.Width ? width : (int)size.Width);
			// qsl column width
			if(box.ShowPseTnx)
			{
				// take largest width of header text, Pse text and Tnx text
				size = g.MeasureString(headers[5], font);
				width = (int)size.Width;
				size = g.MeasureString(box.PseText, font);
				width = width >= (int)size.Width ? width : (int)size.Width;
				size = g.MeasureString(box.TnxText, font);
				colWidths.Add(width >= (int)size.Width ? width : (int)size.Width);
			}
			else
			{
				colWidths.Add(0);
			}
			float totalColWidths = 0;
			for(int col = 0; col < colWidths.Count; col++)
			{
				totalColWidths += colWidths[col];
			}
			BoxMinimumWidth = (int)(totalColWidths + 5);
			if(box.Width < BoxMinimumWidth)
			{
				box.ItemSize = new Size(BoxMinimumWidth, box.Height);
			}
			float colExpansion = (box.Width - totalColWidths) / (box.ShowPseTnx ? 6: 5);
			for(int col = 0; col < colWidths.Count; col++)
			{
				if(colWidths[col] != 0)
				{
					colWidths[col] += colExpansion;
				}
			}
			return colWidths;
		}
		
		/// <summary>
		/// Draw the header text for the Qsos box
		/// </summary>
		/// <param name="g">Graphics object on which to draw the text</param>
		/// <param name="font">Font to draw the text in</param>
		/// <param name="colHeaders">List of headers to draw</param>
		/// <param name="colWidths">List of widths for the columns</param>
		/// <param name="box">Qsos box object which is being drawn</param>
		private void DrawHeaders(Graphics g, Font font, List<string> colHeaders, 
		                         List<float> colWidths, QsosWFBox box)
		{
			SizeF size = g.MeasureString(colHeaders[0], font);
			float colPosition = CardLocation.X + box.X;
			float textY = (float)(CardLocation.Y + box.Y + size.Height + 3 + 2);
			for(int col = 0; col < colWidths.Count; col++)
			{
				if(colWidths[col] > 0)
				{
					size = g.MeasureString(colHeaders[col], font);
					float startX = colPosition + (colWidths[col] - size.Width) / 2;
					g.DrawString(colHeaders[col], font, box.LineTextBrush,
					             startX, textY);
				}
				colPosition += colWidths[col];
			}
		}
		
		/// <summary>
		/// Draw Qsos box confirming text
		/// </summary>
		/// <param name="g">Graphics object of which to draw the text</param>
		/// <param name="font">Font in which to draw the text</param>
		/// <param name="box">Qsos box being drawn</param>
		/// <param name="fontAdjustmentFactor">amount to adjust font sizes by. This should be
		/// 1 except for images where it should be 96F/resolution</param>
		private void DrawConfirmingText(Graphics g, Font font, List<DispQso> qsos,
		                                QsosWFBox box, float fontAdjustmentFactor)
		{
			int startX = CardLocation.X + box.X + 5;
			int y = CardLocation.Y + box.Y + 1;
			string confText = box.ConfirmingText.GetText(QslCard, qsos, QslCard.IsInDesignMode);
			g.DrawString(confText, font, box.LineTextBrush, startX, y);
			if(box.QslCard.IsInDesignMode || qsos.Count != 0)
			{
				SizeF size = g.MeasureString(confText, font);
				startX += (int)size.Width + 2;
				float fontSize = box.FontSize * fontAdjustmentFactor;
				y++; // compensate for bold
				Font callFont = new Font(box.FontName, fontSize, FontStyle.Bold, GraphicsUnit.Point);
				g.DrawString(Callsign, callFont, box.CallsignBrush, startX, y);
				if(box.ShowManager && !Manager.Equals(string.Empty))
				{
					size = g.MeasureString(Callsign, callFont);
					startX += (int)size.Width;
					g.DrawString(box.ViaText + " " + Manager, callFont, box.ManagerBrush, 
					             startX, y);
				}
			}
		}
		
		/// <summary>
		/// Draw the QSO information onto the Qsos Box
		/// </summary>
		/// <param name="g">Graphics object on which to draw the QSO information</param>
		/// <param name="qsos">QSOs to draw</param>
		/// <param name="font">Font to use to draw the QSO information</param>
		/// <param name="colWidths">List of column widths in the QSOs Box</param>
		/// <param name="box">QsosWFBox object that is being drawn</param>
		private void DrawQsos(Graphics g, List<DispQso> qsos, Font font, 
		                         List<float> colWidths, QsosWFBox box)
		{
			if(qsos != null)
			{
				SizeF size = g.MeasureString("X", font);
				float y = CardLocation.Y + box.Y + 2 * (size.Height + 3) + 1;
				foreach(DispQso qso in qsos)
				{
					float xStart = CardLocation.X + box.X;
					string date = GenerateDateText(qso.Date, box.DateFormat);
					xStart = PrintQsoDataColumnAndAdjustToNextStartColumn(
						g, date, font, box.LineTextBrush, colWidths[0], xStart, y);
					xStart = PrintQsoDataColumnAndAdjustToNextStartColumn(
						g, qso.Time, font, box.LineTextBrush, colWidths[1], xStart, y);
					xStart = PrintQsoDataColumnAndAdjustToNextStartColumn(
						g, qso.Band, font, box.LineTextBrush, colWidths[2], xStart, y);
					string freq = qso.Frequency != string.Empty ?
						qso.Frequency : bandFreqs[qso.Band];
					xStart = PrintQsoDataColumnAndAdjustToNextStartColumn(
						g, freq, font, box.LineTextBrush, colWidths[3], xStart, y);
					string mode = qso.Submode.Equals(string.Empty) ?
						qso.Mode : qso.Submode;
					xStart = PrintQsoDataColumnAndAdjustToNextStartColumn(
						g, mode, font, box.LineTextBrush, colWidths[4], xStart, y);
					xStart = PrintQsoDataColumnAndAdjustToNextStartColumn(
						g, qso.RST, font, box.LineTextBrush, colWidths[5], xStart, y);
					string qsl = string.Empty;
					if(qso.Qsl.Equals("No") || qso.Qsl.Equals("Requested") || qso.Qsl.Equals(string.Empty))
					{
						qsl = box.PseText;
					}
					else
					{
						qsl = box.TnxText;
					}
					xStart = PrintQsoDataColumnAndAdjustToNextStartColumn(
						g, qsl, font, box.LineTextBrush, colWidths[6], xStart, y);
					y += (int)size.Height + 3;
				}
			}
		}
		
		/// <summary>
		/// Generate formatted date text
		/// </summary>
		/// <param name="qsoDate">ADIF style date to format for printing</param>
		/// <param name="dateFormat">Format for the date</param>
		/// <returns>formatted date</returns>
		private string GenerateDateText(string qsoDate, string dateFormat)
		{
			string date = string.Empty;
			if(qsoDate != string.Empty)
			{
				switch (dateFormat) 
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
		/// <param name="g">Graphics object on which to print the data</param>
		/// <param name="text">Text to print</param>
		/// <param name="font">Font in which to print the text</param>
		/// <param name="brush">Brush to use when painting the text</param>
		/// <param name="colWidth">Column width</param>
		/// <param name="y">Y position for the text</param>
		/// <returns>X position of the start of the next column</returns>
		private float PrintQsoDataColumnAndAdjustToNextStartColumn(
			Graphics g, string text, Font font, Brush brush, float colWidth, 
			float xStart, float y)
		{
			if (text != string.Empty && colWidth != 0)
			{
				SizeF size = g.MeasureString(text, font);
				float x = xStart + (colWidth - size.Width) / 2;
				g.DrawString(text, font, brush, x, y);
			}
			xStart += colWidth;
			return xStart;
		}
		
		/// <summary>
		/// Handler for MouseMove events
		/// </summary>
		/// <param name="e">MouseEventArgs object that describes this event</param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			CardWFItem selectedItem = QslCard.GetSelectedItem();
			if(e.Button == MouseButtons.None)
			{
				if(selectedItem == null)
				{
					HighlightCardItem(e.X - CardLocation.X, e.Y - CardLocation.Y);
				}
				else
				{
					this.Cursor = GetCardItemCursor(selectedItem, 
						e.X - CardLocation.X, e.Y - CardLocation.Y);
				}
			}
			else if(e.Button == MouseButtons.Left)
			{
				MoveOrResizeCardItem(selectedItem, e.X - CardLocation.X, 
				                     e.Y - CardLocation.Y);
			}
		}
		
		/// <summary>
		/// Handler for MouseLeave events
		/// </summary>
		/// <param name="e">EventArgs object</param>
		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			// clear IsHighlighted property for every card item on the card.
			// This call is necessary because if the mouse is moved quickly away from the
			// highlighted card item, the card item IsHighlighted property might not be
			// unset.
			ClearHighlights();
		}
		
		/// <summary>
		/// Handler for MouseUp events
		/// </summary>
		/// <param name="e">MouseEventArgs object that describes this event</param>
		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if(e.Button == MouseButtons.Right)
			{
				contextMenu.IsOpen = true;
			}
			else if(e.Button == MouseButtons.Left)
			{
				relativeLocation = CardWFItem.RelativeLocations.Outside;
			}
		}
		
		/// <summary>
		/// Handler for mouse down events
		/// </summary>
		/// <param name="e">MouseEventArgs object that describes this event</param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);
			if(e.Button == MouseButtons.Left)
			{
				CardWFItem ci = QslCard.GetSelectedItem();
				if(ci != null)
				{
					relativeLocation = ci.GetRelativeLocation(
						e.X - CardLocation.X, e.Y - CardLocation.Y);
					cursorDownLocation = new Point(
						e.X - CardLocation.X, e.Y - CardLocation.Y);
					originalItemRectangle = new Rectangle(
						ci.X, ci.Y, ci.Width, ci.Height);
				}
			}
		}
		
		/// <summary>
		/// Helper function that sets the IsHighlighted property of every card item
		/// on the displayed card to false;
		/// </summary>
		protected void ClearHighlights()
		{
			if(QslCard.QsosBox != null)
			{
				QslCard.QsosBox.IsHighlighted = false;
			}
			foreach(TextWFItem tItem in QslCard.TextItems)
			{
				tItem.IsHighlighted = false;
			}
			foreach(SecondaryWFImage sImage in QslCard.SecondaryImages)
			{
				sImage.IsHighlighted = false;
			}
			 QslCard.BackgroundImage.IsHighlighted = false;
		}
		
		/// <summary>
		/// Helper method that builds the card view's context menu
		/// </summary>
		private void BuildContextMenu()
		{
			contextMenu.Opened += OnContextMenuOpen;
			System.Windows.Controls.MenuItem select = 
				new System.Windows.Controls.MenuItem();
			select.Header = "Select Highlighted Item";
			select.Name = "SelectItem";
			select.Click += OnSelectItemClicked;
			contextMenu.Items.Add(select);
			
			System.Windows.Controls.MenuItem  deselect = 
				new System.Windows.Controls.MenuItem();
			deselect.Header = "Deselect Item";
			deselect.Name = "DeselectItem";
			deselect.Click += OnDeselectItemClicked;
			contextMenu.Items.Add(deselect);
			
			contextMenu.Items.Add(new System.Windows.Controls.Separator());
			
			System.Windows.Controls.MenuItem addImage =
				new System.Windows.Controls.MenuItem();
			addImage.Header = "Add Image";
			addImage.InputGestureText = "Ctrl-I";
			addImage.Name = "AddImage";
			addImage.Click += OnAddImageClicked;
			contextMenu.Items.Add(addImage);
			
			System.Windows.Controls.MenuItem addText =
				new System.Windows.Controls.MenuItem();
			addText.Header = "Add Text Item";
			addText.InputGestureText = "Ctrl-T";
			addText.Name = "AddText";
			addText.Click += OnAddTextClicked;
			contextMenu.Items.Add(addText);
			
			System.Windows.Controls.MenuItem addQsosBox = 
				new System.Windows.Controls.MenuItem();
			addQsosBox.Header = "Add Qsos Box";
			addQsosBox.Name = "AddQsosBox";
			addQsosBox.Click += OnAddQsosBoxClicked;
			contextMenu.Items.Add(addQsosBox);
			
			contextMenu.Items.Add(new System.Windows.Controls.Separator());
			
			System.Windows.Controls.MenuItem deleteItem =
				new System.Windows.Controls.MenuItem();
			deleteItem.Header = "Delete Selected Item";
			deleteItem.InputGestureText = "Delete";
			deleteItem.Name = "DeleteItem";
			deleteItem.Click += OnDeleteItemClicked;
			contextMenu.Items.Add(deleteItem);

			System.Windows.Controls.MenuItem clearBackground =
				new System.Windows.Controls.MenuItem();
			clearBackground.Header = "Clear Background Image";
			clearBackground.InputGestureText = "Ctrl-Delete";
			clearBackground.Name = "ClearBackgroundItem";
			clearBackground.Click += OnClearBackgroundClicked;
			contextMenu.Items.Add(clearBackground);
		}
		
		/// <summary>
		/// Handler for context menu's opened event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void OnContextMenuOpen(object sender, System.Windows.RoutedEventArgs e)
		{
			highlightedCardItem = QslCard.GetHighlightedItem();
			CardWFItem selectedCardItem = QslCard.GetSelectedItem();
			// determine the IsEnabled property for each menu item
			foreach(System.Windows.Controls.Control ctrl in contextMenu.Items)
			{
				System.Windows.Controls.MenuItem mi = ctrl as System.Windows.Controls.MenuItem;
				if(mi != null)
				{
					switch(mi.Name)
					{
						case "SelectItem":
							mi.IsEnabled = highlightedCardItem != null;
							break;
						case "DeselectItem":
							mi.IsEnabled = selectedCardItem != null;
							break;
						case "ClearBackgroundItem":
							mi.IsEnabled = QslCard.BackgroundImage.ImageFileName != null &&
								QslCard.BackgroundImage.ImageFileName != string.Empty &&
								(selectedCardItem == null || 
								 selectedCardItem == QslCard.BackgroundImage);
							break;
						case "AddImage":
							mi.IsEnabled = selectedCardItem == null;
							break;
						case "AddText":
							mi.IsEnabled = selectedCardItem == null;
							break;
						case "AddQsosBox":
							mi.IsEnabled = selectedCardItem == null &&
								QslCard.QsosBox == null;
							break;
						case "DeleteItem":
							mi.IsEnabled = selectedCardItem != null &&
								selectedCardItem.GetType() !=
								typeof(BackgroundWFImage);
							break;
					}
				}
			}
		}
		
		/// <summary>
		/// Handler for context menu's Select Item menu item clicked event
		/// </summary>
		/// <param name="sender">menu item that was clicked (Select Item)</param>
		/// <param name="e">RoutedEventArgs object</param>
		private void OnSelectItemClicked(object sender, System.Windows.RoutedEventArgs e)
		{
			// pass processing to MainWindow.OnSelectItem_Clicked
			System.Windows.Controls.MenuItem mi = sender as System.Windows.Controls.MenuItem;
			mi.Tag = highlightedCardItem;
			((MainWindow)App.Current.MainWindow).OnSelectItem_Clicked(sender, e);
		}
		
		/// <summary>
		/// Handler for context menu's Deselect Item menu item clicked event
		/// </summary>
		/// <param name="sender">menu item that was clicked (Deselect Item)</param>
		/// <param name="e">RoutedEventArgs object</param>
		private void OnDeselectItemClicked(object sender, System.Windows.RoutedEventArgs e)
		{
			// pass processing to MainWindow.OnNone_Clicked
			((MainWindow)App.Current.MainWindow).OnNone_Clicked(sender, e);
		}
		
		/// <summary>
		/// Handler for context menu's Clear Background Image menu item clicked event
		/// </summary>
		/// <param name="sender">menu item that was clicked (Clear Background Image)</param>
		/// <param name="e">RoutedEventArgs object</param>
		private void OnClearBackgroundClicked(object sender, System.Windows.RoutedEventArgs e)
		{
			// pass processing to MainWindow.ClearBackgroundCommand_Executed
			((MainWindow)App.Current.MainWindow).ClearBackgroundCommand_Executed(
				sender, null);
		}
		
		/// <summary>
		/// Handler for context menu's Add Image menu item clicked event
		/// </summary>
		/// <param name="sender">menu item that was clicked (Add Image)</param>
		/// <param name="e">RoutedEventArgs object</param>
		private void OnAddImageClicked(object sender, System.Windows.RoutedEventArgs e)
		{
			// pass processing to MainWindow.AddImageCommand_Executed
			((MainWindow)App.Current.MainWindow).AddImageCommand_Executed(
				sender, null);
		}
		
		/// <summary>
		/// Handler for context menu's Add Text Item menu item clicked event
		/// </summary>
		/// <param name="sender">menu item that was clicked (Add Text Item)</param>
		/// <param name="e">RoutedEventArgs object</param>
		private void OnAddTextClicked(object sender, System.Windows.RoutedEventArgs e)
		{
			// pass processing to MainWindow.AddTextCommand_Executed
			((MainWindow)App.Current.MainWindow).AddTextCommand_Executed(
				sender, null);
		}
		
		/// <summary>
		/// Handler for context menu's Add Qsos Box menu item clicked event
		/// </summary>
		/// <param name="sender">menu item that was clicked (Add Qsos Box)</param>
		/// <param name="e">>RoutedEventArgs object</param>
		private void OnAddQsosBoxClicked(object sender, System.Windows.RoutedEventArgs e)
		{
			// pass processing to MainWindow.AddQsosBoxCommand_Executed
			((MainWindow)App.Current.MainWindow).AddQsosBoxCommand_Executed(
				sender, null);
		}
		
		/// <summary>
		/// Handler for context menu's Delete Item menu item clicked event
		/// </summary>
		/// <param name="sender">menu item that was clicked (Delete Item)</param>
		/// <param name="e">>RoutedEventArgs object</param>
		private void OnDeleteItemClicked(object sender, System.Windows.RoutedEventArgs e)
		{
			// pass processing to MainWindow.DeleteItemCommand_Executed
			((MainWindow)App.Current.MainWindow).DeleteItemCommand_Executed(
				sender, null);
		}
		
		/// <summary>
		/// Get the cursor to display when an image card item is selected. The cursor
		/// that is returned is determined by the location of the mouse relative
		/// to the card item.
		/// </summary>
		/// <param name="imageItem">Image card item that is selected</param>
		/// <param name="x">Card relative X coordinate</param>
		/// <param name="y">Card relative Y coordinate</param>
		/// <returns>Cursor to display</returns>
		private Cursor GetCardItemCursor(CardWFItem cardItem, int x, int y)
		{
			Cursor cursor;
			switch(cardItem.GetRelativeLocation(x, y))
			{
				case CardWFItem.RelativeLocations.NW:
				case CardWFItem.RelativeLocations.SE:
					cursor = Cursors.SizeNWSE;
					break;
				case CardWFItem.RelativeLocations.NE:
				case CardWFItem.RelativeLocations.SW:
					cursor = Cursors.SizeNESW;
					break;
				case CardWFItem.RelativeLocations.W:
				case CardWFItem.RelativeLocations.E:
					cursor = Cursors.SizeWE;
					break;
				case CardWFItem.RelativeLocations.Inside:
					cursor = Cursors.SizeAll;
					break;
				default:
					cursor = Cursors.Arrow;
					break;
			}
			return cursor;
		}
		
		/// <summary>
		///  Helper method that highlights the card item that the cursor is over
		/// </summary>
		/// <param name="x">Card relative X cursor coordinate</param>
		/// <param name="y">Card relative Y cursor coordinate</param>
		private void HighlightCardItem(int x, int y)
		{
			ClearHighlights();
			if(QslCard.QsosBox != null)
			{
				if(QslCard.QsosBox.Contains(x, y))
				{
					QslCard.QsosBox.IsHighlighted = true;
					return;
				}
			}
			Array revText = QslCard.TextItems.ToArray();
			for(int index = revText.Length - 1; index >= 0; index--)
			{
				if(((TextWFItem)revText.GetValue(index)).Contains(x, y))
				{
					((TextWFItem)revText.GetValue(index)).IsHighlighted = true;
					 return;
				}
			}
			Array revImages = QslCard.SecondaryImages.ToArray();
			for(int index = revImages.Length - 1; index >= 0; index--)
			{
				if(((SecondaryWFImage)revImages.GetValue(index)).Contains(x, y))
				{
					((SecondaryWFImage)revImages.GetValue(index)).IsHighlighted = true;
					return;
				}
			}
			if(QslCard.BackgroundImage.Contains(x, y))
			{
				QslCard.BackgroundImage.IsHighlighted = true;
			}
		}
		
		/// <summary>
		/// Move or resize card items based on Relative location
		/// </summary>
		/// <param name="cardItem">card item to be moved or resized</param>
		/// <param name="x">Current card relative mouse cursor X coordinate</param>
		/// <param name="y">Current card relative mouse cursor Y coordinate</param>
		private void MoveOrResizeCardItem(CardWFItem cardItem, int x, int y)
		{
			double ratio = (double)originalItemRectangle.Width / 
				(double)originalItemRectangle.Height;
			int minWidth = ratio > 1.0 ? (int)(CardWFItem.MinimumSize * ratio) :
				CardWFItem.MinimumSize;
			int width;
			switch(relativeLocation)
			{
				case CardWFItem.RelativeLocations.Inside:
					cardItem.X = originalItemRectangle.X - cursorDownLocation.X + x;
					cardItem.Y = originalItemRectangle.Y - cursorDownLocation.Y + y;
					break;
				case CardWFItem.RelativeLocations.NW:
					width = originalItemRectangle.Width + cursorDownLocation.X - x;
					cardItem.Width = (width > minWidth) ? width : minWidth;
					cardItem.Height = (int)(cardItem.Width / ratio);
					cardItem.X = originalItemRectangle.X + originalItemRectangle.Width - 
						cardItem.Width;
					cardItem.Y = originalItemRectangle.Y + originalItemRectangle.Height -
						cardItem.Height;
					break;
				case CardWFItem.RelativeLocations.NE:
					width = originalItemRectangle.Width - cursorDownLocation.X + x;
					cardItem.Width = (width > minWidth) ? width : minWidth;
					cardItem.Height = (int)(cardItem.Width / ratio);
					cardItem.Y = originalItemRectangle.Y + originalItemRectangle.Height -
						cardItem.Height;
					break;
				case CardWFItem.RelativeLocations.SW:
					width = originalItemRectangle.Width + cursorDownLocation.X - x;
					cardItem.Width = (width > minWidth) ? width : minWidth;
					cardItem.Height = (int)(cardItem.Width / ratio);
					cardItem.X = originalItemRectangle.X + originalItemRectangle.Width - 
						cardItem.Width;
					break;
				case CardWFItem.RelativeLocations.SE:
					width = originalItemRectangle.Width - cursorDownLocation.X + x;
					cardItem.Width = (width > minWidth) ? width : minWidth;
					cardItem.Height = (int)(cardItem.Width / ratio);
					break;
				case CardWFItem.RelativeLocations.E:
					width = originalItemRectangle.Width - cursorDownLocation.X + x;
					cardItem.Width = width > BoxMinimumWidth ? width : BoxMinimumWidth;
					break;
				case CardWFItem.RelativeLocations.W:
					width = originalItemRectangle.Width + cursorDownLocation.X - x;
					cardItem.Width = width > BoxMinimumWidth ? width : BoxMinimumWidth;
					cardItem.X = originalItemRectangle.X + originalItemRectangle.Width -
						cardItem.Width;
					break;
					
			}
			if(relativeLocation != CardWFItem.RelativeLocations.Outside)
			{
				QslCard.IsDirty = true;
			}
			this.Invalidate();
		}
		
	}
}
