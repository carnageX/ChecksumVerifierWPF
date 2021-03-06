﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ChecksumVerifier
{
    class MainViewModel : ViewModelBase
    {
        #region Class Properties
        public List<string> Algorithms { get; set; }
        public ObservableCollection<string> SelectedAlgorithms { get; set; }
        public List<EncodingType> EncodingTypes { get; set; }
        public EncodingType SelectedEncodingType { get; set; }
        private Stopwatch swElapsedTime;
        #endregion

        #region Main Methods
        public MainViewModel()
        {
            this.Algorithms = new List<string>() { HashOption.MD5.Description(), HashOption.MD5CNG.Description(), HashOption.SHA1.Description(),
                HashOption.SHA1Managed.Description(), HashOption.SHA1CNG.Description(), HashOption.SHA256.Description(), HashOption.SHA256Managed.Description(), 
                HashOption.SHA256CNG.Description(), HashOption.SHA384.Description(), HashOption.SHA384Managed.Description(), HashOption.SHA384CNG.Description(), 
                HashOption.SHA512.Description(), HashOption.SHA512Managed.Description(), HashOption.SHA512CNG.Description(), HashOption.RIPEMD160.Description(),
                HashOption.RIPEMD160Managed.Description(), HashOption.CRC16.Description(), HashOption.CRC32.Description() };
            this.SelectedAlgorithms = new ObservableCollection<string>();
            this.SelectedAlgorithms.Add(this.Algorithms[0]);
            this.EncodingTypes = new List<EncodingType>() { 
                new EncodingType(EncType.Default, Encoding.Default), new EncodingType(EncType.ASCII, Encoding.ASCII),
                new EncodingType(EncType.BigEndianUnicode, Encoding.BigEndianUnicode), new EncodingType(EncType.Unicode, Encoding.Unicode), 
                new EncodingType(EncType.UTF7, Encoding.UTF7), new EncodingType(EncType.UTF8, Encoding.UTF8), 
                new EncodingType(EncType.UTF32, Encoding.UTF32)
            };
            this.SelectedEncodingType = new EncodingType(EncType.Default, Encoding.Default);
            this.LblResult = "Ready...";
            this.StatusImage = imageReady;
            this.swElapsedTime = new Stopwatch();
        }//MainViewModel
        #endregion

        #region Statusbar
        public int ProcessingProgress
        {
            get { return this._processingProgress; }
            set
            {
                if(this._processingProgress != value)
                {
                    this._processingProgress = value;
                    RaisePropertyChanged("ProcessingProgress");
                }
            }
        }//ProcessingProgress
        private int _processingProgress;

        public string LblResult
        {
            get { return this._lblResult; }
            set
            {
                if(this._lblResult != value)
                {
                    this._lblResult = value;
                    RaisePropertyChanged("LblResult");
                }
            }
        }//LblResult
        private string _lblResult;

        private BitmapImage imageReady = new BitmapImage(new Uri("/ChecksumVerifier;component/Resources/status_grey.png", UriKind.Relative));
        private BitmapImage imageFinished = new BitmapImage(new Uri("/ChecksumVerifier;component/Resources/status_green.png", UriKind.Relative));
        private BitmapImage imageError = new BitmapImage(new Uri("/ChecksumVerifier;component/Resources/status_red.png", UriKind.Relative));
        private BitmapImage imageRunning = new BitmapImage(new Uri("/ChecksumVerifier;component/Resources/status_yellow.png", UriKind.Relative));

        public BitmapImage StatusImage
        {
            get { return this._statusImage; }
            set 
            { 
                if(this._statusImage != value)
                {
                    _statusImage = value;
                    RaisePropertyChanged("StatusImage");
                }
            }
        }
        private BitmapImage _statusImage;

        public string ElapsedTime
        {
            get { return this._lblElapsedTime; }
            set
            {
                if(this._lblElapsedTime != value)
                {
                    this._lblElapsedTime = value;
                    RaisePropertyChanged("ElapsedTime");
                }
            }
        }//ElapsedTime
        private string _lblElapsedTime;
        #endregion

        #region Single File
        #region Single File Properties
        private BackgroundWorker SF_BwWorker;
        private List<string> SF_CalculatedHashList = new List<string>();
        private int SF_BWProgress { get; set; }

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
        private string _SF_txtUserHash;

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
        private string _SF_txtFilePath;

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
        private string _SF_lblFileSize;

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
        private string _SF_btnCompareText;

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
        private string _SF_tbFileHash;
        #endregion

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
                this.SF_LblFileSize = String.Format("(File Size: {0} KB)", (fileInfo.Length / 1024 + 1).ToString("N0"));
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
                    if(SF_BwWorker == null || !SF_BwWorker.IsBusy)
                    {
                        using(SF_BwWorker = new BackgroundWorker())
                        {
                            this.swElapsedTime = Stopwatch.StartNew();
                            this.ElapsedTime = String.Empty;
                            this.LblResult = "Running...";
                            this.StatusImage = imageRunning;
                            this.SF_CalculatedHashList.Clear();
                            //this.SF_TbFileHash = String.Format("Processing {0} for selected algorithms... Please wait...", this.SF_TxtFilePath);
                            this.ProcessingProgress = 0;
                            SF_BWProgress = 0;
                            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                        
                            this.SF_BwWorker.DoWork += new DoWorkEventHandler(SF_BwWorker_DoWork);
                            this.SF_BwWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(SF_BwWorker_RunWorkerCompleted);
                            this.SF_BwWorker.ProgressChanged += new ProgressChangedEventHandler(SF_BwWorker_ProgressChanged);
                            SF_BwWorker.WorkerReportsProgress = true;
                            SF_BwWorker.WorkerSupportsCancellation = true;

                            SF_BwWorker.RunWorkerAsync();
                        }
                    }
                }
                catch(Exception ex)
                {
                    this.ProcessingProgress = 0;
                    MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    this.LblResult = "Error!";
                    this.StatusImage = imageError;
                    this.SF_LblFileSize = String.Empty;
                    Mouse.OverrideCursor = null;
                }
            }//if
            else
            {
                MessageBox.Show("Error!  No file to calculate checksum!", "Error!", MessageBoxButton.OK, MessageBoxImage.Warning);
                this.LblResult = "Error!";
                this.StatusImage = imageError;
            }//else
        }

        void SF_BwWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.ProcessingProgress = e.ProgressPercentage;
        }

        void SF_BwWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                StringBuilder sbCompareResults = new StringBuilder();

                if (String.IsNullOrWhiteSpace(this.SF_TxtUserHash))
                {
                    for (int i = 0; i < this.SelectedAlgorithms.Count; i++)
                    {
                        sbCompareResults.Append(String.Format("{0} == {1}{2}", this.SelectedAlgorithms[i], this.SF_CalculatedHashList[i], "\r"));
                    }
                }
                else
                {
                    for (int i = 0; i < this.SelectedAlgorithms.Count; i++)
                    {
                        if (ChecksumVerifierLogic.CompareHashes(this.SF_TxtUserHash, this.SF_CalculatedHashList[i]))
                        {
                            sbCompareResults.Append(String.Format("Valid! Matching {0} checksum: {1}{2}", this.SelectedAlgorithms[i], this.SF_CalculatedHashList[i], "\r"));
                        }
                        else
                        {
                            sbCompareResults.Append(String.Format("Invalid! Mismatched {0} checksum: {1}{2}", this.SelectedAlgorithms[i], this.SF_CalculatedHashList[i], "\r"));
                        }
                    }
                }
                this.swElapsedTime.Stop();
                this.LblResult = "Finished!";
                this.StatusImage = imageFinished;
                this.SF_TbFileHash = sbCompareResults.ToString();
                this.ProcessingProgress = 100;
                this.ElapsedTime = String.Format("Elapsed time: {0}", swElapsedTime.Elapsed);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                this.LblResult = "Error!";
                this.StatusImage = imageError;
                this.ProcessingProgress = 0;
            }
            Mouse.OverrideCursor = null;
        }

        private void SF_BwWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            SF_DoComparison(this.SF_TxtFilePath);
        }//SF_Compare

        private void SF_DoComparison(string filepath)
        {
            foreach(string a in this.SelectedAlgorithms)
            {
                this.SF_CalculatedHashList.Add(ChecksumVerifierLogic.GetHash(filepath, a.GetHashOptionValue()));
                SF_BWProgress += (int)(((float)1 / (float)this.SelectedAlgorithms.Count) * 100);
                this.SF_BwWorker.ReportProgress(SF_BWProgress);
            }
        }//SF_DoComparison
        #endregion
        #endregion

        #region Multiple Files
        #region Multiple Files Properties
        private BackgroundWorker MF_BwWorker;
        private List<string> MF_CalculatedHashList = new List<string>();
        private int MF_BWProgress { get; set; }

        public string MF_TxtUserHash
        {
            get { return _MF_txtUserHash; }
            set
            {
                if(this._MF_txtUserHash != value)
                {
                    this._MF_txtUserHash = value;
                    this.MF_BtnCompareText = "Compare";
                    RaisePropertyChanged("MF_TxtUserHash");
                }
            }
        }//MF_TxtUserHash
        private string _MF_txtUserHash;

        public string MF_BtnCompareText
        {
            get
            {
                if (String.IsNullOrEmpty(this._MF_btnCompareText))
                    this.MF_BtnCompareText = "Generate";
                return this._MF_btnCompareText;
            }
            set
            {
                if (this._MF_btnCompareText != value)
                {
                    this._MF_btnCompareText = value;
                    RaisePropertyChanged("MF_BtnCompareText");
                }
            }
        }
        private string _MF_btnCompareText;

        public List<string> MF_FileList
        {
            get { return _MF_fileList; }
            set 
            { 
                if(this._MF_fileList != value)
                {
                    this._MF_fileList = value;
                    RaisePropertyChanged("MF_FileList");
                }
            }
        }
        private List<string> _MF_fileList;

        public List<string> MF_ResultList
        {
            get { return _MF_resultList; }
            set
            {
                if (this._MF_resultList != value)
                {
                    this._MF_resultList = value;
                    RaisePropertyChanged("MF_ResultList");
                }
            }
        }
        private List<string> _MF_resultList;

        public int MF_Progress_Max
        {
            get { return _MF_progress_max; }
            set
            {
                if (_MF_progress_max != value)
                {
                    _MF_progress_max = value;
                    RaisePropertyChanged("MF_Progress_Max");
                }
            }
        }//MF_Progress
        private int _MF_progress_max;

        public int MF_SelectedResultIndex
        {
            get { return _MF_selectedResultIndex; }
            set
            {
                if (_MF_selectedResultIndex != value)
                {
                    _MF_selectedResultIndex = value;
                    RaisePropertyChanged("MF_SelectedResultIndex");
                }
            }
        }//MF_SelectedResultIndex
        private int _MF_selectedResultIndex;
        #endregion

        #region MF Browse File
        public ICommand MF_CmdBrowse
        {
            get
            {
                if (this._MF_cmdBrowse == null)
                    this._MF_cmdBrowse = new RelayCommand(MF_BrowseFile, MF_CanBrowseFile);
                return this._MF_cmdBrowse;
            }
        }
        private RelayCommand _MF_cmdBrowse;

        private bool MF_CanBrowseFile()
        {
            return true;
        }

        private void MF_BrowseFile()
        {
            OpenFileDialog MF_openDialog = new OpenFileDialog();
            MF_openDialog.Multiselect = true;
            if(MF_openDialog.ShowDialog() == true)
            {
                this.MF_FileList = MF_openDialog.FileNames.ToList();
            }
        }
        #endregion

        #region MF Export File
        public ICommand MF_CmdExport
        {
            get
            {
                if(this._MF_cmdExport == null)
                    this._MF_cmdExport = new RelayCommand(MF_Export, MF_CanExport);
                return this._MF_cmdExport;
            }
        }
        private RelayCommand _MF_cmdExport;

        private bool MF_CanExport()
        {
            return true;
        }

        private void MF_Export()
        {
            SaveFileDialog MF_exportFile = new SaveFileDialog();
            MF_exportFile.Filter = "Text File | *.txt";
            MF_exportFile.Title = "Save generated checksums...";
            MF_exportFile.FileName = "ChecksumList.txt";
            if(MF_exportFile.ShowDialog() == true)
            {
                using (StreamWriter swExport = new StreamWriter(MF_exportFile.OpenFile()))
                {
                    for (int i = 0; i < this.MF_ResultList.Count; i++)
                    {
                        swExport.WriteLine(this.MF_ResultList[i].ToString());
                    }//for
                }//using
            }
        }//MF_Export
        #endregion

        #region MF Compare Hashes
        public ICommand MF_CmdCompare
        {
            get
            {
                if (this._MF_cmdCompare == null)
                    this._MF_cmdCompare = new RelayCommand(MF_Compare, MF_CanCompare);
                return this._MF_cmdCompare;
            }
        }
        private RelayCommand _MF_cmdCompare;

        private bool MF_CanCompare()
        {
            return true;
        }

        private void MF_Compare()
        {
            //this.MF_Progress_Max = this.MF_FileList.Count;
            this.MF_ResultList = new List<string>();

            if (this.MF_FileList != null && this.MF_FileList.Count > 0)
            {
                try
                {
                    if (MF_BwWorker == null || !MF_BwWorker.IsBusy)
                    {
                        using (MF_BwWorker = new BackgroundWorker())
                        {
                            this.swElapsedTime = Stopwatch.StartNew();
                            this.ElapsedTime = String.Empty;
                            this.LblResult = "Running...";
                            this.StatusImage = imageRunning;
                            this.ProcessingProgress = 0;
                            MF_BWProgress = 0;
                            MF_CalculatedHashList.Clear();

                            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;
                            this.MF_BwWorker.DoWork += new DoWorkEventHandler(MF_BwWorker_DoWork);
                            this.MF_BwWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(MF_BwWorker_RunWorkerCompleted);
                            this.MF_BwWorker.ProgressChanged += new ProgressChangedEventHandler(MF_BwWorker_ProgressChanged);
                            MF_BwWorker.WorkerReportsProgress = true;
                            MF_BwWorker.WorkerSupportsCancellation = true;

                            MF_BwWorker.RunWorkerAsync();
                        }//using
                    }//if
                }//try
                catch (Exception ex)
                {
                    this.ProcessingProgress = 0;
                    Mouse.OverrideCursor = null;
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.LblResult = "Error!";
                    this.StatusImage = imageError;
                }//catch
            }//if
            else
            {
                this.MF_ResultList.Add("Error!  No file(s) to calculate checksum!");
                this.LblResult = "Error!";
                this.StatusImage = imageError;
            }//else
        }//MF_COmpare

        private void MF_BwWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.ProcessingProgress = e.ProgressPercentage;
        }

        private void MF_BwWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.swElapsedTime.Stop();
            this.ProcessingProgress = 100;
            Mouse.OverrideCursor = null;
            this.MF_ResultList = MF_CalculatedHashList;
            this.ElapsedTime = String.Format("Elapsed time: {0}", swElapsedTime.Elapsed);
            this.LblResult = "Finished!";
            this.StatusImage = imageFinished;
        }

        private void MF_BwWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (string f in this.MF_FileList)
            {
                foreach (string a in this.SelectedAlgorithms)
                {
                    string hash = ChecksumVerifierLogic.GetHash(f, a.GetHashOptionValue());
                    //this.MF_CalculatedHashList.Add(hash);

                    if (!String.IsNullOrWhiteSpace(this.MF_TxtUserHash))
                    {
                        if (ChecksumVerifierLogic.CompareHashes(this.MF_TxtUserHash, hash))
                        {
                            this.MF_CalculatedHashList.Add(String.Format("{0}: Checkum match! - {1} - {2}", f, a, hash));
                        }//if
                        else
                        {
                            this.MF_CalculatedHashList.Add(String.Format("{0}: Checkum mismatch! - {1} - {2}", f, a, hash));
                        }//else
                    }//if
                    else
                    {
                        this.MF_CalculatedHashList.Add(String.Format("{0} - {1} - {2}", f, a, hash));
                    }//else

                    MF_BWProgress += (int)(((float)1 / (float)this.SelectedAlgorithms.Count) * ((float)1 / (float)this.MF_FileList.Count) * 100);
                    this.MF_BwWorker.ReportProgress(MF_BWProgress);
                }//algorithms 
            }//files
        }
        #endregion

        #region MF Copy Selected
        public ICommand MF_CmdCopySelected
        {
            get
            {
                if(this._MF_cmdCopySelected == null)
                    this._MF_cmdCopySelected = new RelayCommand(MF_CopySelected, MF_CanCopySelected);
                return this._MF_cmdCopySelected;
            }
        }
        private RelayCommand _MF_cmdCopySelected;

        private bool MF_CanCopySelected()
        {
            return true;
        }

        private void MF_CopySelected()
        {
            Clipboard.SetText(this.MF_ResultList[this.MF_SelectedResultIndex]);
        }
        #endregion

        #region MF Copy All
        public ICommand MF_CmdCopyAll
        {
            get
            {
                if (this._MF_cmdCopyAll == null)
                    this._MF_cmdCopyAll = new RelayCommand(MF_CopyAll, MF_CanCopyAll);
                return this._MF_cmdCopyAll;
            }
        }
        private RelayCommand _MF_cmdCopyAll;

        private bool MF_CanCopyAll()
        {
            return true;
        }

        private void MF_CopyAll()
        {
            Clipboard.SetText(String.Join("\n", this.MF_ResultList));
        }
        #endregion
        #endregion

        #region Text - Single 
        #region Text - Single Properties
        private BackgroundWorker TS_BwWorker;
        private List<string> TS_CalculatedHashList = new List<string>();
        private int TS_BWProgress { get; set; }

        public string TS_TxtUserText
        {
            get { return this._TS_txtUserText; }
            set
            {
                if(this._TS_txtUserText != value)
                {
                    this._TS_txtUserText = value;
                    RaisePropertyChanged("TS_TxtUserText");
                }
            }
        }
        private string _TS_txtUserText;

        public string TS_TxtUserHash
        {
            get { return this._TS_txtUserHash; }
            set
            {
                if (this._TS_txtUserHash != value)
                {
                    this._TS_txtUserHash = value;
                    RaisePropertyChanged("TS_TxtUserHash");
                }
            }
        }
        private string _TS_txtUserHash;

        public string TS_BtnCompareText
        {
            get
            {
                if (String.IsNullOrEmpty(this._TS_btnCompareText))
                    this.TS_BtnCompareText = "Generate";
                return this._TS_btnCompareText;
            }
            set
            {
                if (this._TS_btnCompareText != value)
                {
                    this._TS_btnCompareText = value;
                    RaisePropertyChanged("TS_BtnCompareText");
                }
            }
        }//BtnCompareText
        private string _TS_btnCompareText;

        public string TS_TbResultHash
        {
            get { return this._TS_tbResultHash; }
            set
            {
                if (this._TS_tbResultHash != value)
                {
                    this._TS_tbResultHash = value;
                    RaisePropertyChanged("TS_TbResultHash");
                }
            }
        }
        private string _TS_tbResultHash;

        public int TS_Progress
        {
            get { return _TS_progress; }
            set
            {
                if (_TS_progress != value)
                {
                    _TS_progress = value;
                    RaisePropertyChanged("TS_Progress");
                }
            }
        }//TS_Progress
        private int _TS_progress;
        #endregion

        #region Text - Single Compare
        public ICommand TS_CmdCompare
        {
            get
            {
                if (this._TS_cmdCompare == null)
                    this._TS_cmdCompare = new RelayCommand(TS_Compare, TS_CanCompare);
                return this._TS_cmdCompare;
            }
        }
        private RelayCommand _TS_cmdCompare;

        private bool TS_CanCompare()
        {
            return true;
        }

        private void TS_Compare()
        {
            if (!String.IsNullOrEmpty(this.TS_TxtUserText))
            {
                try
                {
                    if (TS_BwWorker == null || !TS_BwWorker.IsBusy)
                    {
                        using (TS_BwWorker = new BackgroundWorker())
                        {
                            this.swElapsedTime = Stopwatch.StartNew();
                            this.ElapsedTime = String.Empty;
                            this.LblResult = "Running...";
                            this.StatusImage = imageRunning;
                            this.ProcessingProgress = 0;
                            TS_BWProgress = 0;
                            this.TS_CalculatedHashList.Clear();
                            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

                            this.TS_BwWorker.DoWork += new DoWorkEventHandler(TS_BwWorker_DoWork);
                            this.TS_BwWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(TS_BwWorker_RunWorkerCompleted);
                            this.TS_BwWorker.ProgressChanged += new ProgressChangedEventHandler(TS_BwWorker_ProgressChanged);
                            TS_BwWorker.WorkerReportsProgress = true;
                            TS_BwWorker.WorkerSupportsCancellation = true;

                            TS_BwWorker.RunWorkerAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.ProcessingProgress = 0;
                    this.TS_TbResultHash = "Error! " + ex.Message;
                    this.LblResult = "Error!";
                    this.StatusImage = imageError;
                    Mouse.OverrideCursor = null;
                }
            }
            else
            {
                this.TS_TbResultHash = "No input string given!";
                this.LblResult = "Error!";
                this.StatusImage = imageError;
            }
        }

        void TS_BwWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.ProcessingProgress = e.ProgressPercentage;
        }

        void TS_BwWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                StringBuilder sbResults = new StringBuilder();

                if (String.IsNullOrWhiteSpace(this.TS_TxtUserHash))
                {
                    for (int i = 0; i < this.SelectedAlgorithms.Count; i++)
                    {
                        sbResults.Append(String.Format("{0} == {1}{2}", this.SelectedAlgorithms[i], this.TS_CalculatedHashList[i], "\r"));
                    }
                }
                else
                {
                    for (int i = 0; i < this.SelectedAlgorithms.Count; i++)
                    {
                        if (ChecksumVerifierLogic.CompareHashes(this.TS_TxtUserHash, this.TS_CalculatedHashList[i]))
                        {
                            sbResults.Append(String.Format("Valid! Matching {0} checksum: {1}{2}", this.SelectedAlgorithms[i], this.TS_CalculatedHashList[i], "\r"));
                        }
                        else
                        {
                            sbResults.Append(String.Format("Invalid! Mismatched {0} checksum: {1}{2}", this.SelectedAlgorithms[i], this.TS_CalculatedHashList[i], "\r"));
                        }
                    }
                }
                this.swElapsedTime.Stop();
                this.TS_TbResultHash = sbResults.ToString();
                this.ProcessingProgress = 100;
                this.LblResult = "Finished!";
                this.StatusImage = imageFinished;
                this.ElapsedTime = String.Format("Elapsed time: {0}", swElapsedTime.Elapsed);
            }
            catch(Exception ex)
            {
                this.ProcessingProgress = 0;
                this.TS_TbResultHash = "Error! " + ex.Message;
                this.LblResult = "Error!";
                this.StatusImage = imageError;
            }
            Mouse.OverrideCursor = null;
        }

        void TS_BwWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (string a in this.SelectedAlgorithms)
            {
                this.TS_CalculatedHashList.Add(ChecksumVerifierLogic.GetHash(this.TS_TxtUserText, a.GetHashOptionValue(), this.SelectedEncodingType.Type));
                TS_BWProgress += (int)(((float)1 / (float)this.SelectedAlgorithms.Count) * 100);
                this.TS_BwWorker.ReportProgress(TS_BWProgress);
            }
        }
        #endregion
        #endregion
    }
}
