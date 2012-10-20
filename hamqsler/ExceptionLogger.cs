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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Windows;

namespace hamqsler
{

	/// <summary>
	/// Class for handling the logging and display of exceptions
	/// </summary>
	public class ExceptionLogger : Qsos.Logger
    {
    	public const bool SHOWTRACE = true;
    	public const bool DONTSHOWTRACE = false;
    	public const bool SHOWMESSAGE = true;
    	public const bool DONTSHOWMESSAGE = false;
    	
        private Dictionary<string, bool> debugTypes = new Dictionary<string, bool>();
        private bool badLog = true;
        private FileInfo logInfo = null;
        private bool noWrite = false; 
        private FileStream loggerStream = null;

        /// <summary>
		/// Constructor
		/// </summary>
		/// <param name="logFileName">Name of the file to log messages to.</param>
        public ExceptionLogger(string logFileName)
        {
        	// TODO: Set debugTypes to valid names and values and consider a scheme for setting these at run time
            debugTypes.Add("ADIF", false);
            debugTypes.Add("logTrace", false);
            debugTypes.Add("QSOsel", false);
            debugTypes.Add("QSOprt", false);
            debugTypes.Add("CardDisplay", false);
            debugTypes.Add("ImageableArea", false);
            try
            {
                logInfo = new FileInfo(logFileName);
            }
            catch(SecurityException)
            {
                 ShowMessage("Security Exception: You do not have the required permission to access the log file.\r\n"
                            + "You may continue to use hamqsler, but no logging will be done");
                noWrite = true;
                return;
            }
            catch(UnauthorizedAccessException)
            {
                ShowMessage("Access Exception: Access to the log file is denied.\r\n"
                            + "You may continue to use hamqsler, but no logging will be done");
                noWrite = true;
                return;
            }
            catch(Exception e)
            {
                ShowMessage("Programming Error. Please contact the author with the contents of this message:\r\n"
                        + e.Message + "\r\n"
                        + "You may continue to use hamqsler, but no logging will be done");
                noWrite = true;
                return;
            }
            FileStream fS = logInfo.Create();
            // call to Create opens the filestream, so it must be closed
            // to prevent IO error later
            fS.Close();
        }

