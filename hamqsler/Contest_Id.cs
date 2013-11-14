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
	/// Contest_Id class - contest identifier. Use enumeration values for interoperability.
	/// </summary>
	public class Contest_Id : StringEnumerationValue
	{
		/// <summary>
		/// Constructor
		/// Note: no validation of input is performed in the constructor. Call Validate after
		/// the constructor and when changing values.
		/// </summary>
		/// <param name="id">contest id</param>
		/// <param name="aEnums">AdifEnumerations object containing the Contest_ID enumeration</param>
		public Contest_Id(string id, AdifEnumerations aEnums) 
			: base(id, "Contest_ID", aEnums)
		{
		}
		
		/// <summary>
		///  Change deprecated values to their replacements
		/// </summary>
		/// <param name="qso">Qso2 object containing this field</param>
		/// <returns>string containing message about changes made</returns>
		public override string ModifyValues(Qso2 qso)
		{
			string mod = null;
			if(aEnums.IsDeprecated("Contest_ID", Value))
			{
				string value = Value;
				Value = aEnums.GetReplacementValue("Contest_ID", value);
				mod = string.Format("\tContest_Id:" + Environment.NewLine +
					"\t\tDeprecated value '{0}' changed to '{1}'." +
					Environment.NewLine, value, Value);
			}
			return mod;
		}
	}
}
