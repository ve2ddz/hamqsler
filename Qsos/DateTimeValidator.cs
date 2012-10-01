// //  
// //  Author:
// //       Jim <jim@va3hj.ca>
// // 
// //  Copyright (c) 2011 VA3HJ Software
// // 
// //  This program is free software: you can redistribute it and/or modify
// //  it under the terms of the GNU General Public License as published by
// //  the Free Software Foundation, either version 3 of the License, or
// //  (at your option) any later version.
// // 
// //  This program is distributed in the hope that it will be useful,
// //  but WITHOUT ANY WARRANTY; without even the implied warranty of
// //  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// //  GNU General Public License for more details.
// // 
// //  You should have received a copy of the GNU General Public License
// //  along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
namespace Qsos
{
	public static class DateTimeValidator
	{
		/// <summary>
		/// Validate date
		/// </summary>
		/// <param name="date">
		/// Date in format YYYYMMDD
		/// </param>
		/// <returns>
		/// A <see cref="System.Boolean"/>
		/// </returns>
		public static bool DateIsValid(string date)
		{
			Regex reg8 = new Regex("^[0-9]{8}$");
			if(!reg8.IsMatch(date))
				return false;
			string sYear = date.Substring(0, 4);
			string sMonth = date.Substring(4, 2);
			string sDay = date.Substring(6, 2);
			int year = Int32.Parse(sYear);
			int month = Int32.Parse(sMonth);
			int day = Int32.Parse(sDay);
			if(date.CompareTo("19451101") < 0)
				return false;
			if(month < 1)
				return false;
			if(month > 12)
				return false;
			if(day < 1)
				return false;
			if(day > DateTime.DaysInMonth(year, month))
				return false;
			return true;
		}
		
		public static bool TimeIsValid(string time)
		{
			Regex reg4 = new Regex("^[0-9]{4}$");
			Regex reg6 = new Regex("^[0-9]{6}$");
			if(!reg4.IsMatch(time) && !reg6.IsMatch(time))
				return false;
			string sHour = time.Substring(0, 2);
			string sMinute = time.Substring(2, 2);
			int hour = Int32.Parse(sHour);
			int minute = Int32.Parse(sMinute);
			if(hour > 23)
				return false;
			if(minute > 59)
				return false;
			if(time.Length == 6)
			{
				string sSecond = time.Substring(4, 2);
				int second = Int32.Parse(sSecond);
				if(second > 59)
					return false;
			}
			return true;
		}
	}
}

