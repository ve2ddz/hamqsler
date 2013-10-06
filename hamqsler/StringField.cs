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
using System.Text.RegularExpressions;

namespace hamqsler
{
	/// <summary>
	/// Description of StringField.
	/// </summary>
	public class StringField : AdifField
	{
		/// <summary>
		/// Constructor.
		/// Note: no validation of input is performed in the constructor. Call Validate after
		/// the constructor and when changing values.
		/// </summary>
		/// <param name="value">string value</param>
		public StringField(string value)
		{
			Value = value;
		}

		/// <summary>
		/// Validate the field
		/// </summary>
		/// <returns>true if Value is not null</returns>
		/// <param name="err">Error message if Validate is false, or null</param>
		/// <param name="modStr">Message indicating what values were modified, or null</param>
		/// <returns>true if Value is not null and does not contain a newline character</returns>
		public override bool Validate(out string err, out string modStr)
		{
			err = null;
			modStr = null;
			if(Value == null)
			{
				err = "Value is null";
				return false;
			}
			if(Regex.IsMatch(Value, "[\\n\\r]"))
			{
				err = "String value contains a new line character. This is not allowed in StringField types";
				return false;
			}
			return true;
		}

		/// <summary>
		/// Create ADIF field string from contents of this element
		/// </summary>
		/// <returns>ADIF field string</returns>
		public virtual string ToAdifString()
		{
			return "<" + Name + ":" + Value.Length + ">" + Value;
		}
		
	}
}
