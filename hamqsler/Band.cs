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

namespace hamqsler
{
	/// <summary>
	/// Band class - specifies the logging station's transmitting band
	/// </summary>
	public class Band : EnumerationValue
	{
		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="band">Band value</param>
		/// <param name="aEnums">AdifEnumerations object containing the Band enumeration</param>
		public Band(string band, AdifEnumerations aEnums) : base(band, "Band", aEnums)
		{
		}
		
		/// <summary>
		/// Checks if the input frequency is within this band
		/// </summary>
		/// <param name="freq">Frequency to check</param>
		/// <returns>true if within the band, false otherwise</returns>
		public bool IsWithinBand(string freq)
		{
			string lLimit = string.Empty;
			string uLimit = string.Empty;
			float f = float.Parse(freq, CultureInfo.InvariantCulture);
			if(aEnums.GetBandLimits(Value, out lLimit, out uLimit))
			{
				float lFreq = float.Parse(lLimit, CultureInfo.InvariantCulture);
				float uFreq = float.Parse(uLimit, CultureInfo.InvariantCulture);
				return f >= lFreq && f <= uFreq;
			}
			return false;
		}
	}
}
