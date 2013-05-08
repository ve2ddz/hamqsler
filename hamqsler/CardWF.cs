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
using System.Windows;

namespace hamqsler
{
	/// <summary>
	/// CardWF describes the QslCard.
	/// </summary>
	public class CardWF : CardWFItem
	{
		private static readonly DependencyProperty IsDirtyProperty =
			DependencyProperty.Register("IsDirty", typeof(bool),
			                            typeof(CardWF), new PropertyMetadata(false));
		public bool IsDirty
		{
			get {return (bool)GetValue(IsDirtyProperty);}
			set {SetValue(IsDirtyProperty, value);}
		}

		private static readonly DependencyProperty IsInDesignModeProperty =
			DependencyProperty.Register("IsInDesignMode", typeof(bool),
			                            typeof(CardWF), new PropertyMetadata(false));
		public bool IsInDesignMode
		{
			get {return (bool)GetValue(IsInDesignModeProperty);}
			set {SetValue(IsInDesignModeProperty, value);}
		}
		
		private static readonly DependencyProperty FileNameProperty =
			DependencyProperty.Register("FileName", typeof(string),
			                            typeof(CardWF), new PropertyMetadata(null));
		public string FileName
		{
			get {return GetValue(FileNameProperty) as string;}
			set {SetValue(FileNameProperty, value);}
		}
		
		private BackgroundWFImage backgroundImage = null;
		public BackgroundWFImage BackgroundImage
		{
			get {return backgroundImage;}
			set {backgroundImage = value;}
		}
		
		private static readonly DependencyProperty CardPrintPropertiesProperty =
			DependencyProperty.Register("CardPrintProperties", typeof(PrintProperties),
			                            typeof(CardWF),
			                            new PropertyMetadata(null));
		public PrintProperties CardPrintProperties
		{
			get {return GetValue(CardPrintPropertiesProperty) as PrintProperties;}
			set {SetValue(CardPrintPropertiesProperty, value);}
		}
		
		/// <summary>
		/// DispPropertyChanged event is called whenever a property that affects a
		/// CardWFItem display is changed
		/// </summary>
		public delegate void DisplayPropertyChanged(object sender, EventArgs e);
		public virtual event DisplayPropertyChanged DispPropertyChanged;
		
		/// <summary>
		/// Default constructor
		/// </summary>
		public CardWF() : base()
		{
			QslCard = this;
			App.Logger.Log("In CardWF default constructor:" + Environment.NewLine,
						   App.Logger.DebugPrinting);
			CardPrintProperties = new PrintProperties();
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="width">Width of the card (100 units/inch)</param>
		/// <param name="height">Height of the card (100 units/inch)</param>
		/// <param name="isInDesignMode">design mode indicator:
		/// true if in design mode, false otherwise</param>
		public CardWF(int width, int height, bool isInDesignMode) : base(width, height)
		{
			QslCard = this;
			App.Logger.Log("In CardWF constructor:" + Environment.NewLine,
						   App.Logger.DebugPrinting);
			CardPrintProperties = new PrintProperties();
			IsInDesignMode = isInDesignMode;
			ItemSize = new System.Drawing.Size(width, height);
			BackgroundImage = new BackgroundWFImage();
			BackgroundImage.QslCard = this;
			BackgroundImage.ImageFileName = @"$hamqslerFolder$\Samples\sample.jpg";
		}
		
		/// <summary>
		/// Create a new CardWF object that is a hardcopy of this card
		/// </summary>
		/// <returns>CardWF object that is a hardcopy of this CardWF object</returns>
		public CardWF Clone()
		{
			CardWF card = new CardWF();
			card.ItemSize = new System.Drawing.Size(this.Width, this.Height);
			card.Location = new System.Drawing.Point(this.X, this.Y);
			return card;
		}
		
		/// <summary>
		/// Raise the DispPropertyChanged event
		/// </summary>
		public void RaiseDispPropertyChangedEvent()
		{
			if(DispPropertyChanged != null)
			{
				DispPropertyChanged(this, new EventArgs());
			}
		}
	}
}