        /// <summary>
        /// Log a message based on debugType
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="debugType">debugType (determines whether to log the messsage or not.</param>
        public void Log(string message, string debugType)
        {
            if ((debugType == null || debugType == string.Empty) && badLog == true)
            {
                Exception e = new Exception("Null or empty debugType encountered.\r\n"
                        + "This is a programming error. Contact the author.");
                badLog = false;
                try
                {
                    throw e;
                }
                catch (Exception ex)
                {
                    Log(ex);
                    return;
                }
            }
            bool log;
            bool gotValue = debugTypes.TryGetValue(debugType, out log);
            if (!gotValue && badLog)
            {
                Exception e = new Exception("Invalid debugType encountered.\r\n"
                        + "This is a programming error. Contact the author.");
                badLog = false;
                Log(e);
                return;
            }
            if (log == true)
            {
                Log(message);
            }
        }

        /// <summary>
        /// Log a message always
        /// </summary>
        /// <param name="message">Message to log</param>
        public void Log(string message)
        {
            if (message == null || message == string.Empty || noWrite)
            {
                return;
            }
            if (!noWrite)
            {
                try
                {
                    loggerStream = logInfo.Open(FileMode.Append, FileAccess.Write);
                }
                catch (UnauthorizedAccessException)
                {
                    ShowMessage("Access Exception. The log file cannot be written to because the file is read only\r\n"
                            + "You may continue to use HamQSLer, but no logging will be done");
                    noWrite = true;
                    return;
                }
                catch (FileNotFoundException)
                {
                    ShowMessage("The logging file cannot be found. Did you delete a log file or the folder containing it?\r\n"
                            + "If not, there appears to be a programming error\r\n."
                            + "Please contact the author with this information.\r\n"
                            + "You may continue to use HamQSLer, but no logging will be done");
                    noWrite = true;
                    return;
                }
                catch(DirectoryNotFoundException)
                {
                    ShowMessage("Cannot open the logging file. Did you unmap the drive containing the logging file?\r\n"
                            + "If not, there appears to be a programming error\r\n."
                            + "Please contact the author with this information.\r\n"
                            + "You may continue to use HamQSLer, but no logging will be done");
                    noWrite = true;
                    return;
                }
                catch(IOException ioe)
                {
                    ShowMessage("IO error encountered trying to open the logging file:\r\n"
                            + ioe.Message + "\r\n"
                            + "You may continue to use HamQSLer, but no logging will be done");
                    noWrite = true;
                    return;
                }
                string mess = FormatMessage(message);
                try
                {
                    StreamWriter writer = new StreamWriter(loggerStream);
                    writer.Write(mess);
                    writer.Close();
                }
                catch (IOException e)
                {
                    ShowMessage("The following error occurred while trying to write to the log file:\r\n"
                            + e.Message + "\r\n"
                            + "You may continue to use HamQSLer, but no logging will be done");
                    noWrite = true;
                    return;
                }
                catch (Exception e)
                {
                    ShowMessage("Programming Error. Please contact the author with the contents of this message:\r\n"
                            + e.Message + "\r\n"
                                + "You may continue to use HamQSLer, but no logging will be done");
                    noWrite = true;
                    return;
                }
            }
        }

        /// <summary>
        /// Log and show an exception and its trace
        /// </summary>
        /// <param name="e">Exception to log</param>
        /// <param name="showTrace">Boolean indicating whether to log the exception's trace info</param>
        /// <param name="showMessage">Boolean indicating whether to display a message about the exception</param>
        public void Log(Exception e, bool showTrace=ExceptionLogger.SHOWTRACE, 
                        bool showMessage=ExceptionLogger.SHOWMESSAGE)
        {
            Log("Inside Log(Exception e", "logTrace");
            string msg = GetExceptionInfo(e, showTrace);
            Log("ExceptionInfo: " + msg, "logTrace");
            Log(msg);
            Log("Back from logging exceptionInfo", "logTrace");
            if (showMessage)
            {
                ShowMessage("HamQSLer encountered an error:\r\n" +
                        e.Message + "\r\n" +
                        "See the log file (Help->View Log File) for more information.",
						MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Retrieve text of the exception message and trace info
        /// </summary>
        /// <param name="e">Exception to retrieve info from</param>
        /// <param name="showTrace">Boolean indicating whether to retrieve the trace info</param>
        /// <returns></returns>
        private string GetExceptionInfo(Exception e, bool showTrace=SHOWTRACE)
        {
            Log("Inside GetExceptionInfo", "logTrace");
            string msg = e.GetType() + "\r\n";
            msg += "   " + e.Message + "\r\n";
            if (e.Data.Count > 0)
            {
                msg += "Data:\r\n";
                foreach (DictionaryEntry data in e.Data)
                {
                	// while there may be an entry in e.Data, the value may be null
                	if(data.Value != null)
                	{
	                    Log("data= " + data, "logTrace");
	                    DictionaryEntry pair = (DictionaryEntry) data;
	                    msg += "   " + pair.Key.ToString() + ": " + pair.Value.ToString() + "\r\n";
	                    Log("datastring= " + msg, "logTrace");
                	}
                }
            }
            msg += "Trace:\r\n";
            Log("Trace", "logTrace");
            if (showTrace)
            {
                string trace = e.StackTrace;
                if (trace != null)
                {
                    Log("Trace= " + trace + "\r", "logTrace");
                    int inIndex = trace.IndexOf("in ");
                    Log("inIndex= " + inIndex, "logTrace");
                    if (inIndex != -1)
                    {
                        msg += trace.Substring(0, inIndex) + "\r\n";
                        trace = trace.Substring(inIndex);
                        while ((inIndex = trace.IndexOf("in ", 3)) != -1)
                        {
                            msg += "   " + trace.Substring(0, inIndex) + "\r\n";
                            trace = trace.Substring(inIndex);
                        }
                    }
                    msg += "   " + trace + "\r\n";
                }
            }
            if (e.InnerException != null)
            {
                msg += "Inner Exception:\r\n";
                msg += GetExceptionInfo(e.InnerException, true);
                msg += "\r\n";
            }
            return msg;
        }


        /// <summary>
        /// Format a log message by placing date and time at the front of the first line of
        /// the message, and to offset additional message lines
        /// </summary>
        /// <param name="msg">Message to format</param>
        /// <returns>String containing the formatted message</returns>
        private string FormatMessage(string msg)
        {
            DateTime now = DateTime.Now;
            string time = string.Format("{0:d4}-{1:d2}-{2:d2} {3:d2}:{4:d2}:{5:d2}",
                now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second);
            string message = time + " ";
            string[] lines = msg.Split('\n');
            bool firstline = true;
            foreach (string line in lines)
            {
                if (line != string.Empty)
                {
                    if (firstline)
                    {
                        message += line;
                        message += 1 == lines.Length ? "\r\n" : "\n";
                        firstline = false;
                    }
                    else
                    {
                        message += "                    " + line + "\n";
                    }
                }
            }
            return message;
        }
		
        /// <summary>
        /// Display a MessageBox containing a message (typically an error or exception message.
        /// </summary>
        /// <param name="message">Message to display</param>
        /// <param name="mImage">MessageBoxImage to display in the MessageBox</param>
		public static void ShowMessage(string message, MessageBoxImage mImage = MessageBoxImage.Warning)
		{
			MessageBox.Show(message, "HamQSLer", MessageBoxButton.OK, mImage);
		}
		

    }
}
