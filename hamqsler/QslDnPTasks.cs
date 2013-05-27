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
using System.Globalization;
using System.IO;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

namespace hamqsler
{
	/// <summary>
	/// Static class that encapsulates QslDesignAndPrint related tasks
	/// </summary>
	public static class QslDnPTasks
	{
		/// <summary>
		/// Move image and other files from QslDnP folder to hamqsler folder
		/// </summary>
		/// 
		private static bool copyError = false;
		public static bool CopyError
		{
			get {return copyError;}
		}
		
		// scale factor used to adjust card item dimensions from WPF to Windows Forms
		private const float WPFToWFScale = 100F/96F;
		// scale factor used to adjust font sizes from WPF to Windows Forms
		private const float FontScaleFactor = 0.75F;
		
		/// Move image and other files from QslDnp to hamqsler
		/// </summary>
		/// <param name="qslDir">Name of the directory to copy</param>
		public static void MoveFiles(string qslDir)
		{
			try
			{
				DirectoryInfo qslInfo = new DirectoryInfo(qslDir);
				string qsldnpDir = Environment.GetFolderPath(
					Environment.SpecialFolder.MyDocuments) + "\\QslDnP";
				string hamqsler = ((App)App.Current).HamqslerFolder;
				hamqsler = hamqsler.Substring(0, hamqsler.Length - 1);
				DirectoryInfo hamInfo = new DirectoryInfo(hamqsler);
				foreach(string file in Directory.EnumerateFiles(qslDir))
				{
					// do not copy xqsl or xml files
					if(!file.EndsWith(".xqsl") && !file.EndsWith(".xml"))
					{
						FileInfo fInfo = new FileInfo(file);
						try
						{
							string toFile = hamqsler + fInfo.FullName.Substring( 
								qsldnpDir.Length);
							// if directory does not exist in hamqsler, then create it
							FileInfo hFile = new FileInfo(toFile);
							DirectoryInfo dInfo = new DirectoryInfo(hFile.DirectoryName);
							if(!dInfo.Exists)
							{
								Directory.CreateDirectory(hFile.DirectoryName);
							}
							// copy file to hamqsler folder/subfolder
							File.Copy(file, toFile);
						}
						catch(IOException ioe)
						{
							// error attempting to copy, so log it, and
							// show that a copy error occurred.
							App.Logger.Log(ioe, false, false);
							copyError = true;
						}
					}
				}
				// now process every subdirectory in this folder
				foreach(string dir in Directory.EnumerateDirectories(qslDir))
				{
					// do not copy files from Logs and Samples subdirectories
					if(!dir.EndsWith("Logs") && !dir.EndsWith("Samples"))
					{
						MoveFiles(dir);
					}
				}
			}
			catch(ArgumentException ae)
			{
				App.Logger.Log(ae);
			}
			catch(PathTooLongException pe)
			{
				App.Logger.Log(pe);
			}
		}
		
		/// <summary>
		/// Convert QslDnP card files to hamqsler card files
		/// </summary>
		public static void ConvertCardFiles(string qslDir)
		{
			DirectoryInfo qslInfo = new DirectoryInfo(qslDir);
			string qsldnpDir = Environment.GetFolderPath(
				Environment.SpecialFolder.MyDocuments) + "\\QslDnP";
			string hamqsler = ((App)App.Current).HamqslerFolder;
			hamqsler = hamqsler.Substring(0, hamqsler.Length - 1);
			DirectoryInfo hamInfo = new DirectoryInfo(hamqsler);
			foreach(string file in Directory.EnumerateFiles(qslDir))
			{
				if(file.EndsWith(".xqsl"))
				{
					try
					{
						CardWF card = QslDnPTasks.ConvertFromXQSL(file);
						FileInfo fInfo = new FileInfo(file);
						string toFile = hamqsler + fInfo.FullName.Substring( 
							qsldnpDir.Length);
						// if directory does not exist in hamqsler, then create it
						toFile = toFile.Substring(0, toFile.IndexOf(".xqsl")) +
							".xq1";
						FileInfo hFile = new FileInfo(toFile);
						DirectoryInfo dInfo = new DirectoryInfo(hFile.DirectoryName);
						if(!dInfo.Exists)
						{
							Directory.CreateDirectory(hFile.DirectoryName);
						}
						card.SaveAsXml(hFile.FullName);
					}
					catch(XmlException xe)
					{
						App.Logger.Log(xe, true, false);
						MessageBox.Show("There was a problem with one of the XQSL files" +
						                Environment.NewLine +
						                "so it could not be converted. See the log file" +
						                Environment.NewLine +
						                "for more information.", "Card Conversion Error",
						                MessageBoxButton.OK, MessageBoxImage.Warning);
					}
				}
			}
		}
		
