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
using System.Windows.Shapes;

namespace hamqsler
{
	/// <summary>
	/// Interaction logic for CardView.xaml
	/// </summary>
	public partial class CardView : CardItemView
	{
		private Card qslCard;
		public Card QslCard
		{
			get {return qslCard;}
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="card">Qsl card that view will display</param>
		/// <param name="isInDesignMode">bool indicating whether card is being created
		/// in design mode or print mode</param>
		public CardView(Card card, bool isInDesignMode) : base(card)
		{
			InitializeComponent();
			qslCard = card;
			BuildCard();
			
		}
		
		/// <summary>
		/// Create CardItemViews for each CardItem in the card and add to the CardView
		/// </summary>
		private void BuildCard()
		{
			ImageView iView = new ImageView(QslCard.BackImage);
			CanvasForCard.Children.Add(iView);
			foreach(SecondaryImage si in QslCard.SecondaryImages)
			{
				ImageView view = new ImageView(si);
				CanvasForCard.Children.Add(view);
			}
			foreach(TextItem ti in QslCard.TextItems)
			{
				TextItemView view = new TextItemView(ti);
				CanvasForCard.Children.Add(view);
			}
			if(QslCard.QsosBox != null)
			{
				QsosBoxView qView = new QsosBoxView(QslCard.QsosBox);
				CanvasForCard.Children.Add(qView);
			}
		}
		
		/// <summary>
		/// Determine the CardItemView that the mouse cursor is over
		/// </summary>
		/// <returns>The CardItemView the mouse cursor is over</returns>
		public CardItemView GetCardItemViewCursorIsOver(double x, double y)
		{
			// create a List of the CardItemView objects and reverse it.
			// We need to find the last CardItemView under the mouse cursor
			List<FrameworkElement> revFEList = new List<FrameworkElement>();
			foreach(FrameworkElement elt in CanvasForCard.Children)
			{
				revFEList.Add(elt);
			}
			revFEList.Reverse();
			foreach(FrameworkElement fe in revFEList)
			{
				CardItemView civ = fe as CardItemView;
				if(civ != null && civ.CursorIsOverThisView(x, y))
				{
					return civ;
				}
			}
			return null;
		}
		
		/// <summary>
		/// Remove all CardItemView objects from the CardView, then rebuild the CardView
		/// based on the CardItems in the card.
		/// </summary>
		public void RebuildCardView()
		{
			// get a list of all CardItemViews in the CardView
			List<FrameworkElement> children = new List<FrameworkElement>();
			foreach(FrameworkElement elt in CanvasForCard.Children)
			{
				CardItemView view = elt as CardItemView;
				if(view != null)
				{
					children.Add(view);
				}
			}
			// now delete all of the CardItemViews
			foreach(CardItemView view in children)
			{
				CanvasForCard.Children.Remove(view);
			}
			// now rebuild the view
			BuildCard();
		}
	}
}