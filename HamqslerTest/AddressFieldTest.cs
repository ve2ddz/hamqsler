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
	/// Tests for Address ADIF field object
	/// </summary>
	[TestFixture]
	public class AddressFieldTest
	{
		// test Address.Name accessor
		[Test]
		public void TestName()
		{
			string address = "My Name" + Environment.NewLine +
			                 "2124 Any Street" + Environment.NewLine +
	                         "AnyTown Code" + Environment.NewLine +
	                         "Some Country";
			Address addr = new Address(address);
			Assert.AreEqual("Address", addr.Name);
		}
		
		// test Address.Value accessor
		[Test]
		public void TestValue()
		{
			string address = "My Name" + Environment.NewLine +
			                 "2124 Any Street" + Environment.NewLine +
			                 "AnyTown Code" + Environment.NewLine +
			                 "Some Country";
			Address addr = new Address(address);
			Assert.AreEqual(address, addr.Value);
		}
		
		// test Address.ToAdifString returns correct value
		[Test]
		public void TestToAdifString()
		{
			string address = "My Name" + Environment.NewLine +
			                 "2124 Any Street" + Environment.NewLine +
			                 "AnyTown Code" + Environment.NewLine +
			                 "Some Country";
			Address addr = new Address(address);
			Assert.AreEqual("<Address:52>My Name" + Environment.NewLine +
			                 "2124 Any Street" + Environment.NewLine +
			                 "AnyTown Code" + Environment.NewLine +
			                 "Some Country",
			                 addr.ToAdifString());
		}
		
		// test Validate
		[Test]
		public void TestValidate()
		{
			string address = "My Name" + Environment.NewLine +
			                 "2124 Any Street" + Environment.NewLine +
			                 "AnyTown Code" + Environment.NewLine +
			                 "Some Country";
			Address addr = new Address(address);
			string err = string.Empty;
			string modStr = string.Empty;
			Assert.IsTrue(addr.Validate(out err, out modStr));
			Assert.AreEqual(null, err);
			Assert.AreEqual(null, modStr);
		}
	}
}
