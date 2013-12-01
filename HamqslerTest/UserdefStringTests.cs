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
	// tests for UserdefString class
	[TestFixture]
	public class UserdefStringTests
	{
		Userdef userdef1;
		// TestFixtureSetup
		[TestFixtureSetUp]
		public void TestSepup()
		{
			App.AdifEnums.LoadDocument();
			userdef1 = new Userdef("EPC", "S", App.AdifEnums);
		}
		
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			UserdefString uds = new UserdefString("1234", userdef1);
			Assert.AreEqual("<EPC:4:S>1234", uds.ToAdifString());
		}
		
		// test Validate with valid input
		[Test]
		public void TestValidateValid()
		{
			UserdefString uds = new UserdefString("1234", userdef1);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(uds.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with null value
		[Test]
		public void TestValidateNullValue()
		{
			UserdefString uds = new UserdefString(null, userdef1);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(uds.Validate(out err, out modStr));
			Assert.AreEqual("Value is null.", err);
			Assert.IsNull(modStr);
		}
		
		// test Name accessor
		[Test]
		public void TestNameAccessor()
		{
			UserdefString uds = new UserdefString("1234", userdef1);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(uds.Validate(out err, out modStr));
			Assert.AreEqual("EPC", uds.Name);
		}
	}
}
