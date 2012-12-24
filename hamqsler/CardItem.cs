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
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;

namespace hamqsler
{
	/// <summary>
	/// Abstract class that is the base for Card and all card items.
	/// </summary>
	abstract public class CardItem : DependencyObject
	{
        protected static readonly DependencyProperty DisplayXProperty =
        	DependencyProperty.Register("DisplayX", typeof(double), typeof(CardItem),
        	                            new PropertyMetadata(0.0));
        public virtual double DisplayX
        {
        	get {return (double)GetValue(DisplayXProperty);}
        	set {SetValue(DisplayXProperty, value);}
        }
        
        protected static readonly DependencyProperty DisplayYProperty =
        	DependencyProperty.Register("DisplayY", typeof(double), typeof(CardItem),
        	                            new PropertyMetadata(0.0));
        public virtual double DisplayY
        {
        	get {return (double)GetValue(DisplayYProperty);}
        	set {SetValue(DisplayYProperty, value);}
        }
        
        protected static readonly DependencyProperty DisplayWidthProperty =
        	DependencyProperty.Register("DisplayWidth", typeof(double), typeof(CardItem),
        	                            new PropertyMetadata(0.0));
        public virtual double DisplayWidth
        {
        	get {return (double)GetValue(DisplayWidthProperty);}
        	set {SetValue(DisplayWidthProperty, value);}
        }
        
        protected static readonly DependencyProperty DisplayHeightProperty =
        	DependencyProperty.Register("DisplayHeight", typeof(double), typeof(CardItem),
        	                            new PropertyMetadata(0.0));
        public virtual double DisplayHeight
        {
        	get {return (double)GetValue(DisplayHeightProperty);}
        	set {SetValue(DisplayHeightProperty, value);}
        }
        
        [NonSerialized]
        // Boolean to indicate if if in design or print mode
        protected static readonly DependencyProperty IsInDesignModeProperty =
        	DependencyProperty.Register("IsInDesignMode", typeof(bool), typeof(CardItem),
        	                            new PropertyMetadata(true));
        [XmlIgnore]
        public bool IsInDesignMode
        {
        	get {return (bool)GetValue(IsInDesignModeProperty);}
        	set {SetValue(IsInDesignModeProperty, value);}
        }
        
        protected static readonly DependencyProperty QslCardProperty =
        	DependencyProperty.Register("QslCard", typeof(Card), typeof(CardItem),
        	                            new PropertyMetadata(null));
        [XmlIgnore]
 		public Card QslCard
		{
			get {return (Card)GetValue(QslCardProperty);}
			set {SetValue(QslCardProperty, value);}
		}
		
		/// <summary>
		/// Clip rectangle for this card item 
		/// </summary>
		protected static readonly DependencyProperty ClipRectangleProperty =
			DependencyProperty.Register("ClipRectangle", typeof(Rect), typeof(CardItem),
			                            new PropertyMetadata(new Rect(0, 0, 0, 0)));
		[XmlIgnore]
		public Rect ClipRectangle
		{
			get {return (Rect)GetValue(ClipRectangleProperty);}
			set {SetValue(ClipRectangleProperty, value);}
		}
		
		private static readonly DependencyProperty IsHighlightedProperty =
			DependencyProperty.Register("IsHighlighted", typeof(bool), typeof(CardItem),
			                            new PropertyMetadata(false));
		[XmlIgnore]
		public bool IsHighlighted
		{
			get {return (bool)GetValue(IsHighlightedProperty);}
			set {SetValue(IsHighlightedProperty, value);}
		}
		
		private static readonly DependencyProperty IsSelectedProperty =
			DependencyProperty.Register("IsSelected", typeof(bool), typeof(CardItem),
			                            new PropertyMetadata(false));
		[XmlIgnore]
		public bool IsSelected
		{
			get {return (bool)GetValue(IsSelectedProperty);}
			set {SetValue(IsSelectedProperty, value);}
		}
		
		private bool isLeftMouseButtonDown = false;
		protected bool IsLeftMouseButtonDown
		{
			get {return isLeftMouseButtonDown;}
			set {isLeftMouseButtonDown = value;}
		}
		
		protected Point leftMouseDownPoint = new Point(0, 0);
		protected Rect originalDisplayRectangle;
		
		public enum CursorLocation
		{
			None,
			Outside,
			Inside,
			NW,
			NE,
			SW,
			SE
		};
		
		protected CursorLocation cursorLoc = CursorLocation.Outside;
		
		protected const int cornerSize = 5;
		
				
		/// <summary>
		/// default constructor
		/// </summary>
		/// <param name="isInDesignMode">Boolean to indicate whether this card item is being
		/// created in design or print mode</param>
		public CardItem(bool isInDesignMode = true)
		{
			IsInDesignMode = isInDesignMode;
		}
		
		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="r">Rect object that describes the location and size of the card item</param>
		/// <param name="isInDesignMode">Boolean to indicate whether this card item is being
		/// created in design or print mode</param>
		public CardItem(Rect r, bool isInDesignMode)
		{
			IsInDesignMode = isInDesignMode;
			DisplayX = r.X;
			DisplayY = r.Y;
			DisplayWidth = r.Width;
			DisplayHeight = r.Height;
		}
		
        /// <summary>
        /// Handler for PropertyChanged event
        /// </summary>
        /// <param name="e">DependencyPropertyChangedEventArgs object</param>
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if(e.Property == DisplayXProperty ||
			   e.Property == DisplayYProperty ||
			   e.Property == QslCardProperty)
			{
				// set the ClipRectangle property
				if(QslCard != null)
				{
					ClipRectangle = new Rect(-DisplayX, -DisplayY, QslCard.DisplayWidth,
					                         QslCard.DisplayHeight);
				}
			}
			if((e.Property == DisplayXProperty ||
			   e.Property == DisplayYProperty ||
			   e.Property == DisplayWidthProperty ||
			   e.Property == DisplayHeightProperty) &&
			   QslCard != null)
			{
				QslCard.IsDirty = true;
			}
		}
		
	}
}
