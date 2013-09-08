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
using System.IO;
using System.Reflection;
using System.Text;
using System.Xml;
using NUnit.Framework;
using hamqsler;

namespace hamqslerTest
{
	/// <summary>
	/// Tests for AdifEnumerations class
	/// </summary>
	[TestFixture]
	public class AdifEnumerationsTest
	{
		// test constructor and get version
		[Test]
		public void TestVersion()
		{
			string adifEn = "<?xml version='1.0'?>" + Environment.NewLine +
							"<AdifEnumerations Version='3.0.4.0'>" + Environment.NewLine +
							"" + Environment.NewLine +
							"</AdifEnumerations>" + Environment.NewLine;
			MemoryStream str = new MemoryStream(Encoding.ASCII.GetBytes(adifEn));
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Assert.AreEqual("3.0.4.0", aEnums.Version);
		}
		
		// test null version
		[Test]
		public void TestNullVersion()
		{
			string adifEn = "<?xml version='1.0'?>" + Environment.NewLine +
							"<AdifEnumerations>" + Environment.NewLine +
							"" + Environment.NewLine +
							"</AdifEnumerations>" + Environment.NewLine;
			MemoryStream str = new MemoryStream(Encoding.ASCII.GetBytes(adifEn));
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Assert.IsNull(aEnums.Version);
		}
		
		// test loading of AdifEnumerations.xml
		[Test]
		public void TestLoadingAdifEnumerationsXml()
		{
		    // get the hamqsler assembly
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            // get a stream for the AdifEnumerations.xml file
            // TODO: This is currently an embedded resource in the assembly, but needs to be moved to AppData
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
             // load in the xml file
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Assert.IsNotNull(aEnums.Version);
		}
		
		// test Ant_Path Enumeration
		[Test]
		public void TestAntPathEnumG()
		{
		    // get the hamqsler assembly
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            // get a stream for the AdifEnumerations.xml file
            // TODO: This is currently an embedded resource in the assembly, but needs to be moved to AppData
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
             // load in the xml file
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Assert.IsTrue(aEnums.IsInEnumeration("Ant_Path", "G"));
		}

		// test Ant_Path Enumeration
		[Test]
		public void TestAntPathEnumO()
		{
		    // get the hamqsler assembly
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            // get a stream for the AdifEnumerations.xml file
            // TODO: This is currently an embedded resource in the assembly, but needs to be moved to AppData
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
             // load in the xml file
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Assert.IsTrue(aEnums.IsInEnumeration("Ant_Path", "O"));
		}

		// test Ant_Path Enumeration
		[Test]
		public void TestAntPathEnumS()
		{
		    // get the hamqsler assembly
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            // get a stream for the AdifEnumerations.xml file
            // TODO: This is currently an embedded resource in the assembly, but needs to be moved to AppData
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
             // load in the xml file
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Assert.IsTrue(aEnums.IsInEnumeration("Ant_Path", "S"));
		}

		// test Ant_Path Enumeration
		[Test]
		public void TestAntPathEnumL()
		{
		    // get the hamqsler assembly
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            // get a stream for the AdifEnumerations.xml file
            // TODO: This is currently an embedded resource in the assembly, but needs to be moved to AppData
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
             // load in the xml file
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Assert.IsTrue(aEnums.IsInEnumeration("Ant_Path", "L"));
		}

		// test Ant_Path Enumeration with non value
		[Test]
		public void TestAntPathEnumNoMatch()
		{
		    // get the hamqsler assembly
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            // get a stream for the AdifEnumerations.xml file
            // TODO: This is currently an embedded resource in the assembly, but needs to be moved to AppData
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
             // load in the xml file
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Assert.IsFalse(aEnums.IsInEnumeration("Ant_Path", "Q"));
		}
		
