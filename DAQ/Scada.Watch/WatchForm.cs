﻿
using Microsoft.Win32;
using Scada.Watch.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Scada.Watch
{
	public partial class WatchForm : Form
	{
        private System.Windows.Forms.Timer checkTimer = null;

        private int delayCounter = 0;

        private long timesCounter = 0;

        private const int WatchInterval = 60000;

        private FileSystemWatcher fsw = null;

        private StreamWriter sw = null;

		public WatchForm()
		{
			InitializeComponent();
        }

        private static string Format(DateTime time)
        {
            return time.ToString("yy/MM/dd HH:mm");
        }

        private void InitSysNotifyIcon()
        {
            // Notify Icon
            watchNotifyIcon.Text = "系统监控程序";
            watchNotifyIcon.Icon = new Icon(Resources.Monitor, new Size(16, 16));
            watchNotifyIcon.Visible = true;

            watchNotifyIcon.Click += new EventHandler(OnSysNotifyIconContextMenu);
        }

        private void OnSysNotifyIconContextMenu(object sender, EventArgs e)
        {
            this.Visible = true;
            this.ShowInTaskbar = true;

            this.WindowState = FormWindowState.Normal;
            this.BringToFront();
        }

		private void WatchForm_Load(object sender, EventArgs e)
		{
            /// MessageBox.Show("Break for Attach");
            this.InitSysNotifyIcon();
            this.startStripStatusLabel.Text = string.Format("启动[{0}]", Format(DateTime.Now));
            this.ShowInTaskbar = false;
            this.Visible = false;
            // Watch timer
            this.checkTimer = new System.Windows.Forms.Timer();
            this.checkTimer.Interval = WatchInterval;    // Defines.KeepAliveInterval;
            this.checkTimer.Tick += Per60secTimerTick;
            this.checkTimer.Start();

            // Initial Path
            this.textPath.Text = @"C:\Scada\Install";
            this.SetAutoUpdateDir();
            this.textPath.Enabled = false;
            this.buttonWatch.Enabled = false;

            this.sw = new StreamWriter("watch.txt", true);
            this.FilePutContents("Open");
            SystemEvents.SessionEnding += SystemEvents_SessionEnding;
		}

        private bool Shutdown = false;

        void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
        {
            this.Shutdown = true;

            this.KillScadaProcess(new string[]
                {
                    "MDS.exe",
                    "AIS.exe",
                    "Scada.*",
                });
        }

        private void SetAutoUpdateSearchDir(string dir)
        {
            if (this.fsw != null)
            {
                this.fsw.Dispose();
            }

            if (Directory.Exists(dir))
            {
                this.fsw = new FileSystemWatcher(dir, "*.zip");
                this.fsw.Created += (object sender, FileSystemEventArgs e) =>
                {
                    string fileName = e.Name.ToLower();
                    if (fileName.StartsWith("bin"))
                    {
                        this.UpdateBin(e.FullPath);
                    }
                };
                this.fsw.EnableRaisingEvents = true;
            }
        }

        private void UpdateBin(string filePath)
        {
            this.KillScadaProcess(new string[]
                {
                    "MDS.exe",
                    "AIS.exe",
                    "Scada.*",
                });
            this.delayCounter = 0;
            this.updateStripStatusLabel.Text = string.Format("更新[{0}]", Format(DateTime.Now));
            Thread.Sleep(1000);
            this.OpenProcessByName("Scada.Update", string.Format("\"{0}\"", filePath));
        }

        private void KillScadaProcess(string[] procNames)
        {
            using (Process p = new Process())
            {
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.CreateNoWindow = true;
                p.Start();

                foreach (var procName in procNames)
                {
                    string cmd = string.Format("taskkill /f /im {0}", procName);
                    p.StandardInput.WriteLine(cmd);
                }
                p.StandardInput.WriteLine("exit");
            }
        }

        private void Per60secTimerTick(object sender, EventArgs e)
		{
            if (this.delayCounter >= 2)
            {
                this.WatchProcess("Scada.Main", "/R");

                string scadaDataClient = this.versionCheck.Checked ? "Scada.Data.Client" : "Scada.DataCenterAgent";
                this.WatchProcess(scadaDataClient, "--start");

                // 自动重启数采和数据上传
                if (this.CheckRebootDate(DateTime.Now))
                {
                    FilePutContents("Reboot");
                    this.Reboot(DateTime.Now);
                }
            }

            if (delayCounter > 1000000)
            {
                delayCounter = 0;
            }

            this.delayCounter++;
        }

        private void Reboot(DateTime dateTime)
        {
            string scadaDataClient = this.versionCheck.Checked ? "Scada.Data.Client" : "Scada.DataCenterAgent";

            this.KillScadaProcess(new string[]
                {
                    "Scada.Main.exe", scadaDataClient + ".exe"
                });

            this.WatchProcess("Scada.Main", "/R");
            this.WatchProcess(scadaDataClient, "--start");
            this.lastCheckRebootTime = dateTime;
        }

        private bool CheckRebootDate(DateTime dateTime)
        {
            if (this.lastCheckRebootTime.Month != dateTime.Month)
            {
                if (dateTime.Day == 1)
                {
                    if (dateTime.Hour == 23)
                    {
                        if (dateTime.Minute == 59)
                        {
                            return true;
                        }
                    }
                }
            }            
            return false;
        }

        private void WatchProcess(string procName, string startArgs)
        {
            Process[] ps = Process.GetProcessesByName(procName);
            if (ps == null || ps.Length == 0)
            {
                this.OpenProcessByName(procName, startArgs);
            }
        }

        private static string GetExeFilePath(string fileName)
        {
            string location = Assembly.GetExecutingAssembly().Location;
            string path = Path.GetDirectoryName(location);
            return Path.Combine(path, fileName);
        }

        private void OpenProcessByName(string name, string arg)
        {
            string fileName = GetExeFilePath(name + ".exe");
            try
            {
                ProcessStartInfo processInfo = new ProcessStartInfo();
                processInfo.FileName = fileName;
                if (!string.IsNullOrEmpty(arg))
                {
                    processInfo.Arguments = arg;
                }
                Process.Start(processInfo);
                this.FilePutContents(fileName);
            }
            catch (Exception)
            {
            }
        }

        private void SetAutoUpdateDir()
        {
            string dir = this.textPath.Text;
            this.SetAutoUpdateSearchDir(dir);
        }

        private void buttonWatch_Click(object sender, EventArgs e)
        {
            this.SetAutoUpdateDir();
            this.textPath.Enabled = false;
            this.buttonWatch.Enabled = false;
        }

        private void buttonPathClick(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.SelectedPath = @"C:\Scada\Install";
            DialogResult dr = fbd.ShowDialog();
            if (dr == DialogResult.OK)
            {
                this.textPath.Enabled = true;
                this.buttonWatch.Enabled = true;
                this.textPath.Text = fbd.SelectedPath;
            }
        }

        private void watchNotifyIconDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void WatchForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.Shutdown)
            {
                return;
            }

            DialogResult dr = MessageBox.Show("您确定要退出[系统监控程序]吗？", "Scada.Watch", MessageBoxButtons.OKCancel);
            if (dr != DialogResult.OK)
            {
                e.Cancel = true;
            }
            this.FilePutContents("Close by user");
            this.sw.Close();
        }

        private void WatchForm_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Visible = false;
            }
        }

        private void FilePutContents(string content)
        {
            string line = string.Format("[{0:yyyy-MM-dd HH:mm:ss}] {1}", DateTime.Now, content);
            this.sw.WriteLine(line);
            this.sw.Flush();
        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {

        }

        public DateTime lastCheckRebootTime { get; set; }
    }
}
