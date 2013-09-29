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
	// tests for Rx_Pwr class
	[TestFixture]
	public class RxPwrTests
	{
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			Rx_Pwr power = new Rx_Pwr("0.215");
			Assert.AreEqual("<Rx_Pwr:5>0.215", power.ToAdifString());
		}
		
		// test Validate with valid number
		[Test]
		public void TestValidateValidNumber()
		{
			Rx_Pwr power = new Rx_Pwr("12");
			string err = string.Empty;
			Assert.IsTrue(power.Validate(out err));
			Assert.AreEqual(null, err);
		}

		// test Validate with invalid number
		[Test]
		public void TestValidateInvalidNumber()
		{
			Rx_Pwr power = new Rx_Pwr("fred");
			string err = string.Empty;
			Assert.IsFalse(power.Validate(out err));
			Assert.AreEqual("Value must be a number.", err);
		}

		// test Validate with empty string
		[Test]
		public void TestValidateEmptyString()
		{
			Rx_Pwr power = new Rx_Pwr(string.Empty);
			string err = string.Empty;
			Assert.IsTrue(power.Validate(out err));
			Assert.AreEqual(null, err);
		}
	}
}
