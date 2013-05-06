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
	/// Usercontrol that displays a CardWF object (QslCard).
	/// </summary>
	public partial class FormsCardView : UserControl
	{
		// Reference to the CardItem that this view displays
		private CardWF qslCard;
		public CardWF QslCard
		{
			get {return qslCard;}
			set {qslCard = value;}
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="card">QslCard to display</param>
		public FormsCardView(CardWF card)
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			QslCard = card;
			this.Size = new Size(QslCard.Width, QslCard.Height);
		}
		
		/// <summary>
		/// Draw the card
		/// </summary>
		/// <param name="e">PaintEventArgs object</param>
		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			PaintCard(e.Graphics);
		}
		
		/// <summary>
		/// Draw the card
		/// </summary>
		/// <param name="g">Graphics object on which to draw the card</param>
		public void PaintCard(Graphics g)
		{
			g.FillRectangle(Brushes.White, new Rectangle(
				0, 0, QslCard.Width, QslCard.Height));
			g.DrawRectangle(Pens.Red, new Rectangle(0, 0, QslCard.Width - 1, QslCard.Height - 1));
			g.DrawLine(Pens.Blue, 0, 0, QslCard.Width - 1, QslCard.Height - 1);
			g.DrawLine(Pens.Blue, QslCard.Width - 1, 0, 0, QslCard.Height - 1);
		}
	}
}
