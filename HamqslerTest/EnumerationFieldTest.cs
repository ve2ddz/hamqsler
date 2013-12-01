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
	/// <summary>
	/// Description of EnumerationFieldTest.
	/// </summary>
	[TestFixture]
	public class EnumerationFieldTest
	{
		// TestFixtureSetup
		[TestFixtureSetUp]
		public void TestSepup()
		{
			App.AdifEnums.LoadDocument();
		}
		
		// test that Enumeration fields set properly
		[Test]
		public void TestEnumerationSet()
		{
			string[] enums = {"e1", "e2", "e3", "e4"};
			EnumerationField ef = new EnumerationField(enums);
			Assert.AreEqual(enums, ef.Enumeration);
		}
		
		// test that value is in the enumeration
		[Test]
		public void TestValueInEnumeration()
		{
			string err = string.Empty;
			string[] enums = {"e1", "e2", "e3", "e4"};
			EnumerationField ef = new EnumerationField(enums);
			Assert.IsTrue(ef.IsInEnumeration("e1", out err));
			Assert.AreEqual(null, err);
		}
		
		// test that value is in the enumeration
		[Test]
		public void TestValueInEnumeration1()
		{
			string err = string.Empty;
			string[] enums = {"e1", "e2", "e3", "e4"};
			EnumerationField ef = new EnumerationField(enums);
			Assert.IsTrue(ef.IsInEnumeration("e4", out err));
			Assert.AreEqual(null, err);
		}
		
		// test that Enumeration fields set properly
		[Test]
		public void TestEnumerationSet2()
		{
			EnumerationField ef = new EnumerationField("Arrl_Sect", App.AdifEnums);
			Assert.AreEqual("Arrl_Sect", ef.EnumName);
		}
		
		// test that value is in the enumeration
		[Test]
		public void TestValueInEnumeration2()
		{
			string err = string.Empty;
			EnumerationField ef = new EnumerationField("Arrl_Section", App.AdifEnums);
			Assert.IsTrue(ef.IsInEnumeration("NT", out err));
			Assert.AreEqual(null, err);
		}
		
		// test that value is in the enumeration
		[Test]
		public void TestValueInEnumeration3()
		{
			string err = string.Empty;
			EnumerationField ef = new EnumerationField("Arrl_Section", App.AdifEnums);
			Assert.IsTrue(ef.IsInEnumeration("EB", out err));
			Assert.AreEqual(null, err);
		}
		
		// test that value is not in the enumeration
		[Test]
		public void TestValueNotInEnumeration()
		{
			string err = string.Empty;
			string[] enums = {"e1", "e2", "e3", "e4"};
			EnumerationField ef = new EnumerationField(enums);
			Assert.IsFalse(ef.IsInEnumeration("e5", out err));
			Assert.AreEqual("\tThis QSO Field is of type enumeration. The value 'e5' was not found in enumeration.",
			                err);
		}
		// test that value is not in the enumeration
		[Test]
		public void TestValueNotInEnumeration1()
		{
			string err = string.Empty;
			EnumerationField ef = new EnumerationField("Arrl_Section", App.AdifEnums);
			Assert.IsFalse(ef.IsInEnumeration("ABCD", out err));
			Assert.AreEqual("\tThis QSO Field is of type enumeration. The value 'ABCD' was not found in enumeration.",
			                err);
		}
		
		// validate that value is in enumeration
		[Test]
		public void TestValidateTrue()
		{
			string err = string.Empty;
			string[] enums = {"e1", "e2", "e3", "e4"};
			EnumerationField ef = new EnumerationField(enums);
			Assert.IsTrue(ef.Validate("e1", out err));
		}
		
		// valkidate that value is in enumeration
		[Test]
		public void TestValidateTrue1()
		{
			string err = string.Empty;
			string[] enums = {"e1", "e2", "e3", "e4"};
			EnumerationField ef = new EnumerationField(enums);
			Assert.IsTrue(ef.Validate("e4", out err));			
		}
		
		// validate that value is not in enumeration
		[Test]
		public void TestValidateFalse()
		{
			string err = string.Empty;
			string[] enums = {"e1", "e2", "e3", "e4"};
			EnumerationField ef = new EnumerationField(enums);
			Assert.IsFalse(ef.Validate("e5", out err));						
		}
		
		// validate that value is in enumeration
		[Test]
		public void TestValidateTrue2()
		{
			string err = string.Empty;
			EnumerationField ef = new EnumerationField("Arrl_Section", App.AdifEnums);
			Assert.IsTrue(ef.Validate("NT", out err));
		}
		
		// validate that value is in enumeration
		[Test]
		public void TestValidateTrue3()
		{
			string err = string.Empty;
			EnumerationField ef = new EnumerationField("Arrl_Section", App.AdifEnums);
			Assert.IsTrue(ef.Validate("EB", out err));
		}
		
		// validate that value is not in enumeration
		[Test]
		public void TestValidateFalse1()
		{
			string err = string.Empty;
			EnumerationField ef = new EnumerationField("Arrl_Section", App.AdifEnums);
			Assert.IsFalse(ef.Validate("ABCD", out err));
		}
		
		// test ToString
		[Test]
		public void TestToString()
		{
			string[] values = {"A","B","C"};
			EnumerationField ef = new EnumerationField(values);
			Assert.AreEqual("A,B,C", ef.ToString());
		}
	}
}
