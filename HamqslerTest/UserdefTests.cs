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
	// tests for Userdef class
	[TestFixture]
	public class UserdefTests
	{
		AdifEnumerations aEnums;
		
		// fixture setup
		[TestFixtureSetUp]
		public void Init()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
	        Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			aEnums = new AdifEnumerations(str);
		}

		// test ToAdifString with no Enumeration or range
		[Test]
		public void TestToAdifString()
		{
			Userdef ud = new Userdef("EPC", "N", aEnums);
			Assert.AreEqual("<Userdef1:3:N>EPC", ud.ToAdifString(1));
		}
		
		// test ToAdifString with enumeration
		[Test]
		public void TestToAdifStringEnumeration()
		{
			string[] sizes = {"S","M","L"};
			Userdef ud = new Userdef("SweaterSize", "E", sizes, aEnums);
			Assert.AreEqual("<Userdef2:19:E>SweaterSize,{S,M,L}", ud.ToAdifString(2));
		}
		
		// test ToAdifString with range
		[Test]
		public void TestToAdifStringERange()
		{
			Userdef ud = new Userdef("ShoeSize", "N", "5", "20", aEnums);
			Assert.AreEqual("<Userdef3:15:N>ShoeSize,{5:20}", ud.ToAdifString(3));
		}
		
		// test Validate with valid fields
		[Test]
		public void TestValidateValid()
		{
			Userdef ud = new Userdef("EPC", "N", aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(ud.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with valid fields
		[Test]
		public void TestValidateValidEnums()
		{
			string[] sizes = {"S","M","L"};
			Userdef ud = new Userdef("SweaterSize", "E", sizes, aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(ud.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with valid range
		[Test]
		public void TestValidateValidRange()
		{
			Userdef ud = new Userdef("ShoeSize", "E", "5", "20", aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(ud.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}

		// test Validate with no name
		[Test]
		public void TestValidateNoName()
		{
			Userdef ud = new Userdef("", "N", aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(ud.Validate(out err, out modStr));
			Assert.AreEqual("Invalid fieldname.", err);
			Assert.IsNull(modStr);
		}

		// test Validate with null name
		[Test]
		public void TestValidateNullName()
		{
			Userdef ud = new Userdef(null, "N", aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(ud.Validate(out err, out modStr));
			Assert.AreEqual("Invalid fieldname.", err);
			Assert.IsNull(modStr);
		}

		// test Validate with no datatype
		[Test]
		public void TestValidateNoDataType()
		{
			Userdef ud = new Userdef("EPC", "", aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(ud.Validate(out err, out modStr));
			Assert.AreEqual("Invalid data type.", err);
			Assert.IsNull(modStr);
		}

		// test Validate with null datatype
		[Test]
		public void TestValidateNullDataType()
		{
			Userdef ud = new Userdef("EPC", null, aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(ud.Validate(out err, out modStr));
			Assert.AreEqual("Invalid data type.", err);
			Assert.IsNull(modStr);
		}

		// test Validate with invalid datatype
		[Test]
		public void TestValidateInvalidDataType()
		{
			Userdef ud = new Userdef("EPC", "Q", aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(ud.Validate(out err, out modStr));
			Assert.AreEqual("Invalid data type.", err);
			Assert.IsNull(modStr);
		}

		// test Validate with no enumeration
		[Test]
		public void TestValidateNonEnumeration()
		{
			string[] sizes = {};
			Userdef ud = new Userdef("SweaterSize", "E", sizes, aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(ud.Validate(out err, out modStr));
			Assert.AreEqual("Invalid enumeration.", err);
			Assert.IsNull(modStr);
		}

		// test Validate with null enumeration
		[Test]
		public void TestValidateNullEnumeration()
		{
			Userdef ud = new Userdef("SweaterSize", "E", null, aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(ud.Validate(out err, out modStr));
			Assert.AreEqual("Invalid enumeration.", err);
			Assert.IsNull(modStr);
		}

		// test Validate with non number lower limit
		[Test]
		public void TestValidateNonNumberlLowerLimit(
			[Values("", null, "E5", ".5", "-.5", "+.5")] string lower)
		{
			Userdef ud = new Userdef("ShoeSize", "E", lower, "20", aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(ud.Validate(out err, out modStr));
			Assert.AreEqual("Invalid lower limit.", err);
			Assert.IsNull(modStr);
		}

		// test Validate with non number upper limit
		[Test]
		public void TestValidateNonNumberlUpperLimit(
			[Values("", null, "E5", ".5", "-.5", "+.5")] string upper)
		{
			Userdef ud = new Userdef("ShoeSize", "E", "5", upper, aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(ud.Validate(out err, out modStr));
			Assert.AreEqual("Invalid upper limit.", err);
			Assert.IsNull(modStr);
		}
	}
}
