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
	// tests for Usaca_Counties class
	[TestFixture]
	public class UsacaCountiesTests
	{
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			Usaca_Counties ucs = new Usaca_Counties("MA,Franklin:MA,Hampshire");
			Assert.AreEqual("<Usaca_Counties:24>MA,Franklin:MA,Hampshire", ucs.ToAdifString());
		}
		
		// test Count
		[Test]
		public void TestCount()
		{
			Usaca_Counties ucs = new Usaca_Counties("MA,Franklin:MA,Hampshire");
			Assert.AreEqual(2, ucs.Count);
		}
		
		// test Validate with valid formats for the counties
		[Test]
		public void TestValidateValidCounties()
		{
			Usaca_Counties ucs = new Usaca_Counties("MA,Franklin:MA,Hampshire");
			string err = string.Empty;
			Assert.IsTrue(ucs.Validate(out err));
			Assert.AreEqual(null, err);
		}
		
		// test Validate with invalid format for first county
		[Test]
		public void TestValidateInvalidCounty1()
		{
			Usaca_Counties ucs = new Usaca_Counties("Franklin:MA,Hampshire");
			string err = string.Empty;
			Assert.IsFalse(ucs.Validate(out err));
			Assert.AreEqual("'Franklin' is not in correct county format.", err);
		}
		
		// test Validate with invalid format for second county
		[Test]
		public void TestValidateInvalidCounty2()
		{
			Usaca_Counties ucs = new Usaca_Counties("MA,Franklin:MASS,Hampshire");
			string err = string.Empty;
			Assert.IsFalse(ucs.Validate(out err));
			Assert.AreEqual("'MASS,Hampshire' is not in correct county format.", err);
		}
		
		// test Validate with three counties
		[Test]
		public void TestValidateThreeCounties()
		{
			Usaca_Counties ucs = new Usaca_Counties("MA,Franklin:MA,Hampshire:MA,Somerset");
			string err = string.Empty;
			Assert.IsFalse(ucs.Validate(out err));
			Assert.AreEqual("Usaca_Counties must contain exactly two counties.", err);
		}		

		// test Validate with one county
		[Test]
		public void TestValidateOneCounty()
		{
			Usaca_Counties ucs = new Usaca_Counties("MA,Franklin");
			string err = string.Empty;
			Assert.IsFalse(ucs.Validate(out err));
			Assert.AreEqual("Usaca_Counties must contain exactly two counties.", err);
		}		

		// test Validate with no counties
		[Test]
		public void TestValidateNoCounties()
		{
			Usaca_Counties ucs = new Usaca_Counties(string.Empty);
			string err = string.Empty;
			Assert.IsTrue(ucs.Validate(out err));
			Assert.AreEqual(null, err);
		}		
	}
}
