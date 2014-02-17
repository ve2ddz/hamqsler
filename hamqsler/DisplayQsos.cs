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
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
	// one or more QSOs have been modified (only Manager field is modifiable)
		private bool isDirty = false;
		public bool IsDirty
		{
			get {return isDirty;}
			set {isDirty = value;}
		}
		
		// Manager field of one or more QSOs has been modified. A resort is needed
		private bool needsSorting = false;
		public bool NeedsSorting
		{
			get {return needsSorting;}
			set {needsSorting = value;}
		}
		
		private Qsos2 qsos2 = new Qsos2();
		
		/// <summary>
		/// Default constructor
		/// </summary>
		public DisplayQsos()
		{
		}
		
		/// <summary>
		/// Copy constructor
		/// </summary>
		/// <param name="qsos">DisplayQsos object to copy</param>
		public DisplayQsos(DisplayQsos qsos)
		{
			foreach(QsoWithInclude qwi in qsos)
			{
				AddQso(qwi);
			}
			IsDirty = qsos.IsDirty;
		}
		
		/// <summary>
		/// Import QSOs from the ADIF file and store for display
		/// </summary>
		/// <param name="adifFile">Full file name of the ADIF file</param>
		/// <returns>Error string if any errors, or null if none</returns>
		public string ImportQsos(string adifFile, QSOsView.OrderOfSort so, AdifEnumerations aEnums)
		{
            this.Clear();			// remove all entries from the ObservableCollection
            qsos2.ClearQsos();
            IsDirty = false;
            NeedsSorting = false;
            UserPreferences prefs = ((App)App.Current).UserPreferences;
            prefs.AdifFiles.Clear();
            return AddQsos(adifFile, so, aEnums);
		}
		
		/// <summary>
		/// Add QSOs from the ADIF file and store for display
		/// </summary>
		/// <param name="adifFile">full name of the Adif file</param>
		/// <returns>Error string if any errors, or null if none</returns>
		public string AddQsos(string adifFile, QSOsView.OrderOfSort so, AdifEnumerations aEnums)
		{
			string errorString = string.Empty;
			QsoWithIncludeEqualityComparer eComparer = new QsoWithIncludeEqualityComparer();
			// adifFile already checked to ensure that it exists, so just read contents
			string adifFileContents = File.ReadAllText(adifFile);
			int errLen = errorString.Length;
			bool qsoError = !qsos2.Add(adifFileContents, ref errorString, aEnums);
			List<QsoWithInclude>qList = new List<QsoWithInclude>();
			qList.Clear();
			foreach(Qso2 qso in qsos2)
			{
				QsoWithInclude qwi = new QsoWithInclude(qso);
				qList.Add(qwi);
			}
            List<QsoWithInclude> qList3 = new List<QsoWithInclude>();
            Comparer<QsoWithInclude> comparer = null;
            switch(so)
            {
            		// some code is repeated in each case because there is no way
            		// to define var qs before being used, and it is out of scope
            		// outside the switch
            	case QSOsView.OrderOfSort.DATETIME:		// sort by date/time
           			var qs = from qsoWith in qList
		            	orderby qsoWith.DateTime
		            	select qsoWith;
		            foreach (QsoWithInclude qw in qs) 
		            {
		            	qList3.Add(qw);
		            }
			        comparer = new DateTimeComparer();
           			break;
           		case QSOsView.OrderOfSort.CALL:			// sort by manager, call, date and time
           			qs = from qsoWith in qList
           				orderby qsoWith.ManagerCallDateTime
           				select qsoWith;
		            foreach (QsoWithInclude qw in qs) 
		            {
		            	qList3.Add(qw);
		            }
		            comparer = new CallComparer();
           			break;
           		case QSOsView.OrderOfSort.BUREAU:		// sort by bureau, manager, call, date and time
           			qs = from qsoWith in qList
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
            qsos2.ClearQsos();
			// check if a change was made to the qso contents, and set IsDirty if this is the case.
			// This check must be performed here because the call to Clear sets IsDirty to false;
			if(errorString != null && errLen < errorString.Length)
			{
				IsDirty = true;
			}
			bool dirty = IsDirty;
            foreach(QsoWithInclude qwi in qList3)
            {
            	this.AddQso(qwi);
            }
            IsDirty = dirty;
            UserPreferences prefs = ((App)App.Current).UserPreferences;            
			if(prefs.AdifReloadOnStartup)
			{
				prefs.AdifFiles.Add(adifFile);
				prefs.SerializeAsXml();
			}
            if (qsoError || errorString != null)
            {
                IsDirty = true;
				return errorString;
            }
            NeedsSorting = false;
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
            bool dirty = IsDirty;
            this.Clear();
            this.IsDirty = dirty;
            foreach(QsoWithInclude qwi in qList)
            {
            	this.Add(qwi);
            }
            IsDirty = dirty;
            NeedsSorting = false;			
		}
		
		/// <summary>
		/// Generates a byte array containing QSO info in Adif 2 format (ASCII)
		/// </summary>
		/// <returns>Byte array containing Adif file contents in ASCII</returns>
		public Byte[] GetQsosAsAdif2()
		{
			string adif = qsos2.ToAdifString();		// generate header
			// change encoding to ASCII
			ASCIIEncoding ascii = new ASCIIEncoding();
			Byte[] encodedBytes = ascii.GetBytes(adif);
			return encodedBytes;
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
			bool dirty = IsDirty;
			this.Clear();
			foreach(QsoWithInclude qso in list)
			{
				qso.Include = true;
				this.Add(qso);
			}
			IsDirty = dirty;
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
			bool dirty = IsDirty;
			this.Clear();
			foreach(QsoWithInclude qso in list)
			{
				qso.Include = false;
				this.Add(qso);
			}
			IsDirty = dirty;
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
				if(!qwi.Submode.Equals(string.Empty))
				{
					modes.Add(qwi.Submode);
				}
				else
				{
					modes.Add(qwi.Mode);
				}
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
				sentVias.Add(qwi.SendVia);
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
		/// <param name="seDateTime">Start and end dates and times for inclusion of QSOs</param>
		/// <param name="rcvdStatuses">Dictionary showing checked state of each rcvd checkbox in QsosView</param>
		/// <param name="sentStatuses">Dictionary showing checked state of each sent checkbox in QsosView</param>
		/// <param name="sentViaStatuses">Dictionary showing checked state of each sent via checkbox in QsosView</param>
		public void SetIncludes(ref Dictionary<string, bool> bands,
		                        ref Dictionary<string, bool> modes,
		                        ref StartEndDateTime seDateTime,
		                        ref Dictionary<string, bool> rcvdStatuses,
		                        ref Dictionary<string, bool> sentStatuses,
		                        ref Dictionary<string, bool> sentViaStatuses)
		{
			List<QsoWithInclude> qsos = this.ToList();
			bool dirty = IsDirty;
			this.Clear();
			IsDirty = dirty;
			foreach(QsoWithInclude qwi in qsos)
			{
				string band = qwi.Band.ToLower();
				bool include = bands[band];
				string mode = qwi.Mode;
				if(!qwi.Submode.Equals(string.Empty))
				{
					mode = qwi.Submode;
				}
				include = include && modes[mode];
				include = include && IncludeByDatesTimes(qwi, seDateTime);
				string rcvd = qwi.Rcvd;
				include =include && rcvdStatuses[rcvd];
				string sent = qwi.Sent;
				include = include && sentStatuses[sent];
				string sentVia = qwi.SendVia;
				include = include && sentViaStatuses[sentVia];
				qwi.Include = include;
				this.Add(qwi);
			}
		}
		
		/// <summary>
		/// Helper method that determines if the start date and time of the Qso is between the
		/// start date/time and the end date/time
		/// </summary>
		/// <param name="qwi">QsoWithInclude object to compare</param>
		/// <param name="datesTimes">StartEndDateTime object containing the start and end
		/// dates and times for the comparison</param>
		/// <returns>true if qwi date and time within the dates and times, false otherwise</returns>
		private bool IncludeByDatesTimes(QsoWithInclude qwi, StartEndDateTime datesTimes)
		{
			// Will do string comparisons so concatenate date and time together
			string qsoStartDateTime = qwi.DateTime;
			string startDateTime = datesTimes.ValidStartDate + datesTimes.ValidStartTime;
			string endDateTime = datesTimes.ValidEndDate + datesTimes.ValidEndTime;
			// compare start date/time
			int startCompare = String.Compare(qsoStartDateTime, startDateTime, true);
			if(startCompare < 0)		// if qwi.DateTime earlier than start date/time
			{
				return false;
			}
			else		// if later than start date/tome
			{
				int endCompare = String.Compare(qsoStartDateTime, endDateTime, true);
				if(endCompare <= 0)		// if qwi.DateTime earlier/equal to end date/time
				{
					return true;
				}
				return false;			// qwi.DateTime later than end date/time
			}
		}
		
		/// <summary>
		/// Retrieve the earliest and latest dates and times for QSOs within this collection
		/// </summary>
		/// <param name="startDate">date of earliest QSO</param>
		/// <param name="startTime">time of earliest QSO</param>
		/// <param name="endDate">date of latest QSO</param>
		/// <param name="endTime">time of latest QSO</param>
		public void GetStartEndDatesTimes(out string startDate, out string startTime,
		                                 out string endDate, out string endTime)
		{
			if(this.Count > 0)		// if at least one QSO, get dates/times
			{
				DisplayQsos dispQsos = new DisplayQsos(this);
				dispQsos.SortQSOs(new DateTimeComparer());
				startDate = dispQsos.Items[0].Date;
				startTime = dispQsos.Items[0].Time;
				endDate = dispQsos.Items[dispQsos.Count - 1].Date;
				endTime = dispQsos.Items[dispQsos.Count - 1].Time;
			}
			else					// no QSOs, so set to empty strings
			{
				startDate = string.Empty;
				startTime = string.Empty;
				endDate = string.Empty;
				endTime = string.Empty;
			}
		}
		
		/// <summary>
		/// Update each QSO with the contents of the corresponding QsoWithInclude.Manager property
		/// </summary>
		public void UpdateQSOsWithManager()
		{
			qsos2.Clear();
			foreach(QsoWithInclude qwi in this)
			{
				qwi.UpdateManager();
				qsos2.Add(qwi.Qso);
			}
		}
		
		/// <summary>
		/// Remove all QSOs
		/// </summary>
		public new void Clear()
		{
			base.Clear();
            IsDirty = false;
            NeedsSorting = false;
		}
		
		/// <summary>
		/// Set the Include property of all the Qsos in a list
		/// </summary>
		/// <param name="list">List of all Qsos to set the Include property on</param>
		/// <param name="check">Value to set Include to</param>
		public void SetIncludesChecked(System.Collections.IList list, bool check)
		{
			// set the Include property for each Qso
			foreach(QsoWithInclude qwi in list)
			{
				qwi.Include = check;
			}
			// necessary to remove and add all Qsos so that display is updated.
			List<QsoWithInclude> qsos = this.ToList();
			bool dirty = IsDirty;
			this.Clear();
			foreach(QsoWithInclude qso in qsos)
			{
				this.Add(qso);
			}
			IsDirty = dirty;
		}
		
		/// <summary>
		/// Create lists of DispQsos for printing on cards
		/// </summary>
		/// <param name="card">Card object that will be printed</param>
		/// <returns>A List of DispQsos to be displayed on the cards</returns>
		public List<List<DispQso>> GetDispQsosList(CardWF card)
		{
			List<DispQso> cardQsos = null;
			List<List<DispQso>> cardsQsosList = new List<List<DispQso>>();
			string thisManagerCall = string.Empty;
			string error = string.Empty;
			QsoWithInclude thisQso = new QsoWithInclude(new Qso2(string.Empty, App.AdifEnums,
			                                                     ref error, null));
			int qsosCount = 0;
			HashSet<string> fields = new HashSet<string>();
			HashSet<string> existFields = new HashSet<string>();
			card.GetAdifFieldsForSorting(ref fields, ref existFields);
			foreach(QsoWithInclude qwi in this)
			{
				string manager = qwi.Manager;
				if(!Call.IsValid(manager))
				{
					manager = string.Empty;
				}
				string managerCall = manager + qwi.Callsign;
				if(qwi.Include)
				{
					if(managerCall == thisManagerCall &&
					   card.QsosBox != null &&
					   qsosCount < card.QsosBox.MaximumQsos &&
					   card.QsosBox != null &&
					   AdifFieldsEqual(fields, qwi, thisQso) &&
					   AdifFieldsExist(existFields, qwi, thisQso))
					{
						cardQsos.Add(new DispQso(qwi));
						qsosCount++;
					}
					else
					{
						cardQsos = new List<DispQso>();
						thisManagerCall = managerCall;
						thisQso = qwi;
						cardQsos.Add(new DispQso(qwi));
						qsosCount = 1;
						cardsQsosList.Add(cardQsos);
					}
				}
			}
			return cardsQsosList;
		}
		
		/// <summary>
		/// Determine if Adif fields are equal for two Qsos
		/// </summary>
		/// <param name="fields">Adif Fields to compare</param>
		/// <param name="q1">First Qso in the comparison</param>
		/// <param name="q2">Second Qso in the comparison</param>
		/// <returns>True if all Adif fields in the list are equal for the two Qsos. Equality
		/// means same values in both Qsos, or no value in both Qsos.
		/// False otherwise.</returns>
		public bool AdifFieldsEqual(HashSet<string> fields, QsoWithInclude q1, QsoWithInclude q2)
		{
			foreach(string field in fields)
			{
				if(q1.Qso[field] != null)
				{
					if(q2.Qso[field] == null)
					{
						return false;
					}
					else if(q1.Qso[field] != q2.Qso[field])
					{
						return false;
					}
				}
				else if(q2.Qso[field] != null)
				{
					return false;
				}
			}
			return true;
		}
		
		/// <summary>
		/// Determine if Adif Fields exist in both Qsos
		/// </summary>
		/// <param name="fields">Adif Fields to determine existence of</param>
		/// <param name="q1">First Qso to compare</param>
		/// <param name="q2">Second Qso to compare</param>
		/// <returns>True of both Qsos contains each field or neither Qso contains each field.
		/// False if one Qso contains at least one field that the other does not.</returns>
		private bool AdifFieldsExist(HashSet<string> fields, QsoWithInclude q1, QsoWithInclude q2)
		{
			foreach(string field in fields)
			{
				if(q1.Qso[field] != null)
				{
					if(q2.Qso[field] == null)
					{
						return false;
					}
				}
				else if(q2.Qso[field] != null)
				{
					return false;
				}
			}
			return true;
		}
		
		/// <summary>
		/// Add a Qso
		/// </summary>
		/// <param name="qwi">QsoWithInclude object containing the Qso to be added.</param>
		public void AddQso(QsoWithInclude qwi)
		{
			qwi.Qso.Qsos = qsos2;
			qsos2.Add(qwi.Qso);
			this.Add(qwi);
			this.IsDirty = true;
			this.NeedsSorting = true;
		}
	}
}
