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
	/// GridSquare class - the contacted station's 2-character, 4-character, 6-character, or
	/// 8-character Maidenhead Grid Square
	/// </summary>
	public class GridSquare : StringField
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="square">grid square</param>
		public GridSquare(string square) : base(square)
		{
		}
		
		/// <summary>
		/// Validate the value of the grid square
		/// </summary>
		/// <param name="err">Error message if grid square is invalid, null otherwise</param>
		/// <returns>true if valid, false otherwise</returns>
		public override bool Validate(out string err)
		{
			err = null;
			int len = Value.Length;
			if(len == 2 && Regex.IsMatch(Value.Substring(0, 2), "[A-Ra-r][A-Ra-r]"))
		    {
				return true;
			}
			else if(len == 4 && Regex.IsMatch(Value, "[A-Ra-r][A-Ra-r][0-9][0-9]"))
			{
				return true;
			}
			else if(len == 6 && Regex.IsMatch(Value, "[A-Ra-r][A-Ra-r][0-9][0-9][A-Xa-x][A-Xa-x]"))
			{
				return true;
			}
			else if(len == 8 && Regex.IsMatch(Value, "[A-Ra-r][A-Ra-r][0-9][0-9][A-Xa-x][A-Xa-x][0-9][0-9]"))
			{
				return true;
			}
			err = string.Format("'{0}' is not a valid grid square.", Value);
			return false;
		}
	}
}
