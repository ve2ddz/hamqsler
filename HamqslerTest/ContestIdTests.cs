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
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Contest_Id id = new Contest_Id("RAC-CANADA-WINTER", aEnums);
			Assert.AreEqual("<Contest_Id:17>RAC-CANADA-WINTER", id.ToAdifString());
		}
		
		// test IsInEnumeration with value in enumeration
		[Test]
		public void TestIsInEnumerationTrue()
		{
			string err = string.Empty;
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Contest_Id id = new Contest_Id("RAC-CANADA-WINTER", aEnums);
			Assert.IsTrue(id.IsInEnumeration(out err));
			Assert.AreEqual(null, err);
		}
		
		// test IsInEnumeration with value in enumeration
		[Test]
		public void TestIsInEnumerationFalse()
		{
			string err = string.Empty;
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Contest_Id id = new Contest_Id("e6", aEnums);
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
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Contest_Id id = new Contest_Id("RAC-CANADA-WINTER", aEnums);
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
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Contest_Id id = new Contest_Id("e6", aEnums);
			Assert.IsTrue(id.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
	}
}
