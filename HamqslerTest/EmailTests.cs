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
	// tests for Email class
	[TestFixture]
	public class EmailTests
	{
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			Email email = new Email("hamqsler@va3hj.ca");
			Assert.AreEqual("<Email:17>hamqsler@va3hj.ca", email.ToAdifString());
		}
		
		// test Validate with valid email address
		[Test]
		public void TestValidateValidAddress()
		{
			Email email = new Email("hamqsler@va3hj.ca");
			string err = string.Empty;
			Assert.IsTrue(email.Validate(out err));
			Assert.AreEqual(null, err);
		}
		
		// test Validate with invalid email address
		[Test]
		public void TestValidateInvalidAddress()
		{
			Email email = new Email("hamqsler.va3hj:ca");
			string err = string.Empty;
			Assert.IsFalse(email.Validate(out err));
			Assert.AreEqual("'hamqsler.va3hj:ca' does not appear to be a valid email address.", err);
		}
		
	}
}
