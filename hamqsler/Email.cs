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
	/// Email class - the contacted station's email address
	/// </summary>
	public class Email : StringField
	{
		/// <summary>
		/// Constructor
		/// Note: no validation of input is performed in the constructor. Call Validate after
		/// the constructor and when changing values.
		/// </summary>
		/// <param name="addr">email address</param>
		public Email(string addr) : base(addr)
		{
		}
		
		/// <summary>
		/// Validate the email address
		/// </summary>
		/// <param name="err">Error info if not a valid email address</param>
		/// <param name="modStr">Message if value has been modified (always null for this class)</param>
		/// <returns>true if valid, false if not</returns>
		public override bool Validate(out string err, out string modStr)
		{
			err = null;
			modStr = null;
			// the email regular expression below is copyright by Mykola Dobrochynskyy and
			// released under Code Project Open License (CPOL) 1.02.
			// See http://www.codeproject.com/Articles/22777/Email-Address-Validation-Using-Regular-Expression
			// for the more information about this expression, and
			// http:..www.codeproject.com/info/cpol10.aspx for a copy of the license.
			if(Regex.IsMatch(Value, @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
						     + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
										[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
						     + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
										[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
						     + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$"))
			{
				return true;
			}
			else
			{
				err = string.Format("'{0}' does not appear to be a valid email address.", Value);
				return  false;
			}
		}
	}
}
