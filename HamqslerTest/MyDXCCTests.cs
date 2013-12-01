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
	// tests for My_DXCC class
	[TestFixture]
	public class MyDXCCTests
	{
		// TestFixtureSetup
		[TestFixtureSetUp]
		public void TestSepup()
		{
			App.AdifEnums.LoadDocument();
		}
		
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			My_DXCC dxcc = new My_DXCC("512", App.AdifEnums);
			Assert.AreEqual("<My_DXCC:3>512", dxcc.ToAdifString());
		}
		
		// test Validate with valid country code
		[Test]
		public void TestValidateValidCountryCode()
		{
			My_DXCC dxcc = new My_DXCC("1", App.AdifEnums);
			string err = string.Empty;
			string modStr =string.Empty;
			Assert.IsTrue(dxcc.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid country code
		[Test]
		public void TestValidateInvalidCountryCode()
		{
			My_DXCC dxcc = new My_DXCC("1023", App.AdifEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(dxcc.Validate(out err, out modStr));
			Assert.AreEqual("\tThis QSO Field is of type enumeration. The value '1023' " +
			                    "was not found in enumeration.", err);
			Assert.IsNull(modStr);
		}
		
		// test GetCountryName with valid country code
		[Test]
		public void TestGetCountryNameValidCountryCode()
		{
			My_DXCC dxcc = new My_DXCC("1", App.AdifEnums);
			string err = string.Empty;
			Assert.AreEqual("CANADA", dxcc.GetCountryName(out err));
			Assert.AreEqual(null, err);
		}

		// test GetCountryName with invalid country code
		[Test]
		public void TestGetCountryNameInvalidCountryCode()
		{
			My_DXCC dxcc = new My_DXCC("1023", App.AdifEnums);
			string err = string.Empty;
			Assert.AreEqual(null, dxcc.GetCountryName(out err));
			Assert.AreEqual("\tCountry code '1023' is not a valid code. Country name cannot be retrieved.", 
			                err);
		}
	}
}
