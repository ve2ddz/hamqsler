using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
    	
        public JpegPropsDialog()
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
