/*
 *  Author:
 *       Jim Orcheson <jimorcheson@gmail.com>
 * 
 *  Copyright (c) 2013 Jim Orcheson
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
using System.ComponentModel;
using System.Drawing.Printing;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace hamqsler
{
	/// <summary>
	/// CustomPaperSizesDialog - display and save custom paper sizes
	/// </summary>
	public partial class CustomPaperSizesDialog : Window, IDataErrorInfo
	{
		public static RoutedCommand SaveButtonCommand = new RoutedCommand();
		
		private static readonly DependencyProperty PaperHeightProperty =
			DependencyProperty.Register("PaperHeight", typeof(string), typeof(CustomPaperSizesDialog),
			                            new PropertyMetadata("0"));
		public string PaperHeight
		{
			get {return GetValue(PaperHeightProperty) as string;}
			set {SetValue(PaperHeightProperty, value);}
		}
		
		private static readonly DependencyProperty PaperWidthProperty =
			DependencyProperty.Register("PaperWidth", typeof(string), typeof(CustomPaperSizesDialog),
			                            new PropertyMetadata("0"));
		public string PaperWidth
		{
			get {return GetValue(PaperWidthProperty) as string;}
			set {SetValue(PaperWidthProperty, value);}
		}
		
		private static readonly DependencyProperty PaperNameProperty =
			DependencyProperty.Register("PaperName", typeof(string), typeof(CustomPaperSizesDialog),
			                            new PropertyMetadata(string.Empty));
		public string PaperName
		{
			get {return GetValue(PaperNameProperty) as string;}
			set {SetValue(PaperNameProperty, value);}
		}
		
		private int BEEPFREQUENCY = 800;
		private int BEEPDURATION = 250;
		private Regex whRegex = null;
		private Regex regexData = new Regex("^[0-9]*([\\.,][0-9]{0,2}){0,1}$");
		private MinMaxPaperSizes minMaxPaperSizes = new MinMaxPaperSizes();
		
		/// <summary>
		/// Constructor
		/// </summary>
		public CustomPaperSizesDialog()
		{
			DataContext = this;
			whRegex = new Regex("[0-9\\.,]");
			InitializeComponent();
			paperWidthTextBlock.Text = minMaxPaperSizes.WidthInches;
			paperHeightTextBlock.Text = minMaxPaperSizes.HeightInches;
			foreach(PaperSize size in ((App)App.Current).UserPreferences.CustomPaperSizes)
			{
				paperNameCombobox.Items.Add(size.PaperName);
			}
			paperNameCombobox.Items.Add("Custom");
			paperNameCombobox.Text = "Custom";
		}
		
		/// <summary>
		/// Handler for Close button click event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void Button_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
		
		/// <summary>
		/// Handler for paperNameCombobox selection changed event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">SelectionChangedEventArgs object</param>
		void PaperNameCombobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			float width, height;
			PaperName = e.AddedItems[0] as string;
			if(PaperName.Equals("Custom"))
			{
				PrinterSettings settings = new PrinterSettings();
				PaperSize paperSize = settings.DefaultPageSettings.PaperSize;
				width = (float)(paperSize.Width) / 100F;
				height = (float)paperSize.Height / 100F;
				if(!(bool)inchesButton.IsChecked)
				{
					width *= 25.4F;
					height *= 25.4F;
				}
			}
			else
			{
				PaperSize pSize = null;
				foreach(PaperSize size in ((App)App.Current).UserPreferences.CustomPaperSizes)
				{
					if(size.PaperName.Equals(PaperName))
					{
						pSize = size;
						break;
					}
				}
				if(pSize != null)
				{
					width = (float)(pSize.Width) / 100F;
					height = (float)(pSize.Height) / 100F;
					if(!(bool)inchesButton.IsChecked)
					{
						width *= 25.4F;
						height *= 25.4F;
					}
				}
				else
				{
					Exception ex = new Exception("Program Logic Error: List of paper sizes does not" +
					                             Environment.NewLine +
					                             "match the list of paper size names in the combo box.");
					string comboboxNames = string.Empty;
					for(int i = 0; i < paperNameCombobox.Items.Count - 2; i ++)
					{
						comboboxNames += paperNameCombobox.Items[i] + ", ";
					}
					comboboxNames += paperNameCombobox.Items[paperNameCombobox.Items.Count - 1];
					ex.Data.Add("paperNameComboBox Items", comboboxNames);
					string paperSizeNames = string.Empty;
					List<PaperSize> customSizes = ((App)App.Current).UserPreferences.CustomPaperSizes;
					for(int i = 0; i < customSizes.Count - 2; i++)
					{
						paperSizeNames += customSizes[i].PaperName + ", ";
					}
					paperSizeNames += customSizes[customSizes.Count - 1];
					ex.Data.Add("Custom Sizes", paperSizeNames);
					App.Logger.Log(ex,false, true);
					return;
				}
			}
			PaperWidth = string.Format("{0:f2}", width);
			PaperHeight = string.Format("{0:f2}", height);
		}
		
		/// <summary>
		/// Handler for inchesButton checked event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void InchesButton_Checked(object sender, RoutedEventArgs e)
		{
			
			float width = float.Parse(PaperWidth);
			float height = float.Parse(PaperHeight);
			PaperWidth = string.Format("{0:f2}", width / 25.4F);
			PaperHeight = string.Format("{0:f2}", height / 25.4F);
			paperWidthTextBlock.Text = minMaxPaperSizes.WidthInches;
			paperHeightTextBlock.Text = minMaxPaperSizes.HeightInches;
		}
		
		/// <summary>
		/// Handler for inchesButton unchecked event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void InchesButton_Unchecked(object sender, RoutedEventArgs e)
		{
			if(widthTextBox.Text != string.Empty)
			{
				float width = float.Parse(PaperWidth);
				float height = float.Parse(PaperHeight);
				PaperWidth= string.Format("{0:f2}", width * 25.4F);
				PaperHeight = string.Format("{0:f2}", height * 25.4F);
				paperWidthTextBlock.Text = minMaxPaperSizes.WidthMM;
				paperHeightTextBlock.Text = minMaxPaperSizes.HeightMM;
			}
		}
		
		/// <summary>
		/// Handler for widthTextBox preview text event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">TextCompositionEventArgs</param>
		void WidthTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			if(!whRegex.IsMatch(e.Text))
			{
				Console.Beep(BEEPFREQUENCY, BEEPDURATION);
				e.Handled = true;
			}
		}
		
		/// <summary>
		/// Handler for heightTextBox preview text event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">TextCompositionEventArgs object</param>
		void HeightTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			if(!whRegex.IsMatch(e.Text))
			{
				Console.Beep(BEEPFREQUENCY, BEEPDURATION);
				e.Handled = true;
			}
		}
		
		/// <summary>
		/// Error handling for IDataErrorInfo interface
		/// </summary>
		public string this[string propertyName]
		{
			get 
			{
				string result = null;
				if(propertyName == "PaperHeight")
				{
					if(PaperHeight.Equals(string.Empty))
					{
					   	result = "Value must be between the minimum and maximum sizes" +
								" shown below.";
					}
					else if(!regexData.IsMatch(PaperHeight))
					{
						result = "Must contain only digits and a single" +
							" decimal separator with 0 to 2 decimal places.";
					}
					else
					{
						string strHeight = PaperHeight.Replace(",", ".");
						float height = float.Parse(strHeight);
						float minHeight = minMaxPaperSizes.MinHeightInches;
						float maxHeight = minMaxPaperSizes.MaxHeightInches;
						if(!(bool)inchesButton.IsChecked)
						{
							minHeight = minMaxPaperSizes.MinHeightMM;
							maxHeight = minMaxPaperSizes.MaxHeightMM;
						}
						if(height < minHeight || height > maxHeight)
						{
							result = "Value must be between the minimum and maximum sizes" +
								" shown below.";
						}
					}
				}
				else if(propertyName == "PaperWidth")
				{
					if(PaperWidth.Equals(string.Empty))
					{
					   	result = "Value must be between the minimum and maximum sizes" +
								" shown below.";
					}
					else if(!regexData.IsMatch(PaperWidth))
					{
						result = "Must contain only digits and a single" +
							" decimal separator with 0 to 2 decimal places.";
					}
					else
					{
						string strWidth = PaperWidth.Replace(",", ".");
						float width = float.Parse(strWidth);
						float minWidth = minMaxPaperSizes.MinWidthInches;
						float maxWidth = minMaxPaperSizes.MaxWidthInches;
						if(!(bool)inchesButton.IsChecked)
						{
							minWidth = minMaxPaperSizes.MinWidthMM;
							maxWidth = minMaxPaperSizes.MaxWidthMM;
						}
						if(width < minWidth || width > maxWidth)
						{
							result = "Value must be between the minimum and maximum sizes" +
								" shown below.";
						}
					}
				}
				return result;
			}
		}

        // property required by IDataErrorInfo interface, but not used by WPF
        public string Error
        {
        	get {return null;}
        }
        
        /// <summary>
        /// Handler for save button CanExecute event
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">CanExecuteRoutedEventArgs object</param>
        private void SaveButton_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
        	e.CanExecute = !PaperName.Equals("Custom");
        }
        
        /// <summary>
        /// Handler for save button Executed event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Executed(object sender, ExecutedRoutedEventArgs e)
        {
    		float width = float.Parse(PaperWidth, CultureInfo.InvariantCulture);
    		width *= 100F;
    		float height = float.Parse(PaperHeight, CultureInfo.InvariantCulture);
    		height *= 100F;
    		if(!(bool)inchesButton.IsChecked)
    		{
    			width /= 25.4F;
    			height /= 25.4F;
    		}
    		bool paperSizeFound = false;
        	foreach(PaperSize size in ((App)App.Current).UserPreferences.CustomPaperSizes)
        	{
        		if(PaperName.Equals(size.PaperName))
        		{
        			size.Width = (int)width;
        			size.Height = (int)height;
        			paperSizeFound = true;
        		}
        	}
        	if(!paperSizeFound)
        	{
    			PaperSize pSize = new PaperSize(PaperName, (int)width, (int)height);
    			((App)App.Current).UserPreferences.CustomPaperSizes.Add(pSize);
    			paperNameCombobox.Items.Add(PaperName);
    			paperNameCombobox.Text = PaperName;
    		}
        	((App)App.Current).UserPreferences.SerializeAsXml();
        	paperNameCombobox.Text = PaperName;
        }
	}
}