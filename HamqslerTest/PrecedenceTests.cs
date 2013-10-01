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
	// tests for Precedence class
	[TestFixture]
	public class PrecedenceTests
	{
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			Precedence prec = new Precedence("96");
			Assert.AreEqual("<Precedence:2>96", prec.ToAdifString());
		}
		
		// test Validate - any value allowed
		[Test]
		public void TestValidateValid()
		{
			Precedence prec = new Precedence("96");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(prec.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate - any value allowed
		[Test]
		public void TestValidateValid2()
		{
			Precedence prec = new Precedence("Fred 99");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(prec.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
	}
}
