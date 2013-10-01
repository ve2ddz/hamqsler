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
	/// Fists class - The contacted station's FIST CW Club member information, which starts
	/// with a sequence of digits giving the member's number. For upward-compatibility, any
	/// characters after the last digit of the member number sequence must be allowed.
	/// </summary>
	public class Fists : StringField
	{
		/// <summary>
		/// Constructor
		/// Note: no validation of input is performed in the constructor. Call Validate after
		/// the constructor and when changing values.
		/// </summary>
		/// <param name="info">FISTS CW Club member information</param>
		public Fists(string info) : base(info)
		{
		}
		
		/// <summary>
		/// Validate member information
		/// </summary>
		/// <param name="err">Error message if info does not start with member number, null otherwise</param>
		/// <param name="modStr">Message if value has been modified (always null for this class)</param>
		/// <returns>true if info begins with number, false otherwise</returns>
		public override bool Validate(out string err, out string modStr)
		{
			err = null;
			modStr = null;
			if(!Regex.IsMatch(Value, @"^[0-9]+"))
			{
				err = "Fists must start with member's number.";
				return false;
			}
			if(Regex.IsMatch(Value, @"^[0-9]+[\\.]"))
			{
				err = "Fists number must be digits only.";
				return false;
			}
			return true;
		}
	}
}
