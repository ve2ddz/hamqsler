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
	//tests for Fists_CC class
	[TestFixture]
	public class FistsCCTests
	{
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			Fists_CC fcc = new Fists_CC("1234");
			Assert.AreEqual("<Fists_CC:4>1234", fcc.ToAdifString());
		}
		
		// test Validate with valid CC number
		[Test]
		public void TestValidateValidCC()
		{
			Fists_CC fcc = new Fists_CC("1234");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(fcc.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid CC number
		[Test]
		public void TestValidateInvalidCC()
		{
			Fists_CC fcc = new Fists_CC("12f4");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(fcc.Validate(out err, out modStr));
			Assert.AreEqual("Invalid Fists CC number.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid CC number
		[Test]
		public void TestValidateDecimalCC()
		{
			Fists_CC fcc = new Fists_CC("12.4");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(fcc.Validate(out err, out modStr));
			Assert.AreEqual("Invalid Fists CC number.", err);
			Assert.IsNull(modStr);
		}
	}
}
