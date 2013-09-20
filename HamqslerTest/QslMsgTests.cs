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
	// tests for QslMsg class
	[TestFixture]
	public class QslMsgTests
	{
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			QslMsg msg = new QslMsg("This is a message\r\nline 2 of message");
			Assert.AreEqual("<QslMsg:36>This is a message\r\nline 2 of message", msg.ToAdifString());
		}

		// test Validate single line
		[Test]
		public void TestValidate1Line()
		{
			QslMsg msg = new QslMsg("A single line note");
			string err = null;
			Assert.IsTrue(msg.Validate(out err));
			Assert.AreEqual(null, err);
		}

		// test Validate mulitple lines
		[Test]
		public void TestValidateMultipleLines()
		{
			QslMsg msg = new QslMsg("line1\r\nline2\r\nline3");
			string err = null;
			Assert.IsTrue(msg.Validate(out err));
			Assert.AreEqual(null, err);
		}
	}
}
