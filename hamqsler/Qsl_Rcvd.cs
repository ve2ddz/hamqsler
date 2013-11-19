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
	/// Qsl_Rcvd class - QSL received status
	/// </summary>
	public class Qsl_Rcvd : EnumerationValue
	{
		/// <summary>
		/// Constructor.
		/// Note: no validation of input is performed in the constructor. Call Validate after
		/// the constructor and when changing values.
		/// </summary>
		/// <param name="rcvd">received status</param>
		/// <param name="aEnums">AdifEnumerations object containing the Qsl_Rcvd enumeration</param>
		public Qsl_Rcvd(string rcvd, AdifEnumerations aEnums)
			: base(rcvd, "Qsl_Rcvd", aEnums)
		{
		}
		
		/// <summary>
		///  Change deprecated values to their replacements
		/// </summary>
		/// <param name="qso">Qso2 object containing this field</param>
		/// <returns>string containing message about changes made</returns>
		public override string ModifyValues(Qso2 qso)
		{
			string mod = string.Empty;
			string modStr = null;
			if(Value.Equals("V"))
			{
				Credit_Granted granted = new Credit_Granted("DXCC:card,DXCC_BAND:card,DXCC_Mode:card",
				                                            aEnums);
				qso.ValidateAndAddField(granted, string.Empty, ref mod);
				qso.Fields.Remove(this);
				
				if(mod != null && mod.Length != 0)
				{
					modStr = "\tError encountered attempting to replace Qsl_Rcvd value 'V' with "
						+ "Credit_Granted values of 'DXCC:card,DXCC_BAND:card,DXCC_Mode:card'." +
						Environment.NewLine + 
						"\t" + mod + Environment.NewLine;
				}
				else
				{
					modStr = "\tValue 'V' is deprecated and replaced with Credit_Granted values: 'DXCC:CARD', 'DXCC_BAND:CARD', and 'DXCC_MODE:CARD'." +
			                Environment.NewLine;
				}
			}
			return modStr;
		}
	}
}
