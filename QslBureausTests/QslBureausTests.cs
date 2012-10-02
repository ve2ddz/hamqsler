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
using QslBureaus;
namespace QslBureausTests
{
	[TestFixture()]
	public class QslBureausTests
	{
		// test Version accessor
		[Test()]
		public void TestVersion ()
		{
			Assert.AreEqual("1.1.2", QslBureaus.QslBureaus.Version);
		}
		
		// test CreationDate accessor
		[Test]
		public void TestCreationDate()
		{
			Assert.AreEqual("2011-12-28", QslBureaus.QslBureaus.CreationDate);
		}
		
		// test Bureau with valid bureau
		[Test]
		public void TestBureau()
		{
			Assert.AreEqual("VE10", QslBureaus.QslBureaus.Bureau("VA1CCP"));
		}
		
		// test Bureau with no bureau
		[Test]
		public void TestBureauNoBureau()
		{
			Assert.AreEqual("ZZZZ", QslBureaus.QslBureaus.Bureau("1A0DP"));
		}
		
		// test Bureau for KG4A - should be W4A0
		[Test]
		public void TestBureauKG4A()
		{
			Assert.AreEqual("W4A0", QslBureaus.QslBureaus.Bureau("KG4A"));
		}
		
		// test Bureau for KG4AXY - should be W4A0
		[Test]
		public void TestBureauKG4AXY()
		{
			Assert.AreEqual("W4A0", QslBureaus.QslBureaus.Bureau("KG4AXY"));
		}
		
		// test Bureau for KG4AB - should be W9B0
		[Test]
		public void TestBureauKG4AB()
		{
			Assert.AreEqual("W9B0", QslBureaus.QslBureaus.Bureau("KG4AB"));
		}
		
		// test Bureau for KP2ADH - should be W9C0
		[Test]
		public void TestBureauKP2ADH()
		{
			Assert.AreEqual("W9C0", QslBureaus.QslBureaus.Bureau("KP2ADH"));
		}
		
		// test Bureau for NP3D - should be W9D0
		[Test]
		public void TestBureauNP3D()
		{
			Assert.AreEqual("W9D0", QslBureaus.QslBureaus.Bureau("NP3D"));
		}
		
		// test Bureau for WP4DD - should be W9D0
		[Test]
		public void TestBureauWP4DD()
		{
			Assert.AreEqual("W9D0", QslBureaus.QslBureaus.Bureau("WP4DD"));
		}
		
		// test Bureau for AH2XC - should be W9H2
		[Test]
		public void TestBureauAH2XC()
		{
			Assert.AreEqual("W9H2", QslBureaus.QslBureaus.Bureau("AH2XC"));
		}
		
		// test Bureau for KH3XC - should be W9H3
		[Test]
		public void TestBureauKH3XC()
		{
			Assert.AreEqual("W9H3", QslBureaus.QslBureaus.Bureau("KH3XC"));
		}
		
		// test Bureau for NH6XC - should be W9H6
		[Test]
		public void TestBureauNH6XC()
		{
			Assert.AreEqual("W9H6", QslBureaus.QslBureaus.Bureau("NH6XC"));
		}
		
		// test Bureau for WH7XC - should be W9H6
		[Test]
		public void TestBureauWH7XC()
		{
			Assert.AreEqual("W9H6", QslBureaus.QslBureaus.Bureau("WH7XC"));
		}
		
		// test Bureau for AH8XC - should be W9H8
		[Test]
		public void TestBureauAH8XC()
		{
			Assert.AreEqual("W9H8", QslBureaus.QslBureaus.Bureau("AH8XC"));
		}
		
		// test Bureau for AL0PM - should be W9L0
		[Test]
		public void TestBureauAL0PM()
		{
			Assert.AreEqual("W9L0", QslBureaus.QslBureaus.Bureau("AL0PM"));
		}
		
		// test Bureau for NL7C - should be W9L0
		[Test]
		public void TestBureauNL7C()
		{
			Assert.AreEqual("W9L0", QslBureaus.QslBureaus.Bureau("NL7C"));
		}
		
		// test Bureau for KB4ZNS - should be W4A0
		[Test]
		public void TestBureauKB4ZNS()
		{
			Assert.AreEqual("W4A0", QslBureaus.QslBureaus.Bureau("KB4ZNS"));
		}
		
		// test Bureau for W4BB - should be W400
		[Test]
		public void TestBureauW4BB()
		{
			Assert.AreEqual("W400", QslBureaus.QslBureaus.Bureau("W4BB"));
		}
		
		// test Bureau for R3AXB - should be R000
		[Test]
		public void testBureauR3AXB()
		{
			Assert.AreEqual("R000", QslBureaus.QslBureaus.Bureau("R3AXB"));
		}
		
		// test Bureau for U1AQ - should be R000
		[Test]
		public void testBureauU1AQ()
		{
			Assert.AreEqual("R000", QslBureaus.QslBureaus.Bureau("U1AQ"));
		}
		
		// test Bureau for UT2AX - should be UR00
		[Test]
		public void testBureauUT2AX()
		{
			Assert.AreEqual("UR00", QslBureaus.QslBureaus.Bureau("UT2AX"));
		}
		
		// test Bureau for TU6XV - should be NoBureau
		[Test]
		public void testBureauTU6XV()
		{
			Assert.AreEqual("ZZZZ", QslBureaus.QslBureaus.Bureau("TU6XV"));
		}
		
		// test Bureau for ZA3MM - should be NoBureau
		[Test]
		public void testBureauZA3MM()
		{
			Assert.AreEqual("ZZZZ", QslBureaus.QslBureaus.Bureau("ZA3MM"));
		}
			
	}
}

