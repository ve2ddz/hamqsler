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
	// tests for Country class
	[TestFixture]
	public class CountryTests
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
			Country country = new Country("United States", App.AdifEnums);
			Assert.AreEqual("<Country:13>UNITED STATES", country.ToAdifString());
		}
		
		// test Validate with valid country
		[Test]
		public void ValidateWithValidCountry()
		{
			string err = string.Empty;
			string modStr = string.Empty;
			Country country = new Country("UNITED STATES", App.AdifEnums);
			Assert.IsTrue(country.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid country
		[Test]
		public void ValidateWithInvalidCountry()
		{
			string err = string.Empty;
			string modStr = string.Empty;
			Country country = new Country("COCHISE", App.AdifEnums);
			Assert.IsFalse(country.Validate(out err, out modStr));
			Assert.AreEqual("\t'COCHISE' is not a valid country", err);
			Assert.IsNull(modStr);
		}
		
	}
}
