/*
 *  Author:
 *       Jim Orcheson <jimorcheson@gmail.com>
 * 
 *  Copyright (c) 2012, 2013 Jim Orcheson
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
	/// AdifExistsMacro class - Data and processing related to existence of an ADIF field
	/// </summary>
	[Serializable]
	public class AdifExistsMacro : TextPart
	{
		private static readonly DependencyProperty SeparateCardsByFieldProperty =
			DependencyProperty.Register("SeparateCardsByField", typeof(bool), typeof(AdifExistsMacro),
			                            new PropertyMetadata(true));
		public bool SeparateCardsByField
		{
			get {return (bool)GetValue(SeparateCardsByFieldProperty);}
			set {SetValue(SeparateCardsByFieldProperty, value);}
		}
			
		private static readonly DependencyProperty AdifFieldProperty =
			DependencyProperty.Register("AdifField", typeof(StaticText), typeof(AdifExistsMacro),
			                            new PropertyMetadata(null));
		public StaticText AdifField
		{
			get {return (StaticText)GetValue(AdifFieldProperty);}
			set {SetValue(AdifFieldProperty, value);}
		}
		
		private static readonly DependencyPropertyKey DesignTextPropertyKey =
			DependencyProperty.RegisterReadOnly("DesignText", typeof(TextParts), typeof(AdifExistsMacro),
			                            new PropertyMetadata(null));
		private static readonly DependencyProperty DesignTextProperty = 
			DesignTextPropertyKey.DependencyProperty;
		public TextParts DesignText
		{
			get {return (TextParts)GetValue(DesignTextProperty);}
		}
		
		private static readonly DependencyPropertyKey ExistsTextPropertyKey =
			DependencyProperty.RegisterReadOnly("ExistsText", typeof(TextParts), typeof(AdifExistsMacro),
			                                    new PropertyMetadata(null));
		private static readonly DependencyProperty ExistsTextProperty =
			ExistsTextPropertyKey.DependencyProperty;
		public TextParts ExistsText
		{
			get {return (TextParts)GetValue(ExistsTextProperty);}
		}

		private static readonly DependencyPropertyKey DoesntExistTextPropertyKey =
			DependencyProperty.RegisterReadOnly("DoesntExist", typeof(TextParts), typeof(AdifExistsMacro),
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
		public AdifExistsMacro()
		{
			AdifField = new StaticText();
			SetValue(DesignTextPropertyKey, new TextParts());
			DesignText.Add(new StaticText());
			SetValue(ExistsTextPropertyKey, new TextParts());
			ExistsText.Add(new StaticText());
			SetValue(DoesntExistTextPropertyKey, new TextParts());
			DoesntExistText.Add(new StaticText());
		}
	
			/// <summary>
		/// Creates an AdifExistsMacroGroupBox object based on this AdifExistsMacro
		/// </summary>
		/// <param name="parts">TextParts object that this AdifExistsMacro is included in</param>
		/// <param name="includeContextMenu">Boolean to indicate whether the ContextMenu should be
		/// displayable or not. Use the values defined in MacroExpander.cs rather than true/false.</param>
		/// <returns></returns>
		public override MacroGroupBox BuildGroupBox(TextParts parts, bool includeContextMenu)
		{
			AdifExistsMacroGroupBox box = new AdifExistsMacroGroupBox(parts, this, includeContextMenu);
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
			if(SeparateCardsByField)
			{
				existFields.Add(AdifField.Text);
			}
			foreach(TextPart part in ExistsText)
			{
				part.GetAdifFieldsForSorting(ref fields, ref existFields);
			}
			foreach(TextPart part in DoesntExistText)
			{
				part.GetAdifFieldsForSorting(ref fields, ref existFields);
			}
		}
		
		/// <summary>
		/// Gets the text in this AdifExistsMacro object.
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
			else if(qsos != null && qsos.Count != 0)
			{
				string val = qsos[0].Qso.getValue(AdifField.GetText(
					card, qsos, screen), null);
				if(val != null && val != string.Empty)
				{
					return ExistsText.GetText(card, qsos, screen);
				}
				else
				{
					return DoesntExistText.GetText(card, qsos, screen);
				}
			}
			else
			{
				return DoesntExistText.GetText(card, qsos, screen);
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
		/// Create a deep copy of this AdifExistsMacro
		/// </summary>
		/// <returns>new AdifExistsMacro that is a deep copy of this</returns>
		public override TextPart Clone()
		{
			AdifExistsMacro aeMacro = new AdifExistsMacro();
			aeMacro.SeparateCardsByField = SeparateCardsByField;
			aeMacro.AdifField = (StaticText)AdifField.Clone();
			aeMacro.DesignText.Clear();
			foreach(TextPart part in DesignText)
			{
				aeMacro.DesignText.Add(part.Clone());
			}
			aeMacro.ExistsText.Clear();
			foreach(TextPart part in ExistsText)
			{
				aeMacro.ExistsText.Add(part.Clone());
			}
			aeMacro.DoesntExistText.Clear();
			foreach(TextPart part in DoesntExistText)
			{
				aeMacro.DoesntExistText.Add(part.Clone());
			}
			return aeMacro;
		}
	}

}
