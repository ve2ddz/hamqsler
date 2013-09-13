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
	/// Description of StringEnumerationField.
	/// </summary>
	public class StringEnumerationValue : EnumerationValue
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="value">Field value</param>
		/// <param name="enums">Enumeration values</param>
		public StringEnumerationValue(string value, string[] enums) : base(value, enums)
		{
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="value">Field value</param>
		/// <param name="enumeration">Name of Adif Enumeration</param>
		/// <param name="aEnum">AdifEnumerations object</param>
		public StringEnumerationValue(string value, string enumeration, AdifEnumerations aEnums)
			: base(value, enumeration, aEnums)
		{
		}
		
		/// <summary>
		/// Validate the value.
		/// </summary>
		/// <param name="err">string containing error message if any</param>
		/// <returns>true always, because value does not have to be in enumeration</returns>
		public override bool Validate(out string err)
		{
			err = null;
			return true;
		}
		
		/// <summary>
		/// Test if value is in enumeration
		/// </summary>
		/// <param name="err">error message if not in enumeration</param>
		/// <returns>true if Value is in enumeration, false otherwise</returns>
		public bool IsInEnumeration(out string err)
		{
			return base.IsInEnumeration(Value, out err);
		}
	}
}
