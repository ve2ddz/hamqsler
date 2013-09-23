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
	// tests for Submode class
	[TestFixture]
	public class SubmodeTests
	{
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Submode sm = new Submode("ROS-EME",  aEnums);
			Assert.AreEqual("<Submode:7>ROS-EME", sm.ToAdifString());
		}
		
		// test IsInEnumeration with value in enumeration
		[Test]
		public void TestIsInEnumerationTrue()
		{
			string err = string.Empty;
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Submode sm = new Submode("ROS-EME", aEnums);
			Assert.IsTrue(sm.IsInEnumeration(out err));
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
			Submode sm = new Submode("e6", aEnums);
			Assert.IsFalse(sm.IsInEnumeration(out err));
			Assert.AreEqual("This QSO Field is of type enumeration. The value 'e6' " +
			                    "was not found in enumeration", err);
		}
		
		// test Validate with value in enumeration
		[Test]
		public void TestValidateValueInEnumeration()
		{
			string err = string.Empty;
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Submode sm = new Submode("ROS_MF", aEnums);
			Assert.IsTrue(sm.Validate(out err));
			Assert.AreEqual(null, err);
		}
		
		// test Validate with value not in enumeration
		[Test]
		public void TestValidateValueNotInEnumeration()
		{
			string err = string.Empty;
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Submode id = new Submode("e6", aEnums);
			Assert.IsTrue(id.Validate(out err));
			Assert.AreEqual(null, err);
		}
	}
}
