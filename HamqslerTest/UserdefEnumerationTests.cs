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
	// tests for UserdefEnumeration class
	[TestFixture]
	public class UserdefEnumerationTests
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
			string[] enums = {"A", "B", "E", "V"};
			userdef1 = new Userdef("HamQSLerQSLStatus", "E", enums, aEnums);
		}
		
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			UserdefEnumeration uds = new UserdefEnumeration("V", userdef1);
			Assert.AreEqual("<HamQSLerQSLStatus:1:E>V", uds.ToAdifString());
		}
		
		// test Validate with valid input
		[Test]
		public void TestValidateValid()
		{
			UserdefEnumeration uds = new UserdefEnumeration("B", userdef1);
			string err = string.Empty;
			Assert.IsTrue(uds.Validate(out err));
			Assert.AreEqual(null, err);
		}
		
		
		// test Validate with invalid input
		[Test]
		public void TestValidateInvalid()
		{
			UserdefEnumeration uds = new UserdefEnumeration("I", userdef1);
			string err = string.Empty;
			Assert.IsFalse(uds.Validate(out err));
			Assert.AreEqual("This QSO Field is of type enumeration. The value 'I' was not found in enumeration.", 
			                err);
		}
	}
}
