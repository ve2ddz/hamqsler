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
	/// Lotw_Qsl_Rcvd class - Arrl Logbook of the World QSL received status
	/// </summary>
	public class Lotw_Qsl_Rcvd : EnumerationValue
	{
		/// <summary>
		/// Constructor.
		/// Note: no validation of input is performed in the constructor. Call Validate after
		/// the constructor and when changing values.
		/// </summary>
		/// <param name="rcvd">received status</param>
		/// <param name="aEnums">AdifEnumerations object containing the Qsl_Rcvd enumeration</param>
		public Lotw_Qsl_Rcvd(string rcvd, AdifEnumerations aEnums) : base(rcvd, "Qsl_Rcvd", aEnums)
		{
		}
		
		/// <summary>
		///  Change deprecated values to their replacements
		/// 
		/// </summary>
		/// <param name="qso">Qso2 object containing this field</param>
		/// <returns>string containing message about changes made</returns>
		public override string ModifyValues2(Qso2 qso)
		{
			// this code must be in ModifyValues2, not ModifyValues, because this code changes
			// status of 'V' which must be present when Eqsl_QslRDate.ModifyValues is called.
			// We cannot guarantee the order in which objects in Qso2.Fields is stored.
			string mod = string.Empty;
			string modStr = null;
			if(Value.Equals("V"))
			{
				Credit_Granted granted = qso.GetField("Credit_Granted") as Credit_Granted;
				if(granted == null)
				{
					Credit_Granted credGranted = new Credit_Granted("DXCC:LOTW,DXCC_BAND:LOTW,DXCC_Mode:LOTW",
				                                            aEnums);
					qso.Fields.Add(credGranted);
				}
				else
				{
					granted.Add(new Credit("DXCC:LOTW", aEnums));
					granted.Add(new Credit("DXCC_BAND:LOTW", aEnums));
					granted.Add(new Credit("DXCC_MODE:LOTW", aEnums));
				}
				qso.Fields.Remove(this);
				modStr = "\tValue 'V' is deprecated and replaced with Credit_Granted values: 'DXCC:LOTW', 'DXCC_BAND:LOTW', and 'DXCC_MODE:LOTW'." +
		                Environment.NewLine;
			}
			return modStr;
		}
	}
}
