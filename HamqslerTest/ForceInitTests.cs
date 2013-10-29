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
	// tests for Force_Init class
	[TestFixture]
	public class ForceInitTests
	{
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			Force_Init fi = new Force_Init("Y");
			Assert.AreEqual("<Force_Init:1>Y", fi.ToAdifString());
		}

		// test Validate with valid value
		[Test]
		public void TestValidateY()
		{
			Force_Init bf = new Force_Init("Y");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(bf.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with valid value
		[Test]
		public void TestValidateN()
		{
			Force_Init bf = new Force_Init("N");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(bf.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid value
		[Test]
		public void TestValidateInvalidValue()
		{
			Force_Init bf = new Force_Init("F");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(bf.Validate(out err, out modStr));
			Assert.AreEqual("\tBoolean field must have value 'Y' or 'N'.", err);
			Assert.IsNull(modStr);
		}
	}
}
