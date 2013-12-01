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
	// tests for Contest_Id class
	[TestFixture]
	public class ContestIdTests
	{
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			Contest_Id id = new Contest_Id("RAC-CANADA-WINTER", App.AdifEnums);
			Assert.AreEqual("<Contest_Id:17>RAC-CANADA-WINTER", id.ToAdifString());
		}
		
		// test IsInEnumeration with value in enumeration
		[Test]
		public void TestIsInEnumerationTrue()
		{
			string err = string.Empty;
			Contest_Id id = new Contest_Id("RAC-CANADA-WINTER", App.AdifEnums);
			Assert.IsTrue(id.IsInEnumeration(out err));
			Assert.AreEqual(null, err);
		}
		
		// test IsInEnumeration with value in enumeration
		[Test]
		public void TestIsInEnumerationFalse()
		{
			string err = string.Empty;
			Contest_Id id = new Contest_Id("e6", App.AdifEnums);
			Assert.IsFalse(id.IsInEnumeration(out err));
			Assert.AreEqual("\tThis QSO Field is of type enumeration. The value 'e6' " +
			                    "was not found in enumeration.", err);
		}
		
		// test Validate with value in enumeration
		[Test]
		public void TestValidateValueInEnumeration()
		{
			string err = string.Empty;
			string modStr = string.Empty;
			Contest_Id id = new Contest_Id("RAC-CANADA-WINTER", App.AdifEnums);
			Assert.IsTrue(id.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with value not in enumeration
		[Test]
		public void TestValidateValueNotInEnumeration()
		{
			string err = string.Empty;
			string modStr = string.Empty;
			Contest_Id id = new Contest_Id("e6", App.AdifEnums);
			Assert.IsTrue(id.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test ModifyValues for URE_DX
		[Test]
		public void TestModifyValuesUre_Dx()
		{
			string err = string.Empty;
			string modStr = string.Empty;
			Qso2 qso = new Qso2("<contest_ID:6>URE-DX", App.AdifEnums, ref err);
			Contest_Id id = qso.GetField("Contest_Id") as Contest_Id;
			Assert.IsNotNull(id);
			modStr = id.ModifyValues(qso);
			Assert.AreEqual("UKRAINIAN DX", id.Value);
			Assert.AreEqual("\tContest_Id:" + Environment.NewLine +
			                "\t\tDeprecated value 'URE-DX' changed to 'UKRAINIAN DX'." +
			                Environment.NewLine, modStr);
		}
		
		// test ModifyValues for EA-RTTY
		[Test]
		public void TestModifyValuesEa_Rtty()
		{
			string err = string.Empty;
			string modStr = string.Empty;
			Qso2 qso = new Qso2("<contest_ID:7>EA-RTTY", App.AdifEnums, ref err);
			Contest_Id id = qso.GetField("Contest_Id") as Contest_Id;
			Assert.IsNotNull(id);
			modStr = id.ModifyValues(qso);
			Assert.AreEqual("EA-WW-RTTY", id.Value);
			Assert.AreEqual("\tContest_Id:" + Environment.NewLine +
			                "\t\tDeprecated value 'EA-RTTY' changed to 'EA-WW-RTTY'." +
			                Environment.NewLine, modStr);
		}
		
		// test ModifyValues for Virginia QSO Party
		[Test]
		public void TestModifyValuesVirginia_QSO_Party()
		{
			string err = string.Empty;
			string modStr = string.Empty;
			Qso2 qso = new Qso2("<contest_ID:18>Virginia QSO Party", App.AdifEnums, ref err);
			Contest_Id id = qso.GetField("Contest_Id") as Contest_Id;
			Assert.IsNotNull(id);
			modStr = id.ModifyValues(qso);
			Assert.AreEqual("VA-QSO-PARTY", id.Value);
			Assert.AreEqual("\tContest_Id:" + Environment.NewLine +
			                "\t\tDeprecated value 'Virginia QSO Party' changed to 'VA-QSO-PARTY'." +
			                Environment.NewLine, modStr);
		}
	}
}
