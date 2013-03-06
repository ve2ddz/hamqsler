/*
 * Created by SharpDevelop.
 * User: Jim
 * Date: 06/03/2013
 * Time: 10:53 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using QslBureaus;
using Qsos;

namespace hamqsler
{
	/// <summary>
	/// Interaction logic for AboutDialog.xaml
	/// </summary>
	public partial class AboutDialog : Window
	{
		private static readonly DependencyProperty HamQSLerVersionProperty =
			DependencyProperty.Register("HamQSLerVersion", typeof(string), typeof(AboutDialog),
			                            new PropertyMetadata("not specified"));
		public string HamQSLerVersion
		{
			get {return (string)GetValue(HamQSLerVersionProperty);}
			set {SetValue(HamQSLerVersionProperty, value);}
		}
		
		private static readonly DependencyProperty QsosLibraryVersionProperty =
			DependencyProperty.Register("QsosLibraryVersion", typeof(string), typeof(AboutDialog),
			                            new PropertyMetadata("not specified"));
		public string QsosLibraryVersion
		{
			get {return (string)GetValue(QsosLibraryVersionProperty);}
			set {SetValue(QsosLibraryVersionProperty, value);}
		}
		
		private static readonly DependencyProperty QslBureausLibraryVersionProperty =
			DependencyProperty.Register("QslBureausLibraryVersion", typeof(string), typeof(AboutDialog),
			                            new PropertyMetadata("not specified"));
		public string QslBureausLibraryVersion
		{
			get {return (string)GetValue(QslBureausLibraryVersionProperty);}
			set {SetValue(QslBureausLibraryVersionProperty, value);}
		}
		
		private static readonly DependencyProperty CLRVersionProperty =
			DependencyProperty.Register("CLRVersion", typeof(string), typeof(AboutDialog),
			                            new PropertyMetadata("not specified"));
		public string CLRVersion
		{
			get {return (string)GetValue(CLRVersionProperty);}
			set {SetValue(CLRVersionProperty, value);}
		}
		
		private static readonly DependencyProperty CopyrightProperty =
			DependencyProperty.Register("Copyright", typeof(string), typeof(AboutDialog),
			                            new PropertyMetadata("not specified"));
		public string Copyright
		{
			get {return (string)GetValue(CopyrightProperty);}
			set {SetValue(CopyrightProperty, value);}
		}

		
		/// <summary>
		/// Constructor
		/// </summary>
		public AboutDialog()
		{
			object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(
				typeof(AssemblyCopyrightAttribute), false);
			if(attributes.Length == 0)
			{
				Copyright = string.Empty;
			}
			else
			{
				Copyright = ((AssemblyCopyrightAttribute)attributes[0]).Copyright;
			}
			
			HamQSLerVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
			Assembly assembly = Assembly.GetAssembly(new QslBureau().GetType());
			QslBureausLibraryVersion = assembly.GetName().Version.ToString();
			AdifString aStr = new AdifString("<eor>");
			Type t = aStr.GetType();
			Assembly asm = Assembly.GetAssembly(t);
			QsosLibraryVersion = asm.GetName().Version.ToString();
			Version ver = Environment.Version;
			CLRVersion = string.Format("{0}.{1}.{2}.{3}", ver.Major, ver.Minor, ver.Build, ver.Revision);

			InitializeComponent();
		}

		/// <summary>
		/// Handles Close button clicked event
		/// </summary>
		/// <param name="sender">not used</param>
		/// <param name="e">not used</param>
		private void Close_Clicked(object sender, RoutedEventArgs e)
		{
			DialogResult = true;
            this.Close();

		}
	}
}