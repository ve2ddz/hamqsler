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
	// tests for Station_Callsign class
	[TestFixture]
	public class StationCallsignTests
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
			Station_Callsign oCall = new Station_Callsign("VA3HJ");
			Assert.AreEqual("<Station_Callsign:5>VA3HJ", oCall.ToAdifString());
		}
		
		// test Validate with valid callsign
		[Test]
		public void TestValidateValidCall()
		{
			Station_Callsign oCall = new Station_Callsign("VA3HJ");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(oCall.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid callsign
		[Test]
		public void TestValidateInvalidCall()
		{
			Station_Callsign oCall = new Station_Callsign("VAHJ");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(oCall.Validate(out err, out modStr));
			Assert.AreEqual("\tCallsign 'VAHJ' is invalid.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with compound callsign
		[Test]
		public void TestValidateCompoundCall()
		{
			Station_Callsign oCall = new Station_Callsign("VA3HJ/W8");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(oCall.Validate(out err, out modStr));
			Assert.AreEqual(null, err);
			Assert.IsNull(modStr);
		}
		
		// test ModifyValues with no Owner_Callsign
		[Test]
		public void TestModifyValuesNoOwnerCallsign()
		{
			string err = string.Empty;
			Qso2 qso = new Qso2("", App.AdifEnums, ref err);
			Station_Callsign call = new Station_Callsign("VA3HJ");
			qso.Fields.Add(call);
			Owner_Callsign owner = qso.GetField("Owner_Callsign") as Owner_Callsign;
			Assert.IsNull(owner);
			string mod = call.ModifyValues(qso);
			owner = qso.GetField("Owner_Callsign") as Owner_Callsign;
			Assert.IsNotNull(owner);
			Assert.AreEqual(call.Value, owner.Value);
			Assert.AreEqual("\tOwner_Callsign field generated from Station_Callsign" +
			                Environment.NewLine, mod);
		}
		
		// test ModifyValues with Owner_Callsign
		[Test]
		public void TestModifyValuesWithOwnerCallsign()
		{
			string err = string.Empty;
			Qso2 qso = new Qso2("<Station_Callsign:5>VA3HJ<Owner_Callsign:6>VA3JNO", App.AdifEnums, ref err);
			Station_Callsign call = qso.GetField("Station_Callsign") as Station_Callsign;
			Assert.IsNotNull(call);
			string mod = call.ModifyValues(qso);
			Owner_Callsign owner = qso.GetField("Owner_Callsign") as Owner_Callsign;
			Assert.IsNotNull(owner);
			Assert.AreNotEqual(call.Value, owner.Value);
			Assert.AreEqual("VA3JNO", owner.Value);
			Assert.IsNull(mod);
		}
	}
}
