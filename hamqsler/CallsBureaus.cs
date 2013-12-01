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
	}
}
