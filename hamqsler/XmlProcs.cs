/*
 *  Author:
 *       Jim Orcheson <jimorcheson@gmail.com>
 * 
 *  Copyright (c) 2012, 2013 Jim Orcheson
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
using System.Globalization;
using System.Xml;

namespace hamqsler
{
    /// <summary>
    /// XmlProcs contains procedures for handling the XML for QSL Cards. This is a static class.
    /// </summary>
    public static class XmlProcs
    {
        /// <summary>
        /// Retrieves the first child element for the input XmlNode
        /// </summary>
        /// <param name="parent">The XmlNode to operate on.</param>
        /// <returns>
        /// The first child element of the input XmlNode. Null if there is no child.
        /// </returns>
        public static XmlNode GetFirstChildElement(XmlNode parent)
        {
            XmlNode node = null;
            // does the parent have children?
            if(parent.HasChildNodes)
            {
                node  = parent.FirstChild;
                while (node != null && node.NodeType != XmlNodeType.Element)
                    node = node.NextSibling;
            }
            // return the first child or null
            return node;
        }

        /// <summary>
        /// Returns the next sibling of the input XmlNode
        /// </summary>
        /// <param name="node">The XmlNode to find the next sibling of</param>
        /// <returns>
        /// The next sibling, or null if there is no next sibling
        /// </returns>
        public static XmlNode GetNextSiblingElement(XmlNode node)
        {
            XmlNode siblingNode = node.NextSibling;
            while (siblingNode != null && siblingNode.NodeType != XmlNodeType.Element)
            {
                siblingNode = siblingNode.NextSibling;
            }
            return siblingNode;
        }

        /// <summary>
        /// Return the XmlText node for the input XmlNode
        /// </summary>
        /// <param name="parent">The XmlNode for which to retrieve the text</param>
        /// <returns>
        /// The XmlText node associated with the input node, or null if there is no XmlText node
        /// </returns>
        public static XmlText GetTextNode(XmlNode parent)
        {
            XmlNode textNode = parent.FirstChild;
            while (textNode != null && textNode.NodeType != XmlNodeType.Text)
            {
                textNode = textNode.NextSibling;
            }
            return (XmlText)textNode;
        }

        /// <summary>
        /// Converts the input XmlNode to a System.Drawing.Color object
        /// </summary>
        /// <param name="node">The XmlNode containing the color information</param>
        /// <returns>
        /// Color object corresponding to the XML
        /// </returns>
        /// <exception>
        /// XmlException if the Xml does not contain brush properties
        /// </exception>
        /// <exception>
        /// XmlException if the Xml contains nodes other than a, b, g, r
        /// </exception>
        /// <exception>
        /// XmlException if the Xml does not contain Color information
        /// </exception>
        public static System.Drawing.Color ConvertXmlToColor(
        	XmlNode node, CultureInfo cardCulture)
        {
            XmlNode bNode = XmlProcs.GetFirstChildElement(node);
            if (bNode == null)
                throw new XmlException("Programming Error: Brush has no XML properties");
            switch (bNode.Name)
            {
            	case "System.Windows.Media.SolidColorBrush":
            		XmlNode cNode = XmlProcs.GetFirstChildElement(bNode);
            		switch(cNode.Name)
            		{
		                case "Color":
		                    XmlNode chanNode = XmlProcs.GetFirstChildElement(cNode);
		                    byte a = 0, r = 0, g = 0, b = 0;
		                    while (chanNode != null)
		                    {
		                        XmlText text = XmlProcs.GetTextNode(chanNode);
		                        switch (chanNode.Name)
		                        {
		                            case "A":
		                                a = Byte.Parse(text.Value, cardCulture);
		                                break;
		                            case "R":
		                                r = Byte.Parse(text.Value, cardCulture);
		                                break;
		                            case "G":
		                                g = Byte.Parse(text.Value, cardCulture);
		                                break;
		                            case "B":
		                                b = Byte.Parse(text.Value, cardCulture);
		                                break;
		                            default:
		                                throw new XmlException("Invalid SolidColorBrush color");
		                        }
		                        chanNode = XmlProcs.GetNextSiblingElement(chanNode);
		                    }
		                    return System.Drawing.Color.FromArgb(a, r, g, b);
		                default:
		                    throw new XmlException("Invalid color type");
            		}
            	default:
            		throw new XmlException("Color not specified properly in card file");
            }
        }

	}
}
