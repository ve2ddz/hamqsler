﻿/*
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
	/// DXCC class - the contacted station's Country Code
	/// </summary>
	public class DXCC : EnumerationValue
	{
		/// <summary>
		/// Constructor
		/// Note: no validation of input is performed in the constructor. Call Validate after
		/// the constructor and when changing values.
		/// </summary>
		/// <param name="countryCode">DXCC country code</param>
		/// <param name="aEnums">AdifEnumerations object containing the Country_Code enumeration</param>
		public DXCC(string countryCode, AdifEnumerations aEnums) 
			: base(countryCode, "Country_Code", aEnums)
		{
		}
		
		/// <summary>
		/// Get country name for this DXCC
		/// </summary>
		/// <param name="err">Error message if name not found</param>
		/// <returns>string containing the country name, or null</returns>
		public string GetCountryName(out string err)
		{
			err = null;
			if(aEnums.IsInEnumeration("Country_Code", Value))
			{
				string name = aEnums.GetDescription("Country_Code", Value);
				return name;
			}
			else
			{
				err = string.Format("\tCountry code '{0}' is not a valid code. Country name cannot be " +
				                    "retrieved.", Value);
				return null;
			}
		}
	}
}
