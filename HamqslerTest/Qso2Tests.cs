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
	/// <summary>
	/// tests for Qso2 class
	/// </summary>
	[TestFixture]
	public class Qso2Tests
	{
		AdifEnumerations aEnums;
		string errorString = string.Empty;
		// test fixture setup
		[TestFixtureSetUp]
		public void SetUp()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			aEnums = new AdifEnumerations(str);
		}
		
		// test Count
		[Test, Sequential]
		public void TestCount(
			[Values("<eor>", "<mode:2>CW<band:3>10m<call:5>VA3HJ<Qso_Date:8>20131001<Time_On:4>1017")]
			 string record,
			 [Values(0, 5)] int count)
		{
			Qso2 qso = new Qso2(record, aEnums, ref errorString);
			Assert.AreEqual(count, qso.Count);
			Assert.IsNullOrEmpty(errorString);
		}
		
		// test fields
		[Test]
		public void TestFields()
		{
			Qso2 qso = new Qso2( "<mode:2>CW<band:3>10m<call:5>VA3HJ  <Qso_Date:8>20131001<Time_On:4>1017",
			                    aEnums, ref errorString);
			Assert.AreEqual("Mode", qso.Fields[0].Name);
			Assert.AreEqual("Band", qso.Fields[1].Name);
			Assert.AreEqual("Call", qso.Fields[2].Name);
			Assert.AreEqual("Qso_Date", qso.Fields[3].Name);
			Assert.AreEqual("Time_On", qso.Fields[4].Name);
			Assert.AreEqual("CW", qso.Fields[0].Value);
			Assert.AreEqual("10m", qso.Fields[1].Value);
			Assert.AreEqual("VA3HJ", qso.Fields[2].Value);
			Assert.AreEqual("20131001", qso.Fields[3].Value);
			Assert.AreEqual("1017", qso.Fields[4].Value);
		}
		
		// test fields
		[Test]
		public void TestFields2()
		{
			Qso2 qso = new Qso2("<a_index:3>400" +
			                    "<address:52>" + "My Name" + Environment.NewLine +
			                 	"2124 Any Street" + Environment.NewLine +
			                 	"AnyTown Code" + Environment.NewLine +
			                 	"Some Country" +
			                 	"<age:2>99" +
			                 	"<ant_az:2>27 " +
			                 	"<ANT_EL:3>-84" +
			                 	"<ANT_PATH:1>L",
			                 	aEnums, ref errorString);
			Assert.AreEqual(6, qso.Count);
			Assert.AreEqual("A_Index", qso.Fields[0].Name);
			Assert.AreEqual("Address", qso.Fields[1].Name);
			Assert.AreEqual("Age", qso.Fields[2].Name);
			Assert.AreEqual("Ant_Az", qso.Fields[3].Name);
			Assert.AreEqual("Ant_El", qso.Fields[4].Name);
			Assert.AreEqual("Ant_Path", qso.Fields[5].Name);
			Assert.AreEqual("400", qso.Fields[0].Value);
			Assert.AreEqual("My Name" + Environment.NewLine +
			                 	"2124 Any Street" + Environment.NewLine +
			                 	"AnyTown Code" + Environment.NewLine +
			                 	"Some Country", qso.Fields[1].Value);
			Assert.AreEqual("99", qso.Fields[2].Value);
			Assert.AreEqual("27", qso.Fields[3].Value);
			Assert.AreEqual("-84", qso.Fields[4].Value);
			Assert.AreEqual("L", qso.Fields[5].Value);
		}

		// test fields
		[Test]
		public void TestFields3()
		{
			Qso2 qso = new Qso2("<arrl_sect:2>NT" +
			                    "<award_Granted:37>ARRL_DXCC_CW,DARCDOC_100,CQ_USACA_500" +
			                    "<award_Submitted:37>ARRL_DXCC_CW,DARCDOC_100,CQ_USACA_500" +
			                    "<Band_Rx:2>6m<Check:2>96<Class:2>3A",
			                    aEnums, ref errorString);
			Assert.AreEqual(6, qso.Count);
			Assert.AreEqual("Arrl_Sect", qso.Fields[0].Name);
			Assert.AreEqual("Award_Granted", qso.Fields[1].Name);
			Assert.AreEqual("Award_Submitted", qso.Fields[2].Name);
			Assert.AreEqual("Band_Rx", qso.Fields[3].Name);
			Assert.AreEqual("Check", qso.Fields[4].Name);
			Assert.AreEqual("Class", qso.Fields[5].Name);
			Assert.AreEqual("NT", qso.Fields[0].Value);
			Assert.AreEqual("ARRL_DXCC_CW,DARCDOC_100,CQ_USACA_500", qso.Fields[1].Value);
			Assert.AreEqual("ARRL_DXCC_CW,DARCDOC_100,CQ_USACA_500", qso.Fields[2].Value);
			Assert.AreEqual("6m", qso.Fields[3].Value);
			Assert.AreEqual("96", qso.Fields[4].Value);
			Assert.AreEqual("3A", qso.Fields[5].Value);
		}

		// test fields
		[Test]
		public void TestFields4()
		{
			Qso2 qso = new Qso2("<clublog_Qso_Upload_Date:8>20120816" +
			                    "<clublog_qso_upload_status:1>N" +
			                    "<cnty:11>MA,Somerset" +
			                    "<CoMMent:9>A comment" +
			                    "<Cont:2>NA" +
			                    "<Contacted_OP:5>VA3HJ",
			                    aEnums, ref errorString);
			Assert.AreEqual("Clublog_Qso_Upload_Date", qso.Fields[0].Name);
			Assert.AreEqual("Clublog_Qso_Upload_Status", qso.Fields[1].Name);
			Assert.AreEqual("Cnty", qso.Fields[2].Name);
			Assert.AreEqual("Comment", qso.Fields[3].Name);
			Assert.AreEqual("Cont", qso.Fields[4].Name);
			Assert.AreEqual("Contacted_Op", qso.Fields[5].Name);
			Assert.AreEqual(6, qso.Count);
			Assert.AreEqual("20120816", qso.Fields[0].Value);
			Assert.AreEqual("N", qso.Fields[1].Value);
			Assert.AreEqual("MA,Somerset", qso.Fields[2].Value);
			Assert.AreEqual("A comment", qso.Fields[3].Value);
			Assert.AreEqual("NA", qso.Fields[4].Value);
			Assert.AreEqual("VA3HJ", qso.Fields[5].Value);
		}

		// test fields
		[Test]
		public void TestFields5()
		{
			Qso2 qso = new Qso2("<Contest_Id:12>ON-QSO-PARTY" +
			                    "<Country:6>Canada" +
			                    "<CQZ:1>4" +
			                    "<Credit_Granted:29>CQDX:CARD,CQDX_BAND:CARD&LOTW" +
			                    "<Credit_Submitted:29>CQDX:CARD,CQDX_BAND:CARD&LOTW" +
			                    "<Distance:6>1097.6",
			                    aEnums, ref errorString);
			Assert.AreEqual("Contest_Id", qso.Fields[0].Name);
			Assert.AreEqual("Country", qso.Fields[1].Name);
			Assert.AreEqual("CQZ", qso.Fields[2].Name);
			Assert.AreEqual("Credit_Granted", qso.Fields[3].Name);
			Assert.AreEqual("Credit_Submitted", qso.Fields[4].Name);
			Assert.AreEqual("Distance", qso.Fields[5].Name);
			Assert.AreEqual(6, qso.Count);
			Assert.AreEqual("ON-QSO-PARTY", qso.Fields[0].Value);
			Assert.AreEqual("CANADA", qso.Fields[1].Value);
			Assert.AreEqual("4", qso.Fields[2].Value);
			Assert.AreEqual("CQDX:CARD,CQDX_BAND:CARD&LOTW", qso.Fields[3].Value);
			Assert.AreEqual("CQDX:CARD,CQDX_BAND:CARD&LOTW", qso.Fields[4].Value);
			Assert.AreEqual("1097.6", qso.Fields[5].Value);
		}

		// test fields
		[Test]
		public void TestFields6()
		{
			errorString = string.Empty;
			Qso2 qso = new Qso2("<DXCC:3>289" +
			                    "<Email:13>test@va3hj.ca" +
			                    "<eq_call:6>VA3JNO" +
			                    "<eqsl_Qslrdate:8>20130619" +
			                    "<eqsl_qslsdate:8>20130712" +
			                    "<eqsl_qsl_rcvd:1>Y",
			                    aEnums, ref errorString);
			Assert.AreEqual("DXCC", qso.Fields[0].Name);
			Assert.AreEqual("Email", qso.Fields[1].Name);
			Assert.AreEqual("Eq_Call", qso.Fields[2].Name);
			Assert.AreEqual("Eqsl_QslRDate", qso.Fields[3].Name);
			Assert.AreEqual("Eqsl_QslSDate", qso.Fields[4].Name);
			Assert.AreEqual("Eqsl_Qsl_Rcvd", qso.Fields[5].Name);
			Assert.AreEqual(6, qso.Count);
			Assert.AreEqual("289", qso.Fields[0].Value);
			Assert.AreEqual("test@va3hj.ca", qso.Fields[1].Value);
			Assert.AreEqual("VA3JNO", qso.Fields[2].Value);
			Assert.AreEqual("20130619", qso.Fields[3].Value);
			Assert.AreEqual("20130712", qso.Fields[4].Value);
			Assert.AreEqual("Y", qso.Fields[5].Value);
		}

		// test fields
		[Test]
		public void TestFields7()
		{
			errorString = string.Empty;
			Qso2 qso = new Qso2("<eqsl_qsl_sent:1>Y" +
			                    "<Fists:6>12345C" +
			                    "<Fists_CC:5>12345" +
			                    "<Force_init:1>N" +
			                    "<FREQ:5>7.035" +
			                    "<Freq_Rx:5>7.135",
			                    aEnums, ref errorString);
			Assert.AreEqual(null, errorString);
			Assert.AreEqual("Eqsl_Qsl_Sent", qso.Fields[0].Name);
			Assert.AreEqual("Fists", qso.Fields[1].Name);
			Assert.AreEqual("Fists_CC", qso.Fields[2].Name);
			Assert.AreEqual("Force_Init", qso.Fields[3].Name);
			Assert.AreEqual("Freq", qso.Fields[4].Name);
			Assert.AreEqual("Freq_Rx", qso.Fields[5].Name);
			Assert.AreEqual(6, qso.Count);
			Assert.AreEqual("Y", qso.Fields[0].Value);
			Assert.AreEqual("12345C", qso.Fields[1].Value);
			Assert.AreEqual("12345", qso.Fields[2].Value);
			Assert.AreEqual("N", qso.Fields[3].Value);
			Assert.AreEqual("7.035", qso.Fields[4].Value);
			Assert.AreEqual("7.135", qso.Fields[5].Value);
		}

		// test fields
		[Test]
		public void TestFields8()
		{
			errorString = string.Empty;
			Qso2 qso = new Qso2("<gridsquare:4>FN03" +
			                    "<Guest_Op:6>VA3JNO" +
			                    "<HrdLog_Qso_Upload_Date:8>20130912" +
			                    "<HrdLog_Qso_Upload_Status:1>Y" +
			                    "<IOTA:6>NA-001" +
			                    "<IOTA_ISland_id:12>Newfoundland",
			                    aEnums, ref errorString);
			Assert.AreEqual(null, errorString);
			Assert.AreEqual("GridSquare", qso.Fields[0].Name);
			Assert.AreEqual("Guest_Op", qso.Fields[1].Name);
			Assert.AreEqual("HrdLog_Qso_Upload_Date", qso.Fields[2].Name);
			Assert.AreEqual("HrdLog_Qso_Upload_Status", qso.Fields[3].Name);
			Assert.AreEqual("Iota", qso.Fields[4].Name);
			Assert.AreEqual("Iota_Island_ID", qso.Fields[5].Name);
			Assert.AreEqual(6, qso.Count);
			Assert.AreEqual("FN03", qso.Fields[0].Value);
			Assert.AreEqual("VA3JNO", qso.Fields[1].Value);
			Assert.AreEqual("20130912", qso.Fields[2].Value);
			Assert.AreEqual("Y", qso.Fields[3].Value);
			Assert.AreEqual("NA-001", qso.Fields[4].Value);
			Assert.AreEqual("Newfoundland", qso.Fields[5].Value);
		}

		// test fields
		[Test]
		public void TestFields9()
		{
			errorString = string.Empty;
			Qso2 qso = new Qso2("<ITUZ:2>90" +
			                    "<k_index:1>9" +
			                    "<Lat:11>N027 25.984" +
			                    "<Lon:11>W079 43.526" +
			                    "<Lotw_qslrdate:8>20130612" +
			                    "<LOTW_QSLSdate:8>20120502",
			                    aEnums, ref errorString);
			Assert.AreEqual(null, errorString);
			Assert.AreEqual("ITUZ", qso.Fields[0].Name);
			Assert.AreEqual("K_Index", qso.Fields[1].Name);
			Assert.AreEqual("Lat", qso.Fields[2].Name);
			Assert.AreEqual("Lon", qso.Fields[3].Name);
			Assert.AreEqual("Lotw_QslRDate", qso.Fields[4].Name);
			Assert.AreEqual("Lotw_QslSDate", qso.Fields[5].Name);
			Assert.AreEqual(6, qso.Count);
			Assert.AreEqual("90", qso.Fields[0].Value);
			Assert.AreEqual("9", qso.Fields[1].Value);
			Assert.AreEqual("N027 25.984", qso.Fields[2].Value);
			Assert.AreEqual("W079 43.526", qso.Fields[3].Value);
			Assert.AreEqual("20130612", qso.Fields[4].Value);
			Assert.AreEqual("20120502", qso.Fields[5].Value);
		}

		// test fields
		[Test]
		public void TestFields10()
		{
			errorString = string.Empty;
			Qso2 qso = new Qso2("<lotw_QSL_Rcvd:1>Y" +
			                    "<lotw_qsl_sent:1>Y" +
			                    "<Max_bursts:2>12" +
			                    "<ms_shower:8>Perseids" +
			                    "<my_city:13>Burlington ON" +
			                    "<my_CNTY:9>ON,Halton",
			                    aEnums, ref errorString);
			Assert.AreEqual(null, errorString);
			Assert.AreEqual("Lotw_Qsl_Rcvd", qso.Fields[0].Name);
			Assert.AreEqual("Lotw_Qsl_Sent", qso.Fields[1].Name);
			Assert.AreEqual("Max_Bursts", qso.Fields[2].Name);
			Assert.AreEqual("Ms_Shower", qso.Fields[3].Name);
			Assert.AreEqual("My_City", qso.Fields[4].Name);
			Assert.AreEqual("My_Cnty", qso.Fields[5].Name);
			Assert.AreEqual(6, qso.Count);
			Assert.AreEqual("Y", qso.Fields[0].Value);
			Assert.AreEqual("Y", qso.Fields[1].Value);
			Assert.AreEqual("12", qso.Fields[2].Value);
			Assert.AreEqual("Perseids", qso.Fields[3].Value);
			Assert.AreEqual("Burlington ON", qso.Fields[4].Value);
			Assert.AreEqual("ON,Halton", qso.Fields[5].Value);
		}

		// test fields
		[Test]
		public void TestFields11()
		{
			errorString = string.Empty;
			Qso2 qso = new Qso2("<my_country:6>Canada" +
			                    "<my_cq_zone:1>4" +
			                    "<My_DXCC:1>1" +
			                    "<MY_FISTS:4>1234" +
			                    "<My_Gridsquare:4>FN03" +
			                    "<My_IOTA:6>EU-126",
			                    aEnums, ref errorString);
			Assert.AreEqual(null, errorString);
			Assert.AreEqual("My_Country", qso.Fields[0].Name);
			Assert.AreEqual("My_CQ_Zone", qso.Fields[1].Name);
			Assert.AreEqual("My_DXCC", qso.Fields[2].Name);
			Assert.AreEqual("My_Fists", qso.Fields[3].Name);
			Assert.AreEqual("My_GridSquare", qso.Fields[4].Name);
			Assert.AreEqual("My_Iota", qso.Fields[5].Name);
			Assert.AreEqual(6, qso.Count);
			Assert.AreEqual("CANADA", qso.Fields[0].Value);
			Assert.AreEqual("4", qso.Fields[1].Value);
			Assert.AreEqual("1", qso.Fields[2].Value);
			Assert.AreEqual("1234", qso.Fields[3].Value);
			Assert.AreEqual("FN03", qso.Fields[4].Value);
			Assert.AreEqual("EU-126", qso.Fields[5].Value);
		}

		// test fields
		[Test]
		public void TestFields12()
		{
			errorString = string.Empty;
			Qso2 qso = new Qso2("<my_iota_island_id:4>Cuba" +
			                    "<my_itu_Zone:1>4" +
			                    "<my_Lat:11>N043 21.263" +
			                    "<my_lon:11>W079 43.795" +
			                    "<my_Name:3>Jim" +
			                    "<my_postal_code:7>L7R 1E1",
			                    aEnums, ref errorString);
			Assert.AreEqual(null, errorString);
			Assert.AreEqual("My_Iota_Island_ID", qso.Fields[0].Name);
			Assert.AreEqual("My_ITU_Zone", qso.Fields[1].Name);
			Assert.AreEqual("My_Lat", qso.Fields[2].Name);
			Assert.AreEqual("My_Lon", qso.Fields[3].Name);
			Assert.AreEqual("My_Name", qso.Fields[4].Name);
			Assert.AreEqual("My_Postal_Code", qso.Fields[5].Name);
			Assert.AreEqual(6, qso.Count);
			Assert.AreEqual("Cuba", qso.Fields[0].Value);
			Assert.AreEqual("4", qso.Fields[1].Value);
			Assert.AreEqual("N043 21.263", qso.Fields[2].Value);
			Assert.AreEqual("W079 43.795", qso.Fields[3].Value);
			Assert.AreEqual("Jim", qso.Fields[4].Value);
			Assert.AreEqual("L7R 1E1", qso.Fields[5].Value);
		}

		// test fields
		[Test]
		public void TestFields13()
		{
			errorString = string.Empty;
			Qso2 qso = new Qso2("<my_rig:12>Icom IC-7800" +
			                    "<my_SIG:3>CNE" +
			                    "<my_Sig_info:33>Canadian National Exhibition 2013" +
			                    "<MY_Sota_Ref:10>VE3/ON-046" +
			                    "<My_State:2>ON" +
			                    "<My_Street:13>360 Pearl St.",
			                    aEnums, ref errorString);
			Assert.AreEqual(null, errorString);
			Assert.AreEqual("My_Rig", qso.Fields[0].Name);
			Assert.AreEqual("My_Sig", qso.Fields[1].Name);
			Assert.AreEqual("My_Sig_Info", qso.Fields[2].Name);
			Assert.AreEqual("My_Sota_Ref", qso.Fields[3].Name);
			Assert.AreEqual("My_State", qso.Fields[4].Name);
			Assert.AreEqual("My_Street", qso.Fields[5].Name);
			Assert.AreEqual(6, qso.Count);
			Assert.AreEqual("Icom IC-7800", qso.Fields[0].Value);
			Assert.AreEqual("CNE", qso.Fields[1].Value);
			Assert.AreEqual("Canadian National Exhibition 2013", qso.Fields[2].Value);
			Assert.AreEqual("VE3/ON-046", qso.Fields[3].Value);
			Assert.AreEqual("ON", qso.Fields[4].Value);
			Assert.AreEqual("360 Pearl St.", qso.Fields[5].Value);
		}

		// test fields
		[Test]
		public void TestFields14()
		{
			errorString = string.Empty;
			Qso2 qso = new Qso2("<My_USACA_Counties:24>MA,Franklin:MA,Hampshire" +
			                    "<My_VUCC_Grids:19>EN98,FM08,EM97,FM07" +
			                    "<Name:4>Fred" +
			                    "<Notes:25>Some notes" + Environment.NewLine +
			                    "for this QSO." +
			                    "<nr_bursts:2>47" +
			                    "<nr_pings:2>32",
			                    aEnums, ref errorString);
			Assert.AreEqual(null, errorString);
			Assert.AreEqual("My_Usaca_Counties", qso.Fields[0].Name);
			Assert.AreEqual("My_VUCC_Grids", qso.Fields[1].Name);
			Assert.AreEqual("Name", qso.Fields[2].Name);
			Assert.AreEqual("Notes", qso.Fields[3].Name);
			Assert.AreEqual("Nr_Bursts", qso.Fields[4].Name);
			Assert.AreEqual("Nr_Pings", qso.Fields[5].Name);
			Assert.AreEqual(6, qso.Count);
			Assert.AreEqual("MA,Franklin:MA,Hampshire", qso.Fields[0].Value);
			Assert.AreEqual("EN98,FM08,EM97,FM07", qso.Fields[1].Value);
			Assert.AreEqual("Fred", qso.Fields[2].Value);
			Assert.AreEqual("Some notes" + Environment.NewLine +
			                    "for this QSO.", qso.Fields[3].Value);
			Assert.AreEqual("47", qso.Fields[4].Value);
			Assert.AreEqual("32", qso.Fields[5].Value);
		}
		
		// test fields
		[Test]
		public void TestFields15()
		{
			errorString = string.Empty;
			Qso2 qso = new Qso2("<Operator:6>VA3JNO" +
			                    "<Owner_Callsign:5>VA3HJ" +
			                    "<Pfx:3>VA3" +
			                    "<Precedence:2>96" +
			                    "<Prop_Mode:3>ECH" +
			                    "<Public_Key:16>A456FCBA234999FC",
			                    aEnums, ref errorString);
			Assert.AreEqual(null, errorString);
			Assert.AreEqual("Operator", qso.Fields[0].Name);
			Assert.AreEqual("Owner_Callsign", qso.Fields[1].Name);
			Assert.AreEqual("Pfx", qso.Fields[2].Name);
			Assert.AreEqual("Precedence", qso.Fields[3].Name);
			Assert.AreEqual("Prop_Mode", qso.Fields[4].Name);
			Assert.AreEqual("Public_Key", qso.Fields[5].Name);
			Assert.AreEqual(6, qso.Count);
			Assert.AreEqual("VA3JNO", qso.Fields[0].Value);
			Assert.AreEqual("VA3HJ", qso.Fields[1].Value);
			Assert.AreEqual("VA3", qso.Fields[2].Value);
			Assert.AreEqual("96", qso.Fields[3].Value);
			Assert.AreEqual("ECH", qso.Fields[4].Value);
			Assert.AreEqual("A456FCBA234999FC", qso.Fields[5].Value);
		}
		
		// test fields
		[Test]
		public void TestFields16()
		{
			errorString = string.Empty;
			Qso2 qso = new Qso2("<Qrzcom_Qso_Upload_Date:8>20130219" +
			                    "<Qrzcom_Qso_Upload_Status:1>N" +
			                    "<QslMsg:25>Qsl message" + Environment.NewLine +
			                    "More message" +
			                    "<QslRDate:8>19980328" +
			                    "<QslSDate:8>19960516" +
			                    "<Qsl_Rcvd:1>Y",
			                    aEnums, ref errorString);
			Assert.AreEqual(null, errorString);
			Assert.AreEqual("QrzCom_Qso_Upload_Date", qso.Fields[0].Name);
			Assert.AreEqual("QrzCom_Qso_Upload_Status", qso.Fields[1].Name);
			Assert.AreEqual("QslMsg", qso.Fields[2].Name);
			Assert.AreEqual("QslRDate", qso.Fields[3].Name);
			Assert.AreEqual("QslSDate", qso.Fields[4].Name);
			Assert.AreEqual("Qsl_Rcvd", qso.Fields[5].Name);
			Assert.AreEqual(6, qso.Count);
			Assert.AreEqual("20130219", qso.Fields[0].Value);
			Assert.AreEqual("N", qso.Fields[1].Value);
			Assert.AreEqual("Qsl message" + Environment.NewLine +
			                    "More message", qso.Fields[2].Value);
			Assert.AreEqual("19980328", qso.Fields[3].Value);
			Assert.AreEqual("19960516", qso.Fields[4].Value);
			Assert.AreEqual("Y", qso.Fields[5].Value);
		}
		
		// test fields
		[Test]
		public void TestFields17()
		{
			errorString = string.Empty;
			Qso2 qso = new Qso2("<Qsl_Rcvd_Via:1>B" +
			                    "<Qsl_Sent:1>Y" +
			                    "<Qsl_Sent_Via:1>B" +
			                    "<Qsl_Via:5>VA3HJ " +
			                    "<Qso_Complete:3>NIL" +
			                    "<Qso_Date_Off:8>19990801",
			                    aEnums, ref errorString);
			Assert.AreEqual(null, errorString);
			Assert.AreEqual("Qsl_Rcvd_Via", qso.Fields[0].Name);
			Assert.AreEqual("Qsl_Sent", qso.Fields[1].Name);
			Assert.AreEqual("Qsl_Sent_Via", qso.Fields[2].Name);
			Assert.AreEqual("Qsl_Via", qso.Fields[3].Name);
			Assert.AreEqual("Qso_Complete", qso.Fields[4].Name);
			Assert.AreEqual("Qso_Date_Off", qso.Fields[5].Name);
			Assert.AreEqual(6, qso.Count);
			Assert.AreEqual("B", qso.Fields[0].Value);
			Assert.AreEqual("Y", qso.Fields[1].Value);
			Assert.AreEqual("B", qso.Fields[2].Value);
			Assert.AreEqual("VA3HJ", qso.Fields[3].Value);
			Assert.AreEqual("NIL", qso.Fields[4].Value);
			Assert.AreEqual("19990801", qso.Fields[5].Value);
		}
		
		// test fields
		[Test]
		public void TestFields18()
		{
			errorString = string.Empty;
			Qso2 qso = new Qso2("<Qso_Random:1>Y" +
			                    "<Qth:7>Toronto" +
			                    "<Rig:14>Kenwood TS2000" +
			                    "<Rst_Rcvd:3>579" +
			                    "<Rst_Sent:3>-22" +
			                    "<Rx_Pwr:5>0.125",
			                    aEnums, ref errorString);
			Assert.AreEqual(null, errorString);
			Assert.AreEqual("Qso_Random", qso.Fields[0].Name);
			Assert.AreEqual("Qth", qso.Fields[1].Name);
			Assert.AreEqual("Rig", qso.Fields[2].Name);
			Assert.AreEqual("Rst_Rcvd", qso.Fields[3].Name);
			Assert.AreEqual("Rst_Sent", qso.Fields[4].Name);
			Assert.AreEqual("Rx_Pwr", qso.Fields[5].Name);
			Assert.AreEqual(6, qso.Count);
			Assert.AreEqual("Y", qso.Fields[0].Value);
			Assert.AreEqual("Toronto", qso.Fields[1].Value);
			Assert.AreEqual("Kenwood TS2000", qso.Fields[2].Value);
			Assert.AreEqual("579", qso.Fields[3].Value);
			Assert.AreEqual("-22", qso.Fields[4].Value);
			Assert.AreEqual("0.125", qso.Fields[5].Value);
		}
		
		// test fields
		[Test]
		public void TestFields19()
		{
			errorString = string.Empty;
			Qso2 qso = new Qso2("<Sat_Mode:2>FM" +
			                    "<Sat_Mode:5>AO-27" +
			                    "<SFI:3>107" +
			                    "<Sig:3>CNE" +
			                    "<Sig_Info:10>Some event" +
			                    "<SKCC:3>102",
			                    aEnums, ref errorString);
			Assert.AreEqual(null, errorString);
			Assert.AreEqual("Sat_Mode", qso.Fields[0].Name);
			Assert.AreEqual("Sat_Mode", qso.Fields[1].Name);
			Assert.AreEqual("SFI", qso.Fields[2].Name);
			Assert.AreEqual("Sig", qso.Fields[3].Name);
			Assert.AreEqual("Sig_Info", qso.Fields[4].Name);
			Assert.AreEqual("SKCC", qso.Fields[5].Name);
			Assert.AreEqual(6, qso.Count);
			Assert.AreEqual("FM", qso.Fields[0].Value);
			Assert.AreEqual("AO-27", qso.Fields[1].Value);
			Assert.AreEqual("107", qso.Fields[2].Value);
			Assert.AreEqual("CNE", qso.Fields[3].Value);
			Assert.AreEqual("Some event", qso.Fields[4].Value);
			Assert.AreEqual("102", qso.Fields[5].Value);
		}
		
		// test fields
		[Test]
		public void TestFields20()
		{
			errorString = string.Empty;
			Qso2 qso = new Qso2("<Sota_Ref:10>VE3/ON-046" +
			                    "<Srx:3>107" +
			                    "<Srx_String:10>Jim 96 107" +
			                    "<State:2>ON" +
			                    "<Station_Callsign:5>VA3HJ" +
			                    "<Stx:2>19",
			                    aEnums, ref errorString);
			Assert.AreEqual(null, errorString);
			Assert.AreEqual("Sota_Ref", qso.Fields[0].Name);
			Assert.AreEqual("Srx", qso.Fields[1].Name);
			Assert.AreEqual("Srx_String", qso.Fields[2].Name);
			Assert.AreEqual("State", qso.Fields[3].Name);
			Assert.AreEqual("Station_Callsign", qso.Fields[4].Name);
			Assert.AreEqual("Stx", qso.Fields[5].Name);
			Assert.AreEqual(6, qso.Count);
			Assert.AreEqual("VE3/ON-046", qso.Fields[0].Value);
			Assert.AreEqual("107", qso.Fields[1].Value);
			Assert.AreEqual("Jim 96 107", qso.Fields[2].Value);
			Assert.AreEqual("ON", qso.Fields[3].Value);
			Assert.AreEqual("VA3HJ", qso.Fields[4].Value);
			Assert.AreEqual("19", qso.Fields[5].Value);
		}
		
		// test fields
		[Test]
		public void TestFields21()
		{
			errorString = string.Empty;
			Qso2 qso = new Qso2("<Stx_String:10>Jim 96 107" +
			                    "<SubMode:5>PSK31" +
			                    "<SWL:1>N" +
			                    "<Ten_Ten:5>23174" +
			                    "<Time_Off:6>123456" +
			                    "<Tx_Pwr:3>100",
			                    aEnums, ref errorString);
			Assert.AreEqual(null, errorString);
			Assert.AreEqual("Stx_String", qso.Fields[0].Name);
			Assert.AreEqual("Submode", qso.Fields[1].Name);
			Assert.AreEqual("SWL", qso.Fields[2].Name);
			Assert.AreEqual("Ten_Ten", qso.Fields[3].Name);
			Assert.AreEqual("Time_Off", qso.Fields[4].Name);
			Assert.AreEqual("Tx_Pwr", qso.Fields[5].Name);
			Assert.AreEqual(6, qso.Count);
			Assert.AreEqual("Jim 96 107", qso.Fields[0].Value);
			Assert.AreEqual("PSK31", qso.Fields[1].Value);
			Assert.AreEqual("N", qso.Fields[2].Value);
			Assert.AreEqual("23174", qso.Fields[3].Value);
			Assert.AreEqual("123456", qso.Fields[4].Value);
			Assert.AreEqual("100", qso.Fields[5].Value);
		}
		
		// test fields
		[Test]
		public void TestFields22()
		{
			errorString = string.Empty;
			Qso2 qso = new Qso2("<Usaca_Counties:24>MA,Franklin:MA,Hampshire" +
			                    "<VE_Prov:2>ON" +
			                    "<VUCC_Grids:19>EN98,FM08,EM97,FM07" +
			                    "<Web:19>http://www.va3hj.ca",
			                    aEnums, ref errorString);
			Assert.AreEqual(null, errorString);
			Assert.AreEqual("Usaca_Counties", qso.Fields[0].Name);
			Assert.AreEqual("VE_Prov", qso.Fields[1].Name);
			Assert.AreEqual("VUCC_Grids", qso.Fields[2].Name);
			Assert.AreEqual("Web", qso.Fields[3].Name);
			Assert.AreEqual(4, qso.Count);
			Assert.AreEqual("MA,Franklin:MA,Hampshire", qso.Fields[0].Value);
			Assert.AreEqual("ON", qso.Fields[1].Value);
			Assert.AreEqual("EN98,FM08,EM97,FM07", qso.Fields[2].Value);
			Assert.AreEqual("http://www.va3hj.ca", qso.Fields[3].Value);
		}
		
		// test application defined fields
		[Test]
		public void TestApplicationDefinedField()
		{
			errorString = string.Empty;
			Qso2 qso = new Qso2("<APP_HAMQSLER_TEST:10:S>Test value", aEnums, ref errorString);
			Assert.AreEqual(null, errorString);
			Assert.AreEqual("APP_HAMQSLER_TEST", qso.Fields[0].Name);
			Assert.AreEqual("Test value", qso.Fields[0].Value);
		}
	}
}
