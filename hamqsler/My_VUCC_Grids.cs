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
	/// My_VUCC_Grids class - two or four adjacent Maidenhead grid locators, each four
	/// characters log, representing the logging station's grid squares that the contacted
	/// station may claim for the ARRL VUCC award program (e.g EN98,FM08,EM97,FM07)
	/// </summary>
	public class My_VUCC_Grids : VUCC_Grids
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="grids">list of grid squares</param>
		public My_VUCC_Grids(string grids) : base(grids)
		{
		}
	}
}
