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
	// tests for Station_Callsign class
	[TestFixture]
	public class StationCallsignTests
	{
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			Station_Callsign oCall = new Station_Callsign("VA3HJ");
			Assert.AreEqual("<Station_Callsign:5>VA3HJ", oCall.ToAdifString());
		}
		
		// test Validate with valid callsign
		[Test]
		public void TestValidateValidCall()
		{
			Station_Callsign oCall = new Station_Callsign("VA3HJ");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(oCall.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with invalid callsign
		[Test]
		public void TestValidateInvalidCall()
		{
			Station_Callsign oCall = new Station_Callsign("VAHJ");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(oCall.Validate(out err, out modStr));
			Assert.AreEqual("Callsign 'VAHJ' is invalid.", err);
			Assert.IsNull(modStr);
		}
		
		// test Validate with compound callsign
		[Test]
		public void TestValidateCompoundCall()
		{
			Station_Callsign oCall = new Station_Callsign("VA3HJ/W8");
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsFalse(oCall.Validate(out err, out modStr));
			Assert.AreEqual("Callsign 'VA3HJ/W8' contains modifiers.", err);
			Assert.IsNull(modStr);
		}
	}
}
