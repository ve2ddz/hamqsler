/*
 *  Author:
 *       Jim Orcheson <jimorcheson@gmail.com>
 * 
 *  Copyright (c) 2013 Jim Orcheson
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