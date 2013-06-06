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
    /// Interaction logic for Print4UpDialog.xaml
    /// </summary>
    public partial class Print4UpDialog : Window
    {
    	private static readonly DependencyProperty IncludeCardOutlinesProperty =
    		DependencyProperty.Register("IncludeCardOutlines", typeof(bool), typeof(Print4UpDialog),
    		                            new PropertyMetadata(true));
    	public bool IncludeCardOutlines
    	{
    		get {return (bool)GetValue(IncludeCardOutlinesProperty);}
    		set {SetValue(IncludeCardOutlinesProperty, value);}
    	}
    	
    	private static readonly DependencyProperty ResolutionProperty =
    		DependencyProperty.Register("Resolution", typeof(int), typeof(Print4UpDialog),
    		                            new PropertyMetadata(300));
    	public int Resolution
    	{
    		get {return (int)GetValue(ResolutionProperty);}
    		set {SetValue(ResolutionProperty, value);}
    	}
    	
        public Print4UpDialog()
        {
            InitializeComponent();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            this.Close();
        }
    }
}
