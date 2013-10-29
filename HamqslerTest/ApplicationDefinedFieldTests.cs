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
		AdifEnumerations aEnums;
		// TestFixtureSetup
		[TestFixtureSetUp]
		public void TestSetUp()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			aEnums = new AdifEnumerations(str);
		}
		
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "S", 
			                                                          "Test It", aEnums);
			Assert.AreEqual("<APP_HAMQSLER_TEST:7:S>Test It", adf.ToAdifString());
		}
		
		// test Validate with valid values
		[Test]
		public void TestValidateValidValues()
		{
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "S", 
			                                                          "Test It", aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(adf.Validate(out err, out modStr));
			Assert.AreEqual(null, err);
			Assert.AreEqual(null, modStr);
		}
		
		// test Validate with long valid field name
		[Test]
		public void TestValidateValidValuesLong()
		{
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_EQSL_QSL_SENT", "S", 
			                                                          "R", aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(adf.Validate(out err, out modStr));
			Assert.AreEqual(null, err);
			Assert.AreEqual(null, modStr);
		}
		
		// test Validate with field name not starting with APP_
		[Test]
		public void TestValidateNotApp()
		{
			ApplicationDefinedField adf = new ApplicationDefinedField("APPL_HAMQSLER_TEST", "S", 
			                                                          "Test It", aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(adf.Validate(out err, out modStr));
			Assert.AreEqual("\tInvalid Application Defined Fieldname.", err);
		}
		
		// test Validate with field name not 3 parts
		[Test]
		public void TestValidateNot3Parts()
		{
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLERTEST", "S", 
			                                                          "Test It", aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(adf.Validate(out err, out modStr));
			Assert.AreEqual("\tInvalid Application Defined Fieldname.", err);
		}
		
		// test Validate with field name no PROGRAMNAME
		[Test]
		public void TestValidateNoProgramName()
		{
			ApplicationDefinedField adf = new ApplicationDefinedField("APP__TEST", "S", 
			                                                          "Test It", aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(adf.Validate(out err, out modStr));
			Assert.AreEqual("\tInvalid Application Defined Fieldname.", err);
		}
		
		// test Validate with field name no FIELDNAME
		[Test]
		public void TestValidateNoFieldName()
		{
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_", "S", 
			                                                          "Test It", aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(adf.Validate(out err, out modStr));
			Assert.AreEqual("\tInvalid Application Defined Fieldname.", err);
		}
		
		// test Validate with invalid data type
		[Test]
		public void TestValidateInvalidDataType()
		{
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "Q", 
			                                                          "Test It", aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(adf.Validate(out err, out modStr));
			Assert.AreEqual("\tInvalid Data Type.", err);
		}

		// test Validate with valid AwardList data
		[Test]
		public void TestValidateValidAwardList()
		{
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "A", 
			                                                          "CQWAZ_CW,DARC_DOK", aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(adf.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}

		// test Validate with invalid List data
		[Test]
		public void TestValidateInvalidAwardList()
		{
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "A", 
			                                                          "CQWAZ_CW,CQFRED", aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(adf.Validate(out err, out modStr));
			Assert.AreEqual(null, err);
			Assert.AreEqual("\tInvalid AwardList item: 'CQFRED'. Item removed.", modStr);
			Assert.AreEqual("CQWAZ_CW", adf.Value);
		}

		// test Validate with invalid List data
		[Test]
		public void TestValidateInvalidAwardList2()
		{
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "A", 
			                                                          "CQFRED", aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(adf.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.AreEqual("\tInvalid AwardList item: 'CQFRED'. Item removed.", modStr);
		}

		// test Validate with valid Boolean data
		[Test]
		public void TestValidateValidBoolean()
		{
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "B", 
			                                                          "Y", aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(adf.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}

		// test Validate with invalid Boolean data
		[Test]
		public void TestValidateInvalidBoolean()
		{
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "B", 
			                                                          "F", aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(adf.Validate(out err, out modStr));
			Assert.AreEqual("\tInvalid Boolean Value: 'F'.", err);
			Assert.IsNull(modStr);
		}

		// test Validate with valid CreditList data
		[Test]
		public void TestValidateValidCreditList()
		{
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "C", 
			                                                          "CQWAZ_MODE,IOTA", aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(adf.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}

		// test Validate with invalid CreditList data
		[Test]
		public void TestValidateInvalidCreditList()
		{
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "C", 
			                                                          "CQWAZ_MODE,CQFRED", aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(adf.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.AreEqual("\tInvalid CreditList item: 'CQFRED'.", modStr);
		}

		// test Validate with valid Date
		[Test]
		public void TestValidateValidDate()
		{
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "D", 
			                                                          "20120613", aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(adf.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}

		// test Validate with invalid Date
		[Test]
		public void TestValidateInvalidDate()
		{
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "D", 
			                                                          "19250612", aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(adf.Validate(out err, out modStr));
			Assert.AreEqual("\tDate must be 19300101 or later.", err);
			Assert.IsNull(modStr);
		}

		// test Validate with IntlMulitline String - not allowed in ADI
		[Test] 
		public void TestValidateIntlMultilineString()
		{
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "G", 
			                                                          "Fred", aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(adf.Validate(out err, out modStr));
			Assert.AreEqual("\tInvalid data type: 'G'.", err);
			Assert.IsNull(modStr);
		}

		// test Validate with IntlString - not allowed in ADI
		[Test] 
		public void TestValidateIntlString()
		{
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "I", 
			                                                          "Fred", aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(adf.Validate(out err, out modStr));
			Assert.AreEqual("\tInvalid data type: 'I'.", err);
			Assert.IsNull(modStr);
		}

		// test Validate with valid Location
		[Test]
		public void TestValidateValidLocation()
		{
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "L", 
			                                                          "E179 42.385", aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(adf.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}

		// test Validate with invalid Location
		[Test]
		public void TestValidateInvalidLocation()
		{
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "L", 
			                                                          "E185 42.385", aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(adf.Validate(out err, out modStr));
			Assert.AreEqual("\tInvalid location: 'E185 42.385'.", err);
			Assert.IsNull(modStr);
		}

		// test Validate with Multiline String - all strings are valid
		[Test]
		public void TestValidateValidMultilineString()
		{
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "M", 
			                                                          "E185 42.385", aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(adf.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}

		// test Validate with valid number
		[Test]
		public void TestValidateValidNumber()
		{
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "N", 
			                                                          "42.385", aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(adf.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}

		// test Validate with invalid number
		[Test]
		public void TestValidateInvalidNumber()
		{
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "N", 
			                                                          "E185", aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(adf.Validate(out err, out modStr));
			Assert.AreEqual("\tInvalid number: 'E185'.", err);
			Assert.IsNull(modStr);
		}

		// test Validate with valid SponsoredAwardList
		[Test]
		public void TestValidateValidSponsoredAwardList()
		{
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "P", 
			                                                          "ARRL_WAS_CW", aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(adf.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(err);
		}

		// test Validate with invalid SponsoredAwardList
		[Test]
		public void TestValidateInvalidSponsoredAwardList()
		{
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "P", 
			                                                          "DOK_DARC_FRED", aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(adf.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.AreEqual("\tThe sponsors portion of Awards_Granted is an enumeration." +
			                Environment.NewLine +
			                "\t\tThe value 'DOK_' was not found in enumeration" +
			                Environment.NewLine, modStr);
		}

		// test Validate with valid string
		[Test]
		public void TestValidateValidString()
		{
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "S", 
			                                                          "Test It", aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(adf.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		

		// test Validate with invalid string
		[Test]
		public void TestValidateInvalidString()
		{
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "S", 
			                                                          "Test\r\nIt", aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(adf.Validate(out err, out modStr));
			Assert.AreEqual("\tString value contains a new line character. This is not allowed in StringField types.",
			                err);
			Assert.IsNull(modStr);
		}

		// test Validate with valid Time
		[Test]
		public void TestValidateValidTime()
		{
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "T", 
			                                                          "123456", aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(adf.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}

		// test Validate with invalid Time
		[Test]
		public void TestValidateInvalidTime()
		{
			ApplicationDefinedField adf = new ApplicationDefinedField("APP_HAMQSLER_TEST", "T", 
			                                                          "2403", aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(adf.Validate(out err, out modStr));
			Assert.AreEqual("\tInvalid time.", err);
			Assert.IsNull(modStr);
		}
	}
}
