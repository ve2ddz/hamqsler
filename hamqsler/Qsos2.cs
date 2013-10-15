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
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace hamqsler
{
	/// <summary>
	/// Description of Qsos2.
	/// </summary>
	public class Qsos2 : HashSet<Qso2>
	{
		private List<Userdef> userDefs = new List<Userdef>();
		public List<Userdef> UserDefs
		{
			get {return userDefs;}
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		public Qsos2()
		{
		}
		
		/// <summary>
		/// Add QSOs
		/// </summary>
		/// <param name="adif">string containing ADIF file contents (header and QSOs)</param>
		/// <param name="error">string containing error and modification messages, null otherwise</param>
		/// <param name="aEnums">AdifEnumerations object containing the ADIF enumerations</param>
		/// <returns>true if no errors, false otherwise</returns>
		public bool Add(string adif, ref string error, AdifEnumerations aEnums)
		{
			error = null;
			if(adif == null || adif == string.Empty)
			{
				error = "No QSOs found." + Environment.NewLine;
				return false;
			}
			int eor = adif.ToUpper().IndexOf("<EOR>");
			if(eor == -1)
			{
				error = "No QSOs found." + Environment.NewLine;
				return false;
			}
			int eoh = adif.ToUpper().IndexOf("<EOH>");
			if(eoh != -1)
			{
				string header = adif.Substring(0, eoh);
				bool headerOK = GetUserDefs(header, aEnums, ref error);
				if(!headerOK)
				{
					return false;
				}
				adif = adif.Substring(eoh + 5);
			}
			eor = adif.ToUpper().IndexOf("<EOR>");
			while(eor != -1)
			{
				string adifQso = adif.Substring(0, eor);
				string err = string.Empty;
				Qso2 qso = new Qso2(adif, aEnums, ref err);
				if(err != null)
				{
					error += err;
				}
				string valError = string.Empty;
				bool val = qso.Validate(ref valError);
				if(val)
				{
					this.Add(qso);
				}
				else
				{
					error += valError;
				}
				adif = adif.Substring(eor + 5);
				eor = adif.ToUpper().IndexOf("<EOR>");
			}
			char[] trimChars = {' '};
			adif.Trim(trimChars);
			if(adif.Length > 0)
			{
				error += "Data found after end of last QSO record: " + adif + Environment.NewLine;
			}
			return true;
		}
		
		/// <summary>
		/// Get UserDef fields from header
		/// </summary>
		/// <param name="adif">ADIF string containing the header</param>
		/// <param name="err">Message if there is an error in UserDef fields</param>
		/// <returns>false if error retrieving UserDefs, true otherwise</returns>
		private bool GetUserDefs(string adif, AdifEnumerations aEnums, ref string err)
		{
			string reg = @"^([A-Za-z_]+)((,\{([A-Za-z0-9,]+)|([0-9]+\.{0,1}[0-9]}*):([0-9]+\.{0,1}[0-9]}*)\}){0,1})";
			AdifFields fields = new AdifFields(adif);
			for(int i = 0; i < fields.Count; i ++)
			{
				bool alreadyDefined = false;
				if(fields.FieldNames[i].ToUpper().StartsWith("USERDEF"))
			    {
					Match match = Regex.Match(fields.Values[i], reg);
					foreach(Userdef userdef in UserDefs)
					{
						if(userdef.UName.Equals(match.Groups[1].Value))
						{
							err += "User Defined Field: '" + match.Groups[1] +
								"' already defined. First definition retained." +
								Environment.NewLine;
							alreadyDefined = true;
							break;
						}
					}
					if(!alreadyDefined)
					{
						if(fields.DataTypes[i].Equals(string.Empty))
						{
							err += "User Defined Field: '" + fields.Values[i] +
								"' does not contain a data type. Field not added." +
								Environment.NewLine;
							continue;
						}
						else if(!aEnums.IsInEnumeration("DataType", fields.DataTypes[i]))
						{
							err += "User Defined Field: '" + fields.Values[i] +
								"' does not contain valid data type. Field not added." +
								Environment.NewLine;
							continue;
						}
						Userdef udef;
						switch(fields.DataTypes[i])
						{
							case "E":
								udef = new Userdef(match.Groups[1].Value, fields.DataTypes[i], aEnums);
								break;
							case "N":
								if(!match.Groups[5].Value.Equals(string.Empty) &&
								   !match.Groups[6].Value.Equals(string.Empty))
								{
									udef = new Userdef(match.Groups[1].Value, fields.DataTypes[i], aEnums);
								}
								else
								{
									udef = new Userdef(match.Groups[1].Value, fields.DataTypes[i], aEnums);
								}
								break;
							default:
								udef = new Userdef(match.Groups[1].Value, fields.DataTypes[i], aEnums);
								break;
						}
						userDefs.Add(udef);
					}
				}
			}
			return true;
		}
	}
}
