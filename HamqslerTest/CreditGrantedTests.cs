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
	// tests for Credit_Granted class
	[TestFixture]
	public class CreditGrantedTests
	{
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Credit_Granted credit = new Credit_Granted("IOTA", aEnums);
			Assert.AreEqual("<Credit_Granted:4>IOTA", credit.ToAdifString());
		}
		
		// test ToAdifString with multiple credits
		[Test]
		public void TestToAdifStringMultipleCredits()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Credit_Granted credit = new Credit_Granted("IOTA, DXCC_BAND,DXCC_MODE", aEnums);
			Assert.AreEqual("<Credit_Granted:24>IOTA,DXCC_BAND,DXCC_MODE", credit.ToAdifString());
		}
		
		// test ToAdifString with multiple credits and QSL medium
		[Test]
		public void TestToAdifStringMultipleCreditsWithMedium()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Credit_Granted credit = new Credit_Granted("IOTA, DXCC_BAND:CARD&LOTW,DXCC_MODE", aEnums);
			Assert.AreEqual("<Credit_Granted:34>IOTA,DXCC_BAND:CARD&LOTW,DXCC_MODE", credit.ToAdifString());
		}
	}
}
