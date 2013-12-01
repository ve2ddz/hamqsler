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
	// tests for UserdefTime class
	public class UserdefTimeTests
	{
		Userdef userdef1;
		// TestFixtureSetup
		[TestFixtureSetUp]
		public void TestSepup()
		{
			App.AdifEnums.LoadDocument();
			userdef1 = new Userdef("QslTime", "T", App.AdifEnums);
		}
		
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			UserdefTime uds = new UserdefTime("1248", userdef1);
			Assert.AreEqual("<QslTime:4:T>1248", uds.ToAdifString());
		}
		
		// test Validate with valid input
		[Test]
		public void TestValidateValid()
		{
			UserdefTime uds = new UserdefTime("124819", userdef1);
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
			UserdefTime uds = new UserdefTime("124875", userdef1);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(uds.Validate(out err, out modStr));
			Assert.AreEqual("\tTime must be in HHMM or HHMMSS format.", err);
			Assert.IsNull(modStr);
		}
		
		// test Name accessor
		[Test]
		public void TestNameAccessor()
		{
			UserdefTime uds = new UserdefTime("124819", userdef1);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(uds.Validate(out err, out modStr));
			Assert.AreEqual("QslTime", uds.Name);
		}
	}
}
