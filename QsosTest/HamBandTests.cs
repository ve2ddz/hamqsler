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
	public class HamBandTests
	{
		// test IsThisBand
		[Test()]
		public void TestIsThisBand ()
		{
			HamBand band = HamBands.getHamBand("160m");
			Assert.IsTrue(band.IsThisBand(1.8F));
			Assert.IsTrue(band.IsThisBand(2.0F));
			Assert.IsTrue(band.IsThisBand(1.9F));
			Assert.IsFalse(band.IsThisBand(1.799999F));
			Assert.IsFalse(band.IsThisBand(2.00001F));
		}
	}
}

