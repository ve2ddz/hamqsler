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
	// tests for Tx_Pwr
	[TestFixture]
	public class TxPwrTests
	{
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			Tx_Pwr power = new Tx_Pwr("0.215");
			Assert.AreEqual("<Tx_Pwr:5>0.215", power.ToAdifString());
		}
		
		// test Validate with valid number
		[Test]
		public void TestValidateValidNumber()
		{
			Tx_Pwr power = new Tx_Pwr("12");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(power.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}

		// test Validate with invalid number
		[Test]
		public void TestValidateInvalidNumber()
		{
			Tx_Pwr power = new Tx_Pwr("fred");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(power.Validate(out err, out modStr));
			Assert.AreEqual("Value must be a number.", err);
			Assert.IsNull(modStr);
		}

		// test Validate with empty string
		[Test]
		public void TestValidateEmptyString()
		{
			Tx_Pwr power = new Tx_Pwr(string.Empty);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(power.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
	}
}
