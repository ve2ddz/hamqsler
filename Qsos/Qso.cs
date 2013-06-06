//  
//  Qso.cs
//  
//  Author:
//       Jim <jimorcheson@gmail.com>
// 
//  Copyright (c) 2011 VA3HJ Software
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
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Qsos
{
    /// <summary>
    /// Qso class encapsulates a single QSO
    /// </summary>
    public class Qso : Dictionary<String, String>, IComparable
    {
        AdifString adifStr = null;
        Logger logger = null;
        /// <summary>
        /// default constructor
        /// </summary>
        public Qso() { }

        public Qso(Logger l)
        {
            logger = l;
        }

        /// <summary>
        /// Populates the Qso object from an ADIF string
        /// </summary>
        /// <param name="adifQso">ADIF string</param>
        /// <throws>QsoException if adifQso contains any errors
        /// </throws>
        public void setQsoFromAdif(string adifQso)
        {
            Clear();
            try
            {
                adifStr = new AdifString(adifQso);
            }
            catch (QsoException e)
            {
                e.Data.Add("QSO:", adifQso);
                throw;
            }
            try
            {
                while (adifStr.moreFields())
                {
                    string field = adifStr.getNextField();
                    if (field.ToUpper() == "<EOR>")
                    {
                        break;
                    }
                    setFieldFromAdifField(field);
                }
            }
            catch (QsoException ex)
            {
                // problem with qso field.
                // just add the adif string input and rethrow
                ex.Data.Add("ADIF QSO", adifQso);
                throw;
            }
			// determine if callsign is valid
            if(!validateQso())
            {
                QsoException ex = new QsoException("QSO does not contain minimum required fields:\n" +
                "call, date, time, mode, band or frequency");
                ex.Data.Add("ADIF QSO", adifQso);
                throw ex;
            }
			CallSign call = new CallSign(this["call"]);
			if(!CallSign.IsValid(call.Call))
			{
				QsoException ex = new QsoException("QSO does not contain a valid callsign");
				ex.Data.Add("ADIF QSO", adifQso);
				throw ex;
			}
            // determine if required fields are included
            addBandFreqFields();
        }

        /// <summary>
        /// Validates the Qso object by determining if the Qso contains call, mode, date, 
        /// time_on or time_off and band or freq or freq_rx keys
        /// </summary>
        /// <returns>true if Qso contains call, mode date, time_on or time_off,
        /// and band or freq or freq_rx keys
        /// false otherwise</returns>
        public bool validateQso()
        {
            bool call = false;
            bool date = false;
            bool time = false;
            bool mode = false;
            bool band_freq = false;
            foreach (string key in Keys)
            {
                switch (key.ToUpper())
                {
                    case "CALL":
                        call = true;
                        break;
                    case "QSO_DATE":
                        date = true;
                        break;
                    case "TIME_ON":
                        time = true;
                        break;
                    case "TIME_OFF":
                        time = true;
                        break;
                    case "MODE":
                        mode = true;
                        break;
                    case "BAND":
                        band_freq = true;
                        break;
                    case "FREQ":
                        band_freq = true;
                        break;
                    case "FREQ_RX":
                        band_freq = true;
                        break;
                }
            }

            if(!call || !date || !time || !mode || !band_freq)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        public void addBandFreqFields()
        {
            string band = "";
            bool bBand = false;
            bool bFreq = false;
            HamBand h = null;
            foreach (string key in Keys)
            {
                switch (key.ToUpper())
                {
                    case "BAND":
                        // from band, get freq (lower edge of ham band)
                        bBand = true;
                        h = HamBands.getHamBand(this[key].ToLower());
						break;
                    case "FREQ":
                        // from freq, get band
                        bFreq = true;
                        Single fr = 0;
                        // check if freq is not in format for current culture, and use InvariantCulture if it is not.
                        if (!Single.TryParse(this[key], NumberStyles.Float, CultureInfo.CurrentCulture, out fr))
                        {
                            if (!Single.TryParse(this[key], NumberStyles.Float, CultureInfo.InvariantCulture, out fr))
                            {
                                throw new QsoException("Invalid frequency value specified");
                            }
                        }
                        h = HamBands.getHamBand(fr);
                        band = h.Band;
                        break;
                    case "FREQ_RX":
                        // from freq_rx, get band, but only if no freq field yet
                        if (!bFreq)
                        {
                            goto case "FREQ";
                        }
                        break;
                }
            }
            // set band if not included in this qso
            if (!bBand)
            {
                this["band"] = band;
            }
       }

        /// <summary>
        /// Sets field based on key and value input
        /// </summary>
        /// <param name="key">field key</param>
        /// <param name="value">field value</param>
        /// <exception>QsoException if key is null or empty
        /// </exception>
        public void setField(string key, string value)
        {
            if (key == null || key == string.Empty)
            {
                throw new QsoException("Attempt to add a field with null or empty key");
            }
            this[key] = value;
        }

        /// <summary>
        /// retrieve value associated with key
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>value associated with the key</returns>
        /// <exception>KeyNotFoundException if the Qso does not contain an entry for the key
        /// ArgumentNullException if the key is null
        /// ArgumentException if key is empty
        /// </exception>
        public string getValue(string key)
        {
            return this[key];
        }

        /// <summary>
        /// Retrieve value associated with key. If key not found, return default value
        /// </summary>
        /// <param name="key">key for the value</param>
        /// <param name="defaultVal">default value (if key not found)</param>
        /// <returns>value associated with key, or default value if no entry found</returns>
        /// <exception>
        /// ArgumentNullException if the key is null
        /// ArgumentException if key is empty
        /// </exception>
        public string getValue(string key, string defaultVal)
        {
            if (key == null)
                throw new ArgumentNullException("Key is null");
            if (key == string.Empty)
                throw new ArgumentException("Key contains an empty string");
            string val = string.Empty;
            bool gotVal = TryGetValue(key.ToLower(), out val);
            if (!gotVal)
                val = defaultVal;
            return val;

        }


        /// <summary>
        /// retrieve value associated with key
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>value associated with the key</returns>
        /// <exception>KeyNotFoundException if the Qso does not contain an entry for the key
        /// ArgumentNullException if the key is null
        /// ArgumentException if key is empty
        new public string this[string key]
        {
            get
            {
                if (key == null)
                    throw new ArgumentNullException("trying to retrieve a value using a null key");
                if (key == string.Empty)
                    throw new ArgumentException("Empty key");
                return base[key.ToLower()];
            }
            set
            {
                if (key == null)
                {
                    throw new ArgumentNullException("trying to set value with null key");
                }
                if (key == string.Empty)
                {
                    ArgumentException ex = new ArgumentException("Empty key");
                    throw ex;
                }
                base[key.ToLower()] = value.Trim();
            }
        }

        /// <summary>
        /// Removes the element specified by the key
        /// </summary>
        /// <param name="key">key of the element to remove</param>
        /// <returns>true if element was successfully found and removed;
        /// false otherwise</returns>
        /// <exception cref>
        /// ArgumentNullException if the key is null
        /// ArgumentException if key is empty</exception>
        new public bool Remove(string key)
        {
            if (key == null)
                throw new ArgumentNullException("Attempting to remove element but null key specified");
            if (key == string.Empty)
                throw new ArgumentException("Attempting to remove element but specified key is empty");
            return base.Remove(key.ToLower());
        }

        /// <summary>
        /// convert a QSO field to an ADIF field
        /// </summary>
        /// <param name="key">key of value to convert</param>
        /// <returns>string in ADIF format</returns>
        /// <exception>
        /// ArgumentNullException if the key is null
        /// ArgumentException if key is empty
        /// KeyNotFoundException if key cannot be found</exception>
        public string fieldToAdif(string key)
        {
            if(key == null)
                throw new ArgumentNullException("Invalid key: null");
            if (key == string.Empty)
                throw new ArgumentException("Invalid key: empty");
            string val = this[key];
            return string.Format("<{0}:{1}>{2}", key, val.Length, val);
        }

        public string ToAdifString()
        {
            string adif = string.Empty;
            foreach (string key in Keys)
            {
                adif += fieldToAdif(key);
            }
            adif += "<eor>";
            return adif;
        }


        /// <summary>
        /// Converts an ADIF field into a QSO field and stores it in the QSO object
        /// </summary>
        /// <param name="field">ADIF field to store</param>
        /// <exception>QsoException if:
        /// 1. field is null or empty
        /// 2. ADIF field is invalid
        /// FormatException if size is nothing or non-integer
        /// </exception>
        public void setFieldFromAdifField(string field)
        {
            if (field == null || field == string.Empty)
            {
                throw new QsoException("Invalid ADIF field: null or empty");
            }
            string fStr = AdifString.getFieldName(field);
            if(fStr == null)
            {
                QsoException ex = new QsoException("Programming Error: String passed " +
                    "to Qso.setFieldFromAdifField is not valid.\r\n" +
                    "Contact program vendor.");
                ex.Data.Add("Input:", field);
                throw ex;
            }
            string key = AdifString.getKeyFromFieldName(fStr);
            int len = AdifString.fieldLength(fStr);
            if (len == -1)
            {
                QsoException ex = new QsoException("Programming Error: String passed " +
                    "to Qso.setFieldFromAdifField is not valid.\r\n" +
                    "Contact program vendor.");
                ex.Data.Add("Input:", field);
                throw ex;
            }
            else if (len > 0)
            {
                string value = AdifString.getValueFromField(field);
                setField(key, value);
            }
            else
            {
                setField(key, "");
            }

        }

        /// <summary>
        /// Creates a string representation of the QSO
        /// </summary>
        /// <returns>String representation of the QSO</returns>
        public override string ToString()
        {
            string result = "QSO: ";
            foreach (string key in Keys)
            {
                result += string.Format("{0}='{1}'  ", key, this[key]);
            }
            return result;
        }


        /// <summary>
        /// Implements IComparable.CompareTo method
        /// </summary>
        /// <param name="o">QSO to compare to this object</param>
        /// <returns>0 if the two Qso objects contain the same call, mode, date and time_on
        /// (alternatively time_off), ADIF field values
        /// <0 if the call, mode, date or time_on (time_off) values from this object sort before the
        /// input object, and
        /// >0 if the call, mode, date or time_on (time_off) values from this object sort after the
        /// input object</returns>
        /// <exception>
        /// ArgumentException if the input object is not a Qso
        /// </exception>
        public int CompareTo(object o)
        {
            if (o == null)
                throw new ArgumentNullException("Trying to compare a Qso object with a null object");
            if (!(o is Qso))
            {
                ArgumentException ex = new ArgumentException("Trying to compare a Qso with a non-Qso object");
                ex.Data.Add("Object", o);
                throw ex;
            }
            Qso q = (Qso)o;
            string c = this.getValue("call", string.Empty);
            string qc = q.getValue("call", string.Empty);
            int compare = c.CompareTo(qc);
            if (compare != 0)
                return compare;
            c = this.getValue("mode", string.Empty);
            qc = q.getValue("mode", string.Empty);
            compare = c.CompareTo(qc);
            if (compare != 0)
                return compare;
            c = this.getValue("qso_date", "19450901");
            qc = q.getValue("qso_date", "19450901");
            compare = c.CompareTo(qc);
            if (compare != 0)
                return compare;
            c = this.getValue("time_on", this.getValue("time_off", "0000"));
            qc = q.getValue("time_on", q.getValue("time_off", "0000"));
            compare = c.CompareTo(qc);
            return compare;

        }

        /// <summary>
        /// Compares if two Qso objects are equal (have same call, date, and time_on (or time_off))
        /// </summary>
        /// <param name="obj">Qso to compare to "this"</param>
        /// <returns>true is the Qso objects have the same call, date, and time.
        /// False otherwise</returns>
        /// <exception>
        /// ArgumentException if the input object is a non-Qso object</exception>
        public override bool Equals(object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("Attempting to test if a Qso and a null object are equal");
            if (!(obj is Qso))
            {
                ArgumentException ex = new ArgumentException("Attempting to test if a non-Qso object is "
                    + "equal to a Qso object");
                ex.Data.Add("Object", obj);
                throw ex;
            }
            bool equal = false;
            int compare = this.CompareTo((Qso)obj);
            if (compare == 0)
                equal = true;
            return equal;
        }

        /// <summary>
        /// Calculates hash code for the AdifQso
        /// </summary>
        /// <returns>hash code</returns>
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        /// <summary>
        /// Tests if two AdifQso objects have the same call, date, and time
        /// </summary>
        /// <param name="q1">first AdifQso</param>
        /// <param name="q2">second AdifQso</param>
        /// <returns>True if the two AdifQso objects have the same call, date, and time field values.
        /// False otherwise</returns>
        public static bool operator ==(Qso q1, Qso q2)
        {
            return q1.Equals(q2);
        }

        /// <summary>
        /// Tests if two AdifQso objects do not have the same call, date, and time
        /// </summary>
        /// <param name="q1">first AdifQso</param>
        /// <param name="q2">second AdifQso</param>
        /// <returns>True if the two AdifQso objects do not have the same call, date, and time field values.
        /// False if the calls, dates, and times are the same.</returns>
        public static bool operator !=(Qso q1, Qso q2)
        {
            return !q1.Equals(q2);
        }

    }
}
