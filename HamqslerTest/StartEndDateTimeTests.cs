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
	[TestFixture]
	public class StartEndDateTimeTests
	{
		// test ValidStartDate with valid date
		[Test]
		public void TestValidStartDateValidDate()
		{
			StartEndDateTime sedt = new StartEndDateTime();
			sedt.StartDate = "20130615";
			Assert.AreEqual("20130615", sedt.ValidStartDate);
		}

		// test ValidStartDate with future date
		[Test]
		public void TestInvalidStartDateFutureDate()
		{
			StartEndDateTime sedt = new StartEndDateTime();
			sedt.StartDate = "21000615";
			DateTime dt = DateTime.UtcNow;
			string today = string.Format("{0:d4}{1:d2}{2:d2}", dt.Year, dt.Month, dt.Day);
			// this may fail on very rare occasions when tests run right at midnight
			Assert.AreEqual(today, sedt.ValidStartDate);
		}


		// test ValidStartDate with date before 19300101
		[Test]
		public void TestInvalidStartDate19291231()
		{
			StartEndDateTime sedt = new StartEndDateTime();
			sedt.StartDate = "19291231";
			Assert.AreEqual("19300101", sedt.ValidStartDate);
		}

		// test ValidEndDate with valid date
		[Test]
		public void TestValidEndDateValidDate()
		{
			StartEndDateTime sedt = new StartEndDateTime();
			sedt.EndDate = "20130615";
			Assert.AreEqual("20130615", sedt.ValidEndDate);
		}

		// test ValidEndDate with future date
		[Test]
		public void TestInvalidEndDateFutureDate()
		{
			StartEndDateTime sedt = new StartEndDateTime();
			sedt.EndDate = "21000615";
			DateTime dt = DateTime.UtcNow;
			string today = string.Format("{0:d4}{1:d2}{2:d2}", dt.Year, dt.Month, dt.Day);
			// this may fail on very rare occasions when tests run right at midnight
			Assert.AreEqual(today, sedt.ValidEndDate);
		}

		// test ValidEndDate with date before 19300101
		[Test]
		public void TestInvalidEndDate19291231()
		{
			StartEndDateTime sedt = new StartEndDateTime();
			sedt.EndDate = "19291231";
			Assert.AreEqual("19300101", sedt.ValidEndDate);
		}

		// test ValidStartTime with valid time
		[Test]
		public void TestValidTimeValidTime()
		{
			StartEndDateTime sedt = new StartEndDateTime();
			sedt.StartTime = "201235";
			Assert.AreEqual("201235", sedt.ValidStartTime);
		}

		// test ValidStartTime with future date
		[Test]
		public void TestInvalidStartTimeInvalidTime()
		{
			StartEndDateTime sedt = new StartEndDateTime();
			sedt.StartTime = "2405";
			Assert.AreEqual("0000", sedt.ValidStartTime);
		}

		// test ValidEndTime with valid time
		[Test]
		public void TestValidEndTimeValidTime()
		{
			StartEndDateTime sedt = new StartEndDateTime();
			sedt.EndTime = "201235";
			Assert.AreEqual("201235", sedt.ValidEndTime);
		}

		// test ValidEndTime with future date
		[Test]
		public void TestInvalidEndTimeInvalidTime()
		{
			StartEndDateTime sedt = new StartEndDateTime();
			sedt.EndTime = "2405";
			Assert.AreEqual("0000", sedt.ValidEndTime);
		}

	}
}
