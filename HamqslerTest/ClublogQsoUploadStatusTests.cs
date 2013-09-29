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
	// tests for Clublog_Qso_Upload_Status class
	[TestFixture]
	public class ClublogQsoUploadStatusTests
	{
		// test ToAdifString
		[Test]
		public void TestMethod()
		{
			string err = string.Empty;
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Clublog_Qso_Upload_Status status = new Clublog_Qso_Upload_Status("Y", aEnums);
			Assert.AreEqual("<Clublog_Qso_Upload_Status:1>Y", status.ToAdifString());
		}
		
		// test Validate with valid status
		[Test]
		public void TestValidateY()
		{
			string err = string.Empty;
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Clublog_Qso_Upload_Status status = new Clublog_Qso_Upload_Status("Y", aEnums);
			Assert.IsTrue(status.Validate(out err));
		}
		
		// test Validate with invalid status
		[Test]
		public void TestValidateB()
		{
			string err = string.Empty;
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Clublog_Qso_Upload_Status status = new Clublog_Qso_Upload_Status("B", aEnums);
			Assert.IsFalse(status.Validate(out err));
			Assert.AreEqual("This QSO Field is of type enumeration. The value 'B' was not found in enumeration.",
			                err);
		}
		
	}
}
