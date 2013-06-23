/*
 *  Author:
 *       Jim Orcheson <jimorcheson@gmail.com>
 * 
 *  Copyright (c) 2012, 2013 Jim Orcheson
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
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using Microsoft.Win32;

namespace hamqsler
{
	/// <summary>
	/// Interaction logic for CardTabItem.xaml
	/// </summary>
	public partial class CardTabItem : TabItem
	{
		private static int BEEPFREQUENCY = 800;		// Hz
		private static int BEEPDURATION = 200;		// ms
		
		/// <summary>
		/// CardTabItem constructor
		/// </summary>
		/// <param name="cardWidth">Width of the card in graphics independent units</param>
		/// <param name="cardHeight">Height of the card in graphics independent units</param>
		public CardTabItem(int cardWidth, int cardHeight)
		{
			InitializeComponent();
			// create a card and position it in the middle of the CardCanvas
			CardWF card = new CardWF(cardWidth, cardHeight, true);
			cardPanel.AddCard(card);
			cardProperties.Visibility = Visibility.Visible;
			cardProperties.printPropsPanel.cardsLayoutGroupBox.Visibility = 
				Visibility.Collapsed;
			cardProperties.QslCard = cardPanel.QslCard;
			cardPanel.QslCard.DispPropertyChanged += OnQslCardDispPropertyChanged;
			// load list of font names that are available to Windows Forms
			System.Drawing.Text.InstalledFontCollection fontCol =
				new System.Drawing.Text.InstalledFontCollection();
			foreach(System.Drawing.FontFamily family in fontCol.Families)
			{
				FontFaceComboBox.Items.Add(family.Name);
				QsosBoxFontFaceComboBox.Items.Add(family.Name);
			}
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="card">Card object to display on the CardTabItem</param>
		public CardTabItem(CardWF card)
		{
			InitializeComponent();
			cardPanel.AddCard(card);
			cardProperties.Visibility = Visibility.Visible;
			cardProperties.printPropsPanel.cardsLayoutGroupBox.Visibility = 
				Visibility.Collapsed;
			cardProperties.QslCard = cardPanel.QslCard;
			cardPanel.QslCard.DispPropertyChanged += OnQslCardDispPropertyChanged;
			// load list of font names that are available to Windows Forms
			System.Drawing.Text.InstalledFontCollection fontCol =
				new System.Drawing.Text.InstalledFontCollection();
			foreach(System.Drawing.FontFamily family in fontCol.Families)
			{
				FontFaceComboBox.Items.Add(family.Name);
				QsosBoxFontFaceComboBox.Items.Add(family.Name);
			}
		}
		
		/// <summary>
		/// Set visibility of properties groupboxes based on the CardItem selected in the card
		/// </summary>
		/// <param name="card">Card that contains the CardItems</param>
		public void SetPropertiesVisibility(CardWFItem ci)
		{
			HideAllPropertiesPanels();
			if(ci != null)
			{
				if(ci.GetType() == typeof(BackgroundWFImage))
				{
					backgroundImageProperties.Visibility = Visibility.Visible;
					backgroundImageProperties.DataContext = ci;
				}
				else if(ci.GetType() == typeof(SecondaryWFImage))
				{
					secondaryImageProperties.Visibility = Visibility.Visible;
					secondaryImageProperties.DataContext = ci;
				}
				else if(ci.GetType() == typeof(TextWFItem))
				{
					TextWFItem ti = (TextWFItem)ci;
					textItemProperties.Visibility = Visibility.Visible;
					textItemProperties.DataContext = ti;
					if(ti.Text.Count == 1 && ti.Text[0].GetType() == typeof(StaticText))
					{
						Text.Visibility = Visibility.Visible;
						StaticText sText = (StaticText)ti.Text[0];
						Text.Text = sText.Text;
					}
					else
					{
						Text.Visibility = Visibility.Collapsed;
					}
				}
				else if(ci.GetType() == typeof(QsosWFBox))
				{
					qsosBoxProperties.Visibility = Visibility.Visible;
					qsosBoxProperties.DataContext = ci;
				}
			}
			else
			{
				cardProperties.Visibility = Visibility.Visible;
				cardProperties.DataContext = cardPanel.QslCard;
			}
		}
		
		/// <summary>
		/// Helper method that hides all properties panels. Normally called before a single
		/// proeprty panel is set visible
		/// </summary>
		private void HideAllPropertiesPanels()
		{
			cardProperties.Visibility = Visibility.Collapsed;
			backgroundImageProperties.Visibility = Visibility.Collapsed;
			secondaryImageProperties.Visibility = Visibility.Collapsed;
			textItemProperties.Visibility = Visibility.Collapsed;
			qsosBoxProperties.Visibility = Visibility.Collapsed;
		}
		
		/// <summary>
		/// Handler for loadBackgroundImage click event.
		/// Load an image file
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void LoadBackgroundImage_Click(object sender, RoutedEventArgs e)
		{
			string fileName = GetImageFileNameForLoading("Select Background Image");
			if(fileName != null)
			{
				// if previous image, this will force new image to be centred on
				// the card, even if reloading the same image.
				cardPanel.QslCard.BackgroundImage.ImageFileName = string.Empty;
				// set the file name and show in backgroundImageFileNameTextBox
				cardPanel.QslCard.BackgroundImage.ImageFileName = fileName;
				backgroundImageFileNameTextBox.Text = fileName;
			}
		}

		/// <summary>
		/// Handler for loadSecondaryImage click event.
		/// Load an image file
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void LoadSecondaryImage_Click(object sender, RoutedEventArgs e)
		{
			string fileName = GetImageFileNameForLoading("Select Secondary Image");
			if(fileName != null)
			{
				// if previous image, this will force new image to be centred on
				// the card, even if reloading the same image.
				SecondaryWFImage si = cardPanel.QslCard.GetSelectedItem() as SecondaryWFImage;
				if(si != null)
				{
					si.ImageFileName = string.Empty;
					// set the file name and show in secondaryImageFileNameTextBox
					si.ImageFileName = fileName;
					secondaryImageFileNameTextBox.Text = fileName;
				}
			}
		}
		
		/// <summary>
		/// Open an OpenFileDialog to retrieve an image file name for loading
		/// </summary>
		/// <returns>file name, or null if user clicks the Cancel button or otherwise
		/// closes the dialog</returns>
		private string GetImageFileNameForLoading(string title)
		{
			// create and open OpenFileDialog
			OpenFileDialog oDialog = new OpenFileDialog();
			oDialog.Title = title;
			oDialog.InitialDirectory = ((App)Application.Current).UserPreferences.DefaultImagesFolder;
			oDialog.Filter = "Image Files (*.BMP, *.JPG, *.GIF, *.PNG, *.TIF, *.TIFF)|" +
				"*.BMP;*.JPG;*.GIF;*.PNG;*.TIF;*.TIFF";
			oDialog.CheckFileExists = true;
			oDialog.Multiselect = false;
			if(oDialog.ShowDialog() == true)
			{
				// file has been selected, so see if it is in hamqsler folder or child folder
				// and modify the filename appropriately.
				// This helps support moving the hamqsler folder from one computer to another
				// or one user to another.
				string hamQSLerFolder = ((App)Application.Current).HamqslerFolder;
				string fileName = oDialog.FileName;
				if(fileName.StartsWith(hamQSLerFolder))
				{
					fileName = "$hamqslerFolder$\\" + fileName.Substring(hamQSLerFolder.Length);
				}
				else if(fileName.StartsWith(Environment.GetFolderPath(
					Environment.SpecialFolder.MyDocuments)))
				{
					fileName = "$MyDocs$\\" + fileName.Substring(Environment.GetFolderPath(
						Environment.SpecialFolder.MyDocuments).Length);
				}
				return fileName;
			}
			else
			{
				return null;
			}
		}
		
		/// <summary>
		/// Handler for TextColorButton clicked event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void TextColorButton_Clicked(object sender, RoutedEventArgs e)
		{
			Color col = GetColorFromColorDialog((SolidColorBrush)TextColorButton.Background);
			TextColorButton.Background = new SolidColorBrush(col);
		}
		
		/// <summary>
		/// Helper method that displays a ColorDialog and returns the selected color
		/// </summary>
		/// <param name="brush">SolidColorBrush object containing the color to initialize the dialog with</param>
		/// <returns>Selected color, or the color from the brush if no color selected</returns>
		private Color GetColorFromColorDialog(SolidColorBrush brush)
		{
			// create and display a ColorDialog
			System.Windows.Forms.ColorDialog cDialog = new System.Windows.Forms.ColorDialog();
			cDialog.Color = System.Drawing.Color.FromArgb(brush.Color.A, brush.Color.R,
			                                              brush.Color.G, brush.Color.B);
			Color c = brush.Color;
			cDialog.FullOpen = true;
			if(cDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				// a color was selected, so convert to WPF color and set button background to that color
				System.Drawing.Color color = cDialog.Color;
				c.A = color.A;
				c.R = color.R;
				c.G = color.G;
				c.B = color.B;
			}
			return c;
		}
		
		/// <summary>
		/// PreviewTextInput handler for FontSizeComboBox - allows only 0-9 and '.' characters
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void FontSizeComboBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			Regex sizeReg = new Regex("^\\d*[\\.,]{0,1}\\d{0,1}$");
			if(!sizeReg.IsMatch(e.Text))
			{
				// not valid
				Console.Beep(BEEPFREQUENCY, BEEPDURATION);		// alert user
				e.Handled = true;			// consume the event so that the character is not processed
			}
			
		}
		
		/// <summary>
		/// Handler for the LineTextColorButton click event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void LineTextColorButton_Clicked(object sender, RoutedEventArgs e)
		{
			Color col = GetColorFromColorDialog((SolidColorBrush)LineTextColorButton.Background);
			LineTextColorButton.Background = new SolidColorBrush(col);
		}
		
		/// <summary>
		/// Handler for the CallsignColorButton click event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void CallsignColorButton_Clicked(object sender, RoutedEventArgs e)
		{
			Color col = GetColorFromColorDialog((SolidColorBrush)CallsignColorButton.Background);
			CallsignColorButton.Background = new SolidColorBrush(col);
		}
		
		/// <summary>
		/// Handler for the ManagerColorButton click event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ManagerColorButton_Clicked(object sender, RoutedEventArgs e)
		{
			Color col = GetColorFromColorDialog((SolidColorBrush)ManagerColorButton.Background);
			ManagerColorButton.Background = new SolidColorBrush(col);
		}

		/// <summary>
		/// Handler for the BackgroundButton click event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void BackgroundColorButton_Clicked(object sender, RoutedEventArgs e)
		{
			Color col = GetColorFromColorDialog((SolidColorBrush)BackgroundColorButton.Background);
			BackgroundColorButton.Background = new SolidColorBrush(col);
		}
		
		/// <summary>
		/// Handler for MacrosButton clicked event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void OnMacrosButtonClicked(object sender, RoutedEventArgs e)
		{
			TextWFItem ti = cardPanel.QslCard.GetSelectedItem() as TextWFItem;
			TextMacrosDialog dialog = new TextMacrosDialog(ti.Text);
			dialog.ShowDialog();
			if(ti.Text.Count == 1 && ti.Text[0].GetType() == typeof(StaticText))
			{
				Text.Visibility = Visibility.Visible;
				Text.Text = ti.Text.GetText(cardPanel.QslCard, null, true);
			}
			else
			{
				Text.Visibility = Visibility.Collapsed;
			}
			// remove the first TextPart if it is an empty StaticText object
			ti.Text.RemoveExtraneousStaticText();
			foreach(TextPart part in ti.Text)
			{
				part.RemoveExtraneousStaticTextMacros();
			}
			ti.Text.RemoveExtraneousStaticText();
			ti.CalculateRectangle();
			cardPanel.QslCard.RaiseDispPropertyChangedEvent();
			// must set IsDirty because changing contents of TextParts does not trigger
			// Card PropertyChanged event
			if(dialog.IsDirty)
			{
				cardPanel.QslCard.IsDirty = true;
			}
		}
		
		/// <summary>
		/// Handler for Text box TextChanged event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void Text_TextChanged(object sender, TextChangedEventArgs e)
		{
			TextWFItem ti = textItemProperties.DataContext as TextWFItem;
			if(ti != null)
			{
				StaticText sText = ti.Text[0] as StaticText;
				if(sText != null)
				{
					sText.Text = Text.Text;
					ti.CalculateRectangle();
					cardPanel.QslCard.RaiseDispPropertyChangedEvent();
				// must set IsDirty because changing contents of StaticText object does not trigger
				// Card PropertyChanged event
					cardPanel.QslCard.IsDirty = true;
				}
			}
		}
		
		/// <summary>
		/// Handler for CardTabButton click event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void CardTabButton_Click(object sender, RoutedEventArgs e)
		{
			((MainWindow)App.Current.MainWindow).CloseCardTab(this, true);
		}
		
		/// <summary>
		/// Set the text in the tab
		/// </summary>
		public void SetTabLabel()
		{
			HeaderText.Text = (cardPanel.QslCard.IsDirty ? "*" : string.Empty);
			string fileName = cardPanel.QslCard.FileName;
			if(fileName != null)
			{
				FileInfo fileInfo = new FileInfo(fileName);
				string fName = fileInfo.Name;
				HeaderText.Text += fName;
			}
			else
			{
				HeaderText.Text += "New Card";
			}
		}
		
		private void OnQslCardDispPropertyChanged(object sender, EventArgs e)
		{
			SetTabLabel();
			((MainWindow)App.Current.MainWindow).SetTitle(cardPanel.QslCard.FileName, 
			                                              cardPanel.QslCard.IsDirty);
		}
	}
}