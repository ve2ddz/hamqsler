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
	/// Test class for NumberField class
	/// </summary>
	[TestFixture]
	public class NumberFieldTest
	{
		// test that IsValid returns true for integer value
		[Test]
		public void TestIsValid()
		{
			string err = string.Empty;
			NumberField nf = new NumberField("94");
			Assert.IsTrue(nf.Validate(out err));
			Assert.AreEqual(null, err);
		}
		
		// test IsValid returns true for decimal value 
		[Test]
		public void TestIsValid1()
		{
			string err = string.Empty;
			NumberField nf = new NumberField("94.21345");
			Assert.IsTrue(nf.Validate(out err));
			Assert.AreEqual(null, err);
		}
		
		// test IsValid returns true for decimal value starting with decimal separator
		// Note: Adif standard states that decimal separator is "."
		[Test]
		public void TestIsValid2()
		{
			string err = string.Empty;
			NumberField nf = new NumberField(".27");
			Assert.IsTrue(nf.Validate(out err));
			Assert.AreEqual(null, err);
		}
		
		// test IsValid returns true for decimal value ending with decimal separator
		[Test]
		public void TestIsValid3()
		{
			string err = string.Empty;
			NumberField nf = new NumberField("27.");
			Assert.IsTrue(nf.Validate(out err));
			Assert.AreEqual(null, err);
		}
		
		// test IsValid return true for empty string value
		[Test]
		public void TestIsValid4()
		{
			string err = string.Empty;
			NumberField nf = new NumberField("");
			Assert.IsTrue(nf.Validate(out err));
			Assert.AreEqual(null, err);
		}
		
		// test IsValid returns true for negative number
		[Test]
		public void TestIsValid5()
		{
			string err = string.Empty;
			NumberField nf = new NumberField("-5");
			Assert.IsTrue(nf.Validate(out err));
			Assert.AreEqual(null, err);
		}
		
		// test IsValid returns true for negative number with decimal sep
		[Test]
		public void TestIsValid6()
		{
			string err = string.Empty;
			NumberField nf = new NumberField("-2.3");
			Assert.IsTrue(nf.Validate(out err));
			Assert.AreEqual(null, err);
		}
		
		/// <summary>
		/// test that IsValid returns false for a non number value.
		/// </summary>
		[Test]
		public void TestIsNotValid()
		{
			string err = string.Empty;
			NumberField nf = new NumberField("fred");
			Assert.IsFalse(nf.Validate(out err));
			Assert.AreEqual("Value must be a number", err);
		}
		
		// test that IsValid returns false for decimal separator only
		[Test]
		public void TestIsNotValid1()
		{
			string err = string.Empty;
			NumberField nf = new NumberField(".");
			Assert.IsFalse(nf.Validate(out err));
			Assert.AreEqual("Value must be a number", err);
		}
		
		// test that IsValid returns false for 2 decimal separators
		[Test]
		public void TestIsNotValid2()
		{
			string err = string.Empty;
			NumberField nf = new NumberField("4.2.5");
			Assert.IsFalse(nf.Validate(out err));
			Assert.AreEqual("Value must be a number", err);
		}
	}
}
