// //  
// //  Author:
// //       Jim <jimorcheson@gmail.com>
// // 
// //  Copyright (c) 2011 VA3HJ Software
// //  Copyright (c) 2013 Jim Orcheson
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
	public class CallSignTests
	{
		
		// test FullCall and Call with valid callsigns
		[Test()]
		public void TestCallSign ()
		{
			CallSign c = new CallSign("VA3HJ");
			Assert.AreEqual(c.FullCall, "VA3HJ");
			Assert.AreEqual(c.Call, "VA3HJ");
		}
		
		[Test]
		public void TestCallSign2()
		{
			CallSign c = new CallSign("VA3HJ/KL7");
			Assert.AreEqual(c.FullCall, "VA3HJ/KL7");
			Assert.AreEqual(c.Call, "VA3HJ");
		}
		
		[Test]
		public void TestCallSign3()
		{
			CallSign c = new CallSign("PJ2/VA3HJ");
			Assert.AreEqual(c.FullCall, "PJ2/VA3HJ");
			Assert.AreEqual(c.Call, "VA3HJ");
		}
		
		[Test]
		public void TestCallSign4()
		{
			CallSign c = new CallSign("PJ2/VA3HJ/M");
			Assert.AreEqual(c.FullCall, "PJ2/VA3HJ/M");
			Assert.AreEqual(c.Call, "VA3HJ");
		}
		
		[Test]
		public void TestCallSign5()
		{
			CallSign c = new CallSign("KH7K/VA3HJ");
			Assert.AreEqual(c.FullCall, "KH7K/VA3HJ");
			Assert.AreEqual(c.Call, "VA3HJ");
		}
		
		[Test]
		public void TestCallSign6()
		{
			CallSign c = new CallSign("K5N/KH5K");
			Assert.AreEqual(c.FullCall, "K5N/KH5K");
			Assert.AreEqual(c.Call, "K5N");
		}
		
		[Test]
		public void TestCallSign7()
		{
			CallSign c = new CallSign("AX3GAMES");
			Assert.AreEqual(c.FullCall, "AX3GAMES");
			Assert.AreEqual(c.Call, "AX3GAMES");
		}
		
		// test ctor with null callsign
		[Test, ExpectedException(typeof(QsoException))]
		public void TestCTorNullCall()
		{
			new CallSign(null);
			Assert.Fail("TestCTorNullCall test failed");
		}
		
		// test ctor with empty callsign
		[Test, ExpectedException(typeof(QsoException))]
		public void TestCTorEmptyCall()
		{
			new CallSign(string.Empty);
			Assert.Fail("TestCTorEmptyCall test failed");
		}
		
		// test ctor with call containing only prefixes
		[Test, ExpectedException(typeof(QsoException))]
		public void TestCTorBadCall()
		{
			new CallSign("KH7K/PY0F");
			Assert.Fail("TestCTorBadCall test failed");
		}
		
		// test ctor for special callsign BV100
		[Test]
		public void TestCTorBV100()
		{
			CallSign c = new CallSign("BV100");
			Assert.AreEqual("BV100", c.Call);
		}
		
		// test ctor for special callsign LM9L40Y
		[Test]
		public void TestCTorLM9L40Y()
		{
			CallSign c = new CallSign("LM9L40Y");
			Assert.AreEqual("LM9L40Y", c.Call);
		}
		
		// test ctor for special callsign OEH20
		[Test]
		public void TestCTorOEH20()
		{
			CallSign c = new CallSign("OEH20");
			Assert.AreEqual("OEH20", c.Call);
		}
		
		// test ctor for special callsign SX1912
		[Test]
		public void TestCTorSX1912()
		{
			CallSign c = new CallSign("SX1912");
			Assert.AreEqual("SX1912", c.Call);
		}
		
		// test ctor for special callsign XM31812
		[Test]
		public void TestCTorXM31812()
		{
			CallSign c = new CallSign("XM31812");
			Assert.AreEqual("XM31812", c.Call);
		}
		
		// test ctor for special callsign TE1856
		[Test]
		public void TestCTorTE1856()
		{
			CallSign c = new CallSign("TE1856");
			Assert.AreEqual("TE1856", c.Call);
		}
		
		// test ctor for special callsign 8J2KSG7X
		[Test]
		public void TestCTor8J2KSG7X()
		{
			CallSign c = new CallSign("8J2KSG7X");
			Assert.AreEqual("8J2KSG7X", c.Call);
		}
		
		// test ctor for special callsign ZW1CCOM54
		[Test]
		public void TestCTorZW1CCOM54()
		{
			CallSign c = new CallSign("ZW1CCOM54");
			Assert.AreEqual("ZW1CCOM54", c.Call);
		}
		
		
		// test ctor for special callsign VX31763
		[Test]
		public void TestCTorVX31763()
		{
			CallSign c = new CallSign("VX31763");
			Assert.AreEqual("VX31763", c.Call);
		}
		
		// test ctor for special callsign XV2V40J
		[Test]
		public void TestCTorXV2V40J()
		{
			CallSign c = new CallSign("XV2V40J");
			Assert.AreEqual("XV2V40J", c.Call);
		}
	}
}

