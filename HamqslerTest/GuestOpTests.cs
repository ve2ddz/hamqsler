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
	// tests for Guest_Op class
	[TestFixture]
	public class GuestOpTests
	{
		AdifEnumerations aEnums;
		
		// fixture setup
		[TestFixtureSetUp]
		public void Init()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
	        Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			aEnums = new AdifEnumerations(str);
		}

		// test Validate with valid callsign
		[Test]
		public void TestValidateValidCall()
		{
			Guest_Op op = new Guest_Op("VA3HJ");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(op.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}

		// test Validate with invalid callsign
		[Test]
		public void TestValidateInvalidCall()
		{
			Guest_Op op = new Guest_Op("VAHJ");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(op.Validate(out err, out modStr));
			Assert.AreEqual("\tNot a valid callsign.", err);
			Assert.IsNull(modStr);
		}

		// test Validate with invalid callsign
		[Test]
		public void TestValidateInvalidCall2()
		{
			Guest_Op op = new Guest_Op("PJ2/VA3HJ");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(op.Validate(out err, out modStr));
			Assert.AreEqual("\tNot a valid callsign.", err);
			Assert.IsNull(modStr);
		}

		// test ModifyValues with valid callsign, no Operator
		[Test]
		public void TestModifyValuesValidCall()
		{
			string err = string.Empty;
			Qso2 qso = new Qso2("<Guest_Op:5>VA3HJ", aEnums, ref err);
			Guest_Op guest = qso.GetField("Guest_Op") as Guest_Op;
			string mod = guest.ModifyValues(qso);
			guest = qso.GetField("Guest_Op") as Guest_Op;
			Assert.IsNull(guest);
			Operator op = qso.GetField("Operator") as Operator;
			Assert.IsNotNull(op);
			Station_Callsign call = qso.GetField("Station_Callsign") as Station_Callsign;
			Assert.IsNotNull(call);
			Assert.AreEqual(op.Value, call.Value);
			Assert.AreEqual("\tGuest_Op field changed to Operator field." +
			                Environment.NewLine +
			                "\tStation_Callsign field generated from Operator field." +
			                Environment.NewLine, mod);
		}

		// test ModifyValues with valid callsign and existing Operator
		[Test]
		public void TestModifyValuesWithOperatorField()
		{
			string err = string.Empty;
			Qso2 qso = new Qso2("<Guest_Op:5>VA3HJ<Operator:6>VA3JNO", aEnums, ref err);
			Guest_Op guest = qso.GetField("Guest_Op") as Guest_Op;
			string mod = guest.ModifyValues(qso);
			guest = qso.GetField("Guest_Op") as Guest_Op;
			Assert.IsNull(guest);
			Assert.AreEqual("\tGuest_Op field cannot be changed to Operator field because Operator field already exists. Guest_Op field deleted." +
			                Environment.NewLine, mod);
		}
	}
}
