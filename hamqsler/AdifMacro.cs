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
using System.Collections.Generic;
using System.Windows;

namespace hamqsler
{
	/// <summary>
	/// AdifMacro class - Data and processing related to ADIF fields
	/// </summary>
	[Serializable]
	public class AdifMacro : TextPart
	{
		private static readonly DependencyProperty SeparateCardsByFieldProperty =
			DependencyProperty.Register("SeparateCardsByField", typeof(bool), typeof(AdifMacro),
			                            new PropertyMetadata(true));
		public bool SeparateCardsByField
		{
			get {return (bool)GetValue(SeparateCardsByFieldProperty);}
			set {SetValue(SeparateCardsByFieldProperty, value);}
		}
		
		private static readonly DependencyProperty AdifFieldProperty =
			DependencyProperty.Register("AdifField", typeof(StaticText), typeof(AdifMacro),
			                            new PropertyMetadata(null));
		public StaticText AdifField
		{
			get {return (StaticText)GetValue(AdifFieldProperty);}
			set {SetValue(AdifFieldProperty, value);}
		}
			
		private static readonly DependencyPropertyKey DesignTextPropertyKey =
			DependencyProperty.RegisterReadOnly("DesignText", typeof(TextParts), typeof(AdifMacro),
			                                    new PropertyMetadata(null));
		private static readonly DependencyProperty DesignTextProperty =
			DesignTextPropertyKey.DependencyProperty;
		public TextParts DesignText
		{
			get {return (TextParts)GetValue(DesignTextProperty);}
		}
		
		private static readonly DependencyPropertyKey NoFieldTextPropertyKey =
			DependencyProperty.RegisterReadOnly("NoFieldText", typeof(TextParts), typeof(AdifMacro),
			                                    new PropertyMetadata(null));
		private static readonly DependencyProperty NoFieldTextProperty =
			NoFieldTextPropertyKey.DependencyProperty;
		public TextParts NoFieldText
		{
			get {return (TextParts)GetValue(NoFieldTextProperty);}
		}
			
		/// <summary>
		/// Constructor
		/// </summary>
		public AdifMacro()
		{
			AdifField = new StaticText();
			SetValue(DesignTextPropertyKey, new TextParts());
			DesignText.Add(new StaticText());
			SetValue(NoFieldTextPropertyKey, new TextParts());
			NoFieldText.Add(new StaticText());
		}
	
		/// <summary>
		/// Creates an AdifMacroGroupBox object based on this AdifMacro
		/// </summary>
		/// <param name="parts">TextParts object that this AdifMacro is included in</param>
		/// <param name="includeContextMenu">Boolean to indicate whether the ContextMenu should be
		/// displayable or not. Use the values defined in MacroExpander.cs rather than true/false.</param>
		/// <returns></returns>
		public override MacroGroupBox BuildGroupBox(TextParts parts, bool includeContextMenu)
		{
			AdifMacroGroupBox box = new AdifMacroGroupBox(parts, this, includeContextMenu);
			return box;
		}
		
		/// <summary>
		/// Gets the ADIF fields for sorting QSOs
		/// </summary>
		/// <param name="fields">Adif fields from display macros</param>
		/// <param name="existFields">Adif fields from exists macros</param>
		public override void GetAdifFieldsForSorting(ref HashSet<string>fields, 
												 ref HashSet<string> existFields)
		{
//			if(SeparateCards;
		}
		
		/// <summary>
		/// Gets the text in this AdifMacro object.
		/// </summary>
		/// <returns>
		/// The text
		/// </returns>
		/// <param name="card">Qsl card that is being displayed/printed</param>
		/// <param name="qsos">Qsos to print on the card</param>
		/// <param name='screen'>
		/// Indicates whether in design mode (true) or in print mode (false)
		/// </param>
		public override string GetText(Card card, List<DispQso> qsos, bool screen)
		{
			if(screen)
			{
				return DesignText.GetText(card, qsos, screen);
			}
			else
			{
				throw new NotImplementedException();
			}
		}
		
		/// <summary>
		/// If there is more than one TextPart in DesignText or NoFieldText
		/// and the first TextPart is an empty StaticText, remove it.
		/// </summary>
		public override void RemoveExtraneousStaticTextMacros()
		{
			DesignText.RemoveExtraneousStaticText();
			NoFieldText.RemoveExtraneousStaticText();
		}
		
		/// <summary>
		/// Create a deep copy of this AdifMacro
		/// </summary>
		/// <returns>new AdifMacro that is a deep copy of this one</returns>
		public override TextPart Clone()
		{
			AdifMacro aMacro = new AdifMacro();
			aMacro.SeparateCardsByField = SeparateCardsByField;
			aMacro.AdifField = (StaticText)AdifField.Clone();
			aMacro.DesignText.Clear();
			foreach(TextPart part in DesignText)
			{
				aMacro.DesignText.Add(part.Clone());
			}
			aMacro.NoFieldText.Clear();
			foreach(TextPart part in NoFieldText)
			{
				aMacro.NoFieldText.Add(part.Clone());
			}
			return aMacro;
		}
	}
}
