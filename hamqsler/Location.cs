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
using System.Globalization;
using System.Text.RegularExpressions;

namespace hamqsler
{
	/// <summary>
	/// Location class - a sequence of characters representing a latitude or longitude in
	/// XDDD MM.MMM format, where X is a directional character from the set {E, W, N, S}
	/// DDD is a 3 digit degrees specifier (0 <= DDD <= 180), and
	/// MM.MMM is a 6 digit minutes specifier (0 <= MM.MMM <= 59.999)
	/// </summary>
	public class Location: StringField
	{
		/// <summary>
		/// Constructor
		/// Note: no validation of input is performed in the constructor. Call Validate after
		/// the constructor and when changing values.
		/// </summary>
		/// <param name="location">location specifier</param>
		public Location(string location) : base(location)
		{
		}
		
		/// <summary>
		/// Validate that location is in correct format
		/// </summary>
		/// <param name="err">Error message if location not in correct format, null otherwise</param>
		/// <param name="modStr">Message if value has been modified (always null for this class)</param>
		/// <returns>true if correct format, false otherwise</returns>
		public override bool Validate(out string err, out string modStr)
		{
			err = null;
			modStr = null;
			if(!Regex.IsMatch(Value, @"^[EWNS][0-9]{3} [0-9]{2}\.[0-9]{3}$"))
			{
				err = string.Format("'{0}' is not a valid location.", Value);
				return false;
			}
			string deg = Value.Substring(1, 3);
			int degrees = 0;
			if(!Int32.TryParse(deg, NumberStyles.None, CultureInfo.InvariantCulture, out degrees))
			{
				err = string.Format("'{0}' is not a valid location.", Value);
				return false;				
			}
			if(degrees > 180)
			{
				err = string.Format("'{0}' is not a valid location.", Value);
				return false;				
			}
			string mins = Value.Substring(5, 6);
			float minutes = 0F;
			if(!float.TryParse(mins, NumberStyles.Any, CultureInfo.InvariantCulture,
			                  out minutes))
			{
				err = string.Format("'{0}' is not a valid location.", Value);
				return false;
			}
			if(minutes >= 60F)
			{
				err = string.Format("'{0}' is not a valid location.", Value);
				return false;
			}
			if(degrees == 180 && minutes != 0F)
			{
				err = string.Format("'{0}' is not a valid location.", Value);
				return false;
			}
			return true;
		}
	}
}
