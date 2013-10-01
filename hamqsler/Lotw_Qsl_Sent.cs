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
	/// Lotw_Qsl_Sent class - ARRL Logbook of the World QSL sent status
	/// </summary>
	public class Lotw_Qsl_Sent : EnumerationValue
	{
		/// <summary>
		/// Consructor.
		/// Note: no validation of input is performed in the constructor. Call Validate after
		/// the constructor and when changing values.
		/// </summary>
		/// <param name="sent">QSL sent status</param>
		/// <param name="aEnums">AdifEnumerations object containing the Qsl_Sent enumeration</param>
		public Lotw_Qsl_Sent(string sent, AdifEnumerations aEnums) :
			base(sent, "Qsl_Sent", aEnums)
		{
		}
	}
}
