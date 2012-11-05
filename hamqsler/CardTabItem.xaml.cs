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
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace hamqsler
{
	/// <summary>
	/// Interaction logic for CardTabItem.xaml
	/// </summary>
	public partial class CardTabItem : TabItem
	{
		/// <summary>
		/// CardTabItem constructor
		/// </summary>
		/// <param name="cardWidth">Width of the card in graphics independent units</param>
		/// <param name="cardHeight">Height of the card in graphics independent units</param>
		public CardTabItem(double cardWidth, double cardHeight)
		{
			InitializeComponent();
			// create a card and position it in the middle of the CardCanvas
			cardCanvas.CreateCard(cardWidth, cardHeight);
			this.DataContext = cardCanvas.QslCard;
			
		}
		
		/// <summary>
		/// Handles PrintCardOutlines checkbox clicked event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void PrintCardOutlines_Clicked(object sender, RoutedEventArgs e)
		{
			// force redisplay of the card
			cardCanvas.QslCard.InvalidateVisual();
		}
		
		/// <summary>
		/// Set visibility of properties groupboxes based on the CardItem selected in the card
		/// </summary>
		/// <param name="card">Card that contains the CardItems</param>
		public void SetPropertiesVisibility(Card card)
		{
			bool displayCardProps = true;
			if(card.BackImage.IsSelected)
			{
				displayCardProps = false;
			}
			backgroundImageProperties.Visibility = (card.BackImage.IsSelected) ?
				Visibility.Visible : Visibility.Collapsed;
			cardProperties.Visibility = displayCardProps ? Visibility.Visible :
				Visibility.Collapsed;
		}
		
		/// <summary>
		/// Handler for loadBackgroundImage click event.
		/// Load an image file
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void LoadBackgroundImage_Click(object sender, RoutedEventArgs e)
		{
			// create and open OpenFileDialog
			OpenFileDialog oDialog = new OpenFileDialog();
			oDialog.Title = "Select Background Image";
			oDialog.InitialDirectory = ((App)Application.Current).UserPreferences.DefaultImagesFolder;
			oDialog.Filter = "Image Files (*.BMP, *.JPG, *.GIF, *.PNG)|*.BMP;*.JPG;*.GIF;*.PNG";
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
				// if previous image, this will force new image to be centred on
				// the card, even if reloading the same image.
				cardCanvas.QslCard.BackImage.ImageFileName = string.Empty;
				// set the file name and show in backgroundImageFileNameTextBox
				cardCanvas.QslCard.BackImage.ImageFileName = fileName;
				backgroundImageFileNameTextBox.Text = fileName;
			}
		}
	}
}