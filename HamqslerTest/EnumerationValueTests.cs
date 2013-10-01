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
	[TestFixture]
	public class EnumerationValueTests
	{
		// test Validate returns true for value in enumeration
		[Test]
		public void TestValidateTrue()
		{
			string[] enums = {"e1", "e2", "e3", "e4"};
			EnumerationValue eVal = new EnumerationValue("e1", enums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(eVal.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate returns false for value not in enumeration
		[Test]
		public void TestValidateFalse()
		{
			string[] enums = {"e1", "e2", "e3", "e4"};
			EnumerationValue eVal = new EnumerationValue("e5", enums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(eVal.Validate(out err, out modStr));
			Assert.AreEqual("This QSO Field is of type enumeration. The value 'e5' was not found in enumeration.",
			                err);
			Assert.IsNull(modStr);
		}

	// test Validate returns true for value in enumeration
		[Test]
		public void TestValidateTrue1()
		{
			string err = string.Empty;
			string modStr = string.Empty;
		    // get the hamqsler assembly
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            // get a stream for the AdifEnumerations.xml file
            // TODO: This is currently an embedded resource in the assembly, but needs to be moved to AppData
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
             // load in the xml file
			AdifEnumerations aEnums = new AdifEnumerations(str);
			EnumerationValue ef = new EnumerationValue("NT", "Arrl_Section", aEnums);
			Assert.IsTrue(ef.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate returns false for value not in enumeration
		[Test]
		public void TestValidateFalse1()
		{
			string err = string.Empty;
			string modStr = string.Empty;
		    // get the hamqsler assembly
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            // get a stream for the AdifEnumerations.xml file
            // TODO: This is currently an embedded resource in the assembly, but needs to be moved to AppData
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
             // load in the xml file
			AdifEnumerations aEnums = new AdifEnumerations(str);
			EnumerationValue ef = new EnumerationValue("ABCD", "Arrl_Section", aEnums);
			Assert.IsFalse(ef.Validate(out err, out modStr));
			Assert.AreEqual("This QSO Field is of type enumeration. The value 'ABCD' was not found in enumeration.",
			                err);
			Assert.IsNull(modStr);
		}
		
		// test Validate returns false for null value
		[Test]
		public void TestValidateNullValue()
		{
			string err = string.Empty;
			string modStr = string.Empty;
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			EnumerationValue ef = new EnumerationValue(null, "Arrl_Section", aEnums);
			Assert.IsFalse(ef.Validate(out err, out modStr));
			Assert.AreEqual("Value is null.", err);
			Assert.IsNull(modStr);
		}
		
		// test ToAdifString returns correct value
		[Test]
		public void TestToAdifString()
		{
			string err = string.Empty;
		    // get the hamqsler assembly
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            // get a stream for the AdifEnumerations.xml file
            // TODO: This is currently an embedded resource in the assembly, but needs to be moved to AppData
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
             // load in the xml file
			AdifEnumerations aEnums = new AdifEnumerations(str);
			EnumerationValue ef = new EnumerationValue("NT", "Arrl_Section", aEnums);
			Assert.AreEqual("<EnumerationValue:2>NT", ef.ToAdifString());
		}
	}
}
