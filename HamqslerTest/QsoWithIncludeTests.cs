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
	// tests for QsoWithInclude class
	[TestFixture]
	public class QsoWithIncludeTests
	{
		// test fixture setup
		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			Program.app.GetAdifEnumerations();
		}

		// test IDataErrorInfo interface with valid Manager
		[Test]
		public void TestValidManager()
		{
			Qsos2 qsos2 = new Qsos2();
			string err = string.Empty;
			Qso2 qso = new Qso2("<QSL_VIA:5>VA3HJ<eor>", App.AdifEnums, ref err, qsos2);
			QsoWithInclude qwi = new QsoWithInclude(qso);
			string error = qwi["Manager"];
			Assert.IsNull(error);
		}

		// test IDataErrorInfo interface with invalid Manager
		[Test]
		public void TestInvalidManager()
		{
			Qsos2 qsos2 = new Qsos2();
			string err = string.Empty;
			Qso2 qso = new Qso2("<QSL_VIA:6>bureau<eor>", App.AdifEnums, ref err, qsos2);
			QsoWithInclude qwi = new QsoWithInclude(qso);
			string error = qwi["Manager"];
			Assert.AreEqual("Must either be empty or a valid callsign", error);
		}


		// test IDataErrorInfo interface with invalid Manager (contains prefix)
		[Test]
		public void TestInvalidManagerPrefix()
		{
			Qsos2 qsos2 = new Qsos2();
			string err = string.Empty;
			Qso2 qso = new Qso2("<QSL_VIA:9>VP9/VA3HJ<eor>", App.AdifEnums, ref err, qsos2);
			QsoWithInclude qwi = new QsoWithInclude(qso);
			string error = qwi["Manager"];
			Assert.AreEqual("Must either be empty or a valid callsign", error);
		}
	}
}
