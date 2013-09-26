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
	/// Userdef class - Userdef header field
	/// </summary>
	public class Userdef : AdifField
	{
		AdifEnumerations aEnums;
		
		private string uName = string.Empty;
		public string UName
		{
			get {return uName;}
		}
		
		private EnumerationValue dataType = null;
		public EnumerationValue DataType
		{
			get {return dataType;}
		}
		
		private EnumerationField enumField = null;
		public EnumerationField EnumField
		{
			get {return enumField;}
		}
		
		private string lowerValue = string.Empty;
		public string LowerValue
		{
			get {return lowerValue;}
		}
		private string upperValue = string.Empty;
		public string UpperValue
		{
			get {return upperValue;}
		}

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="name">field name</param>
		/// <param name="type">data type</param>
		/// <param name="num">Userdef number</param>
		public Userdef(string name, string type, AdifEnumerations enums)
		{
			aEnums = enums;
			uName = name;
			dataType = new EnumerationValue(type, "DataType", aEnums);
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="name">field name</param>
		/// <param name="type">data type</param>
		/// <param name="num">Userdef number</param>
		/// <param name="enums">enumeration values</param>
		public Userdef(string name, string type,string[] enums, AdifEnumerations adifEnums)
		{
			aEnums = adifEnums;
			uName = name;
			dataType = new EnumerationValue(type, "DataType", aEnums);
			enumField = new EnumerationField(enums);
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="name">field name</param>
		/// <param name="type">data type</param>
		/// <param name="num">Userdef number</param>
		/// <param name="lLimit">lower limit of range</param>
		/// <param name="uLimit">upper limit of range</param>
		public Userdef(string name, string type, string lLimit, string uLimit, AdifEnumerations enums)
		{
			aEnums = enums;
			uName = name;
			dataType = new EnumerationValue(type, "DataType", aEnums);
			lowerValue = lLimit;
			upperValue = uLimit;
		}
		
		/// <summary>
		/// create string in ADIF format
		/// </summary>
		/// <param name="defNum">Userdef number</param>
		/// <returns>Userdef in ADIF format</returns>
		public string ToAdifString(uint defNum)
		{
			if(EnumField != null)
			{
				string enumString = EnumField.ToString();
				string value = uName + ",{" + enumString + "}";
				return string.Format("<Userdef{0}:{1}:{2}>{3}", defNum, 
				                            value.Length, DataType.Value, value);
			}
			else if(!lowerValue.Equals(string.Empty))
			{
				string rangeString = lowerValue + ":" + upperValue;
				string value = uName + ",{" + rangeString +"}";
				return string.Format("<Userdef{0}:{1}:{2}>{3}", defNum,
				                     value.Length, DataType.Value, value);
			}
			return string.Format("<Userdef{0}:{1}:{2}>{3}", defNum, uName.Length, DataType.Value, UName);
		}
		
		/// <summary>
		/// Validate the Userdef object
		/// </summary>
		/// <param name="err">Error message if object not valid, null otherwise</param>
		/// <returns>true if valid, false otherwise</returns>
		public override bool Validate(out string err)
		{
			err = null;
			if(uName == null || uName.Length == 0)
			{
				err = "Invalid fieldname.";
				return false;
			}
			if(dataType == null || dataType.Value == null || dataType.Value.Length == 0 ||
			   !dataType.Validate(out err))
			{
				err = "Invalid data type.";
				return false;
			} 
			if(EnumField != null && (EnumField.Enumeration == null || EnumField.Enumeration.Length == 0))
			{
				err = "Invalid enumeration.";
				return false;
			}
			if(lowerValue == null)
			{
				err = "Invalid lower limit.";
				return false;				
			}
			if(upperValue == null)
			{
				err = "Invalid upper limit.";
				return false;				
			}
			if(lowerValue.Equals(string.Empty) && upperValue.Equals(string.Empty))
			{
				return true;
			}
			else if(lowerValue.Equals(string.Empty) || !Regex.IsMatch(lowerValue, @"^-{0,1}[0-9]+\.{0,1}[0-9]*$"))
			{
				err = "Invalid lower limit.";
				return false;
			}
			else if(upperValue.Equals(string.Empty) || !Regex.IsMatch(upperValue, @"^-{0,1}[0-9]+\.{0,1}[0-9]*$"))
			{
				err = "Invalid upper limit.";
				return false;				
			}
			return true;
		}
	}
}
