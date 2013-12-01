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
using System.Text;
using NUnit.Framework;
using hamqsler;

namespace hamqslerTest
{
	[TestFixture]
	public class CallsBureausTests
	{
		CallsBureaus callsBureaus;
		
		// fixture setup
		[TestFixtureSetUp]
		public void Init()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
	        Stream str = assembly.GetManifestResourceStream("hamqsler.CallsBureaus.xml");
			callsBureaus = new CallsBureaus();
			callsBureaus.LoadDocument(str);
		}

		// test constructor and get version
		[Test]
		public void TestVersion()
		{
			string cbString = "<?xml version='1.0'?>" + Environment.NewLine +
							"<CallsBureaus Version='0.5.2'>" + Environment.NewLine +
							"<Call>BM100</Call>" + Environment.NewLine +
							"</CallsBureaus>" + Environment.NewLine;
			MemoryStream str = new MemoryStream(Encoding.ASCII.GetBytes(cbString));
			CallsBureaus cBureaus = new CallsBureaus();
			cBureaus.LoadDocument(str);
			str.Close();
			Assert.AreEqual("0.5.2", cBureaus.Version);
		}
		
		// test IsNonStandardCall
		[Test, Sequential]
		public void TestIsNonStandardCall(
			[Values("8J1TS50A", "8J2KSG7X", "BM100",
			       "BN100", "BO100", "BP100",
			      "BQ100", "BU100", "BV100",
			      "BW100", "BX100", "LM9L40Y",
			      "OD70", "OEH20", "SX1912",
			      "TE1856", "VI6ARG30", "XM31812",
			     "XV2V40J", "ZW1CCOM54", "TE1ST2")] string call,
			[Values(true, true, true,
			       true, true, true,
			      true, true, true,
			     true, true, true,
			    true, true, true,
			   true, true, true,
			  true, true, false)] bool found)
		{
			Assert.AreEqual(found, callsBureaus.IsNonStandardCall(call));
		}
	}
}
