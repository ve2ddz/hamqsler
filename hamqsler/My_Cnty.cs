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
	/// My_Cnty class - the logging station's Secondary Administrative Subdivision
	/// (e.g. US County, JA Gun), in format specified in ADIF specification
	/// Note: Secondary Administrative Subdivisions are not enumerated in the ADIF specification.
	/// Therefore, no actual validity checking is performed.
	/// </summary>
	public class My_Cnty : StringField
	{
		public My_Cnty(string cnty) : base(cnty)
		{
		}
	}
}
