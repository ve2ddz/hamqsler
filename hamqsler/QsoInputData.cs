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
using System.ComponentModel;
using System.Globalization;
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
			set {SetValue(StartTimeProperty, value);}
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
		public string SentVia
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
		/// <returns>validation string (error string, or null if no error)</returns>
		public string this[string propertyName]
		{
			get
			{
				if(propertyName == "Callsign")
				{
					return ValidateCallsign();
				}
				else if(propertyName == "Manager")
				{
					return ValidateManager();
				}
				else if(propertyName == "StartDate")
				{
					return ValidateStartDate();
				}
				else if(propertyName == "StartTime")
				{
					return ValidateStartTime();
				}
				else if(propertyName == "Mode")
				{
					return ValidateMode();
				}
				else if(propertyName == "Band")
				{
					return ValidateBand();
				}
				else if(propertyName == "Frequency")
				{
					return ValidateFrequency();
				}
				return null;
			}
		}
		
		/// <summary>
		/// Helper method that validates a callsign
		/// </summary>
		/// <returns>validation string (error string or null if no error)</returns>
		private string ValidateCallsign()
		{
			CallSign call;
			try
			{
				call = new CallSign(Callsign);
			}
			catch(QsoException)
			{
				return "Not a valid callsign";
			}
			if(!CallSign.IsValid(call.Call))
			{
				return "Not a valid callsign";
			}
			return null;
		}
		
		/// <summary>
		/// Helper method that validates manager callsign
		/// </summary>
		/// <returns>validation string (error string or null if no error)</returns>
		private string ValidateManager()
		{
			if(Manager == string.Empty)
			{
				return null;
			}
			CallSign mgrCall;
			try
			{
				mgrCall = new CallSign(Manager);
			}
			catch (QsoException)
			{
				return "Not a valid callsign";
			}
			if(mgrCall.FullCall != mgrCall.Call)
			{
				return "Manager callsign must not contain modifiers (e.g. VA3HJ, not XE1/VA3HJ)";
			}
			if(!CallSign.IsValid(Manager))
			{
				return "Not a valid callsign";
			}
			return null;
		}
		
		/// <summary>
		/// Helper method that validates start date
		/// </summary>
		/// <returns>Validation string (error string, or null if no error)</returns>
		private string ValidateStartDate()
		{
			if(!DateTimeValidator.DateIsValid(StartDate))
			{
				return "Date is not valid. Must be between 19451101 and today";
			}
			return null;
		}
		
		/// <summary>
		/// Helper method that validates start time
		/// </summary>
		/// <returns>Validation string (error string, or null if no error)</returns>
		private string ValidateStartTime()
		{
			if(!DateTimeValidator.TimeIsValid(StartTime))
			{
				return "Time is not valid must be between 000000 and 235959, or 0000 and 2359";
			}
			return null;
		}
		
		/// <summary>
		/// Helper method that validates mode (cannot be string.Empty)
		/// </summary>
		/// <returns>Validation string (error string, or null if no error)</returns>
		private string ValidateMode()
		{
			if(Mode == string.Empty)
			{
				return "A mode must be specified";
			}
			return null;
		}
		
		/// <summary>
		/// Helper method that validates band
		/// </summary>
		/// <returns>Validation string (error string, or null if no error</returns>
		private string ValidateBand()
		{
			if(Frequency != string.Empty)
			{
				float freq;
				string frequency = Frequency.Replace(",", ".");
				float.TryParse(frequency, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out freq);
				HamBand hb;
				try
				{
					hb = HamBands.getHamBand(freq);
				}
				catch(QsoException)
				{
					return "Frequency is not within an enumerated ham band and therefore cannot " +
						"be validated against this Band setting";
				}
				if(!Band.Equals(string.Empty) && Band != hb.Band)
				{
					return "Band does not contain the specified frequency";
				}
			}
			return null;
		}
		
		/// <summary>
		/// Helper method that validates frequency
		/// </summary>
		/// <returns>Validation string (error string, or null if no error)</returns>
		private string ValidateFrequency()
		{
			if(Frequency == string.Empty)
			{
				return null;
			}
			float freq;
			string frequency = Frequency.Replace(",", ".");
			float.TryParse(frequency, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out freq);
			HamBand hb;
			try
			{
				hb = HamBands.getHamBand(freq);
			}
			catch(QsoException)
			{
				return "Frequency is not within an enumerated ham band";
			}
			if(Band != string.Empty && Band != hb.Band)
			{
				return "Frequency is not within the selected band";
			}
			return null;
		}
		
		/// <summary>
		/// Set all properties to empty string
		/// </summary>
		public void ClearQsoData()
		{
			Callsign = string.Empty;
			Manager = string.Empty;
			StartDate = string.Empty;
			StartTime = string.Empty;
			Mode = string.Empty;
			RST = string.Empty;
			Band = string.Empty;
			Frequency = string.Empty;
			Rcvd = string.Empty;
			Sent  =string.Empty;
			SentVia = string.Empty;
		}
			
	}
}
