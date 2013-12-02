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
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace hamqsler
{
	/// <summary>
	/// Description of CallsBureaus.
	/// </summary>
	public class CallsBureaus : AppDataXMLFile
	{
		public string Version
		{
			get 
			{
				XAttribute version = xDoc.Root.Attribute("Version");
				if(version != null)
				{
					return version.Value;
				}
				else
				{
					return null;
				}
			}
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="cbStream">stream containing the XML document</param>
		public CallsBureaus() : base("CallsBureaus.xml")
		{
		}
		
		/// <summary>
		/// Determine if the argument matches one of the non-standard calls
		/// </summary>
		/// <param name="call">callsign to check</param>
		/// <returns>true if call is in non-standard call list, false otherwise</returns>
		public bool IsNonStandardCall(string call)
		{
				var vals = from val in 
					xDoc.Descendants("NonStandardCalls").Descendants("Call")
					select val;
				foreach(var val in vals)
				{
					if(val.Value.Equals(call))
					{
						return true;
					}
				}
			return false;
		}
		
		/// <summary>
		/// Check version of XML file against version provided in argument
		/// </summary>
		/// <param name="version">Version to check against</param>
		/// <returns>true if argument is later than version in XML file</returns>
		public override bool CheckVersion(string version)
		{
			// get this file's version info
			// this code assumes that each part of the vers
			string ver = Version;
			string[] verBits = ver.Split('.');
			string vers = string.Format("{0}{1}{2}", 
			                            (verBits[0].Length == 1 ? "0" : "") + verBits[0],
			                            (verBits[1].Length == 1 ? "0" : "") + verBits[1],
			                            (verBits[2].Length == 1 ? "0" : "") + verBits[2]);
			if(vers.CompareTo(version) < 0)
				return true;
			else
				return false;
		}
		
		/// <summary>
		/// Determine if the input call is a prefix
		/// </summary>
		/// <param name="call">Callsign to check</param>
		/// <returns>true if a valid prefix, false otherwise</returns>
		public bool IsPrefix(string call)
		{
			var prefixes = from val in 
				xDoc.Descendants("PrefixRegexes").Descendants("PrefixRegex")
				select val;
            foreach (string prefix in prefixes)
            {
                // if prefix, then ignore it
                Regex reg = new Regex(prefix);
                if (reg.IsMatch(call))
                {
                    return true;
                }
            }
			return false;
		}
		
		public bool IsCallAndPrefix(string call)
		{
			var callsPrefixes = from val in
				xDoc.Descendants("CallsAndPrefixes").Descendants("CallAndPrefix")
				select val;
			foreach(string pre in callsPrefixes)
			{
				if(call.Equals(pre))
				{
					return true;
				}
			}
			return false;
		}
	}
}
