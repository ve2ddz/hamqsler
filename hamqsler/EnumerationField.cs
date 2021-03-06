﻿/*
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
using System.Xml.Linq;

namespace hamqsler
{
	/// <summary>
	/// Base class for QSO Fields of type enumeration
	/// </summary>
	public class EnumerationField : AdifField
	{
		private string[] enumeration = null;
		public string[] Enumeration
		{
			get {return enumeration;}
			set {enumeration = value;}
		}
		
		private string enumName = null;
		public string EnumName
		{
			get {return enumName;}
			set {enumName = value;}
		}
		
		public AdifEnumerations aEnums = null;
		
		/// <summary>
		/// Constructor
		/// Note: no validation of input is performed in the constructor. Call Validate after
		/// the constructor and when changing values.
		/// </summary>
		/// <param name="enums">Enumeration values</param>
		public EnumerationField(string[] enums)
		{
			Enumeration = enums;
		}
		
		/// <summary>
		/// Constructor
		/// Note: no validation of input is performed in the constructor. Call Validate after
		/// the constructor and when changing values.
		/// </summary>
		/// <param name="adifEnumName">Name of Adif enumeration</param>
		/// <param name="adifEnum">AdifEnumerations object that contains the enumeration</param>
		public EnumerationField(string adifEnumName, AdifEnumerations adifEnum)
		{
			aEnums = adifEnum;
			EnumName = adifEnumName;
		}
		
		/// <summary>
		/// Validate that value is within the enumeration
		/// </summary>
		/// <param name="value">Value to check against enumeration</param>
		/// <param name="err">Error message if value not within enumeration</param>
		/// <returns>true if value within enumeration, false otherwise</returns>
		public virtual bool IsInEnumeration(string value, out string err)
		{
			err = null;
			if(Enumeration != null)
			{
				foreach(string enumer in Enumeration)
				{
					if(value.Equals(enumer))
					{
						return true;
					}
				}
			}
			else if(aEnums != null && EnumName != null)
			{
				if(aEnums.IsInEnumeration(EnumName, value))
				{
					return true;
				}
			}
			err = string.Format("\tThis QSO Field is of type enumeration. The value '{0}' " +
			                    "was not found in enumeration.", value);
			return false;
		}
		
		/// <summary>
		/// Validate that the value is in the enumeration
		/// </summary>
		/// <param name="value">Value to check</param>
		/// <param name="err">error messsage if not in enumeration</param>
		/// <returns></returns>
		public virtual bool Validate(string value, out string err)
		{
			err = null;
			return this.IsInEnumeration(value, out err);
		}
		
		/// <summary>
		/// Get list of enumerated values as a string
		/// </summary>
		/// <returns>string containing enumerated values</returns>
		public override string ToString()
		{
			string value = string.Empty;
			if(enumeration != null)
			{
				foreach(string v in enumeration)
				{
					value += v + ",";
				}
				value = value.Length > 0 ? value.Substring(0, value.Length - 1) : value;
			}
			return value;
		}

		
	}
}
