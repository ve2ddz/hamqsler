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
	/// ApplicationDefinedField class - application defined field
	/// </summary>
	public class ApplicationDefinedField : StringField
	{
		private string fieldName = string.Empty;
		public override string Name {
			get { return fieldName; }
		}
		
		private EnumerationValue dataType = null;
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="fldName">Name of application defined field</param>
		/// <param name="dType">Data type of the field</param>
		/// <param name="value">field value</param>
		public ApplicationDefinedField(string fldName, string dType, string value,
		                              AdifEnumerations aEnums)
			: base(value)
		{
			fieldName = fldName;
			dataType = new EnumerationValue(dType, "DataType", aEnums);
		}
		
		/// <summary>
		/// Output object in ADIF field format
		/// </summary>
		/// <returns>object in ADIF field format</returns>
		public override string ToAdifString()
		{
			return string.Format("<{0}:{1}:{2}>{3}", Name, Value.Length, dataType.Value, Value);
		}
		
		/// <summary>
		/// Validate the field values:
		/// 1. Field name must be of format APP_{PROGRAMNAME}_{FIELDNAME}. PROGRAMNAME and
		/// FIELDNAME are not checked.
		/// 2. Datatype must be in the enumeration of data types defined in 
		/// </summary>
		/// <param name="err">Error message if values are not valid</param>
		/// <returns>true if values are valid, false otherwise</returns>
		public override bool Validate(out string err)
		{
			err = null;
			string[] appParts = Name.Split('_');
			if(appParts.Length != 3 || appParts[1].Equals(string.Empty) ||
			   appParts[2].Equals(string.Empty) ||
			   !appParts[0].ToUpper().Equals("APP"))
			{
				err = "Invalid Application Defined Fieldname.";
				return false;
			}
			if(!dataType.Validate(out err))
			{
				err = "Invalid Data Type.";
				return false;
			}
			switch(dataType.Value)
			{
				case "A":			// AwardList type
					string[] awards = Value.Split(',');
					foreach(string award in awards)
					{
						if(!dataType.aEnums.IsInEnumeration("Award", award))
						{
							err = "Invalid AwardList item: '" + award + "'.";
							return false;
						}
					}
					break;
				case "B":			// Boolean type
					BooleanField bf = new BooleanField(Value);
					if(!bf.Validate(out err))
					{
						err = "Invalid Boolean Value: '" + Value + "'.";
						return false;
					}
					break;
				case "C":			// CreditList type
					string[] credits = Value.Split(',');
					foreach(string credit in credits)
					{
						if(!dataType.aEnums.IsInEnumeration("Credit", credit))
						{
							err = "Invalid CreditList item: '" + credit + "'.";
							return false;
						}
					}
					break;
				case "D":		// Date type
					DateField df = new DateField(Value);
					if(!df.Validate(out err))
					{
						return false;
					}
					break;
				case "L":		// Location type
					Location loc = new Location(Value);
					if(!loc.Validate(out err))
					{
						err = "Invalid location: '" + Value + "'.";
						return false;
					}
					break;
				case "M":		// multiline string type
					MultilineStringField sf = new MultilineStringField(Value);
					if(!sf.Validate(out err))
					{
						err = "Invalid multiline string: '" + Value + "'.";
						return false;
					}
					break;
				case "N":		// number type
					NumberField nf = new NumberField(Value);
					if(!nf.Validate(out err))
					{
						err = "Invalid number: '" + Value + "'.";
						return false;
					}
					break;
				case "P":		// sponsored award list type
					// Award_Granted is sponsored award list type
					Award_Granted granted = new Award_Granted(Value, dataType.aEnums);
					if(!granted.Validate(out err))
					{
						err = "Invalid sponsored award item: '" + Value + "'.";
						return false;
					}
					break;
				case "S":		// string type
					StringField str = new StringField(Value);
					if(!str.Validate(out err))
					{
						return false;
					}
					break;
				case "T":		// time type
					TimeField tf = new TimeField(Value);
					if(!tf.Validate(out err))
					{
						return false;
					}
					break;
				default:
					err = "Invalid data type: '" + dataType.Value + "'.";
					return false;
			}
			return true;
		}
	}
}
