//  
//  QslBureau.cs
//  
//  Author:
//       Jim <jimorcheson@gmail.com>
// 
//  Copyright (c) 2009-2011 VA3HJ Software
//  Copyright (c) 2013 Jim Orcheson
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
using System.Xml;

namespace QslBureaus
{
    /// <summary>
    /// QslBureau holds info about a single Qsl Bureau as read in from QslBureaus.xml
    /// </summary>
    public class QslBureau
    {
        private string regex = string.Empty;
        /// <summary>
        /// regular expression property to compare to a callsign
        /// </summary>
        public string RegularExpression
        {
            get { return regex; }
        }

        private string bureau = "ZZZZ";
        /// <summary>
        /// bureau property that corresponds to the regular expression
        /// </summary>
        public string Bureau
        {
            get { return bureau; }
        }

        /// <summary>
        /// Constructor intended for use only to get assembly info (used in QslBureaus ctor
        /// and QslDesignAndPrint)
        /// </summary>
        public QslBureau() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="node">XmlNode that contains the input from QslBureaus.xml</param>
        /// <exception>NullReferenceException if argument is null, or if the node 
        /// has no attributes.</exception>
        public QslBureau(XmlNode node)
        {
            // get the node's attributes
            XmlAttributeCollection attributes = node.Attributes;
            // process each attribute for values
            foreach (XmlAttribute attr in attributes)
            {
                switch (attr.LocalName)
                {
                    case "Bureau":
                        bureau = attr.Value;
                        break;
                    case "Regex":
                        regex = attr.Value;
                        break;
                }
            }
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            //       
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237  
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            QslBureau qB = (QslBureau) obj;
            if (this.Bureau.Equals(qB.Bureau) &&
                this.RegularExpression.Equals(qB.RegularExpression))
            {
                return true;
            }
            return false;
        }

        // operator==
        public static bool operator ==(QslBureau qB1, QslBureau qB2)
        {
            return qB1.Equals(qB2);
        }

        // operator!=
        public static bool operator !=(QslBureau qB1, QslBureau qB2)
        {
            return !qB1.Equals(qB2);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return this.RegularExpression.GetHashCode();
        }
    }
}
