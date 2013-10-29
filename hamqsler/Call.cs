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
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace hamqsler
{
	/// <summary>
	/// Call class - the contacted station's callsign
	/// </summary>
	public class Call : StringField
	{
        // List for holding the regular expressions for ARRL prefixes that might be 
        // misinterpreted as callsigns
        private static List<Regex> arrlPrefixes = new List<Regex>();
		
		// the following array contains prefixes that may be misinterpreted as callsigns,
		// so special processing will be required.
		private static string[] prefixes = {
			"3D2[CR]",
			"BV9P",
			"CE0[AXZ]",
			"E51[NS]",
			"FO0[AM]",
			"FT[0-9][WX]",
			"HK0[AM]",
			"JD1[MO]",
			"KH[57]K",
			"PY0[FT]",
			"R1FJ",
			"R1MV",
			"VK0[HM]",
			"VK9[CLMNWX]",
			"VP2[MV]",
			"VP6[DP]",
			"VP8[FGHOS]"
		};
		
		// the following array contains valid, non-standard callsigns
		private static string[] nsCalls = {
			"BM100",
			"BN100",
			"BO100",
			"BP100",
			"BQ100",
			"BU100",
			"BV100",
			"BW100",
			"BX100",
			"LM9L40Y",
			"OEH20",
			"SX1912",
			"VI6ARG30",
			"XM31812",
			"TE1856",
			"8J2KSG7X",
			"ZW1CCOM54"			
		};

		/// <summary>
		/// Static constructor
		/// </summary>
		static Call()
		{
			foreach(string prefix in prefixes)
			{
				arrlPrefixes.Add(new Regex("^" + prefix + "$"));
			}

		}
		
		/// <summary>
		/// Constructor
		/// Note: no validation of input is performed in the constructor. Call Validate after
		/// the constructor and when changing values.
		/// </summary>
		/// <param name="call">callsign</param>
		public Call(string call) : base(call)
		{
		}
		
		/// <summary>
		/// Validate the callsign
		/// </summary>
		/// <param name="err">Error message if callsign is not valid</param>
		/// <param name="modStr">Message if value has been modified (always null for this class)</param>
		/// <returns>True if callsign is valid, false otherwise</returns>
		public override bool Validate(out string err, out string modStr)
		{
			err = null;
			modStr = null;
			if(Value == null)
			{
				err = "\tNull callsign is invalid.";
				return false;
			}
			string call = GetCall();
			if(call != null)
			{
				if(IsValid(call))
				{
					return true;
				}
			}
			err = string.Format("\tCallsign '{0}' is invalid.", Value);
			return false;
		}
		
		/// <summary>
		/// Gets calsign from possible prefix/call/suffixes
		/// </summary>
		/// <returns>Callsign if valid, or null if no callsign found</returns>
		public string GetCall()
		{
			List<string> calls = new List<string>();
			string[] parts = Value.Split('/');
			foreach(string c in parts)
			{
				string cc = c;
				if(Call.IsValid(c))
				{
                    foreach (Regex reg in arrlPrefixes)
                    {
                        // if prefix, then ignore it
                        if (reg.IsMatch(c))
                        {
                            cc = null;
                            break;
                        }
                    }
                    // not a prefix, so add to list
                    if (cc != null)
                    {
                        calls.Add(cc);
                    }
				}
			}
			if(calls.Count < 1)
			{
				return null;
			}
			else if(calls.Count == 1)
			{
				return calls[0];
			}
            else
            {
                // if more than one valid call
                foreach (string c in calls)
                {
                    // VP2E is special case where it is both a valid prefix and a valid callsign
                    if (c.Equals("VP2E"))
                    {
                        calls.Remove("VP2E");
                        break;
                    }
                }
                // now assume that the first call is the valid one.
                // this will be the case except in strange circumstances.
                return calls[0];
            }
		}
		
		/// <summary>
		/// Determine if input callsign is valid. Callsign cannot contain prefix or suffix
		/// </summary>
		/// <param name="callsign">Callsign to check for validity</param>
		/// <returns>True if callsign is valid, false otherwise</returns>
		public static bool IsValid(string callsign)
		{
			if(callsign == null || callsign.Equals(string.Empty))
			{
				return false;
			}
			else if(Regex.IsMatch(callsign.ToUpper(), "^[0-9]{0,1}[A-Z]{1,2}[0-9]{1,4}[A-Z]+$"))
			{
				return true;
			}
			else 	
			{
				// special callsigns
				foreach(string nsCall in nsCalls)
				{
					if(callsign == nsCall)
					{
						return true;
					}
				}
				return false;
			}
		}
	}
}
