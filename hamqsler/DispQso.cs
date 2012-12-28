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

namespace hamqsler
{
	/// <summary>
	/// Holder for Qso data to be displayed in QsosBox
	/// </summary>
	public class DispQso
	{
		private string manager = string.Empty;
		public string Manager
		{
			get {return manager;}
			set {manager = value;}
		}
		
		private string callsign = string.Empty;
		public string Callsign
		{
			get {return callsign;}
			set {callsign = value;}
		}
		
		private string date = string.Empty;
		public string Date
		{
			get {return date;}
			set {date = value;}
		}
		
		private string time = string.Empty;
		public string Time
		{
			get {return time;}
			set {time = value;}
		}
		
		private string band = string.Empty;
		public string Band
		{
			get {return band;}
			set {band = value;}
		}
		
		private string frequency = string.Empty;
		public string Frequency
		{
			get {return frequency;}
			set {frequency = value;}
		}
		
		private string mode = string.Empty;
		public string Mode
		{
			get {return mode;}
			set {mode = value;}
		}
		
		private string rst = string.Empty;
		public string RST
		{
			get {return rst;}
			set {rst = value;}
		}
		
		private string qsl = string.Empty;
		public string Qsl
		{
			get {return qsl;}
			set {qsl = value;}
		}
		
		private Qso qso = null;
		public Qso Qso
		{
			get {return qso;}
			set {qso =  value;}
		}
		
		/// <summary>
		/// Default Constructor
		/// </summary>
		public DispQso()
		{
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="qwi">QsoWithInclude object to build DispQso from</param>
		public DispQso(QsoWithInclude qwi)
		{
			Manager = qwi.Manager;
			Callsign = qwi.Callsign;
			Date = qwi.Date;
			Time = qwi.Time;
			Band = qwi.Band;
			Frequency = qwi.Frequency;
			Mode = qwi.Mode;
			RST = qwi.RST;
			Qsl = qwi.Rcvd;
			Qso = qwi.Qso;
		}
	}
}
