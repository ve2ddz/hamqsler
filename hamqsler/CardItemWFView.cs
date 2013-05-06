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
using System.Windows.Forms;
using System.Windows;

namespace hamqsler
{
	/// <summary>
	/// CardItemWFView class - base class for all CardWFItem view classes.
	/// </summary>
	public abstract class CardItemWFView : Control
	{
		// Reference to the CardItem that this view displays
		private CardWFItem cardItem = null;
		public CardWFItem CardItem
		{
			get {return cardItem;}
			set {cardItem = value;}
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		public CardItemWFView()
		{
		}
	}
}
