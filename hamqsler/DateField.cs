/*
 *  Author:
 *       Jim Orcheson <jimorcheson@gmail.com>
 * 
 *  Copyright (c) 2013 Jim Orcheson
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
using System.Text.RegularExpressions;

namespace hamqsler
{
	/// <summary>
	/// DateField class - base class for classes that are of type Date
	/// </summary>
	public class DateField : StringField
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="date">Date in format YYYYMMDD</param>
		public DateField(string date) : base(date)
		{
		}
		
		/// <summary>
		/// Validate the value of the date field (between 19300101 and today)
		/// </summary>
		/// <param name="err">Error message to return if date not valid</param>
		/// <returns>True if date is valid, false if not valid</returns>
		public override bool Validate(out string err)
		{
			err = null;
			if(Value.Length != 8)
			{
				err = "Date must be exactly 8 characters long";
				return false;
			}
			if(!Regex.IsMatch(Value, "^[0-9]+$"))
			{
				err = "Date must contain number characters only";
				return false;
			}
			string year = Value.Substring(0, 4);
			if(year.CompareTo("1930") < 0)
			{
				err = "Date must be 19300101 or later";
				return false;
			}
			DateTime now = DateTime.UtcNow;
			string strNow = string.Format("{0:yyyyMMdd}", now);
			if(Value.CompareTo(strNow) > 0)
			{
				err = "Date must not be later than today";
				return false;
			}
			string month = Value.Substring(4, 2);
			if(month.CompareTo("01") < 0 || month.CompareTo("12") > 0)
			{
				err = "Invalid month in date: must be between 01 and 12";
				return false;
			}
			string day = Value.Substring(6, 2);
			int lastDay = DateTime.DaysInMonth(int.Parse(year), int.Parse(month));
			if(day.CompareTo("01") < 0 || day.CompareTo(lastDay.ToString()) > 0)
			{
				err = "Invalid day in date: must be between 01 and end of month";
				return false;
			}
			return true;
		}
	}
}
