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
				string startDate = StartDate;
				return startDate != string.Empty && 
			                 DateTimeValidator.DateIsValid(startDate) ?
					startDate : "19451101";
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
				string startTime = StartTime;
				return startTime != string.Empty &&
			                 DateTimeValidator.TimeIsValid(startTime) ?
					startTime : "0000";
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
				string endDate = EndDate;
				return endDate != string.Empty &&
			                  DateTimeValidator.DateIsValid(endDate) ?
					endDate : string.Format("{0:yyyyMMdd}", DateTime.UtcNow);
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
				string endTime = EndTime;
				if(endTime == string.Empty ||
				   !DateTimeValidator.TimeIsValid(endTime))
				{
					endTime = "235959";
				}
				DateTime now = DateTime.UtcNow;
				string nowDate = string.Format("{0:yyyyMMdd}", now);
				// there is an extremely tiny window in which nowDate is at the
				// very end of a day and ValidEndDate returns the next day,
				// so it is very important that we code the following test
				// with that in mind
				int compare = string.Compare(nowDate, ValidEndDate);
				if(compare <= 0)
				{
					endTime = string.Format("{0:HHmmss}", now);
				}
				return endTime;
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
			if(!DateTimeValidator.DateIsValid(StartDate))
		    {
		   		return "Invalid date: must be in format 'YYYYMMDD' and between 19451101 and today";
		    }
			return null;
		}
		
		/// <summary>
		/// Helper method that validates the start time
		/// </summary>
		/// <returns>validation string (error string or null if no error)</returns>
		private string ValidateStartTime()
		{
			if(!DateTimeValidator.TimeIsValid(StartTime))
			{
				return "Invalid time: must be in format HHMM or HHMMSS";
			}
			return null;
		}
		
		/// <summary>
		/// Helper method that validates the end date
		/// </summary>
		/// <returns>validation string (error string or null if no error)</returns>
		private string ValidateEndDate()
		{
			if(!DateTimeValidator.DateIsValid(EndDate))
		    {
		   		return "Invalid date: must be in format 'YYYYMMDD' and between 19451101 and today";
		    }
			return null;
		}
		
		/// <summary>
		/// Helper method that validates the end time
		/// </summary>
		/// <returns>validation string (error string or null if no error)</returns>
		private string ValidateEndTime()
		{
			if(!DateTimeValidator.TimeIsValid(EndTime))
			{
				return "Invalid time: must be in format HHMM or HHMMSS";
			}
			return null;
		}
		
	}
}
