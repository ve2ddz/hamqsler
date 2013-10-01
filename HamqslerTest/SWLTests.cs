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
	// tests for SWL class
	[TestFixture]
	public class SWLTests
	{
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			SWL swl = new SWL("Y");
			Assert.AreEqual("<SWL:1>Y", swl.ToAdifString());
		}

		// test Validate with valid value
		[Test]
		public void TestValidateY()
		{
			SWL swl = new SWL("Y");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(swl.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with valid value
		[Test]
		public void TestValidateN()
		{
			SWL swl = new SWL("N");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(swl.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid value
		[Test]
		public void TestValidateInvalidValue()
		{
			SWL swl = new SWL("F");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(swl.Validate(out err, out modStr));
			Assert.AreEqual("Boolean field must have value 'Y' or 'N'.", err);
			Assert.IsNull(modStr);
		}
	}
}
