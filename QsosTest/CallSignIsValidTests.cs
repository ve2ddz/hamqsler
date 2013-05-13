// //  
// //  Author:
// //       Jim <jim@va3hj.ca>
// // 
// //  Copyright (c) 2011 VA3HJ Software
// // 
// //  This program is free software: you can redistribute it and/or modify
// //  it under the terms of the GNU General Public License as published by
// //  the Free Software Foundation, either version 3 of the License, or
// //  (at your option) any later version.
// // 
// //  This program is distributed in the hope that it will be useful,
// //  but WITHOUT ANY WARRANTY; without even the implied warranty of
// //  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// //  GNU General Public License for more details.
// // 
// //  You should have received a copy of the GNU General Public License
// //  along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 
using System;
using NUnit.Framework;
using Qsos;
namespace QsosTests
{
	[TestFixture()]
	public class CallSignIsValidTests
	{
		// test IsValid with null callsign
		[Test()]
		public void TestIsValidNull ()
		{
			Assert.IsFalse(CallSign.IsValid(null));
		}
	
		// test IsValid with empty callsign
		[Test]
		public void TestIsValidEmtpy()
		{
			Assert.IsFalse(CallSign.IsValid(string.Empty));
		}
		
		// test IsValid with some valid callsigns
		[Test]
		public void TestIsValidVA3HJ()
		{
			Assert.IsTrue(CallSign.IsValid("VA3HJ"));
		}
		
		[Test]
		public void TestIsValid9A1TX()
		{
			Assert.IsTrue(CallSign.IsValid("9A1TX"));
		}
		
		[Test]
		public void TestIsValid9AX2000TEST()
		{
			Assert.IsTrue(CallSign.IsValid("9AX2000TEST"));
		}
		
		[Test]
		public void TestIsValidW4XN()
		{
			Assert.IsTrue(CallSign.IsValid("W4XN"));
		}
		
		public void TestIsValidWX4N()
		{
			Assert.IsTrue(CallSign.IsValid("WX4N"));
		}
		
		public void TestIsValidVO500GM()
		{
			Assert.IsTrue(CallSign.IsValid("VO500GM"));
		}
		
		public void TestIsValid9A2BUNCH()
		{
			Assert.IsTrue(CallSign.IsValid("9A2BUNCH"));
		}
		
		public void TestIsValidAX3GAMES()
		{
			Assert.IsTrue(CallSign.IsValid("AX3GAMES"));
		}
		
		// test IsValid with invalid calls
		[Test]
		public void TestIsValidFalse91A66F()
		{
			Assert.IsFalse(CallSign.IsValid("91A66F"));
		}
		
		[Test]
		public void TestIsValidFalsePJ2VA3HJ()
		{
			Assert.IsFalse(CallSign.IsValid("PJ2/VA3HJ"));
		}
		
		[Test]
		public void TestIsValidFalseVA3HJP()
		{
			Assert.IsFalse(CallSign.IsValid("VA3HJ/P"));
		}
		
		[Test]
		public void TestIsValidFalse9A20000AH()
		{
			Assert.IsFalse(CallSign.IsValid("9A20000AH"));
		}
		
		[Test]
		public void TestIsValidFalseWPE6XYH()
		{
			Assert.IsFalse(CallSign.IsValid("WPE6XYH"));
		}
		
		[Test]
		public void TestIsValidFalseW40001XT()
		{
			Assert.IsFalse(CallSign.IsValid("W40001XT"));
		}
		
		// test IsValid with valid non-standard callsign
		[Test]
		public void TestIsValidTrueBV100()
		{
			Assert.IsTrue(CallSign.IsValid("BV100"));
		}
		
		// test IsValid with valid non-standard callsign LM9L40Y
		[Test]
		public void TestIsValidTrueLM9L40Y()
		{
			Assert.IsTrue(CallSign.IsValid("LM9L40Y"));
		}
		
		// test IsValid with invalid callsign LM9L40L
		[Test]
		public void TestIsValidFalseLM9L40L()
		{
			Assert.IsFalse(CallSign.IsValid("LM9L40L"));
		}
		
		// test IsValid with non-standard callsign VI6ARG30
		[Test]
		public void TestIsValidTrueVI6ARG30()
		{
			Assert.IsTrue(CallSign.IsValid("VI6ARG30"));
		}

		// test IsValid with non-standard callsign XM31812
		[Test]
		public void TestIsValidTrueXM31812()
		{
			Assert.IsTrue(CallSign.IsValid("XM31812"));
		}

		// test IsValid with non-standard callsign TE1856
		[Test]
		public void TestIsValidTrueTE1856()
		{
			Assert.IsTrue(CallSign.IsValid("TE1856"));
		}

		// test IsValid with non-standard callsign 
		[Test]
		public void TestIsValidTrue8J2KSG7X()
		{
			Assert.IsTrue(CallSign.IsValid("8J2KSG7X"));
		}

		// test IsValid with non-standard callsign ZW1CCOM54
		[Test]
		public void TestIsValidTrueZW1CCOM54()
		{
			Assert.IsTrue(CallSign.IsValid("ZW1CCOM54"));
		}
	}
}

