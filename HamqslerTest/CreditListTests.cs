﻿/*
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
	// tests for CreditList class
	[TestFixture]
	public class CreditListTests
	{
		// test GetCredits with single matching credit in list
		[Test]
		public void TestGetCreditsSingleMatchingCredit()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Credit credit = new Credit("IOTA", aEnums);
			CreditList list = new CreditList();
			list.Add(credit);
			List<Credit> credits = list.GetCredits("IOTA");
			Assert.AreEqual(1, credits.Count);
			Assert.AreEqual("IOTA", credits[0].CreditName);
			string[] media = new string[credits[0].Media.Count];
			credits[0].Media.CopyTo(media);
			Assert.AreEqual(null, media[0]);
		}
		
		// test GetCredits with 2 matching credits in list
		[Test]
		public void TestGetCreditsTwoMatchingCredits()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Credit credit = new Credit("IOTA", aEnums);
			CreditList list = new CreditList();
			list.Add(credit);
			Credit notIOTA = new Credit("WAS", aEnums);
			list.Add(notIOTA);
			Credit credit2 = new Credit("IOTA:Card&Lotw", aEnums);
			list.Add(credit2);
			List<Credit> credits = list.GetCredits("IOTA");
			Assert.AreEqual(2, credits.Count);
			Assert.AreEqual("IOTA", credits[0].CreditName);
			string[] media = new string[credits[0].Media.Count];
			credits[0].Media.CopyTo(media);
			Assert.AreEqual(null, media[0]);
			Assert.AreEqual("IOTA", credits[1].CreditName);
			media = new string[credits[1].Media.Count];
			credits[1].Media.CopyTo(media);
			Assert.AreEqual("CARD", media[0]);
			Assert.AreEqual("LOTW", media[1]);
		}
		
		// test GetCredits with no matching credits in list
		[Test]
		public void TestGetCreditsNoMatchingCredits()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Credit credit = new Credit("IOTA", aEnums);
			CreditList list = new CreditList();
			list.Add(credit);
			Credit notIOTA = new Credit("WAS", aEnums);
			list.Add(notIOTA);
			Credit credit2 = new Credit("IOTA:Card&Lotw", aEnums);
			list.Add(credit2);
			List<Credit> credits = list.GetCredits("WAB");
			Assert.AreEqual(0, credits.Count);
		}
		
		// test Add with single Credit object
		[Test]
		public void TestAddSingleCredit()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Credit credit = new Credit("IOTA", aEnums);
			CreditList list = new CreditList();
			list.Add(credit);
			Assert.AreEqual(1, list.Count);
			List<Credit> credits = list.GetCredits("IOTA");
			Assert.AreEqual(1, credits.Count);
			Assert.AreEqual("IOTA", credits[0].CreditName);
			string[] media = new string[credits[0].Media.Count];
			credits[0].Media.CopyTo(media);
			Assert.AreEqual(null, media[0]);
		}
		
		// test Add with two Credit objects, different Credit names
		[Test]
		public void TestAddTwoCredits()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Credit credit = new Credit("IOTA", aEnums);
			CreditList list = new CreditList();
			list.Add(credit);
			Assert.AreEqual(1, list.Count);
			Credit c2 = new Credit("WAS:CARD", aEnums);
			list.Add(c2);
			Assert.AreEqual(2, list.Count);
			List<Credit> credits = list.GetCredits("IOTA");
			Assert.AreEqual(1, credits.Count);
			Assert.AreEqual("IOTA", credits[0].CreditName);
			string[] media = new string[credits[0].Media.Count];
			credits[0].Media.CopyTo(media);
			Assert.AreEqual(null, media[0]);
			credits = list.GetCredits("WAS");
			Assert.AreEqual(1, credits.Count);
			Assert.AreEqual("WAS", credits[0].CreditName);
		}
		
		// test Add with two Credit objects, same name, one with no medium and one with medium
		[Test]
		public void TestAddTwoCreditsNullAndMedium()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Credit credit = new Credit("IOTA", aEnums);
			CreditList list = new CreditList();
			list.Add(credit);
			Assert.AreEqual(1, list.Count);
			Credit c2 = new Credit("IOTA:CARD", aEnums);
			list.Add(c2);
			Assert.AreEqual(2, list.Count);
			List<Credit> credits = list.GetCredits("IOTA");
			Assert.AreEqual(2, credits.Count);
			Assert.AreEqual("IOTA", credits[0].CreditName);
			string[] media = new string[credits[0].Media.Count];
			credits[0].Media.CopyTo(media);
			Assert.AreEqual(null, media[0]);
			media = new string[credits[1].Media.Count];
			credits[1].Media.CopyTo(media);
			Assert.AreEqual(1, media.Length);
			Assert.AreEqual("CARD", media[0]);
		}
		
		// test Add with two Credit objects, same name and no media in both
		[Test]
		public void TestAddTwoCreditsBothNullMedia()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Credit credit = new Credit("IOTA", aEnums);
			CreditList list = new CreditList();
			list.Add(credit);
			Assert.AreEqual(1, list.Count);
			Credit c2 = new Credit("IOTA", aEnums);
			list.Add(c2);
			Assert.AreEqual(1, list.Count);
			List<Credit> credits = list.GetCredits("IOTA");
			Assert.AreEqual(1, credits.Count);
			Assert.AreEqual("IOTA", credits[0].CreditName);
			string[] media = new string[credits[0].Media.Count];
			credits[0].Media.CopyTo(media);
			Assert.AreEqual(null, media[0]);
		}
		
		
		// test Add with two Credit objects, same name, different medium
		[Test]
		public void TestAddTwoCreditsDifferentMedia()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Credit credit = new Credit("IOTA:EQSL&LOTW", aEnums);
			CreditList list = new CreditList();
			list.Add(credit);
			Credit c2 = new Credit("IOTA:CARD", aEnums);
			list.Add(c2);
			Assert.AreEqual(1, list.Count);
			List<Credit> credits = list.GetCredits("IOTA");
			Assert.AreEqual(1, credits.Count);
			Assert.AreEqual("IOTA", credits[0].CreditName);
			string[] media = new string[credits[0].Media.Count];
			credits[0].Media.CopyTo(media);
			Assert.AreEqual(3, media.Length);
			Assert.AreEqual("EQSL", media[0]);
			Assert.AreEqual("LOTW", media[1]);
			Assert.AreEqual("CARD", media[2]);
		}
		
		// test constructor with string of 4 credits
		[Test]
		public void TestConstructorWithString4Credits()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			CreditList list = new CreditList("WAS:LOTW,IOTA,DXCC_BAND:LOTW&CARD,DXCC_MODE:LOTW&CARD",
			                                aEnums);
			Assert.AreEqual(4, list.Count);
		}
		
		// test constructor with string of 1 credit, no media
		[Test]
		public void TestConstructorWithString1CreditNoMedia()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			CreditList list = new CreditList("IOTA", aEnums);
			Assert.AreEqual(1, list.Count);
		}
		
		// test ToAdifString with 1 credit
		[Test]
		public void TestToAdifString1Credit()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			CreditList list = new CreditList("IOTA", aEnums);
			Assert.AreEqual("<CreditList:4>IOTA", list.ToAdifString());
		}
		
		// test ToAdifString with 4 credits
		[Test]
		public void TestToAdifString4Credits()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			CreditList list = new CreditList("WAS:LOTW,IOTA,DXCC_BAND:LOTW&CARD,DXCC_MODE:LOTW&CARD", aEnums);
			Assert.AreEqual("<CreditList:53>WAS:LOTW,IOTA,DXCC_BAND:LOTW&CARD,DXCC_MODE:LOTW&CARD", list.ToAdifString());
		}
		
		// test ToAdifString with 0 credits
		[Test]
		public void TestToAdifString0Credits()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			CreditList list = new CreditList();
			Assert.AreEqual("<CreditList:0>", list.ToAdifString());
		}
		
		
		// test Validate with valid single credit
		[Test]
		public void TestValidateValidSingleCredit()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			CreditList credit = new CreditList("IOTA", aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(credit.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with valid multiple credits
		[Test]
		public void TestValidateValidMultipleCredits()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			CreditList credit = new CreditList("IOTA,DXCC_BAND,DXCC_MODE", aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(credit.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with single invalid credit
		[Test]
		public void TestValidateInvalidSingleCredit()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			CreditList credit = new CreditList("IOTA2", aEnums);
			string err = string.Empty;
			string modStr =string.Empty;
			Assert.IsFalse(credit.Validate(out err, out modStr));
			Assert.AreEqual("'IOTA2' not found in Credit enumeration", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with single invalid credit in list of credits
		[Test]
		public void TestValidateInvalidSingleCreditInList()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			CreditList credit = new CreditList("IOTA,DXCC_BANE,DXCC_MODE", aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(credit.Validate(out err, out modStr));
			Assert.AreEqual("'DXCC_BANE' not found in Credit enumeration", err);
			Assert.IsNull(modStr);
		}

		// test ReplaceAwardWithCredit with credits only
		[Test]
		public void TestReplaceAwardsWithCreditsOnlyCredits()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			CreditList credit = new CreditList("IOTA,DXCC_BAND,DXCC_MODE", aEnums);
			string err = string.Empty;
			credit.ReplaceAwardsWithCredits();
			Assert.AreEqual("<CreditList:24>IOTA,DXCC_BAND,DXCC_MODE", credit.ToAdifString());
		}
		
		// test ReplaceAwardWithCredit with award and credits
		[Test]
		public void TestReplaceAwardsWithCreditsAwardAndCredits()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			CreditList credit = new CreditList("IOTA,DXCC_BAND,CQWAZ_CW", aEnums);
			string err = string.Empty;
			credit.ReplaceAwardsWithCredits();
			Assert.AreEqual("<CreditList:25>IOTA,DXCC_BAND,CQWAZ_MODE", credit.ToAdifString());
		}

		// test ReplaceAwardWithCredit with award that has no replacement
		[Test]
		public void TestReplaceAwardsWithCreditsNoReplacementAward()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			CreditList credit = new CreditList("IOTA,JCG,CQWAZ_CW", aEnums);
			string err = string.Empty;
			credit.ReplaceAwardsWithCredits();
			Assert.AreEqual("<CreditList:15>IOTA,CQWAZ_MODE", credit.ToAdifString());
		}

		// test ReplaceAwardWithCredit with award that has same replacement as a credit in the list
		[Test]
		public void TestReplaceAwardsWithCreditsAwardReplacementSameAsCredit()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			CreditList credit = new CreditList("IOTA,JCG,CQWAZ_CW,CQWAZ_MODE", aEnums);
			string err = string.Empty;
			credit.ReplaceAwardsWithCredits();
			Assert.AreEqual("<CreditList:15>IOTA,CQWAZ_MODE", credit.ToAdifString());
		}


		// test ReplaceAwardWithCredit with award that has same replacement as a credit in the list
		// but without the QSL medium value
		[Test]
		public void TestReplaceAwardsWithCreditsAwardReplacementSameAsCreditWithoutMedium()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			CreditList credit = new CreditList("IOTA,JCG,CQWAZ_CW,CQWAZ_MODE:CARD&LOTW", aEnums);
			string err = string.Empty;
			credit.ReplaceAwardsWithCredits();
			Assert.AreEqual("<CreditList:36>IOTA,CQWAZ_MODE,CQWAZ_MODE:CARD&LOTW", credit.ToAdifString());
		}
	}
}
