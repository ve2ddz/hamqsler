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
using System.Xml.Linq;

namespace hamqsler
{
	/// <summary>
	/// Country class - the contacted station's DXCC entity name
	/// </summary>
	public class Country : StringField
	{
		private AdifEnumerations adifEnums;
		
		/// <summary>
		/// Constructor.
		/// Note: no validation of input is performed in the constructor. Call Validate after
		/// the constructor and when changing values.
		/// </summary>
		/// <param name="value">DXCC entity name</param>
		/// <param name="aEnums">AdifEnumeration object containing the Country_Code enumeration</param>
		public Country(string value, AdifEnumerations aEnums) : base(value.ToUpper())
		{
			adifEnums = aEnums;
		}
		
		/// <summary>
		/// Validate the country name
		/// </summary>
		/// <param name="err">Error message if country name is invalid, null if country name is valid</param>
		/// <param name="modStr">Message if value has been modified (always null for this class)</param>
		/// <returns>true if country name is valid, false otherwise</returns>
		public override bool Validate(out string err, out string modStr)
		{
			err = null;
			modStr = null;
			string countryCode = string.Empty;
			if(!adifEnums.GetCountryCodeFromName(Value, out countryCode))
			{
				err = string.Format("\t'{0}' is not a valid country", Value);
				return false;
			}
			return true;
		}
	}
}
