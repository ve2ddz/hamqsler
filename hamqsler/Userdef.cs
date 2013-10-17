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
		
		/// <summary>
		/// Constructor
		/// Note: no validation of input is performed in the constructor. Call Validate after
		/// the constructor and when changing values.
		/// </summary>
		/// <param name="name">field name</param>
		/// <param name="type">data type</param>
		/// <param name="num">Userdef number</param>
		public Userdef(string name, string type, AdifEnumerations enums)
		{
			aEnums = enums;
			uName = name;
			DataType = new EnumerationValue(type, "DataType", aEnums);
		}
		
		/// <summary>
		/// Constructor
		/// Note: no validation of input is performed in the constructor. Call Validate after
		/// the constructor and when changing values.
		/// </summary>
		/// <param name="name">field name</param>
		/// <param name="type">data type</param>
		/// <param name="num">Userdef number</param>
		/// <param name="enums">enumeration values</param>
		public Userdef(string name, string type, string[] enums, AdifEnumerations adifEnums)
		{
			aEnums = adifEnums;
			uName = name;
			DataType = new EnumerationValue(type, "DataType", aEnums);
			EnumField = new EnumerationField(enums);
		}
		
		/// <summary>
		/// Constructor
		/// Note: no validation of input is performed in the constructor. Call Validate after
		/// the constructor and when changing values.
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
			DataType = new EnumerationValue(type, "DataType", aEnums);
			LowerValue = lLimit;
			UpperValue = uLimit;
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
			else if(!LowerValue.Equals(string.Empty))
			{
				string rangeString = LowerValue + ":" + UpperValue;
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
		/// <param name="modStr">Message if value has been modified (always null for this class)</param>
		/// <returns>true if valid, false otherwise</returns>
		public override bool Validate(out string err, out string modStr)
		{
			err = null;
			modStr = null;
			if(uName == null || uName.Length == 0)
			{
				err = "Invalid fieldname.";
				return false;
			}
			if(DataType == null || DataType.Value == null || DataType.Value.Length == 0 ||
			   !DataType.Validate(out err, out modStr))
			{
				err = "Invalid data type.";
				return false;
			} 
			if(EnumField != null && (EnumField.Enumeration == null || EnumField.Enumeration.Length == 0))
			{
				err = "Invalid enumeration.";
				return false;
			}
			if(LowerValue == null)
			{
				err = "Invalid lower limit.";
				return false;				
			}
			if(UpperValue == null)
			{
				err = "Invalid upper limit.";
				return false;				
			}
			if(LowerValue.Equals(string.Empty) && UpperValue.Equals(string.Empty))
			{
				return true;
			}
			else if(LowerValue.Equals(string.Empty) || !Regex.IsMatch(LowerValue, @"^-{0,1}[0-9]+\.{0,1}[0-9]*$"))
			{
				err = "Invalid lower limit.";
				return false;
			}
			else if(UpperValue.Equals(string.Empty) || !Regex.IsMatch(UpperValue, @"^-{0,1}[0-9]+\.{0,1}[0-9]*$"))
			{
				err = "Invalid upper limit.";
				return false;				
			}
			return true;
		}
	}
}
