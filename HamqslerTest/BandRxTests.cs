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
	// tests for Band_Rx class
	[TestFixture]
	public class BandRxTests
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

		// test Validate with valid value
		[Test]
		public void TestValidate2190m()
		{
			Band_Rx band = new Band_Rx("2190m", aEnums);
			string error = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(band.Validate(out error, out modStr));
			Assert.IsNull(error);
			Assert.IsNull(modStr);
		}

		// test Validate with valid value
		[Test]
		public void TestValidate1mm()
		{
			Band_Rx band = new Band_Rx("1mm", aEnums);
			string error = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(band.Validate(out error, out modStr));
			Assert.IsNull(error);
			Assert.IsNull(modStr);
		}
	
			// test Validate with valid value
			[Test]
			public void TestValidate1MM()
			{
				Band_Rx band = new Band_Rx("1MM", aEnums);
				string error = string.Empty;
				string modStr = string.Empty;
				Assert.IsTrue(band.Validate(out error, out modStr));
				Assert.IsNull(error);
				Assert.IsNull(modStr);
			}

		// test Validate with invalid value
		[Test]
		public void TestValidateBadValue()
		{
			Band_Rx band = new Band_Rx("23mm", aEnums);
			string error = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(band.Validate(out error, out modStr));
			Assert.AreEqual("This QSO Field is of type enumeration. The value '23mm' was not found in enumeration.", 
			                error);
			Assert.IsNull(modStr);
		}

		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			Band_Rx band = new Band_Rx("23cm", aEnums);
			Assert.AreEqual("<Band_Rx:4>23cm", band.ToAdifString());
		}
		
		// test IsWithinBand for frequency within the band
		[Test]
		public void TestIsWithinBandValidFreq()
		{
			Band_Rx band = new Band_Rx("40m", aEnums);
			Assert.IsTrue(band.IsWithinBand("7.102"));
		}
		
		// test IsWithinBand for frequency outside of the band
		[Test]
		public void TestIsWithinBandBadFreq()
		{
			Band_Rx band = new Band_Rx("40m", aEnums);
			Assert.IsFalse(band.IsWithinBand("14.302"));
		}

		// test IsWithinBand for frequency outside of the band
		[Test]
		public void TestIsWithinBandBadFreqM()
		{
			Band_Rx band = new Band_Rx("40M", aEnums);
			Assert.IsFalse(band.IsWithinBand("14.302"));
		}
	}
}