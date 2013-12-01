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
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using hamqsler;

namespace hamqslerTest
{
	// tests for Credit class
	[TestFixture]
	public class CreditTests
	{
		// TestFixtureSetup
		[TestFixtureSetUp]
		public void TestSepup()
		{
			App.AdifEnums.LoadDocument();
		}
		
		// test CreditName accessor for credit without QSL medium
		[Test]
		public void TestCreditNameNoMedium()
		{
			Credit credit = new Credit("IOTA", App.AdifEnums);
			Assert.AreEqual("IOTA", credit.CreditName);
		}
		
		// test CreditName accessor for credit with multiple QSL medium fields
		[Test]
		public void TestCreditNameMultipleMediums()
		{
			Credit credit = new Credit("IOTA:Lotw&card", App.AdifEnums);
			Assert.AreEqual("IOTA", credit.CreditName);
		}
		
		// test Media accessor for credit with no QSL medium
		[Test]
		public void TestMediaNoMedia()
		{
			Credit credit = new Credit("IOTA", App.AdifEnums);
			string[] media = new string[credit.Media.Count];
			credit.Media.CopyTo(media);
			Assert.IsNull(media[0]);
		}
		
		// test Media access for credit with two media
		[Test]
		public void TestMediaTwoMedia()
		{
			Credit credit = new Credit("IOTA:Lotw&card", App.AdifEnums);
			string[] media = new string[credit.Media.Count];
			credit.Media.CopyTo(media);
			Assert.AreEqual("LOTW", media[0]);
			Assert.AreEqual("CARD", media[1]);
		}
			
		// test ToString for single member of credit enumeration
		[Test]
		public void TestToStringSingleCredit()
		{
			Credit credit = new Credit("IOTA", App.AdifEnums);
			Assert.AreEqual("IOTA", credit.ToString());
		}
		
		// test ToString for credit and single QSL medium
		[Test]
		public void TestToStringCreditAndSingleMedium()
		{
			Credit credit = new Credit("IOTA:CARD&Lotw", App.AdifEnums);
			Assert.AreEqual("IOTA:CARD&LOTW", credit.ToString());
		}
		
		// test IsInMedia for null medium
		[Test]
		public void TestIsInMediaNull()
		{
			Credit credit = new Credit("IOTA", App.AdifEnums);
			Assert.IsTrue(credit.IsInMedia(null));
		}
		
		// test IsInMedia for medium in list of media
		[Test]
		public void TestIsInMediaTrue()
		{
			Credit credit = new Credit("IOTA:Card&Lotw&eQSL", App.AdifEnums);
			Assert.IsTrue(credit.IsInMedia("CARD"));
			Assert.IsTrue(credit.IsInMedia("LOTW"));
			Assert.IsTrue(credit.IsInMedia("eqsl"));
		}
		
		// test IsInMedia for medium not in list of media
		[Test]
		public void TestIsInMediaFalse()
		{
			Credit credit = new Credit("IOTA:Card&Lotw&eQSL", App.AdifEnums);
			Assert.IsFalse(credit.IsInMedia("QRZCOM"));
		}
		
		// test Validate with valid credit, no media
		[Test]
		public void TestValidateValidCreditNoMedia()
		{
			Credit credit = new Credit("IOTA", App.AdifEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(credit.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid credit, no Media
		[Test]
		public void TestValidateInvalidCreditNoMedia()
		{
			Credit credit = new Credit("IOTA2", App.AdifEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(credit.Validate(out err, out modStr));
			Assert.AreEqual("\t'IOTA2' not found in Credit enumeration", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with valid credit, single invalid Media
		[Test]
		public void TestValidateValidCreditSingleInvalidMedia()
		{
			Credit credit = new Credit("IOTA:crad", App.AdifEnums);
			string err = string.Empty;
			string modStr =string.Empty;
			Assert.IsFalse(credit.Validate(out err, out modStr));
			Assert.AreEqual("\t'CRAD' not found in QSL Medium enumeration", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with valid credit, null media and other media
		[Test]
		public void TestValidateValidCreditNullAndValidMedia()
		{
			Credit credit = new Credit("IOTA", App.AdifEnums);
			credit.Media.Add("Card");
			credit.Media.Add("LotW");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(credit.Validate(out err, out modStr));
			Assert.AreEqual("\tProgramming Error: Credit object cannot contain both null and other QSL Media", err);
			Assert.IsNull(modStr);
		}
	}
}
