using System;
using System.Windows;
using System.Data;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security;
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
		private static ExceptionLogger logger;
		public static ExceptionLogger Logger
		{
			get {return logger;}
		}
		
		private UserPreferences userPrefs;
		public UserPreferences UserPreferences
		{
			get {return userPrefs;}
			set {userPrefs = value;}
		}
		
		/// <summary>
		/// Application startup code
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
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
		/// <param name="sender">not used</param>
		/// <param name="e">DispatcherUnhandledExceptionEventArgs object</param>
		void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
		{
			// TODO: Inplement DispatcherUnhandledException using ExceptionLogger
			if(logger != null)
			{
				logger.Log(e.Exception, ExceptionLogger.SHOWTRACE, ExceptionLogger.DONTSHOWMESSAGE);
			}
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
		/// Create the ExceptionLogger
		/// </summary>
		internal void CreateExceptionLogger()
		{
			logger = new ExceptionLogger(Environment.GetFolderPath(
					Environment.SpecialFolder.MyDocuments) + "/hamqsler/Logs/hamqsler.log");
		}
		
		/// <summary>
		/// Log the application's runtime information to the exception logger
		/// </summary>
		internal void LogRuntimeInfo()
		{
			string os = Environment.OSVersion.VersionString;
/*			// get version of QsosLibary
			// AdifString object only used so that I can get the Type for it to get
			// the Assembly it is in
			Assembly assembly = Assembly.GetAssembly(new QslBureau().GetType());
			AssemblyName aName2 = assembly.GetName();
			AdifString aStr = new AdifString("<eor>");
			Type t = aStr.GetType();
			Assembly asm = Assembly.GetAssembly(t);
			AssemblyName aName = asm.GetName(); */
			// get CLR version info
			Version ver = Environment.Version;
			// get newline for this platform
			string newline = Environment.NewLine;
			// output start log message
			logger.Log(string.Format("HamQSLer started" + newline +
			                         "HamQSLer version: {6}" + newline +
//			                         "QsosLibrary version: {8}" + newline +
//			                         "QslBureaus version: {9}" + newline +
			                         "CLR version: {2}.{3}.{4}.{5}" + newline +
			                         "OS: {0}" + newline + 
			                         "Processors: {1}" + newline +
			                         "Culture: {7}" + newline,
			                         os,
			                         Environment.ProcessorCount, 
									 ver.Major, ver.Minor, ver.Build, ver.Revision,
			                         Assembly.GetExecutingAssembly().GetName().Version.ToString(),
			                         CultureInfo.CurrentCulture.Name //,
//			                         aName.Version.ToString(),
//			                         aName2.Version.ToString()
			                ));
			
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
			userPrefs = UserPreferences.CreateUserPreferences(logger, false, out userPrefsIntialized,
			                                                  out userPrefsError);
		}
		
	}
}