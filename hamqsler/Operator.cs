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
	/// Operator class - the logging operator's callsign
	/// </summary>
	public class Operator : Call
	{
		/// <summary>
		/// Constructor
		/// Note: no validation of input is performed in the constructor. Call Validate after
		/// the constructor and when changing values.
		/// </summary>
		/// <param name="call">callsign</param>
		public Operator(string call) : base(call)
		{
		}
		
		/// <summary>
		/// Validate the callsign. Must be simple call in valid format
		/// </summary>
		/// <param name="err">Error message if call is invalid, null otherwise</param>
		/// <param name="modStr">Message if value has been modified (always null for this class)</param>
		/// <returns>true if call is valid, false otherwise</returns>
		public override bool Validate(out string err, out string modStr)
		{
			err = null;
			modStr = null;
			if(!base.Validate(out err, out modStr))
			{
				return false;
			}
			if(!Regex.IsMatch(Value, "^[A-Za-z0-9]+$"))
			{
				err = string.Format("\tCallsign '{0}' contains modifiers.", Value);
				return false;
			}
			return true;
		}
		
		/// <summary>
		/// Generate Station_Callsign field from this field, if does not exist
		/// </summary>
		/// <param name="qso">Qso2 object containing this field</param>
		/// <returns>string describing the modification made</returns>
		public override string ModifyValues(Qso2 qso)
		{
			string mod = null;
			Station_Callsign call = qso.GetField("Station_Callsign") as Station_Callsign;
			if(call == null)
			{
				call = new Station_Callsign(Value);
				qso.Fields.Add(call);
				call.ModifyValues(qso);
				mod = "\tStation_Callsign field generated from Operator field." +
			                Environment.NewLine;
			}
			return mod;
		}
	}
}
