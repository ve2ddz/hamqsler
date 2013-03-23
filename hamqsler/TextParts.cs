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
//using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace hamqsler
{
	/// <summary>
	/// A very simple class that helps simplify text and TextItem code
	/// </summary>
	[Serializable]
	[XmlInclude(typeof(StaticText))]
	[XmlInclude(typeof(AdifMacro))]
	[XmlInclude(typeof(AdifExistsMacro))]
	[XmlInclude(typeof(CountMacro))]
	[XmlInclude(typeof(ManagerMacro))]
	[XmlInclude(typeof(ManagerExistsMacro))]
	public class TextParts : List<TextPart>
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
		/// <param name="card">Qsl card that is being displayed/printed</param>
		/// <param name="qsos">Qsos to print on the card</param>
		/// <param name='screen'>
		/// Boolean indicating whether the card is being displayed on screen or printed.
		/// </param>
		public string GetText(Card card, List<DispQso> qsos, bool screen)
		{
			string text = string.Empty;
			foreach (object o in this)
			{
				TextPart part = o as TextPart;
				if(part != null)
				{
					text += ((TextPart)part).GetText(card, qsos, screen);
				}
			}
			return text;
		}
		
		/// <summary>
		/// Remove empty StaticText items that are followed by another StaticText item.
		/// </summary>
		public void RemoveExtraneousStaticText()
		{
			TextParts parts = new TextParts();
			for(int partNum = 0; partNum < this.Count - 1; partNum++)
			{
				if(this[partNum].GetType() == typeof(StaticText) &&
				   this[partNum + 1].GetType() == typeof(StaticText))
			    {
					if(!((StaticText)this[partNum]).Text.Equals(string.Empty))
					{
						parts.Add(this[partNum]);
					}
			    }
				else
				{
					parts.Add(this[partNum]);
				}
			}
			parts.Add(this[this.Count - 1]);
			this.Clear();
			foreach(TextPart part in parts)
			{
				this.Add(part);
			}
		}
	}
}
