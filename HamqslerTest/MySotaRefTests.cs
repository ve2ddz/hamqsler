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
	// tests for My_Sota_Ref class
	[TestFixture]
	public class MySotaRefTests
	{
		// test ToAdifString
		[Test]
		public void TestMethod()
		{
			My_Sota_Ref sota = new My_Sota_Ref("VE2/OU-036");
			Assert.AreEqual("<My_Sota_Ref:10>VE2/OU-036", sota.ToAdifString());
		}
		
		// test Validate with valid reference
		[Test]
		public void TestValidateValidSota()
		{
			My_Sota_Ref sota = new My_Sota_Ref("VE2/OU-036");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(sota.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with valid reference
		[Test]
		public void TestValidateValidSota1()
		{
			My_Sota_Ref sota = new My_Sota_Ref("9A/OU-036");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(sota.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid reference
		[Test]
		public void TestValidateInvalidSota()
		{
			My_Sota_Ref sota = new My_Sota_Ref("OU-036");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(sota.Validate(out err, out modStr));
			Assert.AreEqual("\t'OU-036' is not a valid SOTA Reference.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid reference
		[Test]
		public void TestValidateInvalidSota2()
		{
			My_Sota_Ref sota = new My_Sota_Ref("4/OU-036");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(sota.Validate(out err, out modStr));
			Assert.AreEqual("\t'4/OU-036' is not a valid SOTA Reference.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid reference
		[Test]
		public void TestValidateInvalidSota3()
		{
			My_Sota_Ref sota = new My_Sota_Ref("9A/O-036");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(sota.Validate(out err, out modStr));
			Assert.AreEqual("\t'9A/O-036' is not a valid SOTA Reference.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid reference
		[Test]
		public void TestValidateInvalidSota4()
		{
			My_Sota_Ref sota = new My_Sota_Ref("99A/O-036");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(sota.Validate(out err, out modStr));
			Assert.AreEqual("\t'99A/O-036' is not a valid SOTA Reference.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid reference
		[Test]
		public void TestValidateInvalidSota5()
		{
			My_Sota_Ref sota = new My_Sota_Ref("9A/OU-03");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(sota.Validate(out err, out modStr));
			Assert.AreEqual("\t'9A/OU-03' is not a valid SOTA Reference.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid reference
		[Test]
		public void TestValidateInvalidSota6()
		{
			My_Sota_Ref sota = new My_Sota_Ref("9A/OU-0365");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(sota.Validate(out err, out modStr));
			Assert.AreEqual("\t'9A/OU-0365' is not a valid SOTA Reference.", err);
			Assert.IsNull(modStr);
		}
	}
}
