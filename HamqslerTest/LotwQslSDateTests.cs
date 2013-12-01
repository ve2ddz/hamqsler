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
using System.IO;
using System.Reflection;
using NUnit.Framework;
using hamqsler;

namespace hamqslerTest
{
	// tests for Lotw_QslSDate class
	[TestFixture]
	public class LotwQslSDateTests
	{
		// TestFixtureSetup
		[TestFixtureSetUp]
		public void TestSepup()
		{
			App.AdifEnums.LoadDocument();
		}
		
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
			string modStr = string.Empty;
			Assert.IsTrue(date.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid date
		[Test]
		public void TestValidateInvalidDate()
		{
			Lotw_QslSDate date = new Lotw_QslSDate("19250615");
			string err = string.Empty;
			string modStr =string.Empty;
			Assert.IsFalse(date.Validate(out err, out modStr));
			Assert.AreEqual("\tDate must be 19300101 or later.", err);
			Assert.IsNull(modStr);
		}
		
		// test ModifyValues with Lotw_Qsl_Sent value of Y, Q, I
		[Test]
		public void TestModifyValuesTrue(
			[Values("Y", "Q", "I")] string sent)
		{
			string adif = string.Format("<Lotw_Qsl_Sent:1>{0}<Lotw_QslSDate:8>20120916",
			 	                           sent);
			string err = string.Empty;
			Qso2 qso = new Qso2(adif, App.AdifEnums, ref err);
			Lotw_QslSDate date = qso.GetField("Lotw_QslSDate") as Lotw_QslSDate;
			Assert.IsNotNull(date);
			err = date.ModifyValues(qso);
			date = qso.GetField("Lotw_QslSDate") as Lotw_QslSDate;
			Assert.IsNotNull(date);
			Assert.IsNull(err);
		}
		
		// test ModifyValues with Lotw_Qsl_Sent value of N, R
		[Test]
		public void TestModifyValuesFalse(
			[Values("N", "R")] string sent)
		{
			string adif = string.Format("<Lotw_Qsl_Sent:1>{0}<Lotw_QslSDate:8>20120916",
			 	                           sent);
			string err = string.Empty;
			Qso2 qso = new Qso2(adif, App.AdifEnums, ref err);
			Lotw_QslSDate date = qso.GetField("Lotw_QslSDate") as Lotw_QslSDate;
			Assert.IsNotNull(date);
			err = date.ModifyValues(qso);
			date = qso.GetField("Lotw_QslSDate") as Lotw_QslSDate;
			Assert.IsNull(date);
			Assert.AreEqual("\tLotw_QslSDate field deleted. This field is only valid when Lotw_Qsl_Sent field is Y, Q, or I." +
			                 Environment.NewLine, err);
		}
		
		// test ModifyValues with no Lotw_Qsl_Sent
		[Test]
		public void TestModifyValuesNull()
		{
			string err = string.Empty;
			Qso2 qso = new Qso2("<Lotw_QslSDate:8>20120916", App.AdifEnums, ref err);
			Lotw_QslSDate date = qso.GetField("Lotw_QslSDate") as Lotw_QslSDate;
			Assert.IsNotNull(date);
			err = date.ModifyValues(qso);
			date = qso.GetField("Lotw_QslSDate") as Lotw_QslSDate;
			Assert.IsNull(date);
			Assert.AreEqual("\tLotw_QslSDate field deleted. This field is only valid when Lotw_Qsl_Sent field is Y, Q, or I." +
			                 Environment.NewLine, err);
		}
	}
}
