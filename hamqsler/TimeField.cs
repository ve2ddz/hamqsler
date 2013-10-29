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
	/// TimeField class - base class for ADIF time classes
	/// </summary>
	public class TimeField : StringField
	{
		/// <summary>
		/// Constructor
		/// Note: no validation of input is performed in the constructor. Call Validate after
		/// the constructor and when changing values.
		/// </summary>
		/// <param name="time">time in HHMM or HHMMSS format</param>
		public TimeField(string time) : base(time)
		{
		}
		
		/// <summary>
		/// Validate time field
		/// </summary>
		/// <param name="err">Error message if time is invalid, null otherwise</param>
		/// <param name="modStr">Message if value has been modified (always null for this class)</param>
		/// <returns>true if time is valid, false otherwise.</returns>
		public override bool Validate(out string err, out string modStr)
		{
			err = null;
			modStr = null;
			int length = Value.Length;
			if(length == 0)
			{
				return true;
			}
			if(!Regex.IsMatch(Value, "^[0-2][0-9][0-5][0-9]$|^[0-2][0-9][0-5][0-9][0-5][0-9]$"))
			{
				err = "\tTime must be in HHMM or HHMMSS format.";
				return false;
			}
			string hour = Value.Substring(0, 2);
			if(hour.CompareTo("24") >= 0)
			{
				err = "\tInvalid time.";
				return false;
			}
			return true;
		}
	}
}
