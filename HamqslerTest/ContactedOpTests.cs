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
	// tests for Contacted_op class
	[TestFixture]
	public class ContactedOpTests
	{
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			Contacted_Op op = new Contacted_Op("VA3HJ");
			Assert.AreEqual("<Contacted_Op:5>VA3HJ", op.ToAdifString());
		}
		
		// test Validate with valid callsign
		[Test]
		public void TestValidate()
		{
			Contacted_Op op = new Contacted_Op("VA3HJ");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(op.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid callsign
		[Test]
		public void TestValidateInvalidCall()
		{
			Contacted_Op op = new Contacted_Op("stretch");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(op.Validate(out err, out modStr));
			Assert.AreEqual("\tCallsign 'stretch' is invalid.", err);
			Assert.IsNull(modStr);
		}
	}
}
