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
	// tests for UserdefNumber class
	[TestFixture]
	public class UserdefNumberTests
	{
		AdifEnumerations aEnums;
		Userdef userdef1;
		// TestFixtureSetup
		[TestFixtureSetUp]
		public void TestSepup()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			aEnums = new AdifEnumerations(str);
			userdef1 = new Userdef("EPC", "N", aEnums);
		}
		
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			UserdefNumber uds = new UserdefNumber("1234", userdef1);
			Assert.AreEqual("<EPC:4:N>1234", uds.ToAdifString());
		}
		
		// test Validate with valid input
		[Test]
		public void TestValidateValid()
		{
			UserdefNumber uds = new UserdefNumber("1234.5", userdef1);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(uds.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
				
		// test Validate with invalid input
		[Test]
		public void TestValidateInvalid()
		{
			UserdefNumber uds = new UserdefNumber("1234F", userdef1);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(uds.Validate(out err, out modStr));
			Assert.AreEqual("Value must be a number.", err);
			Assert.IsNull(modStr);
		}
				
		// test Validate with valid range data
		[Test]
		public void TestValidateValidRange()
		{
			Userdef userdef2 = new Userdef("EPC2", "N", "5", "20.", aEnums);
			UserdefNumber uds = new UserdefNumber("14.315", userdef2);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(uds.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
				
		// test Validate data outside range
		[Test]
		public void TestValidateOutsideRange()
		{
			Userdef userdef2 = new Userdef("EPC2", "N", "5", "20.", aEnums);
			UserdefNumber uds = new UserdefNumber("1", userdef2);
			string err = string.Empty;
			string modStr = string.Empty;			
			Assert.IsFalse(uds.Validate(out err,out modStr));
			Assert.AreEqual("'1' is not within range specified by the Userdef field.", err);
			Assert.IsNull(modStr);
		}
	}
}
