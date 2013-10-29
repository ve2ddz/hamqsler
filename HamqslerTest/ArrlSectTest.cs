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
		AdifEnumerations aEnums;
		
		// fixture setup
		[TestFixtureSetUp]
		public void Init()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
	        Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			aEnums = new AdifEnumerations(str);
		}

		// test that constructor creates valid object
		[Test]
		public void TestConstructor()
		{
			string err = string.Empty;
			string modStr = string.Empty;
			Arrl_Sect sect = new Arrl_Sect("NT", aEnums);
			Assert.IsTrue(sect.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}


		// test that constructor creates valid object
		[Test]
		public void TestConstructor1()
		{
			string err = string.Empty;
			string modStr = string.Empty;
			Arrl_Sect sect = new Arrl_Sect("ABCD", aEnums);
			Assert.IsFalse(sect.Validate(out err, out modStr));
			Assert.AreEqual("\tThis QSO Field is of type enumeration. The value 'ABCD' was not found in enumeration.",
			                err);
			Assert.IsNull(modStr);
		}
		
		// test that ToAdifString returns correct value
		[Test]
		public void TestToAdifString()
		{
			Arrl_Sect sect = new Arrl_Sect("NT", aEnums);
			Assert.AreEqual("<Arrl_Sect:2>NT", sect.ToAdifString());
		}
	}
}
