﻿/*
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
	/// Credit_Submitted class - list of credits sought for this QSO
	/// </summary>
	public class Credit_Submitted : DelimitedListEnumeration
	{
		public Credit_Submitted(string credit, AdifEnumerations aEnums)
			: base(',', credit, "Credit", aEnums)
		{
		}
		
		/// <summary>
		/// Replace Award with Credit if there is an equivalent
		/// </summary>
		public void ReplaceAwardsWithCredits()
		{
			string [] credits = new string[DelimList.Count];
			DelimList.Items.CopyTo(credits, 0);
			foreach(string credit in credits)
			{
				bool inEnumeration = true;
				if(credit.IndexOf(":") == -1)
				{
					inEnumeration = aEnums.IsInEnumeration("Credit", credit);
				}
				else
				{
					inEnumeration = aEnums.IsInEnumeration("Credit", credit.Substring(0, credit.IndexOf(":")));
				}
				if(!inEnumeration)
				{
					string replacement = aEnums.GetCreditEquivalentForAward(credit);
					if(replacement == null)
					{
						DelimList.Items.Remove(credit);
					}
					else
					{
						DelimList.Items.Remove(credit);
						DelimList.Items.Add(replacement);
					}
				}
			}
		}
	}
}
