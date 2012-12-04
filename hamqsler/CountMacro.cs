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
	/// Description of CountMacro.
	/// </summary>
	[Serializable]
	public class CountMacro : TextPart
	{
		private static readonly DependencyProperty CountEqualsProperty =
			DependencyProperty.Register("CountEquals", typeof(bool), typeof(CountMacro),
			                            new PropertyMetadata(true));
		public bool CountEquals
		{
			get {return (bool)GetValue(CountEqualsProperty);}
			set {SetValue(CountEqualsProperty, value);}
		}
		

		private static readonly DependencyProperty CountLessThanProperty =
			DependencyProperty.Register("CountLessThan", typeof(bool), typeof(CountMacro),
			                            new PropertyMetadata(false));
		public bool CountLessThan
		{
			get {return (bool)GetValue(CountLessThanProperty);}
			set {SetValue(CountLessThanProperty, value);}
		}
		
		private static readonly DependencyProperty CountGreaterThanProperty =
			DependencyProperty.Register("CountGreaterThan", typeof(bool), typeof(CountMacro),
			                            new PropertyMetadata(false));
		public bool CountGreaterThan
		{
			get {return (bool)GetValue(CountGreaterThanProperty);}
			set {SetValue(CountGreaterThanProperty, value);}
		}
		
		private static readonly DependencyProperty CountProperty =
			DependencyProperty.Register("Count", typeof(uint), typeof(CountMacro),
			                            new PropertyMetadata((uint)1));
		public uint Count
		{
			get {return (uint)GetValue(CountProperty);}
			set {SetValue(CountProperty, value);}
		}
		
		private static readonly DependencyPropertyKey DesignTextPropertyKey =
			DependencyProperty.RegisterReadOnly("DesignText", typeof(TextParts), typeof(CountMacro),
			                                    new PropertyMetadata(null));
		private static readonly DependencyProperty DesignTextProperty =
			DesignTextPropertyKey.DependencyProperty;
		public TextParts DesignText
		{
			get {return (TextParts)GetValue(DesignTextProperty);}
		}
		
		private static readonly DependencyPropertyKey TrueTextPropertyKey =
			DependencyProperty.RegisterReadOnly("TrueText", typeof(TextParts), typeof(CountMacro),
			                                    new PropertyMetadata(null));
		private static readonly DependencyProperty TrueTextProperty =
			TrueTextPropertyKey.DependencyProperty;
		public TextParts TrueText
		{
			get {return (TextParts)GetValue(TrueTextProperty);}
		}
		
		private static readonly DependencyPropertyKey FalseTextPropertyKey =
			DependencyProperty.RegisterReadOnly("FalseText", typeof(TextParts), typeof(CountMacro),
			                                    new PropertyMetadata(null));
		private static readonly DependencyProperty FalseTextProperty =
			FalseTextPropertyKey.DependencyProperty;
		public TextParts FalseText
		{
			get {return (TextParts)GetValue(FalseTextProperty);}
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		public CountMacro()
		{
			SetValue(DesignTextPropertyKey, new TextParts());
			DesignText.Add(new StaticText());
			SetValue(TrueTextPropertyKey, new TextParts());
			TrueText.Add(new StaticText());
			SetValue(FalseTextPropertyKey, new TextParts());
			FalseText.Add(new StaticText());
		}
	
		/// <summary>
		/// Creates a CountMacroGroupBox object based on this CountMacro
		/// </summary>
		/// <param name="parts">TextParts object that this CountMacro is included in</param>
		/// <param name="includeContextMenu">Boolean to indicate whether the ContextMenu should be
		/// displayable or not. Use the values defined in MacroExpander.cs rather than true/false.</param>
		/// <returns></returns>
		public override MacroGroupBox BuildGroupBox(TextParts parts, bool includeContextMenu)
		{
			CountMacroGroupBox box = new CountMacroGroupBox(parts, this, includeContextMenu);
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
			throw new NotImplementedException();
		}
		
		/// <summary>
		/// Gets the text in this CountMacro object.
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
		
		/// <summary>
		/// If there is more than one TextPart in DesignText, TrueText, or FalseText
		/// and the first TextPart is an empty StaticText, remove it.
		/// </summary>
		public override void RemoveExtraneousStaticTextMacros()
		{
			DesignText.RemoveExtraneousStaticText();
			TrueText.RemoveExtraneousStaticText();
			FalseText.RemoveExtraneousStaticText();
		}
	}
}
