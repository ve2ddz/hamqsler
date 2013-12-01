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
		// TestFixtureSetup
		[TestFixtureSetUp]
		public void TestSepup()
		{
			App.AdifEnums.LoadDocument();
		}
		
		// test constructor and get version
		[Test]
		public void TestVersion()
		{
			string adifEn = "<?xml version='1.0'?>" + Environment.NewLine +
							"<AdifEnumerations Version='3.0.4.0'>" + Environment.NewLine +
							"" + Environment.NewLine +
							"</AdifEnumerations>" + Environment.NewLine;
			MemoryStream str = new MemoryStream(Encoding.ASCII.GetBytes(adifEn));
			AdifEnumerations aEnums = new AdifEnumerations();
			aEnums.LoadDocument(str);
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
			AdifEnumerations aEnums = new AdifEnumerations();
			aEnums.LoadDocument(str);
			Assert.IsNull(aEnums.Version);
		}
		
		// test loading of AdifEnumerations.xml
		[Test]
		public void TestLoadingAdifEnumerationsXml()
		{
			Assert.IsNotNull(App.AdifEnums.Version);
		}
		
		// test Ant_Path Enumeration
		[Test]
		public void TestAntPathEnumG()
		{
			Assert.IsTrue(App.AdifEnums.IsInEnumeration("Ant_Path", "G"));
		}

		// test Ant_Path Enumeration
		[Test]
		public void TestAntPathEnumO()
		{
			Assert.IsTrue(App.AdifEnums.IsInEnumeration("Ant_Path", "O"));
		}

		// test Ant_Path Enumeration
		[Test]
		public void TestAntPathEnumS()
		{
			Assert.IsTrue(App.AdifEnums.IsInEnumeration("Ant_Path", "S"));
		}

		// test Ant_Path Enumeration
		[Test]
		public void TestAntPathEnumL()
		{
			Assert.IsTrue(App.AdifEnums.IsInEnumeration("Ant_Path", "L"));
		}

		// test Ant_Path Enumeration with non value
		[Test]
		public void TestAntPathEnumNoMatch()
		{
			Assert.IsFalse(App.AdifEnums.IsInEnumeration("Ant_Path", "Q"));
		}
		
		// test GetDescription using Ant_Path enumeration
		[Test]
		public void TestGetDescriptionWithEnum()
		{
			string desc = App.AdifEnums.GetDescription("Ant_Path", "O");
			Assert.IsNotNull(desc);
			Assert.AreEqual("other", desc);
		}


		// test GetDescription with unmatched enumeration value using Ant_Path enumeration
		[Test]
		public void TestGetDescriptionWithBadEnum()
		{
			string desc = App.AdifEnums.GetDescription("Ant_Path", "X");
			Assert.IsNull(desc);
		}
		
		// test GetEnumeratedValues using Ant_Path
		[Test]
		public void TestGetEnumeratedValues()
		{
			string[] values = App.AdifEnums.GetEnumeratedValues("Ant_Path");
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
			string[] values = App.AdifEnums.GetEnumeratedValues("Ant_Path2");
		}
		
		// test if enumeration value is deprecated
		[Test]
		public void TestIsDeprecated()
		{
			Assert.IsTrue(App.AdifEnums.IsDeprecated("Arrl_Section", "NWT"));
		}
			
		// test if enumeration value is not deprecated
		[Test]
		public void TestIsNotDeprecated()
		{
			Assert.IsFalse(App.AdifEnums.IsDeprecated("Arrl_Section", "NT"));
		}
		
		// test if IsDeprecated throws XmlException if value not found
		[Test]
		[ExpectedException(typeof(XmlException))]
		public void TestIsDeprecatedThrowsExceptionValueNotFound()
		{
			App.AdifEnums.IsDeprecated("Arrl_Section", "NXTW");
		}
		
		// test if IsDeprecated throws XmlException if enumeration not found
		[Test]
		[ExpectedException(typeof(XmlException))]
		public void TestIfDeprecatedThrowsExceptionEnumerationNotFound()
		{
			App.AdifEnums.IsDeprecated("Arrl2", "NT");
		}
		
		// test if enumeration value is deleted
		[Test]
		public void TestIsDeleted()
		{
			Assert.IsTrue(App.AdifEnums.IsDeleted("Country_Code", "8"));
		}
		
		// test if enumeration value is deleted for not deleted value
		[Test]
		public void TestIsDeletedNotDeletedValue()
		{
			Assert.IsFalse(App.AdifEnums.IsDeleted("Country_Code", "1"));
		}
		
		// test if IsDeleted throws XmlException if enumeration not found
		[Test]
		[ExpectedException(typeof(XmlException))]
		public void TestIfIsDeletedThrowsExceptionEnumerationNotFound()
		{
			App.AdifEnums.IsDeleted("Country_Code", "1026");
		}
		
		// test GetReplacementValue when IsDeprecated and replacement value exists
		[Test]
		public void TestGetReplacementValueForValidReplacement()
		{
			Assert.AreEqual("NT", App.AdifEnums.GetReplacementValue("Arrl_Section", "NWT"));
		}
		
		// test GetReplacementValue when IsDeprecated not set
		[Test]
		public void TestGetReplacementValueNoReplacement()
		{
			Assert.IsNull(App.AdifEnums.GetReplacementValue("Arrl_Section", "ON"));
		}
			
		// test GetBandLimits for valid band
		[Test]
		public void TestGetBandLimits2190m()
		{
			string lowerLimit = string.Empty;
			string upperLimit = string.Empty;
			Assert.IsTrue(App.AdifEnums.GetBandLimits("2190m", out lowerLimit, out upperLimit));
			Assert.AreEqual(".136", lowerLimit);
			Assert.AreEqual(".137", upperLimit);
		}

		// test GetBandLimits for valid band
		[Test]
		public void TestGetBandLimits1mm()
		{
			string lowerLimit = string.Empty;
			string upperLimit = string.Empty;
			Assert.IsTrue(App.AdifEnums.GetBandLimits("1mm", out lowerLimit, out upperLimit));
			Assert.AreEqual("241000", lowerLimit);
			Assert.AreEqual("250000", upperLimit);
		}
		
		// test GetBandLimits for invalid band
		[Test]
		public void TestGetBandLimitsBadBand()
		{
			string lowerLimit = string.Empty;
			string upperLimit = string.Empty;
			Assert.IsFalse(App.AdifEnums.GetBandLimits("11mm", out lowerLimit, out upperLimit));
		}
		
		// test GetBandFromFrequency for valid band
		[Test]
		public void TestGetBandFromFrequency4m()
		{
			string band = string.Empty;
			Assert.IsTrue(App.AdifEnums.GetBandFromFrequency("70.58", out band));
			Assert.AreEqual("4m", band);
		}
		
		// test GetBandFromFrequency for valid band with freq at lower edge
		[Test]
		public void TestGetBandFromFrequencyLowerEdge()
		{
			string band = string.Empty;
			Assert.IsTrue(App.AdifEnums.GetBandFromFrequency("7", out band));
			Assert.AreEqual("40m", band);
		}
		
		// test GetBandFromFrequency for valid band with freq at upper edge
		[Test]
		public void TestGetBandFromFrequencyUpperEdge()
		{
			string band = string.Empty;
			Assert.IsTrue(App.AdifEnums.GetBandFromFrequency(".479", out band));
			Assert.AreEqual("630m", band);
		}

		// test GetBandFromFrequency for frequency not in band
		[Test]
		public void TestGetBandFromFrequencyInvalidFreq()
		{
			string band = string.Empty;
			Assert.IsFalse(App.AdifEnums.GetBandFromFrequency(".485", out band));
		}

		// test IsInEnumeration for Country Code
		[Test]
		public void TestIsInCountryCodeEnumeration()
		{
			Assert.IsTrue(App.AdifEnums.IsInEnumeration("Country_Code", "26"));
		}

		// test IsInEnumeration for Country Code invalid code
		[Test]
		public void TestIsNotInCountryCodeEnumeration()
		{
			Assert.IsFalse(App.AdifEnums.IsInEnumeration("Country_Code", "73"));
		}

		// test GetDescription for Country Code
		[Test]
		public void TestGetDescriptionCountryCode()
		{
			Assert.AreEqual("BRITISH SOMALI", App.AdifEnums.GetDescription("Country_Code", "26"));
		}

		// test GetDescription for Country Code with '&' in description
		[Test]
		public void TestGetDescriptionCountryCodeAmpersand()
		{
			Assert.AreEqual("AUCKLAND & CAMPBELL", App.AdifEnums.GetDescription("Country_Code", "16"));
		}

		// test GetDescription for Country Code invalid code
		[Test]
		public void TestGetDesscriptionInvalidCountryCode()
		{
			Assert.AreEqual(null, App.AdifEnums.GetDescription("Country_Code", "73"));
		}
		
		// test GetCountryCodeFromName with valid name
		[Test]
		public void TestGetCountryCodeFromValidName()
		{
			string code = string.Empty;
			bool found = App.AdifEnums.GetCountryCodeFromName("BRITISH SOMALI", out code);
			Assert.IsTrue(found);
			Assert.AreEqual("26", code);
		}
		
		// test GetCountryCodeFromName with valid name containing '&'
		[Test]
		public void TestGetCountryCodeFromValidNameAmpersand()
		{
			string code = string.Empty;
			bool found = App.AdifEnums.GetCountryCodeFromName("AUCKLAND & CAMPBELL", out code);
			Assert.IsTrue(found);
			Assert.AreEqual("16", code);
		}
		
		// test GetCountryCodeFromName with invalid name
		[Test]
		public void TestGetCountryCodeFromInvalidName()
		{
			string code = string.Empty;
			bool found = App.AdifEnums.GetCountryCodeFromName("BOOGALOO", out code);
			Assert.IsFalse(found);
			Assert.AreEqual(string.Empty, code);
		}
		
		// test GetCreditEquivalentForAward with replacement value
		[Test]
		public void TestGetCreditEquivalentForAwardWithReplacement()
		{
			string credit = string.Empty;
			Assert.AreEqual("DXCC_MODE", App.AdifEnums.GetCreditEquivalentForAward("DXCC_CW"));
		}

		// test GetCreditEquivalentForAward with replacement value
		[Test]
		public void TestGetCreditEquivalentForAwardWithReplacement2()
		{
			string credit = string.Empty;
			Assert.AreEqual("IOTA", App.AdifEnums.GetCreditEquivalentForAward("IOTA"));
		}

		// test GetCreditEquivalentForAward with no replacement value
		[Test]
		public void TestGetCreditEquivalentForAwardWithNoReplacement()
		{
			string credit = string.Empty;
			Assert.AreEqual(null, App.AdifEnums.GetCreditEquivalentForAward("CPAWARD"));
		}
		
		// test GetModeAndSubmode for null mode
		[Test]
		public void TestGetModeAndSubmodeNullMode()
		{
			string mode = null;
			string newMode = string.Empty;
			string subMode = string.Empty;
			bool status = App.AdifEnums.GetModeAndSubmode(mode, out newMode, out subMode);
			Assert.IsFalse(status);
		}

		// test GetModeAndSubmode for invalid mode
		[Test]
		public void TestGetModeAndSubmodeInvalidMode()
		{
			string mode = "BADFMODE";
			string newMode = string.Empty;
			string subMode = string.Empty;
			bool status = App.AdifEnums.GetModeAndSubmode(mode, out newMode, out subMode);
			Assert.IsFalse(status);
		}

		// test GetModeAndSubmode for mode with no replacement
		[Test]
		public void TestGetModeAndSubmodeNoReplacement()
		{
			string mode = "AM";
			string newMode = string.Empty;
			string subMode = string.Empty;
			bool status = App.AdifEnums.GetModeAndSubmode(mode, out newMode, out subMode);
			Assert.IsTrue(status);
			Assert.AreEqual(mode, newMode);
			Assert.AreEqual(null, subMode);
		}

		// test GetModeAndSubmode for mode with replacement
		[Test]
		public void TestGetModeAndSubmodeReplacement()
		{
			string mode = "DOMINOF";
			string newMode = string.Empty;
			string subMode = string.Empty;
			bool status = App.AdifEnums.GetModeAndSubmode(mode, out newMode, out subMode);
			Assert.IsTrue(status);
			Assert.AreEqual("DOMINO", newMode);
			Assert.AreEqual("DOMINOF", subMode);
		}
		
		// test GetSubmodesFromMode for valid mode
		[Test]
		public void TestGetSubmodesFromModeValidMode()
		{
			string[] submodes = App.AdifEnums.GetSubmodesFromMode("SSB");
			Assert.AreEqual(2, submodes.Length);
			Assert.AreEqual("LSB", submodes[0]);
			Assert.AreEqual("USB", submodes[1]);
		}
		
		// test GetSubmodesFromMode for valid mode but no submodes
		[Test]
		public void TestGetSubmodesFromModeValidModeNoSubmodes()
		{
			string[] submodes = App.AdifEnums.GetSubmodesFromMode("AM");
			Assert.AreEqual(0, submodes.Length);
		}
		
		// test GetSubmodesFromMode for empty mode
		[Test]
		public void TestGetSubmodesFromModeEmptyMode()
		{
			string[] submodes = App.AdifEnums.GetSubmodesFromMode(string.Empty);
			Assert.AreEqual(0, submodes.Length);
		}
		
		// test GetModeFromSubmode with valid submode
		[Test]
		public void TestGetModeFromSubmodeValidSubmode()
		{
			string mode = App.AdifEnums.GetModeFromSubmode("USB");
			Assert.AreEqual("SSB", mode);
		}
		
		// test GetModeFromSubmode with 'invalid' mode
		[Test]
		public void TestGetModeFromSubmodeInvalidSubmode()
		{
			string mode = App.AdifEnums.GetModeFromSubmode("PSB");
			Assert.AreEqual(string.Empty, mode);
		}
		
	}
}
