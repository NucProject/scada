using Scada.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scada.Data.Hub
{
    public partial class HubMainForm : Form
    {
        private HubConfig config = null;

        private bool QuitThread { get; set; }

        private bool NetConnected { get; set; }

        private Thread currentThread;

        public int UpdateFrequency { get; set; }

        // private DataAgent agent;



        public HubMainForm()
        {
            InitializeComponent();

            this.UpdateFrequency = 10;
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            startHubClient();
        }

        private void startHubClient()
        {
            string datahubCfgPath = ConfigPath.GetConfigFilePath("datahub");
            this.config = HubConfig.LoadConfigFromPath(datahubCfgPath);

            this.deviceTreeView.Nodes.Clear();
            this.sendDataList.Items.Clear();

            foreach (var deviceConfig in this.config.GetAllDeviceConfig())
            {
                string nodeName = string.Format("{0} ({1})", deviceConfig.Name, deviceConfig.DisplayName);
                TreeNode deviceNode = new TreeNode(nodeName);
                foreach (var sensorConfig in deviceConfig.GetSensorConfigList())
                {
                    
                    TreeNode sensorNode = new TreeNode(sensorConfig.SensorName);
                    deviceNode.Nodes.Add(sensorNode);
                }
                this.deviceTreeView.Nodes.Add(deviceNode);

                var listItem = new ListViewItem(nodeName);
                listItem.Tag = deviceConfig.Name.ToLower();
                listItem.SubItems.Add("--");
                listItem.SubItems.Add("--");
                listItem.SubItems.Add("--");
                this.sendDataList.Items.Add(listItem);
            }

            // TODO: 可能创建多个线程
            this.StartDataThread(config);
        }

        private void StartDataThread(HubConfig config)
        {
            if (this.currentThread != null)
            {
                this.currentThread.Abort();
            }

            SynchronizationContext sc = SynchronizationContext.Current;
            this.currentThread = new Thread(new ParameterizedThreadStart(this.DataThread));
            this.currentThread.Start(sc);
        }

        private Dictionary<string, DeviceSendInfo> GetThreadInfoDict()
        {
            Dictionary<string, DeviceSendInfo> dict = new Dictionary<string, DeviceSendInfo>();
            foreach (var deviceConfig in this.config.GetAllDeviceConfig())
            {
                string deviceKey = deviceConfig.Name.ToLower();
                dict.Add(deviceKey, new DeviceSendInfo());
            }

            return dict;
        }

        private void DataThread(object param)
        {
            var dict = this.GetThreadInfoDict();
            DataAgent agent = new DataAgent();

            SynchronizationContext sc = (SynchronizationContext)param;
            long counter = 0;
            while (true)
            {
                if (this.QuitThread)
                {
                    break;
                }

                UINotifyEvent notify = UINotifyEvent.CreateNotifyEvent("");
                foreach (var deviceConfig in this.config.GetAllDeviceConfig())
                {
                    DateTime time;
                    if (!this.IsTimeOkToSendDeviceData(deviceConfig, out time))
                    {
                        continue;
                    }

                    if (this.SendDeviceData(agent, deviceConfig, time))
                    {
                        string deviceKey = deviceConfig.Name.ToLower();
                        DeviceSendInfo sendInfo = dict[deviceKey];
                        sendInfo.AddCount();
                    }
                }

                // 每N个回调一次UI, 更新上传状态
                if (counter % this.UpdateFrequency == 0)
                {
                    notify.SetDevicesInfo(dict);
                    sc.Post(new SendOrPostCallback(this.OnSendDeviceDataCallback), notify);
                    Thread.Sleep(500);
                }

                counter++;
            }
        }

        private void OnSendDeviceDataCallback(object o)
        {
            UINotifyEvent notify = (UINotifyEvent)o;


            var info = notify.GetDevicesSendInfo();

            foreach (ListViewItem listItem in this.sendDataList.Items)
            {
                string deviceKey = (string)listItem.Tag;
                DeviceSendStruct d = info[deviceKey];

                listItem.SubItems[1].Text = d.countToday.ToString();
                listItem.SubItems[2].Text = d.countSum.ToString();
                listItem.SubItems[3].Text = "0000-00-00 00:00:00";
            }

            connectStatusLabel.Text = this.NetConnected ? "连接" : "未连接";

            // 更新线程运行时间
            threadStatusLabel.Text = DateTime.Now.ToString();
        }

        /// <summary>
        /// Threading operation
        /// </summary>
        /// <param name="deviceConfig"></param>
        private bool SendDeviceData(DataAgent agent, DeviceConfig deviceConfig, DateTime time)
        {
            Packet p = null;
            if (deviceConfig.IsHubFormat)
            {
                p = HubPacket.CreateRealtimePacket(deviceConfig, time);
            }
            
            // HubPacket p = HubPacket.CreateRealtimePacket(deviceConfig, time);
            if (p == null)
            {
                return false;
            }
            string action = deviceConfig.Action;
            if (agent.SendDataPacket(action, p, time))
            {
                p.SetSendStatus(deviceConfig, time);
            }

            return true;
        }

        /// <summary>
        /// Threading operation
        /// </summary>
        /// <param name="deviceConfig"></param>
        /// <returns></returns>
        private bool IsTimeOkToSendDeviceData(DeviceConfig deviceConfig, out DateTime time)
        {
            if (deviceConfig.TimeToSend == DeviceConfig.Anytime)
            {
                time = DateTime.Now;
                return true;
            }
            else
            {
                time = DateTime.Now; // TODO:
                return true;
            }
            // return false;
        }

        private void configTabPage_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.QuitThread = true;
            Application.Exit();
        }

        private void HubMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.QuitThread = true;
            Application.Exit();
        }

        private void HubMainForm_Load(object sender, EventArgs e)
        {

        }
    }
}
