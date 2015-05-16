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
        MainViewModel vm { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            //this.SF_tbFileHash.TargetUpdated += SF_tbFileHash_TargetUpdated;
            this.SF_rtbFileHash.TargetUpdated += SF_rtbFileHash_TargetUpdated;
            this.SF_lblResult.TargetUpdated += SF_lblResult_TargetUpdated;
            this.vm = new MainViewModel();
            this.DataContext = vm;
        }

        void SF_rtbFileHash_TargetUpdated(object sender, DataTransferEventArgs e)
        {
            //e.Handled = SF_HandleColorOnText();
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

        //void SF_tbFileHash_TargetUpdated(object sender, DataTransferEventArgs e)
        //{
        //    e.Handled = SF_HandleColorOnText();
        //}

        //private bool SF_HandleColorOnText()
        //{
        //    if (!string.IsNullOrWhiteSpace(this.SF_tbFileHash.Text) && !string.IsNullOrEmpty(this.vm.SF_TxtUserHash))
        //    {
        //        string[] substrings = this.SF_tbFileHash.Text.Split('\r');
        //        this.SF_tbFileHash.Inlines.Clear();

        //        string[] subStrings = this.SF_rtbFileHash.Document.

        //        foreach (var item in substrings)
        //        {
        //            if (item.Contains("Valid"))
        //            {
        //                Run runx = new Run(item);
        //                runx.Foreground = Brushes.Green;
        //                this.SF_tbFileHash.Inlines.Add(runx);
        //            }
        //            else
        //            {
        //                Run runx = new Run(item);
        //                runx.Foreground = Brushes.Red;
        //                this.SF_tbFileHash.Inlines.Add(runx);
        //            }
        //            this.SF_tbFileHash.Inlines.Add("\r");
        //        }
        //        return true;
        //    }
        //    return false;
        //}

        


        //#region Properties
        //public List<string> Algorithms
        //{ get; set; }
        //public List<string> SelectedAlgorithms
        //{ get; set; }
        //#endregion

        //#region Form Methods
        //public MainWindow()
        //{
        //    InitializeComponent();
        //    this.SF_lblResult.Visibility = System.Windows.Visibility.Hidden;
        //    this.SF_lblfileSize.Content = "";
        //    this.SF_btnCompare.Content = "Generate";
        //    //this.MF_btnCompare.Content = "Generate";
        //    PopulateAlgorithmList(Algorithms, SelectedAlgorithms);
        //}//MainWindow
        //#endregion

        //#region Single File Compare
        ///// <summary>SF File browse button click</summary>
        ///// <param name="sender">Sender</param>
        ///// <param name="e">Argument</param>
        //private void SF_btnBrowse_Click(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog SF_openFile = new OpenFileDialog();
        //    SF_openFile.ShowDialog();
        //    string filename = SF_openFile.FileName;
        //    if (!String.IsNullOrWhiteSpace(filename))
        //    {
        //        FileInfo fileInfo = new FileInfo(filename);
        //        this.SF_lblfileSize.Content = String.Format("({0} KB)", (fileInfo.Length / 1024 + 1));
        //        this.SF_txtFilePath.Text = filename;
        //    }
        //    SF_progressBar.Value = 0;
        //}

        ///// <summary>SF Compare button click</summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void SF_btnCompare_Click(object sender, RoutedEventArgs e)
        //{
        //    this.SF_lblResult.Visibility = System.Windows.Visibility.Hidden;
        //    this.SF_lblResult.Foreground = Brushes.Black;

        //    if (this.SF_txtFilePath.Text.Trim() != String.Empty)
        //    {
        //        try
        //        {
        //            this.SF_rtbTextFileHash.Foreground = Brushes.Black;
        //            this.SF_rtbTextFileHash.Document.Blocks.Clear();
        //            this.Cursor = Cursors.Wait;
        //            this.SF_progressBar.Value += 99;

        //            List<string> selectedAlgorithms = GetSelectedAlgorithms(this.ccblistChecksums);
        //            List<string> hashList = ChecksumVerifierLogic.GetHash(this.SF_txtFilePath.Text, selectedAlgorithms);

        //            this.SF_progressBar.Value += 1;
        //            this.Cursor = Cursors.Arrow;

        //            if (String.IsNullOrWhiteSpace(this.SF_txtUserHash.Text))
        //            {
        //                for (int i = 0; i < hashList.Count; i++)
        //                {
        //                    this.SF_rtbTextFileHash.AppendText(String.Format("{0} == {1}{2}", selectedAlgorithms[i], hashList[i], "\r"));
        //                }//for
        //            }//if
        //            else
        //            {
        //                for (int i = 0; i < hashList.Count; i++)
        //                {
        //                    //TODO: newline not working for rich text box
        //                    if (ChecksumVerifierLogic.CompareHashes(this.SF_txtUserHash.Text, hashList[i]))
        //                    {
        //                        SF_AppendToResultsColored(String.Format("Valid! Matching {2} checksum: {0}{1}", hashList[i], "\r", selectedAlgorithms[i]), Brushes.Green);
        //                    }//if
        //                    else
        //                    {
        //                        SF_AppendToResultsColored(String.Format("Invalid! Mismatched {2} checksum: {0}{1}", hashList[i], "\r", selectedAlgorithms[i]), Brushes.Red);
        //                    }//else
        //                }//for
        //            }//else
        //            this.SF_lblResult.Foreground = Brushes.Green;
        //            this.SF_lblResult.Content = "Finished!";
        //            this.SF_lblResult.Visibility = System.Windows.Visibility.Visible;
        //        }//try
        //        catch (Exception ex)
        //        {
        //            this.SF_lblResult.Content = "Error! " + ex.Message;
        //            this.SF_lblfileSize.Content = "";
        //            this.SF_lblResult.Visibility = System.Windows.Visibility.Visible;
        //            this.Cursor = Cursors.Arrow;
        //            this.SF_progressBar.Value = 0;
        //        }//catch
        //    }//if
        //    else
        //    {
        //        this.SF_lblResult.Content = "Error!  No file to calculate checksum!";
        //        this.SF_lblResult.Foreground = Brushes.Red;
        //        this.SF_lblResult.Visibility = System.Windows.Visibility.Visible;;
        //    }//else
        //}

        //private void SF_AppendToResultsColored(string appendString, Brush lineColor)
        //{
        //    SF_rtbTextFileHash.Dispatcher.Invoke(
        //        (Action)(() =>
        //        {
        //            SF_rtbTextFileHash.Selection.Select(SF_rtbTextFileHash.Document.ContentEnd, SF_rtbTextFileHash.Document.ContentEnd);
        //            SF_rtbTextFileHash.Selection.Text = appendString;
        //            SF_rtbTextFileHash.Selection.ApplyPropertyValue(TextElement.ForegroundProperty, lineColor);
        //        })
        //    );
        //}

        //private void ccblistChecksums_ItemSelectionChanged(object sender, Xceed.Wpf.Toolkit.Primitives.ItemSelectionChangedEventArgs e)
        //{
        //    this.ccblistChecksums.ItemsSource = this.Algorithms;
        //    StringBuilder sb = new StringBuilder();
        //    foreach (var item in this.ccblistChecksums.SelectedItems)
        //    {
        //        sb.Append(item.ToString());
        //        sb.Append(this.ccblistChecksums.Delimiter);
        //    }
        //    sb = sb.Remove(sb.Length - 1, 1);
        //    this.ccblistChecksums.Text = sb.ToString();
        //}

        //private void SF_txtUserHash_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    if (String.IsNullOrWhiteSpace(SF_txtUserHash.Text))
        //    {
        //        this.SF_btnCompare.Content = "Generate";
        //    }
        //    else
        //    {
        //        this.SF_btnCompare.Content = "Compare";
        //    }
        //}
        //#endregion

        //#region Multiple Files Compare

        //#endregion

        //#region Helper Methods
        ///// <summary>Populate the list of ComboCheckBox with given algorithms</summary>
        ///// <param name="algorithms">String list of algorithms</param>
        //private void PopulateAlgorithmList(List<string> algorithms, List<string> initialSelectedItems)
        //{
        //    //this.ccblistChecksums.ItemsSource = algorithms;
        //    //this.ccblistChecksums.SelectedItemsOverride = initialSelectedItems;

        //    this.ccblistChecksums.DataContext = this;
        //    this.Algorithms = new List<string>() { "MD5", "SHA-1", "SHA-256", "SHA-384", "SHA-512", "RIPEMD160", "CRC16", "CRC32" };
        //    this.SelectedAlgorithms = new List<string>() { this.Algorithms[0] };
        //}//PopulateAlgorithmList

        ///// <summary>Gets the names of the checked algorithms.</summary>
        ///// <param name="algorithmList">List of algorithms from dropdown</param>
        ///// <returns>String list of selected algorithms</returns>
        //private List<string> GetSelectedAlgorithms(CheckComboBox algorithmList)
        //{
        //    List<string> strSelectedAlgorithms = new List<string>();
        //    foreach(var item in algorithmList.SelectedItems)
        //    {
        //        strSelectedAlgorithms.Add(item.ToString());
        //    }
        //    return strSelectedAlgorithms;
        //}

        ///// <summary>String Predicate that matches if the given string mathes an empty string</summary>
        ///// <param name="s">Input string</param>
        ///// <returns>True or False</returns>
        //private static bool IsEmptyString(string s)
        //{
        //    return s.Equals(String.Empty);
        //}//IsEmptyString


        //#endregion
    }
}
