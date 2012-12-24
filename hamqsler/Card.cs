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
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Globalization;
using System.Windows.Media;
using System.Xml;
using System.Xml.Serialization;

namespace hamqsler
{
	/// <summary>
	/// QSL card
	/// </summary>
	[Serializable]
	public class Card : CardItem
	{
		private static readonly DependencyProperty PrintCardOutlinesProperty =
			DependencyProperty.Register("PrintCardOutlines", typeof(bool), typeof(Card),
			                            new PropertyMetadata(true));
		public bool PrintCardOutlines
		{
			get {return (bool)GetValue(PrintCardOutlinesProperty);}
			set {SetValue(PrintCardOutlinesProperty, value);}
		}
		
		private static readonly DependencyProperty FillLastPageWithBlankCardsProperty =
			DependencyProperty.Register("FillLastPageWithBlankCards", typeof(bool), typeof(Card),
			                            new PropertyMetadata(true));
		public bool FillLastPageWithBlankCards
		{
			get {return (bool)GetValue(FillLastPageWithBlankCardsProperty);}
			set {SetValue(FillLastPageWithBlankCardsProperty, value);}
		}
		
		private static readonly DependencyProperty SetCardMarginsToPrinterPageMarginsProperty =
			DependencyProperty.Register("SetCardMarginsToPrinterPageMargins", typeof(bool),
			                            typeof(Card), new PropertyMetadata(true));
		public bool SetCardMarginsToPrinterPageMargins
		{
			get {return (bool)GetValue(SetCardMarginsToPrinterPageMarginsProperty);}
			set {SetValue(SetCardMarginsToPrinterPageMarginsProperty, value);}
		}
		
		private static readonly DependencyProperty BackgroundImageProperty = 
			DependencyProperty.Register("BackImage", typeof(BackgroundImage), typeof(Card),
			                            new PropertyMetadata(null));
		public BackgroundImage BackImage
		{
			get {return (BackgroundImage)GetValue(BackgroundImageProperty);}
			set {SetValue(BackgroundImageProperty, value);}
		}
		
		private List<SecondaryImage> secondaryImages = new List<SecondaryImage>();
		public List<SecondaryImage> SecondaryImages
		{
			get {return secondaryImages;}
		}

		private List<TextItem> textItems = new List<TextItem>();
		public List<TextItem> TextItems
		{
			get {return textItems;}
		}
		
		private QsosBox qsosBox = null;
		public QsosBox QsosBox
		{
			get {return qsosBox;}
			set {qsosBox = value;}
		}
			
		private UserPreferences userPreferences;
		
		private static readonly DependencyProperty IsDirtyProperty =
			DependencyProperty.Register("IsDirty", typeof(bool), typeof(Card),
			                            new PropertyMetadata(false));
		[XmlIgnore]
		public bool IsDirty
		{
			get {return (bool)GetValue(IsDirtyProperty);}
			set {SetValue(IsDirtyProperty, value);}
		}
		
		[XmlIgnore]
		private string fileName = null;
		public string FileName
		{
			get {return fileName;}
			set {fileName = value;}
		}
		
