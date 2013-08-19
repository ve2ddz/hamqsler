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
	/// <summary>
	/// Description of EnumerationFieldTest.
	/// </summary>
	[TestFixture]
	public class EnumerationFieldTest
	{
		// test that Value is set properly
		[Test]
		public void TestValue()
		{
			string[] enums = {"e1", "e2", "e3", "e4"};
			EnumerationField ef = new EnumerationField("e1", enums);
			Assert.AreEqual("e1", ef.Value);
		}
		
		// test that Enumeration fields set properly
		[Test]
		public void TestEnumerationSet()
		{
			string[] enums = {"e1", "e2", "e3", "e4"};
			EnumerationField ef = new EnumerationField("e1", enums);
			Assert.AreEqual(enums, ef.Enumeration);
		}
		
		// test that value is in the enumeration
		[Test]
		public void TestValueInEnumeration()
		{
			string err = string.Empty;
			string[] enums = {"e1", "e2", "e3", "e4"};
			EnumerationField ef = new EnumerationField("e1", enums);
			Assert.IsTrue(ef.Validate(out err));
			Assert.AreEqual(null, err);
		}
		
		// test that value is in the enumeration
		[Test]
		public void TestValueInEnumeration1()
		{
			string err = string.Empty;
			string[] enums = {"e1", "e2", "e3", "e4"};
			EnumerationField ef = new EnumerationField("e4", enums);
			Assert.IsTrue(ef.Validate(out err));
			Assert.AreEqual(null, err);
		}
		
		// test that value is not in the enumeration
		[Test]
		public void TestValueNotInEnumeration()
		{
			string err = string.Empty;
			string[] enums = {"e1", "e2", "e3", "e4"};
			EnumerationField ef = new EnumerationField("e5", enums);
			Assert.IsFalse(ef.Validate(out err));
			Assert.AreEqual("This QSO Field is of type enumeration. The value was not found in enumeration",
			                err);
		}
	}
}
