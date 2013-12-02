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
	/// Description of Pfx.
	/// </summary>
	public class Pfx : StringField
	{
		/// <summary>
		/// Constructor
		/// Note: no validation of input is performed in the constructor. Call Validate after
		/// the constructor and when changing values.
		/// </summary>
		/// <param name="prefix">Prefix value</param>
		public Pfx(string prefix) : base(prefix)
		{
		}
		
		/// <summary>
		/// Validate the prefix
		/// </summary>
		/// <param name="err">Error message if prefix is not valid, null otherwise</param>
		/// <param name="modStr">Message if value has been modified (always null for this class)</param>
		/// <returns>true if prefix is valid, false otherwise</returns>
		public override bool Validate(out string err, out string modStr)
		{
			err = null;
			modStr = null;
			if(!Regex.IsMatch(Value, "^[0-9][A-Za-z][0-9]*$|^[A-Za-z]{1,2}[0-9]+$"))
			{
				if(App.CallBureaus.IsPrefix(Value))
				{
					return true;
				}
				if(App.CallBureaus.IsCallAndPrefix(Value))
				{
					return true;
				}
				err = string.Format("\t'{0}' is not a valid prefix.", Value);
				return false;
			}
			return true;
		}
	}
}