		/// <summary>
		/// Create a CardWF object based on an XQSL file
		/// </summary>
		/// <param name="file">name of the XQSL file</param>
		/// <returns>CardWF object created</returns>
		private static CardWF ConvertFromXQSL(string file)
		{
			CardWF card = null;
			XmlDocument doc = new XmlDocument();
			Stream fStream = new FileStream(file, FileMode.Open);
			doc.Load(fStream);
			XmlNode node = doc.DocumentElement;
			switch(node.Name)
			{
				case "LandscapeCard55x425":
					card = new CardWF(550, 425, false);
					break;
				case "LandscapeCard6x4":	// note fallthrough
				case "LandscapeCard15x10":
					card = new CardWF(600, 400, false);
					break;
				case "LandscapeCard55x35":
					card = new CardWF(550, 350, false);
					break;
				default:
					throw new XmlException("Invalid card file");
			}
			CultureInfo cardCulture = CultureInfo.InvariantCulture;
			XmlAttributeCollection attrCollection = node.Attributes;
			XmlAttribute culture = attrCollection["Culture"];
			if(culture != null)
			{
				cardCulture = new CultureInfo(culture.Value, false);
			}
			XmlNode cNode = XmlProcs.GetFirstChildElement(node);
			if(cNode != null && cNode.Name.Equals("Card"))
			{
				LoadXQSLCard(card, cNode, cardCulture);
			}
			else
			{
				throw new XmlException("Invalid card file - Node is not a Card node");
			}
			return card;
		}
		
		/// <summary>
		/// Convert XQSL card data to CardWF and CardWFItem data
		/// </summary>
		/// <param name="card">CardWF object to convert to</param>
		/// <param name="cardNode">XmlNode object that contains the card info</param>
		/// <param name="culture">CultureInfo that the card was saved with</param>
		private static void LoadXQSLCard(CardWF card, XmlNode cardNode, CultureInfo culture)
		{
			card.TextItems.Clear();
			card.QsosBox = null;
			XmlNode node = XmlProcs.GetFirstChildElement(cardNode);
			while(node != null)
			{
				switch(node.Name)
				{
					case "BackgroundImage":
						CreateBackgroundImage(card, node, culture);
						break;
					case "SecondaryImage":
						SecondaryWFImage sImage = CreateSecondaryImage(card, node, culture);
						card.SecondaryImages.Add(sImage);
						break;
					case "TextItem":
						TextWFItem tItem = CreateTextItem(card, node, culture);
						card.TextItems.Add(tItem);
						break;
					case "QsosBox":
						card.QsosBox = CreateQsosBox(card, node, culture);
						break;
				}
				node = XmlProcs.GetNextSiblingElement(node);
			}
		}
		
		/// <summary>
		/// Retrieve CardItem data
		/// </summary>
		/// <param name="cItem">CardWFItem object that the data belongs to</param>
		/// <param name="ciNode">XmlNode object containing the CardItem data</param>
		/// <param name="culture">CultureInfo object that the XQSL file was stored with</param>
		private static void GetCardItemData(CardWFItem cItem, XmlNode ciNode, CultureInfo culture)
		{
			XmlNode node = XmlProcs.GetFirstChildElement(ciNode);
			if(node.Name != "DisplayRectangle")
			{
				throw new XmlException("CardItem first child not DisplayRectangle");
			}
			node = XmlProcs.GetFirstChildElement(node);
			while(node != null)
			{
				XmlText text = XmlProcs.GetTextNode(node);
				if(text != null)
				{
					float value = 0F;
					if(!float.TryParse(text.Value, NumberStyles.Float, culture, out value))
					{
						if(!float.TryParse(text.Value, NumberStyles.Float,
						                   CultureInfo.InvariantCulture, out value))
						{
							throw new XmlException("Bad DisplayRectangle property value in card file");
						}
					}
					value *= WPFToWFScale;
					switch(node.Name)
					{
						case "X":
							cItem.X = (int)value;
							break;
						case "Y":
							cItem.Y = (int)value;
							break;
						case "Width":
							cItem.Width = (int)value;
							break;
						case "Height":
							cItem.Height = (int)value;
							break;
					}
				}
				else
				{
					throw new XmlException("DisplayRectangle property does not have a value");
				}
				node = XmlProcs.GetNextSiblingElement(node);
			}
			
		}
		
