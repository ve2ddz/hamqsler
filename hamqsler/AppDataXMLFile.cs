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
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace hamqsler
{
	/// <summary>
	/// Description of AppDataXMLFile.
	/// </summary>
	public class AppDataXMLFile
	{
		private string xmlFile = null;
		
		protected XDocument xDoc = null;
		
		private const int WEBREQUESTTIMEOUT = 10000;	// 10 seconds
		
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="fName">name of the XML file</param>
		public AppDataXMLFile(string fName)
		{
			xmlFile = fName;
		}
		
		/// <summary>
		/// Copies the XML file stored within the program assembly to AppData
		/// </summary>
		public void CopyDefaultXmlFile()
		{
			string appDataDirName =  Path.Combine(Environment.GetFolderPath(
						Environment.SpecialFolder.ApplicationData), "HamQSLer");
			DirectoryInfo appDataFolder = new DirectoryInfo(appDataDirName);
			FileInfo fInfo = new FileInfo(appDataFolder.FullName + "/" + xmlFile);
			Assembly assembly = Assembly.GetAssembly((new AdifField()).GetType());
        	Stream str = assembly.GetManifestResourceStream("hamqsler." + xmlFile);
        	FileStream fStream = File.Open(fInfo.FullName, FileMode.Create);
        	byte[] bytes = new Byte[str.Length];
        	str.Read(bytes, 0, (int)str.Length);
        	fStream.Write(bytes, 0, (int)str.Length);
        	fStream.Close();
        	str.Close();
		}
		
		/// <summary>
		/// Load data from XML file
		/// </summary>
		public void LoadDocument()
		{
			string fileName = Path.Combine(Environment.GetFolderPath(
				Environment.SpecialFolder.ApplicationData), @"HamQSLer\" + xmlFile);
			Stream str = new FileStream(fileName, FileMode.Open);
			xDoc = XDocument.Load(str);
			str.Close();
		}
		
		/// <summary>
		/// Load data from XML file
		/// </summary>
		public void LoadDocument(Stream str)
		{
			xDoc = XDocument.Load(str);
		}
		
		/// <summary>
		/// Download configuration file from the website
		/// </summary>
		/// <returns>true if an error occurred retrieving the file, false otherwise</returns>
		public bool DownloadFileFromWebsite()
		{
			bool webError = false;
            // create request for website
            HttpWebRequest httpRequest = null;
            HttpWebResponse response = null;
            StreamReader resStream = null;
			// get this program's version info
	        Version ver = Assembly.GetExecutingAssembly().GetName().Version;
            string location = string.Format("http://www.va3hj.ca/{0}", xmlFile);
            try
            {
            	// build the http request
                httpRequest = (HttpWebRequest)WebRequest.Create(location);
                httpRequest.Timeout = WEBREQUESTTIMEOUT;
                string proxy = ((App)App.Current).UserPreferences.HttpProxyServer;
				int proxyPort = ((App)App.Current).UserPreferences.HttpProxyServerPortNumber;
                if (proxy != string.Empty)
                {
                    httpRequest.Proxy = new WebProxy(proxy, proxyPort);
                }
				// get OS information for including in UserAgent
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
                
                // copy file to AppData folder
				string appDataDirName =  Path.Combine(Environment.GetFolderPath(
							Environment.SpecialFolder.ApplicationData), "HamQSLer");
				DirectoryInfo appDataFolder = new DirectoryInfo(appDataDirName);
				FileInfo fInfo = new FileInfo(appDataFolder.FullName + "/" + xmlFile);
	        	string adifStr = resStream.ReadToEnd();
	        	File.WriteAllText(fInfo.FullName, adifStr);
            }
            catch (SecurityException e)
            {
                App.Logger.Log(e);
				webError = true;
            }
            catch (WebException e)
            {
            	((App)App.Current).DecodeWebException(e, xmlFile);
				webError = true;
            }
            catch (Exception e)
            {
                App.Logger.Log("Error retrieving " + xmlFile + " from website: " + e.Message);
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
 			return webError;
		}
	}
}
