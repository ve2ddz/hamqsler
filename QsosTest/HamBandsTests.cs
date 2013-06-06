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
	public class HamBandsTests
	{
		// test getHamBand for valid band
		[Test()]
		public void TestGetHamBandValidBand ()
		{
			HamBand band = HamBands.getHamBand("2190m");
			Assert.AreEqual(band.Band, "2190m");
			Assert.AreEqual(band.LowerEdge, 0.136F);
			Assert.AreEqual(band.UpperEdge, 0.137F);
		}
		
		// test getHamBand for invalid band
		[Test, ExpectedException(typeof(QsoException))]
		public void TestGetHamBandInvalidBand()
		{
			HamBands.getHamBand("316m");
		}
		
		// test getHamBand with midband frequency
		[Test]
		public void TestGetHamBandMidBandFreq()
		{
			HamBand band = HamBands.getHamBand(28.515F);
			Assert.AreEqual(band.Band, "10m");
		}
		
		// test getHamBand with loweredge frequency
		[Test]
		public void TestGetHamBandLowerEdgeFreq()
		{
			HamBand band = HamBands.getHamBand(28.0F);
			Assert.AreEqual(band.Band, "10m");
		}
		
		// test getHamBand with upperedge frequency
		[Test]
		public void TestGetHamBandUpperEdgeFreq()
		{
			HamBand band = HamBands.getHamBand(29.7F);
			Assert.AreEqual(band.Band, "10m");
		}
		
		// test getHamBand with non-band frequency
		[Test, ExpectedException(typeof(QsoException))]
		public void TestGetHamBandNonBandFreq()
		{
			HamBands.getHamBand(1.5F);
		}
	}
}

