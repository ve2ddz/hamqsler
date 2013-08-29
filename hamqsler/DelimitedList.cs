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
	/// DelimitedList class - a list of items that were input using a delimited list.
	/// </summary>
	public class DelimitedList
	{
		private HashSet<string> items = new HashSet<string>();
		public HashSet<string> Items
		{
			get {return items;}
		}
		
		private char separator;
		
		public int Count
		{
			get {return items.Count;}
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="sep">separator for items in the list</param>
		/// <param name="list">delimited list of values</param>
		public DelimitedList(char sep, string list)
		{
			separator = sep;
			if(list != null && !list.Equals(string.Empty))
			{
				string[] cdItems = list.Split(separator);
				for(int i = 0; i < cdItems.Length; i ++)
				{
					cdItems[i] = cdItems[i].Trim();
				}
				Items.UnionWith(cdItems);
			}
		}
		
		/// <summary>
		/// Tests list of items to see if the input parameter is in the list
		/// </summary>
		/// <param name="value">string to test for</param>
		/// <returns>true if value is found in the list, false otherwise</returns>
		public bool IsInList(string value)
		{
			return Items.Contains(value);
		}
		
		/// <summary>
		/// Get a string representation of the values in the list
		/// </summary>
		/// <returns>comma delimited string of values in the list</returns>
		public override string ToString()
		{
			string output = string.Empty;
			foreach(string item in Items)
			{
				output += item + separator;
			}
			if(Items.Count > 0)
			{
				output = output.Substring(0, output.Length - 1);
			}
			return output;
		}
 	}
}
