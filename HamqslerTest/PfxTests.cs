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
	// tests for Pfx class
	[TestFixture]
	public class PfxTests
	{
		// test ToAdifString()
		[Test]
		public void TestToAdifString()
		{
			Pfx pfx = new Pfx("VA3");
			Assert.AreEqual("<Pfx:3>VA3", pfx.ToAdifString());
		}
		
		// test Validate with valid prefix
		[Test]
		public void TestValidateValidPrefixes(
			[Values("VA3", "W4", "9A", "5C5", "S58", "J79")] string prefix)
		{
			Pfx pfx = new Pfx(prefix);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(pfx.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}

		// test Validate with invalid prefixes
		[Test]
		public void TestValidateInvalidPrefixes(
			[Values("99", "WW", "WWE4", "9A/W4", "TG9F")] string prefix)
		{
			Pfx pfx = new Pfx(prefix);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(pfx.Validate(out err, out modStr));
			Assert.AreEqual(string.Format("\t'{0}' is not a valid prefix.", prefix), err);
			Assert.IsNull(modStr);
		}
	}
}
