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
	// tests for Freq class
	[TestFixture]
	public class FreqTests
	{
		AdifEnumerations aEnums = null;
		
		// test fixture setup
		[TestFixtureSetUp]
		public void Setup()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			aEnums = new AdifEnumerations(str);			
		}
		
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			Freq freq = new Freq("14.235", aEnums);
			Assert.AreEqual("<Freq:6>14.235", freq.ToAdifString());
		}
		
		// test Validate with valid frequency
		[Test]
		public void TestValidateValidFreq()
		{
			Freq freq = new Freq("14.235", aEnums);
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
			Freq freq = new Freq("14.463", aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(freq.Validate(out err, out modStr));
			Assert.AreEqual("'14.463' is outside enumerated band limits.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with non number
		[Test]
		public void TestValidateFreqNonNumber()
		{
			Freq freq = new Freq("Fred", aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(freq.Validate(out err, out modStr));
			Assert.AreEqual("Value must be a number.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with non number
		[Test]
		public void TestValidateFreqNonNumber2()
		{
			Freq freq = new Freq("14.235F", aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(freq.Validate(out err, out modStr));
			Assert.AreEqual("Value must be a number.", err);
			Assert.IsNull(modStr);
		}
		
		// test ModifyValues with band matching frequency
		[Test]
		public void TestModifyValuesWithMatchingValues()
		{
			string err = string.Empty;
			Qso2 qso = new Qso2("<Freq:6>14.263<Band:3>20m", aEnums, ref err);
			Assert.IsNull(err);
			AdifField field = qso.GetField("Freq");
			Assert.IsNotNull(field);
			Freq freq = field as Freq;
			Assert.IsNotNull(freq);
			Assert.IsNull(freq.ModifyValues(qso));
		}
		
		// test ModifyValues with band not matching frequency
		[Test]
		public void TestModifyValuesWithMisMatchedValues()
		{
			string err = string.Empty;
			Qso2 qso = new Qso2("<Freq:6>14.263<Band:3>10m", aEnums, ref err);
			Assert.IsNull(err);
			AdifField field = qso.GetField("Freq");
			Assert.IsNotNull(field);
			Freq freq = field as Freq;
			Assert.IsNotNull(freq);
			Assert.AreEqual("Ham band in Band field does not match band for given frequency." +
						" Band field modified to match the frequency.",
						freq.ModifyValues(qso));
		}
		
		// test ModifyValues without band
		[Test]
		public void TestModifyValuesWithoutBand()
		{
			string err = string.Empty;
			Qso2 qso = new Qso2("<Freq:6>14.263", aEnums, ref err);
			Assert.IsNull(err);
			AdifField field = qso.GetField("Freq");
			Assert.IsNotNull(field);
			Freq freq = field as Freq;
			Assert.IsNotNull(freq);
			Assert.AreEqual("Frequency specified, but band is not. Band field generated.",
						freq.ModifyValues(qso));
		}
	}
}
