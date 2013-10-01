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
	// tests for Time_Off class
	[TestFixture]
	public class TimeOffTests
	{
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			Time_Off tf = new Time_Off("123456");
			Assert.AreEqual("<Time_Off:6>123456", tf.ToAdifString());
		}
		
		// test Validate with empty time
		[Test]
		public void TestValidateValidEmpty()
		{
			Time_Off tf = new Time_Off("");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(tf.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with valid HHMM time
		[Test]
		public void TestValidateValidHHMM()
		{
			Time_Off tf = new Time_Off("1234");
			string err = string.Empty;
			string modStr =string.Empty;
			Assert.IsTrue(tf.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with valid HHMMSS time
		[Test]
		public void TestValidateValidHHMMSS()
		{
			Time_Off tf = new Time_Off("123456");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(tf.Validate(out err,out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid HHMMS time
		[Test]
		public void TestValidateInvalidHHM()
		{
			Time_Off tf = new Time_Off("123");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(tf.Validate(out err, out modStr));
			Assert.AreEqual("Time must be in HHMM or HHMMSS format.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid HHMMS time
		[Test]
		public void TestValidateInvalidHHMMS()
		{
			Time_Off tf = new Time_Off("12345");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(tf.Validate(out err, out modStr));
			Assert.AreEqual("Time must be in HHMM or HHMMSS format.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid hour
		[Test]
		public void TestValidateInvalidHour()
		{
			Time_Off tf = new Time_Off("243456");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(tf.Validate(out err, out modStr));
			Assert.AreEqual("Invalid time.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid minute
		[Test]
		public void TestValidateInvalidMinute()
		{
			Time_Off tf = new Time_Off("126056");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(tf.Validate(out err,out modStr));
			Assert.AreEqual("Time must be in HHMM or HHMMSS format.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid hour
		[Test]
		public void TestValidateInvalidHour2()
		{
			Time_Off tf = new Time_Off("2434");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(tf.Validate(out err, out modStr));
			Assert.AreEqual("Invalid time.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid minute
		[Test]
		public void TestValidateInvalidMinute2()
		{
			Time_Off tf = new Time_Off("1260");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(tf.Validate(out err, out modStr));
			Assert.AreEqual("Time must be in HHMM or HHMMSS format.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid second
		[Test]
		public void TestValidateInvalidSecond()
		{
			Time_Off tf = new Time_Off("125060");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(tf.Validate(out err, out modStr));
			Assert.AreEqual("Time must be in HHMM or HHMMSS format.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with non digit
		[Test]
		public void TestValidateNonDigit()
		{
			Time_Off tf = new Time_Off("1250-0");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(tf.Validate(out err, out modStr));
			Assert.AreEqual("Time must be in HHMM or HHMMSS format.", err);
			Assert.IsNull(modStr);
		}
	}
}
