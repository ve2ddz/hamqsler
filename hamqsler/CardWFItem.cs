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
using System.Drawing;
using System.Windows;

namespace hamqsler
{
	/// <summary>
	/// CardWFItem is the base class for all Card Items in a CardWF object.
	/// </summary>
	public abstract class CardWFItem : DependencyObject
	{
		private static readonly DependencyProperty QslCardProperty =
			DependencyProperty.Register("QslCard", typeof(CardWF), typeof(CardWFItem),
			                            new PropertyMetadata(null));
		public CardWF QslCard
		{
			get {return GetValue(QslCardProperty) as CardWF;}
			set {SetValue(QslCardProperty, value);}
		}
		
		private static readonly DependencyProperty LocationProperty =
			DependencyProperty.Register("Location", typeof(System.Drawing.Point),
			                            typeof(CardWFItem), 
			                            new PropertyMetadata(new System.Drawing.Point(0, 0)));
		public System.Drawing.Point Location
		{
			get {return (System.Drawing.Point)GetValue(LocationProperty);}
			set {SetValue(LocationProperty, value);}
		}
		
		public int X
		{
			get{return Location.X;}
			set
			{
				Location = new System.Drawing.Point(value, Location.Y);
			}
		}
		
		public int Y
		{
			get {return Location.Y;}
			set 
			{
				Location = new System.Drawing.Point(Location.X, value);
			}
		}
		
		private static readonly DependencyProperty ItemSizeProperty =
			DependencyProperty.Register("ItemSize", typeof(System.Drawing.Size),
			                            typeof(CardWFItem),
			                            new PropertyMetadata(new System.Drawing.Size(0, 0)));
		public System.Drawing.Size ItemSize
		{
			get {return (System.Drawing.Size)GetValue(ItemSizeProperty);}
			set {SetValue(ItemSizeProperty, value);}
		}
		
		public int Width
		{
			get {return ItemSize.Width;}
			set 
			{
				ItemSize = new System.Drawing.Size(value, ItemSize.Height);
			}
		}
			
		public int Height
		{
			get {return ItemSize.Height;}
			set 
			{
				ItemSize = new System.Drawing.Size(ItemSize.Width, value);
			}
		}
		
		private static readonly DependencyProperty IsSelectedProperty =
			DependencyProperty.Register("IsSelected", typeof(bool), typeof(CardWFItem),
			                            new PropertyMetadata(false));
		public bool IsSelected
		{
			get {return (bool)GetValue(IsSelectedProperty);}
			set {SetValue(IsSelectedProperty, value);}
		}
		
		private static readonly DependencyProperty IsHighlightedProperty =
			DependencyProperty.Register("IsHighlighed", typeof(bool), typeof(CardWFItem),
			                            new PropertyMetadata(false));
		public bool IsHighlighted
		{
			get {return (bool)GetValue(IsHighlightedProperty);}
			set {SetValue(IsHighlightedProperty, value);}
		}
		
		/// <summary>
		/// Default constructor
		/// </summary>
		public CardWFItem() {}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="width">Card item width</param>
		/// <param name="height">Card item height</param>
		public CardWFItem(int width, int height)
		{
			ItemSize = new System.Drawing.Size(width, height);
		}
		
		/// <summary>
		/// Handler for PropertyChanged event
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if(e.Property == LocationProperty ||
			   e.Property == ItemSizeProperty ||
			   e.Property == IsSelectedProperty ||
			   e.Property == IsHighlightedProperty)
			{
				if(QslCard != null)
				{
					QslCard.RaiseDispPropertyChangedEvent();
				}
			}
		}
		
		public bool Contains(int x, int y)
		{
			Rectangle contents = new Rectangle(this.X, this.Y, this.Width, this.Height);
			return contents.Contains(x, y);
		}
	}
}
