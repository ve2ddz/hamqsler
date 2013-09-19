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
	/// Usaca_Counties class - Two US counties in the case where the contacted station is located
	/// on a border between two counties, representing counties credited to the QSO for the
	/// CQ Magazine USA-CA award program
	/// (e.g. MA,Franklin:MA,Hampshire)
	/// </summary>
	public class Usaca_Counties : AdifField
	{
		private DelimitedList counties = null;
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="cnties">list of two counties separated by ':'</param>
		public Usaca_Counties(string cnties)
		{
			counties = new DelimitedList(':', cnties);
		}
		
		public int Count
		{
			get {return counties.Count;}
		}
		
		/// <summary>
		/// Create string in Adif field format
		/// </summary>
		/// <returns>Counties in Adif field format</returns>
		public string ToAdifString()
		{
			string cnties = counties.ToString();
			return string.Format("<Usaca_Counties:{0}>{1}", cnties.Length, cnties);
		}
		
		/// <summary>
		/// Validate the counties - must be exactly 2, and must be in correct format
		/// </summary>
		/// <param name="err">Error message if not valid, null otherwise</param>
		/// <returns>true if valid, false otherwise</returns>
		public override bool Validate(out string err)
		{
			err = null;
			if(counties.Count != 2 && counties.Count != 0)
			{
				err = "Usaca_Counties must contain exactly two counties.";
				return false;
			}
			foreach(string cnty in counties.Items)
			{
				if(!Regex.IsMatch(cnty, "^[A-Z]{2},[A-Za-z]+[ A-Za-z]*$"))
				{
					err = string.Format("'{0}' is not in correct county format.", cnty);
					return false;
				}
			}
			return true;
		}
	}
}