		/// <summary>
		/// Save BackgroundImage properties based on the XQSL file
		/// </summary>
		/// <param name="card">CardWF object that contains the BackgroundImage</param>
		/// <param name="node">XmlNode object that describes the BackgroundImage</param>
		/// <param name="culture">CultureInfo object that describes the culture
		/// that the XQSL file was saved with</param>
		private static void CreateBackgroundImage(CardWF card, XmlNode node,
		                                          CultureInfo culture)
		{
			XmlNode biNode = XmlProcs.GetFirstChildElement(node);
			switch(biNode.Name)
			{
				case "CardImageBase":
					CreateCardImageBase(card.BackgroundImage, biNode, culture);
					break;
			}
			
		}
		
		/// <summary>
		/// Saves CardImageBase properties for a BackgroundImage or a SecondaryImage
		/// </summary>
		/// <param name="cItem">BackgroundImage or SecondaryImage that the CardImageBase
		/// corresponds to</param>
		/// <param name="node">XmlNode that describes the CardImageBase</param>
		/// <param name="culture">CultureInfo object that describes the culture
		/// that the XQSL file was saved with</param>
		private static void CreateCardImageBase(CardWFItem cItem, XmlNode node,
		                                        CultureInfo culture)
		{
			XmlNode cibNode = XmlProcs.GetFirstChildElement(node);
			while(cibNode != null)
			{
				switch(cibNode.Name)
				{
					case "ImageFileName":
						XmlText text = XmlProcs.GetTextNode(cibNode);
						if(text != null)
						{
							string myDocs = Environment.GetFolderPath(
								Environment.SpecialFolder.MyDocuments);
							string qslDir = myDocs + "\\QslDnP";
							string fileName = text.Value;
							if(fileName.StartsWith("$MyDocs$"))
							{
								fileName = myDocs +
									fileName.Substring("$MyDocs$".Length);
							}
							if(fileName.StartsWith(qslDir))
							{
								fileName = fileName.Substring(
									0, myDocs.Length) + "\\hamqsler"
									+ fileName.Substring(qslDir.Length);
							}
							((ImageWFBase)cItem).ImageFileName = fileName;
						}
							break;
						case "CardItem":
							GetCardItemData(cItem, cibNode, culture);
							break;
				}
				cibNode = XmlProcs.GetNextSiblingElement(cibNode);
			}
		}
		
		/// <summary>
		/// Create a SecondaryWFImage object based on an XmlNode for it from
		/// an XQSL file
		/// </summary>
		/// <param name="card">The CardWF object that will contain the image</param>
		/// <param name="node">XmlNode containing the SecondaryImage data</param>
		/// <param name="culture">CultureInfo object that describes the culture
		/// that the XQSL file was saved with</param>
		/// <returns>The created SecondaryWFImage object</returns>
		private static SecondaryWFImage CreateSecondaryImage(CardWF card, XmlNode node,
		                                                     CultureInfo culture)
		{
			SecondaryWFImage sImage = new SecondaryWFImage();
			sImage.QslCard = card;
			XmlNode siNode = XmlProcs.GetFirstChildElement(node);
			switch(siNode.Name)
			{
				case "CardImageBase":
					CreateCardImageBase(sImage, siNode, culture);
					break;
			}
			return sImage;
		}
		
