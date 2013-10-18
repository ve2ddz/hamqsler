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
	// tests for UserdefLocation class
	[TestFixture]
	public class UserdefLocationTests
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
			userdef1 = new Userdef("MumbaiLatitude", "L", aEnums);
		}
		
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			UserdefLocation uds = new UserdefLocation("N027 26.385", userdef1);
			Assert.AreEqual("<MumbaiLatitude:11:L>N027 26.385", uds.ToAdifString());
		}
		
		// test Validate with valid input
		[Test]
		public void TestValidateValid()
		{
			UserdefLocation uds = new UserdefLocation("N027 26.385", userdef1);
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
			UserdefLocation uds = new UserdefLocation("U027 26.385", userdef1);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(uds.Validate(out err, out modStr));
			Assert.AreEqual("'U027 26.385' is not a valid location.", err);
			Assert.IsNull(modStr);
		}
		
		// test Name accessor
		[Test]
		public void TestNameAccessor()
		{
			UserdefLocation uds = new UserdefLocation("N027 26.385", userdef1);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(uds.Validate(out err, out modStr));
			Assert.AreEqual("MumbaiLatitude", uds.Name);
		}
	}
}
