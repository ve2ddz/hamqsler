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
	/// Freq class - QSO frequency in MHz
	/// </summary>
	public class Freq : NumberField
	{
		private AdifEnumerations adifEnums;
		
		/// <summary>
		/// Constructor
		/// Note: no validation of input is performed in the constructor. Call Validate after
		/// the constructor and when changing values.
		/// </summary>
		/// <param name="freq">Frequency value in MHz</param>
		/// <param name="aEnums">AdifEnumerations object containing Band enumeration</param>
		public Freq(string freq, AdifEnumerations aEnums) : base(freq)
		{
			adifEnums = aEnums;
		}
		
		
		/// <summary>
		/// Validate the frequency = must be within the limits of a defined band
		/// </summary>
		/// <param name="err">Error message if frequency not valid, or null</param>
		/// <param name="modStr">Message if value has been modified (always null for this class)</param>
		/// <returns>true if valid, false otherwise</returns>
		public override bool Validate(out string err, out string modStr)
		{
			err = null;
			modStr = null;
			if(!base.Validate(out err, out modStr))
			{
				return false;
			}
			string band = string.Empty;
			if(!adifEnums.GetBandFromFrequency(Value, out band))
			{
				err = string.Format("'{0}' is outside enumerated band limits.", Value);
				return false;
			}
			return true;
		}
	}
}
