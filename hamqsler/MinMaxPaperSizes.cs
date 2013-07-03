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
using System.Drawing.Printing;

namespace hamqsler
{
	/// <summary>
	///
	/// MinMaxPaperSizes class - gets the minimum and maximum width and height of all standard
	/// papers for the default printer. These values are used in CustomPaperSizesDialog.
	/// </summary>
	public class MinMaxPaperSizes
	{
		private int minWidth = 0;
		public float MinWidthInches
		{
			get {return (float)minWidth / 100F;}
		}
		public float MinWidthMM
		{
			get {return MinWidthInches * 25.4F;}
		}
		
		private int maxWidth = 0;
		public float MaxWidthInches
		{
			get {return (float)maxWidth / 100F;}
		}
		public float MaxWidthMM
		{
			get {return MaxWidthInches * 25.4F;}
		}
		
		private int minHeight = 0;
		public float MinHeightInches
		{
			get {return (float)minHeight / 100F;}
		}
		public float MinHeightMM
		{
			get {return MinHeightInches * 25.4F;}
		}
		
		private int maxHeight = 0;
		public float MaxHeightInches
		{
			get {return (float)maxHeight / 100F;}
		}
		public float MaxHeightMM
		{
			get {return MaxHeightInches * 25.4F;}
		}
		
		private string widthInches = string.Empty;
		public string WidthInches
		{
			get {return widthInches;}
		}
		
		private string heightInches = string.Empty;
		public string HeightInches
		{
			get {return heightInches;}
		}
		
		private string widthMM = string.Empty;
		public string WidthMM
		{
			get {return widthMM;}
		}
		
		private string heightMM = string.Empty;
		public string HeightMM
		{
			get {return heightMM;}
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public MinMaxPaperSizes()
		{
			GenerateMinMaxPaperSizes();
			GeneratePaperSizeLabels();
		}

		/// <summary>
		/// Get the minimum and maximum width and height of all standard papers.
		/// </summary>
		private void GenerateMinMaxPaperSizes()
		{
			// don't know how to get actual min and max sizes, so find min and max sizes from
			// standard paper sizes supported by default printer
			PrinterSettings settings = new PrinterSettings();
			foreach(PaperSize size in settings.PaperSizes)
			{
				if(minWidth == 0)
				{
					minWidth = size.Width;
					maxWidth = size.Width;
					minHeight = size.Height;
					maxHeight = size.Height;
				}
				else
				{
					if(minWidth > size.Width)
					{
						minWidth = size.Width;
					}
					if(maxWidth < size.Width)
					{
						maxWidth = size.Width;
					}
					if(minHeight > size.Height)
					{
						minHeight = size.Height;
					}
					if(maxHeight < size.Height)
					{
						maxHeight = size.Height;
					}
				}
			}
		}
		
		/// <summary>
		/// Generate strings based on minimum and maximum paper widths and heights. These
		/// values are displayed on the CustomPaperSizesDialog
		/// </summary>
		private void GeneratePaperSizeLabels()
		{
			widthInches = string.Format("({0:f2} - {1:f2})", MinWidthInches, MaxWidthInches);
			heightInches = string.Format("({0:f2} - {1:f2})", MinHeightInches, MaxHeightInches);
			widthMM = string.Format("({0:f2} - {1:f2})", MinWidthMM, MaxWidthMM);
			heightMM = string.Format("({0:f2} - {1:f2})", MinHeightMM, MaxHeightMM);
		}
	}
}
