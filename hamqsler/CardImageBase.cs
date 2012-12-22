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
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace hamqsler
{
	/// <summary>
	/// Description of CardImageBase.
	/// </summary>
	[Serializable]
	abstract public class CardImageBase : CardItem
	{
		private static readonly DependencyProperty ImageFileNameProperty =
			DependencyProperty.Register("ImageFileName", typeof(string), typeof(CardImageBase),
			                             new PropertyMetadata(null));
		public string ImageFileName
		{
			get {return (string)GetValue(ImageFileNameProperty);}
			set {SetValue(ImageFileNameProperty, value);}
		}
		
		// overrides standard CardItem.Display accessors to account for the case
		// where there is no image loaded
        public override double DisplayWidth
        {
        	get {return (double)GetValue(DisplayWidthProperty);}
            set {SetValue(DisplayWidthProperty, value); }
        }
        
		public override double DisplayHeight 
		{
			get {return (double)GetValue(DisplayHeightProperty);}
			set {SetValue(DisplayHeightProperty, value); }
		}
        
		[XmlIgnoreAttribute]
		public static BitmapImage EmptyImage = new BitmapImage();
		protected static readonly DependencyProperty BitMapImageProperty =
			DependencyProperty.Register("BitMapImage", typeof(BitmapImage), typeof(CardImageBase),
			                            new PropertyMetadata(EmptyImage));
		[XmlIgnore]
		public BitmapImage BitMapImage
		{
			get {return (BitmapImage)GetValue(BitMapImageProperty);}
			set {SetValue(BitMapImageProperty, value);}
		}
		
        /// <summary>
        /// CalculateRectangle determines the DisplayRectangle size for the image
        /// </summary>
        /// <returns>DisplayRectangle for this image.</returns>
        protected abstract void CalculateRectangle(out double x, out double y, out double w,
                                                  out double h);
        
        /// <summary>
        /// Calculate default display rectangle size (when there is no image file specified)
        /// </summary>
        protected abstract void ResetRectangle();

		/// <summary>
		/// Default constructor
		/// </summary>
        /// <param name="isInDesignMode">Boolean to indicate if this image is to be displayed
        /// in design mode</param>
		public CardImageBase(bool isInDesignMode = true) : base(isInDesignMode)
		{
		}
		
		/// <summary>
		/// Handles PropertyChanged event
		/// </summary>
		/// <param name="e">DependencyProperChangedEventArgs object</param>
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if(e.Property == ImageFileNameProperty)
			{
				// get the image file name
				string fName = (string)e.NewValue;
				if(fName != null && fName != string.Empty)
				{
					string hamqslerFolder = ((App)Application.Current).HamqslerFolder;
					// expand file name if using relative path
					if(fName.StartsWith("$hamqslerFolder$\\"))
					{
						fName = hamqslerFolder + fName.Substring("$hamqslerFolder$\\".Length);
					}
					FileInfo fInfo = new FileInfo(fName);
					if(fInfo.Exists)
					{
						// load and create the image
						try
						{
							BitMapImage = new BitmapImage();
							BitMapImage.BeginInit();
							BitMapImage.UriSource = new Uri(fName, UriKind.RelativeOrAbsolute);
							BitMapImage.EndInit();
						}
						catch(Exception fnfe)
						{
							App.Logger.Log(fnfe, true, true);
							return;
						}
					}
					else
					{
						string msg = string.Format("Image file '{0}' does not exist and cannot be loaded.",
						                             fName);
						App.Logger.Log(msg);
						MessageBox.Show(msg, "Image File Error", MessageBoxButton.OK,
						                MessageBoxImage.Warning);
						BitMapImage = EmptyImage;
					}
					double x, y, w, h;
					CalculateRectangle(out x, out y, out w, out h);
					DisplayX = x;
					DisplayY = y;
					DisplayWidth = w;
					DisplayHeight = h;
				}
				else
				{
					// reset bImage
					BitMapImage = new BitmapImage();
					ResetRectangle();
				}
				// Image has changed, so let QslCard know it has changed
				// and redisplay card
				QslCard.IsDirty = true;
			}
			else if(e.Property == QslCardProperty)
			{
				ResetRectangle();
			}
		}

	}
}
