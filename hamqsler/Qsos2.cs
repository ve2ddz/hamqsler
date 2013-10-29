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
		
		private AdifEnumerations adifEnums;
		public AdifEnumerations AdifEnums
		{
			get {return adifEnums;}
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		public Qsos2() : base(new QsoEqualityComparer())
		{
		}
		
		/// <summary>
		/// Import an ADIF string after clearing existing QSOs
		/// </summary>
		/// <param name="adif">string containing ADIF file contents (header and QSOs)</param>
		/// <param name="error">string containing error and modification messages, null otherwise</param>
		/// <param name="aEnums">AdifEnumerations object containing the ADIF enumerations</param>
		/// <returns>true if no errors, false otherwise</returns>
		public bool Import(string adif, ref string error, AdifEnumerations aEnums)
		{
			adifEnums = aEnums;
			this.ClearQsos();
			return Add(adif, ref error);
		}
		
		/// <summary>
		/// Clear all QSOs and Userdefs
		/// </summary>
		public void ClearQsos()
		{
			userDefs.Clear();
			this.Clear();
		}
		
		/// <summary>
		/// Add QSOs
		/// </summary>
		/// <param name="adif">string containing ADIF file contents (header and QSOs)</param>
		/// <param name="error">string containing error and modification messages, null otherwise</param>
		/// <param name="aEnums">AdifEnumerations object containing the ADIF enumerations</param>
		/// <returns>true if no errors, false otherwise</returns>
		public bool Add(string adif, ref string error, AdifEnumerations aEnums = null)
		{
			if(aEnums != null)
			{
				adifEnums = aEnums;
			}
			error = null;
			string origAdif = adif;
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
				bool headerOK = GetUserDefs(header, adifEnums, ref error);
				if(error != null)
				{
					error = header + error;
				}
				adif = adif.Substring(eoh + 5);
			}
			eor = adif.ToUpper().IndexOf("<EOR>");
			char[] trimChars = {'\r', '\n'};
			string adifQso = string.Empty;
			while(eor != -1)
			{
				bool qsoAddedToError = false;
				int start = adif.IndexOf("<");
				adifQso = adif.Substring(start, eor - start);
				string err = string.Empty;
				Qso2 qso = new Qso2(adif, adifEnums, ref err);
				if(err != null)
				{
					error += adifQso.Trim(trimChars) + "<eor>" +
						Environment.NewLine + err + Environment.NewLine;
					qsoAddedToError = true;
					
				}
				string valError = string.Empty;
				bool val = qso.Validate(ref valError);
				if(val)
				{
					// the following code assumes that the AdifField objects in the Qso
					// only have to be checked for modification once, and that any
					// newly generated AdifFields don't have to be checked.
					List<AdifField> fields = new List<AdifField>();
					foreach(AdifField field in qso.Fields)
					{
						fields.Add(field);
					}
					foreach(AdifField field in fields)
					{
						AdifField f = qso.GetField(field.Name);
						string modString = f.ModifyValues(qso);
						if(modString != null)
						{
							if(!qsoAddedToError)
							{
								error += adifQso + "<eor>" + Environment.NewLine;
							}
							error += modString + Environment.NewLine + Environment.NewLine;
						}
					}
					this.Add(qso);
				}
				else
				{
					if(!qsoAddedToError)
					{
						error += adifQso + "<eor>" + Environment.NewLine;
					}
					error += valError + " - QSO not added." + Environment.NewLine;
				}
				adif = adif.Substring(eor + 5);
				eor = adif.ToUpper().IndexOf("<EOR>");
			}
			if(error != null)
			{
				error += Environment.NewLine;
			}
			string whiteChars = @"^[ \r\n\t]*$";
			if(!Regex.IsMatch(adif, whiteChars))
			{
				error += "Data found after end of last QSO record: '" + adif + "'" +
					Environment.NewLine + Environment.NewLine;
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
			bool headerError = false;
			string reg = @"^([A-Za-z_]+)";
			string enumReg = @",\{([A-Za-z0-9,]+)\}";
			string rangeReg = @"([0-9]+){0,1}:([0-9]+)}*\}{0,1}";
			AdifFields fields = new AdifFields(adif, ref err);
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
							err += "\tUser Defined Field: '" + match.Groups[1] +
								"' already defined. First definition retained.";
							headerError = true;
							alreadyDefined = true;
							break;
						}
					}
					if(!alreadyDefined)
					{
						if(fields.DataTypes[i].Equals(string.Empty))
						{
							err += "\tUser Defined Field: '" + fields.Values[i] +
								"' does not contain a data type. Field not added.";
							headerError = true;
							continue;
						}
						else if(!aEnums.IsInEnumeration("DataType", fields.DataTypes[i]))
						{
							err += "\tUser Defined Field: '" + fields.Values[i] +
								"' does not contain valid data type. Field not added.";
							headerError = true;
							continue;
						}
						Userdef udef;
						fields.DataTypes[i] = fields.DataTypes[i].ToUpper();
						switch(fields.DataTypes[i])
						{
							case "E":
								Match enumMatch = Regex.Match(fields.Values[i], enumReg);
								if(enumMatch.Groups[1].Value.Equals(string.Empty))
								{
									err += "\tUser Defined Field: '" + fields.Values[i] +
										"' is of type Enumeration, but no enumeration is supplied. " +
										"Field not added.";
									headerError = true;
									break;
								}
								string[] enums = enumMatch.Groups[1].Value.Split(',');
								udef = new Userdef(match.Groups[1].Value, "E", enums, aEnums);
								userDefs.Add(udef);
									break;
							case "N":
									Match rangeMatch = Regex.Match(fields.Values[i], rangeReg);
								if(!rangeMatch.Groups[1].Value.Equals(string.Empty) &&
								   !rangeMatch.Groups[2].Value.Equals(string.Empty))
								{
									udef = new Userdef(match.Groups[1].Value, "N", 
										                   rangeMatch.Groups[1].Value, rangeMatch.Groups[2].Value,
										                   aEnums);
									userDefs.Add(udef);
								}
								else
								{
									udef = new Userdef(match.Groups[1].Value, "N", aEnums);
									userDefs.Add(udef);
								}
								break;
							default:
								udef = new Userdef(match.Groups[1].Value, fields.DataTypes[i], aEnums);
								userDefs.Add(udef);
								break;
						}
					}
				}
			}
			return !headerError;
		}
		
		/// <summary>
		/// Convert this object to an ADIF string
		/// </summary>
		/// <returns>Adif string representing the contents of this Qsos2 object</returns>
		public string ToAdifString()
		{
			DateTime now = DateTime.Now.ToUniversalTime();
			string adif = string.Format("Generated on {0} at {1} UTC." +
			                            Environment.NewLine +
			                            "<adif_ver:{2}>{3}" +
			                            Environment.NewLine +
			                            "<programid:8>HamQSLer" +
			                            Environment.NewLine,
			                            now.ToLongDateString(), now.ToShortTimeString(),
			                            adifEnums.Version.Length,
			                       		adifEnums.Version);
			for(int i = 0; i < UserDefs.Count; i++)
			{
				adif += UserDefs[i].ToAdifString(i+1) + Environment.NewLine;
			}
			adif += "<eoh>" + Environment.NewLine;
			foreach(Qso2 qso in this)
			{
				adif += qso.ToAdifString() + Environment.NewLine;
			}
			return adif;
		}
	}
}
