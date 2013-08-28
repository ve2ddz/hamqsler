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

namespace hamqsler
{
	/// <summary>
	/// Adif Field of type EnumerationField with a single string value.
	/// </summary>
	public class EnumerationValue : EnumerationField
	{
		public string Name
		{
			get {return this.GetType().ToString().Substring("hamqsler.".Length);}
		}
		
		private string eltValue = string.Empty;
		public string Value
		{
			get {return eltValue;}
			set {eltValue = value;}
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="value">Field value</param>
		/// <param name="enums">Enumeration values</param>
		public EnumerationValue(string value, string[] enums) : base(enums)
		{
			Value = value;
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="value">Field value</param>
		/// <param name="enumeration">Name of Adif Enumeration</param>
		/// <param name="aEnum">AdifEnumerations object</param>
		public EnumerationValue(string value, string enumeration, AdifEnumerations aEnum) :
			base(enumeration, aEnum)
		{
			Value = value;
		}
		
		/// <summary>
		/// Validate that the value is within the enumeration
		/// </summary>
		/// <param name="err"></param>
		/// <returns></returns>
		public bool Validate(out string err)
		{
			err = null;
			return base.Validate(Value, out err);
		}
		
		public string ToAdifString()
		{
			return "<" + Name + ":" + Value.Length + ">" + Value;
		}
	}
}
