using System;
using System.Windows;
using System.Data;
using System.Xml;
using System.Configuration;

namespace hamqsler
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		
		void Application_Startup(object sender, StartupEventArgs e)
		{
			// create and show SplashPage
			SplashPage splash = new SplashPage();
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
			throw new Exception();
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
//			this.Shutdown();
		}
	}
}