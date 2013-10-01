/*
 *  Author:
 *       Jim Orcheson <jimorcheson@gmail.com>
 * 
 *  Copyright (c) 2013 Jim Orcheson
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

namespace hamqsler
{
	/// <summary>
	/// Adif Field of type EnumerationField with a single string value.
	/// </summary>
	public class EnumerationValue : EnumerationField
	{
		private string eltValue = string.Empty;
		public string Value
		{
			get {return eltValue;}
			set {eltValue = value;}
		}
		
		/// <summary>
		/// Constructor.
		/// Call Validate after constructor because no validation is done in constructor.
		/// </summary>
		/// <param name="value">Field value</param>
		/// <param name="enums">Enumeration values</param>
		public EnumerationValue(string value, string[] enums) : base(enums)
		{
			Value = value;
		}
		
		/// <summary>
		/// Constructor.
		/// Call Validate after constructor because no validation is done in constructor.
		/// </summary>
		/// <param name="value">Field value</param>
		/// <param name="enumeration">Name of Adif Enumeration</param>
		/// <param name="aEnum">AdifEnumerations object</param>
		public EnumerationValue(string value, string enumeration, AdifEnumerations aEnum) :
			base(enumeration, aEnum)
		{
			Value = value;
		}
		
		/// <summary>
		/// Validate that the value is within the enumeration
		/// </summary>
		/// <param name="err">Error message if Validate is false, or null</param>
		/// <param name="modStr">Message indicating what values were modified, or null</param>
		/// <returns>true is Value is valid, false otherwise.</returns>
		public override bool Validate(out string err, out string modStr)
		{
			err = null;
			modStr = null;
			if(Value == null)
			{
				err = "Value is null.";
				return false;
			}
			return base.Validate(Value, out err);
		}
		
		/// <summary>
		/// Get ADIF string for this field
		/// </summary>
		/// <returns>string in ADIF format</returns>
		public virtual string ToAdifString()
		{
			return "<" + Name + ":" + Value.Length + ">" + Value;
		}
	}
}
