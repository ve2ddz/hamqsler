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
using System.Collections.Generic;
using NUnit.Framework;
using hamqsler;

namespace hamqslerTest
{
	/// <summary>
	/// Tests for CommaDelimitedList class
	/// </summary>
	[TestFixture]
	public class DelimitedListTests
	{
		/// <summary>
		/// Tests that DelimitedList.Count returns correct value
		/// </summary>
		[Test]
		public void TestCount()
		{
			DelimitedList cdl = new DelimitedList(',', "item1,item2,item3,item4");
			Assert.AreEqual(4, cdl.Count);
		}
		
		/// <summary>
		/// Tests that a CommaDelimitedList with no entries returns a count of 0
		/// </summary>
		[Test]
		public void TestCount0()
		{
			DelimitedList cdl = new DelimitedList(',', string.Empty);
			Assert.AreEqual(0, cdl.Count);
		}
		
		/// <summary>
		/// Tests CommaDelimitedList.IsInList with values that are in list
		/// </summary>
		[Test]
		public void TestIsInList()
		{
			DelimitedList cdl = new DelimitedList(',', "item1,item2,item3,item4");
			Assert.IsTrue(cdl.IsInList("item1"));
			Assert.IsTrue(cdl.IsInList("item4"));
		}
		
		/// <summary>
		/// Tests CommaDelimitedList.IsInList with values that are in list
		/// </summary>
		[Test]
		public void TestIsInListWhiteSpace()
		{
			DelimitedList cdl = new DelimitedList(':', "item1 : item2:  item3:item4 ");
			Assert.IsTrue(cdl.IsInList("item1"));
			Assert.IsTrue(cdl.IsInList("item2"));
			Assert.IsTrue(cdl.IsInList("item3"));
			Assert.IsTrue(cdl.IsInList("item4"));
		}
		
		/// <summary>
		/// Tests CommaDelimitedList.IsInList with a value that is not in list
		/// </summary>
		[Test]
		public void TestIsInListNotInList()
		{
			DelimitedList cdl = new DelimitedList(',', "item1,item2,item3,item4");
			Assert.IsFalse(cdl.IsInList("item6"));
		}
		
		/// <summary>
		/// Tests that duplicate entries in the list are removed.
		/// </summary>
		[Test]
		public void TestDuplicateEntriesRemoved()
		{
			DelimitedList cdl = new DelimitedList(',', "item1,item2,item3,item2,item4");
			Assert.AreEqual(4, cdl.Count);
			Assert.IsTrue(cdl.IsInList("item1"));
			Assert.IsTrue(cdl.IsInList("item2"));
			Assert.IsTrue(cdl.IsInList("item3"));
			Assert.IsTrue(cdl.IsInList("item4"));
		}

		/// <summary>
		/// Tests that CommaDelimitedList.Items returns the list of all items.
		/// </summary>
		[Test]
		public void TestGetItems()
		{
			DelimitedList cdl = new DelimitedList(':', "item2:item4:item3:item1");
			HashSet<string> items = cdl.Items;
			Assert.AreEqual(4, items.Count);
			Assert.IsTrue(cdl.IsInList("item1"));
			Assert.IsTrue(cdl.IsInList("item2"));
			Assert.IsTrue(cdl.IsInList("item3"));
			Assert.IsTrue(cdl.IsInList("item4"));
		}
		
		/// <summary>
		/// Tests that CommaDelimitedList.ToString returns a command delimited string of values
		/// </summary>
		[Test]
		public void TestToString()
		{
			DelimitedList cdl = new DelimitedList(',', "item1,item2,item3");
			Assert.AreEqual("item1,item2,item3", cdl.ToString());
		}
		
		/// <summary>
		/// Tests that CommaDelimitedList.ToString returns string.Empty when there are no
		/// items in the list
		/// </summary>
		[Test]
		public void TestToStringEmpty()
		{
			DelimitedList cdl = new DelimitedList(',', string.Empty);
			Assert.AreEqual(string.Empty, cdl.ToString());
		}
		
	}
}
