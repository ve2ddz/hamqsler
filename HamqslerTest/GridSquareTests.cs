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
	// tests for GridSquare class
	[TestFixture]
	public class GridSquareTests
	{
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			GridSquare grid = new GridSquare("FN03bi");
			Assert.AreEqual("<GridSquare:6>FN03bi", grid.ToAdifString());
		}
		
		// test Validate with 0-character grid square
		[Test]
		public void TestValidate0Chars()
		{
			GridSquare grid = new GridSquare("");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(grid.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with valid 2-character grid square
		[Test]
		public void TestValidateValid2Chars()
		{
			GridSquare grid = new GridSquare("FN");
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
			GridSquare grid = new GridSquare("F3");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(grid.Validate(out err, out modStr));
			Assert.AreEqual("\t'F3' is not a valid grid square.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with 1-character square
		[Test]
		public void TestValidateInvalid1Char()
		{
			GridSquare grid = new GridSquare("F");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(grid.Validate(out err, out modStr));
			Assert.AreEqual("\t'F' is not a valid grid square.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with 3-character square
		[Test]
		public void TestValidateInvalid3Chars()
		{
			GridSquare grid = new GridSquare("FG2");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(grid.Validate(out err, out modStr));
			Assert.AreEqual("\t'FG2' is not a valid grid square.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with 5-character square
		[Test]
		public void TestValidateInvalid5Chars()
		{
			GridSquare grid = new GridSquare("FG21i");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(grid.Validate(out err, out modStr));
			Assert.AreEqual("\t'FG21i' is not a valid grid square.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with 7-character square
		[Test]
		public void TestValidateInvalid7Chars()
		{
			GridSquare grid = new GridSquare("FG21il5");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(grid.Validate(out err, out modStr));
			Assert.AreEqual("\t'FG21il5' is not a valid grid square.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with 9-character square
		[Test]
		public void TestValidateInvalid9Chars()
		{
			GridSquare grid = new GridSquare("FG21il55k");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(grid.Validate(out err, out modStr));
			Assert.AreEqual("\t'FG21il55k' is not a valid grid square.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with valid 4-character square
		[Test]
		public void TestValidateValid4Chars()
		{
			GridSquare grid = new GridSquare("FG21");
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
			GridSquare grid = new GridSquare("FG2g");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(grid.Validate(out err, out modStr));
			Assert.AreEqual("\t'FG2g' is not a valid grid square.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with valid 6-character square
		[Test]
		public void TestValidateValid6Chars()
		{
			GridSquare grid = new GridSquare("FG21ix");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(grid.Validate(out err,out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid 6-character square
		[Test]
		public void TestValidateInvalid6Chars()
		{
			GridSquare grid = new GridSquare("FG21iy");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(grid.Validate(out err, out modStr));
			Assert.AreEqual("\t'FG21iy' is not a valid grid square.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with valid 8-character square
		[Test]
		public void TestValidateValid8Chars()
		{
			GridSquare grid = new GridSquare("FG21ix04");
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
			GridSquare grid = new GridSquare("FG21iy8f");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(grid.Validate(out err, out modStr));
			Assert.AreEqual("\t'FG21iy8f' is not a valid grid square.", err);
			Assert.IsNull(modStr);
		}
	}
}
