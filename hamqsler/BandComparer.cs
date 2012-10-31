﻿/*
 *  Author:
 *       Jim Orcheson <jimorcheson@gmail.com>
 * 
 *  Copyright (c) 2012 Jim Orcheson
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
using Qsos;
using System;
using System.Collections.Generic;

namespace hamqsler
{
	/// <summary>
	/// BandComparer compares two bands specified as strings (e.g. 160m and 33cm)
	/// </summary>
	public class BandComparer : Comparer<string>
	{
		public override int Compare(string band1, string band2)
		{
			HamBand hb1 = HamBands.getHamBand(band1.ToLower());
			HamBand hb2 = HamBands.getHamBand(band2.ToLower());
			float f1 = hb1.LowerEdge;
			float f2 = hb2.LowerEdge;
			return f1.CompareTo(f2);
		}
	}
}