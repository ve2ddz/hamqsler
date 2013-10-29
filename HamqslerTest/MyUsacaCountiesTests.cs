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
	// tests for My_Usaca_Counties
	[TestFixture]
	public class MyUsacaCountiesTests
	{
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			My_Usaca_Counties ucs = new My_Usaca_Counties("MA,Franklin:MA,Hampshire");
			Assert.AreEqual("<My_Usaca_Counties:24>MA,Franklin:MA,Hampshire", ucs.ToAdifString());
		}
		
		// test Count
		[Test]
		public void TestCount()
		{
			My_Usaca_Counties ucs = new My_Usaca_Counties("MA,Franklin:MA,Hampshire");
			Assert.AreEqual(2, ucs.Count);
		}
		
		// test Validate with valid formats for the counties
		[Test]
		public void TestValidateValidCounties()
		{
			My_Usaca_Counties ucs = new My_Usaca_Counties("MA,Franklin:MA,Hampshire");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(ucs.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid format for first county
		[Test]
		public void TestValidateInvalidCounty1()
		{
			My_Usaca_Counties ucs = new My_Usaca_Counties("Franklin:MA,Hampshire");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(ucs.Validate(out err, out modStr));
			Assert.AreEqual("\t'Franklin' is not in correct county format.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid format for second county
		[Test]
		public void TestValidateInvalidCounty2()
		{
			My_Usaca_Counties ucs = new My_Usaca_Counties("MA,Franklin:MASS,Hampshire");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(ucs.Validate(out err, out modStr));
			Assert.AreEqual("\t'MASS,Hampshire' is not in correct county format.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with three counties
		[Test]
		public void TestValidateThreeCounties()
		{
			My_Usaca_Counties ucs = new My_Usaca_Counties("MA,Franklin:MA,Hampshire:MA,Somerset");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(ucs.Validate(out err, out modStr));
			Assert.AreEqual("My_Usaca_Counties must contain exactly two counties.", err);
			Assert.IsNull(modStr);
		}		

		// test Validate with one county
		[Test]
		public void TestValidateOneCounty()
		{
			My_Usaca_Counties ucs = new My_Usaca_Counties("MA,Franklin");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(ucs.Validate(out err, out modStr));
			Assert.AreEqual("My_Usaca_Counties must contain exactly two counties.", err);
			Assert.IsNull(modStr);
		}		

		// test Validate with no counties
		[Test]
		public void TestValidateNoCounties()
		{
			My_Usaca_Counties ucs = new My_Usaca_Counties(string.Empty);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(ucs.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}		
	}
}
