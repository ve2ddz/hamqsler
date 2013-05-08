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
	/// Description of ManagerMacro.
	/// </summary>
	[Serializable]
	public class ManagerMacro : TextPart
	{
		private static readonly DependencyPropertyKey DesignTextPropertyKey =
			DependencyProperty.RegisterReadOnly("DesignText", typeof(TextParts), typeof(ManagerMacro),
			                                    new PropertyMetadata(null));
		private static readonly DependencyProperty DesignTextProperty =
			DesignTextPropertyKey.DependencyProperty;
		public TextParts DesignText
		{
			get {return (TextParts)GetValue(DesignTextProperty);}
		}
		
		private static readonly DependencyPropertyKey NoManagerTextPropertyKey =
			DependencyProperty.RegisterReadOnly("NoManagerText", typeof(TextParts), typeof(ManagerMacro),
			                                    new PropertyMetadata(null));
		private static readonly DependencyProperty NoManagerTextProperty =
			NoManagerTextPropertyKey.DependencyProperty;
		public TextParts NoManagerText
		{
			get {return (TextParts)GetValue(NoManagerTextProperty);}
		}
		
		public ManagerMacro()
		{
			SetValue(DesignTextPropertyKey, new TextParts());
			DesignText.Add(new StaticText());
			SetValue(NoManagerTextPropertyKey, new TextParts());
			NoManagerText.Add(new StaticText());
		}

		/// <summary>
		/// Creates a ManagerMacroGroupBox object based on this ManagerMacro
		/// </summary>
		/// <param name="parts">TextParts object that this ManagerMacro is included in</param>
		/// <param name="includeContextMenu">Boolean to indicate whether the ContextMenu should be
		/// displayable or not. Use the values defined in MacroExpander.cs rather than true/false.</param>
		/// <returns></returns>
		public override MacroGroupBox BuildGroupBox(TextParts parts, bool includeContextMenu)
		{
			ManagerMacroGroupBox box = new ManagerMacroGroupBox(parts, this, includeContextMenu);
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
		/// Gets the text in this ManagerMacro object.
		/// </summary>
		/// <returns>
		/// The text
		/// </returns>
		/// <param name="card">Qsl card that is being displayed/printed</param>
		/// <param name="qsos">Qsos to print on the card</param>
		/// <param name='screen'>
		/// Indicates whether in design mode (true) or in print mode (false)
		/// </param>
		public override string GetText(CardWF card, List<DispQso> qsos, bool screen)
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
		/// If there is more than one TextPart in DesignText or NoManagerText
		/// and the first TextPart is an empty StaticText, remove it.
		/// </summary>
		public override void RemoveExtraneousStaticTextMacros()
		{
			DesignText.RemoveExtraneousStaticText();
			NoManagerText.RemoveExtraneousStaticText();
		}
		
		/// <summary>
		/// Create a deep copy of this ManagerMacro
		/// </summary>
		/// <returns>ManagerMacro object that is a deep copy of this one</returns>
		public override TextPart Clone()
		{
			ManagerMacro mMacro = new ManagerMacro();
			mMacro.DesignText.Clear();
			foreach(TextPart part in DesignText)
			{
				mMacro.DesignText.Add(part.Clone());
			}
			mMacro.NoManagerText.Clear();
			foreach(TextPart part in NoManagerText)
			{
				mMacro.NoManagerText.Add(part.Clone());
			}
			return mMacro;
		}
	}
}
