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
	/// Description of Mode.
	/// </summary>
	public class Mode : EnumerationValue
	{
		/// <summary>
		/// Constructor.
		/// Note: no validation of input is performed in the constructor. Call Validate after
		/// the constructor and when changing values.
		/// </summary>
		/// <param name="mode">mode</param>
		/// <param name="aEnums">AdifEnumerations object containing the Mode enumeration</param>
		public Mode(string mode, AdifEnumerations aEnums) : base(mode, "Mode", aEnums)
		{
		}
		
		/// <summary>
		/// Validate this field. Note: only test for null. Must (for ADIF files < ADIF 3.0.4).
		/// </summary>
		/// <param name="err">Error message if fails validation, null otherwise</param>
		/// <param name="modString">Message indicating what values were modified - returns null</param>
		/// <returns>False if value is null, true otherwise.</returns>
		public override bool Validate(out string err, out string modStr)
		{
			err = null;
			modStr = null;
			if(Value == null)
			{
				err = "Value is null.";
				return false;
			}
			return true;
		}
		
		/// <summary>
		/// Check value for this field and modify it or other fields in QSO if required
		/// </summary>
		/// <param name="qso">Qso2 object containing this field</param>
		/// <returns>string indicating changes made, or null if no changes</returns>
	public override string ModifyValues(Qso2 qso)
		{
			string mod = null;
			string initMode = Value;
			string submode = qso["Submode", string.Empty];
			
			if(!aEnums.IsInEnumeration("Mode", Value))
			{
				if(submode.Equals(string.Empty))
				{
					qso["Submode"] = Value;
					qso.Fields.Remove(this);
					mod = "\tMode not found in Mode enumeration. Submode set to mode value and mode cleared.";
				}
				else
				{
					qso["Mode"] = aEnums.GetModeFromSubmode(submode);
					mod = "\tMode not found in Mode enumeration. Mode set to mode for submode.";
				}
			}
			else
			{
				if(!submode.Equals(string.Empty))
				{
					string mode = aEnums.GetModeFromSubmode(submode);
					if(mode.Equals(string.Empty))
					{
						mod = "\tSubmode is not defined for the Mode value. Both values are retained.";
					}
					else if(!mode.Equals(initMode))
					{
						qso["Mode"] = mode;
						mod = "\tMode - submode mismatch. Mode set to proper mode for submode.";
					}
				}
			}
			return mod;
		}
	}
}
