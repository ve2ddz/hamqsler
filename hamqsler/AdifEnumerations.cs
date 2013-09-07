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
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace hamqsler
{
	/// <summary>
	/// Container for AdifEnumerations. These are read in from AdifEnumerations.xml
	/// </summary>
	public class AdifEnumerations
	{
		XDocument AdifEnumerationsDoc = null;
		
		public string Version
		{
			get 
			{
				XAttribute version = AdifEnumerationsDoc.Root.Attribute("Version");
				if(version != null)
				{
					return version.Value;
				}
				else
				{
					return null;
				}
			}
		}
		
		/// <summary>
		/// Default constructor 
		/// </summary>
		public AdifEnumerations(Stream adifStream)
		{
            AdifEnumerationsDoc = XDocument.Load(adifStream);
		}
		
		/// <summary>
		/// Check if value is within the specified enumeration
		/// </summary>
		/// <param name="enumeration">Enumeration to check</param>
		/// <param name="value">Value to check</param>
		/// <exception cref="XmlException">XmlException if enumeration is not found in the document</exception>
		/// <returns>true if within value is in enumeration, false otherwise</returns>
		public bool IsInEnumeration(string enumeration, string value)
		{
			List<XElement> elts = AdifEnumerationsDoc.Descendants(enumeration).ToList();
			
			if(elts.Count == 0)
			{
				string err = string.Format("Programming error: Attempting to retrieve {0} enumeration from " +
				              "AdifEnumerations but not found", enumeration);
				throw new XmlException(err);
			}
			IEnumerable<XElement> enums = elts[0].Descendants("EnumValue");
			foreach(XElement elt in enums)
			{
				if(value.Equals(elt.Value))
				{
					return true;
				}
			}
			return false;
		}
		
		/// <summary>
		/// Retrieve the description for a value in an enumeration. Use this only for enumerations that
		/// do not have children. E.G. use for Ant_Path enumeration, but not for Band enumeration
		/// </summary>
		/// <param name="enumeration">Enumeration to search</param>
		/// <param name="value">Value in the enumeration</param>
		/// <returns>Value of Meaning attribute for the value, null if no attribute</returns>
		public string GetDescription(string enumeration, string value)
		{
			List<XElement> elts = AdifEnumerationsDoc.Descendants(enumeration).ToList();
			IEnumerable<XElement> enums = elts[0].Descendants("EnumValue");
			foreach(XElement elt in enums)
			{
				if(value.Equals(elt.Value))
				{
					return elt.Attribute("Description").Value.ToString();
				}
			}
			return null;
		}
		
		/// <summary>
		/// Get all enumerated values for an enumeration
		/// </summary>
		/// <param name="enumeration">Enumeration to retrieve values for</param>
		/// <exception cref="XmlException">XmlException thrown if the enumeration is not included
		/// in the Enumeration file</exception>
		/// <returns>an array of strings containing the retrieved values</returns>
		public string[] GetEnumeratedValues(string enumeration)
		{
			IEnumerable<XElement> elts = AdifEnumerationsDoc.Descendants(enumeration);
			if(elts.Count() > 0)
			{
				var vals = from val in elts.Descendants("EnumValue") select val;
				string[] values = new string[vals.Count()];
				int i = 0;
				foreach(var val in vals)
				{
					values[i++] = (string)val;
					System.Diagnostics.Debug.WriteLine(string.Format("{0}: {1}", i, val));
				}
				return values;
			}
			else
			{
				string err = string.Format("Programming Error: {0} enumeration not found in AdifEnumerations",
				                           enumeration);
				throw new XmlException(err);
			}
		}
		
		/// <summary>
		/// Helper method that returns the XElement with EnumValue equal to value in enumeration
		/// </summary>
		/// <param name="enumeration">Enumeration</param>
		/// <param name="value">Value for the EnumValue</param>
		/// <exception cref="XmlException">XmlException thrown if the enumeration is not included
		/// in the Enumeration file</exception>
		/// <returns>EnumValue XElement for value or null</returns>
		private XElement GetEnumValue(string enumeration, string value)
		{
			IEnumerable<XElement> elts = AdifEnumerationsDoc.Descendants(enumeration);
			if(elts.Count() > 0)
			{
				var vals = from val in elts.Descendants("EnumValue") select val;
				foreach(var val in vals)
				{
					if(val.Value.Equals(value))
					{
						return val;
					}
				}
				return null;
			}
			else
			{
				string err = string.Format("Error: Enumeration {0} not found in Enumeration file",
				                          enumeration);
				throw new XmlException(err);
			}
			
		}
		
		/// <summary>
		/// Check if the EnumValue for value contains Deprecated attribute with value 'Yes'
		/// </summary>
		/// <param name="enumeration">Enumeration in the AdifEnumerations document</param>
		/// <param name="value">EnumValue to check for</param>
		/// <exception cref="XmlException">XmlException thrown if the enumeration is not included
		/// in the Enumeration file, or if the value is not found in the enumeration in the file.</exception>
		/// <returns>true if Deprecated='Yes' found, false otherwise</returns>
		public bool IsDeprecated(string enumeration, string value)
		{
			XElement val = GetEnumValue(enumeration, value);
			if(val != null)
			{
				XAttribute deprecated = val.Attribute("Deprecated");
				if(deprecated != null)
				{
					return deprecated.Value.Equals("Yes");
				}
				return false;
			}
			else
			{
				string err = string.Format("Error: {0} not found in {1} enumeration in Enumeration file",
				                         value, enumeration);
				throw new XmlException(err);
			}
		}
		
		/// <summary>
		/// Get replacement value if value is deprecated
		/// </summary>
		/// <param name="enumeration">Enumeration in the AdifEnumerations document</param>
		/// <param name="value">EnumValue to check for</param>
		/// <exception cref="XmlException">XmlException thrown if the enumeration is not included
		/// in the Enumeration file, or if the value is not found in the enumeration in the file.</exception>
		/// <returns>Replacement value if there is one, or null if not.</returns>
		public string GetReplacementValue(string enumeration, string value)
		{
			XElement val = GetEnumValue(enumeration, value);
			if(val != null)
			{
				XAttribute replaceWith = val.Attribute("ReplaceWith");
				if(replaceWith != null)
				{
					return replaceWith.Value;
				}
				else
				{
					return null;
				}
			}
			else
			{
				string err = string.Format("Error: {0} not found in {1} enumeration in Enumeration file",
				                         value, enumeration);
				throw new XmlException(err);
			}
		}
		
		/// <summary>
		/// Get the lower and upper band limits for the specified band
		/// </summary>
		/// <param name="band">Band to get limits for</param>
		/// <param name="lLimit">lower band limit</param>
		/// <param name="uLimit">upper band limit</param>
		/// <returns>true if band exists, false otherwise</returns>
		public bool GetBandLimits(string band, out string lLimit, out string uLimit)
		{
			lLimit = string.Empty;
			uLimit = string.Empty;
			XElement b = GetEnumValue("Band", band);
			if(b != null)
			{
				XAttribute lower = b.Attribute("LowerFreq");
				XAttribute upper = b.Attribute("UpperFreq");
				lLimit = lower.Value;
				uLimit = upper.Value;
				return true;
			}
			return false;
		}
		
		/// <summary>
		/// Get band for the specified frequency
		/// </summary>
		/// <param name="freq">Frequency to get band for</param>
		/// <param name="band">Band containing the frequency</param>
		/// <returns>true if freq is within a band, false otherwise.</returns>
		public bool GetBandFromFrequency(string freq, out string band)
		{
			band = string.Empty;
			List<XElement> elts = AdifEnumerationsDoc.Descendants("Band").ToList();
			IEnumerable<XElement> bands = elts.Descendants("EnumValue");
			float f = float.Parse(freq, CultureInfo.InvariantCulture);
			foreach(XElement b in bands)
			{
				string lower = b.Attribute("LowerFreq").Value;
				string upper = b.Attribute("UpperFreq").Value;
				float lFreq = float.Parse(lower, CultureInfo.InvariantCulture);
				float uFreq = float.Parse(upper, CultureInfo.InvariantCulture);
				if(f >= lFreq && f <= uFreq)
				{
					band = b.Value;
					return true;
				}
			}
			return false;
		}
		
		/// <summary>
		/// Get country code given country name
		/// </summary>
		/// <param name="name">country name</param>
		/// <param name="code">code for the named country</param>
		/// <returns>true if country found, false otherwise</returns>
		public bool GetCountryCodeFromName(string name, out string code)
		{
			code = string.Empty;
			List<XElement> elts = AdifEnumerationsDoc.Descendants("Country_Code").ToList();
			IEnumerable<XElement> codes = elts.Descendants("EnumValue");
			foreach(XElement c in codes)
			{
				string desc = c.Attribute("Description").Value;
				if(desc.Equals(name))
				{
					code = c.Value;
					return true;
				}
			}
			return false;
		}
	}
}
