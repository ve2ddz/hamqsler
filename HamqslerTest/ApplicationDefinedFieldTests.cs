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
using NUnit.Framework;
using hamqsler;

namespace hamqslerTest
{
	// tests for ApplicationDefinedField class
	[TestFixture]
	public class ApplicationDefinedFieldTests
	{
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "S", 
			                                                          "Test It", aEnums);
			Assert.AreEqual("<APP_HAMQSLER_TEST:7:S>Test It", adf.ToAdifString());
		}
		
		// test Validate with valid values
		[Test]
		public void TestValidateValidValues()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "S", 
			                                                          "Test It", aEnums);
			string err = string.Empty;
			Assert.IsTrue(adf.Validate(out err));
			Assert.AreEqual(null, err);
		}
		
		// test Validate with field name not starting with APP_
		[Test]
		public void TestValidateNotApp()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			ApplicationDefinedField adf = new ApplicationDefinedField("APPL_HAMQSLER_TEST", "S", 
			                                                          "Test It", aEnums);
			string err = string.Empty;
			Assert.IsFalse(adf.Validate(out err));
			Assert.AreEqual("Invalid Application Defined Fieldname.", err);
		}
		
		// test Validate with field name not 3 parts
		[Test]
		public void TestValidateNot3Parts()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLERTEST", "S", 
			                                                          "Test It", aEnums);
			string err = string.Empty;
			Assert.IsFalse(adf.Validate(out err));
			Assert.AreEqual("Invalid Application Defined Fieldname.", err);
		}
		
		// test Validate with field name no PROGRAMNAME
		[Test]
		public void TestValidateNoProgramName()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			ApplicationDefinedField adf = new ApplicationDefinedField("APP__TEST", "S", 
			                                                          "Test It", aEnums);
			string err = string.Empty;
			Assert.IsFalse(adf.Validate(out err));
			Assert.AreEqual("Invalid Application Defined Fieldname.", err);
		}
		
		// test Validate with field name no FIELDNAME
		[Test]
		public void TestValidateNoFieldName()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_", "S", 
			                                                          "Test It", aEnums);
			string err = string.Empty;
			Assert.IsFalse(adf.Validate(out err));
			Assert.AreEqual("Invalid Application Defined Fieldname.", err);
		}
		
		// test Validate with invalid data type
		[Test]
		public void TestValidateInvalidDataType()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "Q", 
			                                                          "Test It", aEnums);
			string err = string.Empty;
			Assert.IsFalse(adf.Validate(out err));
			Assert.AreEqual("Invalid Data Type.", err);
		}

		// test Validate with valid AwardList data
		[Test]
		public void TestValidateValidAwardList()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "A", 
			                                                          "CQWAZ_CW,DARC_DOK", aEnums);
			string err = string.Empty;
			Assert.IsTrue(adf.Validate(out err));
			Assert.AreEqual(null, err);
		}

		// test Validate with invalid List data
		[Test]
		public void TestValidateInvalidAwardList()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "A", 
			                                                          "CQWAZ_CW,CQFRED", aEnums);
			string err = string.Empty;
			Assert.IsFalse(adf.Validate(out err));
			Assert.AreEqual("Invalid AwardList item: 'CQFRED'.", err);
		}

		// test Validate with valid CreditList data
		[Test]
		public void TestValidateValidBoolean()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "B", 
			                                                          "Y", aEnums);
			string err = string.Empty;
			Assert.IsTrue(adf.Validate(out err));
			Assert.AreEqual(null, err);
		}

		// test Validate with invalid CreditList data
		[Test]
		public void TestValidateInvalidBoolean()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "B", 
			                                                          "F", aEnums);
			string err = string.Empty;
			Assert.IsFalse(adf.Validate(out err));
			Assert.AreEqual("Invalid Boolean Value: 'F'.", err);
		}

		// test Validate with valid CreditList data
		[Test]
		public void TestValidateValidCreditList()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "C", 
			                                                          "CQWAZ_MODE,IOTA", aEnums);
			string err = string.Empty;
			Assert.IsTrue(adf.Validate(out err));
			Assert.AreEqual(null, err);
		}

		// test Validate with invalid CreditList data
		[Test]
		public void TestValidateInvalidCreditList()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "C", 
			                                                          "CQWAZ_MODE,CQFRED", aEnums);
			string err = string.Empty;
			Assert.IsFalse(adf.Validate(out err));
			Assert.AreEqual("Invalid CreditList item: 'CQFRED'.", err);
		}

		// test Validate with valid Date
		[Test]
		public void TestValidateValidDate()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "D", 
			                                                          "20120613", aEnums);
			string err = string.Empty;
			Assert.IsTrue(adf.Validate(out err));
			Assert.AreEqual(null, err);
		}

		// test Validate with invalid Date
		[Test]
		public void TestValidateInvalidDate()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "D", 
			                                                          "19250612", aEnums);
			string err = string.Empty;
			Assert.IsFalse(adf.Validate(out err));
			Assert.AreEqual("Date must be 19300101 or later", err);
		}

		// test Validate with IntlMulitline String - not allowed in ADI
		[Test] 
		public void TestValidateIntlMultilineString()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "G", 
			                                                          "Fred", aEnums);
			string err = string.Empty;
			Assert.IsFalse(adf.Validate(out err));
			Assert.AreEqual("Invalid data type: 'G'.", err);
		}

		// test Validate with IntlString - not allowed in ADI
		[Test] 
		public void TestValidateIntlString()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "I", 
			                                                          "Fred", aEnums);
			string err = string.Empty;
			Assert.IsFalse(adf.Validate(out err));
			Assert.AreEqual("Invalid data type: 'I'.", err);
		}

		// test Validate with valid Location
		[Test]
		public void TestValidateValidLocation()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "L", 
			                                                          "E179 42.385", aEnums);
			string err = string.Empty;
			Assert.IsTrue(adf.Validate(out err));
			Assert.AreEqual(null, err);
		}

		// test Validate with invalid Location
		[Test]
		public void TestValidateInvalidLocation()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "L", 
			                                                          "E185 42.385", aEnums);
			string err = string.Empty;
			Assert.IsFalse(adf.Validate(out err));
			Assert.AreEqual("Invalid location: 'E185 42.385'.", err);
		}

		// test Validate with Multiline String - all strings are valid
		[Test]
		public void TestValidateValidMultilineString()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "M", 
			                                                          "E185 42.385", aEnums);
			string err = string.Empty;
			Assert.IsTrue(adf.Validate(out err));
			Assert.AreEqual(null, err);
		}

		// test Validate with valid number
		[Test]
		public void TestValidateValidNumber()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "N", 
			                                                          "42.385", aEnums);
			string err = string.Empty;
			Assert.IsTrue(adf.Validate(out err));
			Assert.AreEqual(null, err);
		}

		// test Validate with invalid number
		[Test]
		public void TestValidateInvalidNumber()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "N", 
			                                                          "E185", aEnums);
			string err = string.Empty;
			Assert.IsFalse(adf.Validate(out err));
			Assert.AreEqual("Invalid number: 'E185'.", err);
		}

		// test Validate with valid SponsoredAwardList
		[Test]
		public void TestValidateValidSponsoredAwardList()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "P", 
			                                                          "ARRL_WAS_CW", aEnums);
			string err = string.Empty;
			Assert.IsTrue(adf.Validate(out err));
			Assert.AreEqual(null, err);
		}

		// test Validate with invalid SponsoredAwardList
		[Test]
		public void TestValidateInvalidSponsoredAwardList()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "P", 
			                                                          "DOK_DARC_FRED", aEnums);
			string err = string.Empty;
			Assert.IsFalse(adf.Validate(out err));
			Assert.AreEqual("Invalid sponsored award item: 'DOK_DARC_FRED'.", err);
		}

		// test Validate with valid string
		[Test]
		public void TestValidateValidString()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "S", 
			                                                          "Test It", aEnums);
			string err = string.Empty;
			Assert.IsTrue(adf.Validate(out err));
			Assert.AreEqual(null, err);
		}
		

		// test Validate with invalid string
		[Test]
		public void TestValidateInvalidString()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "S", 
			                                                          "Test\r\nIt", aEnums);
			string err = string.Empty;
			Assert.IsFalse(adf.Validate(out err));
			Assert.AreEqual("String value contains a new line character. This is not allowed in StringField types",
			                err);
		}

		// test Validate with valid Time
		[Test]
		public void TestValidateValidTime()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "T", 
			                                                          "123456", aEnums);
			string err = string.Empty;
			Assert.IsTrue(adf.Validate(out err));
			Assert.AreEqual(null, err);
		}

		// test Validate with invalid Time
		[Test]
		public void TestValidateInvalidTime()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "T", 
			                                                          "2403", aEnums);
			string err = string.Empty;
			Assert.IsFalse(adf.Validate(out err));
			Assert.AreEqual("Invalid time.", err);
		}
	}
}
