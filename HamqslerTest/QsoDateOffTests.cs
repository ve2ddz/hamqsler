﻿/*
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
	/// <summary>
	///  tests for Qso_Date_Off
	/// </summary>
	[TestFixture]
	public class QsoDateOffTests
	{
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			Qso_Date_Off date = new Qso_Date_Off("20130920");
			Assert.AreEqual("<Qso_Date_Off:8>20130920", date.ToAdifString());
		}
	}
}
