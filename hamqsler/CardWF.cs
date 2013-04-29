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

namespace hamqsler
{
	/// <summary>
	/// CardWF describes the QslCard.
	/// </summary>
	public class CardWF
	{
		private Point location = new Point(0,0);
		public Point Location
		{
			get {return location;}
			set {location = value;}
		}
		
		public int X
		{
			get{return location.X;}
			set{location.X = value;}
		}
		
		public int Y
		{
			get {return location.Y;}
			set {location.Y = value;}
		}
		
		private Size cardSize = new Size(0,0);
		public Size CardSize
		{
			get {return cardSize;}
			set {cardSize = value;}
		}
		
		public int Width
		{
			get {return cardSize.Width;}
			set {cardSize.Width = value;}
		}
			
		public int Height
		{
			get {return cardSize.Height;}
			set {cardSize.Height = value;}
		}
		
		private bool isDirty = false;
		public bool IsDirty
		{
			get {return isDirty;}
			set {isDirty = value;}
		}
		
		private bool isInDesignMode = false;
		public bool IsInDesignMode
		{
			get {return isInDesignMode;}
			set {isInDesignMode = value;}
		}
		
		private string fileName = null;
		public string FileName
		{
			get {return fileName;}
			set {fileName = value;}
		}
		
		/// <summary>
		/// Default constructor
		/// </summary>
		public CardWF()
		{
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="width">Width of the card (100 units/inch)</param>
		/// <param name="height">Height of the card (100 units/inch)</param>
		/// <param name="isInDesignMode"></param>
		public CardWF(int width, int height, bool isInDesignMode)
		{
			IsInDesignMode = isInDesignMode;
			CardSize = new Size(width, height);
		}
	}
}
