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
	/// Base class for all number based ADIF Fields
	/// </summary>
	public class NumberField : AdifField
	{
		/// <summary>
		/// constructor
		/// </summary>
		/// <param name="value">Value for the field</param>
		public NumberField(string value) : base(value)
		{
		}
		
		/// <summary>
		/// Validates the value as a number
		/// </summary>
		/// <returns>true if value is empty or a number</returns>
		public override bool IsValid()
		{
			if(string.Equals(string.Empty, Value))
		    {
		   		return true;
		    }
			return Regex.IsMatch(Value, @"^-{0,1}([\d]+|[\d]+\.[\d]*|\.[\d]+)$");
		}
	}
}
