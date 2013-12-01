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
	// tests for Credit_Granted class
	[TestFixture]
	public class CreditGrantedTests
	{
		// TestFixtureSetup
		[TestFixtureSetUp]
		public void TestSepup()
		{
			App.AdifEnums.LoadDocument();
		}
		
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			Credit_Granted credit = new Credit_Granted("IOTA", App.AdifEnums);
			Assert.AreEqual("<Credit_Granted:4>IOTA", credit.ToAdifString());
		}
		
		// test ToAdifString with multiple credits
		[Test]
		public void TestToAdifStringMultipleCredits()
		{
			Credit_Granted credit = new Credit_Granted("IOTA, DXCC_BAND,DXCC_MODE", App.AdifEnums);
			Assert.AreEqual("<Credit_Granted:24>IOTA,DXCC_BAND,DXCC_MODE", credit.ToAdifString());
		}
		
		// test ToAdifString with multiple credits and QSL medium
		[Test]
		public void TestToAdifStringMultipleCreditsWithMedium()
		{
			Credit_Granted credit = 
				new Credit_Granted("IOTA, DXCC_BAND:CARD&LOTW,DXCC_MODE", App.AdifEnums);
			Assert.AreEqual("<Credit_Granted:34>IOTA,DXCC_BAND:CARD&LOTW,DXCC_MODE", credit.ToAdifString());
		}
		
		// test Validate with multiple credits and QSL media
		[Test]
		public void TestValidateMultipleCreditsQslMedia()
		{
			string err = string.Empty;
			string mod = string.Empty;
			Credit_Granted credit = 
				new Credit_Granted("DXCC:card,DXCC_BAND:card,DXCC_Mode:card", App.AdifEnums);
			bool valid = credit.Validate(out err, out mod);
			Assert.IsTrue(valid);
			Assert.AreEqual(null, err);
			Assert.AreEqual(null, mod);
		}
		
		// test ReplaceAwardWithCredit with credits only
		[Test]
		public void TestReplaceAwardsWithCreditsOnlyCredits()
		{
			Credit_Granted credit = 
				new Credit_Granted("IOTA,DXCC_BAND,DXCC_MODE", App.AdifEnums);
			string err = string.Empty;
			credit.ReplaceAwardsWithCredits(ref err);
			Assert.AreEqual("<Credit_Granted:24>IOTA,DXCC_BAND,DXCC_MODE", credit.ToAdifString());
			Assert.IsNull(err);
		}
		
		// test ReplaceAwardWithCredit with award and credits
		[Test]
		public void TestReplaceAwardsWithCreditsAwardAndCredits()
		{
			Credit_Granted credit = 
				new Credit_Granted("IOTA,DXCC_BAND,CQWAZ_CW", App.AdifEnums);
			string err = string.Empty;
			credit.ReplaceAwardsWithCredits(ref err);
			Assert.AreEqual("<Credit_Granted:25>IOTA,DXCC_BAND,CQWAZ_MODE", credit.ToAdifString());
			Assert.AreEqual("\t\tAward 'CQWAZ_CW' replaced with Credit 'CQWAZ_MODE'." +
			                Environment.NewLine, err);
		}

		// test ReplaceAwardWithCredit with award that has no replacement
		[Test]
		public void TestReplaceAwardsWithCreditsNoReplacementAward()
		{
			Credit_Granted credit = 
				new Credit_Granted("IOTA,JCG,CQWAZ_CW", App.AdifEnums);
			string err = string.Empty;
			credit.ReplaceAwardsWithCredits(ref err);
			Assert.AreEqual("<Credit_Granted:15>IOTA,CQWAZ_MODE", credit.ToAdifString());
			Assert.AreEqual("\t\tAward 'JCG' deleted because there is no equivalent Credit." +
			                Environment.NewLine +
			                "\t\tAward 'CQWAZ_CW' replaced with Credit 'CQWAZ_MODE'." +
			                Environment.NewLine, err);
		}

		// test ReplaceAwardWithCredit with award that has same replacement as a credit in the list
		[Test]
		public void TestReplaceAwardsWithCreditsAwardReplacementSameAsCredit()
		{
			Credit_Granted credit = 
				new Credit_Granted("IOTA,JCG,CQWAZ_CW,CQWAZ_MODE", App.AdifEnums);
			string err = string.Empty;
			credit.ReplaceAwardsWithCredits(ref err);
			Assert.AreEqual("<Credit_Granted:15>IOTA,CQWAZ_MODE", credit.ToAdifString());
			Assert.AreEqual("\t\tAward 'JCG' deleted because there is no equivalent Credit." +
			                Environment.NewLine +
			                "\t\tAward 'CQWAZ_CW' replaced with Credit 'CQWAZ_MODE'." +
			                Environment.NewLine, err);
		}


		// test ReplaceAwardWithCredit with award that has same replacement as a credit in the list
		// but without the QSL medium value
		[Test]
		public void TestReplaceAwardsWithCreditsAwardReplacementSameAsCreditWithoutMedium()
		{
			Credit_Granted credit = 
				new Credit_Granted("IOTA,JCG,CQWAZ_CW,CQWAZ_MODE:CARD&LOTW", App.AdifEnums);
			string err = string.Empty;
			credit.ReplaceAwardsWithCredits(ref err);
			Assert.AreEqual("<Credit_Granted:36>IOTA,CQWAZ_MODE,CQWAZ_MODE:CARD&LOTW", credit.ToAdifString());
			Assert.AreEqual("\t\tAward 'JCG' deleted because there is no equivalent Credit." +
			                Environment.NewLine +
			                "\t\tAward 'CQWAZ_CW' replaced with Credit 'CQWAZ_MODE'." +
			                Environment.NewLine, err);
		}

		// test add credit with medium, credit exists but medium different
		[Test]
		public void TestAddMediumToExistingCredit()
		{
			string err = string.Empty;
			Qso2 qso = new Qso2("<Credit_Granted:49>DXCC:CARD&LOTW,DXCC_MODE:CARD&LOTW,DXCC_BAND:CARD",
			                    App.AdifEnums, ref err);
			Credit_Granted granted = qso.GetField("Credit_Granted") as Credit_Granted;
			List<Credit> creds = granted.GetCredits("DXCC");
			Assert.AreEqual(1, creds.Count);
			Assert.AreEqual(2, creds[0].Media.Count);
			Assert.AreEqual("DXCC:CARD&LOTW", creds[0].ToString());
			granted.Add(new Credit("DXCC:EQSL", App.AdifEnums));
			creds = granted.GetCredits("DXCC");
			Assert.AreEqual(3, creds[0].Media.Count);
			Assert.AreEqual("DXCC:CARD&LOTW&EQSL", creds[0].ToString());
			Assert.IsTrue(creds[0].IsInMedia("EQSL"));
		}
	}
}
