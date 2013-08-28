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
	public class ArrlSectTest
	{
		// test that constructor creates valid object
		[Test]
		public void TestConstructor()
		{
			string err = string.Empty;
		    // get the hamqsler assembly
			Assembly assembly = Assembly.GetAssembly((new AdifField(string.Empty)).GetType());
            // get a stream for the AdifEnumerations.xml file
            // TODO: This is currently an embedded resource in the assembly, but needs to be moved to AppData
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
             // load in the xml file
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Arrl_Sect sect = new Arrl_Sect("NT", aEnums);
			Assert.IsTrue(sect.Validate(out err));
		}


		// test that constructor creates valid object
		[Test]
		public void TestConstructor1()
		{
			string err = string.Empty;
		    // get the hamqsler assembly
			Assembly assembly = Assembly.GetAssembly((new AdifField(string.Empty)).GetType());
            // get a stream for the AdifEnumerations.xml file
            // TODO: This is currently an embedded resource in the assembly, but needs to be moved to AppData
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
             // load in the xml file
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Arrl_Sect sect = new Arrl_Sect("ABCD", aEnums);
			Assert.IsFalse(sect.Validate(out err));
			Assert.AreEqual("This QSO Field is of type enumeration. The value was not found in enumeration",
			                err);
		}
		
		// test that ToAdifString returns correct value
		[Test]
		public void TestToAdifString()
		{
			string err = string.Empty;
		    // get the hamqsler assembly
			Assembly assembly = Assembly.GetAssembly((new AdifField(string.Empty)).GetType());
            // get a stream for the AdifEnumerations.xml file
            // TODO: This is currently an embedded resource in the assembly, but needs to be moved to AppData
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
             // load in the xml file
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Arrl_Sect sect = new Arrl_Sect("NT", aEnums);
			Assert.AreEqual("<Arrl_Sect:2>NT", sect.ToAdifString());
		}
	}
}
