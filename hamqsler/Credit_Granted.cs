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
	/// Credit_Granted class - list of credits granted to this QSO
	/// </summary>
	public class Credit_Granted : CreditList
	{
		public override string Value 
		{
			get {return base.Value;}
		}
		
		/// <summary>
		/// Constructor
		/// Note: no validation of input is performed in the constructor. Call Validate after
		/// the constructor and when changing values.
		/// </summary>
		/// <param name="credits">Credits list</param>
		/// <param name="aEnums">AdifEnumerations object containing Award, Credit, and
		/// QSL_Medium enumerations</param>
		public Credit_Granted(string credits, AdifEnumerations aEnums)
			: base(credits, aEnums)
		{
		}
	}
}
