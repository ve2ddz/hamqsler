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

namespace hamqsler
{
	/// <summary>
	/// EqualityComparer for QsoWithInclude objects
	/// </summary>
	public class QsoWithIncludeEqualityComparer : EqualityComparer<QsoWithInclude>
	{
		/// <summary>
		/// Tests for the equality of two QsoWithInclude objects
		/// </summary>
		/// <param name="q1">First QsoWithInclude object</param>
		/// <param name="q2">Second QsoWithInclude object</param>
		/// <returns>true if the QsoWithInclude objects are equal, false otherwise</returns>
		public override bool Equals(QsoWithInclude q1, QsoWithInclude q2)
		{
			return q1.BureauManagerCallDateTime == q2.BureauManagerCallDateTime &&
				q1.Mode == q2.Mode && q1.Submode == q2.Submode &&q1.Band == q2.Band &&
				q1.Frequency == q2.Frequency &&
				q1.RST == q2.RST;
		}
		
		/// <summary>
		/// Generates a hash code for the QsoWithInclude
		/// </summary>
		/// <param name="q">QsoWithInclude object to generate hash code for.</param>
		/// <returns></returns>
		public override int GetHashCode(QsoWithInclude q)
		{
			string hCode = q.Callsign + q.Manager + q.DateTime;
			return hCode.GetHashCode();
		}
	}
	
}
