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
			                "<mode:2>CW<band:3>10m<call:5>VA3HJ<Qso_Date:8>20111001<Time_On:4>1017" +
			                Environment.NewLine, error);

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
			Assert.AreEqual("User Defined Field: 'QRP_ARCI" + 
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
			Assert.AreEqual("User Defined Field: 'QRP_ARCI" +
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
			Assert.AreEqual("User Defined Field: 'SweaterSize' is of type Enumeration, " +
			                "but no enumeration is supplied. Field not added.", error);
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
			Assert.AreEqual("Invalid length specified for 'userdef1' in header. 'userdef1' not saved." +
			                Environment.NewLine, error);
		}
	}
}
