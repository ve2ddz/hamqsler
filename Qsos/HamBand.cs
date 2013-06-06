//  
//  HamBand.cs
//  
//  Author:
//       Jim <jimorcheson@gmail.com>
// 
//  Copyright (c) 2011 VA3HJ Software
//  Copyright (c) 2013 Jim Orcheson
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
using System.Linq;

namespace Qsos
{

    /// <summary>
    /// HamBand class encapsulates the band name and the lower and upper frequencies
    /// of the band
    /// </summary>
    public class HamBand
    {
        private string band;
        private float lowerEdge;
        private float upperEdge;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="b">name of the band - should be as defined in ADIF standard</param>
        /// <param name="lEdge">lowest frequency of the band (in MHz) as defined in ADIF standard</param>
        /// <param name="uEdge">highest frequency of the band (in MHz) as defined in ADIF standard</param>
        /// <exception>
        /// QsoException if lower edge frequency is larger than or equal to upper edge frequency
        /// </exception>
        internal HamBand(string b, float lEdge, float uEdge)
        {
            if (lEdge >= uEdge)
            {
                QsoException ex = new QsoException("Invalid HamBand constructor paramaters");
                ex.Data.Add("Band", b);
                ex.Data.Add("Lower Edge", lEdge);
                ex.Data.Add("Upper Edge", uEdge);
                throw ex;
            }
            band = b;
            lowerEdge = lEdge;
            upperEdge = uEdge;
        }

        /// <summary>
        /// readonly band property
        /// </summary>
        public string Band
        {
            get { return band; }
        }


        /// <summary>
        /// readonly lower band edge frequency
        /// </summary>
        public float LowerEdge
        {
            get { return lowerEdge; }
        }


        /// <summary>
        /// readonly upper band edge frequency
        /// </summary>
        public float UpperEdge
        {
            get { return upperEdge; }
        }

        /// <summary>
        /// determines if the input frequency is within this HamBand
        /// </summary>
        /// <param name="freq">frequency (in MHz) to test if within this band</param>
        /// <returns>
        /// true if input param is between lower and upper band edges
        /// </returns>
        public bool IsThisBand(float freq)
        {
            if (freq >= lowerEdge && freq <= upperEdge)
            {
                return true;
            }
            return false;
        }

    }
}
