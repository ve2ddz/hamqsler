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
	// tests for Qso_Complete class
	[TestFixture]
	public class QsoCompleteTests
	{
		// TestFixtureSetup
		[TestFixtureSetUp]
		public void TestSepup()
		{
			App.AdifEnums.LoadDocument();
		}
		
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			Qso_Complete qc = new Qso_Complete("NIL", App.AdifEnums);
			Assert.AreEqual("<Qso_Complete:3>NIL", qc.ToAdifString());
		}

		// test Validate with valid value
		[Test]
		public void TestValidateValidValue()
		{
			Qso_Complete qc = new Qso_Complete("?", App.AdifEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(qc.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}

		// test Validate with invalid value
		[Test]
		public void TestValidateInvalidValue()
		{
			Qso_Complete qc = new Qso_Complete("F", App.AdifEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(qc.Validate(out err, out modStr));
			Assert.AreEqual("\tThis QSO Field is of type enumeration. The value 'F' was not found in enumeration.",
			                err);
			Assert.IsNull(modStr);
		}
	}
}
