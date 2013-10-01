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
	// tests for Cont class
	[TestFixture]
	public class ContTests
	{
		// test ToAdifString()
		[Test]
		public void TestToAdifString()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
 			AdifEnumerations aEnums = new AdifEnumerations(str);
			Cont cont = new Cont("AF", aEnums);
			Assert.AreEqual("<Cont:2>AF", cont.ToAdifString());
		}
		
		// test Validate
		[Test]
		public void TestValidate()
		{
			string err = string.Empty;
			string modStr =string.Empty;
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
 			AdifEnumerations aEnums = new AdifEnumerations(str);
			Cont cont = new Cont("AF", aEnums);
			Assert.IsTrue(cont.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid value
		[Test]
		public void TestValidateInvalidValue()
		{
			string err = string.Empty;
			string modStr = string.Empty;
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
 			AdifEnumerations aEnums = new AdifEnumerations(str);
			Cont cont = new Cont("AX", aEnums);
			Assert.IsFalse(cont.Validate(out err, out modStr));
			Assert.AreEqual("This QSO Field is of type enumeration. The value 'AX' " +
			                "was not found in enumeration.", err);
			Assert.IsNull(modStr);
		}
	}
}
