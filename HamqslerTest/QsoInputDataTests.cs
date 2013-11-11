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
	// tests for QsoInputData class
	[TestFixture]
	public class QsoInputDataTests
	{
		// test IDataErrorInfo interface with valid Callsign
		[Test]
		public void TestValidCallsign()
		{
			QsoInputData iData = new QsoInputData();
			iData.Callsign = "VA3HJ";
			string error = iData["Callsign"];
			Assert.IsNull(error);
		}

		// test IDataErrorInfo interface with invalid Callsign
		[Test]
		public void TestInvalidCallsign()
		{
			QsoInputData iData = new QsoInputData();
			iData.Callsign = "VAHJ";
			string error = iData["Callsign"];
			Assert.AreEqual("Not a valid callsign", error);
		}

		// test IDataErrorInfo interface with valid Manager
		[Test]
		public void TestValidManager()
		{
			QsoInputData iData = new QsoInputData();
			iData.Manager = "VA3HJ";
			string error = iData["Manager"];
			Assert.IsNull(error);
		}

		// test IDataErrorInfo interface with valid Manager
		[Test]
		public void TestInvalidManagerModifiers()
		{
			QsoInputData iData = new QsoInputData();
			iData.Manager = "VA3HJ/W4";
			string error = iData["Manager"];
			Assert.AreEqual("Manager callsign must not contain modifiers (e.g. VA3HJ, not XE1/VA3HJ)",
			                error);
		}

		// test IDataErrorInfo interface with valid Manager
		[Test]
		public void TestInvalidManagerCall()
		{
			QsoInputData iData = new QsoInputData();
			iData.Manager = "VAHJ";
			string error = iData["Manager"];
			Assert.AreEqual("Not a valid callsign", error);
		}

		// test IDataErrorInfo interface with valid StartDate
		[Test]
		public void TestValidStartDate()
		{
			QsoInputData iData = new QsoInputData();
			iData.StartDate = "20130615";
			string error = iData["StartDate"];
			Assert.IsNull(error);
		}

		// test IDataErrorInfo interface with invalid StartDate
		[Test]
		public void TestInvalidStartDate()
		{
			QsoInputData iData = new QsoInputData();
			iData.StartDate = "19291231";
			string error = iData["StartDate"];
			Assert.AreEqual("\tDate must be 19300101 or later.", error);
		}

		// test IDataErrorInfo interface with valid StartTime
		[Test]
		public void TestValidStartTime()
		{
			QsoInputData iData = new QsoInputData();
			iData.StartTime = "122539";
			string error = iData["StartTime"];
			Assert.IsNull(error);
		}

		// test IDataErrorInfo interface with invalid StartDate
		[Test]
		public void TestInvalidStartTimeLength()
		{
			QsoInputData iData = new QsoInputData();
			iData.StartTime = "12253";
			string error = iData["StartTime"];
			Assert.AreEqual("\tTime must be in HHMM or HHMMSS format.", error);
		}

		// test IDataErrorInfo interface with invalid StartDate
		[Test]
		public void TestInvalidStartTimeBadTime()
		{
			QsoInputData iData = new QsoInputData();
			iData.StartTime = "252555";
			string error = iData["StartTime"];
			Assert.AreEqual("\tInvalid time.", error);
		}

		// test IDataErrorInfo interface with valid Mode
		[Test]
		public void TestValidMode()
		{
			QsoInputData iData = new QsoInputData();
			iData.Mode = "SSB";
			string error = iData["Mode"];
			Assert.IsNull(error);
		}


		// test IDataErrorInfo interface with null Mode
		[Test]
		public void TestNullMode()
		{
			QsoInputData iData = new QsoInputData();
			iData.Mode = null;
			string error = iData["Mode"];
			Assert.AreEqual("Value is null.", error);
		}

		// test IDataErrorInfo interface with invalid Mode
		[Test]
		public void TestInvalidMode()
		{
			QsoInputData iData = new QsoInputData();
			iData.Mode = "SQUIBB";
			string error = iData["Mode"];
			Assert.AreEqual(null, error);
		}

		// test IDataErrorInfo interface with valid band, no freq
		[Test]
		public void TestValidBandNoFreq()
		{
			QsoInputData iData = new QsoInputData();
			iData.Band = "10m";
			string error = iData["Band"];
			Assert.IsNull(error);
		}

		// test IDataErrorInfo interface with valid band and valid freq
		[Test]
		public void TestValidBandValidFreq()
		{
			QsoInputData iData = new QsoInputData();
			iData.Band = "10m";
			iData.Frequency = "28.453";
			string error = iData["Band"];
			Assert.IsNull(error);
		}

		// test IDataErrorInfo interface with valid band, invalid freq
		[Test]
		public void TestValidBandInvalidFreq()
		{
			QsoInputData iData = new QsoInputData();
			iData.Band = "10m";
			iData.Frequency = "27.453";
			string error = iData["Band"];
			Assert.AreEqual("Frequency is not within an enumerated ham band and therefore cannot be validated against this Band setting", error);
		}

		// test IDataErrorInfo interface with valid band, freq in diff band
		[Test]
		public void TestValidBandFreqDifferentBand()
		{
			QsoInputData iData = new QsoInputData();
			iData.Band = "10m";
			iData.Frequency = "21.203";
			string error = iData["Band"];
			Assert.AreEqual("Band does not contain the specified frequency", error);
		}

		// test IDataErrorInfo interface with valid freq, no band
		[Test]
		public void TestValidFreqNoBand()
		{
			QsoInputData iData = new QsoInputData();
			iData.Frequency = "21.203";
			string error = iData["Frequency"];
			Assert.IsNull(error);
		}

		// test IDataErrorInfo interface with valid band and valid freq
		[Test]
		public void TestValidFreqValidBand()
		{
			QsoInputData iData = new QsoInputData();
			iData.Band = "10m";
			iData.Frequency = "28.453";
			string error = iData["Frequency"];
			Assert.IsNull(error);
		}

		// test IDataErrorInfo interface with invalid band, valid freq
		[Test]
		public void TestValidFreqInvalidBand()
		{
			QsoInputData iData = new QsoInputData();
			iData.Band = "11m";
			iData.Frequency = "28.453";
			string error = iData["Frequency"];
			Assert.AreEqual("Frequency is not within the selected band", error);
		}

		// test IDataErrorInfo interface with valid band, freq in diff band
		[Test]
		public void TestValidFreqDifferentBand()
		{
			QsoInputData iData = new QsoInputData();
			iData.Band = "10m";
			iData.Frequency = "21.203";
			string error = iData["Frequency"];
			Assert.AreEqual("Frequency is not within the selected band", error);
		}
	}
}
