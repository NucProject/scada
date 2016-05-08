using Scada.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Scada.Data.Hub
{
    public partial class HubMainForm : Form
    {
        public HubMainForm()
        {
            InitializeComponent();
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
            HubConfig config = HubConfig.LoadConfigFromPath(datahubCfgPath);

            this.deviceTreeView.Nodes.Clear();

            foreach (var deviceConfig in config.GetAllDeviceConfig())
            {
                TreeNode deviceNode = new TreeNode(deviceConfig.Name);
                foreach (var sensorConfig in deviceConfig.GetSensorConfigList())
                {
                    TreeNode sensorNode = new TreeNode(sensorConfig.SensorName);
                    deviceNode.Nodes.Add(sensorNode);
                }
                this.deviceTreeView.Nodes.Add(deviceNode);
            }
        }
    }
}
