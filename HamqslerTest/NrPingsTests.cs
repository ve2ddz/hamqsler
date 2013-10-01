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
using NUnit.Framework;
using hamqsler;

namespace hamqslerTest
{
	// tests for Nr_Pings class
	[TestFixture]
	public class NrPingsTests
	{
		// test ToAdifString
		[Test]
		public void TestMethod()
		{
			Nr_Pings pings = new Nr_Pings("12");
			Assert.AreEqual("<Nr_Pings:2>12", pings.ToAdifString());
		}
		
		// test Validate with valid number
		[Test]
		public void TestValidateValidNumber()
		{
			Nr_Pings pings = new Nr_Pings("12");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(pings.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}

		// test Validate with invalid number
		[Test]
		public void TestValidateInvalidNumber()
		{
			Nr_Pings pings = new Nr_Pings("fred");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(pings.Validate(out err, out modStr));
			Assert.AreEqual("Value must be a number.", err);
			Assert.IsNull(modStr);
		}

		// test Validate with empty string
		[Test]
		public void TestValidateEmptyString()
		{
			Nr_Pings pings = new Nr_Pings(string.Empty);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(pings.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
	}
}
