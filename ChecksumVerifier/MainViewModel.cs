using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChecksumVerifier
{
    class MainViewModel : ViewModelBase
    {
        public List<string> Algorithms { get; set; }
        public ObservableCollection<string> SelectedAlgorithms { get; set; }

        #region Single File
        public MainViewModel()
        {
            this.Algorithms = new List<string>() { "MD5", "SHA-1", "SHA-256", "SHA-384", "SHA-512", "RIPEMD160", "CRC16", "CRC32" };
            this.SelectedAlgorithms = new ObservableCollection<string>();
            this.SelectedAlgorithms.Add(this.Algorithms[0]);
        }

        public string SF_TxtUserHash
        {
            get { return this._SF_txtUserHash; }
            set
            {
                if (this._SF_txtUserHash != value)
                {
                    this._SF_txtUserHash = value;
                    this.SF_BtnCompareText = "Compare";
                    RaisePropertyChanged("SF_TxtUserHash");
                }
            }
        }//TxtUserHash
        public string _SF_txtUserHash;

        public string SF_TxtFilePath
        {
            get { return this._SF_txtFilePath; }
            set
            {
                if (this._SF_txtFilePath != value)
                {
                    this._SF_txtFilePath = value;
                    RaisePropertyChanged("SF_TxtFilePath");
                }
            }
        }//TxtFilePath
        public string _SF_txtFilePath;

        public string SF_LblFileSize
        {
            get 
            {
                if (String.IsNullOrEmpty(this._SF_txtFilePath))
                    this.SF_LblFileSize = String.Empty;
                return this._SF_lblFileSize; 
            }
            set
            {
                if (_SF_lblFileSize != value)
                {
                    this._SF_lblFileSize = value;
                    RaisePropertyChanged("SF_LblFileSize");
                }
            }
        }//LblFileSize
        public string _SF_lblFileSize;

        public string SF_BtnCompareText
        {
            get
            {
                if (String.IsNullOrEmpty(this._SF_btnCompareText))
                    this.SF_BtnCompareText = "Generate";
                return this._SF_btnCompareText;
            }
            set
            {
                if (this._SF_btnCompareText != value)
                {
                    this._SF_btnCompareText = value;
                    RaisePropertyChanged("SF_BtnCompareText");
                }
            }
        }//BtnCompareText
        public string _SF_btnCompareText;

        public string SF_TbFileHash
        {
            get { return this._SF_tbFileHash; }
            set
            {
                if (this._SF_tbFileHash != value)
                {
                    this._SF_tbFileHash = value;
                    RaisePropertyChanged("SF_TbFileHash");
                }
            }
        }//TbFileHash
        public string _SF_tbFileHash;

        public string SF_LblResult
        {
            get { return _SF_lblResult; }
            set
            {
                if(_SF_lblResult != value)
                {
                    _SF_lblResult = value;
                    RaisePropertyChanged("SF_LblResult");
                }
            }
        }
        public string _SF_lblResult;

        public int SF_Progress
        {
            get { return _SF_progress; }
            set
            {
                if(_SF_progress != value)
                {
                    _SF_progress = value;
                    RaisePropertyChanged("SF_Progress");
                }
            }
        }
        public int _SF_progress;
        //progress bar

        #region SF Browse File
        public ICommand SF_CmdBrowse
        {
            get
            {
                if (this._SF_cmdBrowse == null)
                    this._SF_cmdBrowse = new RelayCommand(SF_BrowseFile, SF_CanBrowseFile);
                return this._SF_cmdBrowse;
            }
        }//CmdBrowse
        private RelayCommand _SF_cmdBrowse;

        private bool SF_CanBrowseFile()
        {
            return true;
        }

        private void SF_BrowseFile()
        {
            OpenFileDialog SF_openDialog = new OpenFileDialog();
            if(SF_openDialog.ShowDialog() == true)
            {
                this.SF_TxtFilePath = SF_openDialog.FileName;
                FileInfo fileInfo = new FileInfo(this.SF_TxtFilePath);
                this.SF_LblFileSize = String.Format("(File Size: {0} KB)", (fileInfo.Length / 1024 + 1));
            }
        }//SF_BrowseFile
        #endregion

        #region SF Compare Hash
        public ICommand SF_CmdCompare
        {
            get
            {
                if (this._SF_cmdCompare == null)
                    this._SF_cmdCompare = new RelayCommand(SF_Compare, SF_CanCompare);
                return this._SF_cmdCompare;
            }
        }
        private RelayCommand _SF_cmdCompare;

        private bool SF_CanCompare()
        {
            return true;
        }

        private void SF_Compare()
        {
            if(!String.IsNullOrWhiteSpace(this.SF_TxtFilePath))
            {
                try
                {
                    this.SF_Progress = 50;
                    //this.SF_LblResult = String.Empty;
                    StringBuilder sbCompareResults = new StringBuilder();
                    List<string> hashList = ChecksumVerifierLogic.GetHash(this.SF_TxtFilePath, this.SelectedAlgorithms.ToList());
                    this._SF_progress += 40;
                    if (String.IsNullOrWhiteSpace(this.SF_TxtUserHash))
                    {
                        for (int i = 0; i < hashList.Count; i++)
                        {
                            sbCompareResults.Append(String.Format("{0} == {1}{2}", this.SelectedAlgorithms[i], hashList[i], "\r"));
                        }
                    }
                    else
                    {
                        for (int i = 0; i < hashList.Count; i++)
                        {
                            if (ChecksumVerifierLogic.CompareHashes(this.SF_TxtUserHash, hashList[i]))
                            {
                                sbCompareResults.Append(String.Format("Valid! Matching {0} checksum: {1}{2}", this.SelectedAlgorithms[i], hashList[i], "\r"));
                            }
                            else
                            {
                                sbCompareResults.Append(String.Format("Invalid! Mismatched {0} checksum: {1}{2}", this.SelectedAlgorithms[i], hashList[i], "\r"));
                            }
                        }
                    }
                    this.SF_Progress = 100;
                    this.SF_LblResult = "Finished!";
                    this.SF_TbFileHash = sbCompareResults.ToString();
                }
                catch(Exception ex)
                {
                    this._SF_progress = 0;
                    this.SF_LblResult = "Error! " + ex.Message;
                    this.SF_LblFileSize = String.Empty;
                }
            }//if
            else
            {
                this._SF_lblResult = "Error!  No file to calculate checksum!";
            }//else
        }//SF_Compare
        #endregion
        #endregion
    }
}