		/// <summary>
		/// Create a TextWFItem object based on an XmlNode for it from an
		/// XQSL file
		/// </summary>
		/// <param name="card">The CardWF object that will contain the text item</param>
		/// <param name="node">XmlNode object containing the text item data</param>
		/// <param name="culture">CultureInfo object that describes the culture
		/// that the XQSL file was saved with</param>
		/// <returns></returns>
		private static TextWFItem CreateTextItem(CardWF card, XmlNode node,
		                                         CultureInfo culture)
		{
			TextWFItem tItem = new TextWFItem();
			tItem.QslCard = card;
			XmlNode tiNode = XmlProcs.GetFirstChildElement(node);
			while(tiNode != null)
			{
				XmlText text = XmlProcs.GetTextNode(tiNode);
				switch(tiNode.Name)
				{
					case "CardItem":
						GetCardItemData(tItem, tiNode, culture);
						break;
					case "FaceName":
						tItem.TextFontFace = text.Value;
						break;
					case "TextFontWeight":
						tItem.IsBold = false;
						switch(text.Value)
						{
							case "Normal":
								break;
							case "Bold":	// note fall through
							case "Black":
								tItem.IsBold = true;
								break;
						}
						break;
					case "IsItalic":
						tItem.IsItalic = text.Value=="true" ? true : false;
						break;
					case "FontSize":
						if(text != null)
						{
							float value = 0F;
							if(!float.TryParse(text.Value, NumberStyles.Float, 
							                   culture, out value))
							{
								if(!float.TryParse(text.Value, NumberStyles.Float,
								                   CultureInfo.InvariantCulture,
								                   out value))
								{
									XmlException ex = new XmlException(
										"Bad TextItem property value in Card File" +
										Environment.NewLine +
										"Did you modify the card file by hand?");
									ex.Data.Add("Property", "FontSize");
									ex.Data.Add("Value", text.Value);
									App.Logger.Log(ex);
								}
							}
							tItem.FontSize = value * FontScaleFactor;
						}
						break;
					case "TextBrush":
						tItem.TextColor = XmlProcs.ConvertXmlToColor(tiNode, culture);
						break;
					case "Text":
						StaticText sText = new StaticText();
						if(text.Value != null)
						{
							sText.Text = text.Value;
							tItem.Text.Clear();
							tItem.Text.Add(sText);
						}
						break;
					case "CheckboxBefore":
						if(text != null)
						{
							tItem.CheckboxBefore = text.Value == "true" ? true : false;
						}
						break;
					case "CheckboxAfter":
						if(text != null)
						{
							tItem.CheckboxAfter = text.Value == "true" ? true : false;
						}
						break;
				}
				tiNode = XmlProcs.GetNextSiblingElement(tiNode);
			}
			tItem.CalculateRectangle();
			return tItem;
		}
		
