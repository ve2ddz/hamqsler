//  
//  AdifString.cs
//  
//  Author:
//       Jim <jim@va3hj.ca>
// 
//  Copyright (c) 2011 VA3HJ Software
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
using System.Linq;
using System.Text;

namespace Qsos
{
    public class AdifString
    {
        private string inString = string.Empty;
        int index = 0;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="inStr"></param>
        /// <exception>
        /// QsoExcpeption if input string does not contain <eor>
        /// </exception>
        public AdifString(string inStr)
        {
            inString = inStr;
            string upper = inStr.ToUpper();
            int iEOR = upper.IndexOf("<EOR>");
            if (iEOR == -1)
            {
                QsoException ex = new QsoException("ADIF string does not contain <eor>");
                ex.Data.Add("ADIF String", inStr);
                throw ex;
            }
        }

        /// <summary>
        /// determines if there are more fields in the AdifString
        /// </summary>
        /// <returns>
        /// true if another field exists
        /// false if no more fields</returns>
        public bool moreFields()
        {
            string field = null;
            try
            {
                // see if we can get a field
                field = getAField();
                // getAField increments index past the field, so must decrement
                // to before field
                index -= field.Length;
            }
            catch (Exception)
            {
                // ignore, let getFirstField or getNextField catch
                return true;
            }
            if (field == null)
                return false;
            else
                return true;
        }

        /// <summary>
        /// reset to first field and retrieve it
        /// </summary>
        /// <returns>first field in ADIFString</returns>
        /// <exception>
        /// QsoException for any error</exception>
        public string getFirstField()
        {
            index = 0;
            string field = null;
            try
            {
                field = getAField();
            }
            catch (Exception e)
            {
                QsoException ex = new QsoException(
                    "Error occured while attempting to retrieve the first ADIF field"
                    , e);
                throw ex;
            }
            return field;
        }

        /// <summary>
        /// retrieves the next field in the ADIF string
        /// </summary>
        /// <returns>next string in the ADIFString</returns>
        /// <exception>
        /// QsoException if there are no more fields or if field is invalid</exception>
        public string getNextField()
        {
            string field = null;
            try
            {
                field = getAField();
            }
            catch (Exception e)
            {
                QsoException ex = new QsoException(
                    "Error occured while attempting to retrieve the next ADIF field"
                    , e);
                throw ex;
            }
            if (field == null)
            {
                QsoException ex = new QsoException("Programming Error: Attempted to " +
                    "retrieve field from an ADIFString but no fields remain. \r\n" +
                    "Contact program vendor.");
                throw ex;
            }
            return field;
        }

        /// <summary>
        /// gets the next field in the ADIF QSO, if one exists
        /// </summary>
        /// <returns>
        /// null if no field
        /// field if field exists</returns>
        /// <exception>
        /// QsoException if no '>' found
        /// ArgumentException if the data length is not specified
        /// FormatException if length is not an integer
        /// OverflowException if length is greater than Int32.MaxValue
        /// </exception>
        private string getAField()
        {
            // get
            string adif = inString.Substring(index);
            int fStart = adif.IndexOf('<');
            if(fStart == -1)
            {
                return null;
            }
            adif = adif.Substring(fStart);
            int fEnd = adif.IndexOf('>');
            if (fEnd == -1)
            {
                return null;
            }
            string fieldName = adif.Substring(0, fEnd + 1);
            int dataLen = fieldLength(fieldName);
            if (dataLen == -1)
            {
                QsoException ex = new QsoException("Invalid QSO field name");
                ex.Data.Add("QSO Field Name:", fieldName);
                throw ex;
            }
            else if (dataLen == 0)
            {
                index += fieldName.Length + fStart;
                return fieldName;
            }
            else
            {
                index += fieldName.Length + dataLen + fStart; 
                return adif.Substring(0, fieldName.Length + dataLen);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fName">String containing the field name</param>
        /// <returns>
        /// 0 if there is no data for the field
        /// -1 if there is an error in the field
        /// >1 if data exists for the field</returns>
        /// <exception>
        /// ArgumentNullException if no length is specified
        /// FormatException if length is not an integer
        /// OverflowException if length is greater than System.Int32.MaxValue
        /// </exception>
        public static int fieldLength(string fName)
        {
            if (fName == null)
            {
                QsoException ex = new QsoException("Programming Error: Attempting to " +
                    "get the field length contained in a null QSO field.\r\n" +
                    "Contact program vendor.");
                throw ex;
            }
            // look for end of field name
            int iEnd = fName.IndexOf('>');
            // not there? return -1 to show error
            if (iEnd == -1)
            {
                return -1;
            }
            // look for separator before size
            int index = fName.IndexOf(":");
            if (index == -1)
            {
                // no size, so return 0
                return 0;
            }
            // look for separator for data type
            int iType = fName.IndexOf(':', index + 1);
            if (iType != -1)
            {
                // separator found, so length is between index and iEnd
                iEnd = iType;
            }
            string strLen = fName.Substring(index + 1, iEnd - index - 1);
            int len = Int32.Parse(strLen);
            return len;
        }

        /// <summary>
        /// returns the field name part of the string
        /// </summary>
        /// <param name="field">string to parse for the field name</param>
        /// <returns>null if string does not contain both < and >
        /// otherwise returns the text between and including < and >
        /// </returns>
        public static string getFieldName(string field)
        {
            // look for start of field
            int iStart = field.IndexOf('<');
            if (iStart == -1)
            {
                return null;
            }
            int iEnd = field.IndexOf('>');
            if (iEnd == -1)
            {
                return null;
            }
            return field.Substring(iStart, iEnd - iStart + 1);
        }

        /// <summary>
        /// retrieves key from the field name
        /// </summary>
        /// <param name="fName">field name</param>
        /// <returns>key from field name</returns>
        /// <exception>
        /// QsoException if input does not contain both '<' and '>'</exception>
        public static string getKeyFromFieldName(string fName)
        {
            string field = getFieldName(fName);
            if (field == null)
            {
                QsoException ex = new QsoException("Programming Error: Trying to get " +
                    "key from an invalid QSO field name.\r\n" +
                    "Contact program vendor.");
                ex.Data.Add("QSO field name:", fName);
                throw ex;
            }
            int index = field.IndexOf(':');
            if (index == -1)
            {
                index = field.IndexOf('>');
            }
            return field.Substring(1, index - 1);
        }

        /// <summary>
        /// retrieve value from QSO field
        /// </summary>
        /// <param name="field">QSO field</param>
        /// <returns>value specified in QSO field</returns>
        /// <exception>
        /// QsoException if value in input contains fewer than specified number of characters
        /// </exception>
        public static string getValueFromField(string field)
        {
            string fName = getFieldName(field);
            int valueLen = fieldLength(fName);
            int index = field.IndexOf(fName) + fName.Length;
            if (index + valueLen > field.Length)
            {
                QsoException ex = new QsoException("Programming Error: Field does " +
                    "not contain specified number of characters in the value.\r\n" +
                    "Contact program vendor");
                ex.Data.Add("QSO field:", field);
                throw ex;
            }
            string value = field.Substring(index, valueLen);
            return value;
        }
    }
}
