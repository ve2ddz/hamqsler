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
	// tests for Public_Key class
	[TestFixture]
	public class PublicKeyTests
	{
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			Public_Key key = new Public_Key("A65412B5");
			Assert.AreEqual("<Public_Key:8>A65412B5", key.ToAdifString());
		}

		// test Validate - no validation is actually performed
		[Test]
		public void TestValidate()
		{
			Public_Key key = new Public_Key("A65412B5");
			string err = string.Empty;
			Assert.IsTrue(key.Validate(out err));
			Assert.AreEqual(null, err);
		}

		// test Validate - null key
		[Test]
		public void TestValidateNullKey()
		{
			Public_Key key = new Public_Key(null);
			string err = string.Empty;
			Assert.IsFalse(key.Validate(out err));
			Assert.AreEqual("Value is null", err);
		}
	}
}
