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
	// tests for Pfx class
	[TestFixture]
	public class PfxTests
	{
		// test ToAdifString()
		[Test]
		public void TestToAdifString()
		{
			Pfx pfx = new Pfx("VA3");
			Assert.AreEqual("<Pfx:3>VA3", pfx.ToAdifString());
		}
		
		// test Validate with valid prefix
		[Test]
		public void TestValidateValidVA3()
		{
			Pfx pfx = new Pfx("VA3");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(pfx.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with valid prefix
		[Test]
		public void TestValidateValidW4()
		{
			Pfx pfx = new Pfx("W4");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(pfx.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with valid prefix
		[Test]
		public void TestValidateValid9A()
		{
			Pfx pfx = new Pfx("9A");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(pfx.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
				
		// test Validate with valid prefix
		[Test]
		public void TestValidateValid5C5()
		{
			Pfx pfx = new Pfx("5C5");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(pfx.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}

		// test Validate with valid prefix
		[Test]
		public void TestValidateValidS58()
		{
			Pfx pfx = new Pfx("S58");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(pfx.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}

		// test Validate with valid prefix
		[Test]
		public void TestValidateValidJ79()
		{
			Pfx pfx = new Pfx("J79");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(pfx.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}

		// test Validate with invalid prefix
		[Test]
		public void TestValidateInvalid99()
		{
			Pfx pfx = new Pfx("99");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(pfx.Validate(out err, out modStr));
			Assert.AreEqual("\t'99' is not a valid prefix.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid prefix
		[Test]
		public void TestValidateInvalidWW()
		{
			Pfx pfx = new Pfx("WW");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(pfx.Validate(out err, out modStr));
			Assert.AreEqual("\t'WW' is not a valid prefix.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid prefix
		[Test]
		public void TestValidateInvalidWWE4()
		{
			Pfx pfx = new Pfx("WWE4");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(pfx.Validate(out err, out modStr));
			Assert.AreEqual("\t'WWE4' is not a valid prefix.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid prefix
		[Test]
		public void TestValidateInvalid9AW4()
		{
			Pfx pfx = new Pfx("9A/W4");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(pfx.Validate(out err, out modStr));
			Assert.AreEqual("\t'9A/W4' is not a valid prefix.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid prefix
		[Test]
		public void TestValidateInvalidTG9F()
		{
			Pfx pfx = new Pfx("TG9F");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(pfx.Validate(out err, out modStr));
			Assert.AreEqual("\t'TG9F' is not a valid prefix.", err);
			Assert.IsNull(modStr);
		}
	}
}
