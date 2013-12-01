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
using System.IO;
using System.Reflection;
using NUnit.Framework;
using hamqsler;

namespace hamqslerTest
{
	// tests for Credit_Submitted class
	[TestFixture]
	public class CreditSubmittedTests
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
			Credit_Submitted credit = new Credit_Submitted("IOTA", App.AdifEnums);
			Assert.AreEqual("<Credit_Submitted:4>IOTA", credit.ToAdifString());
		}
		
		// test ToAdifString with multiple credits
		[Test]
		public void TestToAdifStringMultipleCredits()
		{
			Credit_Submitted credit = 
				new Credit_Submitted("IOTA, DXCC_BAND,DXCC_MODE", App.AdifEnums);
			Assert.AreEqual("<Credit_Submitted:24>IOTA,DXCC_BAND,DXCC_MODE", credit.ToAdifString());
		}
		
		// test ToAdifString with multiple credits and QSL medium
		[Test]
		public void TestToAdifStringMultipleCreditsWithMedium()
		{
			Credit_Submitted credit = 
				new Credit_Submitted("IOTA, DXCC_BAND:CARD&LOTW,DXCC_MODE", App.AdifEnums);
			Assert.AreEqual("<Credit_Submitted:34>IOTA,DXCC_BAND:CARD&LOTW,DXCC_MODE", credit.ToAdifString());
		}
		
		// test ReplaceAwardWithCredit with credits only
		[Test]
		public void TestReplaceAwardsWithCreditsOnlyCredits()
		{
			Credit_Submitted credit = new Credit_Submitted("IOTA,DXCC_BAND,DXCC_MODE", App.AdifEnums);
			string err = string.Empty;
			credit.ReplaceAwardsWithCredits(ref err);
			Assert.AreEqual("<Credit_Submitted:24>IOTA,DXCC_BAND,DXCC_MODE", credit.ToAdifString());
			Assert.IsNull(err);
		}
		
		// test ReplaceAwardWithCredit with award and credits
		[Test]
		public void TestReplaceAwardsWithCreditsAwardAndCredits()
		{
			Credit_Submitted credit = new Credit_Submitted("IOTA,DXCC_BAND,CQWAZ_CW", App.AdifEnums);
			string err = string.Empty;
			credit.ReplaceAwardsWithCredits(ref err);
			Assert.AreEqual("<Credit_Submitted:25>IOTA,DXCC_BAND,CQWAZ_MODE", credit.ToAdifString());
			Assert.AreEqual("\t\tAward 'CQWAZ_CW' replaced with Credit 'CQWAZ_MODE'." +
			                Environment.NewLine, err);
		}

		// test ReplaceAwardWithCredit with award that has no replacement
		[Test]
		public void TestReplaceAwardsWithCreditsNoReplacementAward()
		{
			Credit_Submitted credit = new Credit_Submitted("IOTA,JCG,CQWAZ_CW", App.AdifEnums);
			string err = string.Empty;
			credit.ReplaceAwardsWithCredits(ref err);
			Assert.AreEqual("<Credit_Submitted:15>IOTA,CQWAZ_MODE", credit.ToAdifString());
			Assert.AreEqual("\t\tAward 'JCG' deleted because there is no equivalent Credit." +
			                Environment.NewLine +
			                "\t\tAward 'CQWAZ_CW' replaced with Credit 'CQWAZ_MODE'." +
			                Environment.NewLine, err);
		}

		// test ReplaceAwardWithCredit with award that has same replacement as a credit in the list
		[Test]
		public void TestReplaceAwardsWithCreditsAwardReplacementSameAsCredit()
		{
			Credit_Submitted credit = new Credit_Submitted("IOTA,JCG,CQWAZ_CW,CQWAZ_MODE", 
			                                               App.AdifEnums);
			string err = string.Empty;
			credit.ReplaceAwardsWithCredits(ref err);
			Assert.AreEqual("<Credit_Submitted:15>IOTA,CQWAZ_MODE", credit.ToAdifString());
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
			Credit_Submitted credit = 
				new Credit_Submitted("IOTA,JCG,CQWAZ_CW,CQWAZ_MODE:CARD&LOTW", App.AdifEnums);
			string err = string.Empty;
			credit.ReplaceAwardsWithCredits(ref err);
			Assert.AreEqual("<Credit_Submitted:36>IOTA,CQWAZ_MODE,CQWAZ_MODE:CARD&LOTW", credit.ToAdifString());
			Assert.AreEqual("\t\tAward 'JCG' deleted because there is no equivalent Credit." +
			                Environment.NewLine +
			                "\t\tAward 'CQWAZ_CW' replaced with Credit 'CQWAZ_MODE'." +
			                Environment.NewLine, err);
		}
	}
}
