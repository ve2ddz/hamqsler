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
	// tests for VE_Prov
	[TestFixture]
	public class VEProvTests
	{
		AdifEnumerations aEnums;
		
		// fixture setup
		[TestFixtureSetUp]
		public void Init()
		{
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
	        Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
			aEnums = new AdifEnumerations(str);
		}

		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			VE_Prov prov = new VE_Prov("ON");
			Assert.AreEqual("<VE_Prov:2>ON", prov.ToAdifString());
		}
	
		// test ModifyValues with no State
		[Test]
		public void TestModifyValuesNoState()
		{
			string err = string.Empty;
			Qso2 qso = new Qso2("<VE_PROV:2>ON", aEnums, ref err);
			VE_Prov prov = qso.GetField("VE_Prov") as VE_Prov;
			Assert.IsNotNull(prov);
			string mod = prov.ModifyValues(qso);
			prov = qso.GetField("VE_Prov") as VE_Prov;
			Assert.IsNull(prov);
			State state = qso.GetField("State") as State;
			Assert.AreEqual("ON", state.Value);
			Assert.AreEqual("\tVE_Prov field deprecated. VE_Prov field deleted and replaced with State field." +
			                Environment.NewLine, mod);
		}
		
		// test ModifyValues with State
		[Test]
		public void TestModifyValuesState()
		{
			string err = string.Empty;
			Qso2 qso = new Qso2("<VE_PROV:2>ON<STATE:2>AB", aEnums, ref err);
			VE_Prov prov = qso.GetField("VE_Prov") as VE_Prov;
			Assert.IsNotNull(prov);
			string mod = prov.ModifyValues(qso);
			prov = qso.GetField("VE_Prov") as VE_Prov;
			Assert.IsNull(prov);
			State state = qso.GetField("State") as State;
			Assert.AreEqual("AB", state.Value);
			Assert.AreEqual("\tVE_Prov field deprecated. State field already exists, so VE_Prov field deleted." +
			                Environment.NewLine, mod);
		}
	}
}
