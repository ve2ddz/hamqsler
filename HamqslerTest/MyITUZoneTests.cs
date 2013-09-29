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
	// tests for My_ITU_Zone class
	[TestFixture]
	public class MyITUZoneTests
	{
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			My_ITU_Zone zone = new My_ITU_Zone("19");
			Assert.AreEqual("<My_ITU_Zone:2>19", zone.ToAdifString());
		}

		// test Validate with valid zone
		[Test]
		public void TestValidateValidZone()
		{
			My_ITU_Zone zone = new My_ITU_Zone("19");
			string err = string.Empty;
			Assert.IsTrue(zone.Validate(out err));
			Assert.AreEqual(null, err);
		}

		// test Validate with invalid zone
		[Test]
		public void TestValidateInvalidZone()
		{
			My_ITU_Zone zone = new My_ITU_Zone("fb");
			string err = string.Empty;
			Assert.IsFalse(zone.Validate(out err));
			Assert.AreEqual("Value must be a number.", err);
		}
	}
}
