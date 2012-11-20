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
		/// Copy Constructor
		/// </summary>
		/// <param name='parts'>
		/// TextParts object to copy
		/// </param>
		public TextParts(TextParts parts)
		{
			foreach(object o in parts)
			{
				switch(o.GetType().Name)
				{
				case "StaticText":
					StaticText st = CopyStaticText((StaticText)o);
					this.Add(st);
					break;
/*				case "AdifMacro":
					AdifMacro am = CopyAdifMacro((AdifMacro)o);
					this.Add(am);
					break;
				case "AdifExistsMacro":
					AdifExistsMacro aem = CopyAdifExistsMacro((AdifExistsMacro)o);
					this.Add(aem);
					break;
				case "CountMacro":
					CountMacro cm = CopyCountMacro((CountMacro)o);
					this.Add(cm);
					break;
				case "ManagerMacro":
					ManagerMacro mm = CopyManagerMacro((ManagerMacro)o);
					this.Add(mm);
					break;
				case "ManagerExistsMacro":
					ManagerExistsMacro me = CopyManagerExistsMacro((ManagerExistsMacro)o);
					this.Add(me);
					break;*/
				}
			}
		}
		
		/// <summary>
		/// Helper method that makes a copy of a StaticText object
		/// </summary>
		/// <returns>
		/// Copy of the StaticText object
		/// </returns>
		/// <param name='text'>
		/// StaticText object to copy
		/// </param>
		private StaticText CopyStaticText(StaticText text)
		{
			StaticText st = new StaticText();
			st.Text = text.Text;
			return st;
		}
		
/*		/// <summary>
		/// Helper method that creates a copy of an AdifMacro object.
		/// Note the use of recursion.
		/// </summary>
		/// <returns>
		/// Copy of the AdifMacro object
		/// </returns>
		/// <param name='aMac'>
		/// The AdifMacro object to copy
		/// </param>
		private AdifMacro CopyAdifMacro(AdifMacro aMac)
		{
			AdifMacro macro = new AdifMacro();
			macro.AdifField = CopyStaticText(aMac.AdifField);
			macro.DesignText = new TextParts(aMac.DesignText);
			macro.NoFieldText = new TextParts(aMac.NoFieldText);
			return macro;
		}
		
		/// <summary>
		/// Helper method that creates a copy of an AdifExistsMacro object.
		/// Not the use of recursion.
		/// </summary>
		/// <returns>
		/// Copy of the AdifExistsMacro object
		/// </returns>
		/// <param name='aMac'>
		/// The AdifExistsMacro object to copy
		/// </param>
		private AdifExistsMacro CopyAdifExistsMacro(AdifExistsMacro aMac)
		{
			AdifExistsMacro macro = new AdifExistsMacro();
			macro.AdifField = CopyStaticText(aMac.AdifField);
			macro.ExistsText = new TextParts(aMac.ExistsText);
			macro.DoesntExistText = new TextParts(aMac.DoesntExistText);
			macro.DesignText = new TextParts(aMac.DesignText);
			return macro;
		}
		
		/// <summary>
		/// Helper method that creates a copy of a CountMacro object.
		/// Note the use of recursion.
		/// </summary>
		/// <returns>
		/// Copy of the CountMacro object.
		/// </returns>
		/// <param name='cMac'>
		/// CountMacro object to copy.
		/// </param>
		private CountMacro CopyCountMacro(CountMacro cMac)
		{
			CountMacro macro = new CountMacro();
			macro.Condition = cMac.Condition;
			macro.Count = cMac.Count;
			macro.DesignText = new TextParts(cMac.DesignText);
			macro.TrueText = new TextParts(cMac.TrueText);
			macro.FalseText = new TextParts(cMac.FalseText);
			return macro;
		}
		
		/// <summary>
		/// Helper method that creates a copy of a ManagerMacro object.
		/// Note the use of recursion.
		/// </summary>
		/// <returns>
		/// Copy of the ManagerMacro object.
		/// </returns>
		/// <param name='mMac'>
		/// ManagerMacro object to copy.
		/// </param>
		private ManagerMacro CopyManagerMacro(ManagerMacro mMac)
		{
			ManagerMacro macro = new ManagerMacro();
			macro.DesignText = new TextParts(mMac.DesignText);
			macro.NoManagerText = new TextParts(mMac.NoManagerText);
			return macro;
		}
		
		/// <summary>
		/// Helper method that creates a copy of a ManagerExistsMacro object.
		/// Note the use of recursion.
		/// </summary>
		/// <returns>
		/// Copy of the ManagerExistsMacro object.
		/// </returns>
		/// <param name='mMac'>
		/// ManagerExistsMacro object to copy.
		/// </param>
		private ManagerExistsMacro CopyManagerExistsMacro(ManagerExistsMacro mMac)
		{
			ManagerExistsMacro macro = new ManagerExistsMacro();
			macro.DesignText = new TextParts(mMac.DesignText);
			macro.ExistsText = new TextParts(mMac.ExistsText);
			macro.DoesntExistText = new TextParts(mMac.DoesntExistText);
			return macro;
		}*/
		
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
	}
}
