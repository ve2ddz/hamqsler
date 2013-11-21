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
	/// QslRDate class - QSL received date
	/// </summary>
	public class QslRDate : DateField
	{
		/// <summary>
		/// Constructor
		/// Note: no validation of input is performed in the constructor. Call Validate after
		/// the constructor and when changing values.
		/// </summary>
		/// <param name="date">date QSL received</param>
		public QslRDate(string date) : base(date)
		{
		}
		
		/// <summary>
		/// This field is valid only if Eqsl_Qsl_Rcvd is Y, I, or V.
		/// Delete field if other value or null
		/// </summary>
		/// <param name="qso">Qso2 object containing this field</param>
		/// <returns>string containing modification message</returns>
		public override string ModifyValues(Qso2 qso)
		{
			string mod = null;
			Qsl_Rcvd rcvd = qso.GetField("Qsl_Rcvd") as Qsl_Rcvd;
			if(rcvd == null || (rcvd.Value != "Y" && rcvd.Value != "I" && rcvd.Value != "V"))
			{
				qso.Fields.Remove(this);
				mod = "\tQslRDate field deleted. This field is only valid when " +
						"Qsl_Rcvd field is Y, I, or V." +
		                Environment.NewLine;
			}
			return mod;
		}
	}
}
