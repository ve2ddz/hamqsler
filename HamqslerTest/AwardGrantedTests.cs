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
	/// <summary>
	/// Tests for Award_Granted class
	/// </summary>
	[TestFixture]
	public class AwardGrantedTests
	{
		/// <summary>
		/// Test Count accessor
		/// </summary>
		[Test]
		public void TestCount()
		{
		    // get the hamqsler assembly
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            // get a stream for the AdifEnumerations.xml file
            // TODO: This is currently an embedded resource in the assembly, but needs to be moved to AppData
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
             // load in the xml file
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Award_Granted aSub = new Award_Granted("ARRL_DXCC_CW,DARC_DOC_100,CQ_USACA_500", aEnums);
			Assert.AreEqual(3, aSub.Count);
		}
		
		/// <summary>
		/// Test Validate method with valid awards
		/// </summary>
		[Test]
		public void TestValidateOK()
		{
		    // get the hamqsler assembly
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            // get a stream for the AdifEnumerations.xml file
            // TODO: This is currently an embedded resource in the assembly, but needs to be moved to AppData
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
             // load in the xml file
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Award_Granted aSub = new Award_Granted("ARRL_DXCC_CW,DARC_DOC_100,CQ_USACA_500", aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(aSub.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid award
		[Test]
		public void TestValidateBadEntry()
		{
		    // get the hamqsler assembly
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            // get a stream for the AdifEnumerations.xml file
            // TODO: This is currently an embedded resource in the assembly, but needs to be moved to AppData
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
             // load in the xml file
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Award_Granted aSub = new Award_Granted("ARRL_DXCC_CW,DARCDOC_100,CQ_USACA_500", aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(aSub.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.AreEqual("The sponsors portion of Awards_Granted is an enumeration." +
			                Environment.NewLine +
			                "The value 'DARCDOC_' was not found in enumeration",
			                modStr);
		}
		
		// test Validate method with no awards
		[Test]
		public void TestValidateNoAwards()
		{
		    // get the hamqsler assembly
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            // get a stream for the AdifEnumerations.xml file
            // TODO: This is currently an embedded resource in the assembly, but needs to be moved to AppData
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
             // load in the xml file
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Award_Granted aSub = new Award_Granted(string.Empty, aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(aSub.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		[Test]
		public void TestValidateNull()
		{
		    // get the hamqsler assembly
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            // get a stream for the AdifEnumerations.xml file
            // TODO: This is currently an embedded resource in the assembly, but needs to be moved to AppData
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
             // load in the xml file
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Award_Granted aSub = new Award_Granted(null, aEnums);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(aSub.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
		    // get the hamqsler assembly
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            // get a stream for the AdifEnumerations.xml file
            // TODO: This is currently an embedded resource in the assembly, but needs to be moved to AppData
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
             // load in the xml file
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Award_Granted aSub = new Award_Granted("ARRL_DXCC_CW,DARC_DOC_100,CQ_USACA_500", aEnums);
			Assert.AreEqual("<Award_Granted:38>ARRL_DXCC_CW,DARC_DOC_100,CQ_USACA_500",
			                aSub.ToAdifString());
		}
		
		// test ToAdifString with null string
		[Test]
		public void TestToAdifStringNull()
		{
		    // get the hamqsler assembly
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            // get a stream for the AdifEnumerations.xml file
            // TODO: This is currently an embedded resource in the assembly, but needs to be moved to AppData
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
             // load in the xml file
			AdifEnumerations aEnums = new AdifEnumerations(str);
			Award_Granted aSub = new Award_Granted(null, aEnums);
			Assert.AreEqual("<Award_Granted:0>",
			                aSub.ToAdifString());
		}
	}
}
