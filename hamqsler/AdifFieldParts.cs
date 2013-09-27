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
using System.Text.RegularExpressions;

namespace hamqsler
{
	/// <summary>
	///  AdifFieldParts class - used to break up an AdifField string into its constituent parts
	///  AdifField string format is <FieldName:Length of value(:data type - optional)>Value(,{optional enumeration or range})
	/// </summary>
	public class AdifFieldParts
	{
		// ADIF field name - null if AdifField string is invalid in any way
		private string field = null;
		public string Field
		{
			get {return field;}
		}
		
		
		private string value = null;
		public string Value
		{
			get {return value;}
		}
		
		private string dataType = null;
		public string DataType
		{
			get {return dataType;}
		}
		
		private EnumerationField enumField;
		public EnumerationField Enumeration
		{
			get {return enumField;}
		}
		
		private string lowerValue;
		public string LowerValue
		{
			get {return lowerValue;}
		}
		
		private string upperValue;
		public string UpperValue
		{
			get {return upperValue;}
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="field">Adif field string</param>
		public AdifFieldParts(string adifField)
		{
			char[] trimChars = {' '};
			adifField = adifField.Trim(trimChars);
			Regex regex = new Regex("^<([A-Za-z0-9_]+):([1-9][0-9]*)(:([A-Z])){0,1}>" +
			                        "([A-Za-z0-9_]*)(,{(([A-Za-z0-9_,]+)|([0-9]+(\\.[0-9]*)*:[0-9]+(\\.[0-9]*)*))}){0,1}$");
			Match m = regex.Match(adifField);
			if(m.Success)
			{
				field = m.Groups[1].Value;
				dataType = m.Groups[4].Value;
				value = m.Groups[5].Value;
				int len = Int32.Parse(m.Groups[2].Value);
				if(len < value.Length)
				{
					value = value.Substring(0, len);
				}
				string enumers = m.Groups[8].Value;
				if(!enumers.Equals(string.Empty))
				{
					enumField = new EnumerationField(enumers.Split(','));
				}
				string rangeString = m.Groups[9].Value;
				if(!rangeString.Equals(string.Empty))
				{
					string[] range = rangeString.Split(':');
					lowerValue = range[0];
					upperValue = range[1];
					float lower = float.Parse(lowerValue);
					float upper = float.Parse(upperValue);
					if(lower > upper)
					{
						field = null;		// show bad ADIF field
					}
				}
			}
		}
	}
}
