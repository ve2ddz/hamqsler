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
	/// Base class for all Adif fields
	/// </summary>
	public class AdifField
	{
		public virtual string Name
		{
			get {return this.GetType().ToString().Substring("hamqsler.".Length);}
		}
		
		/// <summary>
		/// Constructor.
		/// Call validate after constructor because no validation is performed in constructor.
		/// </summary>
		public AdifField()
		{
		}
		
		/// <summary>
		/// Validate the field
		/// </summary>
		/// <returns>true if Value is not null</returns>
		/// <param name="err">Error message if Validate is false, or null</param>
		/// <param name="modString">Message indicating what values were modified, or null</param>
		/// <returns>true if Value is not null</returns>
		public virtual bool Validate(out string err, out string modString)
		{
			err = null;
			modString = null;
			return true;
		}
		
	}
}
