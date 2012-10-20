/*
 *  Author:
 *       Jim Orcheson <jimorcheson@gmail.com>
 * 
 *  Copyright (c) 2012 Jim Orcheson
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
 using Qsos;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace hamqsler
{
	/// <summary>
	/// Description of DisplayQsos.
	/// </summary>
	public class DisplayQsos : ObservableCollection<QsoWithInclude>
	{
		
		private bool isDirty = false;
		public bool IsDirty
		{
			get {return isDirty;}
			set {isDirty = value;}
		}
		
		/// <summary>
		/// Default constructor
		/// </summary>
		public DisplayQsos()
		{
		}
		
		/// <summary>
		/// Import QSOs from the ADIF file and store for display
		/// </summary>
		/// <param name="adifFile">Full file name of the ADIF file</param>
		/// <returns>Error string if any errors, or null if none</returns>
		public string ImportQsos(string adifFile)
		{
            this.Clear();			// remove all entries from the ObservableCollection
            return AddQsos(adifFile);
		}
		
		/// <summary>
		/// Add QSOs from the ADIF file and store for display
		/// </summary>
		/// <param name="adifFile">full name of the Adif file</param>
		/// <returns>Error string if any errors, or null if none</returns>
		public string AddQsos(string adifFile)
		{
			QsoWithIncludeEqualityComparer eComparer = new QsoWithIncludeEqualityComparer();
			List<QsoWithInclude>qList = new List<QsoWithInclude>();
			foreach(QsoWithInclude qwi in this)
			{
				qList.Add(qwi);
			}
			bool qsoError;
			string error = AddQsosFromAdifFile(adifFile, ref qList, out qsoError);
			// make sure we do not have any duplicates
            var qList2 = qList.Distinct<QsoWithInclude>(eComparer);
            var qs = from qsoWith in qList2
            	orderby qsoWith.DateTime
            	select qsoWith;
            List<QsoWithInclude> qList3 = new List<QsoWithInclude>();
            foreach (QsoWithInclude qw in qs) 
            {
            	qList3.Add(qw);
            }
            // now add the items in sorted order
            DateTimeComparer dtComparer = new DateTimeComparer();
            qList3.Sort(dtComparer);
            this.Clear();
            foreach(QsoWithInclude qwi in qList3)
            {
            	this.Add(qwi);
            }
            if (qsoError)
            {
                return "One or more QSOs contains an invalid field.\n\rThese QSOs have not been imported.\n\r" +
                    "See the log file for details.";
            }
            return null;	// no error
		}
		
		/// <summary>
		/// Adds QSOs from the specified ADIF file to the list
		/// </summary>
		/// <param name="adifFile">Name of the ADIF file</param>
		/// <param name="qList">QSOs list to add the ADIF file QSOs to</param>
		/// <param name="qsoError">Bool indicating that a QSO related error occurred.</param>
		/// <returns>Error string or null</returns>
		private string AddQsosFromAdifFile(string adifFile, ref List<QsoWithInclude> qList, out bool qsoError)
		{
			ExceptionLogger logger = App.Logger;
			qsoError = false;
			// read all of the ADIF file
			string adifFileContents = File.ReadAllText(adifFile);
			// bypass header if any
			int index = 0;
			string adif = (string)adifFileContents.Clone();
			index = adif.ToUpper().IndexOf("<EOH>");
			if(index != -1)
			{
				adif = adif.Substring(index + 5); // move past eoh
			}
			// now split for each QSO, create Qso, and add to a SortedList
			index = adif.ToUpper().IndexOf("<EOR>");
			if (index == -1)			// check that file contains at least one ADIF record
			{
				return "You are attempting to import a file that does not contain ADIF records.";
			}
			adif = adif.Substring(adif.IndexOf('<'));  // skip any characters before first qso record
			Qso q;
            qsoError = false;      // keep track of bad QSOs; display MessageBox if one or more errors
            // for each ADIF record
			while ((index = adif.ToUpper().IndexOf("<EOR>")) != -1)
			{
				string qsoStr = adif.Substring(0, index) + "<EOR>";
				try
				{
					q = new Qso(logger);

					q.setQsoFromAdif(qsoStr);  // create Qso from ADIF record
                    QsoWithInclude qso = new QsoWithInclude(q); 
                    qList.Add(qso);	// add Qso to the list
                }
				catch (QsoException ex)
				{
					ex.Data.Add("Input String", qsoStr);
					logger.Log(ex, false, false);
                    qsoError = true;
				}
				
				adif = adif.Substring(index + 5);   // remove qso from the string
			}
			return null;
		}
	}
}