		// test GetDescription using Ant_Path enumeration
		[Test]
		public void TestGetDescriptionWithEnum()
		{
		    // get the hamqsler assembly
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            // get a stream for the AdifEnumerations.xml file
            // TODO: This is currently an embedded resource in the assembly, but needs to be moved to AppData
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
             // load in the xml file
			AdifEnumerations aEnums = new AdifEnumerations(str);
			string desc = aEnums.GetDescription("Ant_Path", "O");
			Assert.IsNotNull(desc);
			Assert.AreEqual("other", desc);
		}


		// test GetDescription with unmatched enumeration value using Ant_Path enumeration
		[Test]
		public void TestGetDescriptionWithBadEnum()
		{
		    // get the hamqsler assembly
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            // get a stream for the AdifEnumerations.xml file
            // TODO: This is currently an embedded resource in the assembly, but needs to be moved to AppData
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
             // load in the xml file
			AdifEnumerations aEnums = new AdifEnumerations(str);
			string desc = aEnums.GetDescription("Ant_Path", "X");
			Assert.IsNull(desc);
		}
		
		// test GetEnumeratedValues using Ant_Path
		[Test]
		public void TestGetEnumeratedValues()
		{
			// get the hamqsler assembly
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            // get a stream for the AdifEnumerations.xml file
            // TODO: This is currently an embedded resource in the assembly, but needs to be moved to AppData
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
             // load in the xml file
			AdifEnumerations aEnums = new AdifEnumerations(str);
			string[] values = aEnums.GetEnumeratedValues("Ant_Path");
			string[] testValues = {"G", "O", "S", "L"};
			for(int i = 0; i < values.Length; i++)
			{
				Assert.AreEqual(testValues[i], values[i]);
			}
		}
		
		// test GetEnumeratedValues using non-existent enumeration
		[Test]
		[ExpectedException(typeof(XmlException))]
		public void TestGetEnumeratedValuesWithNoEnumeration()
		{
			// get the hamqsler assembly
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            // get a stream for the AdifEnumerations.xml file
            // TODO: This is currently an embedded resource in the assembly, but needs to be moved to AppData
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
             // load in the xml file
			AdifEnumerations aEnums = new AdifEnumerations(str);
			string[] values = aEnums.GetEnumeratedValues("Ant_Path2");
		}
		
		// test if enumeration value is deprecated
		[Test]
		public void TestIsDeprecated()
		{
			// get the hamqsler assembly
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            // get a stream for the AdifEnumerations.xml file
            // TODO: This is currently an embedded resource in the assembly, but needs to be moved to AppData
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
             // load in the xml file
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Assert.IsTrue(aEnums.IsDeprecated("Arrl_Section", "NWT"));
		}
			
		// test if enumeration value is not deprecated
		[Test]
		public void TestIsNotDeprecated()
		{
			// get the hamqsler assembly
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            // get a stream for the AdifEnumerations.xml file
            // TODO: This is currently an embedded resource in the assembly, but needs to be moved to AppData
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
             // load in the xml file
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Assert.IsFalse(aEnums.IsDeprecated("Arrl_Section", "NT"));
		}
		
		// test if IsDeprecated throws XmlException if value not found
		[Test]
		[ExpectedException(typeof(XmlException))]
		public void TestIsDeprecatedThrowsExceptionValueNotFound()
		{
			// get the hamqsler assembly
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            // get a stream for the AdifEnumerations.xml file
            // TODO: This is currently an embedded resource in the assembly, but needs to be moved to AppData
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
             // load in the xml file
			AdifEnumerations aEnums = new AdifEnumerations(str);
			aEnums.IsDeprecated("Arrl_Section", "NXTW");
		}
		
		// test if IsDeprecated throws XmlException if enumeration not found
		[Test]
		[ExpectedException(typeof(XmlException))]
		public void TestIfDeprecatedThrowsExceptionEnumerationNotFound()
		{
			// get the hamqsler assembly
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            // get a stream for the AdifEnumerations.xml file
            // TODO: This is currently an embedded resource in the assembly, but needs to be moved to AppData
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
             // load in the xml file
			AdifEnumerations aEnums = new AdifEnumerations(str);
			aEnums.IsDeprecated("Arrl2", "NT");
		}
		
