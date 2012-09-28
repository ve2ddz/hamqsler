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
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

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
			Thread thr = new Thread(new ThreadStart(InitializeApplication));
			thr.Start();

		}
		
		public delegate void RunDelegate();

		
		private void InitializeApplication()
		{
			// check and create hamqsler directories and copy sample files
			bool directoriesError = false;
			bool showHamqslerLabel = ((App)Application.Current).BuildHamQslerDirectories(ref directoriesError);
			if(directoriesError)		// error occurred creating directories
			{
				this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new
				                       RunDelegate(ShowHamQslerCreatedProblemLabel));
			}
			else if(showHamqslerLabel)		// created directories/copied files
			{
				this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new RunDelegate(
					ShowHamQslerCreatedLabel));
//				});
			}
			// create ExceptionLogger
			((App)Application.Current).CreateExceptionLogger();
			((App)Application.Current).LogRuntimeInfo();		// output run start info
			// load existing UserPreferences file, or create new one
			bool userPrefsError = false;
			bool showUserPrefsLabel = false;
/*			userPrefs = MainClass.GetUserPreferences(logger, out showUserPrefsLabel, out userPrefsError);
			if(userPrefsError)			// error reading or writing UserPreferences file
			{
				splash.ShowUserPrefsErrorLabel();
			}
			else if(showUserPrefsLabel)		// UserPreferences file has been created
			{
				splash.ShowUserPrefsCreatedLabel();
			}*/
			// check for new program version and data file updates
			bool webError = false;
			bool newHamQslerVersion = false;
/*			splash.ShowCheckingForUpdatesLabel();
			// updates will contain program and file names with most recent versions available for download
			Dictionary<string, string>updates = MainClass.GetProgramVersions(logger, out webError);
			if(webError)		// error retrieving file containing version info
			{
				splash.ShowWebErrorLabel();
			}
			else
			{
				// have version info, so check if any updates needed
				foreach(string key in updates.Keys)
				{
					switch(key)
					{
					case "HamQsler":
						newHamQslerVersion = CheckHamQslerVersion(updates[key]);
						if(newHamQslerVersion)
						{
							splash.ShowNewHamQslerVersionLabel();
						}
						break;
					}
				}
			}
			splash.HideCheckingForUpdatesLabel();
			
			if(!directoriesError && !userPrefsError && !showHamqslerLabel &&
					!showUserPrefsLabel && !webError && !newHamQslerVersion)		// done initializing so start up MainWindow
			{
				Application.Invoke(delegate {
					CreateAndShowMainWindow();
				});
			} */
			if(directoriesError || newHamQslerVersion)		// terminate class error
			{
				this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new RunDelegate(
					ShowTerminateButton));
			}
			if(userPrefsError || showHamqslerLabel || showUserPrefsLabel || webError ||		// info message
						directoriesError || newHamQslerVersion)
			{
				this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new RunDelegate(
					ShowContinueButton));
			}
			
		}

		/// <summary>
		/// Shows the hamQslerCreatedLabel.
		/// </summary>
		public void ShowHamQslerCreatedLabel()
		{
			hamqslerCreatedLabel.Visibility = Visibility.Visible;
		}
		
		/// <summary>
		/// Shows the hamQslerCreatedProblemLabel.
		/// </summary>
		public void ShowHamQslerCreatedProblemLabel()
		{
			hamqslerCreatedProblemLabel.Visibility = Visibility.Visible;
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
			App app = (App)Application.Current;
			app.ShowMainWindow();
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