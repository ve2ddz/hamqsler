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
	// tests for TimeField class
	[TestFixture]
	public class TimeFieldTests
	{
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			TimeField tf = new TimeField("123456");
			Assert.AreEqual("<TimeField:6>123456", tf.ToAdifString());
		}
		
		// test Validate with empty time
		[Test]
		public void TestValidateValidEmpty()
		{
			TimeField tf = new TimeField("");
			string err = string.Empty;
			Assert.IsTrue(tf.Validate(out err));
			Assert.AreEqual(null, err);
		}
		
		// test Validate with valid HHMM time
		[Test]
		public void TestValidateValidHHMM()
		{
			TimeField tf = new TimeField("1234");
			string err = string.Empty;
			Assert.IsTrue(tf.Validate(out err));
			Assert.AreEqual(null, err);
		}
		
		// test Validate with valid HHMMSS time
		[Test]
		public void TestValidateValidHHMMSS()
		{
			TimeField tf = new TimeField("123456");
			string err = string.Empty;
			Assert.IsTrue(tf.Validate(out err));
			Assert.AreEqual(null, err);
		}
		
		// test Validate with invalid HHMMS time
		[Test]
		public void TestValidateInvalidHHM()
		{
			TimeField tf = new TimeField("123");
			string err = string.Empty;
			Assert.IsFalse(tf.Validate(out err));
			Assert.AreEqual("Time must be in HHMM or HHMMSS format.", err);
		}
		
		// test Validate with invalid HHMMS time
		[Test]
		public void TestValidateInvalidHHMMS()
		{
			TimeField tf = new TimeField("12345");
			string err = string.Empty;
			Assert.IsFalse(tf.Validate(out err));
			Assert.AreEqual("Time must be in HHMM or HHMMSS format.", err);
		}
		
		// test Validate with invalid hour
		[Test]
		public void TestValidateInvalidHour()
		{
			TimeField tf = new TimeField("243456");
			string err = string.Empty;
			Assert.IsFalse(tf.Validate(out err));
			Assert.AreEqual("Invalid time.", err);
		}
		
		// test Validate with invalid minute
		[Test]
		public void TestValidateInvalidMinute()
		{
			TimeField tf = new TimeField("126056");
			string err = string.Empty;
			Assert.IsFalse(tf.Validate(out err));
			Assert.AreEqual("Time must be in HHMM or HHMMSS format.", err);
		}
		
		// test Validate with invalid hour
		[Test]
		public void TestValidateInvalidHour2()
		{
			TimeField tf = new TimeField("2434");
			string err = string.Empty;
			Assert.IsFalse(tf.Validate(out err));
			Assert.AreEqual("Invalid time.", err);
		}
		
		// test Validate with invalid minute
		[Test]
		public void TestValidateInvalidMinute2()
		{
			TimeField tf = new TimeField("1260");
			string err = string.Empty;
			Assert.IsFalse(tf.Validate(out err));
			Assert.AreEqual("Time must be in HHMM or HHMMSS format.", err);
		}
		
		// test Validate with invalid second
		[Test]
		public void TestValidateInvalidSecond()
		{
			TimeField tf = new TimeField("125060");
			string err = string.Empty;
			Assert.IsFalse(tf.Validate(out err));
			Assert.AreEqual("Time must be in HHMM or HHMMSS format.", err);
		}
		
		// test Validate with non digit
		[Test]
		public void TestValidateNonDigit()
		{
			TimeField tf = new TimeField("1250-0");
			string err = string.Empty;
			Assert.IsFalse(tf.Validate(out err));
			Assert.AreEqual("Time must be in HHMM or HHMMSS format.", err);
		}
	}
}
