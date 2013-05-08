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
using System.Drawing;
using System.IO;
using System.Windows;
using System.Xml.Serialization;

namespace hamqsler
{
	/// <summary>
	/// Description of ImageWFBase.
	/// </summary>
	public abstract class ImageWFBase : CardWFItem
	{
		private static readonly DependencyProperty ImageFileNameProperty =
			DependencyProperty.Register("ImageFileName", typeof(string), typeof(CardImageBase),
			                             new PropertyMetadata(null));
		public string ImageFileName
		{
			get {return (string)GetValue(ImageFileNameProperty);}
			set {SetValue(ImageFileNameProperty, value);}
		}
		
		public static System.Drawing.Bitmap EmptyImage = new System.Drawing.Bitmap(1,1);
		protected static readonly DependencyProperty ImageProperty =
			DependencyProperty.Register("Image", typeof(Image), typeof(CardImageBase),
			                            new PropertyMetadata(EmptyImage));
		[XmlIgnore]
		public Bitmap Image
		{
			get {return (Bitmap)GetValue(ImageProperty);}
			set {SetValue(ImageProperty, value);}
		}
		
		/// <summary>
		/// Default constructor
		/// </summary>
		public ImageWFBase()
		{
		}

		/// <summary>
		/// Calculates the size and location for the image
		/// </summary>
		protected abstract void CalculateRectangle();
        
        /// <summary>
        /// Calculate default size and location (when there is no image file specified)
        /// </summary>
        protected abstract void ResetRectangle();
        
        /// <summary>
        /// Handle PropertyChanged event
        /// </summary>
        /// <param name="e">DependencyPropertyChangedEventArgs object</param>
		protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
		{
			base.OnPropertyChanged(e);
			if(e.Property == ImageFileNameProperty)
			{
				string fName = e.NewValue as string;
				if(fName != null && fName != string.Empty)
				{
					string hamqslerFolder = ((App)Application.Current).HamqslerFolder;
					// expand file name if using relative path
					if(fName.StartsWith(@"$hamqslerFolder$\"))
					{
						fName = hamqslerFolder + fName.Substring(@"$hamqslerFolder$\".Length);
					}
					FileInfo fInfo = new FileInfo(fName);
					if(fInfo.Exists)
					{
						// load and create the image
						try
						{
							Image = new Bitmap(fName);
							if(QslCard != null)
							{
								QslCard.IsDirty = true;
							}
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
						Exception ex = new Exception(msg);
						throw ex;
					}
					CalculateRectangle();
				}
				else
				{
					// reset bImage
					Image = EmptyImage;
					ResetRectangle();
				}
				if(QslCard != null)
				{
					QslCard.IsDirty = true;
				}
			}
		}
	}
}
