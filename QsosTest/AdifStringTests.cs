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
using System;
using NUnit.Framework;
using Qsos;
namespace QsosTests
{
	[TestFixture()]
	public class AdifStringTests
	{
		// test AdifString construction with no <eor>
		[Test(), ExpectedException( typeof(QsoException) )]
		public void TestAdifStringNoEor ()
		{
			new AdifString("<call:5>VA3HJ <qso_date:8>20080722 <time_on:6>061219 <mode:2>CW" +
                    "<band:4>160m<qsl_rcvd:1>n<qsl_sent:1>Y");
		}
		
		// test getFieldName with valid input
		[Test]
		public void TestGetFieldNameValidInput ()
		{
			Assert.AreEqual(AdifString.getFieldName("<eor>"), "<eor>" );
		}
		
		// test getFieldName with missing '<'
		[Test]
		public void TestGetFieldNameMissingStart()
		{
			Assert.IsNull(AdifString.getFieldName("eor>"));
		}
		
		// test getFieldName with missing '>'
		[Test]
		public void TestGetFieldNameMissingEnd()
		{
			Assert.IsNull(AdifString.getFieldName("<eor"));
		}
		
		// test fieldLength with valid length
		[Test]
		public void TestFieldLengthValidLength()
		{
			Assert.AreEqual(AdifString.fieldLength("<call:5>"), 5);
		}
		
		// test fieldLength with missing '>'
		[Test]
		public void TestFieldLengthMissingEnd()
		{
			Assert.AreEqual(AdifString.fieldLength("<call:5"), -1);
		}
		
		// test fieldLength with missing ':' in input
		[Test]
		public void TestFieldLengthMissingSize()
		{
			Assert.AreEqual(AdifString.fieldLength("<call5>"), 0);
		}
		
		// test fieldLength with no length specified
		[Test, ExpectedException(typeof(FormatException))]
		public void TestFieldLengthMissingSize2()
		{
			AdifString.fieldLength("<call:>");
		}
		
		// test fieldLength with text length
		[Test, ExpectedException(typeof (FormatException))]
		public void TestFieldLengthTextLength()
		{
			AdifString.fieldLength("<call:F>");
		}
		
		// test fieldLength with double length
		[Test, ExpectedException(typeof(FormatException))]
		public void TestFieldLengthDoubleLength()
		{
			AdifString.fieldLength("<call:3.5>");
		}
		
		// test fieldLength with too large size
		[Test, ExpectedException(typeof(OverflowException))]
		public void TesstFieldLengthTooLarge()
		{
			AdifString.fieldLength("<call:999999999999>");
		}
		
		// test fieldLength with null field
		[Test, ExpectedException(typeof(QsoException))]
		public void TestFieldLengthNullField()
		{
			AdifString.fieldLength(null);
		}
		
		// test moreField with valid ADIF string
		[Test]
		public void TestMoreFieldsValid()
		{
			AdifString aS = new AdifString("<call:5>VA3HJ <eor>");
			Assert.IsTrue(aS.moreFields());
		}
		
		// test moreFields with bad field in ADIF string
		[Test]
		public void TestMoreFieldsBadField()
		{
			AdifString aS = new AdifString("<call:>VA3HJ <eor>");
			Assert.IsTrue(aS.moreFields());
		}
		
		// test getNextField with valid data
		[Test]
		public void TestGetNextFieldValidData()
		{
			AdifString aS = new AdifString("<call:5>VA3HJ <qso_date:8>20080722" +
				"<time_on:6>061219 <eor>");
			Assert.AreEqual(aS.getNextField(), "<call:5>VA3HJ");
			Assert.AreEqual(aS.getNextField(), "<qso_date:8>20080722");
			Assert.AreEqual(aS.getNextField(), "<time_on:6>061219");
			Assert.AreEqual(aS.getNextField(), "<eor>");
		}
		
		// test getNextField with valid data, but no more fields
		[Test, ExpectedException(typeof(QsoException))]
		public void TestGetNextFieldNoMoreFields()
		{
			AdifString aS = new AdifString("<call:5>VA3HJ <qso_date:8>20080722 " +
                "<time_on:6>061219 <eor>");
            aS.getNextField();
            aS.getNextField();
            aS.getNextField();
            aS.getNextField();
			// should fail on this call
			aS.getNextField();
			Assert.Fail("TestGetNextFieldNoMoreFields test failed");
		}
		
