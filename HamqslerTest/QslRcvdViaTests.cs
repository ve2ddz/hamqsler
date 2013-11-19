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
	// tests for Qsl_Rcvd_Via class
	[TestFixture]
	public class QslRcvdViaTests
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

		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			Qsl_Rcvd_Via status = new Qsl_Rcvd_Via("B", aEnums);
			Assert.AreEqual("<Qsl_Rcvd_Via:1>B", status.ToAdifString());
		}

		// test Validate with valid value
		[Test]
		public void TestValidateValid()
		{
			Qsl_Rcvd_Via status = new Qsl_Rcvd_Via("B", aEnums);
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
			Qsl_Rcvd_Via status = new Qsl_Rcvd_Via("F", aEnums);
			Assert.IsFalse(status.Validate(out err, out modStr));
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
			Qso2 qso = new Qso2("<Qsl_Rcvd_Via:1>E", aEnums, ref err);
			Qsl_Rcvd_Via status = qso.GetField("Qsl_Rcvd_Via") as Qsl_Rcvd_Via;
			Assert.IsNotNull(status);
			string mod = status.ModifyValues(qso);
			Assert.IsNull(mod);
			status = qso.GetField("Qsl_Rcvd_Via") as Qsl_Rcvd_Via;
			Assert.IsNotNull(status);
			Assert.AreEqual("E", status.Value);
		}

		
		// test ModifyValues with M status
		[Test]
		public void TestModifyValuesMStatus()
		{
			string err = string.Empty;
			string modStr = string.Empty;
			Qso2 qso = new Qso2("<Qsl_Rcvd_Via:1>M", aEnums, ref err);
			Qsl_Rcvd_Via status = qso.GetField("Qsl_Rcvd_Via") as Qsl_Rcvd_Via;
			Assert.IsNotNull(status);
			string mod = status.ModifyValues(qso);
			Assert.AreEqual("\tQsl_Rcvd_Via value 'M' deprecated with no replacement value. Field deleted."
			                + Environment.NewLine, mod);
			status = qso.GetField("Qsl_Rcvd_Via") as Qsl_Rcvd_Via;
			Assert.IsNull(status);
		}
	}
}
