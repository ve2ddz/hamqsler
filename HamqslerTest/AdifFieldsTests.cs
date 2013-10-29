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
			string err = string.Empty;
			AdifFields fields = new AdifFields(record, ref err);
			Assert.AreEqual(count, fields.Count);
			Assert.AreEqual(null, err);
		}
		
		// test Accessors
		[Test]
		public void TestAccessors()
		{
			string record = "<mode:2:s>CW<Time_On:4:T>1017<adif_ver:5>3.0.4";
			string err = string.Empty;
			AdifFields fields = new AdifFields(record, ref err);
			Assert.AreEqual("mode", fields.FieldNames[0]);
			Assert.AreEqual("Time_On", fields.FieldNames[1]);
			Assert.AreEqual("adif_ver", fields.FieldNames[2]);
			Assert.AreEqual("s", fields.DataTypes[0]);
			Assert.AreEqual("T", fields.DataTypes[1]);
			Assert.AreEqual(string.Empty, fields.DataTypes[2]);
			Assert.AreEqual("CW", fields.Values[0]);
			Assert.AreEqual("1017", fields.Values[1]);
			Assert.AreEqual("3.0.4", fields.Values[2]);
		}
	}
}
