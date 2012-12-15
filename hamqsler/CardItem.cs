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

namespace hamqsler
{
	/// <summary>
	/// Abstract class that is the base for Card and all card items.
	/// </summary>
	abstract public class CardItem : UIElement
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
        public bool IsInDesignMode
        {
        	get {return (bool)GetValue(IsInDesignModeProperty);}
        	set {SetValue(IsInDesignModeProperty, value);}
        }
        
        protected static readonly DependencyProperty QslCardProperty =
        	DependencyProperty.Register("QslCard", typeof(Card), typeof(CardItem),
        	                            new PropertyMetadata(null));
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
		public Rect ClipRectangle
		{
			get {return (Rect)GetValue(ClipRectangleProperty);}
			set {SetValue(ClipRectangleProperty, value);}
		}
		
		private static readonly DependencyProperty IsHighlightedProperty =
			DependencyProperty.Register("IsHighlighted", typeof(bool), typeof(CardItem),
			                            new PropertyMetadata(false));
		public bool IsHighlighted
		{
			get {return (bool)GetValue(IsHighlightedProperty);}
			set {SetValue(IsHighlightedProperty, value);}
		}
		
		private static readonly DependencyProperty IsSelectedProperty =
			DependencyProperty.Register("IsSelected", typeof(bool), typeof(CardItem),
			                            new PropertyMetadata(false));
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
		
		public static Pen hightlightPen = CreateHighlightPen();
		public static Pen selectPen = CreateSelectPen();
		
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
			InitializeCardItem();
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
			InitializeCardItem();
			DisplayX = r.X;
			DisplayY = r.Y;
			DisplayWidth = r.Width;
			DisplayHeight = r.Height;
		}
		
		/// <summary>
		/// Perform actions common to all constructors
		/// </summary>
        private void InitializeCardItem()
        {
            this.SnapsToDevicePixels = true;
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
				ClipRectangle = new Rect(-DisplayX, -DisplayY, QslCard.DisplayWidth,
				                         QslCard.DisplayHeight);
			}
		}
		
		/// <summary>
		/// Handle MouseLeftButtonDown event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">MosueButtonEventArgs object</param>
        // Must be public because called from CardCanvas
        public virtual void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (this.IsSelected)
            {
                Point pt = e.GetPosition(QslCard);
                if(GetCursorLocation(pt.X, pt.Y) != CursorLocation.Outside)
                {
               		// only want to capture mouse if cursor within this card item
	                IsLeftMouseButtonDown = true;
	                originalDisplayRectangle = new Rect(DisplayX, DisplayY, DisplayWidth,
	                                                    DisplayHeight);
	                leftMouseDownPoint = pt;
	                this.CaptureMouse();
                }
            }
            e.Handled = true;
        }

		/// <summary>
		/// Handle LeftMouseButtonUp event 
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
        // Must be public because called from CardCanvas
        public virtual void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (this.IsSelected && IsLeftMouseButtonDown)
            {
                IsLeftMouseButtonDown = false;
                this.ReleaseMouseCapture();
            }
        }

		/// <summary>
		/// Handles MouseMove events when mouse is over this CardItem
		/// </summary>
		/// <param name="e">MouseEventArgs object</param>
		public virtual void MoveMouse(MouseEventArgs e) 
		{
			if(!this.IsSelected)
			{
				if(!this.IsHighlighted)
				{
					QslCard.ClearHighlighted();
					this.IsHighlighted = true;
				}
			}		
		}

		/// <summary>
		/// Gets the location within this CardItem
		/// </summary>
		/// <param>
		/// X coordinate
		/// </param>
		/// <param>
		/// Y coordinate
		/// </param>
		/// <returns>
		/// CursorLocation of the coordinates
		/// </returns>
		public CursorLocation GetCursorLocation(double x, double y)
		{
			Rect nw = new Rect(DisplayX - cornerSize,
			                   DisplayY - cornerSize,
			                   2 * cornerSize, 2 * cornerSize);
			Rect ne = new Rect(DisplayX + DisplayWidth - cornerSize,
			                   DisplayY - cornerSize,
			                   2 * cornerSize, 2 * cornerSize);
			Rect se = new Rect(DisplayX + DisplayWidth - cornerSize,
			                   DisplayY + DisplayHeight - cornerSize,
			                   2 * cornerSize, 2 * cornerSize);
			Rect sw = new Rect(DisplayX - cornerSize,
			                   DisplayY + DisplayHeight - cornerSize,
			                   2 * cornerSize, 2 * cornerSize);
			if(WithinRectangle(nw, x, y))
 		    {
			   	return CursorLocation.NW;
		    }
			else if(WithinRectangle(ne, x, y))
			{
				return CursorLocation.NE;
			}
			else if(WithinRectangle(se, x, y))
			{
				return CursorLocation.SE;
			}
			else if(WithinRectangle(sw, x, y))
			{
				return CursorLocation.SW;
			}
			else if(WithinRectangle(new Rect(DisplayX, DisplayY, DisplayWidth, DisplayHeight), x, y))
			{
				return CursorLocation.Inside;
			}
			else
			{
				return CursorLocation.Outside;
			}
		}
		
		/// <summary>
		///  Helper method to determine if x, y is within the rectangle
		/// </summary>
		/// <param>The rectangle to test
		/// <param>
		/// X coordinate to test
		/// </param>
		/// <param>
		/// Y coordinate to test
		/// </param>
		/// <returns>
		/// true if x, y within the rectangle
		/// false if x, y not within the rectangle
		/// </returns>
		public static bool WithinRectangle(Rect r, double x, double y)
		{
			return (x >= r.X) && (x <= r.X + r.Width) && (y >= r.Y) && (y <= r.Y + r.Height);
		}
		
		/// <summary>
		/// Static method that creates the highlight pen
		/// </summary>
		/// <returns>the highlight pen</returns>
		private static Pen CreateHighlightPen()
		{
			Pen hPen = new Pen(Brushes.OrangeRed, 3);
			double[] dashes = {8, 5, 5, 4};
			hPen.DashStyle = new DashStyle(dashes, 0);
			return hPen;
		}
		
		/// <summary>
		/// Static method that creates the select pen
		/// </summary>
		/// <returns>the select pen</returns>
		private static Pen CreateSelectPen()
		{
			Pen sPen = new Pen(Brushes.Blue, 3);
			double[] dashes = {8, 5, 5, 4};
			sPen.DashStyle = new DashStyle(dashes, 5);
			return sPen;
		}
		
		/// <summary>
		/// Call the OnRender method for this CardItem. This method is necessary because OnRender
		/// is not called directly, but rather through Card.OnRender and OnRender is a protected
		/// method inherited from UIElement
		/// </summary>
		/// <param name="dc">DrawingContext to render this CardItem on</param>
		public void Render(DrawingContext dc)
		{
			this.OnRender(dc);
		}
				
	}
}
