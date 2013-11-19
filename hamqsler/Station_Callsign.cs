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
using System.Text.RegularExpressions;

namespace hamqsler
{
	/// <summary>
	/// Station_Callsign class - the logging station's callsign (the callsign used over the air)
	/// </summary>
	public class Station_Callsign : Call
	{
		/// <summary>
		/// Constructor
		/// Note: no validation of input is performed in the constructor. Call Validate after
		/// the constructor and when changing values.
		/// </summary>
		/// <param name="call">callsign</param>
		public Station_Callsign(string call) : base(call)
		{
		}
		
		/// <summary>
		/// Generate Owner_Callsign field if it does not exist
		/// </summary>
		/// <param name="qso">Qso2 object containing this field</param>
		/// <returns>string indicating changes made</returns>
		public override string ModifyValues(Qso2 qso)
		{
			string mod = null;
			Owner_Callsign owner = qso.GetField("Owner_Callsign") as Owner_Callsign;
			if(owner == null)
			{
				owner = new Owner_Callsign(Value);
				qso.Fields.Add(owner);
				owner.ModifyValues(qso);
				mod = "\tOwner_Callsign field generated from Station_Callsign" +
			                Environment.NewLine;
			}
			return mod;
		}
	}
}
