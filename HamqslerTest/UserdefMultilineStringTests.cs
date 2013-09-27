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
	// tests for UserdefMultilineString class
	[TestFixture]
	public class UserdefMultilineStringTests
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
			userdef1 = new Userdef("EPC", "M", aEnums);
		}
		
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			UserdefMultilineString uds = new UserdefMultilineString(@"1234\n\r56", userdef1);
			Assert.AreEqual(@"<EPC:10:M>1234\n\r56", uds.ToAdifString());
		}
		
		// test Validate with valid input
		[Test]
		public void TestValidateValid()
		{
			UserdefMultilineString uds = new UserdefMultilineString(@"1234\n\r56", userdef1);
			string err = string.Empty;
			Assert.IsTrue(uds.Validate(out err));
			Assert.AreEqual(null, err);
		}
		
		// test Validate with null value
		[Test]
		public void TestValidateNullValue()
		{
			UserdefMultilineString uds = new UserdefMultilineString(null, userdef1);
			string err = string.Empty;
			Assert.IsFalse(uds.Validate(out err));
			Assert.AreEqual("Value is null", err);
		}
	}
}
