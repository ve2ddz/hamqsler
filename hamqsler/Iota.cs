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
	/// Iota class - The contacted station's IOTA designator, in format CC-XXX where
	/// CC is a member of the Continent enumeration, and XXX is the island designator
	/// with 000 < = XXX <c= 999
	/// </summary>
	public class Iota : StringField
	{
		private AdifEnumerations adifEnums = null;
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="iota">IOTA designator</param>
		/// <param name="aEnums">AdifEnumerations object containing Continent enumeration</param>
		public Iota(string iota, AdifEnumerations aEnums) : base(iota)
		{
			adifEnums = aEnums;
		}
		
		/// <summary>
		/// Validate the value - must be valid IOTA designator
		/// </summary>
		/// <param name="err">Error message if invalid, null otherwise</param>
		/// <returns>true if designator is in valid format, false otherwise</returns>
		public override bool Validate(out string err)
		{
			err = null;
			if(!Regex.IsMatch(Value, "^[A-Z]{2}-[0-9]{3}$"))
			{
				err = string.Format("'{0}' is not a valid IOTA designator.", Value);
				return false;
			}
			string[] parts = Value.Split('-');
			if(!adifEnums.IsInEnumeration("Continent", parts[0]))
			{
				err = string.Format("'{0}' is not a valid IOTA designator.", Value);
				return false;
			}
			return true;
		}
	}
}
