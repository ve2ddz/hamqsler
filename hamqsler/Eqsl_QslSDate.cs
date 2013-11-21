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
	/// Eqsl_QslSDate class - date QSL sent to eQSL.cc
	/// </summary>
	public class Eqsl_QslSDate :DateField
	{
		/// <summary>
		/// Constructor.
		/// Note: no validation of input is performed in the constructor. Call Validate after
		/// the constructor and when changing values.
		/// </summary>
		/// <param name="date">date QSL sent</param>
		public Eqsl_QslSDate(string date) : base(date)
		{
		}
		
		/// <summary>
		/// This field is valid only if Eqsl_Qsl_Sent is Y, Q, or I.
		/// Delete field if other value or null
		/// </summary>
		/// <param name="qso">Qso2 object containing this field</param>
		/// <returns>string containing modification message</returns>
		public override string ModifyValues(Qso2 qso)
		{
			string mod = null;
			Eqsl_Qsl_Sent sent = qso.GetField("Eqsl_Qsl_Sent") as Eqsl_Qsl_Sent;
			if(sent == null || (sent.Value != "Y" && sent.Value != "Q" && sent.Value != "I"))
			{
				qso.Fields.Remove(this);
				mod = "\tEqsl_QslSDate field deleted. This field is only valid when " +
						"Eqsl_Qsl_Sent field is Y, Q, or I." +
		                Environment.NewLine;
			}
			return mod;
		}
	}
}
