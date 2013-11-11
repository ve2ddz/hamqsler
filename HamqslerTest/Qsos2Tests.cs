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
// tests for Qsos2 class
	[TestFixture]
	public class Qsos2Tests
	{
		AdifEnumerations aEnums;
		string error;
		Qsos2 qsos;
		// test fixture setup
		[TestFixtureSetUp]
		public void FixtureSetUp()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			aEnums = new AdifEnumerations(str);
		}
		
		// test setup
		[SetUp]
		public void Setup()
		{
			qsos = new Qsos2();
			error = string.Empty;
		}
		
		// test empty ADIF file
		[Test]
		public void TestEmptyAdifFile(
			[Values(null, "")] string adif)
		{
			Assert.IsFalse(qsos.Add(adif, ref error, aEnums));
			Assert.AreEqual("No QSOs found." + Environment.NewLine, error);
		}
		
		// test 1 QSO, no header
		[Test]
		public void Test1QSONoHeader()
		{
			string adif = "<mode:2>CW<band:3>10m<call:5>VA3HJ<Qso_Date:8>20131001<Time_On:4>1017<eor>";
			Assert.IsTrue(qsos.Add(adif, ref error, aEnums));
			Assert.AreEqual(1, qsos.Count);
		}
		
		// test no record
		[Test]
		public void TestNoRecord()
		{
			string adif = "<mode:2>CW<band:3>10m<call:5>VA3HJ<Qso_Date:8>20131001<Time_On:4>1017";
			Assert.IsFalse(qsos.Add(adif, ref error, aEnums));
			Assert.AreEqual("No QSOs found." + Environment.NewLine, error);
		}
		
		// test records with no eor on last record
		[Test]
		public void TestNoEorLastRecord()
		{
			string adif = "<mode:2>CW<band:3>10m<call:5>VA3HJ<Qso_Date:8>20131001<Time_On:4>1017<eor>" +
				"<mode:2>CW<band:3>10m<call:5>VA3HJ<Qso_Date:8>20121001<Time_On:4>1017<eor>" +
				"<mode:2>CW<band:3>10m<call:5>VA3HJ<Qso_Date:8>20111001<Time_On:4>1017";
			Assert.IsTrue(qsos.Add(adif, ref error, aEnums));
			Assert.AreEqual(2, qsos.Count);
			Assert.AreEqual("Data found after end of last QSO record: " +
			                "'<mode:2>CW<band:3>10m<call:5>VA3HJ<Qso_Date:8>20111001<Time_On:4>1017'"
			                + Environment.NewLine + Environment.NewLine, error);

		}
		
		// test header with QSOs
		[Test]
		public void TestHeader()
		{
			string adif = "some header text" + Environment.NewLine +
				"<adif_ver:5>3.0.4" + Environment.NewLine +
				"<eoh>" + Environment.NewLine +
				"<mode:2>CW<band:3>10m<call:5>VA3HJ<Qso_Date:8>20131001<Time_On:4>1017<eor>" +
				Environment.NewLine +
				"<mode:2>CW<band:3>10m<call:5>VA3HJ<Qso_Date:8>20121001<Time_On:4>1017<eor>";
			Assert.IsTrue(qsos.Add(adif, ref error, aEnums));
			Assert.AreEqual(2, qsos.Count);
			Assert.AreEqual(null, error);
		}
		
		// test header with UserDef and QSOs with no user defined fields
		[Test]
		public void TestUserDefNoFields()
		{
			string adif = "some header text" + Environment.NewLine +
				"<adif_ver:5>3.0.4" + Environment.NewLine +
				"<userdef1:8:N>QRP_ARCI" +Environment.NewLine +
				"<eoh>" + Environment.NewLine +
				"<mode:2>CW<band:3>10m<call:5>VA3HJ<Qso_Date:8>20131001<Time_On:4>1017<eor>" +
				Environment.NewLine +
				"<mode:2>CW<band:3>10m<call:5>VA3HJ<Qso_Date:8>20121001<Time_On:4>1017<eor>";
			Assert.IsTrue(qsos.Add(adif, ref error, aEnums));
			Assert.AreEqual(null, error);
			Assert.AreEqual(1, qsos.UserDefs.Count);
			Assert.AreEqual("QRP_ARCI", qsos.UserDefs[0].UName);
		}
		
		// test header with Userdef with invalid data type
		[Test]
		public void TestUserdefInvalidDataType()
		{
			string adif = "some header text" + Environment.NewLine +
				"<adif_ver:5>3.0.4" + Environment.NewLine +
				"<userdef1:8:H>QRP_ARCI" +Environment.NewLine +
				"<eoh>" + Environment.NewLine +
				"<mode:2>CW<band:3>10m<call:5>VA3HJ<Qso_Date:8>20131001<Time_On:4>1017<eor>" +
				Environment.NewLine +
				"<mode:2>CW<band:3>10m<call:5>VA3HJ<Qso_Date:8>20121001<Time_On:4>1017<eor>";
			Assert.IsTrue(qsos.Add(adif, ref error, aEnums));
			Assert.AreEqual("some header text" + Environment.NewLine +
							"<adif_ver:5>3.0.4" + Environment.NewLine +
							"<userdef1:8:H>QRP_ARCI" +Environment.NewLine +
							"\tUser Defined Field: 'QRP_ARCI" +
							"' does not contain valid data type. Field not added." +
							Environment.NewLine, error);
			Assert.AreEqual(0, qsos.UserDefs.Count);
		}
		
		// test headers with Userdef defined twice
		[Test]
		public void TestSameUserdefs()
		{
			string adif = "some header text" + Environment.NewLine +
				"<adif_ver:5>3.0.4" + Environment.NewLine +
				"<userdef1:8:N>QRP_ARCI" +Environment.NewLine +
				"<eoh>" + Environment.NewLine +
				"<mode:2>CW<band:3>10m<call:5>VA3HJ<Qso_Date:8>20131001<Time_On:4>1017<eor>" +
				Environment.NewLine +
				"<mode:2>CW<band:3>10m<call:5>VA3HJ<Qso_Date:8>20121001<Time_On:4>1017<eor>";
			Assert.IsTrue(qsos.Add(adif, ref error, aEnums));
			Assert.AreEqual(1, qsos.UserDefs.Count);
			Assert.IsTrue(qsos.Add(adif, ref error, aEnums));
			Assert.AreEqual(1, qsos.UserDefs.Count);
			Assert.AreEqual("some header text" + Environment.NewLine +
							"<adif_ver:5>3.0.4" + Environment.NewLine +
							"<userdef1:8:N>QRP_ARCI" +Environment.NewLine +
							"\tUser Defined Field: 'QRP_ARCI" +
							"' already defined. First definition retained." +
							Environment.NewLine, error);
		}
		
		// test header with Userdef having enumeration
		[Test]
		public void TestUserdefEnumeration()
		{
			string adif = "some header text" + Environment.NewLine +
				"<adif_ver:5>3.0.4" + Environment.NewLine +
				"<userdef1:19:E>SweaterSize,{S,M,L}" +Environment.NewLine +
				"<eoh>" + Environment.NewLine +
				"<mode:2>CW<band:3>10m<call:5>VA3HJ<Qso_Date:8>20131001<Time_On:4>1017<eor>" +
				Environment.NewLine +
				"<mode:2>CW<band:3>10m<call:5>VA3HJ<Qso_Date:8>20121001<Time_On:4>1017<eor>";
			Assert.IsTrue(qsos.Add(adif, ref error, aEnums));
			Assert.AreEqual(1, qsos.UserDefs.Count);
			Assert.AreEqual("SweaterSize", qsos.UserDefs[0].UName);
			Assert.AreEqual("S,M,L", qsos.UserDefs[0].EnumField.ToString());
		}
		
		// test header with Userdef enumeration type with no enumeration
		[Test]
		public void TestUserdefEnumTypeNoEnum()
		{
			string adif = "some header text" + Environment.NewLine +
				"<adif_ver:5>3.0.4" + Environment.NewLine +
				"<userdef1:11:E>SweaterSize" +Environment.NewLine +
				"<eoh>" + Environment.NewLine +
				"<mode:2>CW<band:3>10m<call:5>VA3HJ<Qso_Date:8>20131001<Time_On:4>1017<eor>" +
				Environment.NewLine +
				"<mode:2>CW<band:3>10m<call:5>VA3HJ<Qso_Date:8>20121001<Time_On:4>1017<eor>";
			Assert.IsTrue(qsos.Add(adif, ref error, aEnums));
			Assert.AreEqual(0, qsos.UserDefs.Count);
			Assert.AreEqual("some header text" + Environment.NewLine +
							"<adif_ver:5>3.0.4" + Environment.NewLine +
							"<userdef1:11:E>SweaterSize" +Environment.NewLine +
							"\tUser Defined Field: 'SweaterSize' is of type Enumeration, " +
			                "but no enumeration is supplied. Field not added." +
							Environment.NewLine, error);
		}
		
		// test header with Userdef having numerical limits
		[Test]
		public void TestUserdefLimits()
		{
			string adif = "some header text" + Environment.NewLine +
				"<adif_ver:5>3.0.4" + Environment.NewLine +
				"<userdef1:15:N>ShoeSize,{5:20}" +Environment.NewLine +
				"<eoh>" + Environment.NewLine +
				"<mode:2>CW<band:3>10m<call:5>VA3HJ<Qso_Date:8>20131001<Time_On:4>1017<eor>" +
				Environment.NewLine +
				"<mode:2>CW<band:3>10m<call:5>VA3HJ<Qso_Date:8>20121001<Time_On:4>1017<eor>";
			Assert.IsTrue(qsos.Add(adif, ref error, aEnums));
			Assert.AreEqual(1, qsos.UserDefs.Count);
			Assert.AreEqual("ShoeSize", qsos.UserDefs[0].UName);
			Assert.AreEqual("5", qsos.UserDefs[0].LowerValue);
			Assert.AreEqual("20", qsos.UserDefs[0].UpperValue);
		}
		
		// test header with Userdef having no numerical limits
		[Test]
		public void TestUserdefNumericalNoLimits()
		{
			string adif = "some header text" + Environment.NewLine +
				"<adif_ver:5>3.0.4" + Environment.NewLine +
				"<userdef1:8:N>ShoeSize" +Environment.NewLine +
				"<eoh>" + Environment.NewLine +
				"<mode:2>CW<band:3>10m<call:5>VA3HJ<Qso_Date:8>20131001<Time_On:4>1017<eor>" +
				Environment.NewLine +
				"<mode:2>CW<band:3>10m<call:5>VA3HJ<Qso_Date:8>20121001<Time_On:4>1017<eor>";
			Assert.IsTrue(qsos.Add(adif, ref error, aEnums));
			Assert.AreEqual(1, qsos.UserDefs.Count);
			Assert.AreEqual("ShoeSize", qsos.UserDefs[0].UName);
			Assert.AreEqual(string.Empty, qsos.UserDefs[0].LowerValue);
			Assert.AreEqual(string.Empty, qsos.UserDefs[0].UpperValue);
		}
		
		// test header with last Userdef having invalid length
		[Test]
		public void TestBadUserdefLength()
		{
			string adif = "some header text" + Environment.NewLine +
				"<adif_ver:5>3.0.4" + Environment.NewLine +
				"<userdef1:14:N>ShoeSize" +Environment.NewLine +
				"<eoh>" + Environment.NewLine +
				"<mode:2>CW<band:3>10m<call:5>VA3HJ<Qso_Date:8>20131001<Time_On:4>1017<eor>" +
				Environment.NewLine +
				"<mode:2>CW<band:3>10m<call:5>VA3HJ<Qso_Date:8>20121001<Time_On:4>1017<eor>";
			Assert.IsTrue(qsos.Add(adif, ref error, aEnums));
			Assert.AreEqual("some header text" + Environment.NewLine +
							"<adif_ver:5>3.0.4" + Environment.NewLine +
							"<userdef1:14:N>ShoeSize" +Environment.NewLine +
							"\tInvalid length specified for '<userdef1:14:N>' in header. '<userdef1:14:N>' not saved." +
			                Environment.NewLine, error);
		}
		
		// test multiple errors
		[Test]
		public void TestMultipleErrors()
		{
			string adif = "some header text" + Environment.NewLine +
				"<adif_ver:5>3.0.4" + Environment.NewLine +
				"<eoh>" + Environment.NewLine +
				"<mode:2>CW<band:3>10m<call:5>VA3HJ<Qso_Date:8>2013100<Time_On:4>1017<eor>" +
				Environment.NewLine +
				"<mode:2>CW<band:3>10m<call:5>VA3HJ<Qso_Date:8>20121001<Time_On:4>1017<eor>";
			Assert.IsTrue(qsos.Add(adif, ref error, aEnums));
			Assert.AreEqual("<mode:2>CW<band:3>10m<call:5>VA3HJ<Qso_Date:8>2013100<Time_On:4>1017<eor>" +
							Environment.NewLine +
							"\tQso_Date:2013100< - \tDate must contain number characters only. - Field deleted." +
							 Environment.NewLine + Environment.NewLine +
							 "\tInvalid QSO: Qso_Date not specified. - QSO not added." +
							 Environment.NewLine  + Environment.NewLine, error);
		}
		
		// test Import
		[Test]
		public void TestImport()
		{
			string adif = "some header text" + Environment.NewLine +
				"<adif_ver:5>3.0.4" + Environment.NewLine +
				"<userdef1:8:N>QRP_ARCI" +Environment.NewLine +
				"<eoh>" + Environment.NewLine +
				"<mode:2>CW<band:3>10m<call:5>VA3HJ<Qso_Date:8>20131001<Time_On:4>1017<eor>" +
				Environment.NewLine +
				"<mode:2>CW<band:3>10m<call:5>VA3HJ<Qso_Date:8>20121001<Time_On:4>1017<eor>";
			Assert.IsTrue(qsos.Import(adif, ref error, aEnums));
			Assert.AreEqual(1, qsos.UserDefs.Count);
			Assert.AreEqual(2, qsos.Count);
			Assert.IsTrue(qsos.Import(adif, ref error, aEnums));
			Assert.AreEqual(1, qsos.UserDefs.Count);
			Assert.AreEqual(null, error);
			Assert.AreEqual(2, qsos.Count);
		}

		// test QSO not added if missing one of required fields
		[Test]
		public void TestInvalidQsoNotAdded()
		{
			string adif = "some header text" + Environment.NewLine +
				"<adif_ver:5>3.0.4" + Environment.NewLine +
				"<userdef1:8:N>QRP_ARCI" +Environment.NewLine +
				"<eoh>" + Environment.NewLine +
				"<mode:2>CW<band:3>10m<call:5>VA3HJ<Qso_Date:8>20131001<Time_On:4>1017<eor>" +
				Environment.NewLine +
				"<mode:2>CW<band:3>10m<Qso_Date:8>20121001<Time_On:4>1017<eor>";
			Assert.IsTrue(qsos.Import(adif, ref error, aEnums));
			Assert.AreEqual(1, qsos.UserDefs.Count);
			Assert.AreEqual(1, qsos.Count);
		}

		// test Clear
		[Test]
		public void TestClear()
		{
			string adif = "some header text" + Environment.NewLine +
				"<adif_ver:5>3.0.4" + Environment.NewLine +
				"<userdef1:8:N>QRP_ARCI" +Environment.NewLine +
				"<eoh>" + Environment.NewLine +
				"<mode:2>CW<band:3>10m<call:5>VA3HJ<Qso_Date:8>20131001<Time_On:4>1017<eor>" +
				Environment.NewLine +
				"<mode:2>CW<band:3>10m<call:5>VA3HJ<Qso_Date:8>20121001<Time_On:4>1017<eor>";
			Assert.IsTrue(qsos.Import(adif, ref error, aEnums));
			Assert.AreEqual(1, qsos.UserDefs.Count);
			Assert.AreEqual(2, qsos.Count);
			qsos.ClearQsos();
			Assert.AreEqual(0, qsos.UserDefs.Count);
			Assert.AreEqual(null, error);
			Assert.AreEqual(0, qsos.Count);
		}
		
		// test duplicate Qso not added
		[Test]
		public void TestDuplicateQsoNotAdded()
		{
			string adif = "some header text" + Environment.NewLine +
				"<adif_ver:5>3.0.4" + Environment.NewLine +
				"<userdef1:8:N>QRP_ARCI" +Environment.NewLine +
				"<eoh>" + Environment.NewLine +
				"<mode:2>CW<band:3>10m<call:5>VA3HJ<Qso_Date:8>20131001<Time_On:4>1017<eor>" +
				Environment.NewLine +
				"<mode:2>CW<band:3>10m<call:5>VA3HJ<Qso_Date:8>20131001<Time_On:4>1017<eor>";
			Assert.IsTrue(qsos.Import(adif, ref error, aEnums));
			Assert.AreEqual(1, qsos.Count);
			
			Assert.IsTrue(qsos.Add(adif, ref error, aEnums));
			Assert.AreEqual(1, qsos.Count);
		}
		
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			string adif = "some header text" + Environment.NewLine +
				"<adif_ver:5>3.0.4" + Environment.NewLine +
				"<userdef1:8:N>QRP_ARCI" +Environment.NewLine +
				"<userdef2:19:E>SweaterSize,{S,M,L}" +Environment.NewLine +
				"<userdef3:15:N>ShoeSize,{5:20}" +Environment.NewLine +
				"<eoh>" + Environment.NewLine +
				"<mode:2>CW<band:3>10m<call:5>VA3HJ<Qso_Date:8>20131001<Time_On:4>1017<eor>" +
				Environment.NewLine +
				"<mode:2>CW<band:3>10m<call:6>VA3JNO<Qso_Date:8>20130801<Time_On:4>1017<eor>";
			Assert.IsTrue(qsos.Import(adif, ref error, aEnums));
			Assert.AreEqual(null, error);
			DateTime now = DateTime.Now.ToUniversalTime();
			string date = string.Format("{0:d4}{1:d2}{2:d2}", now.Year, now.Month, now.Day);
			string time = string.Format("{0:d2}{1:d2}{2:d2}", now.Hour, now.Minute, now.Second);
			string createdLine = string.Format("<created_timestamp:15>{0} {1}", date, time);
			string adifString = qsos.ToAdifString();
			string[] lines = adifString.Split('\r');
			if(!lines[2].Equals("\n" + createdLine))
			{
				now = now.AddSeconds(-1.0);
				date = string.Format("{0:d4}{1:d2}{2:d2}", now.Year, now.Month, now.Day);
				time = string.Format("{0:d2}{1:d2}{2:d2}", now.Hour, now.Minute, now.Second);
				createdLine = string.Format("\n<created_timestamp:15>{0} {1}", date, time);
				if(!lines[2].Equals("\n" + createdLine))
				{
					Assert.Fail("Date and time on created_timestamp line not within 1 second of current time");
				}
			}
				string adifVer = qsos.AdifEnums.Version;
				int index = adifVer.LastIndexOf('.');
				adifVer = adifVer.Substring(0, index);
				Assembly assembly = Assembly.GetAssembly(qsos.GetType());
				string version = assembly.GetName().Version.ToString();
				index = version.LastIndexOf('.');
				version = version.Substring(0, index);
				string compString = "Adif File" + Environment.NewLine +
				string.Format("<adif_ver:{0}>{1}", adifVer.Length, adifVer) + Environment.NewLine +
				createdLine + Environment.NewLine + 
				"<programid:8>HamQSLer" + Environment.NewLine +
				string.Format("<programversion:{0}>{1}", version.Length, version) +
				Environment.NewLine +
				"<Userdef1:8:N>QRP_ARCI" +Environment.NewLine +
				"<Userdef2:19:E>SweaterSize,{S,M,L}" +Environment.NewLine +
				"<Userdef3:15:N>ShoeSize,{5:20}" +Environment.NewLine +
				"<eoh>" + Environment.NewLine +
				"<Mode:2>CW<Band:3>10m<Call:5>VA3HJ<Qso_Date:8>20131001<Time_On:4>1017<eor>" +
				Environment.NewLine +
				"<Mode:2>CW<Band:3>10m<Call:6>VA3JNO<Qso_Date:8>20130801<Time_On:4>1017<eor>" +
				Environment.NewLine;
			Assert.AreEqual(compString, qsos.ToAdifString());

		}

		// test ModifyValues with missing field in each QSO
		[Test]
		public void TestModifyValuesWithMissingFieldInEachQso()
		{
			string adif = "some header text" + Environment.NewLine +
				"<adif_ver:5>3.0.4" + Environment.NewLine +
				"<userdef1:8:N>QRP_ARCI" +Environment.NewLine +
				"<eoh>" + Environment.NewLine +
				"<mode:2>CW<band:3>10m<call:5>VA3HJ<Time_On:4>1017<eor>" +
				Environment.NewLine +
				"<mode:2>CW<band:3>10m<call:5>VA3HJ<Qso_Date:8>20131001<Time_On:4>1017<eor>";
			Assert.IsTrue(qsos.Import(adif, ref error, aEnums));
			Assert.AreEqual(1, qsos.Count);
			Assert.AreEqual("<mode:2>CW<band:3>10m<call:5>VA3HJ<Time_On:4>1017<eor>" +
			                Environment.NewLine + "\tInvalid QSO: Qso_Date not specified. - QSO not added." +
			                Environment.NewLine + Environment.NewLine, error);
		}
	}
}
