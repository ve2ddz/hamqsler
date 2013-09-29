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
	// tests for Qsl_Rcvd_Via class
	[TestFixture]
	public class QslRcvdViaTests
	{
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Qsl_Rcvd_Via status = new Qsl_Rcvd_Via("B", aEnums);
			Assert.AreEqual("<Qsl_Rcvd_Via:1>B", status.ToAdifString());
		}

		// test Validate with valid value
		[Test]
		public void TestValidateValid()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Qsl_Rcvd_Via status = new Qsl_Rcvd_Via("B", aEnums);
			string err = string.Empty;
			Assert.IsTrue(status.Validate(out err));
			Assert.AreEqual(null, err);
		}

		// test Validate with invalid value
		[Test]
		public void TestValidateInvalid()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			string err = string.Empty;
			Qsl_Rcvd_Via status = new Qsl_Rcvd_Via("F", aEnums);
			Assert.IsFalse(status.Validate(out err));
			Assert.AreEqual("This QSO Field is of type enumeration. The value 'F' was not found in enumeration.", 
			                err);
		}
	}
}
