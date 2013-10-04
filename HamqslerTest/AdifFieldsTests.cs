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
using System.Collections.Generic;
using System.Text.RegularExpressions;
using NUnit.Framework;
using hamqsler;

namespace hamqslerTest
{
	// tests for AdifFields class
	[TestFixture]
	public class AdifFieldsTests
	{
		// test Constructor and Count Empty record
		[Test, Sequential]
		public void TestConstructorAndCountEmpty(
			[Values("", "<mode:2>CW<band:3>10m<call:5>VA3HJ<Qso_Date:8>20131001<Time_On:4>1017",
			       "<mode:2>CW<band:3>10m<call:5>VA3HJ<Qso_Date:8>20131001<Time_On:4>1017<eor>",
			       "<eor>")] string record,
			[Values(0, 5, 5, 0)] int count)
		{
			AdifFields fields = new AdifFields(record);
			Assert.AreEqual(count, fields.Count);
		}
	}
}
