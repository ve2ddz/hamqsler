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
	/// <summary>
	/// Description of AdifFieldTest.
	/// </summary>
	[TestFixture]
	public class AdifFieldTest
	{
		// test Name accessor
		[Test]
		public void TestGetName()
		{
			AdifField field = new AdifField("VA3HJ");
			Assert.AreEqual("AdifField", field.Name);
		}
		
		// test Value accessor
		[Test]
		public void TestGetValue()
		{
			AdifField field = new AdifField("VA3HJ");
			Assert.AreEqual("VA3HJ", field.Value);
		}
		
		// test ToAdifField
		[Test]
		public void TestToAdifString()
		{
			AdifField field = new AdifField("VA3HJ");
			Assert.AreEqual("<AdifField:5>VA3HJ", field.ToAdifString());
		}
		
		// test IsValid
		[Test]
		public void TestIsValid()
		{
			string err = string.Empty;
			AdifField field = new AdifField("VA3HJ");
			Assert.IsTrue(field.Validate(out err));
			Assert.IsNull(err);
		}
		
		// test IsValid returns false
		[Test]
		public void TestIsValidFalse()
		{
			string err = string.Empty;
			AdifField field = new AdifField(null);
			Assert.IsFalse(field.Validate(out err));
			Assert.AreEqual("Value is null", err);
		}
	}
}
