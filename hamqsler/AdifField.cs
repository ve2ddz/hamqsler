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
	/// Base class for all Adif fields
	/// </summary>
	public class AdifField
	{
		public string Name
		{
			get {return this.GetType().ToString().Substring("hamqsler.".Length);}
		}
		
		private string eltValue = string.Empty;
		public string Value
		{
			get {return eltValue;}
			set {eltValue = value;}
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="value">field value</param>
		public AdifField(string value)
		{
			Value = value;
		}
		
		/// <summary>
		/// Create ADIF field string from contents of this element
		/// </summary>
		/// <returns>ADIF field string</returns>
		public virtual string ToAdifString()
		{
			return "<" + Name + ":" + Value.Length + ">" + Value;
		}
		
		/// <summary>
		/// Validate the field
		/// </summary>
		/// <returns>true if Value is not null</returns>
		/// <param name="err">Error message if Validate is false, or null</param>
		/// <returns>true if Value is not null</returns>
		public virtual bool Validate(out string err)
		{
			err = null;
			if(Value == null)
			{
				err = "Value is null";
				return false;
			}
			return true;
		}
		
	}
}
