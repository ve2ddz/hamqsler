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
using System.Windows;

namespace hamqsler
{
	/// <summary>
	/// Description of StartEndDateTime.
	/// </summary>
	public class StartEndDateTime : DependencyObject, IDataErrorInfo
	{
		private static readonly DependencyProperty StartDateProperty =
			DependencyProperty.Register("StartDate", typeof(string), typeof(StartEndDateTime),
			                            new PropertyMetadata(string.Empty));
		public string StartDate
		{
			get {return (string)GetValue(StartDateProperty);}
			set {SetValue(StartDateProperty, value);}
		}
		public string ValidStartDate
		{
			get 
			{
				DateField df = new DateField(StartDate);
				string err = string.Empty;
				string mod = string.Empty;
				DateTime dt = DateTime.UtcNow;
				string today = string.Format("{0:d4}{1:d2}{2:d2}", dt.Year, dt.Month, dt.Day);
				if(today.CompareTo(StartDate) < 0)
				{
					return today;
				}
				return StartDate != string.Empty && df.Validate(out err, out mod) ?
					StartDate : "19300101";
			}
		}
			                            
		private static readonly DependencyProperty StartTimeProperty =
			DependencyProperty.Register("StartTime", typeof(string), typeof(StartEndDateTime),
			                            new PropertyMetadata(string.Empty));
		public string StartTime
		{
			get {return (string)GetValue(StartTimeProperty);}
			set {SetValue(StartTimeProperty, value);}			
		}
		public string ValidStartTime
		{
			get 
			{
				TimeField time = new TimeField(StartTime);
				string err = string.Empty;
				string mod = string.Empty;
				return StartTime != string.Empty && time.Validate(out err, out mod) ?
					StartTime : "0000";
			}
		}

		private static readonly DependencyProperty EndDateProperty =
			DependencyProperty.Register("EndDate", typeof(string), typeof(StartEndDateTime),
			                            new PropertyMetadata(string.Empty));
		public string EndDate
		{
			get {return (string)GetValue(EndDateProperty);}
			set {SetValue(EndDateProperty, value);}
		}
		public string ValidEndDate
		{
			get 
			{
				DateField df = new DateField(EndDate);
				string err = string.Empty;
				string mod = string.Empty;
				DateTime dt = DateTime.UtcNow;
				string today = string.Format("{0:d4}{1:d2}{2:d2}", dt.Year, dt.Month, dt.Day);
				if(today.CompareTo(EndDate) < 0)
				{
					return today;
				}
				return EndDate != string.Empty && df.Validate(out err, out mod) ?
					EndDate : "19300101";
			}
		}
		
		private static readonly DependencyProperty EndTimeProperty =
			DependencyProperty.Register("EndTime", typeof(string), typeof(StartEndDateTime),
			                            new PropertyMetadata(string.Empty));
		public string EndTime
		{
			get {return (string)GetValue(EndTimeProperty);}
			set {SetValue(EndTimeProperty, value);}
		}
		public string ValidEndTime
		{
			get 
			{
				TimeField time = new TimeField(EndTime);
				string err = string.Empty;
				string mod = string.Empty;
				return EndTime != string.Empty && time.Validate(out err, out mod) ?
					EndTime : "0000";
			}			
		}
		
		/// <summary>
		/// Error accessor for IDateErrorInfo interface (not used in WPF)
		/// </summary>
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
				if(propertyName == "StartDate")
			    {
					return ValidateStartDate();
			    }
				else if(propertyName == "StartTime")
				{
					return ValidateStartTime();
				}
				else if(propertyName == "EndDate")
				{
					return ValidateEndDate();
				}
				else if(propertyName == "EndTime")
				{
					return ValidateEndTime();
				}
				return null;
			}
		}
		
		/// <summary>
		/// Helper method that validates the start date
		/// </summary>
		/// <returns>validation string (error string or null if no error)</returns>
		private string ValidateStartDate()
		{
			DateField df = new DateField(StartDate);
			string err = string.Empty;
			string mod = string.Empty;
			if(!df.Validate(out err, out mod))
		    {
		   		return err;
		    }
			return null;
		}
		
		/// <summary>
		/// Helper method that validates the start time
		/// </summary>
		/// <returns>validation string (error string or null if no error)</returns>
		private string ValidateStartTime()
		{
			TimeField time = new TimeField(EndTime);
			string err = string.Empty;
			string mod = string.Empty;
			if(!time.Validate(out err, out mod))
			{
				return err;
			}
			return null;
		}
		
		/// <summary>
		/// Helper method that validates the end date
		/// </summary>
		/// <returns>validation string (error string or null if no error)</returns>
		private string ValidateEndDate()
		{
			DateField df = new DateField(EndDate);
			string err = string.Empty;
			string mod = string.Empty;
			if(!df.Validate(out err, out mod))
		    {
		   		return err;
		    }
			return null;
		}
		
		/// <summary>
		/// Helper method that validates the end time
		/// </summary>
		/// <returns>validation string (error string or null if no error)</returns>
		private string ValidateEndTime()
		{
			TimeField time = new TimeField(EndTime);
			string err = string.Empty;
			string mod = string.Empty;
			if(!time.Validate(out err, out mod))
			{
				return err;
			}
			return null;
		}
		
	}
}
