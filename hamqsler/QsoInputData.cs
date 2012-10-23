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
using System;
using System.ComponentModel;
using System.Windows;

namespace hamqsler
{
	/// <summary>
	/// Temporary storage for QSO data entered into the QsoInputDialog.
	/// Used to validate values input into the dialog.
	/// </summary>
	public class QsoInputData : DependencyObject, IDataErrorInfo
	{
		private static readonly DependencyProperty CallsignProperty = 
			DependencyProperty.Register("Callsign", typeof(string), typeof(QsoInputData),
			                            new PropertyMetadata(string.Empty));
		public string Callsign
		{
			get {return (string)GetValue(CallsignProperty);}
			set {SetValue(CallsignProperty, value);}
		}
		
		private static readonly DependencyProperty ManagerProperty =
			DependencyProperty.Register("Manager", typeof(string), typeof(QsoInputData),
			                            new PropertyMetadata(string.Empty));
		public string Manager
		{
			get {return (string)GetValue(ManagerProperty);}
			set {SetValue(ManagerProperty, value);}
		}
		
		private static readonly DependencyProperty StartDateProperty = 
			DependencyProperty.Register("StartDate", typeof(string), typeof(QsoInputData),
			                            new PropertyMetadata(string.Empty));
		public string StartDate
		{
			get {return (string)GetValue(StartDateProperty);}
			set {SetValue(StartDateProperty, value);}
		}
		
		private static readonly DependencyProperty StartTimeProperty =
			DependencyProperty.Register("StartTime", typeof(string), typeof(QsoInputData),
			                            new PropertyMetadata(string.Empty));
		public string StartTime
		{
			get {return (string)GetValue(StartTimeProperty);}
			set {SetValue(StartDateProperty, value);}
		}
		
		private static readonly DependencyProperty ModeProperty =
			DependencyProperty.Register("Mode", typeof(string), typeof(QsoInputData),
			                            new PropertyMetadata(string.Empty));
		public string Mode
		{
			get {return (string)GetValue(ModeProperty);}
			set {SetValue(ModeProperty, value);}
		}
		
		private static readonly DependencyProperty RSTProperty =
			DependencyProperty.Register("RST", typeof(string), typeof(QsoInputData),
			                            new PropertyMetadata(string.Empty));
		public string RST
		{
			get {return (string)GetValue(RSTProperty);}
			set {SetValue(RSTProperty, value);}
		}
		
		private static readonly DependencyProperty BandProperty =
			DependencyProperty.Register("Band", typeof(string), typeof(QsoInputData),
			                            new PropertyMetadata(string.Empty));
		public string Band
		{
			get {return (string)GetValue(BandProperty);}
			set {SetValue(BandProperty, value);}
		}
		
		private static readonly DependencyProperty FrequencyProperty =
			DependencyProperty.Register("Frequency", typeof(string), typeof(QsoInputData),
			                            new PropertyMetadata(string.Empty));
		public string Frequency
		{
			get {return (string)GetValue(FrequencyProperty);}
			set {SetValue(FrequencyProperty, value);}
		}
		
		private static readonly DependencyProperty RcvdProperty =
			DependencyProperty.Register("Rcvd", typeof(string), typeof(QsoInputData),
			                            new PropertyMetadata(string.Empty));
		public string Rcvd
		{
			get {return (string)GetValue(RcvdProperty);}
			set {SetValue(RcvdProperty, value);}
		}
		
		private static readonly DependencyProperty SentProperty =
			DependencyProperty.Register("Sent", typeof(string), typeof(QsoInputData),
			                            new PropertyMetadata(string.Empty));
		public string Sent
		{
			get {return (string)GetValue(SentProperty);}
			set {SetValue(SentProperty, value);}
		}
		
		private static readonly DependencyProperty SentViaProperty =
			DependencyProperty.Register("SentVia", typeof(string), typeof(QsoInputData),
			                            new PropertyMetadata(string.Empty));
		private string SentVia
		{
			get {return (string)GetValue(SentViaProperty);}
			set {SetValue(SentViaProperty, value);}
		}
		
        // property required by IDataErrorInfo interface, but not used by WPF
        public string Error
        {
        	get {return null;}
        }

        /// <summary>
		/// Error handling for IDataErrorInfo interface
		/// </summary>
		public string this[string propertyName]
		{
			get
			{
				return null;
			}
		}
	}
}
