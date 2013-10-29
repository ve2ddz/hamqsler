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
	// tests for My_VUCC_Grids
	[TestFixture]
	public class MyVUCCGridsTests
	{
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			My_VUCC_Grids grids = new My_VUCC_Grids("EN98,FM08,EM97,FM07");
			Assert.AreEqual("<My_VUCC_Grids:19>EN98,FM08,EM97,FM07", grids.ToAdifString());
		}
		
		// test Validate with two valid grid squares
		[Test]
		public void TestValidate2Valid()
		{
			My_VUCC_Grids grids = new My_VUCC_Grids("EN98,EN97");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(grids.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with four valid grid squares
		[Test]
		public void TestValidate4Valid()
		{
			My_VUCC_Grids grids = new My_VUCC_Grids("EN98,EN97,FM08,FM07");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(grids.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with one valid grid square
		[Test]
		public void TestValidate1Valid()
		{
			My_VUCC_Grids grids = new My_VUCC_Grids("EN98");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(grids.Validate(out err, out modStr));
			Assert.AreEqual("\tMy_VUCC_Grids contains 1 grid squares. Must contain either 2 or 4 grid squares.", 
			                err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with 0 grid squares
		[Test]
		public void TestValidate0Valid()
		{
			My_VUCC_Grids grids = new My_VUCC_Grids(string.Empty);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(grids.Validate(out err,out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid grid square
		[Test]
		public void TestValidateInvalid()
		{
			My_VUCC_Grids grids = new My_VUCC_Grids("EN9,EN97");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(grids.Validate(out err, out modStr));
			Assert.AreEqual("\t'EN9' is not a valid My_VUCC_Grids grid square", 
			                err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid grid square
		[Test]
		public void TestValidateInvalid1()
		{
			My_VUCC_Grids grids = new My_VUCC_Grids("EN98,EN9");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(grids.Validate(out err, out modStr));
			Assert.AreEqual("\t'EN9' is not a valid My_VUCC_Grids grid square", 
			                err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid grid square
		[Test]
		public void TestValidateInvalid2()
		{
			My_VUCC_Grids grids = new My_VUCC_Grids("EN98,EN97bf");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(grids.Validate(out err, out modStr));
			Assert.AreEqual("\t'EN97bf' is not a valid My_VUCC_Grids grid square", 
			                err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid grid square
		[Test]
		public void TestValidateInvalid3()
		{
			My_VUCC_Grids grids = new My_VUCC_Grids("AX98,EN97");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(grids.Validate(out err, out modStr));
			Assert.AreEqual("\t'AX98' is not a valid My_VUCC_Grids grid square", 
			                err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid grid square
		[Test]
		public void TestValidateInvalid4()
		{
			My_VUCC_Grids grids = new My_VUCC_Grids("9A98,EN97");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(grids.Validate(out err, out modStr));
			Assert.AreEqual("\t'9A98' is not a valid My_VUCC_Grids grid square", 
			                err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid grid square
		[Test]
		public void TestValidateInvalid5()
		{
			My_VUCC_Grids grids = new My_VUCC_Grids("EN9B,EN97");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(grids.Validate(out err, out modStr));
			Assert.AreEqual("\t'EN9B' is not a valid My_VUCC_Grids grid square", 
			                err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with valid grid squares with spaces
		[Test]
		public void TestValidateSpaces()
		{
			My_VUCC_Grids grids = new My_VUCC_Grids("EN98 , EN97");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(grids.Validate(out err,out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
	}
}
