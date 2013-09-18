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
	// tests for Lotw_QslSDate class
	[TestFixture]
	public class LotwQslSDateTests
	{
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			Lotw_QslSDate date = new Lotw_QslSDate("19990615");
			Assert.AreEqual("<Lotw_QslSDate:8>19990615", date.ToAdifString());
		}
		
		// test Validate with valid date
		[Test]
		public void TestValidateValidDate()
		{
			Lotw_QslSDate date = new Lotw_QslSDate("19990615");
			string err = string.Empty;
			Assert.IsTrue(date.Validate(out err));
			Assert.AreEqual(null, err);
		}
		
		// test Validate with invalid date
		[Test]
		public void TestValidateInvalidDate()
		{
			Lotw_QslSDate date = new Lotw_QslSDate("19250615");
			string err = string.Empty;
			Assert.IsFalse(date.Validate(out err));
			Assert.AreEqual("Date must be 19300101 or later", err);
		}
	}
}