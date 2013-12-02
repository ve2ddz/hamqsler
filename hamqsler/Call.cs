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
		/// Gets callsign from possible prefix/call/suffixes
		/// </summary>
		/// <returns>Callsign if valid, or null if no callsign found</returns>
		public string GetCall()
		{
			List<string> calls = new List<string>();
			string[] parts = Value.Split('/');
			foreach(string c in parts)
			{
			if(Call.IsValid(c))
				{
                    if(!App.CallBureaus.IsPrefix(c))
					{
						calls.Add(c);
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
                	if(App.CallBureaus.IsCallAndPrefix(c))
                	{
                		calls.Remove(c);
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
				return App.CallBureaus.IsNonStandardCall(callsign);
			}
		}
	}
}
