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
 using QslBureaus;
 using Qsos;
using System;

namespace hamqsler
{
	/// <summary>
	/// QSO object adjusted for display in a QsosView
	/// </summary>
	public class QsoWithInclude
	{
		private bool include;
		public bool Include
		{
			get {return include;}
			set {include = value;}
		}
		
		private string callsign;
		public string Callsign
		{
			get {return callsign;}
		}
		
		private string manager;
		public string Manager
		{
			get {return manager;}
			set {manager = value;}
		}
		
		private string date;
		public string Date
		{
			get {return date;}
		}
		
		private string time;
		public string Time
		{
			get {return time;}
		}
		
		private string band;
		public string Band
		{
			get {return band;}
		}
		
		private string frequency;
		public string Frequency
		{
			get {return frequency;}
		}
		
		private string mode;
		public string Mode
		{
			get {return mode;}
		}
		
		private string rst;
		public string RST
		{
			get {return rst;}
		}
		
		private string sent;
		public string Sent
		{
			get 
			{
				string status;
				switch(sent)
				{
					case "Y":
						status = "Yes";
						break;
					case "N":
						status = "No";
						break;
					case "R":
						status = "Requested";
						break;
					case "Q":
						status = "Queued";
						break;
					case "I":
						status = "Ignore";
						break;
					default:
						status = sent;
						break;
				}
				return status;
			}
		}
		
		private string rcvd;
		public string Rcvd
		{
			get {return rcvd;}
		}
		
		private string sentVia;
		public string SentVia
		{
			get {return sentVia;}
		}
		
		private string bureau;
		public string Bureau
		{
			get {return bureau;}
		}
		
		public string DateTime
		{
			get {return date + time;}
		}
		
		public string ManagerCallDateTime
		{
			get {return manager + "-" + callsign + date + time;}
		}
			
		public string BureauManagerCallDateTime
		{
			get {string bmcdt = ((sentVia == string.Empty || sentVia == "B") &&
			                     !bureau.Equals(QslBureaus.QslBureaus.NoBureau)) ?
			                     	bureau : QslBureaus.QslBureaus.NoBureau;
				return bmcdt + ManagerCallDateTime;}
		}
		
		public string ViaBureau
		{
			get {return bureau != QslBureaus.QslBureaus.NoBureau && 
						(sentVia == string.Empty || sentVia == "B") 
					? "Y" : string.Empty;}
		}
		
		private Qso qso;
		public Qso Qso
		{
			get {return qso;}
		}

		/// <summary>
		/// Constructor - build a QsoWithInclude from a Qso object
		/// </summary>
		/// <param name="q">Qso object to build QsoWithInclude from</param>
		public QsoWithInclude(Qso q)
		{
			include = true;
			callsign = q.getValue("call");
			manager = q.getValue("qsl_via", string.Empty);
			date = q.getValue("qso_date");
			time = q.getValue("time_on", null);
			if(time == null)
			{
				time = q.getValue("time_off", string.Empty);
			}
			band = q.getValue("band", string.Empty).ToLower();
			frequency = q.getValue("freq", string.Empty);
			mode = q.getValue("mode").ToUpper();
			rst = q.getValue("rst", string.Empty);
			sent = q.getValue("qsl_sent", string.Empty).ToUpper();
			rcvd = q.getValue("qsl_rcvd", string.Empty).ToUpper();
			sentVia = q.getValue("qsl_sent_via", string.Empty).ToUpper();
			string mcall = (CallSign.IsValid(manager) ? manager : callsign);
			bureau = QslBureaus.QslBureaus.Bureau(mcall);
			qso = q;
		}
	}
}
