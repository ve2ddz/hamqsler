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
	/// Sota_Ref class - the contacted station's International SOTA Reference
	/// </summary>
	public class Sota_Ref : StringField
	{
		/// <summary>
		/// Constructor.
		/// Note: no validation of input is performed in the constructor. Call Validate after
		/// the constructor and when changing values.
		/// </summary>
		/// <param name="sota">SOTA reference</param>
		public Sota_Ref(string sota) : base(sota)
		{
		}
		
		/// <summary>
		/// Validate the SOTA Reference value.
		/// Value is checked for format, not actual value
		/// </summary>
		/// <param name="err">Error message if value is not valid, null otherwise</param>
		/// <param name="modStr">Message if value has been modified (always null for this class)</param>
		/// <returns>true if format is valid, false otherwise</returns>
		public override bool Validate(out string err, out string modStr)
		{
			err = null;
			modStr = null;
			if(!Regex.IsMatch(Value, "^[0-9]{0,1}[A-Za-z]{1,2}[0-9]{0,1}/[A-Za-z]{2}-[0-9]{3}$"))
			{
				err = string.Format("\t'{0}' is not a valid SOTA Reference.", Value);
				return false;
			}
			return true;
		}
	}
}
