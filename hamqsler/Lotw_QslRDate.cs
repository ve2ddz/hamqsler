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
	/// Lotw_QslRDate class - date QSL received from ARRL Logbook of the World
	/// </summary>
	public class Lotw_QslRDate : DateField
	{
		/// <summary>
		/// Constructor.
		/// Note: no validation of input is performed in the constructor. Call Validate after
		/// the constructor and when changing values.
		/// </summary>
		/// <param name="date">date QSL was received</param>
		public Lotw_QslRDate(string date) : base(date)
		{
		}
		/// <summary>
		/// This field is valid only if Lotw_Qsl_Rcvd is Y, I, or V.
		/// Delete field if other value or null
		/// </summary>
		/// <param name="qso">Qso2 object containing this field</param>
		/// <returns>string containing modification message</returns>
		public override string ModifyValues(Qso2 qso)
		{
			string mod = null;
			Lotw_Qsl_Rcvd rcvd = qso.GetField("Lotw_Qsl_Rcvd") as Lotw_Qsl_Rcvd;
			if(rcvd == null || (rcvd.Value != "Y" && rcvd.Value != "I" && rcvd.Value != "V"))
			{
				qso.Fields.Remove(this);
				mod = "\tLotw_QslRDate field deleted. This field is only valid when " +
						"Lotw_Qsl_Rcvd field is Y, I, or V." +
		                Environment.NewLine;
			}
			return mod;
		}
	}
}
