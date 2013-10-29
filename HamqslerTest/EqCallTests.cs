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
	// tests for Eq_Call class
	[TestFixture]
	public class EqCallTests
	{
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			Eq_Call call = new Eq_Call("VA3HJ");
			Assert.AreEqual("<Eq_Call:5>VA3HJ", call.ToAdifString());
		}
		
		// test Validate with valid callsign
		[Test]
		public void TestValidateValidCall()
		{
			Eq_Call call = new Eq_Call("VA3HJ");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(call.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
				
		// test Validate with compound callsign
		[Test]
		public void TestValidateCompoundCall()
		{
			Eq_Call call = new Eq_Call("VP9/VA3HJ");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(call.Validate(out err, out modStr));
			Assert.AreEqual("\t'VP9/VA3HJ' is not a valid callsign.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid callsign
		[Test]
		public void TestValidateInvalidCall()
		{
			Eq_Call call = new Eq_Call("VAHJ");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(call.Validate(out err, out modStr));
			Assert.AreEqual("\t'VAHJ' is not a valid callsign.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with special callsign
		[Test]
		public void TestValidateSpecialCall()
		{
			Eq_Call call = new Eq_Call("8J2KSG7X");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(call.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
	}
}
