//  
//  CallSign.cs
//  
//  Author:
//       Jim <jim@va3hj.ca>
// 
//  Copyright (c) 2011 VA3HJ Software
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Qsos
{
    /// <summary>
    /// Class for processing callsigns and retrieving the callsign from a full call
    /// </summary>
    public class CallSign
    {
        private string fullCall = string.Empty;
        /// <summary>
        /// The full callsign as input to the constructor
        /// </summary>
        public string FullCall
        {
            get { return fullCall; }
        }

        private string call = string.Empty;
        /// <summary>
        /// The actual station callsign determined from the full callsign input to constructor
        /// For example: if full callsign is PJ2/VA3HJ/M, then actual call is VA3HJ
        /// </summary>
        public string Call
        {
            get { return call; }
        }

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
			"XM31812"		
		};

        // static constructor used to load the List above
        static CallSign()
        {
			foreach(string prefix in prefixes)
			{
				arrlPrefixes.Add(new Regex("^" + prefix + "$"));
			}
        }

        /// <summary>
        /// CallSign constructor
        /// </summary>
        /// <param name="call">the callsign to create the object with</param>
        /// <exception>QsoException if the input callsign is null or empty string, or
        /// if the callsign input is not valid</exception>
        public CallSign(string call)
        {
            if (call == null || call.Equals(string.Empty))
            {
                throw new QsoException("Invalid input callsign: either null or empty string");
            }
            call = call.Trim();
            this.call = getCall(call);
            if(this.call != null)
            {
                fullCall = call;
            }
            else
            {
                throw new QsoException("Invalid input callsign: '" + call + "'");
            }
        }

        // helper function that retrieves the call from the full callsign
        private string getCall(string call)
        {
            // list for those portions of the full callsign that are valid by themselves
            List<string> calls = new List<string>();
            // split the full callsign into its parts
            string[] parts = call.Split('/');
            // process each part
            foreach (string c in parts)
            {
                string cc = c;
                if (IsValid(c))
                {
                    // if part is valid, check that is not a prefix
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
            // is there a valid call?
            if (calls.Count < 1)
            {
                // no, so return null
                return null;
            }
            else if (calls.Count == 1)
            {
                // if only one portion is valid, this must be the call
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
        /// Static method to determine if the form of the input call is valid
        /// </summary>
        /// <param name="call">callsign to check</param>
        /// <returns>true if form of callsign is valid, false otherwise</returns>
        public static bool IsValid(string call)
        {
            // make sure that a callsign was actually input
            if (call == null || call.Equals(string.Empty))
            {
                return false;
            }
            Regex regex = new Regex("^[A-Z]{1,2}[0-9]{1,4}[A-Z]+$|^[0-9][A-Z]{1,2}[0-9]{1,4}[A-Z]+$");
            if(regex.IsMatch(call))
			{
				return true;
			}
			else
			{
				foreach(string nsCall in nsCalls)
				{
					if(call == nsCall)
					{
						return true;
					}
				}
				return false;
			}
        }
    }
}
