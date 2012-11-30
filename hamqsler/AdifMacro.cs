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
		/// Creates an AdifMacroExpander object based on this AdifMacro
		/// </summary>
		/// <param name="parts">TextParts object that this AdifMacro is included in</param>
		/// <param name="includeContextMenu"></param>
		/// <returns></returns>
		public override MacroExpander BuildExpander(TextParts parts, bool includeContextMenu)
		{
			AdifMacroExpander expander = new AdifMacroExpander(parts, this, includeContextMenu);
			expander.IsExpanded = true;
			return expander;
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
		/// <param name='screen'>
		/// Indicates whether in design mode (true) or in print mode (false)
		/// </param>
		public override string GetText(bool screen)
		{
			if(screen)
			{
				return DesignText.GetText(screen);
			}
			else
			{
				throw new NotImplementedException();
			}
		}
	}
}
