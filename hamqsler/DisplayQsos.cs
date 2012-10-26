﻿/*
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

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
		public string ImportQsos(string adifFile, QSOsView.OrderOfSort so)
		{
            this.Clear();			// remove all entries from the ObservableCollection
            return AddQsos(adifFile, so);
		}
		
		/// <summary>
		/// Add QSOs from the ADIF file and store for display
		/// </summary>
		/// <param name="adifFile">full name of the Adif file</param>
		/// <returns>Error string if any errors, or null if none</returns>
		public string AddQsos(string adifFile, QSOsView.OrderOfSort so)
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
            List<QsoWithInclude> qList3 = new List<QsoWithInclude>();
            Comparer<QsoWithInclude> comparer = null;
            switch(so)
            {
            		// some code is repeated in each case because there is no way
            		// to define var qs before being used, and it is out of scope
            		// outside the switch
            	case QSOsView.OrderOfSort.DATETIME:		// sort by date/time
           			var qs = from qsoWith in qList2
		            	orderby qsoWith.DateTime
		            	select qsoWith;
		            foreach (QsoWithInclude qw in qs) 
		            {
		            	qList3.Add(qw);
		            }
			        comparer = new DateTimeComparer();
           			break;
           		case QSOsView.OrderOfSort.CALL:			// sort by manager, call, date and time
           			qs = from qsoWith in qList2
           				orderby qsoWith.ManagerCallDateTime
           				select qsoWith;
		            foreach (QsoWithInclude qw in qs) 
		            {
		            	qList3.Add(qw);
		            }
		            comparer = new CallComparer();
           			break;
           		case QSOsView.OrderOfSort.BUREAU:		// sort by bureau, manager, call, date and time
           			qs = from qsoWith in qList2
           				orderby qsoWith.BureauManagerCallDateTime
           				select qsoWith;
		            foreach (QsoWithInclude qw in qs) 
		            {
		            	qList3.Add(qw);
		            }
		            comparer = new BureauComparer();
           			break;
             }
            // now add the items in sorted order
            qList3.Sort(comparer);
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
		/// Sort QSOs based on a comparer
		/// </summary>
		/// <param name="comparer">Comparer object used to sort</param>
		public void SortQSOs(Comparer<QsoWithInclude> comparer)
		{
			// to sort we must move the QSOs to a List
			List<QsoWithInclude> qList = new List<QsoWithInclude>();
			foreach(QsoWithInclude q in this)
			{
				qList.Add(q);
			}
			// sort and put QSOs back
            qList.Sort(comparer);
            this.Clear();
            foreach(QsoWithInclude qwi in qList)
            {
            	this.Add(qwi);
            }
			
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
		
		/// <summary>
		/// Generates a byte array containing QSO info in Adif 2 format (ASCII)
		/// </summary>
		/// <returns>Byte array containing Adif file contents in ASCII</returns>
		public Byte[] GetQsosAsAdif2()
		{
			string adif = GenerateAdifHeader();		// generate header
			// add each QSO
			foreach(QsoWithInclude qwi in this)
			{
				Qso qso = qwi.Qso;
				if(IsDirty)
				{
					qso.setField("qsl_sent_via", qwi.Manager);
				}
				adif += qso.ToAdifString() + "\r\n";
			}
			// change encoding to ASCII
			ASCIIEncoding ascii = new ASCIIEncoding();
			Byte[] encodedBytes = ascii.GetBytes(adif);
			return encodedBytes;
		}
		
		/// <summary>
		/// Create ADIF header
		/// </summary>
		/// <returns>string containing ADIF header</returns>
		private string GenerateAdifHeader()
		{
			// build the ADIF header
			string vers = Assembly.GetExecutingAssembly().GetName().Version.ToString();
			DateTime now = DateTime.Now.ToUniversalTime();
			string datetime = string.Format("{0:D4}-{1:D2}-{2:D2} {3:D2}:{4:D2}:{5:D2} UTC",
			                                now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
			string adif = string.Format("ADIF file created by HamQSLer version {0}\r\n"
			                            + "Copyright (c) 2012 - {1} by VA3HJ Software\r\n"
			                            + "http://www.va3hj.ca\r\n\r\n"
			                            + "Created: {2}\r\n\r\n"
			                            + "<ADIF_VERS:3>2.0\r\n"
			                            + "<PROGRAM:8>HamQSLer\r\n"
			                            + "<PROGRAMVERSION:{3}>{0}\r\n"
			                            + "<EOH>\r\n",
			                            vers, now.Year, datetime,
			                            vers.Length);
			return adif;
		}
		
		/// <summary>
		/// Sets the Include value for every Qso
		/// </summary>
		public void IncludeAllQsos()
		{
			//This seems overkill (retrieve the items, delete the collection,
			// make the change and add back to collection.
			// At first glance, a call to OnCollectionChanged would seem to be better,
			// but there is no action for making a change to one of more elements,
			// only add, remove, reset, move.
			List<QsoWithInclude> list = this.ToList();
			this.Clear();
			foreach(QsoWithInclude qso in list)
			{
				qso.Include = true;
				this.Add(qso);
			}
		}
		
		/// <summary>
		/// Clears the Include value for every Qso
		/// </summary>
		public void ExcludeAllQsos()
		{
			//This seems overkill (retrieve the items, delete the collection,
			// make the change and add back to collection.
			// At first glance, a call to OnCollectionChanged would seem to be better,
			// but there is no action for making a change to one of more elements,
			// only add, remove, reset, move.
			List<QsoWithInclude> list = this.ToList();
			this.Clear();
			foreach(QsoWithInclude qso in list)
			{
				qso.Include = false;
				this.Add(qso);
			}
		}
		
		/// <summary>
		/// Gets list of bands in the QSOs
		/// </summary>
		/// <returns>list of bands</returns>
		public List<string> GetBands()
		{
			List<string> bands = new List<string>();
			foreach(QsoWithInclude qwi in this)
			{
				bands.Add(qwi.Band);
			}
			List<string> bands2 = new List<string>(bands.Distinct());
			bands2.Sort(new BandComparer());
			return bands2;
		}
		
		/// <summary>
		/// Gets list of modes in the QSOs
		/// </summary>
		/// <returns>list of modes</returns>
		public List<string> GetModes()
		{
			List<string> modes = new List<string>();
			foreach(QsoWithInclude qwi in this)
			{
				modes.Add(qwi.Mode);
			}
			List<string> modes2 = new List<string>(modes.Distinct());
			modes2.Sort();
			return modes2;
		}
		
		/// <summary>
		/// Get list of QSL sent statuses in the QSOs
		/// </summary>
		/// <returns>list of sent statuses</returns>
		public List<string> GetSentStatuses()
		{
			List<string> sents = new List<string>();
			foreach(QsoWithInclude qwi in this)
			{
				sents.Add(qwi.Sent);
			}
			List<string> sents2 = new List<string>(sents.Distinct());
			sents2.Sort();
			return sents2;
		}
		
		/// <summary>
		/// Get list of QSO sent via statuses in the QSOs
		/// </summary>
		/// <returns>list of sent via statuses</returns>
		public List<string> GetSentViaStatuses()
		{
			List<string> sentVias = new List<string>();
			foreach(QsoWithInclude qwi in this)
			{
				sentVias.Add(qwi.SentVia);
			}
			List<string> sentVias2 = new List<string>(sentVias.Distinct());
			sentVias2.Sort();
			return sentVias2;
		}
		
		/// <summary>
		/// Get list of QSO rcvd statuses in the QSOs
		/// </summary>
		/// <returns>list of rcvd statuses</returns>
		public List<string> GetRcvdStatuses()
		{
			List<string> rcvds = new List<string>();
			foreach(QsoWithInclude qwi in this)
			{
				rcvds.Add(qwi.Rcvd);
			}
			List<string> rcvds2 = new List<string>(rcvds.Distinct());
			rcvds2.Sort();
			return rcvds2;
		}
		
		/// <summary>
		/// Set the Include property of each QSO based on selection criteria
		/// (settings of the various band, mode, and qsl status checkboxes, and the date/time values
		/// </summary>
		/// <param name="bands">Dictionary showing the checked state of each band checkbox in QsosView</param>
		/// <param name="modes">Dictionary showing checked state of each mode checkbox in QsosView</param>
		public void SetIncludes(ref Dictionary<string, bool> bands,
		                        ref Dictionary<string, bool> modes,
		                        ref Dictionary<string, bool> rcvdStatuses,
		                        ref Dictionary<string, bool> sentStatuses,
		                        ref Dictionary<string, bool> sentViaStatuses)
		{
			List<QsoWithInclude> qsos = this.ToList();
			this.Clear();
			foreach(QsoWithInclude qwi in qsos)
			{
				string band = qwi.Band.ToLower();
				qwi.Include = bands[band];
				string mode = qwi.Mode;
				qwi.Include = qwi.Include && modes[mode];
				string rcvd = qwi.Rcvd;
				qwi.Include = qwi.Include && rcvdStatuses[rcvd];
				string sent = qwi.Sent;
				qwi.Include = qwi.Include && sentStatuses[sent];
				string sentVia = qwi.SentVia;
				qwi.Include = qwi.Include && sentViaStatuses[sentVia];
				this.Add(qwi);
			}
		}
	}
}
