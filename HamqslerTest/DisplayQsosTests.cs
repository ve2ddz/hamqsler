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
	/// <summary>
	/// tests for DisplayQsos class
	/// </summary>
	[TestFixture]
	public class DisplayQsosTests
	{
		string errorString = string.Empty;
		
		// test fixture setup
		// test Add with Qsos2 object
		[Test]
		public void TestAddQso()
		{
			Qsos2 qsos = new Qsos2();
			string err = string.Empty;
			Qso2 qso = new Qso2("<Mode:3>SSB<Band:3>40m<qso_date:8>20130615<time_on:6>124316",
			                    App.AdifEnums, ref err, qsos);
			QsoWithInclude qwi = new QsoWithInclude(qso);
			DisplayQsos dQsos = new DisplayQsos();
			Assert.IsFalse(dQsos.IsDirty);
			Assert.IsFalse(dQsos.NeedsSorting);
			dQsos.AddQso(qwi);
			Assert.AreEqual(1, dQsos.Count);
			Assert.IsTrue(dQsos.IsDirty);
			Assert.IsTrue(dQsos.NeedsSorting);
			Assert.AreNotEqual(qsos, dQsos[0].Qso.Qsos);
		}
		
		// test Add with no Qsos2 object
		[Test]
		public void TestAddQsoNoQsos2()
		{
			string err = string.Empty;
			Qso2 qso = new Qso2("<Mode:3>SSB<Band:3>40m<qso_date:8>20130615<time_on:6>124316",
			                    App.AdifEnums, ref err);
			QsoWithInclude qwi = new QsoWithInclude(qso);
			DisplayQsos dQsos = new DisplayQsos();
			Assert.IsFalse(dQsos.IsDirty);
			Assert.IsFalse(dQsos.NeedsSorting);
			dQsos.AddQso(qwi);
			Assert.AreEqual(1, dQsos.Count);
			Assert.IsTrue(dQsos.IsDirty);
			Assert.IsTrue(dQsos.NeedsSorting);
		}

		// test Clear
		[Test]
		public void TestClear()
		{
			string err = string.Empty;
			Qso2 qso = new Qso2("<Mode:3>SSB<Band:3>40m<qso_date:8>20130615<time_on:6>124316",
			                    App.AdifEnums, ref err);
			QsoWithInclude qwi = new QsoWithInclude(qso);
			DisplayQsos dQsos = new DisplayQsos();
			dQsos.AddQso(qwi);
			Assert.AreEqual(1, dQsos.Count);
			Assert.IsTrue(dQsos.IsDirty);
			Assert.IsTrue(dQsos.NeedsSorting);
			dQsos.Clear();
			Assert.AreEqual(0, dQsos.Count);
			Assert.IsFalse(dQsos.IsDirty);
			Assert.IsFalse(dQsos.NeedsSorting);
		}
	}
}