		/// <summary>
		/// Create a QsosWFBox object based on an XmlNode for it from an
		/// XQSL file
		/// </summary>
		/// <param name="card">The CardWF object that will contain the text item</param>
		/// <param name="node">XmlNode object that contains the data for the text item</param>
		/// <param name="culture">CultureInfo object that describes the culture
		/// that the XQSL file was saved with</param>
		/// <returns></returns>
		private static QsosWFBox CreateQsosBox(CardWF card, XmlNode node,
		                                       CultureInfo culture)
		{
			QsosWFBox qBox = new QsosWFBox();
			qBox.QslCard = card;
			qBox.FontSize = 12.0F * FontScaleFactor;
			XmlNode qNode = XmlProcs.GetFirstChildElement(node);
			while(qNode != null)
			{
				XmlText text = XmlProcs.GetTextNode(qNode);
				switch(qNode.Name)
				{
					case "QsoBoxBase":
						qNode = XmlProcs.GetFirstChildElement(qNode);
						break;
					case "CardItem":
						GetCardItemData(qBox, qNode, culture);
						break;
					case "ShowManager":
						if(text != null)
						{
							qBox.ShowManager = text.Value=="true" ? true : false;
						}
						break;
					case "ShowFrequency":
						if(text != null)
						{
							qBox.ShowFrequency = text.Value=="true" ? true : false;
						}
						break;
					case "ShowPseTnx":
						if(text != null)
						{
							qBox.ShowPseTnx = text.Value=="true" ? true : false;
						}
						break;
					case "MaximumQsos":
						if(text != null)
						{
							qBox.MaximumQsos = Int32.Parse(text.Value, culture);
						}
						break;
					case "DateFormat":
						if(text != null)
						{
							qBox.DateFormat = text.Value;
						}
						break;
					case "LineTextBrush":
						qBox.LineTextColor = XmlProcs.ConvertXmlToColor(qNode, culture);
						break;
					case "CallsignBrush":
						qBox.CallsignColor = XmlProcs.ConvertXmlToColor(qNode, culture);
						break;
					case "ManagerBrush":
						qBox.ManagerColor = XmlProcs.ConvertXmlToColor(qNode, culture);
						break;
					case "FaceName":
						if(text != null)
						{
							qBox.FontName = text.Value;
						}
						break;
					case "BackgroundBrush":
						qBox.BackgroundColor = XmlProcs.ConvertXmlToColor(qNode, culture);
						break;
					case "BackgroundOpacity":
						if(text != null)
						{
							float value = 0F;
							if(!float.TryParse(text.Value, NumberStyles.Float, 
							                   culture, out value))
							{
								if(!float.TryParse(text.Value, NumberStyles.Float,
								                   CultureInfo.InvariantCulture,
								                   out value))
								{
									XmlException ex = new XmlException(
										"Bad QsosBox property value in card file." +
										Environment.NewLine +
										"Did you modify the card file by hand?");
									ex.Data.Add("Property", "BackgroundOpacity");
									ex.Data.Add("Value", text.Value);
									App.Logger.Log(ex);
								}
							}
							qBox.BackgroundOpacity = value;
						}
						break;
					case "ConfirmingMultiText":
						CountMacro cMacro = new CountMacro();
						StaticText designText = new StaticText();
						designText.Text = "Confirming 2-Way QSO$ With";
						cMacro.DesignText.Clear();
						cMacro.DesignText.Add(designText);
						StaticText falseText = new StaticText();
						falseText.Text = text.Value;
						cMacro.FalseText.Clear();
						cMacro.FalseText.Add(falseText);
						qBox.ConfirmingText.Clear();
						qBox.ConfirmingText.Add(cMacro);
						break;
					case "Confirming1Text":		// code assumes that Confirming1Text occurs
												// after ConfirmingMultiText in the
												// xqsl file. This matches the save order in
												// QslDesignAndPrint.
						StaticText trueText = new StaticText();
						trueText.Text = text.Value;
						CountMacro countMacro = qBox.ConfirmingText[0] as CountMacro;
						countMacro.TrueText.Clear();
						countMacro.TrueText.Add(trueText);
						break;
					case "ViaText":
						if(text != null)
						{
							qBox.ViaText = text.Value;
						}
						break;
					case "YYYYMMDDText":
						if(text != null)
						{
							qBox.YYYYMMDDText = text.Value;
						}
						break;
					case "DDMMMYYText":
						if(text != null)
						{
							qBox.DDMMMYYText = text.Value;
						}
						break;
					case "DDMMYYText":
						if(text != null)
						{
							qBox.DDMMYYText = text.Value;
						}
						break;
					case "TimeText":
						if(text != null)
						{
							qBox.TimeText = text.Value;
						}
						break;
					case "ModeText":
						if(text != null)
						{
							qBox.ModeText = text.Value;
						}
						break;
					case "BandText":
						if(text != null)
						{
							qBox.BandText = text.Value;
						}
						break;
					case "FrequencyText":
						if(text != null)
						{
							qBox.FreqText = text.Value;
						}
						break;
					case "RSTText":
						if(text != null)
						{
							qBox.RSTText = text.Value;
						}
						break;
					case "QSLText":
						if(text != null)
						{
							qBox.QSLText = text.Value;
						}
						break;
					case "PseText":
						if(text != null)
						{
							qBox.PseText = text.Value;
						}
						break;
					case "TnxText":
						if(text != null)
						{
							qBox.TnxText = text.Value;
						}
						break;
				}
				qNode = XmlProcs.GetNextSiblingElement(qNode);
			}
			qBox.CalculateRectangle(qBox.MaximumQsos);
			return qBox;
		}
	}
}