		/// <summary>
		/// default constructor (called when loading Card from XML)
		/// </summary>
		public Card()
		{
			userPreferences = ((App)Application.Current).UserPreferences;
			DisplayX = 0;
			DisplayY = 0;
			DisplayWidth = 0;
			DisplayHeight = 0;
			QslCard = this;
		}
		
		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="width">Width of the card in device independent units</param>
		/// <param name="height">Height of the card in device independent units</param>
        /// <param name="isInDesignMode">Boolean to indicate if this image is to be displayed
        /// in design mode</param>
		public Card(double width, double height, bool isInDesignMode)
			: base(new Rect(0, 0, width, height), isInDesignMode)
		{
			userPreferences = ((App)Application.Current).UserPreferences;
			// card properties
			PrintCardOutlines = userPreferences.PrintCardOutlines;
			FillLastPageWithBlankCards = userPreferences.PrintCardOutlines;
			SetCardMarginsToPrinterPageMargins = userPreferences.SetCardMarginsToPrinterPageMargins;
			// background image
			BackImage = new BackgroundImage(isInDesignMode);
			BackImage.QslCard = this;
			// call text item
			TextItem call = new TextItem(isInDesignMode);
			call.QslCard = this;
			foreach(TextPart part in userPreferences.Callsign)
			{
				call.Text.Add(part);
			}
			call.SetDisplayText();
			call.TextFontFace = userPreferences.DefaultTextItemsFontFace;
			call.TextFontWeight = FontWeights.Black;
			call.FontSize = 72.0;
			call.DisplayX = -70;
			call.DisplayY = 0;
			call.DisplayWidth = 0;
			call.DisplayHeight = 0;
			call.SetDisplayText();
			TextItems.Add(call);
			// name Qth text item
			TextItem nameQth = new TextItem(isInDesignMode);
			nameQth.QslCard = this;
			foreach(TextPart part in userPreferences.NameQth)
			{
				nameQth.Text.Add(part);
			}
			nameQth.SetDisplayText();
			nameQth.TextFontFace = userPreferences.DefaultTextItemsFontFace;
			nameQth.TextFontWeight = FontWeights.Normal;
			nameQth.FontSize = 10.0;
			nameQth.DisplayX = DisplayWidth / 2;
			nameQth.DisplayY = 15;
			nameQth.DisplayWidth = 0;
			nameQth.DisplayHeight = 0;
			nameQth.SetDisplayText();
			TextItems.Add(nameQth);
			// salutation
			TextItem salutation = new TextItem(isInDesignMode);
			salutation.QslCard = this;
			foreach(TextPart part in userPreferences.Salutation)
			{
				salutation.Text.Add(part);
			}
			salutation.SetDisplayText();
			salutation.TextFontFace = userPreferences.DefaultTextItemsFontFace;
			salutation.FontSize = 10.0;
			salutation.DisplayX = 5;
			salutation.DisplayY = DisplayHeight - 25;
			salutation.DisplayWidth = 0;
			salutation.DisplayHeight = 0;
			salutation.SetDisplayText();
			TextItems.Add(salutation);
			
			// QsosBox
			qsosBox = new QsosBox(isInDesignMode);
			qsosBox.QslCard = this;
			// more card properties
			QslCard = this;
			IsDirty = false;
		}
		
		/// <summary>
		/// Gets the CardItem that is highlighted
		/// </summary>
		/// <returns>Highlighted card item or null if no card item is highlighted</returns>
		public CardItem GetHighlighted()
		{
			if(qsosBox != null && qsosBox.IsHighlighted)
			{
				return qsosBox;
			}
			foreach(TextItem ti in TextItems)
			{
				if(ti.IsHighlighted)
				{
					return ti;
				}
			}
			foreach(SecondaryImage si in SecondaryImages)
			{
				if(si.IsHighlighted)
				{
					return si;
				}
			}
			if(BackImage.IsHighlighted)
			{
				return BackImage;
			}
			return null;
		}
		
		/// <summary>
		/// Clears the IsHighlighted property of every CardItem child of this Card
		/// </summary>
		public void ClearHighlighted()
		{
			if(qsosBox != null)
			{
				qsosBox.IsHighlighted = false;
			}
			foreach(TextItem ti in TextItems)
			{
				ti.IsHighlighted = false;
			}
			foreach(SecondaryImage si in SecondaryImages)
			{
				si.IsHighlighted = false;
			}
			BackImage.IsHighlighted = false;
		}
		
		/// <summary>
		/// Get the CardItem that is selected
		/// </summary>
		/// <returns>Selected card item, or null if none selected</returns>
		public CardItem GetSelected()
		{
			if(qsosBox != null && qsosBox.IsSelected)
			{
				return qsosBox;
			}
			foreach(TextItem ti in TextItems)
			{
				if(ti.IsSelected)
					return ti;
			}
			foreach(SecondaryImage si in SecondaryImages)
			{
				if(si.IsSelected)
					return si;
			}
			if(BackImage.IsSelected)
			{
				return BackImage;
			}
			return null;
		}
		
