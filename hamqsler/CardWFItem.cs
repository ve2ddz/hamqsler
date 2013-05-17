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
using System.Xml.Serialization;

namespace hamqsler
{
	/// <summary>
	/// CardWFItem is the base class for all Card Items in a CardWF object.
	/// </summary>
	[Serializable]
	public abstract class CardWFItem : DependencyObject
	{
		protected static readonly DependencyProperty QslCardProperty =
			DependencyProperty.Register("QslCard", typeof(CardWF), typeof(CardWFItem),
			                            new PropertyMetadata(null));
		[XmlIgnore]
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
		
		[XmlIgnore]
		public int X
		{
			get{return Location.X;}
			set
			{
				Location = new System.Drawing.Point(value, Location.Y);
			}
		}
		
		[XmlIgnore]
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
		
		[XmlIgnore]
		public int Width
		{
			get {return ItemSize.Width;}
			set 
			{
				ItemSize = new System.Drawing.Size(value, ItemSize.Height);
			}
		}
			
		[XmlIgnore]
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
		[XmlIgnore]
		public bool IsSelected
		{
			get {return (bool)GetValue(IsSelectedProperty);}
			set {SetValue(IsSelectedProperty, value);}
		}
		
		private static readonly DependencyProperty IsHighlightedProperty =
			DependencyProperty.Register("IsHighlighed", typeof(bool), typeof(CardWFItem),
			                            new PropertyMetadata(false));
		[XmlIgnore]
		public bool IsHighlighted
		{
			get {return (bool)GetValue(IsHighlightedProperty);}
			set {SetValue(IsHighlightedProperty, value);}
		}
		
		private const int SENSITIVESIZE = 10;	// should always be divisible by 2;
		public enum RelativeLocations
		{
			Outside,
			Inside,
			NW,
			NE,
			SW,
			SE,
			E,
			W
		};
		
		private RelativeLocations relativeLocation = RelativeLocations.Inside;
		[XmlIgnore]
		public RelativeLocations RelativeLocation
		{
			get {return relativeLocation;}
			set {relativeLocation = value;}
		}
		
		[XmlIgnore]
		public Rectangle NW
		{
			get {return new Rectangle(this.X - SENSITIVESIZE / 2,
			                          this.Y - SENSITIVESIZE / 2,
			                          SENSITIVESIZE, SENSITIVESIZE);}
		}
		
		[XmlIgnore]
		public Rectangle NE
		{
			get {return new Rectangle(this.X + this.Width - SENSITIVESIZE / 2,
			                          this.Y - SENSITIVESIZE / 2,
			                          SENSITIVESIZE, SENSITIVESIZE);}
		}
		
		[XmlIgnore]
		public Rectangle SW
		{
			get {return new Rectangle(this.X - SENSITIVESIZE / 2,
			                          this.Y + this.Height - SENSITIVESIZE / 2,
			                          SENSITIVESIZE, SENSITIVESIZE);}
		}
		
		[XmlIgnore]
		public Rectangle SE
		{
			get {return new Rectangle(this.X + this.Width - SENSITIVESIZE / 2,
			                          this.Y + this.Height - SENSITIVESIZE / 2,
			                          SENSITIVESIZE, SENSITIVESIZE);}
		}
		
		[XmlIgnore]
		public Rectangle W
		{
			get {return new Rectangle(this.X - SENSITIVESIZE / 2,
			                          this.Y, SENSITIVESIZE, this.Height);}
		}
		
		[XmlIgnore]
		public Rectangle E
		{
			get {return new Rectangle(this.X + this.Width - SENSITIVESIZE / 2,
			                          this.Y, SENSITIVESIZE, this.Height);}
		}
		
		public abstract RelativeLocations GetRelativeLocation(int x, int y);
		[XmlIgnore]
		public static int MinimumSize = 25;

		
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
		
		/// <summary>
		/// Determine if the input coordinates are inside the card item
		/// </summary>
		/// <param name="x">X coordinate</param>
		/// <param name="y">Y coordinate</param>
		/// <returns>true if coordinates are inside this card item, false otherwise</returns>
		public bool Contains(int x, int y)
		{
			Rectangle contents = new Rectangle(this.X, this.Y, this.Width, this.Height);
			return contents.Contains(x, y);
		}
		
		/// <summary>
		/// Copy the CardWFItem properties from one CardWFItem to this one.
		/// </summary>
		/// <param name="cItem">CardWFItem object whose properties are to be copied</param>
		protected void CopyBaseProperties(CardWFItem cItem)
		{
			ItemSize = new System.Drawing.Size(cItem.Width, cItem.Height);
			Location = new System.Drawing.Point(cItem.X, cItem.Y);

		}
	}
}
