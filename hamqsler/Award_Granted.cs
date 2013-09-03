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

namespace hamqsler
{
	/// <summary>
	/// Award_Granted class - the list of awards granted by a sponsor.
	/// This field might not be used in a QSO record. It is included here
	/// for completeness
	/// </summary>
	public class Award_Granted : DelimitedListEnumeration
	{
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="awardsList">lit of awards that have been granted</param>
		public Award_Granted(string awardsList, AdifEnumerations aEnums) : 
			base(',', awardsList, "Sponsored_Award", aEnums)
		{
		}
		
		/// <summary>
		/// Validate the awards list
		/// </summary>
		/// <param name="error">Error message if the SPONSOR portion of at least one award is not
		/// in the SponsoredAward enumeration.</param>
		/// <returns>true if no awards or SPONSOR portion of all awards are in SponsoredAward
		/// enumeration, false if SPONSOR portion of at least one award is not in SponsoredAward
		/// enumberation.</returns>
		public override bool Validate(out string error)
		{
			error = string.Empty;
			foreach(string award in DelimList.Items)
			{
				string[] parts = award.Split('_');
				string sponsor = parts[0] + "_";
				if(!base.Validate(sponsor, out error))
				{
					error = string.Format("The sponsors portion of Awards_Granted is an enumeration." +
					    Environment.NewLine +
						"The value '{0}' was not found in enumeration", sponsor);
					return false;
				}
			}
			return true;
		}
	}
}
