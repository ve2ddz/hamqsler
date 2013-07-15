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
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

namespace hamqsler
{
	/// <summary>
	/// CardWF describes the QslCard.
	/// </summary>
	[Serializable]
	public class CardWF : CardWFItem
	{
		private static readonly DependencyProperty IsDirtyProperty =
			DependencyProperty.Register("IsDirty", typeof(bool),
			                            typeof(CardWF), new PropertyMetadata(false));
		[XmlIgnore]
		public bool IsDirty
		{
			get {return (bool)GetValue(IsDirtyProperty);}
			set {SetValue(IsDirtyProperty, value);}
		}
		
		private static readonly DependencyProperty IsInDesignModeProperty =		
		DependencyProperty.Register("IsInDesignMode", typeof(bool),
			                            typeof(CardWF), new PropertyMetadata(null));
		[XmlIgnore]
		public bool IsInDesignMode
		{
			get {return (bool)GetValue(IsInDesignModeProperty);}
			set {SetValue(IsInDesignModeProperty, value);}
		}
		
		private static readonly DependencyProperty FileNameProperty =
			DependencyProperty.Register("FileName", typeof(string),
			                            typeof(CardWF), new PropertyMetadata(null));
		[XmlIgnore]
		public string FileName
		{
			get {return GetValue(FileNameProperty) as string;}
			set {SetValue(FileNameProperty, value);}
		}
		
		private BackgroundWFImage backgroundImage = new BackgroundWFImage();
		public BackgroundWFImage BackgroundImage
		{
			get {return backgroundImage;}
			set {backgroundImage = value;}
		}
		
		private List<SecondaryWFImage> secondaryImages = new List<SecondaryWFImage>();
		public List<SecondaryWFImage> SecondaryImages
		{
			get {return secondaryImages;}
		}
		
		private List<TextWFItem> textItems = new List<TextWFItem>();
		public List<TextWFItem> TextItems
		{
			get {return textItems;}
		}
		
		private QsosWFBox qsosBox = new QsosWFBox(true);
		public QsosWFBox QsosBox
		{
			get {return qsosBox;}
			set {qsosBox = value;}
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
			QsosBox = null;
			QslCard = this;
			App.Logger.Log("In CardWF default constructor:" + Environment.NewLine,
						   App.Logger.DebugPrinting);
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
			UserPreferences prefs = ((App)App.Current).UserPreferences;
			QslCard = this;
			App.Logger.Log("In CardWF constructor:" + Environment.NewLine,
						   App.Logger.DebugPrinting);
			CardPrintProperties = new PrintProperties();
			IsInDesignMode = isInDesignMode;
			ItemSize = new System.Drawing.Size(width, height);
			// background image
			BackgroundImage.QslCard = this;
			// call text item
			TextWFItem call = new TextWFItem();
			call.QslCard = this;
			foreach(TextPart part in prefs.Callsign)
			{
				call.Text.Add(part);
			}
			call.TextFontFace = prefs.DefaultTextItemsFontFace;
			call.IsBold = true;
			call.FontSize = 48.0F;
			call.X = -70;
			call.Y = 0;
			TextItems.Add(call);
			// name and Qth text item
			TextWFItem nameQth = new TextWFItem();
			nameQth.QslCard = this;
			foreach(TextPart part in prefs.NameQth)
			{
				nameQth.Text.Add(part);
			}
			nameQth.TextFontFace = prefs.DefaultTextItemsFontFace;
			nameQth.IsBold = false;
			nameQth.FontSize = 10.0F;
			nameQth.X = this.Width / 2;
			nameQth.Y = this.Height / 20;
			TextItems.Add(nameQth);
			// salutation text item
			TextWFItem salutation = new TextWFItem();
			salutation.QslCard = this;
			foreach(TextPart part in prefs.Salutation)
			{
				salutation.Text.Add(part);
			}
			salutation.TextFontFace = prefs.DefaultTextItemsFontFace;
			salutation.FontSize = 10.0F;
			salutation.X = this.Width / 75;
			salutation.Y = (int)((float)this.Height * 9F / 10F);
			TextItems.Add(salutation);
			// QsosBox
			QsosBox.QslCard = this;
		}
		
