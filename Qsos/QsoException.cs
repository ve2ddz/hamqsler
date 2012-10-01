//  
//  QsoException.cs
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
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Qsos
{

/// <summary>
/// QsoException is the exception thrown for errors detected by classes in the Qsos library.
/// </summary>
    public class QsoException: Exception
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="eStr">The text string describing the error that caused the exception.</param>
        public QsoException(string eStr)
            : base(eStr)
        { }

        public QsoException(string eStr, Exception e)
            : base(eStr, e)
        { }
    }
}
