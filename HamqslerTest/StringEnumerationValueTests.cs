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
	// tests for StringEnumerationValue class
	[TestFixture]
	public class StringEnumerationValueTests
	{
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			string[] enums = {"e1", "e2", "e3", "e4"};
			StringEnumerationValue se = new StringEnumerationValue("e1", enums);
			Assert.AreEqual("<StringEnumerationValue:2>e1", se.ToAdifString());
		}
		
		// test ToAdifString with ADIF enumeration
		[Test]
		public void TestToAdifString2()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			StringEnumerationValue se = new StringEnumerationValue("e6", "Contest_ID", aEnums);
			Assert.AreEqual("<StringEnumerationValue:2>e6", se.ToAdifString());
		}
		
		// test IsInEnumeration with value in enumeration
		[Test]
		public void TestIsInEnumerationTrue()
		{
			string err = string.Empty;
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			StringEnumerationValue se = new StringEnumerationValue("RAC-CANADA-DAY", "Contest_ID", aEnums);
			Assert.IsTrue(se.IsInEnumeration(out err));
			Assert.AreEqual(null, err);
		}
		
		// test IsInEnumeration with value not in enumeration
		[Test]
		public void TestIsInEnumerationFalse()
		{
			string err = string.Empty;
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			StringEnumerationValue se = new StringEnumerationValue("e4", "Contest_ID", aEnums);
			Assert.IsFalse(se.IsInEnumeration(out err));
			Assert.AreEqual("\tThis QSO Field is of type enumeration. The value 'e4' " +
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
			StringEnumerationValue se = new StringEnumerationValue("RAC-CANADA-DAY", "Contest_ID", aEnums);
			Assert.IsTrue(se.Validate(out err,out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with value notin enumeration
		[Test]
		public void TestValidateValueNotInEnumeration()
		{
			string err = string.Empty;
			string modStr = string.Empty;
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			StringEnumerationValue se = new StringEnumerationValue("e4", "Contest_ID", aEnums);
			Assert.IsTrue(se.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
	}
}
