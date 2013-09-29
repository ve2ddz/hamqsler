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
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			My_DXCC dxcc = new My_DXCC("512", aEnums);
			Assert.AreEqual("<My_DXCC:3>512", dxcc.ToAdifString());
		}
		
		// test Validate with valid country code
		[Test]
		public void TestValidateValidCountryCode()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			My_DXCC dxcc = new My_DXCC("1", aEnums);
			string err = string.Empty;
			Assert.IsTrue(dxcc.Validate(out err));
			Assert.AreEqual(null, err);
		}
		
		// test Validate with invalid country code
		[Test]
		public void TestValidateInvalidCountryCode()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			My_DXCC dxcc = new My_DXCC("1023", aEnums);
			string err = string.Empty;
			Assert.IsFalse(dxcc.Validate(out err));
			Assert.AreEqual("This QSO Field is of type enumeration. The value '1023' " +
			                    "was not found in enumeration.", err);
		}
		
		// test GetCountryName with valid country code
		[Test]
		public void TestGetCountryNameValidCountryCode()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			My_DXCC dxcc = new My_DXCC("1", aEnums);
			string err = string.Empty;
			Assert.AreEqual("CANADA", dxcc.GetCountryName(out err));
			Assert.AreEqual(null, err);
		}

		// test GetCountryName with invalid country code
		[Test]
		public void TestGetCountryNameInvalidCountryCode()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			My_DXCC dxcc = new My_DXCC("1023", aEnums);
			string err = string.Empty;
			Assert.AreEqual(null, dxcc.GetCountryName(out err));
			Assert.AreEqual("Country code '1023' is not a valid code. Country name cannot be retrieved.", 
			                err);
		}
	}
}