		/// <summary>
		/// Adds a SecondaryImage to the card. No image is actually loaded; this must be done
		/// from the SecondaryImageProperties panel.
		/// </summary>
		public void AddImage()
		{
			SecondaryImage si = new SecondaryImage();
			SecondaryImages.Add(si);
			si.QslCard = this;
			CardItem ci = GetHighlighted();
			if(ci != null)
			{
				ci.IsHighlighted = false;
			}
			si.IsSelected = true;
			IsDirty = true;
		}
				
		/// <summary>
		/// Adds a TextItem to the card. 
		/// </summary>
		public void AddText()
		{
			TextItem ti = new TextItem();
			ti.QslCard = this;
			TextItems.Add(ti);
			StaticText sText = new StaticText();
			sText.Text = "Text Item";
			ti.Text.Add(sText);
			ti.SetDisplayText();
			CardItem ci = GetHighlighted();
			if(ci != null)
			{
				ci.IsHighlighted = false;
			}
			ti.IsSelected = true;
			IsDirty = true;
		}
		
		/// <summary>
		/// Adds a QsosBox to the card
		/// </summary>
		public void AddQsosBox()
		{
			if(QsosBox == null)
			{
				QsosBox qBox = new QsosBox(IsInDesignMode);
				qBox.QslCard = this;
				this.QsosBox = qBox;
				qBox.IsSelected = true;
				IsDirty = true;
			}
		}
		
		/// <summary>
		/// Deletes selected CardItem
		/// </summary>
		public void DeleteItem()
		{
			CardItem ci = QslCard.GetSelected();
			if(ci != null)
			{
				if(ci.GetType() == typeof(SecondaryImage))
				{
					SecondaryImages.Remove((SecondaryImage)ci);
				}
				else if(ci.GetType() == typeof(TextItem))
				{
					TextItems.Remove((TextItem)ci);
				}
				else if(ci.GetType() == typeof(QsosBox))
				{
					QsosBox = null;
				}
				IsDirty = true;
			}
		}
		
		/// <summary>
		/// Save this card as an XML file
		/// </summary>
		/// <param name="fileName">Path to the file to save the XML text in</param>
		public void SaveAsXml(string fileName)
		{
			XmlSerializer xmlFormat = new XmlSerializer(typeof(Card),
			                                            new Type[]{typeof(BackgroundImage),
			                                            	typeof(SecondaryImage),
			                                            	typeof(CardImageBase),
			                                            	typeof(TextItem),
			                                            	typeof(QsosBox),
			                                            	typeof(TextParts),
			                                            	typeof(StaticText),
			                                            	typeof(AdifMacro),
			                                            	typeof(AdifExistsMacro),
			                                            	typeof(CountMacro),
			                                            	typeof(ManagerMacro),
			                                            	typeof(ManagerExistsMacro),
			                                            	typeof(SolidColorBrush),
			                                            	typeof(MatrixTransform)});
			using (Stream fStream = new FileStream(fileName, FileMode.Create,
			                                       FileAccess.Write, FileShare.Read))
			{
				xmlFormat.Serialize(fStream, this);
				this.FileName = fileName;
				this.IsDirty = false;
			}
		}
		
		/// <summary>
		/// Handler for PropertyChanged event
		/// </summary>
		/// <param name="e">DependencyPropertyChangedEventArgs object</param>
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if(e.Property == PrintCardOutlinesProperty ||
			   e.Property == FillLastPageWithBlankCardsProperty ||
			   e.Property == SetCardMarginsToPrinterPageMarginsProperty)
			{
				IsDirty = true;
			}
			else if(e.Property == IsDirtyProperty)
			{
				((MainWindow)App.Current.MainWindow).SetTitle(FileName, IsDirty);
			}
		}
		
