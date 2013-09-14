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
		// test CreditName accessor for credit without QSL medium
		[Test]
		public void TestCreditNameNoMedium()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Credit credit = new Credit("IOTA", aEnums);
			Assert.AreEqual("IOTA", credit.CreditName);
		}
		
		// test CreditName accessor for credit with multiple QSL medium fields
		[Test]
		public void TestCreditNameMultipleMediums()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Credit credit = new Credit("IOTA:Lotw&card", aEnums);
			Assert.AreEqual("IOTA", credit.CreditName);
		}
		
		// test Media accessor for credit with no QSL medium
		[Test]
		public void TestMediaNoMedia()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Credit credit = new Credit("IOTA", aEnums);
			string[] media = new string[credit.Media.Count];
			credit.Media.CopyTo(media);
			Assert.IsNull(media[0]);
		}
		
		// test Media access for credit with two media
		[Test]
		public void TestMediaTwoMedia()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Credit credit = new Credit("IOTA:Lotw&card", aEnums);
			string[] media = new string[credit.Media.Count];
			credit.Media.CopyTo(media);
			Assert.AreEqual("LOTW", media[0]);
			Assert.AreEqual("CARD", media[1]);
		}
			
		// test ToString for single member of credit enumeration
		[Test]
		public void TestToStringSingleCredit()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Credit credit = new Credit("IOTA", aEnums);
			Assert.AreEqual("IOTA", credit.ToString());
		}
		
		// test ToString for credit and single QSL medium
		[Test]
		public void TestToStringCreditAndSingleMedium()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Credit credit = new Credit("IOTA:CARD&Lotw", aEnums);
			Assert.AreEqual("IOTA:CARD&LOTW", credit.ToString());
		}
		
		// test IsInMedia for null medium
		[Test]
		public void TestIsInMediaNull()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Credit credit = new Credit("IOTA", aEnums);
			Assert.IsTrue(credit.IsInMedia(null));
		}
		
		// test IsInMedia for medium in list of media
		[Test]
		public void TestIsInMediaTrue()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Credit credit = new Credit("IOTA:Card&Lotw&eQSL", aEnums);
			Assert.IsTrue(credit.IsInMedia("CARD"));
			Assert.IsTrue(credit.IsInMedia("LOTW"));
			Assert.IsTrue(credit.IsInMedia("eqsl"));
		}
		
		// test IsInMedia for medium not in list of media
		[Test]
		public void TestIsInMediaFalse()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Credit credit = new Credit("IOTA:Card&Lotw&eQSL", aEnums);
			Assert.IsFalse(credit.IsInMedia("QRZCOM"));
		}
		
		// test Validate with valid credit, no media
		[Test]
		public void TestValidateValidCreditNoMedia()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Credit credit = new Credit("IOTA", aEnums);
			string err = string.Empty;
			Assert.IsTrue(credit.Validate(out err));
			Assert.AreEqual(null, err);
		}
		
		// test Validate with invalid credit, no Media
		[Test]
		public void TestValidateInvalidCreditNoMedia()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Credit credit = new Credit("IOTA2", aEnums);
			string err = string.Empty;
			Assert.IsFalse(credit.Validate(out err));
			Assert.AreEqual("'IOTA2' not found in Credit enumeration", err);
		}
		
		// test Validate with valid credit, single invalid Media
		[Test]
		public void TestValidateValidCreditSingleInvalidMedia()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Credit credit = new Credit("IOTA:crad", aEnums);
			string err = string.Empty;
			Assert.IsFalse(credit.Validate(out err));
			Assert.AreEqual("'CRAD' not found in QSL Medium enumeration", err);
		}
		
		// test Validate with valid credit, null media and other media
		[Test]
		public void TestValidateValidCreditNullAndValidMedia()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Credit credit = new Credit("IOTA", aEnums);
			credit.Media.Add("Card");
			credit.Media.Add("LotW");
			string err = string.Empty;
			Assert.IsFalse(credit.Validate(out err));
			Assert.AreEqual("Programming Error: Credit object cannot contain both null and other QSL Media", err);
		}
		
	}
}
