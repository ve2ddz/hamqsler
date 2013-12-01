﻿/*
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
	// tests for Qsl_Sent_Via class
	[TestFixture]
	public class QslSentViaTests
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
			Qsl_Sent_Via status = new Qsl_Sent_Via("B", App.AdifEnums);
			Assert.AreEqual("<Qsl_Sent_Via:1>B", status.ToAdifString());
		}

		// test Validate with valid value
		[Test]
		public void TestValidateValid()
		{
			Qsl_Sent_Via status = new Qsl_Sent_Via("B", App.AdifEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(status.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}

		// test Validate with invalid value
		[Test]
		public void TestValidateInvalid()
		{
			string err = string.Empty;
			string modStr = string.Empty;
			Qsl_Sent_Via status = new Qsl_Sent_Via("F", App.AdifEnums);
			Assert.IsFalse(status.Validate(out err,out modStr));
			Assert.AreEqual("\tThis QSO Field is of type enumeration. The value 'F' was not found in enumeration.", 
			                err);
			Assert.IsNull(modStr);
		}
		
		// test ModifyValues with non-M status
		[Test]
		public void TestModifyValuesEStatus()
		{
			string err = string.Empty;
			string modStr = string.Empty;
			Qso2 qso = new Qso2("<Qsl_Sent_Via:1>E", App.AdifEnums, ref err);
			Qsl_Sent_Via status = qso.GetField("Qsl_Sent_Via") as Qsl_Sent_Via;
			Assert.IsNotNull(status);
			string mod = status.ModifyValues(qso);
			Assert.IsNull(mod);
			status = qso.GetField("Qsl_Sent_Via") as Qsl_Sent_Via;
			Assert.IsNotNull(status);
			Assert.AreEqual("E", status.Value);
		}

		
		// test ModifyValues with M status
		[Test]
		public void TestModifyValuesMStatus()
		{
			string err = string.Empty;
			string modStr = string.Empty;
			Qso2 qso = new Qso2("<Qsl_Sent_Via:1>M", App.AdifEnums, ref err);
			Qsl_Sent_Via status = qso.GetField("Qsl_Sent_Via") as Qsl_Sent_Via;
			Assert.IsNotNull(status);
			string mod = status.ModifyValues(qso);
			Assert.AreEqual("\tQsl_Sent_Via value 'M' deprecated with no replacement value. Field deleted."
			                + Environment.NewLine, mod);
			status = qso.GetField("Qsl_Sent_Via") as Qsl_Sent_Via;
			Assert.IsNull(status);
		}
	}
}