		/// <summary>
		/// Deserialize a Qsl Card saved as XML
		/// </summary>
		/// <param name="fileName">Name of XML file containing card description</param>
		/// <returns>Card object described by the XML file</returns>
		public static Card DeserializeCard(string fileName)
		{
			XmlSerializer serializer = new XmlSerializer(typeof(Card),
			                                             new Type[]{typeof(SolidColorBrush),
			                                             	typeof(MatrixTransform)});
			FileStream fs = new FileStream(fileName, FileMode.Open);
			XmlReader reader = XmlReader.Create(fs);
			
			Card card = (Card)serializer.Deserialize(reader);
			fs.Close();
			// set QslCard for each CardItem in card to this card
			card.BackImage.QslCard = card;
			foreach(SecondaryImage si in card.SecondaryImages)
			{
				si.QslCard = card;
			}
			foreach(TextItem ti in card.TextItems)
			{
				ti.QslCard = card;
			}
			if(card.QsosBox != null)
			{
				card.QsosBox.QslCard = card;
			}
			return card;
		}
		
		/// <summary>
		/// Deserialize a QslDnP card save as XML
		/// </summary>
		/// <param name="fileName">Name of the QslDnP card file</param>
		/// <returns>Card object described by the XML file</returns>
		public static Card DeserializeQslDnPCard(string fileName)
		{
            XmlDocument doc = new XmlDocument();
            Stream fStream = new FileStream(fileName, FileMode.Open);
            doc.Load(fStream);
            XmlNode node = doc.DocumentElement;
            Card card = new Card();
            switch(node.Name)
            {
            	case "LandscapeCard55x425":
            	case "LandscapeCard6x4":
            	case "LandscapeCard15x10":
            	case "LandscapeCard55x35":
            		CultureInfo cardCulture = CultureInfo.InvariantCulture;
            		XmlAttributeCollection attrCollection = node.Attributes;
            		XmlAttribute culture = attrCollection["Culture"];
            		if(culture != null)
            		{
            			cardCulture = new CultureInfo(culture.Value, false);
            		}
            		XmlNode cNode = XmlProcs.GetFirstChildElement(node);
            		// make sure the node is a Card node, then process it
            		if(cNode != null && cNode.Name.Equals("Card"))
            		{
            			card.LoadCard(cNode, cardCulture);
            		}
            		else
            		{
            			throw new XmlException("Invalid Card file. Node is not a Card node");
            		}
            		break;
            	default:
            		throw new XmlException("Invalid QslDnP card file");
            }
            return card;
		}
		
		/// <summary>
		/// Load values for this Card from QslDnP card file contents
		/// </summary>
		/// <param name="itemNode">The Card node</param>
		/// <param name="culture">CultureInfo that the card was created in</param>
		private void LoadCard(XmlNode cardNode, CultureInfo culture)
		{
			XmlNode node = XmlProcs.GetFirstChildElement(cardNode);
			while(node != null)
			{
				switch(node.Name)
				{
					case "CardItem":
						base.Load(node, culture);
						QslCard = this;
						break;
					case "BackgroundImage":
						BackImage = new BackgroundImage(true);
						BackImage.Load(node, culture);
						BackImage.QslCard = this;
						break;
					case "SecondaryImage":
						SecondaryImage sImage = new SecondaryImage();
						sImage.Load(node, culture);
						SecondaryImages.Add(sImage);
						sImage.QslCard = this;
						break;
					case "TextItem":
						TextItem ti = new TextItem();
						ti.Load(node, culture);
						TextItems.Add(ti);
						ti.QslCard = this;
						ti.SetDisplayText();
						break;
					case "QsosBox":
						QsosBox = new QsosBox(true);
						QsosBox.Load(node, culture);
						QsosBox.QslCard = this;
						break;
				}
				node = XmlProcs.GetNextSiblingElement(node);
			}
		}
				
	}
}
