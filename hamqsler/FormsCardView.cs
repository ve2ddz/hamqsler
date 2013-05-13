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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Windows.Forms;

namespace hamqsler
{
	/// <summary>
	/// Control that displays a CardWF object (QslCard).
	/// </summary>
	public class FormsCardView : Panel
	{
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
		private Point cursorDownLocation = new Point(0, 0);
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
			PaintCard(e.Graphics);
		}
		
		/// <summary>
		/// Draw the card and its constituent items
		/// </summary>
		/// <param name="g">Graphics object on which to draw the card</param>
		public void PaintCard(Graphics g)
		{
			g.TextRenderingHint = TextRenderingHint.AntiAlias;
			GraphicsState state;
			g.FillRectangle(Brushes.White, new Rectangle(
				CardLocation.X, CardLocation.Y, QslCard.Width, QslCard.Height));
			if(QslCard.IsInDesignMode)
			{
				// Create a bitmap containing the card and its items.
				//This is necessary because we want to display the parts of the card
				// items outside the card boundary at 40% opacity, and this can
				// only be done with Graphics.DrawImage, not Graphics.DrawString, etc.
				Bitmap designSurface = new Bitmap(this.Width, this.Height);
				Graphics bGraphics = Graphics.FromImage(designSurface);
				PaintCardItems(bGraphics);
				// create a graphics path that excludes the card itself
				GraphicsPath path = new GraphicsPath();
				path.AddRectangle(new RectangleF(this.Location.X, this.Location.Y, 
				                                 this.Width, this.Height));
				path.AddRectangle(new RectangleF(CardLocation.X, CardLocation.Y,
				                                 QslCard.Width, QslCard.Height));
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
			RectangleF clipRect = new RectangleF(CardLocation.X, CardLocation.Y, 
			                                     QslCard.Width, QslCard.Height);
			g.Clip = new Region(clipRect);
			PaintCardItems(g);
			// paint the card outline if requested
			if(QslCard.CardPrintProperties.PrintCardOutlines)
			{
				g.DrawRectangle(Pens.Black, new Rectangle(
					CardLocation.X, CardLocation.Y, QslCard.Width - 1, QslCard.Height - 1));
			}
			g.Restore(state);

		}
		
		// Helper method that paints each card item on the card
		private void PaintCardItems(Graphics g)
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
				PaintTextItem(g, tItem);
			}
			if(QslCard.QsosBox != null)
			{
				PaintQsosBox(g, QslCard.QsosBox);
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
		/// <param name="tItem">TextWFItem object to draw</param>
		private void PaintTextItem(Graphics g, TextWFItem tItem)
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
			Font font = new Font(new FontFamily(tItem.TextFontFace), tItem.FontSize,
				         style, GraphicsUnit.Point);
			int startTextX = CardLocation.X + tItem.X + tItem.Height + 4;
			g.DrawString(tItem.Text.GetText(QslCard, null, QslCard.IsInDesignMode),
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
				                tItem.Height + 4,
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
		/// <param name="qBox">QsosWFBox object to draw</param>
		private void PaintQsosBox(Graphics g, QsosWFBox qBox)
		{
			Pen pen = new Pen(qBox.LineTextBrush, 1);
			g.SmoothingMode = SmoothingMode.AntiAlias;
			
			Font font = new Font(new System.Drawing.FontFamily(
				qBox.FontName), qBox.FontSize, FontStyle.Regular, GraphicsUnit.Point);
			List<string> colHeaders = CreateColumnHeaders(qBox);
			List<float> colWidths = CreateColumnWidths(font, colHeaders, qBox);
			GraphicsPath path = CreateOutsideBoxPath(qBox);
			FillQsosBoxBackground(g, path, qBox);
			CreateInsideBoxPath(ref path, font, colHeaders, colWidths, qBox);
			g.DrawPath(pen, path);
			DrawConfirmingText(g, font, qBox);
			DrawHeaders(g, font, colHeaders, colWidths, qBox);
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
		/// <param name="path">Graphics path containing the outline of the Qsos box</param>
		/// <param name="font">Font that text in the Qsos box is drawn in</param>
		/// <param name="colHeaders">Header text for the columns in the Qsos box</param>
		/// <param name="colWidths">Width of each column in the Qsos box</param>
		/// <param name="box">Qsos box being dreawn</param>
		private void CreateInsideBoxPath(ref GraphicsPath path, Font font, 
		                                 List<string> colHeaders,
		                                 List<float> colWidths, QsosWFBox box)
		{
			int left = CardLocation.X + box.X;
			int top = CardLocation.Y + box.Y;
			int right = left + box.Width;
			int bottom = top + box.Height;
			System.Drawing.Size size = System.Windows.Forms.TextRenderer.MeasureText(
				"SampleText", font);
			int lineY = top + size.Height + 4;
			while(lineY < bottom)
			{
				path.AddLine(left, lineY, right, lineY);
				// the following line is needed to prevent diagonal lines between the end
				// of the line just drawn and the start of the next line (if any)
				path.AddLine(right, lineY, left, lineY);
				lineY += size.Height + 4;
			}
			int lineX = left;
			int topY = top + size.Height + 4;
			path.AddLine(left, bottom - size.Height + 4, left, topY);
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
					headers.Add(box.YYYYMMDDText);
					break;
				case "DD-MMM-YY":
					headers.Add(box.DDMMMYYText);
					break;
				case "DD-MM-YY":
					headers.Add(box.DDMMYYText);
					break;
			}
			headers.Add(box.TimeText);
			headers.Add(box.BandText);
			headers.Add(box.FreqText);
			headers.Add(box.ModeText);
			headers.Add(box.RSTText);
			headers.Add(box.QSLText);
			return headers;
		}
		
		/// <summary>
		/// Create list of column widths
		/// </summary>
		/// <param name="font">Font in which text in the Qsos Box will be displayed</param>
		/// <param name="headers">List of column headers</param>
		/// <param name="box">Qsos box for which the column widths are being created</param>
		/// <returns>List of column widths</returns>
		protected List<float> CreateColumnWidths(Font font, List<string> headers, QsosWFBox box)
		{
			List<float> colWidths = new List<float>();
			int width = 0;
			// date column width
			int dateLen = headers[0].Length;
			string dateMeasure = "--";
			for(int i = 0; i < dateLen - 2; i++)
			{
				dateMeasure += "M";
			}
			Size size = TextRenderer.MeasureText(dateMeasure, font);
			colWidths.Add(size.Width);
			// time column width
			size = TextRenderer.MeasureText("000000", font);
			width = size.Width;
			size = TextRenderer.MeasureText(headers[1], font);
			colWidths.Add(width >= size.Width ? width : size.Width);
			if(box.ShowFrequency)
			{
				colWidths.Add(0);		// band column width
				// assuming frequency limited to 8 characters
				size = TextRenderer.MeasureText("241000.0", font);
				width = size.Width;
				size = TextRenderer.MeasureText(headers[3], font);
				colWidths.Add(width >= size.Width ? width : size.Width);
			}
			else
			{
				// assuming 2190m is the largest band width
				size = TextRenderer.MeasureText("2190m", font);
				width = size.Width;
				size = TextRenderer.MeasureText(headers[2], font);
				colWidths.Add(width >= size.Width ? width : size.Width);
				colWidths.Add(0);			// frequency column width
			}
			// mode column width
			// assuming mode is limited to 13 characters (e.g. OLIVIA-BEACON)
			size = TextRenderer.MeasureText("MMMMMMMMMMMMM", font);
			colWidths.Add(size.Width);
			// rst column width
			// assuming signal report limited to 3 chars
			size = TextRenderer.MeasureText("599", font);
			width = size.Width;
			size = TextRenderer.MeasureText(headers[4], font);
			colWidths.Add(width >= size.Width ? width : size.Width);
			// qsl column width
			if(box.ShowPseTnx)
			{
				// take largest width of header text, Pse text and Tnx text
				size = TextRenderer.MeasureText(headers[5], font);
				width = size.Width;
				size = TextRenderer.MeasureText(box.PseText, font);
				width = width >= size.Width ? width : size.Width;
				size = TextRenderer.MeasureText(box.TnxText, font);
				colWidths.Add(width >= size.Width ? width : size.Width);
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
			Size size = TextRenderer.MeasureText(colHeaders[0], font);
			float colPosition = CardLocation.X + box.X;
			float textY = (float)(CardLocation.Y + box.Y + size.Height + 4 + 2);
			for(int col = 0; col < colWidths.Count; col++)
			{
				if(colWidths[col] > 0)
				{
					size = TextRenderer.MeasureText(colHeaders[col], font);
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
		private void DrawConfirmingText(Graphics g, Font font, QsosWFBox box)
		{
			int startX = CardLocation.X + box.X + 5;
			int y = CardLocation.Y + box.Y + 2;
			string confText = box.ConfirmingText.GetText(QslCard, null, QslCard.IsInDesignMode);
			g.DrawString(confText, font, box.LineTextBrush, startX, y);
			Size size = TextRenderer.MeasureText(confText, font);
			startX += size.Width;
			Font callFont = new Font(box.FontName, box.FontSize,FontStyle.Bold);
			string call = "XXXXXX";
			g.DrawString(call, callFont, box.CallsignBrush, startX, y);
			if(box.ShowManager)
			{
				size = TextRenderer.MeasureText(call, callFont);
				startX += size.Width;
				string mgr = "ZZZZZZ";
				g.DrawString(box.ViaText + " " + mgr, callFont, box.ManagerBrush, 
				             startX, y);
			}
				
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
			select.Header = "Select Item";
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
			addImage.Name = "AddImage";
			addImage.Click += OnAddImageClicked;
			contextMenu.Items.Add(addImage);
			
			System.Windows.Controls.MenuItem addText =
				new System.Windows.Controls.MenuItem();
			addText.Header = "Add Text Item";
			addText.Name = "AddText";
			addText.Click += OnAddTextClicked;
			contextMenu.Items.Add(addText);
			
			contextMenu.Items.Add(new System.Windows.Controls.Separator());

			System.Windows.Controls.MenuItem clearBackground =
				new System.Windows.Controls.MenuItem();
			clearBackground.Header = "Clear Background Image";
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
			// pass processing to MainWindow.ClearBackgroundCommand_Executed
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
			// pass processing to MainWindow.ClearBackgroundCommand_Executed
			((MainWindow)App.Current.MainWindow).AddTextCommand_Executed(
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
			this.Invalidate();
		}
		
	}
}
