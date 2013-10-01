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
	// tests for DateField class
	[TestFixture]
	public class DateFieldTests
	{
		// test ToAdifField
		[Test]
		public void TestToAdifField()
		{
			DateField date = new DateField("19960412");
			Assert.AreEqual("<DateField:8>19960412", date.ToAdifString());
		}
		
		// test Validate with valid date
		[Test]
		public void TestValidateValidDate()
		{
			DateField date = new DateField("19960412");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(date.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with date that is too short
		[Test]
		public void TestValidateDateTooShort()
		{
			DateField date = new DateField("1996412");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(date.Validate(out err, out modStr));
			Assert.AreEqual("Date must be exactly 8 characters long.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with date that is too long
		[Test]
		public void TestValidateDateTooLong()
		{
			DateField date = new DateField("199604123");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(date.Validate(out err, out modStr));
			Assert.AreEqual("Date must be exactly 8 characters long.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with date containing non-numeric characters
		[Test]
		public void TestValidateNonNumeric()
		{
			DateField date = new DateField("1996MR12");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(date.Validate(out err,out modStr));
			Assert.AreEqual("Date must contain number characters only.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with date before 1930
		[Test]
		public void TestValidate1929()
		{
			DateField date = new DateField("19291231");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(date.Validate(out err, out modStr));
			Assert.AreEqual("Date must be 19300101 or later.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with date later than now
		[Test]
		public void TestValidateLaterThanNow()
		{
			DateTime now = DateTime.UtcNow;
			DateTime future = now.AddMonths(2);
			string strNow = string.Format("{0:yyyyMMdd}", future);
			DateField date = new DateField(strNow);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(date.Validate(out err, out modStr));
			Assert.AreEqual("Date must not be later than today.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid month (=00)
		[Test]
		public void TestValidateMonth00()
		{
			DateField date = new DateField("20130012");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(date.Validate(out err, out modStr));
			Assert.AreEqual("Invalid month in date: must be between 01 and 12.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid month (=13)
		[Test]
		public void TestValidateMonth13()
		{
			DateField date = new DateField("20121312");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(date.Validate(out err, out modStr));
			Assert.AreEqual("Invalid month in date: must be between 01 and 12.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid day (=00)
		[Test]
		public void TestValidateDay00()
		{
			DateField date = new DateField("20120400");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(date.Validate(out err,out modStr));
			Assert.AreEqual("Invalid day in date: must be between 01 and end of month.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid day (greater than last day of month)
		[Test]
		public void TestValidateDayAfterEndOfMonth()
		{
			DateField date = new DateField("20120431");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(date.Validate(out err, out modStr));
			Assert.AreEqual("Invalid day in date: must be between 01 and end of month.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with Feb 29 of leap year
		[Test]
		public void TestValidateDayFebLeapYear()
		{
			DateField date = new DateField("20000229");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(date.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
	}
}
