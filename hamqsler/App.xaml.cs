/*
 *  Author:
 *       Jim Orcheson <jimorcheson@gmail.com>
 * 
 *  Copyright (c) 2012, 2013 Jim Orcheson
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
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

using QslBureaus;
using Qsos;

namespace hamqsler
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private const int WEBREQUESTTIMEOUT = 10000;		// 10 seconds
		private SplashPage splash;
		private static ExceptionLogger logger;
		public static ExceptionLogger Logger
		{
			get {return logger;}
		}
		
		string created = string.Empty;
		
		/// <summary>
		/// UserPreferences object. Stored in App because it is created before MainWindow is 
		/// created.
		/// </summary>
		private UserPreferences userPrefs;
		public UserPreferences UserPreferences
		{
			get {return userPrefs;}
			set {userPrefs = value;}
		}
		
		private static readonly string hamqslerFolder = Environment.GetFolderPath(
        		Environment.SpecialFolder.MyDocuments) + "\\hamqsler\\";
		public string HamqslerFolder
		{
			get {return hamqslerFolder;}
		}
		
		/// <summary>
		/// Application startup code
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		void Application_Startup(object sender, StartupEventArgs e)
		{
			Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
			// create and show SplashPage
			splash = new SplashPage();
			splash.ShowDialog();
			this.MainWindow = new MainWindow();
			MainWindow.Show();
		}
		
		/// <summary>
		/// Called if an exception is not handled elsewhere.
		/// Only thing to do is maybe display a message, log the exception, and terminate
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">DispatcherUnhandledExceptionEventArgs object</param>
		private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
		{
			if(logger != null)
			{
				logger.Log(e.Exception, ExceptionLogger.SHOWTRACE, ExceptionLogger.DONTSHOWMESSAGE);
			}
			MessageBox.Show("HamQSLer must terminate because of an unhandled exception." +
			                Environment.NewLine +
			                "Information about the exception is provided in the log file." +
			                Environment	.NewLine +
			                "Please log a bug report by following the instructions on the website" +
			                Environment.NewLine +
			                "and include the log file." +
			                Environment.NewLine +
			                "Do not restart the program before doing this because this will" +
			                Environment.NewLine +
			                "cause the log file to be overwritten.",
			                "Unhandled Exception", MessageBoxButton.OK, MessageBoxImage.Error);
			this.Shutdown();
		}

			/// <summary>
		/// Helper method that creates the hamqsler directory and subdirectories in the user's 
		/// 'Documents' directory (in My Documents directory in Windows, in home directory in Unix systems)
		/// </summary>
		/// <param name="terminate">boolean indicating if an error occurred creating the directories
		/// or their contents.</param>
		/// <returns>boolean indicating if directories have been created</returns>
		public bool BuildHamQslerDirectories(out bool terminate)
		{
			bool showHamqslerCreatedLabel = false;
			terminate = false;
			try
			{
				// check if hamqsler directory exists and create it if it doesn't
				string hamqsler = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/hamqsler";
				DirectoryInfo qslsInfo = new DirectoryInfo(hamqsler);
				if (!qslsInfo.Exists)
				{
					qslsInfo.Create();
					created = "hamqsler directory created" + Environment.NewLine;
					showHamqslerCreatedLabel = true;
				}
				// check if Printing subdirectory exists and create it if it doesn't
				DirectoryInfo printingDirInfo = new DirectoryInfo(hamqsler + "/Printing");
				if(!printingDirInfo.Exists)
				{
					printingDirInfo.Create();
					created += "hamqsler/Printing directory created" + Environment.NewLine;
					showHamqslerCreatedLabel = true;
				}
				// check if Logs subdirectory exists and create it if it doesn't
				DirectoryInfo logsDirInfo = new DirectoryInfo(hamqsler + "/Logs");
				if(!logsDirInfo.Exists)
				{
					logsDirInfo.Create();
					created += "hamqsler/Logs directory created" + Environment.NewLine;
					showHamqslerCreatedLabel = true;
				}
				// check if Samples directory exists and create it if it doesn't
				DirectoryInfo samplesDirInfo = new DirectoryInfo(hamqsler + "/Samples");
				if (!samplesDirInfo.Exists)
				{
					samplesDirInfo.Create();
					created += "hamqsler/Samples directory created" + Environment.NewLine;
					showHamqslerCreatedLabel = true;
				}
				// copy Samples files if they don't exist
				DirectoryInfo samplesInfo = new DirectoryInfo(
							Environment.CurrentDirectory + "/Samples");
				if (!samplesInfo.Exists)
				{
					terminate = true;
					showHamqslerCreatedLabel = false;
					return showHamqslerCreatedLabel;
				}
				FileInfo[] files = samplesInfo.GetFiles();
				foreach (FileInfo file in files)
				{
					FileInfo fInfo = new FileInfo(samplesDirInfo.FullName + "/" + file.Name);
					if (!fInfo.Exists)
					{
						file.CopyTo(fInfo.FullName);
						created += fInfo.Name + " copied" + Environment.NewLine;
						showHamqslerCreatedLabel = true;
					}
				}
			}
			catch (Exception ex)		// error of some kind so let user know
			{
				splash.Dispatcher.BeginInvoke(DispatcherPriority.Normal,
				                              (ThreadStart) delegate() {
				    MessageBox.Show("An error has occurred while attempting to create the hamqsler directory\r\n"
								+ " or to copy its contents:\r\n" + ex.Message,
								"Intiialization Error",MessageBoxButton.OK, MessageBoxImage.Stop);
				});

				terminate = true;		// indicate error
			}
			return showHamqslerCreatedLabel;
		}

		/// <summary>
		/// Create the exception logger
		/// </summary>
		/// <param name="securityException">security exception indicator</param>
		/// <param name="accessException">access exception indicator</param>
		internal void CreateExceptionLogger(out bool securityException, out bool accessException)
		{
			logger = new ExceptionLogger(Environment.GetFolderPath(
					Environment.SpecialFolder.MyDocuments) + "/hamqsler/Logs/hamqsler.log",
					out securityException, out accessException);
		}
		
		/// <summary>
		/// Log the application's runtime information to the exception logger
		/// </summary>
		internal void LogRuntimeInfo()
		{
			string os = Environment.OSVersion.VersionString;			
			Assembly assembly = Assembly.GetAssembly(new QslBureau().GetType());
			// get version of QsosLibary
			// AdifString object only used so that I can get the Type for it to get
			// the Assembly it is in
			AssemblyName aName2 = assembly.GetName();
			AdifString aStr = new AdifString("<eor>");
			Type t = aStr.GetType();
			Assembly asm = Assembly.GetAssembly(t);
			AssemblyName aName = asm.GetName(); 
			// get CLR version info
			Version ver = Environment.Version;
			// get newline for this platform
			string newline = Environment.NewLine;
			// output start log message
			logger.Log(string.Format("HamQSLer started" + newline +
			                         "HamQSLer version: {6}" + newline +
			                         "QsosLibrary version: {8}" + newline +
			                         "QslBureaus version: {9}" + newline +
			                         "CLR version: {2}.{3}.{4}.{5}" + newline +
			                         "OS: {0}" + newline + 
			                         "Processors: {1}" + newline +
			                         "Culture: {7}" + newline,
			                         os,
			                         Environment.ProcessorCount, 
									 ver.Major, ver.Minor, ver.Build, ver.Revision,
			                         Assembly.GetExecutingAssembly().GetName().Version.ToString(),
			                         CultureInfo.CurrentCulture.Name,
			                         aName.Version.ToString(),
			                         aName2.Version.ToString()
			                ));
			if(!created.Equals(string.Empty))
			{
				logger.Log(created);
			}
			
		}

		/// <summary>
		/// Create the user preferences object. Actual work is done in UserPreferences class
		/// </summary>
		/// <param name="userPrefsIntialized">boolean indicating whether the prefs obejct was
		/// initialized.</param>
		/// <param name="userPrefsError">boolean indicating if an error occurred while loading
		/// the prefs object</param>
		internal void GetUserPreferences(
				out bool userPrefsIntialized, out bool userPrefsError)
		{
			userPrefs = UserPreferences.CreateUserPreferences(false, out userPrefsIntialized,
			                                                  out userPrefsError);
		}
		
		/// <summary>
		/// Reads and checks version info from hamqslerVersion.txt on website versus versions currently
		/// being used.
		/// </summary>
		/// <param name='webError'>
		/// Boolean indicating if an error was encountered while attempting to retrieve the version info
		/// </param>
		/// <param name="newStableVersion">
		/// Boolean indicating if a new stable version of HamQSLer is available for download</param>
		/// <param name="newDevelopmentVersion">
		/// Boolean indicating if a new development version of HamQSLer is available for download</param>
		internal void GetProgramVersions(out bool webError, out bool newStableVersion,
		                                out bool newDevelopmentVersion)
        {
			webError = false;
			newStableVersion = false;
			newDevelopmentVersion = false;
			Dictionary<string, string> versions = new Dictionary<string, string>();
            // create request for website
            HttpWebRequest httpRequest = null;
            HttpWebResponse response = null;
            StreamReader resStream = null;
			// get this program's version info
	        Version ver = Assembly.GetExecutingAssembly().GetName().Version;
            string location = string.Format("http://www.va3hj.ca/hamqslerVersion.txt?ver={0}.{1}.{2}",
                    ver.Major, ver.Minor, ver.Build);
            try
            {
            	// build the http request
                httpRequest = (HttpWebRequest)WebRequest.Create(location);
                httpRequest.Timeout = WEBREQUESTTIMEOUT;
				string proxy = userPrefs.HttpProxyServer;
				int proxyPort = userPrefs.HttpProxyServerPortNumber;
                if (proxy != string.Empty)
                {
                    httpRequest.Proxy = new WebProxy(proxy, proxyPort);
                }
				// get OS information for including in UserAgentt
				string os = "Unknown";
                string osVer = Environment.OSVersion.VersionString;
                Regex osRegex = new Regex("(Windows NT [\\d]\\.[\\d])");
                Match match = osRegex.Match(osVer);
				os = match.Value;
                httpRequest.UserAgent = string.Format("HamQsler ({0}; [{1}])",
                            os, CultureInfo.CurrentCulture.Name);

                // *** Retrieve request info headers
                response = (HttpWebResponse)httpRequest.GetResponse();

                Encoding enc = Encoding.UTF8;
                // get response from website
                resStream = new StreamReader(response.GetResponseStream(), enc);
				Regex updates = new Regex("([a-zA-Z]+)\\s([0-9]{6})");
				// build the Dictionary of names and versions from the contents of hamqslerVersion.txt
				// It is expected that each line will contain a file or program name and its version.
				// Version is exactly 6 digits containing 2 digits for Major build number,
				// 2 digits for Minor build number, and 2 digits for Build number
				while(!resStream.EndOfStream)
				{
					string resp = resStream.ReadLine();
					MatchCollection matches = updates.Matches(resp);
					versions.Add(matches[0].Groups[1].Value, matches[0].Groups[2].Value);
				}
			}
            catch (SecurityException e)
            {
                logger.Log(e);
				webError = true;
            }
            catch (WebException e)
            {
                DecodeWebException(e);
				webError = true;
            }
            catch (Exception e)
            {
                logger.Log("Error retrieving version info: " + e.Message);
				webError = true;
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
                if (resStream != null)
                {
                    resStream.Close();
                }
            }
            if(!webError)
            {
				// have version info, so check if any updates available
				foreach(string key in versions.Keys)
				{
					switch(key)
					{
						case "stable":
							newStableVersion = CheckHamQslerVersion(versions[key]);
							break;
						case "development":
							newDevelopmentVersion = CheckHamQslerVersion(versions[key]);
							break;
					}
				}
            }
		}
		
    	/// <summary>
    	/// Decodes and logs web exception info
    	/// </summary>
    	/// <param name="e">WebException being decoded</param>
        private void DecodeWebException(WebException e)
        {
        	// get status and response
            WebExceptionStatus status = e.Status;
            // process the status
            switch(status)
            {
                case WebExceptionStatus.Timeout:
                    break;
                case WebExceptionStatus.ConnectFailure:
                    logger.Log("Website access error: ConnectFailure.\r\n"
                        + "Probable cause is one of:\r\n"
                        + "\tWrong proxy server name or IP address specified;\r\n"
                        + "\tWrong proxy server port number;\r\n"
                        + "\tProxy server is down;\r\n"
                        + "\tYour Internet connection is down; or,\r\n"
                        + "\twww.va3hj.ca is down.");
                    break;
                default:
                    logger.Log("Website access error: " + status.ToString()
                        + "\r\nError: " + e.Message);
                    break;
            }
        }
		
		/// <summary>
		/// Checks program version number
		/// </summary>
		/// <returns>
		/// Boolean indicating whether a new version of the program is available or not
		/// </returns>
		private bool CheckHamQslerVersion(string version)
		{
			// get this program's version info
	        Version ver = Assembly.GetExecutingAssembly().GetName().Version;
			
			string vers = string.Format("{0:d2}{1:d2}{2:d2}", ver.Major, ver.Minor, ver.Build);
			if(vers.CompareTo(version) < 0)
				return true;
			else
				return false;
		}
	}
}