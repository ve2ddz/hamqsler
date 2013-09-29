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
	// tests for Mode class
	[TestFixture]
	public class ModeTests
	{
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Mode mode = new Mode("PSK", aEnums);
			Assert.AreEqual("<Mode:3>PSK", mode.ToAdifString());
		}
		
		// test Validate with valid mode
		[Test]
		public void TestValidateValidMode()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Mode mode = new Mode("PSK", aEnums);
			string err = string.Empty;
			Assert.IsTrue(mode.Validate(out err));
			Assert.AreEqual(null, err);
		}
		
		// test Validate with invalid mode
		[Test]
		public void TestValidateInvalidMode()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Mode mode = new Mode("BADMODE", aEnums);
			string err = string.Empty;
			Assert.IsFalse(mode.Validate(out err));
			Assert.AreEqual("This QSO Field is of type enumeration. The value 'BADMODE' was not found in enumeration.",
			                err);
		}

		// test Validate with null mode
		[Test]
		public void TestValidateNullMode()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Mode mode = new Mode(null, aEnums);
			string err = string.Empty;
			Assert.IsFalse(mode.Validate(out err));
			Assert.AreEqual("Value is null.", err);
		}
	}
}
