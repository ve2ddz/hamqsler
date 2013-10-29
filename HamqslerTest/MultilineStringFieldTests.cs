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
	[TestFixture]
	public class MultilineStringFieldTests
	{
		// test Value accessor
		[Test]
		public void TestGetValue()
		{
			MultilineStringField field = new MultilineStringField("VA3HJ" + Environment.NewLine +
			                                                      "VA3JNO");
			Assert.AreEqual("VA3HJ" + Environment.NewLine + "VA3JNO", field.Value);
		}
		
		// test ToAdifField
		[Test]
		public void TestToAdifString()
		{
			MultilineStringField field = new MultilineStringField("VA3HJ" + Environment.NewLine +
			                                                      "VA3JNO");
			Assert.AreEqual("<MultilineStringField:13>VA3HJ" + Environment.NewLine + "VA3JNO", 
			                field.ToAdifString());
		}
		
		// test IsValid
		[Test]
		public void TestIsValid()
		{
			string err = string.Empty;
			string modStr = string.Empty;
			MultilineStringField field = new MultilineStringField("VA3HJ" + Environment.NewLine +
			                                                      "VA3JNO");
			Assert.IsTrue(field.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test IsValid returns false
		[Test]
		public void TestIsValidFalse()
		{
			string err = string.Empty;
			string modStr = string.Empty;
			MultilineStringField field = new MultilineStringField(null);
			Assert.IsFalse(field.Validate(out err, out modStr));
			Assert.AreEqual("\tValue is null", err);
			Assert.IsNull(modStr);
		}
	}
}
