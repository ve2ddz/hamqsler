/*
 *  Author:
 *       Jim Orcheson <jimorcheson@gmail.com>
 * 
 *  Copyright (c) 2012, 2013 Jim Orcheson
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
using System.Collections.Generic;
using System.Globalization;

namespace hamqsler
{
	/// <summary>
	/// BandComparer compares two bands specified as strings (e.g. 160m and 33cm)
	/// </summary>
	public class BandComparer : Comparer<string>
	{
		public override int Compare(string band1, string band2)
		{
			string lower1 = null;
			string upper1 = null;
			string lower2 = null;
			string upper2 = null;
			App.AdifEnums.GetBandLimits(band1, out lower1, out upper1);
			App.AdifEnums.GetBandLimits(band2, out lower2, out upper2);
			float f1 = float.Parse(lower1, CultureInfo.InvariantCulture);
			float f2 = float.Parse(lower2, CultureInfo.InvariantCulture);
			return f1.CompareTo(f2);
		}
	}
}
