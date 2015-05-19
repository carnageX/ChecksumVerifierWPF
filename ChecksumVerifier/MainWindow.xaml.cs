using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

namespace ChecksumVerifier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Class Properties
        MainViewModel vm { get; set; }
        #endregion

        #region MainWindow Methods
        public MainWindow()
        {
            InitializeComponent();
            this.SF_rtbFileHash.TargetUpdated += SF_rtbFileHash_TargetUpdated;
            this.SF_lblResult.TargetUpdated += SF_lblResult_TargetUpdated;
            this.vm = new MainViewModel();
            this.DataContext = vm;
        }
        #endregion

        #region Single File Events
        void SF_rtbFileHash_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            if(!String.IsNullOrWhiteSpace(vm.SF_TxtUserHash))
            {
                e.Handled = SF_HandleColorOnText();
            }
        }

        void SF_lblResult_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            e.Handled = SF_HandleResultColor();
        }

        private bool SF_HandleResultColor()
        {
            if(this.SF_lblResult.Content != null && !String.IsNullOrEmpty(this.SF_lblResult.Content.ToString()))
            {
                string resultString = this.SF_lblResult.Content.ToString();
                if(resultString.Substring(0, 6).Equals("Error!"))
                {
                    this.SF_lblResult.Foreground = Brushes.Red;
                }
                else
                {
                    this.SF_lblResult.Foreground = Brushes.Green;
                }
                return true;
            }
            return false;
        }


        private bool SF_HandleColorOnText()
        {
            TextRange tr = new TextRange(this.SF_rtbFileHash.Document.ContentStart, this.SF_rtbFileHash.Document.ContentEnd);
            if(!String.IsNullOrEmpty(tr.Text))
            {
                string[] substrings = tr.Text.Split('\r');
                this.SF_rtbFileHash.Document.Blocks.Clear();

                foreach(var s in substrings)
                {
                    if(!String.IsNullOrWhiteSpace(s))
                    { 
                        Paragraph p = new Paragraph();
                        p.Inlines.Add(s.Trim());
                        if(s.Contains("Valid"))
                        {
                            p.Foreground = Brushes.Green;
                        }
                        else
                        {
                            p.Foreground = Brushes.Red;
                        }
                        this.SF_rtbFileHash.Document.Blocks.Add(p);
                    }
                }
                return true;
            }
            return false;
        }

        private void SF_txtUserHash_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(this.SF_txtUserHash.Text))
            {
                //this.SF_btnCompare.Content = "";
                vm.SF_BtnCompareText = "Generate";
            }
            else
            {
                vm.SF_BtnCompareText = "Compare";
            }
        }
        #endregion

        #region Multiple File Events
        private void MF_txtUserHash_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(this.MF_txtUserHash.Text))
            {
                vm.MF_BtnCompareText = "Generate";
            }
            else
            {
                vm.MF_BtnCompareText = "Compare";
            }
        }
        #endregion
    }
}
