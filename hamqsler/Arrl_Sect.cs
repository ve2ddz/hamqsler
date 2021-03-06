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
	/// ArrlSect class - the contacted station's ARRL section
	/// </summary>
	public class Arrl_Sect : EnumerationValue
	{
		/// <summary>
		/// Constructor
		/// Note: no validation of input is performed in the constructor. Call Validate after
		/// the constructor and when changing values.
		/// </summary>
		/// <param name="value">Arrl section</param>
		/// <param name="aEnums">AdifEnumerations object containing the Arrl_Section enumeration</param>
		public Arrl_Sect(string value, AdifEnumerations aEnums) : base(value, "Arrl_Section", aEnums)
		{
		}
		
		/// <summary>
		/// Modify value for any section that is deprecated or deleted.
		/// </summary>
		/// <param name="qso">Qso2 object containing this field</param>
		/// <returns>string containing modifications made</returns>
		public override string ModifyValues(Qso2 qso)
		{
			string mod = null;
			if(aEnums.IsDeprecated("Arrl_Section", Value))
			{
				string value = Value;
				Value = aEnums.GetReplacementValue("Arrl_Section", value);
				mod = "\tArrl_Sect:" + Environment.NewLine +
					"\t\tDeprecated section 'NWT' changed to 'NT'." +
					Environment.NewLine;
			}
			return mod;
		}
	}
}
