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

namespace hamqsler
{
	/// <summary>
	/// Credit class - holds a single Credit member for a CreditList.
	/// </summary>
	public class Credit
	{
		private AdifEnumerations adifEnums = null;
		
		private string creditName;
		public string CreditName
		{
			get {return creditName;}
		}
		
		private HashSet<string> media = new HashSet<string>();
		public HashSet<string> Media
		{
			get {return media;}
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="credit">credit info (item from Credit enumeration plus QSL medium values</param>
		public Credit(string credit, AdifEnumerations aEnums)
		{
			adifEnums = aEnums;
			string[] parts = credit.Split(':');
			creditName = parts[0];
			if(parts.Length > 1)
			{
				string[] mediums = parts[1].Split('&');
				foreach(string med in mediums)
				{
					Media.Add(med.ToUpper());
				}
			}
			else
			{
				Media.Add(null);
			}
		}

		/// <summary>
		/// Overridden ToString method
		/// </summary>
		/// <returns>string containing the credit information</returns>
		public override string ToString()
		{
			string cred = CreditName;
			if((Media.Count != 1) || (!Media.Contains(null)))
			{
				cred += ":";
				foreach(string med in Media)
				{
					cred += med + "&";
				}
				if(cred.LastIndexOf('&') != -1)
				{
					cred = cred.Substring(0, cred.LastIndexOf('&'));
				}
			}
			return cred;
		}
		
		/// <summary>
		/// Check if a value is one of the media in the media list
		/// </summary>
		/// <param name="medium">medium to check for</param>
		/// <returns>true if medium is in list, false otherwise</returns>
		public bool IsInMedia(string medium)
		{
			if(medium != null)
			{
				medium = medium.ToUpper();
			}
			foreach(string med in Media)
			{
				if(med == null && medium == null)
				{
					return true;
				}
				else
				{
					if(medium.Equals(med))
					{
						return true;
					}
				}
			}
			return false;
		}
		
		/// <summary>
		/// Validate that the Credit and QSL Media are valid (credit is in Credit enumeration,
		/// and media are either null or all are in QSL Medium enumeration)
		/// </summary>
		/// <param name="err">Reason that Validate failed, or null if passed</param>
		/// <returns>true if valid, false if not valid</returns>
		public bool Validate(out string err)
		{
			err = null;
			if(!adifEnums.IsInEnumeration("Credit", CreditName))
			{
				err = string.Format("'{0}' not found in Credit enumeration", CreditName);
				return false;
			}
			else
			{
				if(Media.Contains(null))
				{
					if(Media.Count == 1)
					{
						return true;
					}
					else
					{
						err = "Programming Error: Credit object cannot contain both null and other QSL Media";
						return false;
					}
				}
				foreach(string medium in Media)
				{
					if(!adifEnums.IsInEnumeration("QSL_Medium", medium))
					{
						err = string.Format("'{0}' not found in QSL Medium enumeration", medium);
						return false;
					}
				}
			}
			return true;
		}
	}
}
