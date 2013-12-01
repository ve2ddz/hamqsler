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
	// tests for Freq_Rx class
	[TestFixture]
	public class FreqRxTests
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
			Freq_Rx freq = new Freq_Rx("14.235", App.AdifEnums);
			Assert.AreEqual("<Freq_Rx:6>14.235", freq.ToAdifString());
		}
		
		// test Validate with valid frequency
		[Test]
		public void TestValidateValidFreq()
		{
			Freq_Rx freq = new Freq_Rx("14.235", App.AdifEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(freq.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with frequency outside band limits
		[Test]
		public void TestValidateFreqOutsideBands()
		{
			Freq_Rx freq = new Freq_Rx("14.463", App.AdifEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(freq.Validate(out err, out modStr));
			Assert.AreEqual("\t'14.463' is outside enumerated band limits.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with non number
		[Test]
		public void TestValidateFreqNonNumber()
		{
			Freq_Rx freq = new Freq_Rx("Fred", App.AdifEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(freq.Validate(out err, out modStr));
			Assert.AreEqual("\tValue must be a number.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with non number
		[Test]
		public void TestValidateFreqNonNumber2()
		{
			Freq_Rx freq = new Freq_Rx("14.235F", App.AdifEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(freq.Validate(out err, out modStr));
			Assert.AreEqual("\tValue must be a number.", err);
			Assert.IsNull(modStr);
		}
		
		// test ModifyValues with band matching frequency
		[Test]
		public void TestModifyValuesWithMatchingValues()
		{
			string err = string.Empty;
			Qso2 qso = new Qso2("<Freq_Rx:6>14.263<Band_Rx:3>20m", App.AdifEnums, ref err);
			Assert.IsNull(err);
			AdifField field = qso.GetField("Freq_Rx");
			Assert.IsNotNull(field);
			Freq_Rx freq = field as Freq_Rx;
			Assert.IsNotNull(freq);
			Assert.IsNull(freq.ModifyValues(qso));
		}
		
		// test ModifyValues with band not matching frequency
		[Test]
		public void TestModifyValuesWithMisMatchedValues()
		{
			string err = string.Empty;
			Qso2 qso = new Qso2("<Freq_Rx:6>14.263<Band_Rx:3>10m", App.AdifEnums, ref err);
			Assert.IsNull(err);
			AdifField field = qso.GetField("Freq_Rx");
			Assert.IsNotNull(field);
			Freq_Rx freq = field as Freq_Rx;
			Assert.IsNotNull(freq);
			Assert.AreEqual("\tHam band in Band_Rx field does not match band for given Freq_Rx." +
						" Band_Rx field modified to match the frequency.",
						freq.ModifyValues(qso));
			Assert.AreEqual("20m", qso["band_rx"]);
		}
		
		// test ModifyValues without band
		[Test]
		public void TestModifyValuesWithoutBand()
		{
			string err = string.Empty;
			Qso2 qso = new Qso2("<Freq_Rx:5>7.263", App.AdifEnums, ref err);
			Assert.IsNull(err);
			AdifField field = qso.GetField("Freq_Rx");
			Assert.IsNotNull(field);
			Freq_Rx freq = field as Freq_Rx;
			Assert.IsNotNull(freq);
			Assert.AreEqual("\tFreq_Rx specified, but Band_Rx is not. Band_Rx field generated.",
						freq.ModifyValues(qso));
			Assert.IsNotNull(qso["band_rx"]);
			Assert.AreEqual("40m", qso["band_rx"]);
		}
	}
}
