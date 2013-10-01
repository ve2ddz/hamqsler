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
	/// UserdefDate class - user defined field of type date
	/// </summary>
	public class UserdefDate : DateField
	{
		private Userdef userdef;
		
		/// <summary>
		/// Constructor
		/// Note: no validation of input is performed in the constructor. Call Validate after
		/// the constructor and when changing values.
		/// </summary>
		/// <param name="value">field value</param>
		/// <param name="userdefField">Userdef object that defines this field type</param>
		public UserdefDate(string value, Userdef userdefField) : base(value)
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
	}
}
