using MySql.Data.MySqlClient;
using Scada.Common;
using Scada.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Scada.MainVision
{
    /// <summary>
    /// Interaction logic for SamplerControlPanel.xaml
    /// </summary>
    public partial class SamplerControlPanel : UserControl
    {

        private DispatcherTimer dispatcherTimer;

        // 采样器连接状态
        private int connecting_status = 0;
        private int running_status = 0;

        // 采样类型, 0:定时采样，1：定量采样
        private int sampling_type = 0;

        private int factor = 1;

        public MySql.Data.MySqlClient.MySqlConnection dbConn { get; set; }

        private DBDataProvider dataProvider;

        public void SetDataProvider(DBDataProvider dataProvider)
        {
            this.dataProvider = dataProvider;
        }

        public SamplerControlPanel(string deviceKey, DBDataProvider dataProvider)
        {
            InitializeComponent();
            this.DeviceKey = deviceKey;
            this.SetDataProvider(dataProvider);

            RadioSamplebyTime.IsChecked = true;
            sampling_type = 0;

            // 初始化
            this.ConnectButton.IsEnabled = true;
            this.DisconnectButton.IsEnabled = false;
            this.StartButton.IsEnabled = false;
            this.StopButton.IsEnabled = false;
            this.ResetButton.IsEnabled = false;

            // this.dataPane.Initialize(new string[] { "最近采样时间", "瞬时采样流量", "累计采样流量", "累积采样时间", "滤纸报警", "流量报警", "主电源报警" });

            this.dbConn = this.dataProvider.GetMySqlConnection();

            if (this.dbConn != null)
            {
                MySqlCommand cmd = this.dbConn.CreateCommand();
                var dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
                dispatcherTimer.Tick += (s, evt) =>
                {
                    this.RefreshTick(cmd);
                };
                dispatcherTimer.Interval = new TimeSpan(0, 0, 5);
                dispatcherTimer.Start();
                this.RefreshTick(cmd);
            }
            else
            { return; }
        }

        private void RefreshTick(MySqlCommand cmd)
        {
            // 别的地方调用过此方法了
            // this.dataProvider.RefreshTimeNow(cmd);
            UpdateDataPanel();
        }

        private void UpdateDataPanel()
        {
            var d = this.dataProvider.GetLatestEntry(this.DeviceKey == "scada.mds" ? DataProvider.DeviceKey_MDS : DataProvider.DeviceKey_AIS);
            // panel.Check(Get(d, "time", ""));
            // NOTICE：数据库中没有任何记录时，d的对象仍然可以创建成功，所以需要加入d.Count==0
            if (d == null || d.Count == 0)
            {
                return;
            }

            //"瞬时采样流量", "累计采样流量", "累积采样时间"
            // panel.SetData(

            this.DataStatusLabel.Content = GetStatus(d, "status", "");
            this.DataTimeLabel.Content = Get(d, "time", "");
            this.DataFlowLabel.Content = Get(d, "flow", "m³/h");
            this.DataFlowAllLabel.Content = Get(d, "volume", "m³");
            this.DataTimeAllLabel.Content = Get(d, "hours", "h");
            this.DataAlarm1Label.Content = GetAlarm(d, "alarm1", "");
            this.DataAlarm2Label.Content = GetAlarm(d, "alarm2", "");
            this.DataAlarm3Label.Content = GetAlarm(d, "alarm3", "");

            MarkAlarm(d, "alarm1", this.DataAlarm1Label, 4);
            MarkAlarm(d, "alarm2", this.DataAlarm2Label, 5);
            MarkAlarm(d, "alarm3", this.DataAlarm3Label, 6);
        }

        private bool CheckDeviceFile(string strFlag)
        {
            string statusPath = ConfigPath.GetConfigFilePath("status");
            if (!Directory.Exists(statusPath))
            {
                Directory.CreateDirectory(statusPath);
            }

            string relFileName = string.Format("status\\@{0}-{1}", this.DeviceKey, strFlag);
            string fileName = ConfigPath.GetConfigFilePath(relFileName);

            return File.Exists(fileName);
        }

        private string GetRemoteCommand(string cmd)
        {
            return string.Format("{0}:{1}", this.DeviceKey.ToLower(), cmd);
        }

        private void OnConnectButton(object sender, RoutedEventArgs e)
        {
            if (connecting_status != 0)
            {
                MessageBox.Show("连接错误");
                return;
            }

            string strFlowSetting;
            string strTimeSetting;

            // 定量采样
            if (sampling_type == 1)
            {
                float volume;
                float flow;
                float time;
                try
                {
                    volume = float.Parse(TimeSettingText.Text);
                    flow = factor * float.Parse(FlowSettingText.Text);
                    if (DeviceKey.Equals("scada.ais"))
                    { time = 1000 * volume / flow; }
                    else
                    { time = volume / flow; }
                    
                }
                catch (Exception)
                { return; }

                strFlowSetting = flow.ToString();
                strTimeSetting = time.ToString("0.0");
            }
            // 定时采样
            else
            {
                float flow;
                try
                {
                    flow = factor * float.Parse(FlowSettingText.Text);
                }
                catch (Exception)
                { return; }

                strFlowSetting = flow.ToString();
                strTimeSetting = TimeSettingText.Text;
            }
            
            string strCmd = string.Format("connect,{0},{1}", strFlowSetting, strTimeSetting); 

            // 开始连接
            Command.Send(Ports.MainSiemens, GetRemoteCommand(strCmd));

            this.ConnectButton.IsEnabled = false;
            this.StatusLabel.Content = "尝试连接,请等待...";

            if (dispatcherTimer == null)
            {
                dispatcherTimer = new DispatcherTimer();
                dispatcherTimer.Tick += dispatcherTimerTick;
                dispatcherTimer.Interval = new TimeSpan(0, 0, 5);
                dispatcherTimer.Start();
            }
        }

        void dispatcherTimerTick(object sender, EventArgs e)
        {
            if (this.CheckDeviceFile("connecting"))
            {
                connecting_status = 1;
                this.ConnectButton.IsEnabled = false;
                this.DisconnectButton.IsEnabled = true;

                if (this.CheckDeviceFile("running"))
                {
                    running_status = 1;
                    this.StartButton.IsEnabled = false;
                    this.StopButton.IsEnabled = true;
                    this.StatusLabel.Content = "采样中";
                }
                else
                {
                    running_status = 0;
                    this.StartButton.IsEnabled = true;
                    this.StopButton.IsEnabled = false;
                    this.StatusLabel.Content = "准备";
                }
            }
            else
            {
                connecting_status = 0;
                this.ConnectButton.IsEnabled = true;
                this.DisconnectButton.IsEnabled = false;
                this.StartButton.IsEnabled = false;
                this.StopButton.IsEnabled = false;
                this.StatusLabel.Content = "未连接";
            }
        }

        private void OnDisconnectButton(object sender, RoutedEventArgs e)
        {
            if (connecting_status != 1)
            {
                MessageBox.Show("断开连接错误，请稍后再试");
                return;
            }

            Command.Send(Ports.MainSiemens, GetRemoteCommand("disconnect"));

            DisconnectButton.IsEnabled = false;
            ConnectButton.IsEnabled = false;
            StartButton.IsEnabled = false;
            StopButton.IsEnabled = false;
        }

        private void OnStartButton(object sender, RoutedEventArgs e)
        {
            if (connecting_status != 1 || running_status != 0)
            {
                MessageBox.Show("启动错误，请稍后再试");
                return;
            }

            string sid = "";

            if (this.SidText.Text == "")
            {
                sid = string.Format("SID-{0}", DateTime.Now.ToString("yyyyMMdd-HHmmss"));
            }
            else
            {
                sid = this.SidText.Text;
            }

            string cmd = string.Format("start:Sid={0}", sid);
            Command.Send(Ports.MainSiemens, GetRemoteCommand(cmd));
        }

        private void OnStopButton(object sender, RoutedEventArgs e)
        {
            if (connecting_status != 1 || running_status != 1)
            {
                MessageBox.Show("停止错误，请稍后再试");
                return;
            }

            Command.Send(Ports.MainSiemens, GetRemoteCommand("stop"));
        }


        private void OnResetButton(object sender, RoutedEventArgs e)
        {
            Command.Send(Ports.MainSiemens, GetRemoteCommand("reset"));
        }
        public string DeviceKey
        {
            get;
            set;
        }

        private bool IsStarted()
        {
            return true;
        }

        private void RadioSamplebyTime_Checked(object sender, RoutedEventArgs e)
        {
            sampling_type = 0;

            if (RadioSamplebyTime.IsChecked == true)
            {
                Text4.Content = "输入采样总时间";
                Text5.Content = "（小时）";
            }

            if (this.DeviceKey.Equals("scada.mds"))
            {
                Text6.Content = "(立方米/小时)";
                TimeSettingText.Text = "12";
                FlowSettingText.Text = "600";
                factor = 1;
                
            }
            else if (this.DeviceKey.Equals("scada.ais"))
            {
                Text6.Content = "（升/分钟）";
                TimeSettingText.Text = "12";
                FlowSettingText.Text = "40";
                factor = 60;
            }
            else { }
        }

        private void RadioSamplebyVolume_Checked(object sender, RoutedEventArgs e)
        {
            sampling_type = 1;

            if (RadioSamplebyVolume.IsChecked == true)
            {
                Text4.Content = "输入采样总量";
                Text5.Content = "（立方米）";
            }

            if (this.DeviceKey.Equals("scada.mds"))
            {
                Text6.Content = "(立方米/小时)";
                TimeSettingText.Text = "10000";
                FlowSettingText.Text = "600";
                factor = 1;
            }
            else if (this.DeviceKey.Equals("scada.ais"))
            {
                Text6.Content = "（升/分钟）";
                TimeSettingText.Text = "100";
                FlowSettingText.Text = "40";
                factor = 60;
            }
            else { }
        }


        private string Get(Dictionary<string, object> d, string key, string s)
        {
            string v = this.GetDisplayString(d, key.ToLower());
            double dv;
            if (double.TryParse(v, out dv))
            {
                return dv.ToString("0.0") + " " + s;
            }
            return v + " " + s;
        }

        private string GetAlarm(Dictionary<string, object> d, string key, string s)
        {
            string v = this.GetDisplayString(d, key.ToLower());
            bool alarm = (v == "1");
            return alarm ? "报警" : "正常";
        }

        private string GetStatus(Dictionary<string, object> d, string key, string s)
        {
            string v = this.GetDisplayString(d, key.ToLower());
            bool running = (v == "1");
            return running ? "采样中" : "停止中";
        }

        private void MarkAlarm(Dictionary<string, object> d, string key, Label label, int index)
        {
            string v = this.GetDisplayString(d, key.ToLower());
            bool alarm = (v == "1");
            // pane.SetDataColor(index, alarm);

            if (alarm)
            {
                label.Foreground = Brushes.Red;
            }
            else
            {
                label.Foreground = Brushes.Black;
            }

        }

        private string GetDisplayString(Dictionary<string, object> d, string key)
        {
            if (d.ContainsKey(key))
            {
                return (string)d[key];
            }
            return string.Empty;
        }
    }
}
