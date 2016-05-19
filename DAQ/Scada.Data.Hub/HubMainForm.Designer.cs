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
            this.deviceTreeView = new System.Windows.Forms.TreeView();
            this.sendTabPage = new System.Windows.Forms.TabPage();
            this.sendDataList = new System.Windows.Forms.ListView();
            this.menuBar = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.restartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.connectStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.threadStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.mainTab.SuspendLayout();
            this.configTabPage.SuspendLayout();
            this.sendTabPage.SuspendLayout();
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
            this.configTabPage.Click += new System.EventHandler(this.configTabPage_Click);
            // 
            // deviceTreeView
            // 
            this.deviceTreeView.Location = new System.Drawing.Point(8, 6);
            this.deviceTreeView.Name = "deviceTreeView";
            this.deviceTreeView.Size = new System.Drawing.Size(420, 300);
            this.deviceTreeView.TabIndex = 1;
            // 
            // sendTabPage
            // 
            this.sendTabPage.Controls.Add(this.sendDataList);
            this.sendTabPage.Location = new System.Drawing.Point(4, 22);
            this.sendTabPage.Name = "sendTabPage";
            this.sendTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.sendTabPage.Size = new System.Drawing.Size(897, 327);
            this.sendTabPage.TabIndex = 2;
            this.sendTabPage.Text = "发送情况";
            this.sendTabPage.UseVisualStyleBackColor = true;
            // 
            // sendDataList
            // 
            this.sendDataList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4});
            this.sendDataList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sendDataList.Location = new System.Drawing.Point(3, 3);
            this.sendDataList.Name = "sendDataList";
            this.sendDataList.Size = new System.Drawing.Size(891, 321);
            this.sendDataList.TabIndex = 0;
            this.sendDataList.UseCompatibleStateImageBehavior = false;
            this.sendDataList.View = System.Windows.Forms.View.Details;
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
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // statusBar
            // 
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectStatusLabel,
            this.threadStatusLabel});
            this.statusBar.Location = new System.Drawing.Point(0, 356);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(905, 22);
            this.statusBar.TabIndex = 2;
            this.statusBar.Text = "statusStrip1";
            // 
            // connectStatusLabel
            // 
            this.connectStatusLabel.Name = "connectStatusLabel";
            this.connectStatusLabel.Size = new System.Drawing.Size(52, 17);
            this.connectStatusLabel.Text = "[未连接]";
            // 
            // threadStatusLabel
            // 
            this.threadStatusLabel.Name = "threadStatusLabel";
            this.threadStatusLabel.Size = new System.Drawing.Size(59, 17);
            this.threadStatusLabel.Text = "线程时间:";
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "设备名称";
            this.columnHeader1.Width = 119;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "当日发送数量";
            this.columnHeader2.Width = 130;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "累积发送数量";
            this.columnHeader3.Width = 143;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "最后发送时间";
            this.columnHeader4.Width = 235;
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
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.HubMainForm_FormClosing);
            this.mainTab.ResumeLayout(false);
            this.configTabPage.ResumeLayout(false);
            this.sendTabPage.ResumeLayout(false);
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
        private System.Windows.Forms.ListView sendDataList;
        private System.Windows.Forms.ToolStripStatusLabel threadStatusLabel;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
    }
}

