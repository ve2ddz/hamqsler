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
using System.Collections;
using System.Xml;

namespace hamqsler
{
	/// <summary>
	/// A very simple class that helps simplify text and TextItem code
	/// </summary>
	[Serializable]
	public class TextParts : ArrayList
	{
		/// <summary>
		/// Default Constructor
		/// </summary>
		public TextParts () {}
		
		/// <summary>
		/// Get the text associated with these TextParts
		/// </summary>
		/// <returns>
		/// The text.
		/// </returns>
		/// <param name='screen'>
		/// Boolean indicating whether the card is being displayed on screen or printed.
		/// </param>
		public string GetText(bool screen)
		{
			string text = string.Empty;
			foreach (object o in this)
			{
				TextPart part = o as TextPart;
				if(part != null)
				{
					text += ((TextPart)part).GetText(screen);
				}
			}
			return text;
		}
		
		public void RemoveExtraneousStaticText()
		{
			if(this.Count > 1 && this[0].GetType() == typeof(StaticText)
			   && ((StaticText)this[0]).Text.Equals(string.Empty))
			{
				this.RemoveAt(0);
			}
		}
	}
}
