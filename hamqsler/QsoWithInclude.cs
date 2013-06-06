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
using System.ComponentModel;
using Qsos;

namespace hamqsler
{
	/// <summary>
	/// QSO object adjusted for display in a QsosView
	/// </summary>
	public class QsoWithInclude : IDataErrorInfo
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
			get 
			{
				string retValue = string.Empty;
				switch(rcvd)
				{
					case "Y":
						retValue = "Yes";
						break;
					case "N":
						retValue = "No";
						break;
					case "R":
						retValue = "Requested";
						break;
					case "V":
						retValue = "Verified";
						break;
					case "I":
						retValue = "Ignore";
						break;
					case "":
						retValue = string.Empty;
						break;
					default:
						retValue = "***";
						break;
				}
				return retValue;
			}
		}
		
		private string sendVia;
		public string SendVia
		{
			get 
			{
				string retValue = string.Empty;
				switch(sendVia)
				{
					case "B":
						retValue = "Bureau";
						break;
					case "D":
						retValue = "Direct";
						break;
					case "E":
						retValue = "Electronic";
						break;
					case "M":
						retValue = "Manager";
						break;
					case "":
						retValue = "Bureau";
						break;
					default:
						retValue = "Bureau";
						break;
				}
				string call = Callsign;
				if(!Manager.Equals(string.Empty))
				{
					call = Manager;
				}
				string buro = QslBureaus.QslBureaus.Bureau(call);
				if(retValue.Equals("Bureau") && buro.Equals(QslBureaus.QslBureaus.NoBureau))
				{
					retValue = "Direct";
				}
				return retValue;
			}
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
			get 
			{
				// assume manager is valid
				string mcdt = manager + "-" + callsign + date + time;
				if(manager != string.Empty)
				{
					try
					{
						// now check if manager is valid and modify return value if not
						CallSign mgr = new CallSign(Manager);
						if(mgr.FullCall != mgr.Call)
						{
							mcdt = "-" + callsign + date + time;
						}
					}
					catch(QsoException)		// thrown if manager not valid callsign
					{
						mcdt = "-" + callsign + date + time;
					}
				}
				return mcdt;
			}
		}
			
		public string BureauManagerCallDateTime
		{
			get {string bmcdt = (SendVia.Equals("Bureau") &&
			                     !bureau.Equals(QslBureaus.QslBureaus.NoBureau)) ?
			                     	bureau : QslBureaus.QslBureaus.NoBureau;
				return bmcdt + ManagerCallDateTime;}
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
			callsign = q.getValue("call", string.Empty);
			manager = q.getValue("qsl_via", string.Empty);
			date = q.getValue("qso_date", string.Empty);
			time = q.getValue("time_on", null);
			if(time == null)
			{
				time = q.getValue("time_off", string.Empty);
			}
			band = q.getValue("band", string.Empty).ToLower();
			frequency = q.getValue("freq", string.Empty);
			mode = q.getValue("mode", string.Empty).ToUpper();
			rst = q.getValue("rst_sent", string.Empty);
			sent = q.getValue("qsl_sent", string.Empty).ToUpper();
			rcvd = q.getValue("qsl_rcvd", string.Empty).ToUpper();
			sendVia = q.getValue("qsl_sent_via", string.Empty).ToUpper();
			string mcall = (CallSign.IsValid(manager) ? manager : callsign);
			bureau = QslBureaus.QslBureaus.Bureau(mcall);
			qso = q;
		}
		
		/// <summary>
		/// Error accessor from the IDataErrorInfo interface. This is not used.
		/// </summary>
		public string Error
		{
			get
			{
				return null;
			}
		}
		
		/// <summary>
		/// Validator from the IDataErrorInfo interface
		/// </summary>
		public string this[string propertyName]
		{
			get
			{
				if(propertyName == "Manager")
				{
					if(Manager == string.Empty)
					{
						return null;
					}
					CallSign mgr;
					try
					{
						mgr = new CallSign(Manager);
					}
					catch(QsoException)
					{
						return "Must either be empty or a valid callsign";
					}
					if(mgr.FullCall == mgr.Call)
					{
						return null;
					}
					else
					{
						return "Must either be empty or a valid callsign";
					}
				}
				return null;
			}
		}
		
		/// <summary>
		/// Update the 'qsl_via field of the QSO with the value in Manager
		/// </summary>
		public void UpdateManager()
		{
			this.Qso.setField("qsl_via", Manager);
		}
	}
}
