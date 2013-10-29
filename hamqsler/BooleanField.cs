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
	/// BooleanField class - base class for QSO fields of type Boolean
	/// </summary>
	public class BooleanField : StringField
	{
		/// <summary>
		/// Constructor
		/// Note: no validation of input is performed in the constructor. Call Validate after
		/// the constructor and when changing values.
		/// </summary>
		/// <param name="value">'Y' for true, 'N' for false</param>
		public BooleanField(string value) : base(value)
		{
		}
		
		/// <summary>
		/// Validate the boolean value
		/// </summary>
		/// <param name="err">Message if value is not valid</param>
		/// <param name="modStr">Message if value has been modified (always null for this class)</param>
		/// <returns>true if value is valud, false otherwise</returns>
		public override bool Validate(out string err, out string modStr)
		{
			err = null;
			modStr = null;
			switch(Value)
			{
				case "Y":
				case "N":
					return true;
				default:
					err = "\tBoolean field must have value 'Y' or 'N'.";
					return false;
			}
		}
	}
}
