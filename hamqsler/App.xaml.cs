using System;
using System.Windows;
using System.Data;
using System.IO;
using System.Threading;
using System.Windows.Threading;
using System.Xml;
using System.Configuration;

namespace hamqsler
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private SplashPage splash;
		
		void Application_Startup(object sender, StartupEventArgs e)
		{
			// create and show SplashPage
			splash = new SplashPage();
			splash.ShowDialog();
		}
		
		/// <summary>
		/// Create and show the program's main window
		/// This should be called only once, from the SplashPage when it is closing
		/// </summary>
		internal void ShowMainWindow()
		{
			MainWindow win = new MainWindow();
			win.Show();
		}
		
		/// <summary>
		/// Called if an exception is not handled elsewhere.
		/// Only thing to do is maybe display a message, log the exception, and terminate
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
		{
			// TODO: Inplement DispatcherUnhandledException using ExceptionLogger
			// Close each window so that program will terminate.
			// This call is needed because ShutDownMode is set to LastWindowClose
			foreach (Window w in this.Windows)
			{
				w.Hide();
			}
			this.Shutdown();
		}

			/// <summary>
		/// Helper method that creates the hamqsler directory and subdirectories in the user's 
		/// 'Documents' directory (in My Documents directory in Windows, in home directory in Unix systems)
		/// </summary>
		public bool BuildHamQslerDirectories(ref bool terminate)
		{
			bool showHamqslerCreatedLabel = false;
			try
			{
				// check if hamqsler directory exists and create it if it doesn't
				string hamqsler = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/hamqsler";
				DirectoryInfo qslsInfo = new DirectoryInfo(hamqsler);
				if (!qslsInfo.Exists)
				{
					qslsInfo.Create();
					showHamqslerCreatedLabel = true;
				}
				// check if Printing subdirectory exists and create it if it doesn't
				DirectoryInfo printingDirInfo = new DirectoryInfo(hamqsler + "/Printing");
				if(!printingDirInfo.Exists)
				{
					printingDirInfo.Create();
					showHamqslerCreatedLabel = true;
				}
				// check if Logs subdirectory exists and create it if it doesn't
				DirectoryInfo logsDirInfo = new DirectoryInfo(hamqsler + "/Logs");
				if(!logsDirInfo.Exists)
				{
					logsDirInfo.Create();
					showHamqslerCreatedLabel = true;
				}
				// check if Samples directory exists and create it if it doesn't
				DirectoryInfo samplesDirInfo = new DirectoryInfo(hamqsler + "/Samples");
				if (!samplesDirInfo.Exists)
				{
					samplesDirInfo.Create();
					showHamqslerCreatedLabel = true;
				}
				// copy Samples files if they don't exist
				DirectoryInfo samplesInfo = new DirectoryInfo(
							Environment.CurrentDirectory + "/Samples");
				if (!samplesInfo.Exists)
				{
					return showHamqslerCreatedLabel;
				}
				FileInfo[] files = samplesInfo.GetFiles();
				foreach (FileInfo file in files)
				{
					FileInfo fInfo = new FileInfo(samplesDirInfo.FullName + "/" + file.Name);
					if (!fInfo.Exists)
					{
						file.CopyTo(fInfo.FullName);
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
		}	}
}