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
using System.Collections.Generic;

namespace hamqsler
{
	/// <summary>
	/// CreditList class - collection of Credit objects representing an ADIF CreditList data type
	/// </summary>
	public class CreditList : AdifField
	{
		private List<Credit> creditList = new List<Credit>();
		
		public int Count
		{
			get {return creditList.Count;}
		}
		
		/// <summary>
		/// Default constructor
		/// </summary>
		public CreditList()
		{
		}
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="credits">string containing credits informatin</param>
		/// <param name="aEnums">AdifEnumerations object containing the Award, Credit, and
		/// QSL Medium enumerations</param>
		public CreditList(string credits, AdifEnumerations aEnums)
		{
			string[] creds = credits.Split(',');
			foreach(string credit in creds)
			{
				Credit c = new Credit(credit, aEnums);
				this.Add(c);
			}
		}
		
		/// <summary>
		/// Retrieve the list of Credit objects that have the specified credit name
		/// </summary>
		/// <param name="creditName">Name of the credit to retrieve list for</param>
		/// <returns>List of Credits containing the specified name</returns>
		public List<Credit> GetCredits(string creditName)
		{
			List<Credit> credits = new List<Credit>();
			foreach(Credit c in creditList)
			{
				if(c.CreditName.Equals(creditName))
				{
					credits.Add(c);
				}
			}
			return credits;
		}
		
		/// <summary>
		/// Add credit to credit list. If a Credit object of same name already exists with media,
		/// just add the media
		/// </summary>
		/// <param name="item">Credit object to add</param>
		public void Add(Credit item)
		{
			List<Credit> credits = GetCredits(item.CreditName);
			if(credits.Count == 0)
			{
				creditList.Add(item);
				return;
			}
			string[] itemMedia = new string[item.Media.Count];
			item.Media.CopyTo(itemMedia);
			foreach(Credit credit in credits)
			{
				string[] media = new string[credit.Media.Count];
				credit.Media.CopyTo(media);
				if(itemMedia.Length == 1 && itemMedia[0] == null && media.Length == 1 && media[0] == null)
				{
					return;
				}
				else if(itemMedia[0] != null && media[0] != null)
				{
					foreach(string it in itemMedia)
					{
						credit.Media.Add(it);
					}
					return;
				}
			}
			creditList.Add(item);
		}
		
		/// <summary>
		/// Convert list to an Adif string
		/// </summary>
		/// <returns>string containing creditlist info in ADIF format</returns>
		public string ToAdifString()
		{
			string adif = string.Empty;
			foreach(Credit credit in creditList)
			{
				adif += credit.ToString() + ",";
			}
			if(adif.Length != 0)
			{
				adif = adif.Substring(0, adif.Length -1);
			}
			return "<" + Name + ":" + adif.Length + ">" + adif;
		}
	}
}
