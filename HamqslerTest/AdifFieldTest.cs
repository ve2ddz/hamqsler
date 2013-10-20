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
	/// <summary>
	/// Description of AdifFieldTest.
	/// </summary>
	[TestFixture]
	public class AdifFieldTest
	{
		// test Name accessor
		[Test]
		public void TestGetName()
		{
			AdifField field = new AdifField();
			Assert.AreEqual("AdifField", field.Name);
		}
		
		// test IsValid
		[Test]
		public void TestIsValid()
		{
			string err = string.Empty;
			string modStr = string.Empty;
			AdifField field = new AdifField();
			Assert.IsTrue(field.Validate(out err, out modStr));
			Assert.IsNull(err);
			Assert.IsNull(modStr);
		}
		
		// test ToAdifField with no datatype
		[Test]
		public void TestToAdifFieldNoDataType()
		{
			AdifField field = new AdifField();
			field.Value = "Fred";
			Assert.AreEqual("<AdifField:4>Fred", field.ToAdifString());
		}
	}
}
