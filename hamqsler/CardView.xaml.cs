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
	/// Interaction logic for CardView.xaml
	/// </summary>
	public partial class CardView : Canvas
	{
		private static readonly DependencyProperty QslCardProperty =
			DependencyProperty.Register("QslCard", typeof(Card), typeof(CardView),
			                            new PropertyMetadata(null));
		public Card QslCard
		{
			get {return (Card)GetValue(QslCardProperty);}
			set {SetValue(QslCardProperty, value);}
		}
		
		private static readonly DependencyProperty IsDesignModeProperty =
			DependencyProperty.Register("IsDesignMode", typeof(bool), typeof(CardView),
			                            new PropertyMetadata(true));
		public bool IsDesignMode
		{
			get {return (bool)GetValue(IsDesignModeProperty);}
			set {SetValue(IsDesignModeProperty, value);}
		}
		
		public CardView(bool isDesignMode)
		{
			InitializeComponent();
			IsDesignMode = isDesignMode;
		}
		
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if(e.Property == QslCardProperty)
			{
				this.DataContext = QslCard;
				BuildCard();
			}
		}
		
		private void BuildCard()
		{
			foreach(TextItem ti in QslCard.TextItems)
			{
				TextItemView view = new TextItemView();
				view.DataContext = ti;
				Canvas.SetLeft(view, ti.DisplayX);
				Canvas.SetTop(view, ti.DisplayY);
				this.Children.Add(view);
			}
		}
	}
}