		// test getNextField with no data length
		[Test, ExpectedException(typeof(QsoException))]
		public void TestGetNextFieldNoDataLength()
		{
			AdifString aS = new AdifString("<call:>VA3HJ<eor>");
			aS.getNextField();
			Assert.Fail("TestGetNextFieldNoDataLength test failed");
		}
		
		// test getNextField with invalid length
		[Test, ExpectedException(typeof(QsoException))]
		public void TestGetNextFieldBadDataLength()
		{
			AdifString aS = new AdifString("<call:F>VA3HJ<eor>");
			aS.getNextField();
			Assert.Fail("TestGetNextFieldBadDataLength test failed");
		}
		
		// test getNextField with overflow on data length
		[Test, ExpectedException(typeof(QsoException))]
		public void TestGetNextFieldOverflowLength()
		{
			AdifString aS = new AdifString("<call:999999999999>VA3HJ<eor>");
			aS.getNextField(); 
			Assert.Fail("TestGetNextFieldOverflowLength test failed");
		}
		
		// test getFirstField with valid data
		[Test]
		public void TestGetFirstFieldValidData()
		{
			AdifString aS = new AdifString("<call:5>VA3HJ <qso_date:8>20080722 " +
                "<time_on:6>061219 <eor>");
			Assert.AreEqual(aS.getNextField(), "<call:5>VA3HJ");
			Assert.AreEqual(aS.getFirstField(), "<call:5>VA3HJ");
		}
		
		// test getFirstField with no data length
		[Test, ExpectedException(typeof(QsoException))]
		public void TestGetFirstFieldNoDataLength()
		{
			AdifString aS = new AdifString("<call:>VA3HJ<eor>");
			aS.getFirstField();
			Assert.Fail("TestGetFirstFieldNoDataLength test failed");
		}
		
		// test getFirstField with invalid length
		[Test, ExpectedException(typeof(QsoException))]
		public void TestGetFirstFieldBadDataLength()
		{
			AdifString aS = new AdifString("<call:F>VA3HJ<eor>");
			aS.getFirstField();
			Assert.Fail("TestGetFirstFieldBadDataLength test failed");
		}
		
		// test getFirstField with overflow on length
		[Test, ExpectedException(typeof(QsoException))]
		public void TestGetFirstFieldOverflowLength()
		{
			AdifString aS = new AdifString("<call:999999999999>VA3HJ<eor>");
			aS.getFirstField();
			Assert.Fail("TestGetFirstFieldOverflowLength test failed");
		}
		
		// test getKeyFromFieldName with valid input
		[Test]
		public void TestGetKeyFromFieldName()
		{
			Assert.AreEqual(AdifString.getKeyFromFieldName("<call:5>"), "call");
		}
		
		// test getKeyFromFieldName with missing '<'
		[Test, ExpectedException(typeof(QsoException))]
		public void TestGetKeyFromFieldNameMissingStart()
		{
			AdifString.getKeyFromFieldName("call:5>");
			Assert.Fail("TestGetKeyFromFieldNameMissingStart test failed");
		}
		
		// test getKeyFromFieldName with missing '>'
		[Test, ExpectedException(typeof(QsoException))]
		public void TestGetKeyFromFieldNameMissingEnd()
		{
			AdifString.getKeyFromFieldName("<call:5");
			Assert.Fail("TestGetKeyFromFieldNameMissingEnd test failed");
		}
		
		// test getValueFromField with valid input
		[Test]
		public void TestGetValueFromFieldValidInput()
		{
			Assert.AreEqual(AdifString.getValueFromField(" <call:5>VA3HJ "), "VA3HJ");
		}
		
		// test getValueFromField with too few value chars
		[Test, ExpectedException(typeof(QsoException))]
		public void TestGetValueFromFieldTooSmallValue()
		{
			AdifString.getValueFromField("<call:7>VA3HJ ");
			Assert.Fail("TestGetValueFromFieldTooSmallValue test failed");
		}
	}
}

