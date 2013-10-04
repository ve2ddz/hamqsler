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
	/// AdifFields class - used to break up a QSO record into its various fields and the
	/// fields into parts
	/// </summary>
	public class AdifFields
	{
		private List<string> fieldNames = new List<string>();
		/// <summary>
		/// string array of field names, one for each field
		/// </summary>
		public string[] FieldNames
		{
			get {return fieldNames.ToArray();}
		}
		
		private List<string> dataTypes = new List<string>();
		/// <summary>
		/// string array of data types, one for each field
		/// </summary>
		public string[] DataTypes
		{
			get {return dataTypes.ToArray();}
		}
		
		private List<string> values = new List<string>();
		/// <summary>
		/// string array of values, one for each field
		/// </summary>
		public string[] Values
		{
			get {return values.ToArray();}
		}
		
		/// <summary>
		/// count of the number of fields
		/// </summary>
		public int Count
		{
			get {return FieldNames.Length;}
		}
		
		/// <summary>
		/// Constructor - builds Lists for field name, data type, and value, one item of
		/// each for each ADIF field in the input string
		/// </summary>
		/// <param name="record">ADIF string containing QSO record</param>
		public AdifFields(string record)
		{
			string reg = "<([A-Za-z0-9_]+):([0-9]+)(:([A-Z])){0,1}>";
			string rec = record.ToUpper();
			int valLength = 0;
			if(rec.Contains("<EOR>"))
			{
				int eorIndex = rec.IndexOf("<EOR>");
				record = record.Substring(0, eorIndex);
			}
			while(record.Length != 0)
			{
				int fieldStart = record.IndexOf('<');
				record = record.Substring(fieldStart);
				int fieldEnd = record.IndexOf('>');
				string field = record.Substring(0, fieldEnd + 1);
				Match m = Regex.Match(field, reg);
				if(m.Groups.Count > 0)
				{
					fieldNames.Add(m.Groups[1].Value);
					dataTypes.Add(m.Groups[4].Value);
					string len = m.Groups[2].Value;
					valLength = Int32.Parse(len);
					values.Add(record.Substring(fieldEnd + 1, valLength));
				}
				record = record.Substring(fieldEnd + valLength + 1);
			
			}
		}
	}
}
