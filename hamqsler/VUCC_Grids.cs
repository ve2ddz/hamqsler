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
	/// VUCC_Grids class - Two or four adjacent Maidenhead grid locators, each 4 characters long,
	/// representing the contacted station's grid squares cardited to the QSO for the ARRL
	/// VUCC program (e.g. EN98,FM08,EM97,FM07)
	/// </summary>
	public class VUCC_Grids : AdifField
	{
		private DelimitedList gridSquares = null;
		
		public override string Value 
		{
			get { return gridSquares.ToString(); }
			set
			{
				string[] gs = value.Split(',');
				string grds = string.Empty;
				char[] trimChars = new char[1];
				trimChars[0] = ' ';
				foreach(string grid in gs)
				{
					string gr = grid.Trim(trimChars);
					grds += gr + ",";
				}
				grds = grds.Substring(0, grds.Length - 1);
				
				gridSquares = new DelimitedList(',', grds);
			}
		}
		
		public int Count
		{
			get {return gridSquares.Count;}
		}
		
		/// <summary>
		/// Constructor
		/// Note: no validation of input is performed in the constructor. Call Validate after
		/// the constructor and when changing values.
		/// </summary>
		/// <param name="grids">list of two or four grid squares</param>
		public VUCC_Grids(string grids)
		{
			string[] gs = grids.Split(',');
			string grds = string.Empty;
			char[] trimChars = new char[1];
			trimChars[0] = ' ';
			foreach(string grid in gs)
			{
				string gr = grid.Trim(trimChars);
				grds += gr + ",";
			}
			grds = grds.Substring(0, grds.Length - 1);
			gridSquares = new DelimitedList(',', grds);
		}
		
		/// <summary>
		/// Get VUCC_Grids in Adif Field format
		/// </summary>
		/// <returns>VUCC_Grids in Adif Field format</returns>
		public override string ToAdifString()
		{
			string squares = gridSquares.ToString();
			return string.Format("<{0}:{1}>{2}", base.Name, squares.Length, squares);
		}
		
		/// <summary>
		/// Validate the grid squares
		/// </summary>
		/// <param name="err">Error message if one or more grid squares is not valid, null otherwise</param>
		/// <param name="modStr">Message if value has been modified (always null for this class)</param>
		/// <returns>true if 0, 2 or 4 grid squares and grid squares are in correct format</returns>
		public override bool Validate(out string err, out string modStr)
		{
			err = null;
			modStr = null;
			if(!(Count == 2) && !(Count == 4) && !(Count == 0))
			{
				err = string.Format("\t{1} contains {0} grid squares. Must contain either 2 or 4 grid squares.",
				                    Count, base.Name);
				return false;
			}
			foreach(string grid in gridSquares.Items)
			{
				if(!Regex.IsMatch(grid, "^[A-Ra-r][A-Ra-r][0-9][0-9]$"))
				{
					err = string.Format("\t'{0}' is not a valid {1} grid square",
					                    grid, base.Name);
					return false;
				}
			}
			return true;
		}
	}
}
