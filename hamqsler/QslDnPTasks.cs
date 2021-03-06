﻿/*
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
using System.Globalization;
using System.IO;
using System.Windows;
using System.Xml;

namespace hamqsler
{
	/// <summary>
	/// Static class that encapsulates QslDesignAndPrint related tasks
	/// </summary>
	public static class QslDnPTasks
	{
		/// <summary>
		/// Move image and other files from one folder to another folder
		/// </summary>
		/// 
		private static bool copyError = false;
		public static bool CopyError
		{
			get {return copyError;}
		}
		
		private static List<string> fontNames = new List<string>();
		
		// scale factor used to adjust card item dimensions from WPF to Windows Forms
		private const float WPFToWFScale = 100F/96F;
		// scale factor used to adjust font sizes from WPF to Windows Forms
		private const float FontScaleFactor = 0.75F;
		
		/// <summary>
		/// Static constructor
		/// </summary>
		static QslDnPTasks()
		{
			System.Drawing.Text.InstalledFontCollection fontCol =
				new System.Drawing.Text.InstalledFontCollection();
			foreach(System.Drawing.FontFamily family in fontCol.Families)
			{
				fontNames.Add(family.Name);
			}

		}
		/// Move image and other files from QslDnp to hamqsler
		/// </summary>
		/// <param name="fromFolder">Name of the directory to copy files from</param>
		/// <param name="toFolder">Name of the directory to copy files to</param>
		public static int MoveFiles(string fromFolder, string toFolder)
		{
			int copyCount = 0;
			try
			{
				foreach(string file in Directory.EnumerateFiles(fromFolder))
				{
					// do not copy xqsl, xml or log files, or files used in the QslDesignAndPrint
					// getting started tutorial
					if((!file.EndsWith(".xqsl") && !file.EndsWith(".xml") && 
					    !file.EndsWith(".log") && !file.EndsWith(".log-old")) &&
					   (!fromFolder.EndsWith("Samples") || (!file.EndsWith("test2.adi") &&
					                                        !file.EndsWith("moonbasin.jpg"))))
					{
						FileInfo fInfo = new FileInfo(file);
						try
						{
							string toFile = toFolder + "\\" + fInfo.Name;
							// if directory does not exist in hamqsler, then create it
							FileInfo hFile = new FileInfo(toFile);
							DirectoryInfo dInfo = new DirectoryInfo(hFile.DirectoryName);
							if(!dInfo.Exists)
							{
								Directory.CreateDirectory(hFile.DirectoryName);
							}
							// copy file to desination folder/subfolder
							File.Copy(file, toFile);
							copyCount++;
							string fromTo = string.Format("File: {0} copied to {1}", file, toFile);
							App.Logger.Log(fromTo);
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
				foreach(string dir in Directory.EnumerateDirectories(fromFolder))
				{
					string toDir = toFolder + dir.Substring(fromFolder.Length);
					copyCount += MoveFiles(dir, toDir);
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
			return copyCount;
		}
		
		/// <summary>
		/// Convert QslDnP card files to hamqsler card files
		/// </summary>
		/// <param name="fromFolder">name of the source directory of the card files to convert</param>
		/// <param name="toFolder">name of the directory to save the converted card files in</param>
		public static int ConvertCardFiles(string fromFolder, string toFolder)
		{
			int convertCount = 0;
			foreach(string file in Directory.EnumerateFiles(fromFolder))
			{
				if(file.EndsWith(".xqsl"))
				{
					try
					{
						// do not copy ...\Samples\sample.xqsl because this would overwrite
						// the file created in the getting started tutorial
						if(!fromFolder.EndsWith("Samples") ||
						   (fromFolder.EndsWith("Samples") && !file.EndsWith("sample.xqsl")))
						{
							CardWF card = QslDnPTasks.ConvertFromXQSL(file);
							if(card == null)
							{
								continue;
							}
							FileInfo fInfo = new FileInfo(file);
							string toFile = toFolder + "\\" + fInfo.Name;
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
							convertCount++;
							string fromTo = string.Format("Card file: {0} converted to {1}",
							                              file, toFile);
							App.Logger.Log(fromTo);
						}
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
				// now process every subdirectory in this folder
				foreach(string dir in Directory.EnumerateDirectories(fromFolder))
				{
					string toDir = toFolder + dir.Substring(fromFolder.Length);
					convertCount += ConvertCardFiles(dir, toDir);
				}
			}
			return convertCount;
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
			try
			{
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
					case "LandscapeCard":
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
				fStream.Close();
				return card;
			}
			catch(IOException ioe)
			{
				Exception ex = new Exception("IO error occurred loading file: " + file, ioe);
				App.Logger.Log(ex, true, false);
				string err = string.Format("IO error encountered while trying to convert file {0}." +
				                           Environment.NewLine +
				                           "See the log file for more information.", file);

				MessageBox.Show(err, "IO Error During Conversion", MessageBoxButton.OK,
				                MessageBoxImage.Error);
				return null;
			}
			catch(XmlException xe)
			{
				Exception ex = new Exception("XML error occurred while reading file: " + file, xe);
				App.Logger.Log(ex, true, false);
				string err = string.Format("XML error encountered while trying to convert file {0}." +
				                           Environment.NewLine +
				                           "See the log file for more information.", file);

				MessageBox.Show(err, "XML Error During Conversion", MessageBoxButton.OK,
				                MessageBoxImage.Error);
				return null;
			}
					
					
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
						if(sImage != null)
						{
							card.SecondaryImages.Add(sImage);
						}
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
							string hamQSLerFolder = ((App)Application.Current).HamqslerFolder;
							string fileName = text.Value;
							if(fileName.StartsWith(myDocs))
							{
								fileName = "$MyDocs$" + fileName.Substring(myDocs.Length);
							}
							// if(fileName.StartsWith(qslDir))
							
							if(fileName.StartsWith("$MyDocs$\\QslDnP"))
							{
								fileName = "$hamqslerFolder$" + 
									fileName.Substring("$MyDocs$\\QslDnP".Length);
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
		/// <returns>The created SecondaryWFImage object or null if ImageFileName is empty string</returns>
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
						
						tItem.TextFontFace = GetValidFont(text.Value);
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
							qBox.FontName = GetValidFont(text.Value);
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
						designText.Text = ((App)App.Current).UserPreferences.
							ConfirmingText.GetText(card, null, true);
						cMacro.DesignText.Clear();
						cMacro.DesignText.Add(designText);
						StaticText falseText = new StaticText();
						string fText = text.Value;
						if(!fText.EndsWith("  "))
						{
							fText += "  ";
						}
						falseText.Text = fText;
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
						string tText = text.Value;
						if(!tText.EndsWith("  "))
						{
							tText += "  ";
						}
						trueText.Text = tText;
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
			return qBox;
		}
		
		/// <summary>
		/// Validate the font.
		/// </summary>
		/// <param name="font">Font to validate</param>
		/// <returns>input font if installed on system, or the first font in the installed fonts
		/// list if not</returns>
		private static string GetValidFont(string font)
		{
			string fName = fontNames[0];
			foreach(string fontName in fontNames)
			{
				if(fontName.Equals(font))
				{
					return font;
				}
			}
			App.Logger.Log("Font: " + font + " not found. Replaced with " + fName);
			return fName;
		}
		
	}
}
