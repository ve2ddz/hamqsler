//  
//  HamBands.cs
//  
//  Author:
//       Jim <jim@va3hj.ca>
// 
//  Copyright (c) 2011 VA3HJ Software
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qsos
{
    /// <summary>
    /// static class that defines all ham bands
    /// </summary>
    public static class HamBands
    {
        static private HamBand[] bands = {new HamBand("2190m", .136F, .137F),
                              new HamBand("560m", 0.501F, 0.504F),
                              new HamBand("160m", 1.8F, 2.0F),
                              new HamBand("80m", 3.5F, 4.0F),
                              new HamBand("60m", 5.102F, 5.404F),
                              new HamBand("40m", 7.0F, 7.3F),
                              new HamBand("30m", 10.1F, 10.15F),
                              new HamBand("20m", 14.0F, 14.35F),
                              new HamBand("17m", 18.068F, 18.168F),
                              new HamBand("15m", 21.0F, 21.45F),
                              new HamBand("12m", 24.89F, 24.99F),
                              new HamBand("10m", 28.0F, 29.7F),
                              new HamBand("6m", 50.0F, 54.0F),
                              new HamBand("4m", 70.0F, 71.0F),
                              new HamBand("2m", 144.0F, 148.0F),
                              new HamBand("1.25m", 222.0F, 225.0F),
                              new HamBand("70cm", 420.0F, 450.0F),
                              new HamBand("33cm", 902.0F, 928.0F),
                              new HamBand("23cm", 1240.0F, 1300.0F),
                              new HamBand("13cm", 2300.0F, 2450.0F),
                              new HamBand("9cm", 3300.0F, 3500.0F),
                              new HamBand("6cm", 5650.0F, 5925.0F),
                              new HamBand("3cm", 10000.0F, 10500.0F),
                              new HamBand("1.25cm", 24000.0F, 24250.0F),
                              new HamBand("6mm", 47000.0F, 47200.0F),
                              new HamBand("4mm", 75500.0F, 81000.0F),
                              new HamBand("2.5mm", 119980.0F, 120020.0F),
                              new HamBand("2mm", 142000.0F, 149000.0F),
                              new HamBand("1mm", 241000.0F, 250000.0F)
                          };

        /// <summary>
        /// searches the ham bands for a match
        /// </summary>
        /// <param name="b">the band to look for</param>
        /// <returns>
        /// HamBand instance that matches the requested band
        /// </returns>
        public static HamBand getHamBand(string b)
        {
            foreach(HamBand band in bands)
            {
                if(band.Band == b)
                {
                    return band;
                }
            }
            throw new QsoException(b + " is not an ADIF enumerated ham band.");
        }

        /// <summary>
        /// searches for the ham band that contains the input frequency
        /// </summary>
        /// <param name="freq">frequency in MHz</param>
        /// <returns>
        /// HamBand instance that contains the input frequency
        /// 
        /// </returns>
        public static HamBand getHamBand(float freq)
        {
            foreach (HamBand band in bands)
            {
                if (band.IsThisBand(freq))
                {
                    return band;
                }
            }
            throw new QsoException("Frequency " + string.Format("{0}", freq)  + " MHz is not within an ADIF enumerated ham band.");
        }
    }
}
