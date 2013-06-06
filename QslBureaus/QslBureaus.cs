//  
//  QslBureaus.cs
//  
//  Author:
//       Jim <jimorcheson@gmail.com>
// 
//  Copyright (c) 2009-2011 VA3HJ Software
// 	Copyright (c) 2013 Jim Orcheson
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;

namespace QslBureaus
{
    /// <summary>
    /// QslBureaus holds information about each QSL Bureau.
    /// It is the main interface to QSL Bureau information.
    /// </summary>
    public static class QslBureaus
    {
		public static readonly string NoBureau = "ZZZZ";
        private static string version = string.Empty;
        /// <summary>
        /// Version property for the QslBureaus.xml file
        /// </summary>
        public static string Version
        {
            get { return version; }
        }

        private static string creationDate = string.Empty;
        /// <summary>
        /// Creation Date property for the QslBureaus.xml file
        /// </summary>
        public static string CreationDate
        {
            get { return creationDate; }
        }

        private static List<QslBureau> bureaus = new List<QslBureau>();
        /// <summary>
        /// Bureaus property - returns a List<QslBureau> of all Qsl Bureaus
        /// </summary>
        public static List<QslBureau> Bureaus
        {
            get { return bureaus; }
        }

        /// <summary>
        /// Static constructor for QslBureaus
        /// Note that there is no non-static constructor for this class
        /// and no way to instantiate an object of this class type
        /// </summary>
        /// <exception>may throw any of the following Exceptions:
        /// NullReferenceException if QslBureaus.xml embedded resource cannot be found
        ///     or for any node that does not contain attributes
        /// XmlException if there is a load or parse error for QslBureaus.xml
        /// IOException if there is an IO error while opening QslBureaus.xml
        /// SecurityException if user does not have read permission on the DLL
        ///     (not likely since user would not be able to execute this code).
        /// </exception>
        static QslBureaus()
        {
            // get the assembly for this DLL
            Assembly assembly = Assembly.GetAssembly(new QslBureau().GetType());
            // get a stream for the QslBureaus.xml file (an embedded resource in the DLL
            Stream str = assembly.GetManifestResourceStream("QslBureaus.QslBureaus.xml");
             // load in the xml file
            XmlDocument doc = new XmlDocument();
            doc.Load(str);
            // get the node that constitutes the document element
            XmlNode node = doc.DocumentElement;
            if (node.Name.Equals("QslBureaus"))
            {
                // retrieve the attributes and their values
                XmlAttributeCollection bureausAttributes = node.Attributes;
                foreach (XmlAttribute attr in bureausAttributes)
                {
                    switch (attr.LocalName)
                    {
                        case "Version":
                            version = attr.Value;
                            break;
                        case "CreationDate":
                            creationDate = attr.Value;
                            break;
                    }
                }
                // now process each chile node (QslBureau type)
                node = node.FirstChild;
                while (node != null)
                {
                    if (node.Name.Equals("QslBureau"))
                    {
                        bureaus.Add(new QslBureau(node));
                        node = node.NextSibling;
                    }
                    // child node is not a QslBureau, so error
                    else
                    {
                        throw new XmlException("Invalid QslBureaus.xml file: QslBureaus node contains non-QslBureau node");
                    }
                }
            }
            // document element is not a QslBureaus, so error
            else
            {
                throw new XmlException("Invalid QslBureaus.xml file: first node is not \"QslBureaus\"");
            }
        }

        /// <summary>
        /// Retrieves the Qsl Bureau corresponding to the input callsign
        /// </summary>
        /// <param name="callsign">Callsign to retrieve bureau for</param>
        /// <returns>Qsl bureau corresponding to the callsign, or "ZZZZ" if 
        /// country corresponding to the callsign does not have a bureau.</returns>
        public static string Bureau(string callsign)
        {
            string buro = NoBureau;
            foreach(QslBureau qslB in bureaus)
            {
                Regex buroReg = new Regex(qslB.RegularExpression);
                if(buroReg.IsMatch(callsign))
                {
                    buro = qslB.Bureau;
                    break;
                }
            }
            return buro;
        }

    }
}
