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
using NUnit.Framework;
using hamqsler;

namespace hamqslerTest
{
	// Tests for DispQso class
	[TestFixture]
	public class DispQsoTests
	{
		Qso2 qso;
		// tests setup
		[SetUp]
		public void Setup()
		{
			App.AdifEnums.LoadDocument();
			string err = string.Empty;
			qso = new Qso2(string.Empty, App.AdifEnums, ref err, null);
		}
		
		// test QSL status
		[Test, Sequential]
		public void TestQslStatus(
			[Values("Y", "Y", "Y", "Y",
			       	"N", "N", "N", "N",
			        "R", "R", "R", "R",
			        "Q", "Q", "Q", "Q",
			        "I", "I", "I", "I")] string sent,
			[Values("Y", "N", "R", "I",
			        "Y", "N", "R", "I",
			        "Y", "N", "R", "I",
			        "Y", "N", "R", "I",
			        "Y", "N", "R", "I")] string rcvd,
			[Values("", "", "", "",
			        "", "", "", "",
			        "Tnx", "Pse", "Pse", "",
			        "Tnx", "Pse", "Pse", "",
			        "", "", "", "")] string qsl)
		{
			qso["Qsl_Sent"] = sent;
			qso["Qsl_Rcvd"] = rcvd;
			QsoWithInclude qwi = new QsoWithInclude(qso);
			DispQso dQso = new DispQso(qwi);
			Assert.AreEqual(qsl, dQso.Qsl);
		}
	}
}