		/// <summary>
		/// Create a new CardWF object that is a hardcopy of this card
		/// </summary>
		/// <returns>CardWF object that is a hardcopy of this CardWF object</returns>
		public CardWF Clone()
		{
			CardWF card = new CardWF();
			card.CopyBaseProperties(this);
			card.QslCard = card;
			card.BackgroundImage = BackgroundImage.Clone();
			card.backgroundImage.QslCard = card;
			foreach(SecondaryWFImage sImage in SecondaryImages)
			{
				SecondaryWFImage si = sImage.Clone();
				si.QslCard = card;
				card.SecondaryImages.Add(si);
				
			}
			foreach(TextWFItem tItem in TextItems)
			{
				TextWFItem ti = tItem.Clone();
				ti.QslCard = card;
				card.TextItems.Add(ti);
			}
			card.QsosBox = null;
			if(this.QsosBox != null)
			{
				QsosWFBox box = QsosBox.Clone();
				box.QslCard = card;
				card.QsosBox = box;
			}
			card.CardPrintProperties = new PrintProperties(this.CardPrintProperties);
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
		
		/// <summary>
		/// Retrieve the card item that is selected on this card
		/// </summary>
		/// <returns>Selected card item, if any, or null if none selected</returns>
		public CardWFItem GetSelectedItem()
		{
			if(QsosBox != null && QsosBox.IsSelected)
			{
				return QsosBox;
			}
			foreach(TextWFItem tItem in TextItems)
			{
				if(tItem.IsSelected)
				{
					return tItem;
				}
			}
			foreach(SecondaryWFImage sImage in SecondaryImages)
			{
				if(sImage.IsSelected)
				{
					return sImage;
				}
			}
			if(BackgroundImage.IsSelected)
			{
				return BackgroundImage;
			}
			else return null;
		}
		
		/// <summary>
		/// Retrieve the highlighted card item on this card
		/// </summary>
		/// <returns>Highlighted card item, if any, or null if none highlighted</returns>
		public CardWFItem GetHighlightedItem()
		{
			if(QsosBox != null && QsosBox.IsHighlighted)
			{
				return QsosBox;
			}
			foreach(TextWFItem tItem in TextItems)
			{
				if(tItem.IsHighlighted)
				{
					return tItem;
				}
			}
			foreach(SecondaryWFImage sImage in SecondaryImages)
			{
				if(sImage.IsHighlighted)
				{
					return sImage;
				}
			}
			if(BackgroundImage.IsHighlighted)
			{
				return BackgroundImage;
			}
			else return null;
		}
		
		/// <summary>
		/// Get the relative position of the input coordinates relative to this card
		/// </summary>
		/// <param name="x">Card relative X coordinate</param>
		/// <param name="y">Card relative Y coordinate</param>
		/// <returns>RelativeLocations value</returns>
		public override CardWFItem.RelativeLocations GetRelativeLocation(int x, int y)
		{
			RelativeLocations location = RelativeLocations.Outside;
			if(this.Contains(x, y))
			{
				location = RelativeLocations.Inside;
			}
			return location;
		}
		
		/// <summary>
		/// Delete the selected card item
		/// </summary>
		public void DeleteSelectedItem()
		{
			CardWFItem ci = GetSelectedItem();
			if(ci != null)
			{
				if(ci.GetType() == typeof(SecondaryWFImage))
				{
					SecondaryImages.Remove((SecondaryWFImage)ci);
				}
				else if(ci.GetType() == typeof(TextWFItem))
				{
					TextItems.Remove((TextWFItem)ci);
				}
				else if(ci.GetType() == typeof(QsosWFBox))
				{
					QsosBox = null;
				}
				IsDirty = true;
				RaiseDispPropertyChangedEvent();
			}
		}
		
		/// <summary>
		/// Handler for CardPrintProperties.PrintPropertiesChanged event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void OnPrintPropertiesChanged(object sender, EventArgs e)
		{
			this.IsDirty = true;
			RaiseDispPropertyChangedEvent();
		}

		/// <summary>
		/// Save this card as an XML file
		/// </summary>
		/// <param name="fileName">Path to the file to save the XML text in</param>
		public void SaveAsXml(string fileName)
		{
			XmlSerializer xmlFormat = new XmlSerializer(typeof(CardWF),
			                                            new Type[]{typeof(BackgroundWFImage),
			                                            	typeof(SecondaryWFImage),
			                                            	typeof(ImageWFBase),
			                                            	typeof(TextWFItem),
			                                            	typeof(QsosWFBox),
			                                            	typeof(TextParts),
			                                            	typeof(StaticText),
			                                            	typeof(AdifMacro),
			                                            	typeof(AdifExistsMacro),
			                                            	typeof(CountMacro),
			                                            	typeof(ManagerMacro),
			                                            	typeof(ManagerExistsMacro)});
			using (Stream fStream = new FileStream(fileName, FileMode.Create,
			                                       FileAccess.Write, FileShare.Read))
			{
				xmlFormat.Serialize(fStream, this);
				this.FileName = fileName;
				this.IsDirty = false;
			}
		}
		
		/// <summary>
		/// Deserialize a Qsl Card saved as XML
		/// </summary>
		/// <param name="fileName">Name of XML file containing card description</param>
		/// <returns>Card object described by the XML file</returns>
		public static CardWF DeserializeCard(string fileName)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(CardWF));
			FileStream fs = new FileStream(fileName, FileMode.Open);
			XmlReader reader = XmlReader.Create(fs);
			
