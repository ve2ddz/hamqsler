/*
 *  Author:
 *       Jim Orcheson <jimorcheson@gmail.com>
 * 
 *  Copyright (c) 2012 - 2014 Jim Orcheson
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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using System.Xml;

namespace hamqsler
{
	/// <summary>
	/// Interaction logic for SplashPage.xaml
	/// SplashPage is displayed when App is started.
	/// It performs many initialization tasks, such as loading and
	/// checking the UserPreferences,
	/// and checking if a new version of the program is available.
	/// </summary>
	public partial class SplashPage : Window
	{
		private bool userPrefsError = false;
		private bool showUserPrefsLabel = false;
		
		public SplashPage()
		{
			InitializeComponent();
		}
		
		/// <summary>
		/// Perform all startup functions such as:
		/// Load or create UserPreferences, and
		/// check if new version of program is available.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Window_ContentRendered(object sender, EventArgs e)
		{
			InitializeApplication();

		}
		
		/// <summary>
		/// Performs actions such as:
		/// 1. Creating the hamqsler folder and subfolders in My Documents, and
		///    copies sample files to it.
		/// 2. Creates the ExceptionLogger object
		/// 3. Loads or creates the UserPreferences object
		/// 4. Checks if new program components are available for download
		/// Note: this method must be run under the main thread
		/// </summary>
		private void InitializeApplication()
		{
			Cursor oldCursor = this.Cursor;
			this.Cursor = Cursors.Wait;
			UpdateUI();
			// check and create hamqsler directories and copy sample files
			bool directoriesError = false;
			bool showHamqslerLabel = ((App)Application.Current).BuildHamQslerDirectories(out directoriesError);
			if(directoriesError)		// error occurred creating directories
			{
				ShowSplashPageLabel(hamqslerCreatedProblemLabel);
			}
			else if(showHamqslerLabel)		// created directories/copied files
			{
				ShowSplashPageLabel(hamqslerCreatedLabel);
			}
			// create ExceptionLogger
			bool securityException = false;
			bool accessException = false;
			((App)Application.Current).CreateExceptionLogger(out securityException, out accessException);
			if(securityException)
			{
				ShowSplashPageLabel(logPermissionErrorLabel);
			}
			else if(accessException)
			{
				ShowSplashPageLabel(logAccessErrorLabel);
			}
			// load existing UserPreferences file, or create new one
			// it is necessary to run this on the UI thread because UserPreferences is a
			// Dependency object.
			GetUserPreferences();

			bool adifError = false;
			try
			{
				App.AdifEnums.LoadDocument();
			}
			catch(XmlException xe)
			{
				App.Logger.Log(xe, ExceptionLogger.SHOWTRACE, ExceptionLogger.DONTSHOWMESSAGE);
				try
				{
					App.AdifEnums.CopyDefaultXmlFile();
					App.AdifEnums.LoadDocument();
					ShowSplashPageLabel(adifEnumerationsErrorLabel);
					adifError = true;
				}
				catch(Exception e)
				{
					App.Logger.Log(e, ExceptionLogger.SHOWTRACE, ExceptionLogger.DONTSHOWMESSAGE);
					MessageBox.Show("Problem accessing AdifEnumerations.xml file." +
					                Environment.NewLine +
					                "Also encountered error attempting to replace AdifEnumerations.xml" +
					                Environment.NewLine +
					                "file with default file shipped with the program." +
					                Environment.NewLine +
					                "See log file for information on the errors." +
					                Environment.NewLine +
					                Environment.NewLine +
					                "Program cannot continue without this file. Program will terminate.",
					                "AdifEnumerations.xml Error",
					                MessageBoxButton.OK,
					                MessageBoxImage.Error);
					// force program termination
					System.Diagnostics.Process.GetCurrentProcess().Kill();
				}
			}
			bool callsBureausError = false;
			try
			{
				App.CallBureaus.LoadDocument();
			}
			catch(XmlException xe)
			{
				App.Logger.Log(xe, ExceptionLogger.SHOWTRACE, ExceptionLogger.DONTSHOWMESSAGE);
				try
				{
					App.CallBureaus.CopyDefaultXmlFile();
					App.CallBureaus.LoadDocument();
					ShowSplashPageLabel(callsBureausErrorLabel);
					callsBureausError = true;
				}
				catch(Exception e)
				{
					App.Logger.Log(e, ExceptionLogger.SHOWTRACE, ExceptionLogger.DONTSHOWMESSAGE);
					MessageBox.Show("Problem accessing CallsBureaus.xml file." +
					                Environment.NewLine +
					                "Also encountered error attempting to replace CallsBureaus.xml" +
					                Environment.NewLine +
					                "file with default file shipped with the program." +
					                Environment.NewLine +
					                "See log file for information on the errors." +
					                Environment.NewLine +
					                Environment.NewLine +
					                "Program cannot continue without this file. Program will terminate.",
					                "AdifEnumerations.xml Error",
					                MessageBoxButton.OK,
					                MessageBoxImage.Error);
					// force program termination
					System.Diagnostics.Process.GetCurrentProcess().Kill();
				}
			}
			((App)Application.Current).LogRuntimeInfo();		// output run start info
			// check for new program version and data file updates
			bool webError = false;
			bool newStableVersion = false;
			bool newDevelopmentVersion = false;
			bool newAdifEnumerationsVersion = false;
			bool newCallsBureausVersion = false;
			ShowSplashPageLabel(checkingForUpdatesLabel);
			// updates will contain program and file names with most recent versions available for download
			// cannot proceed until UserPreferences have been created.
			if(((App)Application.Current).UserPreferences.CheckForNewVersions)
			{
				((App)Application.Current).GetProgramVersions(out webError,
					                  						  out newStableVersion,
					                  						  out newDevelopmentVersion,
					                  						 out newAdifEnumerationsVersion,
					                  						out newCallsBureausVersion);
				if(webError)		// error retrieving file containing version info
				{
					ShowSplashPageLabel(webErrorLabel);
				}
				else
				{			
					if(newStableVersion)
					{
						ShowSplashPageLabel(newStableVersionLabel);
					}
					if(((App)Application.Current).UserPreferences.CheckForDevelopmentVersions &&
					   newDevelopmentVersion)
					{
						ShowSplashPageLabel(newDevelopmentVersionLabel);
					}
					if(newAdifEnumerationsVersion)
					{
						try
						{
							bool wError = App.AdifEnums.DownloadFileFromWebsite();
							webError = webError || wError;
							App.AdifEnums.LoadDocument();
							if(!wError)
							{
								ShowSplashPageLabel(newAdifEnumerationsLabel);
								App.Logger.Log("AdifEnumerations.xml version " + App.AdifEnums.Version +
								           " downloaded.");
							}
							else
							{
								ShowSplashPageLabel(cannotDownloadAdifEnumerationsErrorLabel);
							}
						}
						catch(Exception e)
						{
							App.Logger.Log(e);
							ShowSplashPageLabel(newAdifEnumsErrorLabel);
							try
							{
							bool wError = App.CallBureaus.DownloadFileFromWebsite();
							webError = webError || wError;
								App.AdifEnums.LoadDocument();
								ShowSplashPageLabel(adifEnumerationsErrorLabel);
							}
							catch(Exception ex)
							{
								App.Logger.Log(ex, ExceptionLogger.SHOWTRACE, ExceptionLogger.DONTSHOWMESSAGE);
								MessageBox.Show("Problem accessing downloaded AdifEnumerations.xml file." +
								                Environment.NewLine +
								                "Also encountered error attempting to replace AdifEnumerations.xml" +
								                Environment.NewLine +
								                "file with default file shipped with the program." +
								                Environment.NewLine +
								                "See log file for information on the errors." +
								                Environment.NewLine +
								                Environment.NewLine +
								                "Program cannot continue without this file. Program will terminate.",
								                "AdifEnumerations.xml Error",
								                MessageBoxButton.OK,
								                MessageBoxImage.Error);
								// force program termination
								System.Diagnostics.Process.GetCurrentProcess().Kill();
							}
						}
						ShowSplashPageLabel(newAdifEnumerationsLabel);
					}
					if(newCallsBureausVersion)
					{
						try
						{
							bool wError = App.CallBureaus.DownloadFileFromWebsite();
							webError = webError || wError;
							App.CallBureaus.LoadDocument();
							if(!wError)
							{
								ShowSplashPageLabel(newCallsBureausLabel);
								App.Logger.Log("CallsBureaus.xml version " + App.CallBureaus.Version +
								           " downloaded.");
							}
							else
							{
								ShowSplashPageLabel(cannotDownloadCallsBureausErrorLabel);
							}
						}
						catch(Exception e)
						{
							App.Logger.Log(e);
							ShowSplashPageLabel(newCallsBureausErrorLabel);
							try
							{
								App.CallBureaus.CopyDefaultXmlFile();
								App.CallBureaus.LoadDocument();
								ShowSplashPageLabel(callsBureausErrorLabel);
							}
							catch(Exception ex)
							{
								App.Logger.Log(ex, ExceptionLogger.SHOWTRACE, ExceptionLogger.DONTSHOWMESSAGE);
								MessageBox.Show("Problem accessing downloaded CallsBureaus.xml file." +
								                Environment.NewLine +
								                "Also encountered error attempting to replace CallsBureaus.xml" +
								                Environment.NewLine +
								                "file with default file shipped with the program." +
								                Environment.NewLine +
								                "See log file for information on the errors." +
								                Environment.NewLine +
								                Environment.NewLine +
								                "Program cannot continue without this file. Program will terminate.",
								                "AdifEnumerations.xml Error",
								                MessageBoxButton.OK,
								                MessageBoxImage.Error);
								// force program termination
								System.Diagnostics.Process.GetCurrentProcess().Kill();
							}
						}
					}
				}
				((App)App.Current).UserPreferences.BuildReportByModeList();
				checkingForUpdatesLabel.Visibility = Visibility.Collapsed;
				UpdateUI();
			}
			
			if(directoriesError || newStableVersion || newDevelopmentVersion ||
			   securityException || accessException)		// terminate class error
			{
				ShowTerminateButton();
			}
			if(userPrefsError || showHamqslerLabel || showUserPrefsLabel || webError ||		// info message
						directoriesError || newStableVersion || newDevelopmentVersion ||
						securityException || accessException || newAdifEnumerationsVersion ||
						newCallsBureausVersion || adifError || callsBureausError)
			{
				ShowContinueButton();
			}
			else
			{
				this.Close();
			}
			this.Cursor = oldCursor;
			
		}
		
		/// <summary>
		/// Force an update of the window.
		/// This method was created as the result of a post by Muljadi Budiman and one of the commenters.
		/// http://geekswithblogs.net/NewThingsILearned/archive/2008/08/25/refresh--update-wpf-controls.aspx
		/// </summary>
		private void UpdateUI()
		{
			// while this method could probably be inlined, it is written as a separate method
			// to allow proper crediting and to provide separation in case the method has to
			// be modified. It is called from many locations in the InitializeApplication processing.
			this.Dispatcher.Invoke(DispatcherPriority.ContextIdle, (Action)delegate() {});
		}
		
		/// <summary>
		/// Load or create UserPreferences object and show related labels as appropriate
		/// </summary>
		public void GetUserPreferences()
		{
			((App)Application.Current).GetUserPreferences(
					out showUserPrefsLabel, out userPrefsError);
			if(userPrefsError)			// error reading or writing UserPreferences file
			{
				ShowSplashPageLabel(userPrefsErrorLabel);
			}
			else if(showUserPrefsLabel)		// UserPreferences file has been created
			{
				ShowSplashPageLabel(userPrefsCreatedLabel);
			}			
		}

		/// <summary>
		/// Shows the hamQslerCreatedLabel.
		/// </summary>
		public void ShowSplashPageLabel(Label label)
		{
			label.Visibility = Visibility.Visible;
			UpdateUI();
		}

		/// <summary>
		/// Shows the OkButton.
		/// </summary>
		public void ShowContinueButton()
		{
			okButton.Visibility = Visibility.Visible;
		}
		
		/// <summary>
		/// Shows the termButton.
		/// </summary>
		public void ShowTerminateButton()
		{
			termButton.Visibility = Visibility.Visible;
		}
		
		/// <summary>
		/// Handles okButton clicks - show the main window and close this one
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">RoutedEventArgs object for this event</param>
		void OkButton_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
			e.Handled = true;
		}
		
		/// <summary>
		/// Handles termButton clicks - close this window so that the application terminates
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void TermButton_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
			Application.Current.Shutdown();
			e.Handled = true;
		}
	}
}