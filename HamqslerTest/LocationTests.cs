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
	// tests for Location class
	[TestFixture]
	public class LocationTests
	{
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			Location loc = new Location("W079 54.086");
			Assert.AreEqual("<Location:11>W079 54.086", loc.ToAdifString());
		}

		// test Validate with valid location
		[Test]
		public void TestValidateValidLocation()
		{
			Location loc = new Location("W079 54.086");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(loc.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}

		// test Validate with invalid direction
		[Test]
		public void TestValidateInvalidDirection()
		{
			Location loc = new Location("U079 54.086");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(loc.Validate(out err, out modStr));
			Assert.AreEqual("'U079 54.086' is not a valid location.", err);
			Assert.IsNull(modStr);
		}

		// test Validate with invalid location format
		[Test]
		public void TestValidateInvalidLocation()
		{
			Location loc = new Location("S07 54.086");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(loc.Validate(out err, out modStr));
			Assert.AreEqual("'S07 54.086' is not a valid location.", err);
			Assert.IsNull(modStr);
		}

		// test Validate with invalid location format
		[Test]
		public void TestValidateInvalidLocation1()
		{
			Location loc = new Location("S079  54.086");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(loc.Validate(out err, out modStr));
			Assert.AreEqual("'S079  54.086' is not a valid location.", err);
			Assert.IsNull(modStr);
		}

		// test Validate with invalid location format
		[Test]
		public void TestValidateInvalidLocation2()
		{
			Location loc = new Location("S079 4.086");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(loc.Validate(out err, out modStr));
			Assert.AreEqual("'S079 4.086' is not a valid location.", err);
			Assert.IsNull(modStr);
		}

		// test Validate with invalid location format
		[Test]
		public void TestValidateInvalidLocation3()
		{
			Location loc = new Location("S079 543.08");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(loc.Validate(out err, out modStr));
			Assert.AreEqual("'S079 543.08' is not a valid location.", err);
			Assert.IsNull(modStr);
		}

		// test Validate with invalid location format
		[Test]
		public void TestValidateInvalidLocation4()
		{
			Location loc = new Location("S079 54.0866");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(loc.Validate(out err, out modStr));
			Assert.AreEqual("'S079 54.0866' is not a valid location.", err);
			Assert.IsNull(modStr);
		}

		// test Validate with invalid location format
		[Test]
		public void TestValidateInvalidLocation5()
		{
			Location loc = new Location("S079 54.0f6");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(loc.Validate(out err, out modStr));
			Assert.AreEqual("'S079 54.0f6' is not a valid location.", err);
			Assert.IsNull(modStr);
		}

		// test Validate with invalid location format
		[Test]
		public void TestValidateInvalidLocation7()
		{
			Location loc = new Location("S181 54.086");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(loc.Validate(out err,out modStr));
			Assert.AreEqual("'S181 54.086' is not a valid location.", err);
			Assert.IsNull(modStr);
		}

		// test Validate with invalid location format
		[Test]
		public void TestValidateInvalidLocation8()
		{
			Location loc = new Location("S073 60.000");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(loc.Validate(out err, out modStr));
			Assert.AreEqual("'S073 60.000' is not a valid location.", err);
			Assert.IsNull(modStr);
		}

		// test Validate with invalid location format
		[Test]
		public void TestValidateInvalidLocation9()
		{
			Location loc = new Location("S180 00.001");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(loc.Validate(out err, out modStr));
			Assert.AreEqual("'S180 00.001' is not a valid location.", err);
			Assert.IsNull(modStr);
		}
	}
}
