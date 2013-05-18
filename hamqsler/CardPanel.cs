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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace hamqsler
{
	/// <summary>
	/// CardPanel displays a CardWF object
	/// </summary>
	public partial class CardPanel : UserControl
	{
		private CardWF qslCard;
		public CardWF QslCard
		{
			get {return qslCard;}
			set {qslCard = value;}
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		public CardPanel()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
		}

		/// <summary>
		/// Create a QSL card and place it in the middle of the card canvas
		/// </summary>
		/// <param name="cardWidth">Width of card in device independent units</param>
		/// <param name="cardHeight">Height of card in device independent units</param>
		public void CreateCard(int cardWidth, int cardHeight)
		{
			AddCard(new CardWF(cardWidth, cardHeight, true));
		}
		
		/// <summary>
		/// Create a FormsCardView to display the QslCard and position the view
		/// in the middle of the CardPanel
		/// </summary>
		/// <param name="card">CardWF object to create view of</param>
		public void AddCard(CardWF card)
		{
			QslCard = card;
			FormsCardView view = new FormsCardView(QslCard);
			view.Location = new Point(0, 0);
			view.Size = new Size(this.Width, this.Height);
			view.CardLocation = new Point((this.Width - QslCard.Width) / 2,
			                          (this.Height - QslCard.Height) / 2);
			this.Controls.Add(view);
		}
		
		/// <summary>
		/// Clear the filename and therefore the image for the card's background image
		/// </summary>
		public void ClearBackgroundImage()
		{
			QslCard.BackgroundImage.ImageFileName = string.Empty;
		}
		
		/// <summary>
		/// Add a SecondaryWFImage to the card
		/// </summary>
		/// <returns>SecondaryWFImage that was added</returns>
		public SecondaryWFImage AddImage()
		{
			SecondaryWFImage sImage = new SecondaryWFImage();
			sImage.QslCard = QslCard;
			sImage.IsSelected = true;
			QslCard.SecondaryImages.Add(sImage);
			return sImage;			
		}
		
		/// <summary>
		/// Add a TextWFItem to the card
		/// </summary>
		/// <returns>TextWFItem that was added</returns>
		public TextWFItem AddTextItem()
		{
			TextWFItem tItem = new TextWFItem();
			tItem.QslCard = QslCard;
			tItem.IsSelected = true;
			StaticText sText = new StaticText();
			sText.Text = "Text Item";
			tItem.Text.Add(sText);
			tItem.CalculateRectangle();
			tItem.X = (QslCard.Width - tItem.Width) / 2;
			tItem.Y = (QslCard.Height - tItem.Height) / 2;
			QslCard.TextItems.Add(tItem);
			return tItem;
		}
		
		/// <summary>
		/// Add a QsosWFBox to the card
		/// </summary>
		/// <returns>QsosWFBox that was added</returns>
		public QsosWFBox AddQsosBox()
		{
			QsosWFBox box = new QsosWFBox(true);
			box.QslCard = QslCard;
			box.IsSelected = true;
			QslCard.QsosBox = box;
			return box;
		}
		
		/// <summary>
		/// Delete the selected card item from the card
		/// </summary>
		public void DeleteItem()
		{
			QslCard.DeleteSelectedItem();			
		}
	}
}