		// test if enumeration value is deleted
		[Test]
		public void TestIsDeleted()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Assert.IsTrue(aEnums.IsDeleted("Country_Code", "8"));
		}
		
		// test if enumeration value is deleted for not deleted value
		[Test]
		public void TestIsDeletedNotDeletedValue()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Assert.IsFalse(aEnums.IsDeleted("Country_Code", "1"));
		}
		
		// test if IsDeleted throws XmlException if enumeration not found
		[Test]
		[ExpectedException(typeof(XmlException))]
		public void TestIfIsDeletedThrowsExceptionEnumerationNotFound()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			aEnums.IsDeleted("Country_Code", "1026");
		}
		
		// test GetReplacementValue when IsDeprecated and replacement value exists
		[Test]
		public void TestGetReplacementValueForValidReplacement()
		{
			// get the hamqsler assembly
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            // get a stream for the AdifEnumerations.xml file
            // TODO: This is currently an embedded resource in the assembly, but needs to be moved to AppData
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
             // load in the xml file
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Assert.AreEqual("NT", aEnums.GetReplacementValue("Arrl_Section", "NWT"));
		}
		
		// test GetReplacementValue when IsDeprecated not set
		[Test]
		public void TestGetReplacementValueNoReplacement()
		{
			// get the hamqsler assembly
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            // get a stream for the AdifEnumerations.xml file
            // TODO: This is currently an embedded resource in the assembly, but needs to be moved to AppData
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
             // load in the xml file
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Assert.IsNull(aEnums.GetReplacementValue("Arrl_Section", "ON"));
		}
			
		// test GetBandLimits for valid band
		[Test]
		public void TestGetBandLimits2190m()
		{
			// get the hamqsler assembly
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            // get a stream for the AdifEnumerations.xml file
            // TODO: This is currently an embedded resource in the assembly, but needs to be moved to AppData
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
             // load in the xml file
			AdifEnumerations aEnums = new AdifEnumerations(str);
			string lowerLimit = string.Empty;
			string upperLimit = string.Empty;
			Assert.IsTrue(aEnums.GetBandLimits("2190m", out lowerLimit, out upperLimit));
			Assert.AreEqual(".136", lowerLimit);
			Assert.AreEqual(".137", upperLimit);
		}

		// test GetBandLimits for valid band
		[Test]
		public void TestGetBandLimits1mm()
		{
			// get the hamqsler assembly
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            // get a stream for the AdifEnumerations.xml file
            // TODO: This is currently an embedded resource in the assembly, but needs to be moved to AppData
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
             // load in the xml file
			AdifEnumerations aEnums = new AdifEnumerations(str);
			string lowerLimit = string.Empty;
			string upperLimit = string.Empty;
			Assert.IsTrue(aEnums.GetBandLimits("1mm", out lowerLimit, out upperLimit));
			Assert.AreEqual("241000", lowerLimit);
			Assert.AreEqual("250000", upperLimit);
		}
		
		// test GetBandLimits for invalid band
		[Test]
		public void TestGetBandLimitsBadBand()
		{
			// get the hamqsler assembly
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            // get a stream for the AdifEnumerations.xml file
            // TODO: This is currently an embedded resource in the assembly, but needs to be moved to AppData
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
             // load in the xml file
			AdifEnumerations aEnums = new AdifEnumerations(str);
			string lowerLimit = string.Empty;
			string upperLimit = string.Empty;
			Assert.IsFalse(aEnums.GetBandLimits("11mm", out lowerLimit, out upperLimit));
		}
		
		// test GetBandFromFrequency for valid band
		[Test]
		public void TestGetBandFromFrequency4m()
		{
			// get the hamqsler assembly
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            // get a stream for the AdifEnumerations.xml file
            // TODO: This is currently an embedded resource in the assembly, but needs to be moved to AppData
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
             // load in the xml file
			AdifEnumerations aEnums = new AdifEnumerations(str);
			string band = string.Empty;
			Assert.IsTrue(aEnums.GetBandFromFrequency("70.58", out band));
			Assert.AreEqual("4m", band);
		}
		
		// test GetBandFromFrequency for valid band with freq at lower edge
		[Test]
		public void TestGetBandFromFrequencyLowerEdge()
		{
			// get the hamqsler assembly
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            // get a stream for the AdifEnumerations.xml file
            // TODO: This is currently an embedded resource in the assembly, but needs to be moved to AppData
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
             // load in the xml file
			AdifEnumerations aEnums = new AdifEnumerations(str);
			string band = string.Empty;
			Assert.IsTrue(aEnums.GetBandFromFrequency("7", out band));
			Assert.AreEqual("40m", band);
		}
		
		// test GetBandFromFrequency for valid band with freq at upper edge
		[Test]
		public void TestGetBandFromFrequencyUpperEdge()
		{
			// get the hamqsler assembly
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            // get a stream for the AdifEnumerations.xml file
            // TODO: This is currently an embedded resource in the assembly, but needs to be moved to AppData
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
             // load in the xml file
			AdifEnumerations aEnums = new AdifEnumerations(str);
			string band = string.Empty;
			Assert.IsTrue(aEnums.GetBandFromFrequency(".479", out band));
			Assert.AreEqual("630m", band);
		}

		// test GetBandFromFrequency for frequency not in band
		[Test]
		public void TestGetBandFromFrequencyInvalidFreq()
		{
			// get the hamqsler assembly
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            // get a stream for the AdifEnumerations.xml file
            // TODO: This is currently an embedded resource in the assembly, but needs to be moved to AppData
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
             // load in the xml file
			AdifEnumerations aEnums = new AdifEnumerations(str);
			string band = string.Empty;
			Assert.IsFalse(aEnums.GetBandFromFrequency(".485", out band));
		}

		// test IsInEnumeration for Country Code
		[Test]
		public void TestIsInCountryCodeEnumeration()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Assert.IsTrue(aEnums.IsInEnumeration("Country_Code", "26"));
		}

		// test IsInEnumeration for Country Code invalid code
		[Test]
		public void TestIsNotInCountryCodeEnumeration()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Assert.IsFalse(aEnums.IsInEnumeration("Country_Code", "73"));
		}

		// test GetDescription for Country Code
		[Test]
		public void TestGetDesscriptionCountryCode()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Assert.AreEqual("BRITISH SOMALI", aEnums.GetDescription("Country_Code", "26"));
		}

		// test GetDescription for Country Code with '&' in description
		[Test]
		public void TestGetDescriptionCountryCodeAmpersand()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Assert.AreEqual("AUCKLAND & CAMPBELL", aEnums.GetDescription("Country_Code", "16"));
		}

		// test GetDescription for Country Code invalid code
		[Test]
		public void TestGetDesscriptionInvalidCountryCode()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Assert.AreEqual(null, aEnums.GetDescription("Country_Code", "73"));
		}
		
		// test GetCountryCodeFromName with valid name
		[Test]
		public void TestGetCountryCodeFromValidName()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			string code = string.Empty;
			bool found = aEnums.GetCountryCodeFromName("BRITISH SOMALI", out code);
			Assert.IsTrue(found);
			Assert.AreEqual("26", code);
		}
		
		// test GetCountryCodeFromName with valid name containing '&'
		[Test]
		public void TestGetCountryCodeFromValidNameAmpersand()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			string code = string.Empty;
			bool found = aEnums.GetCountryCodeFromName("AUCKLAND & CAMPBELL", out code);
			Assert.IsTrue(found);
			Assert.AreEqual("16", code);
		}
		
		// test GetCountryCodeFromName with invalid name
		[Test]
		public void TestGetCountryCodeFromInValidName()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			string code = string.Empty;
			bool found = aEnums.GetCountryCodeFromName("BOOGALOO", out code);
			Assert.IsFalse(found);
			Assert.AreEqual(string.Empty, code);
		}
	}
}
