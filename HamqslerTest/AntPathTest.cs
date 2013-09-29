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
	// test fixture for AntPath class
	[TestFixture]
	public class AntPathTest
	{
		// test that constructor creates valid object
		[Test]
		public void TestConstructor()
		{
			string err = string.Empty;
		    // get the hamqsler assembly
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            // get a stream for the AdifEnumerations.xml file
            // TODO: This is currently an embedded resource in the assembly, but needs to be moved to AppData
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
             // load in the xml file
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Ant_Path ap = new Ant_Path("G", aEnums);
			Assert.IsTrue(ap.Validate(out err));
		}


		// test that constructor creates valid object
		[Test]
		public void TestConstructor1()
		{
			string err = string.Empty;
		    // get the hamqsler assembly
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            // get a stream for the AdifEnumerations.xml file
            // TODO: This is currently an embedded resource in the assembly, but needs to be moved to AppData
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
             // load in the xml file
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Ant_Path ap = new Ant_Path("F", aEnums);
			Assert.IsFalse(ap.Validate(out err));
			Assert.AreEqual("This QSO Field is of type enumeration. The value 'F' was not found in enumeration.",
			                err);
		}
		
		// test that ToAdifString returns correct value
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
			Ant_Path ap = new Ant_Path("G", aEnums);
			Assert.AreEqual("<Ant_Path:1>G", ap.ToAdifString());
		}
	}
}
