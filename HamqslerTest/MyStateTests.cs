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
	// tests for My_State class
	[TestFixture]
	public class MyStateTests
	{
		// test ToAdifString
		[Test]
		public void TestToAdifString()
		{
			My_State state = new My_State("ON");
			Assert.AreEqual("<My_State:2>ON", state.ToAdifString());
		}

		// test Validate - any value is acceptable at this time
		[Test]
		public void TestValidate()
		{
			My_State state = new My_State("ON");
			string err = string.Empty;
			Assert.IsTrue(state.Validate(out err));
			Assert.AreEqual(null, err);
		}
	}
}
