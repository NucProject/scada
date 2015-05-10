using Scada.MainSettings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Scada.Main
{
    /// <summary>
    /// 
    /// </summary>
    public partial class SettingsForm : Form
    {
        // private SettingsContext context;

        public SettingsForm()
        {
            InitializeComponent();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            // 暂时隐藏不需要的设备设置页
            this.tabControl.Controls.Remove(this.tabPage4);
            this.tabControl.Controls.Remove(this.tabPage5);
            this.tabControl.Controls.Remove(this.tabPage6);
            this.tabControl.Controls.Remove(this.tabPage7);
            this.tabControl.Controls.Remove(this.tabPage8);
            this.tabControl.Controls.Remove(this.tabPage9);

            this.tabPage1.Controls.Add(new HpicCfgForm());
            this.tabPage2.Controls.Add(new NaICfgForm());
            this.tabPage3.Controls.Add(new WeatherCfgForm());
            //this.tabPage4.Controls.Add(new MdsCfgForm());
            //this.tabPage5.Controls.Add(new IsCfgForm());
            //this.tabPage6.Controls.Add(new EnvCfgForm());
            //this.tabPage7.Controls.Add(new DwdCfgForm());
            //this.tabPage8.Controls.Add(new CinderlCfgForm());
            //this.tabPage9.Controls.Add(new HPGECfgForm());
        }

        private void sureButton_Click(object sender, EventArgs e)
        {
            TabPage page = this.tabControl.TabPages[this.tabControl.SelectedIndex];
            IApply c = (IApply)page.Controls[0];
            c.Apply();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            TabPage page = this.tabControl.TabPages[this.tabControl.SelectedIndex];
            IApply c = (IApply)page.Controls[0];
            c.Cancel();
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = this.tabControl.SelectedIndex;
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }




    }
}
