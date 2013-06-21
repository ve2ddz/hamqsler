/*
 *  Author:
 *       Jim Orcheson <jimorcheson@gmail.com>
 * 
 *  Copyright © 2013 Jim Orcheson
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
using System.Linq;
using System.Windows;

namespace hamqsler
{
    /// <summary>
    /// Interaction logic for JpegPropsDialog.xaml
    /// </summary>
    public partial class JpegPropsDialog : Window
    {
    	private static readonly DependencyProperty ShowQsosProperty =
    		DependencyProperty.Register("ShowQsos", typeof(bool), typeof(JpegPropsDialog),
    		                            new PropertyMetadata(false));
    	public bool ShowQsos
    	{
    		get {return (bool)GetValue(ShowQsosProperty);}
    		set {SetValue(ShowQsosProperty, value);}
    	}
    	
    	private static readonly DependencyProperty ResolutionProperty =
    		DependencyProperty.Register("Resolution", typeof(int), typeof(JpegPropsDialog),
    		                            new PropertyMetadata(96));
    	public int Resolution
    	{
    		get {return (int)GetValue(ResolutionProperty);}
    		set {SetValue(ResolutionProperty, value);}
    	}
    	
    	private static readonly DependencyProperty QualityProperty =
    		DependencyProperty.Register("Quality", typeof(int), typeof(JpegPropsDialog),
    		                            new PropertyMetadata(85));
    	public int Quality
    	{
    		get {return (int)GetValue(QualityProperty);}
    		set {SetValue(QualityProperty, value);}
    	}
    	
    	/// <summary>
    	/// Constructor
    	/// </summary>
    	/// <param name="enableShowQSOs">Boolean indicating if the Show QSOs checkbox
    	/// should be enabled</param>
        public JpegPropsDialog(bool enableShowQSOs)
        {
            InitializeComponent();
            QsosCheckbox.IsEnabled = enableShowQSOs;
        }

        /// <summary>
        /// Handler for OK button click event
        /// </summary>
        /// <param name="sender">not used</param>
        /// <param name="e">not used</param>
        private void OK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }
    }
}
