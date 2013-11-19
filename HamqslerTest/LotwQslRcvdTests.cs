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
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using hamqsler;

namespace hamqslerTest
{
	// tests for Lotw_Qsl_Rcvd class
	[TestFixture]
	public class LotwQslRcvdTests
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
			Lotw_Qsl_Rcvd rcvd = new Lotw_Qsl_Rcvd("Y", aEnums);
			Assert.AreEqual("<Lotw_Qsl_Rcvd:1>Y", rcvd.ToAdifString());
		}
		
		// test Validate with valid value
		[Test]
		public void TestValidateValidValue()
		{
			string err = string.Empty;
			string modStr = string.Empty;
			Lotw_Qsl_Rcvd rcvd = new Lotw_Qsl_Rcvd("Y", aEnums);
			Assert.IsTrue(rcvd.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid value
		[Test]
		public void TestValidateInvalidValue()
		{
			string err = string.Empty;
			string modStr = string.Empty;
			Lotw_Qsl_Rcvd rcvd = new Lotw_Qsl_Rcvd("F", aEnums);
			Assert.IsFalse(rcvd.Validate(out err, out modStr));
			Assert.AreEqual("\tThis QSO Field is of type enumeration. The value 'F' was not found in enumeration.",
			                err);
			Assert.IsNull(modStr);
		}

		// test ModifyValues with value 'V'
		[Test]
		public void TestModifyValuesV()
		{
			string err = string.Empty;
			string modStr = string.Empty;
			Qso2 qso = new Qso2("<Lotw_Qsl_Rcvd:1>V", aEnums,ref err);
			Lotw_Qsl_Rcvd rcvd = qso.GetField("Lotw_Qsl_Rcvd") as Lotw_Qsl_Rcvd;
			Assert.IsNotNull(rcvd);
			modStr = rcvd.ModifyValues(qso);
			rcvd = qso.GetField("Lotw_Qsl_Rcvd") as Lotw_Qsl_Rcvd;
			Assert.IsNull(rcvd);
			Assert.AreEqual("\tValue 'V' is deprecated and replaced with Credit_Granted values: " +
			                "'DXCC:LOTW', 'DXCC_BAND:LOTW', and 'DXCC_MODE:LOTW'." +
			                Environment.NewLine, modStr);
			Credit_Granted granted = qso.GetField("Credit_Granted") as Credit_Granted;
			Assert.IsNotNull(granted);
			List<Credit> credits = granted.GetCredits("DXCC");
			Assert.IsTrue(credits[0].IsInMedia("LOTW"));
			credits = granted.GetCredits("DXCC_BAND");
			Assert.IsTrue(credits[0].IsInMedia("LOTW"));
			credits = granted.GetCredits("DXCC_Mode");
			Assert.IsTrue(credits[0].IsInMedia("lotw"));
		}
	}
}
