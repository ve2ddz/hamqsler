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
	/// QSO Field of type enumeration
	/// </summary>
	public class EnumerationField : AdifField
	{
		private string[] enumeration = null;
		public string [] Enumeration
		{
			get {return enumeration;}
			set {enumeration = value;}
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="value">Field value</param>
		/// <param name="enums">Enumeration values</param>
		public EnumerationField(string value, string [] enums) : base(value)
		{
			Enumeration = enums;
		}
		
		/// <summary>
		/// Validate that value is within the enumeration
		/// </summary>
		/// <param name="err">Error message if value not within enumeration</param>
		/// <returns>true if value within enumeration, false otherwise</returns>
		public override bool Validate(out string err)
		{
			err = null;
			foreach(string enumer in Enumeration)
			{
				if(Value.Equals(enumer))
				{
					return true;
				}
			}
			err = "This QSO Field is of type enumeration. The value was not found in enumeration";
			return false;
		}
	}
}
