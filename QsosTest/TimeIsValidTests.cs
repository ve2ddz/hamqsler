// //  
// //  Author:
// //       Jim <jimorcheson@gmail.com>
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
	public class TimeIsValidTests
	{
		// test TimeIsValid with valid time
		[Test()]
		public void TestTimeIsValid ()
		{
			Assert.IsTrue(DateTimeValidator.TimeIsValid("121426"));
		}
		
		// test TimeIsValid with hour 24
		[Test]
		public void TestTimeIsValidHour24()
		{
			Assert.IsFalse(DateTimeValidator.TimeIsValid("240615"));
		}
		
		// test TimeIsValid with minute 60
		[Test]
		public void TestTimeIsValidMinute60()
		{
			Assert.IsFalse(DateTimeValidator.TimeIsValid("236014"));
		}
		
		// test TimeIsValid with second 60
		[Test]
		public void TestTimeIsValidSecond60()
		{
			Assert.IsFalse(DateTimeValidator.TimeIsValid("230960"));
		}
		
		// test TimeIsValid with hour 23
		[Test]
		public void TestTimeIsValidHour23()
		{
			Assert.IsTrue(DateTimeValidator.TimeIsValid("230916"));
		}
		
		// test TimeIsValid with hour 0
		[Test]
		public void TestTimeIsValidHour0()
		{
			Assert.IsTrue(DateTimeValidator.TimeIsValid("000913"));
		}
		
		// test TimeIsValid with minute 59
		[Test]
		public void TestTimeIsValidMinute59()
		{
			Assert.IsTrue(DateTimeValidator.TimeIsValid("055913"));
		}
		
		// test TimeIsValid with minute 0
		[Test]
		public void TestTimeIsValidMinute0()
		{
			Assert.IsTrue(DateTimeValidator.TimeIsValid("050013"));
		}
		
		// test TimeIsValid with second 59
		[Test]
		public void TestTimeIsValidSecond59()
		{
			Assert.IsTrue(DateTimeValidator.TimeIsValid("000059"));
		}
		
		// test TimeIsValid with second 0
		[Test]
		public void TestTimeIsValidSecond0()
		{
			Assert.IsTrue(DateTimeValidator.TimeIsValid("000000"));
		}
		
		// test TimeIsValid with time HHMM
		[Test]
		public void TestTimeIsValidHHMM()
		{
			Assert.IsTrue(DateTimeValidator.TimeIsValid("2359"));
		}
		
		// test TimeIsValid with time HHMM, hour 24
		[Test]
		public void TestTimeIsValid2400()
		{
			Assert.IsFalse(DateTimeValidator.TimeIsValid("2400"));
		}
		
		// test TimeIsValid with time HHMM, minute 60
		[Test]
		public void TestTimeIsValid0560()
		{
			Assert.IsFalse(DateTimeValidator.TimeIsValid("0560"));
		}
		
		// test TimeIsValid with time 0000
		[Test]
		public void TestTimeIsValid0000()
		{
			Assert.IsTrue(DateTimeValidator.TimeIsValid("0000"));
		}
		
		// test TimeIsValid with time only 3 digits
		[Test]
		public void TestTimeIsValid3Digits()
		{
			Assert.IsFalse(DateTimeValidator.TimeIsValid("056"));
		}
		
		// test TimeIsValid with time of 5 digits
		[Test]
		public void TestTimeIsValid5Digits()
		{
			Assert.IsFalse(DateTimeValidator.TimeIsValid("00561"));
		}
		
		// test TimeIsValid with time of 7 digits
		[Test]
		public void TestTimeIsValid7Digits()
		{
				Assert.IsFalse(DateTimeValidator.TimeIsValid("0056123"));
		}
		
		// test TimeIsValid with invalid chars
		[Test]
		public void TestTimeIsValidInvalidChars()
		{
			Assert.IsFalse(DateTimeValidator.TimeIsValid("-01125"));
			Assert.IsFalse(DateTimeValidator.TimeIsValid("03d512"));
			Assert.IsFalse(DateTimeValidator.TimeIsValid("031f"));
		}
	}
}

