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
	// tests for My_Iota class
	[TestFixture]
	public class MyIotaTests
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
			My_Iota iota = new My_Iota("NA-001", App.AdifEnums);
			Assert.AreEqual("<My_Iota:6>NA-001", iota.ToAdifString());
		}
		
		// test Validate with valid IOTA designator
		[Test]
		public void TestValidateValidDesignator()
		{
			My_Iota iota = new My_Iota("NA-001", App.AdifEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(iota.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid IOTA designator
		[Test]
		public void TestValidateInvalidDesignator2()
		{
			My_Iota iota = new My_Iota("SNA-001", App.AdifEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(iota.Validate(out err, out modStr));
			Assert.AreEqual("\t'SNA-001' is not a valid IOTA designator.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid IOTA designator
		[Test]
		public void TestValidateInvalidDesignator3()
		{
			My_Iota iota = new My_Iota("NA-0011", App.AdifEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(iota.Validate(out err, out modStr));
			Assert.AreEqual("\t'NA-0011' is not a valid IOTA designator.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid IOTA designator
		[Test]
		public void TestValidateInvalidDesignator4()
		{
			My_Iota iota = new My_Iota("NA-0F1", App.AdifEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(iota.Validate(out err, out modStr));
			Assert.AreEqual("\t'NA-0F1' is not a valid IOTA designator.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid IOTA designator (bad format)
		[Test]
		public void TestValidateBadFormat()
		{
			My_Iota iota = new My_Iota("BA0001", App.AdifEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(iota.Validate(out err, out modStr));
			Assert.AreEqual("\t'BA0001' is not a valid IOTA designator.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid IOTA designator (bad continent
		[Test]
		public void TestValidateBadContinent()
		{
			My_Iota iota = new My_Iota("BA-001", App.AdifEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(iota.Validate(out err, out modStr));
			Assert.AreEqual("\t'BA-001' is not a valid IOTA designator.", err);
			Assert.IsNull(modStr);
		}
	}
}
