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
	// tests for Call class
	[TestFixture]
	public class CallTests
	{
		// test fixture setup
		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			App.CallBureaus.LoadDocument();
		}

		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			Call call = new Call("VA3HJ");
			Assert.AreEqual("<Call:5>VA3HJ", call.ToAdifString());
		}
		
		// test ToAdifString for W4/VA3HJ/M
		[Test]
		public void TestToAdifStringPreSuf()
		{
			Call call = new Call("W4/VA3HJ/M");
			Assert.AreEqual("<Call:10>W4/VA3HJ/M", call.ToAdifString());
		}
		
		// test Validate with simple callsign
		[Test]
		public void TestValidateVA3HJ()
		{
			Call call = new Call("VA3HJ");
			string error = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(call.Validate(out error, out modStr));
			Assert.IsNull(error);
			Assert.IsNull(modStr);
		}
		
		// test Validate with simple callsign
		[Test]
		public void TestValidate6Y5BF()
		{
			Call call = new Call("6Y5BF");
			string error = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(call.Validate(out error, out modStr));
			Assert.IsNull(error);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid callsign
		[Test]
		public void TestValidateVAHJ()
		{
			Call call = new Call("VAHJ");
			string error = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(call.Validate(out error, out modStr));
			Assert.AreEqual("\tCallsign 'VAHJ' is invalid.", error);
			Assert.IsNull(modStr);
		}
		
		// test Validate with another invalid callsign
		[Test]
		public void TestValidate9A()
		{
			Call call = new Call("9A");
			string error = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(call.Validate(out error, out modStr));
			Assert.AreEqual("\tCallsign '9A' is invalid.", error);
			Assert.IsNull(modStr);
		}
		
		// test Validate with prefix
		[Test]
		public void TestValidatePrefix()
		{
			Call call = new Call("GB2/VA3HJ");
			string error = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(call.Validate(out error, out modStr));
			Assert.IsNull(error);
			Assert.IsNull(modStr);
		}
		
		// test Validate with empty callsign
		[Test]
		public void TestValidateEmpty()
		{
			Call call = new Call(string.Empty);
			string error = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(call.Validate(out error, out modStr));
			Assert.AreEqual("\tCallsign '' is invalid.", error);
			Assert.IsNull(modStr);
		}
		
		// test Validate with empty callsign
		[Test]
		public void TestValidateNull()
		{
			Call call = new Call(null);
			string error = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(call.Validate(out error, out modStr));
			Assert.AreEqual("\tNull callsign is invalid.", error);
			Assert.IsNull(modStr);
		}
		
		// test Validate for OEH20
		[Test]
		public void TestValidateOEH20()
		{
			Call call = new Call("OEH20");
			string error = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(call.Validate(out error, out modStr));
			Assert.IsNull(error);
			Assert.IsNull(modStr);
		}
		
		// test Validate for ZW1CCOM54
		[Test]
		public void TestValidateZW1CCOM54()
		{
			Call call = new Call("ZW1CCOM54");
			string error = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(call.Validate(out error, out modStr));
			Assert.IsNull(error);
			Assert.IsNull(modStr);
		}
		
		// test GetCall with no prefix or suffix
		[Test]
		public void TestGetCallNone()
		{
			Call call = new Call("VA3HJ");
			Assert.AreEqual("VA3HJ", call.GetCall());
		}
		
		// test GetCall with invalid callsign
		[Test]
		public void TestGetCallInvalidCall()
		{
			Call call = new Call("VAHJ");
			Assert.AreEqual(null, call.GetCall());
		}
		
		// test GetCall with prefix
		[Test]
		public void TestGetCallPrefix()
		{
			Call call = new Call("9A5/VA3HJ");
			Assert.AreEqual("VA3HJ", call.GetCall());
		}
		
		// test GetCall with suffix
		[Test]
		public void TestGetCallSuffix()
		{
			Call call = new Call("VA3HJ/P");
			Assert.AreEqual("VA3HJ", call.GetCall());
		}
		
		// test GetCall with 3D2R prefix
		[Test]
		public void TestGetCall3D2RPrefix()
		{
			Call call = new Call("3D2R/VA3HJ");
			Assert.AreEqual("VA3HJ", call.GetCall());
		}			
		
		// test GetCall with VP6D prefix
		[Test]
		public void TestGetCallVP6DPrefix()
		{
			Call call = new Call("VP6D/VA3HJ");
			Assert.AreEqual("VA3HJ", call.GetCall());
		}			
		
		// test GetCall with VP6D prefix
		[Test]
		public void TestGetCallR1FJPrefix()
		{
			Call call = new Call("R1FJ/UA0BMD");
			Assert.AreEqual("UA0BMD", call.GetCall());
		}
		
		// test GetCall with VP2E
		[Test]
		public void TestGetCallVP2E()
		{
			Call call = new Call("VP2E");
			Assert.AreEqual("VP2E", call.GetCall());
		}
		
		// test GetCall with VP2E prefix
		[Test]
		public void TestGetCallVP2EPrefix()
		{
			Call call = new Call("VP2E/VA3HJ");
			Assert.AreEqual("VA3HJ", call.GetCall());
		}
		
		// test GetCall with XM31812
		[Test]
		public void TestGetCallXM31812()
		{
			Call call = new Call("XM31812");
			Assert.AreEqual("XM31812", call.GetCall());
		}
		
		// test GetCall with XM31812
		[Test]
		public void TestGetCall5C5CW()
		{
			Call call = new Call("5C5CW");
			Assert.AreEqual("5C5CW", call.GetCall());
		}
		
		// test IsValid with null call
		[Test]
		public void TestIsValidNullCall()
		{
			Assert.IsFalse(Call.IsValid(null));
		}
		
		// test IsValid with empty call
		[Test]
		public void TestIsValidEmptyCall()
		{
			Assert.IsFalse(Call.IsValid(string.Empty));
		}
		
		// test IsValid with simple call
		[Test]
		public void TestIsValidSimpleCall()
		{
			Assert.IsTrue(Call.IsValid("VA3HJ"));
		}
		
		// test IsValid with invalid call
		[Test]
		public void TestIsValidBadCall()
		{
			Assert.IsFalse(Call.IsValid("VAHJ"));
		}
		
		// test IsValid with prefix
		[Test]
		public void TestIsValidWithPrefix()
		{
			Assert.IsFalse(Call.IsValid("GB2/VA3HJ"));
		}
		
		// test IsValid with suffix
		[Test]
		public void TestIsValidWithSuffix()
		{
			Assert.IsFalse(Call.IsValid("VA3HJ/M"));
		}
		
		// test IsValid for BM100
		[Test]
		public void TestIsValidBM100()
		{
			Assert.IsTrue(Call.IsValid("BM100"));
		}
		
		// test IsValid for BW100
		[Test]
		public void TestIsValidBW100()
		{
			Assert.IsTrue(Call.IsValid("BW100"));
		}
		
		// test IsValid for LM9L40Y
		[Test]
		public void TestIsValidLM9L40Y()
		{
			Assert.IsTrue(Call.IsValid("LM9L40Y"));
		}
		
		// test IsValid for SX1912
		[Test]
		public void TestIsValidSX1912()
		{
			Assert.IsTrue(Call.IsValid("SX1912"));
		}
		
		// test IsValid for 8J2KSG7X
		[Test]
		public void TestIsValid8J2KSG7X()
		{
			Assert.IsTrue(Call.IsValid("8J2KSG7X"));
		}
		
		
		// test IsValid for XV2V40J
		[Test]
		public void TestIsValidXV2V40J()
		{
			Assert.IsTrue(Call.IsValid("XV2V40J"));
		}
		// test IsValid for 5C5CW
		[Test]
		public void TestIsValid5C5CW()
		{
			Assert.IsTrue(Call.IsValid("5C5CW"));
		}
	}
}
