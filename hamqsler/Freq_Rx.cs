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
	/// Freq_Rx class - In a split frequency QSO, the logging station's receiving frequency in MHz
	/// </summary>
	public class Freq_Rx : Freq
	{
		/// <summary>
		/// Constructor.
		/// Note: no validation of input is performed in the constructor. Call Validate after
		/// the constructor and when changing values.
		/// </summary>
		/// <param name="freq">frequency</param>
		/// <param name="aEnums">AdifEnumerations object containing the Band enumeration</param>
		public Freq_Rx(string freq, AdifEnumerations aEnums) : base(freq, aEnums)
		{
		}

		/// <summary>
		/// Check value for this field and modify it or other fields in QSO if required
		/// </summary>
		/// <param name="qso">Qso2 object containing this field</param>
		/// <returns>string indicating changes made, or null if no changes</returns>
		public override string ModifyValues(Qso2 qso)
		{
			string mods = null;
			string b = qso["band_rx"];
			string bandFromFreq = string.Empty;
			adifEnums.GetBandFromFrequency(Value, out bandFromFreq);
			if(b != null)
			{
				if(!b.Equals(bandFromFreq))
				{
					qso["band_rx"] = bandFromFreq;
					mods = "\tHam band in Band_Rx field does not match band for given Freq_Rx." +
						" Band_Rx field modified to match the frequency.";
				}
			}
			else
			{
//				string band = string.Empty;
//				adifEnums.GetBandFromFrequency(Value, out band);
				Band_Rx fBand = new Band_Rx(bandFromFreq, adifEnums);
				qso.Fields.Add(fBand);
				mods = "\tFreq_Rx specified, but Band_Rx is not. Band_Rx field generated.";
			}
			return mods;
		}
	}
}
