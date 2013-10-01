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
	// tests for SKCC class
	[TestFixture]
	public class SKCCTests
	{
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			SKCC skcc = new SKCC("102C");
			Assert.AreEqual("<SKCC:4>102C", skcc.ToAdifString());
		}

		// test Validate - any value is valid
		[Test]
		public void TestValidate()
		{
			SKCC skcc = new SKCC("102C");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(skcc.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
	}
}
