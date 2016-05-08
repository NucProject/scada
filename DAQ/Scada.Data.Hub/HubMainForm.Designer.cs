namespace Scada.Data.Hub
{
    partial class HubMainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.mainTab = new System.Windows.Forms.TabControl();
            this.configTabPage = new System.Windows.Forms.TabPage();
            this.sendTabPage = new System.Windows.Forms.TabPage();
            this.menuBar = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.connectStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.deviceTreeView = new System.Windows.Forms.TreeView();
            this.mainTab.SuspendLayout();
            this.configTabPage.SuspendLayout();
            this.menuBar.SuspendLayout();
            this.statusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTab
            // 
            this.mainTab.Controls.Add(this.configTabPage);
            this.mainTab.Controls.Add(this.sendTabPage);
            this.mainTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTab.Location = new System.Drawing.Point(0, 25);
            this.mainTab.Name = "mainTab";
            this.mainTab.SelectedIndex = 0;
            this.mainTab.Size = new System.Drawing.Size(905, 353);
            this.mainTab.TabIndex = 0;
            // 
            // configTabPage
            // 
            this.configTabPage.Controls.Add(this.deviceTreeView);
            this.configTabPage.Location = new System.Drawing.Point(4, 22);
            this.configTabPage.Name = "configTabPage";
            this.configTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.configTabPage.Size = new System.Drawing.Size(897, 327);
            this.configTabPage.TabIndex = 1;
            this.configTabPage.Text = "配置管理";
            this.configTabPage.UseVisualStyleBackColor = true;
            // 
            // sendTabPage
            // 
            this.sendTabPage.Location = new System.Drawing.Point(4, 22);
            this.sendTabPage.Name = "sendTabPage";
            this.sendTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.sendTabPage.Size = new System.Drawing.Size(897, 327);
            this.sendTabPage.TabIndex = 2;
            this.sendTabPage.Text = "tabPage1";
            this.sendTabPage.UseVisualStyleBackColor = true;
            // 
            // menuBar
            // 
            this.menuBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuBar.Location = new System.Drawing.Point(0, 0);
            this.menuBar.Name = "menuBar";
            this.menuBar.Size = new System.Drawing.Size(905, 25);
            this.menuBar.TabIndex = 1;
            this.menuBar.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.restartToolStripMenuItem,
            this.stopToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.fileToolStripMenuItem.Text = "文件";
            // 
            // restartToolStripMenuItem
            // 
            this.restartToolStripMenuItem.Name = "restartToolStripMenuItem";
            this.restartToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.restartToolStripMenuItem.Text = "重新启动";
            this.restartToolStripMenuItem.Click += new System.EventHandler(this.restartToolStripMenuItem_Click);
            // 
            // stopToolStripMenuItem
            // 
            this.stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            this.stopToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.stopToolStripMenuItem.Text = "停止上传";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(121, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.exitToolStripMenuItem.Text = "退出";
            // 
            // statusBar
            // 
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectStatusLabel});
            this.statusBar.Location = new System.Drawing.Point(0, 356);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(905, 22);
            this.statusBar.TabIndex = 2;
            this.statusBar.Text = "statusStrip1";
            // 
            // connectStatusLabel
            // 
            this.connectStatusLabel.Name = "connectStatusLabel";
            this.connectStatusLabel.Size = new System.Drawing.Size(44, 17);
            this.connectStatusLabel.Text = "未连接";
            // 
            // deviceTreeView
            // 
            this.deviceTreeView.Location = new System.Drawing.Point(8, 6);
            this.deviceTreeView.Name = "deviceTreeView";
            this.deviceTreeView.Size = new System.Drawing.Size(420, 300);
            this.deviceTreeView.TabIndex = 1;
            // 
            // HubMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(905, 378);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.mainTab);
            this.Controls.Add(this.menuBar);
            this.Name = "HubMainForm";
            this.Text = "数据上传";
            this.mainTab.ResumeLayout(false);
            this.configTabPage.ResumeLayout(false);
            this.menuBar.ResumeLayout(false);
            this.menuBar.PerformLayout();
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl mainTab;
        private System.Windows.Forms.TabPage configTabPage;
        private System.Windows.Forms.MenuStrip menuBar;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripStatusLabel connectStatusLabel;
        private System.Windows.Forms.TabPage sendTabPage;
        private System.Windows.Forms.ToolStripMenuItem restartToolStripMenuItem;
        private System.Windows.Forms.TreeView deviceTreeView;
    }
}

