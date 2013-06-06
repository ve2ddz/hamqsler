// //  
// //  Author:
// //       Jim <jimorcheson@gmail.com>
// // 
// //  Copyright (c) 2011 VA3HJ Software
// //  Copyright (c) 2013 Jim Orcheson
// // 
// //  This program is free software: you can redistribute it and/or modify
// //  it under the terms of the GNU General Public License as published by
// //  the Free Software Foundation, either version 3 of the License, or
// //  (at your option) any later version.
// // 
// //  This program is distributed in the hope that it will be useful,
// //  but WITHOUT ANY WARRANTY; without even the implied warranty of
// //  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// //  GNU General Public License for more details.
// // 
// //  You should have received a copy of the GNU General Public License
// //  along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 
using System;
using NUnit.Framework;
using Qsos;
namespace QsosTests
{
	[TestFixture()]
	public class DateIsValidTests
	{
		// test DateIsValid with valid date
		[Test()]
		public void TestIsValidValidDate ()
		{
			Assert.IsTrue(DateTimeValidator.DateIsValid("20011014"));
		}
		
		// test DateIsValid with date before 19451101
		[Test]
		public void TestIsValidBefore1945()
		{
			Assert.IsFalse(DateTimeValidator.DateIsValid("19451031"));
		}
		
		// test DateIsValid with 0 month
		[Test]
		public void TestDateIsValidMonth0()
		{
			Assert.IsFalse(DateTimeValidator.DateIsValid("20110014"));
		}
		
		// test DateIsValid with 13 month
		[Test]
		public void TestDateIsValidMonth13()
		{
			Assert.IsFalse(DateTimeValidator.DateIsValid("20101304"));
		}
		
		// test DateIsValid with 0 day
		[Test]
		public void TestDateIsValidDay0()
		{
			Assert.IsFalse(DateTimeValidator.DateIsValid("20110400"));
		}
		
		// test DateIsValid with Jan day 32
		[Test]
		public void TestDateIsValidJan32()
		{
			Assert.IsFalse(DateTimeValidator.DateIsValid("20110132"));
		}
		
		// test DateIsValid with Feb day 29
		[Test]
		public void TestDateIsValidFeb29()
		{
			Assert.IsFalse(DateTimeValidator.DateIsValid("20110229"));
		}
		
		// test DateIsValid with Feb day 29 in leap year
		[Test]
		public void TestDateIsValidFeb29LeapYear()
		{
			Assert.IsTrue(DateTimeValidator.DateIsValid("20080229"));
		}
		
		// test DateIsValid with Mar day 32
		[Test]
		public void TestDateIsValidMar32()
		{
			Assert.IsFalse(DateTimeValidator.DateIsValid("20110332"));
		}
		
		// test DateIsValid with Apr day 31
		[Test]
		public void TestDateIsValidApr31()
		{
			Assert.IsFalse(DateTimeValidator.DateIsValid("20110431"));
		}
		
		// test DateIsValid with May day 32
		[Test]
		public void TestDateIsValidMay32()
		{
			Assert.IsFalse(DateTimeValidator.DateIsValid("20100532"));
		}
		
		// test DateIsValid with Jun day 31
		[Test]
		public void TestDateIsValidJun31()
		{
			Assert.IsFalse(DateTimeValidator.DateIsValid("20100631"));
		}
		
		// test DateIsValid with Jul day 32
		[Test]
		public void TestDateIsValidJul32()
		{
			Assert.IsFalse(DateTimeValidator.DateIsValid("20100732"));
		}
		
		// test DateIsValid with Aug day 32
		[Test]
		public void TestDateIsValidAug32()
		{
			Assert.IsFalse(DateTimeValidator.DateIsValid("20100832"));
		}
		
		// test DateIsValid with Sep day 31
		[Test]
		public void TestDateIsValidSep31()
		{
			Assert.IsFalse(DateTimeValidator.DateIsValid("20100931"));
		}
		
		// test DateIsValid with Oct day 32
		[Test]
		public void TestDateIsValidOct32()
		{
			Assert.IsFalse(DateTimeValidator.DateIsValid("20101032"));
		}
		
		// test DateIsValid with Nov day 31
		[Test]
		public void TestDateIsValidNov31()
		{
			Assert.IsFalse(DateTimeValidator.DateIsValid("20101131"));
		}
		
		// test DateIsValid with Dec day 32
		[Test]
		public void TestDateIsValidDec32()
		{
			Assert.IsFalse(DateTimeValidator.DateIsValid("20101232"));
		}
		
		// test DateIsValid with Dec day 31
		[Test]
		public void TestDateIsValidDec31()
		{
			Assert.IsTrue(DateTimeValidator.DateIsValid("20101231"));
		}
		
		// test DateIsValid with invalid characters
		[Test]
		public void TestDateIsValidInvalidChars()
		{
			Assert.IsFalse(DateTimeValidator.DateIsValid("c0111102"));
			Assert.IsFalse(DateTimeValidator.DateIsValid("200112-1"));
		}
		
		// test DateIsValid with less than 8 characters
		[Test]
		public void TestDateIsValid7Chars()
		{
			Assert.IsFalse(DateTimeValidator.DateIsValid("2001026"));
		}
		
		// test DateIsValid with more than 8 chars
		[Test]
		public void TestDateIsValid9Chars()
		{
			Assert.IsFalse(DateTimeValidator.DateIsValid("200112123"));
		}
	}
}

