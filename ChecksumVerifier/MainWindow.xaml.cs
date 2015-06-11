using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

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
            this.lblResult.TargetUpdated += SF_lblResult_TargetUpdated;
            this.TS_rtbFileHash.TargetUpdated += TS_rtbFileHash_TargetUpdated;

            this.vm = new MainViewModel();
            this.DataContext = vm;
            vm.MF_Progress_Max = 1;
            this.TS_cbEncodingType.SelectedIndex = 0;
        }
        #endregion

        #region Statusbar Events
        void SF_lblResult_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            e.Handled = SF_HandleResultColor();
        }

        private bool SF_HandleResultColor()
        {
            if (this.lblResult.Content != null && !String.IsNullOrEmpty(this.lblResult.Content.ToString()))
            {
                string resultString = this.lblResult.Content.ToString();
                if (resultString.Contains("Error"))
                {
                    this.lblResult.Foreground = Brushes.Red;
                }
                else if(resultString.Contains("Finished"))
                {
                    this.lblResult.Foreground = Brushes.Green;
                }
                else
                {
                    this.lblResult.Foreground = Brushes.Black;
                }
                return true;
            }
            return false;
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

        private bool SF_HandleColorOnText()
        {
            TextRange tr = new TextRange(this.SF_rtbFileHash.Document.ContentStart, this.SF_rtbFileHash.Document.ContentEnd);
            if (!String.IsNullOrEmpty(tr.Text))
            {
                string[] substrings = tr.Text.Split('\r');
                this.SF_rtbFileHash.Document.Blocks.Clear();

                foreach(var s in substrings)
                {
                    if(!String.IsNullOrWhiteSpace(s))
                    { 
                        Paragraph p = new Paragraph();
                        p.Margin = new Thickness(0);
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

        private void MF_LbResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vm.MF_SelectedResultIndex = this.MF_LbResults.SelectedIndex;
        }
        #endregion

        #region Text Single Events
        void TS_rtbFileHash_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(vm.TS_TxtUserHash))
            {
                e.Handled = TS_HandleColorOnText();
            }
        }

        private bool TS_HandleColorOnText()
        {
            TextRange tr = new TextRange(this.TS_rtbFileHash.Document.ContentStart, this.TS_rtbFileHash.Document.ContentEnd);
            if (!String.IsNullOrEmpty(tr.Text))
            {
                string[] substrings = tr.Text.Split('\r');
                this.TS_rtbFileHash.Document.Blocks.Clear();

                foreach (var s in substrings)
                {
                    if (!String.IsNullOrWhiteSpace(s))
                    {
                        Paragraph p = new Paragraph();
                        p.Margin = new Thickness(0);
                        p.Inlines.Add(s.Trim());
                        if (s.Contains("Valid"))
                        {
                            p.Foreground = Brushes.Green;
                        }
                        else
                        {
                            p.Foreground = Brushes.Red;
                        }
                        this.TS_rtbFileHash.Document.Blocks.Add(p);
                    }
                }
                return true;
            }
            return false;
        }

        private void TS_txtUserHash_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(this.TS_txtUserHash.Text))
            {
                vm.TS_BtnCompareText = "Generate";
            }
            else
            {
                vm.TS_BtnCompareText = "Compare";
            }
        }
        #endregion
    }
}
