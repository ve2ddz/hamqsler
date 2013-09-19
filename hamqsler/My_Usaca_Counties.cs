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
	/// My_Usaca_Counties class - two US counties in the case where the logging station is
	/// located on a border between two counties, representing counties that the contacted
	/// station may claim for the CQ Magazine USA-CA award program
	/// (e.g. MA,Franklin:MA,Hampshire)
	/// </summary>
	public class My_Usaca_Counties : Usaca_Counties
	{
		public My_Usaca_Counties(string counties) : base(counties)
		{
		}
	}
}
