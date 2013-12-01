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
	[TestFixture]
	public class ArrlSectTest
	{
		// TestFixtureSetup
		[TestFixtureSetUp]
		public void TestSepup()
		{
			App.AdifEnums.LoadDocument();
		}
		
		[Test]
		public void TestConstructor()
		{
			string err = string.Empty;
			string modStr = string.Empty;
			Arrl_Sect sect = new Arrl_Sect("NT", App.AdifEnums);
			Assert.IsTrue(sect.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}


		// test that constructor creates valid object
		[Test]
		public void TestConstructor1()
		{
			string err = string.Empty;
			string modStr = string.Empty;
			Arrl_Sect sect = new Arrl_Sect("ABCD", App.AdifEnums);
			Assert.IsFalse(sect.Validate(out err, out modStr));
			Assert.AreEqual("\tThis QSO Field is of type enumeration. The value 'ABCD' was not found in enumeration.",
			                err);
			Assert.IsNull(modStr);
		}
		
		// test that ToAdifString returns correct value
		[Test]
		public void TestToAdifString()
		{
			Arrl_Sect sect = new Arrl_Sect("NT", App.AdifEnums);
			Assert.AreEqual("<Arrl_Sect:2>NT", sect.ToAdifString());
		}
		
		// test modification of NWT section to NT
		[Test]
		public void TestModifyNWT()
		{
			string err = string.Empty;
			Qso2 qso = new Qso2("<arrl_sect:3>NWT", App.AdifEnums, ref err);
			Arrl_Sect nwt = qso.GetField("Arrl_Sect") as Arrl_Sect;
			Assert.IsNotNull(nwt);
			string mod = nwt.ModifyValues(qso);
			Assert.AreEqual("NT", nwt.Value);
			Assert.AreEqual("\tArrl_Sect:" + Environment.NewLine +
			                "\t\tDeprecated section 'NWT' changed to 'NT'." +
			                Environment.NewLine, mod);
		}
	}
}
