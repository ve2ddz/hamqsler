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
	/// Tests for DelimitedListEnumeration class
	/// </summary>
	[TestFixture]
	public class DelimitedListEnumerationTests
	{
		// test constructor and Count accessor
		[Test]
		public void TestCount()
		{
			string list = "item1,item4,item3";
			string[] enumeration = {"item1", "item2", "item3", "item4"};
			DelimitedListEnumeration dLE = new DelimitedListEnumeration(',', list, enumeration);
			Assert.AreEqual(3, dLE.Count);
		}
		
		// test constructor and Count accessor
		[Test]
		public void TestCount2()
		{
			string list = "ON:NT:EB:EPA";
			string err = string.Empty;
		    // get the hamqsler assembly
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            // get a stream for the AdifEnumerations.xml file
            // TODO: This is currently an embedded resource in the assembly, but needs to be moved to AppData
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
             // load in the xml file
			AdifEnumerations aEnums = new AdifEnumerations(str);
			DelimitedListEnumeration dLE = new DelimitedListEnumeration(':', list, "Arrl_Section", aEnums);
			Assert.AreEqual(4, dLE.Count);
		}
		
		// test IsInList method
		[Test]
		public void TestIsInList()
		{
			string list = "item1,item4,item3";
			string[] enumeration = {"item1", "item2", "item3", "item4"};
			DelimitedListEnumeration dLE = new DelimitedListEnumeration(',', list, enumeration);
			Assert.IsTrue(dLE.IsInList("item1"));
		}
		
		// test IsInList method
		[Test]
		public void TestIsInList1()
		{
			string list = "item1,item4,item3";
			string[] enumeration = {"item1", "item2", "item3", "item4"};
			DelimitedListEnumeration dLE = new DelimitedListEnumeration(',', list, enumeration);
			Assert.IsTrue(dLE.IsInList("item3"));			
		}
		
		// test IsInList with item not in list
		[Test]
		public void TestIsInListFalse()
		{
			string list = "item1,item4,item3";
			string[] enumeration = {"item1", "item2", "item3", "item4"};
			DelimitedListEnumeration dLE = new DelimitedListEnumeration(',', list, enumeration);
			Assert.IsFalse(dLE.IsInList("item6"));						
		}
		
		// test Validate method
		[Test]
		public void TestValidate()
		{
			string list = "item1,item4,item3";
			string[] enumeration = {"item1", "item2", "item3", "item4"};
			DelimitedListEnumeration dLE = new DelimitedListEnumeration(',', list, enumeration);
			string err = string.Empty;
			Assert.IsTrue(dLE.Validate(out err));
		}
		
		// test Validate method with item not in enumeration
		[Test]
		public void TestValidateInvalidValue()
		{
			string list = "ON:NT:EOR:EPA";
			string err = string.Empty;
		    // get the hamqsler assembly
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            // get a stream for the AdifEnumerations.xml file
            // TODO: This is currently an embedded resource in the assembly, but needs to be moved to AppData
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
             // load in the xml file
			AdifEnumerations aEnums = new AdifEnumerations(str);
			DelimitedListEnumeration dLE = new DelimitedListEnumeration(':', list, "Arrl_Section", aEnums);
			Assert.IsFalse(dLE.Validate(out err));
			Assert.AreEqual("This QSO Field is of type enumeration. The value 'EOR' was not found in enumeration.",
			                err);
		}
		
		// test ToAdifString method
		[Test]
		public void TestToAdifString()
		{
			string list = "ON:NT:OR:EPA";
			string err = string.Empty;
		    // get the hamqsler assembly
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            // get a stream for the AdifEnumerations.xml file
            // TODO: This is currently an embedded resource in the assembly, but needs to be moved to AppData
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
             // load in the xml file
			AdifEnumerations aEnums = new AdifEnumerations(str);
			DelimitedListEnumeration dLE = new DelimitedListEnumeration(':', list, "Arrl_Section", aEnums);
			Assert.AreEqual("<DelimitedListEnumeration:12>ON:NT:OR:EPA", dLE.ToAdifString());
		}
		
		// test Validate for a single item
		[Test]
		public void TestValidateSingleItem()
		{
			string list = "ON:NT:OR:EPA";
			string err = string.Empty;
		    // get the hamqsler assembly
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            // get a stream for the AdifEnumerations.xml file
            // TODO: This is currently an embedded resource in the assembly, but needs to be moved to AppData
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
             // load in the xml file
			AdifEnumerations aEnums = new AdifEnumerations(str);
			DelimitedListEnumeration dLE = new DelimitedListEnumeration(':', list, "Arrl_Section", aEnums);
			Assert.IsTrue(dLE.Validate("NT", out err));
		}
		
		// test Validate for non-included item
		[Test]
		public void TestValidateSingleItemNotPresent()
		{
			string list = "ON:NT:OR:EPA";
			string err = string.Empty;
		    // get the hamqsler assembly
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
            // get a stream for the AdifEnumerations.xml file
            // TODO: This is currently an embedded resource in the assembly, but needs to be moved to AppData
            Stream str = assembly.GetManifestResourceStream("hamqsler.AdifEnumerations.xml");
             // load in the xml file
			AdifEnumerations aEnums = new AdifEnumerations(str);
			DelimitedListEnumeration dLE = new DelimitedListEnumeration(':', list, "Arrl_Section", aEnums);
			Assert.IsFalse(dLE.Validate("XPS", out err));
			Assert.AreEqual("This QSO Field is of type enumeration. The value 'XPS' " +
			                    "was not found in enumeration.", err);
		}
		
	}
}
