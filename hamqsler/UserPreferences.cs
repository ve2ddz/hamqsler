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
using System;
using System.IO;
using System.Security;
using System.Windows;
using System.Xml.Serialization;

namespace hamqsler
{
	/// <summary>
    /// UserPrefs contains user preference information
    /// It is serialized to file .hamqsler in the hamqsler folder
	/// </summary>
	[Serializable]
	public class UserPreferences : DependencyObject
	{
        /// <summary>
        /// Default callsign for the card
        /// </summary>
        private static readonly DependencyProperty CallsignProperty =
            DependencyProperty.Register("Callsign", typeof(string), typeof(UserPreferences),
            new PropertyMetadata("MyCall"));
        public string Callsign
        {
            get { return (string)GetValue(CallsignProperty); }
            set { SetValue(CallsignProperty, value); }
        }
        
        /// <summary>
        /// Default Name and QTH text for the card
        /// </summary>
        private static readonly DependencyProperty NameQthProperty =
            DependencyProperty.Register("NameQth", typeof(string), typeof(UserPreferences),
            new PropertyMetadata("MyQTH"));
        public string NameQth
        {
            get { return (string)GetValue(NameQthProperty); }
            set { SetValue(NameQthProperty, value); }
        }
        
        
        private ExceptionLogger logger = null;
        public ExceptionLogger Logger
        {
            set { logger = value; }
        }
        
        private static string userPreferencesFilename =  Environment.GetFolderPath(
        		Environment.SpecialFolder.MyDocuments) + "/hamqsler/.hamqsler";
        public static string UserPreferencesFilename
        {
        	get {return userPreferencesFilename;}
        }

        /// <summary>
        ///  Default constructor used by the deserialization process.
        /// Do not call this directly, use the static CreateUserPreferences method instead

        /// </summary>
		public UserPreferences()
		{

		}
		
		/// <summary>
		/// Constructor used to create a clone of a UserPrefs object

		/// </summary>
		/// <param name="userPrefs">UserPreferences object to clone</param>
		public UserPreferences(UserPreferences prefs)
		{
			Callsign = prefs.Callsign;
		}

