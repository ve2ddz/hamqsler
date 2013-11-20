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
		private string eltValue = string.Empty;
		public virtual string Value
		{
			get {return eltValue;}
			set {eltValue = value;}
		}
		
		
		private EnumerationValue dataType = null;
		public EnumerationValue DataType
		{
			get {return dataType;}
			set {dataType = value;}
		}
		
		private EnumerationField enumField = null;
		public EnumerationField EnumField
		{
			get {return enumField;}
			set {enumField = value;}
		}
		
		private string lowerValue = string.Empty;
		public string LowerValue
		{
			get {return lowerValue;}
			set {lowerValue = value;}
		}
		private string upperValue = string.Empty;
		public string UpperValue
		{
			get {return upperValue;}
			set {upperValue = value;}
		}

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
		
		/// <summary>
		/// Create Adif record for this field
		/// </summary>
		/// <returns>Adif formatted field for this object</returns>
		public virtual string ToAdifString()
		{
			string adif = string.Empty;
			if(DataType != null && DataType.Value != string.Empty)
			{
				adif += string.Format("<{0}:{1}:{3}>{2}", Name, Value.Length, Value, 
				                      DataType.Value);
			}
			else
			{
				adif += string.Format("<{0}:{1}>{2}", Name, Value.Length, Value);
			}
			return adif;
		}
		
		/// <summary>
		/// check 2 AdifField object for equality
		/// </summary>
		/// <param name="obj">AdifField object to compare with this one</param>
		/// <returns>true if equal, false otherwise</returns>
		public override bool Equals(object obj)
		{
			AdifField f = obj as AdifField;
			if(f == null)
			{
				return false;
			}
			return this.ToAdifString().Equals(f.ToAdifString());
		}

		public override int GetHashCode()
		{
			return this.ToAdifString().GetHashCode();
		}

		/// <summary>
		/// Check value for this field and modify it or other fields in QSO if required
		/// </summary>
		/// <param name="qso">Qso2 object containing this field</param>
		/// <returns>string indicating changes made, or null if no changes</returns>
		public virtual string ModifyValues(Qso2 qso)
		{
			return null;
		}

		/// <summary>
		/// Check value for this field and modify it or other fields in QSO if required.
		/// Call this after call to ModifyValues for changes that should occur after
		/// the normal modifications
		/// </summary>
		/// <param name="qso">Qso2 object containing this field</param>
		/// <returns>string indicating changes made, or null if no changes</returns>
		public virtual string ModifyValues2(Qso2 qso)
		{
			return null;
		}
	}
}
