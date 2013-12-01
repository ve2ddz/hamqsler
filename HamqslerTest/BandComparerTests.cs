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
	// tests BandComparer class
	[TestFixture]
	public class BandComparerTests
	{
		// TestFixtureSetup
		[TestFixtureSetUp]
		public void TestSepup()
		{
			App.AdifEnums.LoadDocument();
		}
		
		// test Compare with band1 < band2
		[Test]
		public void TestCompare1Less2()
		{
			string band1 = "10m";
			string band2 = "6m";
			BandComparer bc = new BandComparer();
			Assert.IsTrue(bc.Compare(band1, band2) < 0);
		}

		// test Compare with band1 > band2
		[Test]
		public void TestCompare1Greater2()
		{
			string band1 = "6m";
			string band2 = "20m";
			BandComparer bc = new BandComparer();
			Assert.IsTrue(bc.Compare(band1, band2) > 0);
		}

		// test Compare with band1 == band2
		[Test]
		public void TestCompareEqual()
		{
			string band1 = "10m";
			string band2 = "10m";
			BandComparer bc = new BandComparer();
			Assert.IsTrue(bc.Compare(band1, band2) == 0);
		}
	}
}
