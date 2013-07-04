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
	[Serializable]
	public abstract class ImageWFBase : CardWFItem
	{
		private static readonly DependencyProperty ImageFileNameProperty =
			DependencyProperty.Register("ImageFileName", typeof(string), typeof(ImageWFBase),
			                             new PropertyMetadata(null));
		public string ImageFileName
		{
			get {return (string)GetValue(ImageFileNameProperty);}
			set {SetValue(ImageFileNameProperty, value);}
		}
		
		public static System.Drawing.Bitmap EmptyImage = new System.Drawing.Bitmap(1,1);
		protected static readonly DependencyProperty ImageProperty =
			DependencyProperty.Register("Image", typeof(Image), typeof(ImageWFBase),
			                            new PropertyMetadata(EmptyImage));
		[XmlIgnore]
		public Bitmap Image
		{
			get {return (Bitmap)GetValue(ImageProperty);}
			set {SetValue(ImageProperty, value);}
		}
		
		private bool modifiedDuringLoad = false;
		[XmlIgnore]
		public bool ModifiedDuringLoad
		{
			get {return modifiedDuringLoad;}
			set {modifiedDuringLoad = value;}
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
				string hamqslerFolder = ((App)Application.Current).HamqslerFolder;
				string fName = e.NewValue as string;
				if(fName != null && fName != string.Empty)
				{
					// expand file name if using relative path
					if(fName.StartsWith(@"$hamqslerFolder$\"))
					{
						fName = hamqslerFolder + fName.Substring(@"$hamqslerFolder$\".Length);
					}
					else if(fName.StartsWith(@"$MyDocs$"))
					{
						fName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + 
							fName.Substring(@"$MyDocs$".Length);
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
						// image file does not exist
						// warn user and give him a chance to look for it
						string msg = string.Format("Image file '{0}' does not exist and cannot be loaded.",
						                             fName);
						App.Logger.Log(msg);
						msg += Environment.NewLine +
	                           "Do you want to search for the file?" +
							   Environment.NewLine +
	                           "Click OK to search, Cancel to delete the image.";
						MessageBoxResult result = MessageBox.Show(msg, "Image File Error", 
								                                  MessageBoxButton.OKCancel,
							                					  MessageBoxImage.Warning);
						if(result == MessageBoxResult.Cancel)
						{
							// does not want to replace file
                            ModifiedDuringLoad = true;
							ImageFileName = string.Empty;
							return;
						}
						else
						{
							// wants to replace file, so let him search for it
                            Microsoft.Win32.OpenFileDialog oFile = new Microsoft.Win32.OpenFileDialog();
	                        oFile.Filter = "Image Files (*.BMP, *.JPG, *.GIF, *.PNG. *.TIF, *.TIFF)" +
	                        	"|*.BMP;*.JPG;*.GIF;*.PNG;*.TIF;*.TIFF";
	                        oFile.CheckFileExists = true;
	                        oFile.Multiselect = false;
                            if (oFile.ShowDialog() == true)
                            {
                            	// new image file found
                            	ModifiedDuringLoad = true;
                            	string file = oFile.FileName;
								if(file.StartsWith(hamqslerFolder))
								{
									file = "$hamqslerFolder$\\" + 
										file.Substring(hamqslerFolder.Length);
								}
								else if(file.StartsWith(Environment.GetFolderPath(
									Environment.SpecialFolder.MyDocuments)))
								{
									file = "$MyDocs$\\" + fName.Substring(
										Environment.GetFolderPath(
										Environment.SpecialFolder.MyDocuments).Length);
								}
				                ImageFileName =file;
				                App.Logger.Log("Image file replaced by " + file);
                            }
                            else
                            {
                            	// image file not found
                            	ModifiedDuringLoad = true;
                            	ImageFileName = string.Empty;
                            	return;
                            }
						}
					}
					CalculateRectangle();
				}
				else
				{
					// reset Image
					Image = EmptyImage;
					if(QslCard != null)
					{
						ResetRectangle();
					}
				}
				if(QslCard != null)
				{
					QslCard.IsDirty = true;
				}
			}
			else if(e.Property == QslCardProperty)
			{
				if(Image == EmptyImage)
				{
					ResetRectangle();
				}
			}
		}
		
		/// <summary>
		/// Determine the relative location of the input coordinates within the image item 
		/// </summary>
		/// <param name="x">Card relative X coordinate</param>
		/// <param name="y">Card relative Y coordinate</param>
		/// <returns>Relative location</returns>
		public override CardWFItem.RelativeLocations GetRelativeLocation(int x, int y)
		{
			RelativeLocations location = RelativeLocations.Outside;
			if(NW.Contains(x, y))
			{
				location = CardWFItem.RelativeLocations.NW;
			}
			else if(NE.Contains(x, y))
			{
				location = RelativeLocations.NE;
			}
			else if(SW.Contains(x, y))
			{
				location = RelativeLocations.SW;
			}
			else if(SE.Contains(x, y))
			{
				location = RelativeLocations.SE;
			}
			else if(this.Contains(x, y))
			{
				location = RelativeLocations.Inside;
			}
			return location;
		}
		
		/// <summary>
		/// Copy the ImageWFBase properties
		/// </summary>
		/// <param name="image">ImageWFBase object whose properties are to be copied</param>
		public void CopyImageBaseProperties(ImageWFBase image)
		{
			CopyBaseProperties(image);
			ImageFileName = image.ImageFileName;
		}
			
	}
}
