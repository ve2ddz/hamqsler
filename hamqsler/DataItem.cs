/*
 *  Author:
 *       Jim Orcheson <jimorcheson@gmail.com>
 * 
 *  Copyright (c) 2014 Jim Orcheson
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
	/// Generic class that mimics Dictionary or Hashtable DictionaryItem or KeyValuePair<string, string>.
	/// This is required because Dictionary and HashTable are not serializable.
	/// </summary>
	public class DataItem
	{
		public string Key;
 
		public string Value;
		 
		public DataItem() {}
		
		public DataItem(string key, string value)
		{
			Key = key;
		    Value = value;
		}
	}
}
