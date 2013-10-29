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
	// tests for CQZ class
	[TestFixture]
	public class CQZTests
	{
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			CQZ cqz = new CQZ("10");
			Assert.AreEqual("<CQZ:2>10", cqz.ToAdifString());
		}
		
		// test Validate with valid value
		[Test]
		public void TestValidateWithValidValue()
		{
			CQZ cqz = new CQZ("10");
			string err = string.Empty;
			string modStr =string.Empty;
			Assert.IsTrue(cqz.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid value
		[Test]
		public void TestValidateWithInvalidValue()
		{
			string err = string.Empty;
			string modStr = string.Empty;
			CQZ cqz = new CQZ("hump");
			Assert.IsFalse(cqz.Validate(out err, out modStr));
			Assert.AreEqual("\tValue must be a number.", err);
			Assert.IsNull(modStr);
		}
	}
}