			CardWF card = (CardWF)serializer.Deserialize(reader);
			fs.Close();
			// set QslCard for each CardItem in card to this card
			card.BackgroundImage.QslCard = card;
			foreach(SecondaryWFImage si in card.SecondaryImages)
			{
				si.QslCard = card;
			}
			foreach(TextWFItem ti in card.TextItems)
			{
				ti.QslCard = card;
				System.Drawing.Text.InstalledFontCollection fontCol =
					new System.Drawing.Text.InstalledFontCollection();
				// Must ensure that the font is installed. Otherwise card not displayed.
				string fontName = fontCol.Families[0].Name;
				foreach(System.Drawing.FontFamily family in fontCol.Families)
				{
					if(family.Name == ti.TextFontFace)
					{
						fontName = ti.TextFontFace;
						break;
					}
				}
				ti.TextFontFace = fontName;
				foreach(TextPart part in ti.Text)
				{
					part.RemoveExtraneousStaticTextMacros();
				}
			}
			if(card.QsosBox != null)
			{
				card.QsosBox.QslCard = card;
				foreach(TextPart part in card.QsosBox.ConfirmingText)
				{
					part.RemoveExtraneousStaticTextMacros();
				}
			}
			return card;
		}
		
		/// <summary>
		/// Handler for property changed event
		/// </summary>
		/// <param name="e">DependencyPropertyChangedEventArgs object describing
		/// the change</param>
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if(e.Property == IsInDesignModeProperty)
			{
				if((bool)e.NewValue == true && CardPrintProperties != null)
				{
					if(IsInDesignMode)
					{
						CardPrintProperties.PrintPropertiesChanged += OnPrintPropertiesChanged;
					}
					else
					{
						CardPrintProperties.PrintPropertiesChanged -= OnPrintPropertiesChanged;
					}
				}
			}
			else if(e.Property == IsDirtyProperty)
			{
//				RaiseDispPropertyChangedEvent();
			}
		}
		
		/// <summary>
		/// Gets a HashSet of strings containing the adifField names that Qsos should be sorted on
		/// </summary>
		/// <param name='fields'>
		/// HashSet of strings containing adifField names retrieved from AdifMacros with 
		/// SeparateCardsByField set.
		/// </param>
		/// <param name='existFields'>
		/// HashSet of strings containing adifField names retrieved from AdifExistsMacros with
		/// SeparateCardsByField set.
		/// </param>
		public void GetAdifFieldsForSorting(ref HashSet<string>fields, ref HashSet<string>existFields)
		{
			foreach(TextWFItem ti in TextItems)
			{
				foreach(TextPart part in ti.Text)
				{
					part.GetAdifFieldsForSorting(ref fields, ref existFields);
				}
			}
			if(QsosBox != null)
			{
				foreach(TextPart part in QsosBox.ConfirmingText)
				{
					part.GetAdifFieldsForSorting(ref fields, ref existFields);
				}
			}
		}
	}
}
