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
	// tests for Mode class
	[TestFixture]
	public class ModeTests
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
			Mode mode = new Mode("PSK", App.AdifEnums);
			Assert.AreEqual("<Mode:3>PSK", mode.ToAdifString());
		}
		
		// test Validate with valid mode
		[Test]
		public void TestValidateValidMode()
		{
			Mode mode = new Mode("PSK", App.AdifEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(mode.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid mode
		// Should validate as true. Further validation and handling is performed in
		// ModifyValues
		[Test]
		public void TestValidateInvalidMode(
			[Values("BADMODE", "")] string modeName)
		{
			Mode mode = new Mode(modeName, App.AdifEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(mode.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}

		// test Validate with null mode
		[Test]
		public void TestValidateNullMode()
		{
			Mode mode = new Mode(null, App.AdifEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(mode.Validate(out err, out modStr));
			Assert.AreEqual("Value is null.", err);
			Assert.IsNull(modStr);
		}
		
		// test ModifyValues with valid mode and no submode
		[Test]
		public void TestModifyValuesWithValidModeNoSubmode()
		{
			string err = string.Empty;
			Qso2 qso = new Qso2("<Mode:2>AM", App.AdifEnums, ref err);
			AdifField field = qso.GetField("Mode");
			Assert.IsNotNull(field);
			Mode mode = field as Mode;
			Assert.IsNotNull(mode);
			Assert.AreEqual(null, mode.ModifyValues(qso));
		}
		
		
		// test ModifyValues with valid mode and valid submode
		[Test]
		public void TestModifyValuesWithValidModeValidSubmode()
		{
			string err = string.Empty;
			Qso2 qso = new Qso2("<Mode:3>PSK<Submode:5>PSK31", App.AdifEnums, ref err);
			AdifField field = qso.GetField("Mode");
			Assert.IsNotNull(field);
			Mode mode = field as Mode;
			Assert.IsNotNull(mode);
			Assert.AreEqual(null, mode.ModifyValues(qso));
			Submode sub = qso.GetField("Submode") as Submode;
			Assert.IsNotNull(sub);
			Assert.AreEqual("PSK31", sub.Value);
		}
		// test ModifyValues with invalid mode and no submode
		[Test]
		public void TestModifyValuesWithInvalidModeNoSubmode()
		{
			string err = string.Empty;
			Qso2 qso = new Qso2("<Mode:6>SQUIBB", App.AdifEnums, ref err);
			AdifField field = qso.GetField("Mode");
			Assert.IsNotNull(field);
			Mode mode = field as Mode;
			Assert.IsNotNull(mode);
			Assert.AreEqual("\tMode not found in Mode enumeration. Submode set to mode value and mode cleared.",
			                mode.ModifyValues(qso));
			Assert.AreEqual(null, qso["Mode"]);
			Submode submode = qso.GetField("Submode") as Submode;
			Assert.IsNotNull(submode);
			Assert.AreEqual("SQUIBB", submode.Value);
		}
		
		// test ModifyValues with invalid mode and submode set
		[Test]
		public void TestModifyValuesWithInalidModeWithValidSubmode()
		{
			string err = string.Empty;
			Qso2 qso = new Qso2("<Mode:6>SQUIBB<Submode:3>LSB", App.AdifEnums, ref err);
			AdifField field = qso.GetField("Mode");
			Assert.IsNotNull(field);
			Mode mode = field as Mode;
			Assert.IsNotNull(mode);
			Assert.AreEqual("\tMode not found in Mode enumeration. Mode set to mode for submode.",
			                mode.ModifyValues(qso));
			Assert.AreEqual("SSB", mode.Value);
			Submode submode = qso.GetField("Submode") as Submode;
			Assert.IsNotNull(submode);
			Assert.AreEqual("LSB", submode.Value);
		}
		
		// test ModifyValues with valid mode and mismatched submode
		[Test]
		public void TestModifyValuesWithValidModeWithMismatchedSubmode()
		{
			string err = string.Empty;
			Qso2 qso = new Qso2("<Mode:3>PSK<Submode:3>LSB", App.AdifEnums, ref err);
			AdifField field = qso.GetField("Mode");
			Assert.IsNotNull(field);
			Mode mode = field as Mode;
			Assert.IsNotNull(mode);
			Assert.AreEqual("\tMode - submode mismatch. Mode set to proper mode for submode.",
			                mode.ModifyValues(qso));
			Assert.AreEqual("SSB", mode.Value);
			Submode submode = qso.GetField("Submode") as Submode;
			Assert.IsNotNull(submode);
			Assert.AreEqual("LSB", submode.Value);
		}
	}
}
