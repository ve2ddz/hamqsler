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
	// tests for QsoEualityComparer class
	[TestFixture]
	public class QsoEqualityComparerTests
	{
		string errorString = string.Empty;
		// TestFixtureSetup
		[TestFixtureSetUp]
		public void TestSepup()
		{
			App.AdifEnums.LoadDocument();
		}
		
		// test Equals with equal Qsos
		[Test]
		public void TestEqualsEqualQsos()
		{
			Qso2 q1 = new Qso2("<Call:6>VA3JNO<Mode:3>SSB<Freq:5>7.235<qso_date:8>20130615<time_on:6>124316",
			                    App.AdifEnums, ref errorString);
			Qso2 q2 = new Qso2("<Call:6>VA3JNO<Mode:3>SSB<Freq:5>7.235<qso_date:8>20130615<time_on:6>124316",
			                    App.AdifEnums, ref errorString);
			QsoEqualityComparer comparer = new QsoEqualityComparer();
			Assert.IsTrue(comparer.Equals(q1, q2));
		}

		
		// test Equals with unequal Qsos
		[Test]
		public void TestEqualsUnEqualQsos()
		{
			Qso2 q1 = new Qso2("<Call:6>VA3JNO<Mode:3>SSB<Freq:5>7.235<qso_date:8>20130615<time_on:6>124316",
			                    App.AdifEnums, ref errorString);
			Qso2 q2 = new Qso2("<Call:6>VA3JNO<Mode:3>SSB<Freq:5>7.235<qso_date:8>20130622<time_on:6>124316",
			                    App.AdifEnums, ref errorString);
			QsoEqualityComparer comparer = new QsoEqualityComparer();
			Assert.IsFalse(comparer.Equals(q1, q2));
		}
	}
}
