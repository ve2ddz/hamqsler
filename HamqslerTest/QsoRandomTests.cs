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
using NUnit.Framework;
using hamqsler;

namespace hamqslerTest
{
	// tests for Qso_Random class
	[TestFixture]
	public class QsoRandomTests
	{
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			Qso_Random fi = new Qso_Random("Y");
			Assert.AreEqual("<Qso_Random:1>Y", fi.ToAdifString());
		}

		// test Validate with valid value
		[Test]
		public void TestValidateY()
		{
			Qso_Random bf = new Qso_Random("Y");
			string err = string.Empty;
			Assert.IsTrue(bf.Validate(out err));
			Assert.AreEqual(null, err);
		}
		
		// test Validate with valid value
		[Test]
		public void TestValidateN()
		{
			Qso_Random bf = new Qso_Random("N");
			string err = string.Empty;
			Assert.IsTrue(bf.Validate(out err));
			Assert.AreEqual(null, err);
		}
		
		// test Validate with invalid value
		[Test]
		public void TestValidateInvalidValue()
		{
			Qso_Random bf = new Qso_Random("F");
			string err = string.Empty;
			Assert.IsFalse(bf.Validate(out err));
			Assert.AreEqual("Boolean field must have value 'Y' or 'N'.", err);
		}
	}
}
