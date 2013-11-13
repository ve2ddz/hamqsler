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
using System.Reflection;
using NUnit.Framework;
using hamqsler;

namespace hamqslerTest
{
	// tests for UserPreferences class
	[TestFixture]
	public class UserPreferencesTests
	{
		UserPreferences prefs = null;
		Type prefsType = null;
		// testfixture setup
		[TestFixtureSetUp]
		public void FixtureSetup()
		{
			prefs = new UserPreferences();			
			prefsType = prefs.GetType();
		}
		
		// test IDataErrorInfo interface with valid Frequency
		[Test, Sequential]
		public void TestValidFrequency(
			[Values("Frequency2190m", "Frequency630m", "Frequency560m",
			        "Frequency160m", "Frequency80m", "Frequency60m",
			        "Frequency40m", "Frequency30m", "Frequency20m",
			        "Frequency17m", "Frequency15m", "Frequency12m",
			        "Frequency10m", "Frequency6m", "Frequency4m",
			        "Frequency2m", "Frequency1p25m", "Frequency70cm",
			        "Frequency33cm", "Frequency23cm", "Frequency13cm",
			        "Frequency9cm", "Frequency6cm", "Frequency3cm",
			        "Frequency1p25cm", "Frequency6mm", "Frequency4mm",
			        "Frequency2p5mm", "Frequency2mm", "Frequency1mm")] string property,
			[Values("0.136", "0.472", "0.501",
			        "1.8", "3.5", "5.2",
			        "7", "10.1", "14",
			        "18.1", "21", "24.9",
			        "28", "50", "70",
			        "144", "222", "420",
			        "902", "1240", "2300",
			        "3300", "5650", "10000",
			        "24000", "47000", "75500",
			        "119980", "142000", "241000")] string freq)
		{
			PropertyInfo prefsPropInfo = prefsType.GetProperty(property);
			prefsPropInfo.SetValue(prefs, freq, null);
			Assert.AreEqual(freq, prefsPropInfo.GetValue(prefs, null));
			string error = prefs[property];
			Assert.IsNull(error);
		}

		// test IDataErrorInfo interface with invalid Frequency
		[Test, Sequential]
		public void TestInvalidFrequency(
			[Values("Frequency2190m", "Frequency630m", "Frequency560m",
			        "Frequency160m", "Frequency80m", "Frequency60m",
			        "Frequency40m", "Frequency30m", "Frequency20m",
			        "Frequency17m", "Frequency15m", "Frequency12m",
			        "Frequency10m", "Frequency6m", "Frequency4m",
			        "Frequency2m", "Frequency1p25m", "Frequency70cm",
			        "Frequency33cm", "Frequency23cm", "Frequency13cm",
			        "Frequency9cm", "Frequency6cm", "Frequency3cm",
			        "Frequency1p25cm", "Frequency6mm", "Frequency4mm",
			        "Frequency2p5mm", "Frequency2mm", "Frequency1mm")] string property)
		{
			
			PropertyInfo prefsPropInfo = prefsType.GetProperty(property);
			prefsPropInfo.SetValue(prefs, "0.132", null);
			string error = prefs[property];
			string band = property.Substring(9);
			band = band.Replace('p', '.');
			string lower = string.Empty;
			string upper = string.Empty;
			App.AdifEnums.GetBandLimits(band, out lower, out upper);
			string errorString = string.Format("String must be between {0} and {1} MHz",
			                                   lower, upper);
			Assert.AreEqual(errorString, error);
		}
	}
}
