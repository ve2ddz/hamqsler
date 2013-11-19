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
	/// Guest_Op class - the logging operator's callsign.
	/// This field is deprecated. Use Operator instead.
	/// </summary>
	public class Guest_Op : Call
	{
		/// <summary>
		/// Constructor
		/// Note: no validation of input is performed in the constructor. Call Validate after
		/// the constructor and when changing values.
		/// </summary>
		/// <param name="call">callsign of the logging operator</param>
		public Guest_Op(string call) : base(call)
		{
		}
		
		/// <summary>
		/// Validate the callsign - must be valid call with no prefix or suffix
		/// </summary>
		/// <param name="err">Error message if not valid simple callsign, null if valid</param>
		/// <param name="modStr">Message if value has been modified (always null for this class)</param>
		/// <returns>true if callsign is valid, false otherwise</returns>
		public override bool Validate(out string err, out string modstr)
		{
			err = null;
			modstr = null;
			if(!Value.Equals(this.GetCall()))
		    {
		   		err = "\tNot a valid callsign.";
		   		return false;
		    }
			return true;
		}
		
		/// <summary>
		/// Delete this field and change to Operator field if that does not already exist.
		/// </summary>
		/// <param name="qso">Qso2 object containing this field</param>
		/// <returns>Message indicating any modifications made</returns>
		public override string ModifyValues(Qso2 qso)
		{
			string mod = null;
			Operator op = qso.GetField("Operator") as Operator;
			if(op == null)
			{
				op = new Operator(Value);
				qso.Fields.Add(op);
				mod = "\tGuest_Op field changed to Operator field." +
			                Environment.NewLine;
				string mod2 = op.ModifyValues(qso);
				if(mod2 != null)
				{
					mod += mod2;
				}
			}
			else
			{
				mod = "\tGuest_Op field cannot be changed to Operator field because Operator field already exists. Guest_Op field deleted." +
			           		Environment.NewLine;
			}
			qso.Fields.Remove(this);
			return mod;
		}
	}
}
