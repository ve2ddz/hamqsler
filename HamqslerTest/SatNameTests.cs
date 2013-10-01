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
	// tests for Sat_Name class
	[TestFixture]
	public class SatNameTests
	{
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			Sat_Name name = new Sat_Name("AO-25");
			Assert.AreEqual("<Sat_Name:5>AO-25", name.ToAdifString());
		}

		// test Validate - any value is valid
		public void TestValidate()
		{
			Sat_Name name = new Sat_Name("AO-25");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(name.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
	}
}
