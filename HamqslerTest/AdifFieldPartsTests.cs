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
	// test class for AdifFieldParts
	[TestFixture]
	public class AdifFieldPartsTests
	{
		// test Constructor valid string no DataType
		[Test]
		public void TestConstructorValidNoDataType()
		{
			AdifFieldParts parts = new AdifFieldParts("<Time:6>123456");
			Assert.AreEqual("Time", parts.Field);
			Assert.AreEqual("", parts.DataType);
			Assert.AreEqual("123456", parts.Value);
			Assert.AreEqual(null, parts.Enumeration);
		}
		
		// test Constructor valid string with whitespace before and after
		[Test]
		public void TestConstructorValidWhitespace()
		{
			AdifFieldParts parts = new AdifFieldParts(" <Time:6>123456 ");
			Assert.AreEqual("Time", parts.Field);
			Assert.AreEqual("", parts.DataType);
			Assert.AreEqual("123456", parts.Value);
			Assert.AreEqual(null, parts.Enumeration);
		}
		
		// test Constructor valid string with enumeration
		[Test]
		public void TestConstructorValidEnumeration()
		{
			AdifFieldParts parts = new AdifFieldParts("<Userdef1:19:E>SweaterSize,{S,M,L}");
			Assert.AreEqual("Userdef1", parts.Field);
			Assert.AreEqual("E", parts.DataType);
			Assert.AreEqual("SweaterSize", parts.Value);
			string err = string.Empty;
			Assert.IsTrue(parts.Enumeration.IsInEnumeration("S", out err));
			Assert.IsTrue(parts.Enumeration.IsInEnumeration("M", out err));
			Assert.IsTrue(parts.Enumeration.IsInEnumeration("L", out err));
			Assert.IsFalse(parts.Enumeration.IsInEnumeration("XL", out err));
		}
		
		// test Constructor valid string with range
		[Test]
		public void TestConstructorValidRange()
		{
			AdifFieldParts parts = new AdifFieldParts("<USERDEF3:8:N>ShoeSize,{5:20}");
			Assert.AreEqual("USERDEF3", parts.Field);
			Assert.AreEqual("N", parts.DataType);
			Assert.AreEqual("ShoeSize", parts.Value);
			string err = string.Empty;
			Assert.AreEqual("5", parts.LowerValue);
			Assert.AreEqual("20", parts.UpperValue);
		}

		// test Constructor valid string with float range
		[Test]
		public void TestConstructorValidRange2()
		{
			AdifFieldParts parts = new AdifFieldParts("<USERDEF3:8:N>ShoeSize,{5.2:20.}");
			Assert.AreEqual("USERDEF3", parts.Field);
			Assert.AreEqual("N", parts.DataType);
			Assert.AreEqual("ShoeSize", parts.Value);
			string err = string.Empty;
			Assert.AreEqual("5.2", parts.LowerValue);
			Assert.AreEqual("20.", parts.UpperValue);
		}
		
		// test Constructor valid string with range
		[Test]
		public void TestConstructorBadRange()
		{
			AdifFieldParts parts = new AdifFieldParts("<USERDEF3:8:N>ShoeSize,{20:5}");
			Assert.AreEqual(null, parts.Field);
		}
		
		// test Constructor valid string with range
		[Test]
		public void TestConstructorBadRange2()
		{
			AdifFieldParts parts = new AdifFieldParts("<USERDEF3:8:N>ShoeSize,{20:E}");
			Assert.AreEqual(null, parts.Field);
		}
	}
}
