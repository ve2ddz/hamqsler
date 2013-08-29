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
	/// Description of DelimitedListEnumeration.
	/// </summary>
	public class DelimitedListEnumeration : EnumerationField
	{
		private DelimitedList delimList = null;
		
		public int Count
		{
			get {return delimList.Count;}
		}
		
		public string Name
		{
			get {return this.GetType().ToString().Substring("hamqsler.".Length);}
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="separator">list delimiter</param>
		/// <param name="list">delimited list</param>
		/// <param name="enums">list of enumerated values</param>
		public DelimitedListEnumeration(char separator, string list, string[] enums)
			: base(enums)
		{
			delimList = new DelimitedList(separator, list);
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="separator">list delimiter</param>
		/// <param name="list">delimited list</param>
		/// <param name="enumeration">name of enumeration in AdifEnumerations</param>
		/// <param name="aEnums">AdifEnumerations object containing the enumeration</param>
		public DelimitedListEnumeration(char separator, string list, string enumeration,
		                                AdifEnumerations aEnums)
			: base(enumeration, aEnums)
		{
			delimList = new DelimitedList(separator, list);
		}
		
		/// <summary>
		/// Determines if an item is in the delimited list
		/// </summary>
		/// <param name="item">item to check if in list</param>
		/// <returns></returns>
		public bool IsInList(string item)
		{
			return delimList.IsInList(item);
		}
		
		/// <summary>
		/// Check that all items in the delimited list are in the enumeration
		/// </summary>
		/// <param name="err">Error message if at least one item is not in the enumeration</param>
		/// <returns>true if all values in the delimited list are in the enumeration, false otherwise</returns>
		public bool Validate(out string err)
		{
			err = string.Empty;
			foreach(string item in delimList.Items)
			{
				if(!base.Validate(item, out err))
			   {
			   		return false;
			   }
			}
			return true;
		}
		
		public string ToAdifString()
		{
			string list = delimList.ToString();
			return "<" + Name + ":" + list.Length + ">" + list;

		}
	}
}
