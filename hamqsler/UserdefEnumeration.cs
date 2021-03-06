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
	/// UserdefEnumeration class - user defined field of type enumeration
	/// </summary>
	public class UserdefEnumeration : StringField
	{
		private Userdef userdef;
		
		public override string Name 
		{
			get { return userdef.UName; }
		}
		
		/// <summary>
		/// Constructor
		/// Note: no validation of input is performed in the constructor. Call Validate after
		/// the constructor and when changing values.
		/// </summary>
		/// <param name="value">field value</param>
		/// <param name="userdefField">Userdef object that defines this field type</param>
		public UserdefEnumeration(string value, Userdef userdefField) : base(value)
		{
			userdef = userdefField;
		}
		
		/// <summary>
		/// Create ADIF string for this field
		/// </summary>
		/// <returns>ADIF string represented by this object</returns>
		public override string ToAdifString()
		{
			return string.Format("<{0}:{1}:{2}>{3}", userdef.UName, Value.Length,
			                     userdef.DataType.Value, Value);
		}
		
		/// <summary>
		/// Validate the UserdefEnumeration
		/// </summary>
		/// <param name="err">Error message if value is not valid</param>
		/// <param name="modStr">Message if value has been modified (always null for this class)</param>
		/// <returns></returns>
		public override bool Validate(out string err, out string modStr)
		{
			if(!base.Validate(out err, out modStr))
			{
				return false;
			}
			if(!userdef.EnumField.Validate(Value, out err))
			{
				return false;
			}
			return true;
		}
	}
}
