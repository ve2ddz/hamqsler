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
using System.Collections.Generic;

namespace hamqsler
{
	/// <summary>
	/// Equality comparer for two Qso2 objects
	/// </summary>
	public class QsoEqualityComparer : IEqualityComparer<Qso2>
	{
		public bool Equals(Qso2 q1, Qso2 q2)
		{
			return q1.Equals(q2);
		}
	
		/// <summary>
		/// get hash code for this Qso object
		/// </summary>
		/// <param name="qso">Qso2 object to generate hash code for</param>
		/// <returns>hash code for the Qso</returns>
		public int GetHashCode(Qso2 qso)
		{
			return qso.GetHashCode();
		}
	}
}