        /// <summary>
        ///  Factory for UserPrefs objects.
        ///  Deserializes UserPrefs object stored in qslPrefs.xml if file exists.
        ///  Otherwise creates a default UserPrefs object
        /// </summary>
        /// <param name="logger">ExceptionLogger object to send messages and exceptions to</param>
        /// <param name="showMessages">boolean indicating whether an error message should be
        /// displayed if an error occurs.</param>
        /// <param name="prefsInitilized">boolean indicating whether the prefs object was initialized
        /// or loaded. Value of true indicates that the object was initialized, not loaded</param>
        /// <param name="prefsError">boolean indicating if an error occurred while trying
        /// to deserialize the prefs file.</param>
        /// <returns>Deserialized or default UserPrefs object</returns>
        public static UserPreferences CreateUserPreferences(ExceptionLogger logger,
                                                            bool showMessages,
                                                            out bool prefsInitialized,
                                                            out bool prefsError)
        {
            // filename of the user prefs file
            
            FileStream fStream = null;
            // default user preferences. Created here in case there are new preferences that
            // were not saved in the preferences file
            UserPreferences prefs = CreateDefaultUserPreferences(logger, out prefsInitialized);
            prefsError = true;		// set to false if prefs file is read
            try
            {
                // get file info
 				FileInfo prefsInfo = new FileInfo(userPreferencesFilename);
                if (prefsInfo.Exists)
                {
                    // file exists, so read it
                    fStream = new FileStream(userPreferencesFilename, FileMode.Open,
                        FileAccess.Read);
                    XmlSerializer xmlFormat = new XmlSerializer(typeof(UserPreferences));
                    prefs = (UserPreferences)xmlFormat.Deserialize(fStream);
                    prefsInitialized = false;	// prefs file has been loaded
                    prefsError = false;			// no error loading file
                }
                else 
                {
                	prefsError = false;
                	if(showMessages)
	                {
	                    // no user prefs file, so inform user, and create a default
	                    MessageBox.Show("User Preferences file does not exist.\r\n"
	                        + "User Preferences are being initialized.\r\n"
	                        + "Preferences may be set from the Edit dropdown menu",
	                        "No User Preferences Found",
	                        MessageBoxButton.OK, MessageBoxImage.Information);
	                }
                }
            }
            catch (SecurityException e)
            {
                if (logger != null)
                {
                    Exception ex = new Exception("You do not have permission to access the User Preferences File.\r\n"
                        + "Preferences will be initialized.",
                        e);
                    logger.Log(ex);
                }
                else if(showMessages)
                {
                    MessageBox.Show("You do not have permission to access the User Preferences File.\r\n"
                        + "Preferences will be initialized.",
                        "Cannot access User Preferences File",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (UnauthorizedAccessException e)
            {
                if (logger != null)
                {
                    Exception ex = new Exception("You do not have permission to access the User Preferences File.\r\n"
                        + "Preferences will be initialized.",
                        e);
                    logger.Log(ex);
                }
                else if(showMessages)
                {
                    MessageBox.Show("You do not have permission to access the User Preferences File.\r\n"
                        + "Preferences will be initialized.",
                        "Cannot access User Preferences File",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
             }
            catch (PathTooLongException e)
            {
                if (logger != null)
                {
                    Exception ex = new Exception("Path to User Preferences File exceeds the system defined maximum length.\r\n"
                        + "Preferences will be initialized.",
                        e);
                    logger.Log(ex);
                }
                else if(showMessages)
                {
                    MessageBox.Show("Path to User Preferences File exceeds the system defined maximum length.\r\n"
                        + "Preferences will be initialized.",
                        "Cannot access User Preferences File",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (FileNotFoundException e)
            {
            	prefsInitialized = true;
            	prefsError = false;
                if (logger != null)
                {
                    Exception ex = new Exception("There appears to be a problem with your file system.\r\n"
                        + "This program has found the file, but it cannot be opened.\r\n"
                        + "Preferences will be initialized.",
                        e);
                    logger.Log(ex);
                }
                else if(showMessages)
                {
                    MessageBox.Show("There appears to be a problem with your file system.\r\n"
                        + "This program has found the file, but it cannot be opened.\r\n"
                        + "Preferences will be initialized.",
                        "Cannot access User Preferences File",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (IOException e)
            {
                if (logger != null)
                {
                    Exception ex = new Exception("An IO error occurred while attempting to access the User Preferences file:\r\n"
                        + e.Message + "\r\n"
                        + "Preferences will be initialized.",
                        e);
                    logger.Log(ex);
                }
                else if(showMessages)
                {
                    MessageBox.Show("An IO error occurred while attempting to access the User Preferences file:\r\n"
                        + e.Message + "\r\n"
                        + "Preferences will be initialized.",
                        "Cannot access User Preferences File",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception e)
            {
                if (logger != null)
                {
                    Exception ex = new Exception("Programming Error: Bad User Preferences filename or mode:\r\n"
                        + userPreferencesFilename + "\r\n"
                        + "Please do the following when you have finished using QslDesignAndPrint:\r\n"
                        + "1. Make a copy of the log file.\r\n"
                        + "2. Post a message on the QslDesignAndPrint forum including the content of this message/r/n"
                        + "Preferences will be initialized. You may continue to use the program.",
                        e);
                    logger.Log(ex);
                }
                else if(showMessages)
                {
                    MessageBox.Show("Programming Error: Bad User Preferences User filename:\r\n"
                        + userPreferencesFilename + "\r\n"
                        + "Please do the following when you have finished using QslDesignAndPrint:\r\n"
                        + "1. Post a message on the QslDesignAndPrint forum including the content of this message/r/n"
                        + "Preferences will be initialized. You may continue to use the program.",
                        "Cannot access User Preferences File",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            finally
            {
                // be sure to close the file
                if(fStream != null)
                    fStream.Close();
            }
            // if prefs have been initialized, it is necessary to save the file
            // return the UserPrefs object
            if(prefsInitialized)
            {
            	// note: we do not want to suppress error Message box displays when saving the prefs file
            	prefs.SerializeAsXml();
            }
            return prefs;                
        }

        /// <summary>
        /// Helper method that creates a default UserPrefs object
        /// </summary>
        /// <param name="logger">ExceptionLogger object that exceptions and messages will be sent to</param>
        /// <param name="prefsInitialized">boolean indicating that the prefs object was created</param>
        /// <returns></returns>
        private static UserPreferences CreateDefaultUserPreferences(ExceptionLogger logger,
                                                                    out bool prefsInitialized)
        {
            UserPreferences prefs = new UserPreferences();
            prefsInitialized = true;
            prefs.Logger = logger;
            if (logger != null)
            {
                logger.Log("User Preferences have been initialized");
            }
            return prefs;
        }

       /// <summary>
        /// Saves the User Preferences into /hamqsler/.hamqsler
        /// </summary>
        private void SerializeAsXml()
        {
            // get path to prefs file
            XmlSerializer xmlFormat = new XmlSerializer(typeof(UserPreferences));
            Stream fStream = null;
            try
            {
                // open the file and serialize the user prefs to the file
                fStream = new FileStream(userPreferencesFilename, FileMode.Create,
                        FileAccess.Write, FileShare.None);
                xmlFormat.Serialize(fStream, this);
            }
            catch (ArgumentOutOfRangeException e)
                // invalid file access mode
            {
                if (logger != null)
                {
                    // wrap the exception in another message and log it
                    Exception ex = new Exception("Programmer Error: Invalid File Access Mode"
                        + "\r\nUnable to save preferences file."
                        + "\r\n\r\nPlease do the following:\r\n"
                        + "1. Terminate this program.\r\n"
                        + "2. Make a backup copy of the log file\r\n"
                        + "3. Post a message using the Contact Us form at www.va3hj.ca", e);
                    logger.Log(ex);
                }
                else
                {
                    // can't log, so just display a message
                    MessageBox.Show("Programmer Error: Invalid File Access Mode"
                        + "\r\nUnable to save preferences file."
                        + "\r\n\r\nPlease do the following:\r\n"
                        + "1. Terminate this program.\r\n"
                        + "2. Post a message containing this error message\r\n"
                        + "     using the Contact Us form at www.va3hj.ca\r\n"
                        + "     and stating that the error was not posted to the log file\r\n"
                        + "3. Restart QslDesignAndPrint and proceed without modifying preferences.",
                        "Cannot Save Preferences File", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            catch (ArgumentNullException e)
                // path to the file is null
            {
                if (logger != null)
                {
                    // wrap exception in another and log it
                    Exception ex = new Exception("Programmer Error: Null User Preferences file name."
                        + "\r\nUnable to save preferences file."
                        + "\r\n\r\nPlease do the following:\r\n"
                        + "1. Terminate this program.\r\n"
                        + "2. Make a backup copy of the log file\r\n"
                        + "3. Post a message using the Contact Us form at www.va3hj.ca", e);
                    logger.Log(ex);
                }
                else
                {
                    // can't log so just display message
                    MessageBox.Show("Programmer Error: Null User Preferences file name."
                        + "\r\nUnable to save preferences file."
                        + "\r\n\r\nPlease do the following:\r\n"
                        + "1. Terminate this program.\r\n"
                        + "2. Make a backup copy of the log file\r\n"
                        + "3. Post a message containing this error message\r\n"
                        + "     using the Contact Us form at www.va3hj.ca\r\n"
                        + "4. Restart QslDesignAndPrint and proceed without modifying preferences.",
                        "Cannot Save Preferences File", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            catch (ArgumentException e)
                // path is empty, contains only whitespace or illegal characters
                // or refers to a non-file device
            {
                if (logger != null)
                {
                    // wrap exception in another and log it
                    Exception ex = new Exception("Programmer Error: Invalid User Preferences file name: "
                        + userPreferencesFilename
                        + "\r\nUnable to save preferences file."
                        + "\r\n\r\nPlease do the following:\r\n"
                        + "1. Terminate this program.\r\n"
                        + "2. Make a backup copy of the log file\r\n"
                        + "3. Post a message using the Contact Us form at www.va3hj.ca", e);
                    logger.Log(ex);
                }
                else
                {
                    // can't log, so just display it
                    MessageBox.Show("Programmer Error: Invalid User Preferences file name: "
                        + userPreferencesFilename
                        + "\r\nUnable to save preferences file."
                        + "\r\n\r\nPlease do the following:\r\n"
                        + "1. Terminate this program.\r\n"
                        + "2. Make a backup copy of the log file\r\n"
                        + "3. Post a message containing this error message\r\n"
                        + "     using the Contact Us at www.va3hj.ca\r\n"
                        + "4. Restart QslDesignAndPrint and proceed without modifying preferences.",
                        "Cannot Save Preferences File", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            catch (NotSupportedException e)
                // path refers to a non-file device
            {
                if (logger != null)
                {
                    // wrap exception in another and log it
                    Exception ex = new Exception("Programmer Error: Invalid User Preferences file name: "
                        + userPreferencesFilename
                        + "\r\nUnable to save preferences file."
                        + "\r\n\r\nPlease do the following:\r\n"
                        + "1. Terminate this program.\r\n"
                        + "2. Make a backup copy of the log file\r\n"
                        + "3. Post a message using the Contact Us at www.va3hj.ca"
                        + "4. Restart QslDesignAndPrint and proceed without modifying preferences", 
                        e);
                    logger.Log(ex);
                }
                else
                {
                    // can't log so just display message
                    MessageBox.Show("Programmer Error: Invalid User Preferences file name: "
                        + userPreferencesFilename
                        + "\r\nUnable to save preferences file."
                        + "\r\n\r\nPlease do the following:\r\n"
                        + "1. Terminate this program.\r\n"
                        + "2. Post a message containing this error message\r\n"
                        + "     using the Contact Us form at www.va3hj.ca\r\n"
                        + "3. Restart QslDesignAndPrint and proceed without modifying preferences.",
                        "Cannot Save Preferences File", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            catch (PathTooLongException e)
                // the path, filename, or both exceeds system defined max length
            {
                if (logger != null)
                {
                    // wrap exception in another and log it
                    Exception ex = new Exception("The path or filename of the User Preferences file is too long:\r\n"
                            + userPreferencesFilename + "\r\n"
                            + "The most likely cause is a path to the QslDnP directory being more than 248 characters.\r\n"
                            + "The log file may contain more information."
                            , e);
                    logger.Log(ex);
                }
                else
                {
                    // can't log, so just display message
                    MessageBox.Show("The path or filename of the User Preferences file is too long:\r\n"
                            + userPreferencesFilename + "\r\n"
                            + "The most likely cause is a path to the QslDnP directory being more than 248 characters.\r\n"
                            + "Error message: " + e.Message,
                        "Cannot Save Preferences File", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            catch (IOException e)
                // IO error occurred
            {
                if (logger != null)
                {
                    // wrap exception in another and log it
                    Exception ex = new Exception("IO Error While Attempting to Open or Write to the Preferences File."
                        + "Some possible causes include:\r\n"
                        + "1. You do not have write permission on the file or directory\r\n"
                        + "2. Disk error.\r\n\r\n"
                        + "See the log file for more information on the error", e);
                    logger.Log(ex);
                }
                else
                {
                    // can't log, so just display message
                    MessageBox.Show("IO Error While Attempting to Open or Write to the Preferences File:\r\n"
                        + e.Message + "\r\n\r\n"
                        + "Some possible causes include:\r\n"
                        + "1. You do not have write permission on the file or directory\r\n"
                        + "2. Disk error.\r\n",
                        "Cannot Save Preferences File", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            catch (SecurityException e)
                // caller does not have required permission
            {
                if (logger != null)
                {
                    // wrap exception in another, then log it
                    Exception ex = new Exception("You do not have the required permissions to open or write the Preferences File\r\n"
                            , e);
                    logger.Log(ex);
                }
                else
                {
                    // can't log, so just display message
                    MessageBox.Show("You do not have the required permissions to open or write to the Preferences File\r\n",
                        "Cannot Save Preferences File", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            catch (UnauthorizedAccessException e)
                // write access not allowed to the file (probably read-only)
            {
                if (logger != null)
                {
                    // wrap in another exception and log it
                    Exception ex = new Exception("Write access to the Preferences File is not allowed by the operating system.\r\n"
                        + "Check to see if the QslDnP directory or userprefs.xml is set to read-only.",
                        e);
                    logger.Log(ex);
                }
                else
                {
                    // can't log, so just display message
                    MessageBox.Show("Write access to the Preferences File is not allowed by the operating system.\r\n"
                        + "Check to see if the QslDnP directory or userprefs.xml is set to read-only.",
                        "Cannot Save Preferences File", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            catch (InvalidOperationException e)
                // error occurred during serialization
            {
                if (logger != null)
                {
                    // wrap error in another exception and log it
                    Exception ex = new Exception("An error occurred during serializing or saving the User Preferences File:"
                        + "\r\n" + e.InnerException.Message
                        + "\r\nIf you do not understand the error message, post a message using"
                        + "\r\nthe Contact Us form at www.va3hj.ca and include the contents of this message",
                        e);
                    logger.Log(ex);
                }
                else
                {
                    // can't log, so just display message
                    MessageBox.Show("An error occurred during serializing or saving the User Preferences File:"
                        + "\r\n" + e.InnerException.Message
                        + "\r\nIf you do not understand the error message, post a message using"
                        + "\r\nthe Contact Us form at www.va3hj.ca and include the contents of this message",
                        "Cannot Save Preferences File", MessageBoxButton.OK,
                        MessageBoxImage.Error);
                }
            }
            finally
            {
                // make sure that the file is closed.
                if (fStream != null)
                    fStream.Close();
            }
        }	}
}
