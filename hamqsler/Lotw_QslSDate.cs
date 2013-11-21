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
	/// Lotw_QslSDate class - date QSL sent to ARRL Logbook of the World
	/// </summary>
	public class Lotw_QslSDate : DateField
	{
		/// <summary>
		/// Constructor.
		/// Note: no validation of input is performed in the constructor. Call Validate after
		/// the constructor and when changing values.
		/// </summary>
		/// <param name="date">date QSL was sent</param>
		public Lotw_QslSDate(string date) : base(date)
		{
		}
		
		/// <summary>
		/// This field is valid only if Lotw_Qsl_Sent is Y, Q, or I.
		/// Delete field if other value or null
		/// </summary>
		/// <param name="qso">Qso2 object containing this field</param>
		/// <returns>string containing modification message</returns>
		public override string ModifyValues(Qso2 qso)
		{
			string mod = null;
			Lotw_Qsl_Sent sent = qso.GetField("Lotw_Qsl_Sent") as Lotw_Qsl_Sent;
			if(sent == null || (sent.Value != "Y" && sent.Value != "Q" && sent.Value != "I"))
			{
				qso.Fields.Remove(this);
				mod = "\tLotw_QslSDate field deleted. This field is only valid when " +
						"Lotw_Qsl_Sent field is Y, Q, or I." +
		                Environment.NewLine;
			}
			return mod;
		}
	}
}
