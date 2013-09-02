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
	/// Tests for StringField class
	/// </summary>
	[TestFixture]
	public class StringFieldTests
	{
		// test Name accessor
		[Test]
		public void TestGetName()
		{
			StringField field = new StringField("VA3HJ");
			Assert.AreEqual("StringField", field.Name);
		}
		
		// test Value accessor
		[Test]
		public void TestGetValue()
		{
			StringField field = new StringField("VA3HJ");
			Assert.AreEqual("VA3HJ", field.Value);
		}
		
		// test ToAdifField
		[Test]
		public void TestToAdifString()
		{
			StringField field = new StringField("VA3HJ");
			Assert.AreEqual("<StringField:5>VA3HJ", field.ToAdifString());
		}
		
		// test IsValid
		[Test]
		public void TestIsValid()
		{
			string err = string.Empty;
			StringField field = new StringField("VA3HJ");
			Assert.IsTrue(field.Validate(out err));
			Assert.IsNull(err);
		}
		
		// test IsValid returns false
		[Test]
		public void TestIsValidFalse()
		{
			string err = string.Empty;
			StringField field = new StringField(null);
			Assert.IsFalse(field.Validate(out err));
			Assert.AreEqual("Value is null", err);
		}
		
		// test IsValid with NewLine = should return false
		[Test]
		public void TestIsValidNewLine()
		{
			string err = string.Empty;
			StringField field = new StringField("VA3HJ" + Environment.NewLine + "VA3JNO");
			Assert.IsFalse(field.Validate(out err));
			Assert.AreEqual("String value contains a new line character. This is not allowed in StringField types",
			                err);
		}
	}
}
