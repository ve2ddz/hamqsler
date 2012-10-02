// //  
// //  Author:
// //       Jim <jim@va3hj.ca>
// // 
// //  Copyright (c) 2011 VA3HJ Software
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
// 
using System;
using System.Xml;
using NUnit.Framework;
using QslBureaus;
namespace QslBureausTests
{
	[TestFixture()]
	public class QslBureauTests
	{
		private QslBureau qslB = null;
		
		[SetUp]
		public void Setup()
		{
           XmlDocument xml = new XmlDocument();
            string qB = "<QslBureau Bureau=\"VE\" Regex=\"VE.*\" DXCC=\"1\" />";
            xml.LoadXml(qB);
            XmlNode node = xml.DocumentElement;
            qslB = new QslBureau(node);
		}
		
		// test ctor and Bureau accessor
		[Test()]
		public void TestBureau ()
		{
			Assert.AreEqual("VE", qslB.Bureau);
		}
		
		// test ctor and RegularExpression accessor
		[Test]
		public void TestRegex()
		{
			Assert.AreEqual("VE.*", qslB.RegularExpression);
		}
		
		// test Equals
		[Test]
		public void TestEquals()
		{
           XmlDocument xml = new XmlDocument();
            string qB = "<QslBureau Bureau=\"VE\" Regex=\"VE.*\" DXCC=\"1\" />";
            xml.LoadXml(qB);
            XmlNode node = xml.DocumentElement;
            QslBureau qsl = new QslBureau(node);
            Assert.AreEqual(qslB, qsl);
			Assert.IsTrue(qslB == qsl);
        }
		
		// test not Equal
		[Test]
		public void TestNotEquals()
		{
            XmlDocument xml = new XmlDocument();
            string qB = "<QslBureau Bureau=\"VE\" Regex=\"VE1.*\" DXCC=\"1\" />";
            xml.LoadXml(qB);
            XmlNode node = xml.DocumentElement;
            QslBureau qsl = new QslBureau(node);
            Assert.IsTrue(qslB != qsl);
		}			
			
	}
}

