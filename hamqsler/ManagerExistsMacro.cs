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
	/// Description of ManagerExistsMacro.
	/// </summary>
	[Serializable]
	public class ManagerExistsMacro : TextPart
	{
		private static readonly DependencyPropertyKey DesignTextPropertyKey =
			DependencyProperty.RegisterReadOnly("DesignText", typeof(TextParts), 
			                                    typeof(ManagerExistsMacro),
			                                    new PropertyMetadata(null));
		private static readonly DependencyProperty DesignTextProperty =
			DesignTextPropertyKey.DependencyProperty;
		public TextParts DesignText
		{
			get {return (TextParts)GetValue(DesignTextProperty);}
		}
		
		private static readonly DependencyPropertyKey ExistsTextPropertyKey =
			DependencyProperty.RegisterReadOnly("ExistsText", typeof(TextParts), 
			                                    typeof(ManagerExistsMacro),
			                                    new PropertyMetadata(null));
		private static readonly DependencyProperty ExistsTextProperty =
			ExistsTextPropertyKey.DependencyProperty;
		public TextParts ExistsText
		{
			get {return (TextParts)GetValue(ExistsTextProperty);}
		}
		
		private static readonly DependencyPropertyKey DoesntExistTextPropertyKey =
			DependencyProperty.RegisterReadOnly("DoesntExistText", typeof(TextParts), 
			                                    typeof(ManagerExistsMacro),
			                                    new PropertyMetadata(null));
		private static readonly DependencyProperty DoesntExistTextProperty =
			DoesntExistTextPropertyKey.DependencyProperty;
		public TextParts DoesntExistText
		{
			get {return (TextParts)GetValue(DoesntExistTextProperty);}
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		public ManagerExistsMacro()
		{
			SetValue(DesignTextPropertyKey, new TextParts());
			DesignText.Add(new StaticText());
			SetValue(ExistsTextPropertyKey, new TextParts());
			ExistsText.Add(new StaticText());
			SetValue(DoesntExistTextPropertyKey, new TextParts());
			DoesntExistText.Add(new StaticText());
		}
	
		/// <summary>
		/// Creates a ManagerExistsGroupBox object based on this ManagerExistsMacro
		/// </summary>
		/// <param name="parts">TextParts object that this ManagerExistsMacro is included in</param>
		/// <param name="includeContextMenu">Boolean to indicate whether the ContextMenu should be
		/// displayable or not. Use the values defined in MacroExpander.cs rather than true/false.</param>
		/// <returns></returns>
		public override MacroGroupBox BuildGroupBox(TextParts parts, bool includeContextMenu)
		{
			ManagerExistsMacroGroupBox box = new ManagerExistsMacroGroupBox(parts, this, 
			                                                                includeContextMenu);
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
		/// Gets the text in this ManagerExistsMacro object.
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
		/// If there is more than one TextPart in DesignText, ExistsText, or DoesntExistText
		/// and the first TextPart is an empty StaticText, remove it.
		/// </summary>
		public override void RemoveExtraneousStaticTextMacros()
		{
			DesignText.RemoveExtraneousStaticText();
			ExistsText.RemoveExtraneousStaticText();
			DoesntExistText.RemoveExtraneousStaticText();
		}
		
		/// <summary>
		/// Create a deep clone of this ManagerExistsMacro
		/// </summary>
		/// <returns>ManagerExistsMacro that is a deep copy of this one</returns>
		public override TextPart Clone()
		{
			ManagerExistsMacro meMacro = new ManagerExistsMacro();
			meMacro.DesignText.Clear();
			foreach(TextPart part in DesignText)
			{
				meMacro.DesignText.Add(part.Clone());
			}
			meMacro.ExistsText.Clear();
			foreach(TextPart part in ExistsText)
			{
				meMacro.ExistsText.Add(part.Clone());
			}
			meMacro.DoesntExistText.Clear();
			foreach(TextPart part in DoesntExistText)
			{
				meMacro.DoesntExistText.Add(part.Clone());
			}
			return meMacro;
		}
	}
}
