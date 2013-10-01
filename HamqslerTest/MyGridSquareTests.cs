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
	// tests for My_GridSquare class
	[TestFixture]
	public class MyGridSquareTests
	{
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			My_GridSquare grid = new My_GridSquare("FN03bi");
			Assert.AreEqual("<My_GridSquare:6>FN03bi", grid.ToAdifString());
		}
		
		// test Validate with valid 2-character grid square
		[Test]
		public void TestValidateValid2Chars()
		{
			My_GridSquare grid = new My_GridSquare("FN");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(grid.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid 2-character grid square
		[Test]
		public void TestValidateInvalid2Chars()
		{
			My_GridSquare grid = new My_GridSquare("F3");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(grid.Validate(out err, out modStr));
			Assert.AreEqual("'F3' is not a valid grid square.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with 1-character square
		[Test]
		public void TestValidateInvalid1Char()
		{
			My_GridSquare grid = new My_GridSquare("F");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(grid.Validate(out err, out modStr));
			Assert.AreEqual("'F' is not a valid grid square.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with 3-character square
		[Test]
		public void TestValidateInvalid3Chars()
		{
			My_GridSquare grid = new My_GridSquare("FG2");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(grid.Validate(out err, out modStr));
			Assert.AreEqual("'FG2' is not a valid grid square.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with 5-character square
		[Test]
		public void TestValidateInvalid5Chars()
		{
			My_GridSquare grid = new My_GridSquare("FG21i");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(grid.Validate(out err, out modStr));
			Assert.AreEqual("'FG21i' is not a valid grid square.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with 7-character square
		[Test]
		public void TestValidateInvalid7Chars()
		{
			My_GridSquare grid = new My_GridSquare("FG21il5");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(grid.Validate(out err, out modStr));
			Assert.AreEqual("'FG21il5' is not a valid grid square.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with 9-character square
		[Test]
		public void TestValidateInvalid9Chars()
		{
			My_GridSquare grid = new My_GridSquare("FG21il55k");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(grid.Validate(out err, out modStr));
			Assert.AreEqual("'FG21il55k' is not a valid grid square.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with valid 4-character square
		[Test]
		public void TestValidateValid4Chars()
		{
			My_GridSquare grid = new My_GridSquare("FG21");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(grid.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid 4-character square
		[Test]
		public void TestValidateInvalid4Chars()
		{
			My_GridSquare grid = new My_GridSquare("FG2g");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(grid.Validate(out err, out modStr));
			Assert.AreEqual("'FG2g' is not a valid grid square.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with valid 6-character square
		[Test]
		public void TestValidateValid6Chars()
		{
			My_GridSquare grid = new My_GridSquare("FG21ix");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(grid.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid 6-character square
		[Test]
		public void TestValidateInvalid6Chars()
		{
			My_GridSquare grid = new My_GridSquare("FG21iy");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(grid.Validate(out err, out modStr));
			Assert.AreEqual("'FG21iy' is not a valid grid square.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with valid 8-character square
		[Test]
		public void TestValidateValid8Chars()
		{
			My_GridSquare grid = new My_GridSquare("FG21ix04");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(grid.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid 8-character square
		[Test]
		public void TestValidateInvalid8Chars()
		{
			My_GridSquare grid = new My_GridSquare("FG21iy8f");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(grid.Validate(out err, out modStr));
			Assert.AreEqual("'FG21iy8f' is not a valid grid square.", err);
			Assert.IsNull(modStr);
		}
	}
}
