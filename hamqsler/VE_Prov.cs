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
	/// VE_Prov class - deprecated. Use State instead
	/// </summary>
	public class VE_Prov : StringField
	{
		/// <summary>
		/// Constructor
		/// Note: no validation of input is performed in the constructor. Call Validate after
		/// the constructor and when changing values.
		/// </summary>
		/// <param name="prov">Canadian province</param>
		public VE_Prov(string prov) : base(prov)
		{
		}

		/// <summary>
		/// Change VE_PROV to STATE
		/// </summary>
		/// <param name="qso">Qso2 object containing this field</param>
		/// <returns>string indicating the change made</returns></returns>
		public override string ModifyValues(Qso2 qso)
		{
			string mod = null;
			State state = qso.GetField("State") as State;
			if(state == null)
			{
				state = new State(Value);
				qso.Fields.Add(state);
				mod = "\tVE_Prov field deprecated. VE_Prov field deleted and replace with State field." +
					Environment.NewLine;
				state.ModifyValues(qso);
				
			}
			else
			{
				mod = "\tVE_Prov field deprecated. State field already exists, so VE_Prov field deleted." +
			                Environment.NewLine;
			}
			qso.Fields.Remove(this);
			return mod;
		}
	}
}
