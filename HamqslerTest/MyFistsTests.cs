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
	// tests for My_Fists class
	[TestFixture]
	public class MyFistsTests
	{
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			My_Fists fists = new My_Fists("1234");
			Assert.AreEqual("<My_Fists:4>1234", fists.ToAdifString());
		}
		
		// test Validate with number only
		[Test]
		public void TestValidateNumberOnly()
		{
			My_Fists fists = new My_Fists("1234");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(fists.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}

		// test Validate with number and other data
		[Test]
		public void TestValidateNumberAndData()
		{
			My_Fists fists = new My_Fists("1234 Fred is dead");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(fists.Validate(out err,out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with no number
		[Test]
		public void TestValidateNoNumber()
		{
			My_Fists fists = new My_Fists("Fred is dead");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(fists.Validate(out err, out modStr));
			Assert.AreEqual("Fists must start with member's number.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate starting with non-number
		[Test]
		public void TestValidateStartWithoutNumber()
		{
			My_Fists fists = new My_Fists("F1234 red is dead");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(fists.Validate(out err, out modStr));
			Assert.AreEqual("Fists must start with member's number.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate starting with number including decimal point
		[Test]
		public void TestValidateDecimal()
		{
			My_Fists fists = new My_Fists("12.34");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(fists.Validate(out err, out modStr));
			Assert.AreEqual("Fists number must be digits only.", err);
			Assert.IsNull(modStr);
		}

		// test Validate starting with number including decimal point
		[Test]
		public void TestValidateDecimal2()
		{
			My_Fists fists = new My_Fists("12.34Fred");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(fists.Validate(out err, out modStr));
			Assert.AreEqual("Fists number must be digits only.", err);
			Assert.IsNull(modStr);
		}
	}
}
