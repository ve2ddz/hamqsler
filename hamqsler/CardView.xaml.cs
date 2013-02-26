/*
 *  Author:
 *       Jim Orcheson <jimorcheson@gmail.com>
 * 
 *  Copyright © 2012, 2013 Jim Orcheson
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
		private static readonly DependencyProperty PrintCardOutlinesProperty =
			DependencyProperty.Register("PrintCardOutlines", typeof(bool), typeof(CardView),
			                            new PropertyMetadata(false));
		public bool PrintCardOutlines
		{
			get {return (bool)GetValue(PrintCardOutlinesProperty);}
			set {SetValue(PrintCardOutlinesProperty, value);}
		}
		
		private static readonly DependencyProperty SetCardMarginsToPrinterMarginsProperty =
			DependencyProperty.Register("SetCardMarginsToPrinterMargins", typeof(bool),
			                            typeof(CardView), new PropertyMetadata(false));
		public bool SetCardMarginsToPrinterMargins
		{
			get {return (bool)GetValue(SetCardMarginsToPrinterMarginsProperty);}
			set {SetValue(SetCardMarginsToPrinterMarginsProperty, value);}
		}
		
		private Card qslCard = null;
		public Card QslCard
		{
			get {return qslCard;}
			set {qslCard = value;}
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="card">Qsl card that view will display</param>
		/// <param name="isInDesignMode">bool indicating whether card is being created
		/// in design mode or print mode</param>
		public CardView(Card card) : base(card)
		{
			qslCard = card;
			DataContext = qslCard;
			InitializeComponent();
			BuildCard();
			
		}
		
		public CardView(Card card, bool showCardOutline, bool showCardMargins, double margin) :
			base(card)
		{
			PrintCardOutlines = showCardOutline;
			SetCardMarginsToPrinterMargins = showCardMargins;
			CardMargin = margin;
			qslCard = card;
			DataContext = qslCard;
			InitializeComponent();
			BuildCard();
		}
		
		/// <summary>
		/// Create CardItemViews for each CardItem in the card and add to the CardView
		/// </summary>
		private void BuildCard()
		{
			ImageView iView = new ImageView(QslCard.BackImage);
			iView.CardMargin = CardMargin;
			CanvasForCard.Children.Add(iView);
			foreach(SecondaryImage si in QslCard.SecondaryImages)
			{
				ImageView view = new ImageView(si);
				view.CardMargin = CardMargin;
				CanvasForCard.Children.Add(view);
			}
			foreach(TextItem ti in QslCard.TextItems)
			{
				TextItemView view = new TextItemView(ti);
				view.CardMargin = CardMargin;
				CanvasForCard.Children.Add(view);
				view.SetDisplayText(null);
			}
			if(QslCard.QsosBox != null)
			{
				QsosBoxView qView = new QsosBoxView(QslCard.QsosBox);
				qView.CardMargin = CardMargin;
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
		/// Retrieve the CardItemView of the selected CardItem
		/// </summary>
		/// <returns>CardItemView corresponding to the selected CardItem, or null if no selection</returns>
		public CardItemView GetCardItemViewForSelectedCardItem()
		{
			foreach(FrameworkElement elt in CanvasForCard.Children)
			{
				CardItemView civ = elt as CardItemView;
				if(civ != null && civ.ItemData.IsSelected)
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