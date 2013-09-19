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
	// tests for Notes class
	[TestFixture]
	public class NotesTests
	{
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			string n = "line1" + Environment.NewLine + "line2";
			Notes notes = new Notes(n);
			Assert.AreEqual("<Notes:12>line1\r\nline2", notes.ToAdifString());
		}
		
		// test Validate single line
		[Test]
		public void TestValidate1Line()
		{
			Notes notes = new Notes("A single line note");
			string err = null;
			Assert.IsTrue(notes.Validate(out err));
			Assert.AreEqual(null, err);
		}

		// test Validate mulitple lines
		[Test]
		public void TestValidateMultipleLines()
		{
			Notes notes = new Notes("line1\r\nline2\r\nline3");
			string err = null;
			Assert.IsTrue(notes.Validate(out err));
			Assert.AreEqual(null, err);
		}
	}
}
