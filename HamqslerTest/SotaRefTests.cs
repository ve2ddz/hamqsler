﻿/*
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
	// tests for Sota_Ref class
	[TestFixture]
	public class SotaRefTests
	{
		// test ToAdifString
		[Test]
		public void TestMethod()
		{
			Sota_Ref sota = new Sota_Ref("VE2/OU-036");
			Assert.AreEqual("<Sota_Ref:10>VE2/OU-036", sota.ToAdifString());
		}
		
		// test Validate with valid reference
		[Test]
		public void TestValidateValidSota()
		{
			Sota_Ref sota = new Sota_Ref("VE2/OU-036");
			string err = string.Empty;
			Assert.IsTrue(sota.Validate(out err));
			Assert.AreEqual(null, err);
		}
		
		// test Validate with valid reference
		[Test]
		public void TestValidateValidSota1()
		{
			Sota_Ref sota = new Sota_Ref("9A/OU-036");
			string err = string.Empty;
			Assert.IsTrue(sota.Validate(out err));
			Assert.AreEqual(null, err);
		}
		
		// test Validate with invalid reference
		[Test]
		public void TestValidateInvalidSota()
		{
			Sota_Ref sota = new Sota_Ref("OU-036");
			string err = string.Empty;
			Assert.IsFalse(sota.Validate(out err));
			Assert.AreEqual("'OU-036' is not a valid SOTA Reference.", err);
		}
		
		// test Validate with invalid reference
		[Test]
		public void TestValidateInvalidSota2()
		{
			Sota_Ref sota = new Sota_Ref("4/OU-036");
			string err = string.Empty;
			Assert.IsFalse(sota.Validate(out err));
			Assert.AreEqual("'4/OU-036' is not a valid SOTA Reference.", err);
		}
		
		// test Validate with invalid reference
		[Test]
		public void TestValidateInvalidSota3()
		{
			Sota_Ref sota = new Sota_Ref("9A/O-036");
			string err = string.Empty;
			Assert.IsFalse(sota.Validate(out err));
			Assert.AreEqual("'9A/O-036' is not a valid SOTA Reference.", err);
		}
		
		// test Validate with invalid reference
		[Test]
		public void TestValidateInvalidSota4()
		{
			Sota_Ref sota = new Sota_Ref("99A/O-036");
			string err = string.Empty;
			Assert.IsFalse(sota.Validate(out err));
			Assert.AreEqual("'99A/O-036' is not a valid SOTA Reference.", err);
		}
		
		// test Validate with invalid reference
		[Test]
		public void TestValidateInvalidSota5()
		{
			Sota_Ref sota = new Sota_Ref("9A/OU-03");
			string err = string.Empty;
			Assert.IsFalse(sota.Validate(out err));
			Assert.AreEqual("'9A/OU-03' is not a valid SOTA Reference.", err);
		}
		
		// test Validate with invalid reference
		[Test]
		public void TestValidateInvalidSota6()
		{
			Sota_Ref sota = new Sota_Ref("9A/OU-0365");
			string err = string.Empty;
			Assert.IsFalse(sota.Validate(out err));
			Assert.AreEqual("'9A/OU-0365' is not a valid SOTA Reference.", err);
		}
		
	}
}
