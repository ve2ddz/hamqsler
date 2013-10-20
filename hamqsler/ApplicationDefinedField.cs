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
		
		/// <summary>
		/// Constructor
		/// Note: no validation of input is performed in the constructor. Call Validate after
		/// the constructor and when changing values.
		/// </summary>
		/// <param name="fldName">Name of application defined field</param>
		/// <param name="dType">Data type of the field</param>
		/// <param name="value">field value</param>
		public ApplicationDefinedField(string fldName, string dType, string value,
		                              AdifEnumerations aEnums)
			: base(value)
		{
			fieldName = fldName;
			DataType = new EnumerationValue(dType, "DataType", aEnums);
		}
		
		/// <summary>
		/// Validate the field values:
		/// 1. Field name must be of format APP_{PROGRAMNAME}_{FIELDNAME}. PROGRAMNAME and
		/// FIELDNAME are not checked.
		/// 2. Datatype must be in the enumeration of data types defined in 
		/// </summary>
		/// <param name="err">Error message if values are not valid</param>
		/// <param name="modStr">Message if value has been modified</param>
		/// <returns>true if values are valid, false otherwise</returns>
		public override bool Validate(out string err, out string modStr)
		{
			err = null;
			modStr = null;
			string[] appParts = Name.Split('_');
			if(appParts.Length != 3 || appParts[1].Equals(string.Empty) ||
			   appParts[2].Equals(string.Empty) ||
			   !appParts[0].ToUpper().Equals("APP"))
			{
				err = "Invalid Application Defined Fieldname.";
				return false;
			}
			if(!DataType.Validate(out err, out modStr))
			{
				err = "Invalid Data Type.";
				return false;
			}
			switch(DataType.Value)
			{
				case "A":			// AwardList type
					string[] awards = Value.Split(',');
					string[] awardsList = Value.Split(',');
					for(int i = 0; i < awards.Length; i++)
					{
						if(!DataType.aEnums.IsInEnumeration("Award", awards[i]))
						{
							modStr = "Invalid AwardList item: '" + awards[i] + "'. Item removed.";
							awardsList[i] = null;
						}
					}
					Value = string.Empty;
					foreach(string award in awardsList)
					{
						if(award != null)
						{
							Value += award + ",";
						}
					}
					if(Value != string.Empty)
					{
						Value = Value.Substring(0, Value.Length - 1);
					}
					break;
				case "B":			// Boolean type
					BooleanField bf = new BooleanField(Value);
					if(!bf.Validate(out err, out modStr))
					{
						err = "Invalid Boolean Value: '" + Value + "'.";
						return false;
					}
					break;
				case "C":			// CreditList type
					string[] credits = Value.Split(',');
					string[] creditList = Value.Split(',');
					for(int i = 0; i < credits.Length; i++)
					{
						if(!DataType.aEnums.IsInEnumeration("Credit", credits[i]))
						{
							modStr = "Invalid CreditList item: '" + credits[i] + "'.";
							creditList[i] = null;
						}
					}
					Value = string.Empty;
					foreach(string credit in creditList)
					{
						if(credit != null)
						{
							Value += credit + ",";
						}
					}
					if(Value != string.Empty)
					{
						Value = Value.Substring(0, Value.Length - 1);
					}
					break;
				case "D":		// Date type
					DateField df = new DateField(Value);
					if(!df.Validate(out err, out modStr))
					{
						return false;
					}
					break;
				case "L":		// Location type
					Location loc = new Location(Value);
					if(!loc.Validate(out err, out modStr))
					{
						err = "Invalid location: '" + Value + "'.";
						return false;
					}
					break;
				case "M":		// multiline string type
					MultilineStringField sf = new MultilineStringField(Value);
					if(!sf.Validate(out err, out modStr))
					{
						err = "Invalid multiline string: '" + Value + "'.";
						return false;
					}
					break;
				case "N":		// number type
					NumberField nf = new NumberField(Value);
					if(!nf.Validate(out err, out modStr))
					{
						err = "Invalid number: '" + Value + "'.";
						return false;
					}
					break;
				case "P":		// sponsored award list type
					awards = Value.Split(',');
					string[] awardList = Value.Split(',');
					for(int i = 0; i < awardList.Length; i++)
					{
						string gError = string.Empty;
						string gMod = string.Empty;
						Award_Granted granted = new Award_Granted(awards[i], DataType.aEnums);
						if(!granted.Validate(out gError, out gMod))
						{
							err += gError + Environment.NewLine;
						}
						if(gMod != null)
						{
							modStr += gMod + Environment.NewLine;
						}
					}
					return err == null;
				case "S":		// string type
					StringField str = new StringField(Value);
					if(!str.Validate(out err, out modStr))
					{
						return false;
					}
					break;
				case "T":		// time type
					TimeField tf = new TimeField(Value);
					if(!tf.Validate(out err, out modStr))
					{
						return false;
					}
					break;
				default:
					err = "Invalid data type: '" + DataType.Value + "'.";
					return false;
			}
			return true;
		}
	}
}
