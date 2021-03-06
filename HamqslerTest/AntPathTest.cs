﻿/*
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
			Ant_Path ap = new Ant_Path("G", aEnums);
			Assert.IsTrue(ap.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}


		// test that constructor creates valid object
		[Test]
		public void TestConstructor1()
		{
			string err = string.Empty;
			string modStr = string.Empty;
			Ant_Path ap = new Ant_Path("F", aEnums);
			Assert.IsFalse(ap.Validate(out err, out modStr));
			Assert.AreEqual("\tThis QSO Field is of type enumeration. The value 'F' was not found in enumeration.",
			                err);
			Assert.IsNull(modStr);
		}
		
		// test that ToAdifString returns correct value
		[Test]
		public void TestToAdifString()
		{
			string err = string.Empty;
		    // get the hamqsler assembly
			Ant_Path ap = new Ant_Path("G", aEnums);
			Assert.AreEqual("<Ant_Path:1>G", ap.ToAdifString());
		}
	}
}
