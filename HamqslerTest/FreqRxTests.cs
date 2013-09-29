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
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Freq_Rx freq = new Freq_Rx("14.235", aEnums);
			Assert.AreEqual("<Freq_Rx:6>14.235", freq.ToAdifString());
		}
		
		// test Validate with valid frequency
		[Test]
		public void TestValidateValidFreq()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Freq_Rx freq = new Freq_Rx("14.235", aEnums);
			string err = string.Empty;
			Assert.IsTrue(freq.Validate(out err));
			Assert.AreEqual(null, err);
		}
		
		// test Validate with frequency outside band limits
		[Test]
		public void TestValidateFreqOutsideBands()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Freq_Rx freq = new Freq_Rx("14.463", aEnums);
			string err = string.Empty;
			Assert.IsFalse(freq.Validate(out err));
			Assert.AreEqual("'14.463' is outside enumerated band limits.", err);
		}
		
		// test Validate with non number
		[Test]
		public void TestValidateFreqNonNumber()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Freq_Rx freq = new Freq_Rx("Fred", aEnums);
			string err = string.Empty;
			Assert.IsFalse(freq.Validate(out err));
			Assert.AreEqual("Value must be a number.", err);
		}
		
		// test Validate with non number
		[Test]
		public void TestValidateFreqNonNumber2()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Freq_Rx freq = new Freq_Rx("14.235F", aEnums);
			string err = string.Empty;
			Assert.IsFalse(freq.Validate(out err));
			Assert.AreEqual("Value must be a number.", err);
		}	}
}
