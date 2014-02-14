﻿namespace Scada.Data.Client.Tcp
{
    partial class AgentWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AgentWindow));
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel1 = new System.Windows.Forms.Panel();
            this.mainTabCtrl = new System.Windows.Forms.TabControl();
            this.connPage = new System.Windows.Forms.TabPage();
            this.mainListBox = new System.Windows.Forms.ListBox();
            this.historyPage = new System.Windows.Forms.TabPage();
            this.dataPage = new System.Windows.Forms.TabPage();
            this.detailsListView = new System.Windows.Forms.ListView();
            this.deviceCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.timeCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.historyTimeCol = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.OpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.StartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PauseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.QuitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DispToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LoggerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ClsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.sysNotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.startStripButton = new System.Windows.Forms.ToolStripButton();
            this.loggerStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.connHistoryList = new System.Windows.Forms.ListBox();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.mainTabCtrl.SuspendLayout();
            this.connPage.SuspendLayout();
            this.historyPage.SuspendLayout();
            this.dataPage.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.statusStrip1);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitter1);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.panel1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(520, 388);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            // 
            // toolStripContainer1.RightToolStripPanel
            // 
            this.toolStripContainer1.RightToolStripPanel.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.toolStripContainer1.Size = new System.Drawing.Size(520, 457);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.menuStrip1);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.Location = new System.Drawing.Point(0, 0);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(520, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 388);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.mainTabCtrl);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(520, 388);
            this.panel1.TabIndex = 1;
            // 
            // mainTabCtrl
            // 
            this.mainTabCtrl.Controls.Add(this.connPage);
            this.mainTabCtrl.Controls.Add(this.historyPage);
            this.mainTabCtrl.Controls.Add(this.dataPage);
            this.mainTabCtrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTabCtrl.Location = new System.Drawing.Point(0, 0);
            this.mainTabCtrl.Name = "mainTabCtrl";
            this.mainTabCtrl.SelectedIndex = 0;
            this.mainTabCtrl.Size = new System.Drawing.Size(520, 388);
            this.mainTabCtrl.TabIndex = 4;
            // 
            // connPage
            // 
            this.connPage.Controls.Add(this.mainListBox);
            this.connPage.Location = new System.Drawing.Point(4, 22);
            this.connPage.Name = "connPage";
            this.connPage.Padding = new System.Windows.Forms.Padding(3);
            this.connPage.Size = new System.Drawing.Size(512, 362);
            this.connPage.TabIndex = 0;
            this.connPage.Text = "连接信息";
            this.connPage.UseVisualStyleBackColor = true;
            // 
            // listBox1
            // 
            this.mainListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainListBox.FormattingEnabled = true;
            this.mainListBox.Location = new System.Drawing.Point(3, 3);
            this.mainListBox.Name = "listBox1";
            this.mainListBox.Size = new System.Drawing.Size(506, 356);
            this.mainListBox.TabIndex = 0;
            // 
            // historyPage
            // 
            this.historyPage.Controls.Add(this.connHistoryList);
            this.historyPage.Location = new System.Drawing.Point(4, 22);
            this.historyPage.Name = "historyPage";
            this.historyPage.Padding = new System.Windows.Forms.Padding(3);
            this.historyPage.Size = new System.Drawing.Size(512, 362);
            this.historyPage.TabIndex = 1;
            this.historyPage.Text = "连接历史";
            this.historyPage.UseVisualStyleBackColor = true;
            // 
            // dataPage
            // 
            this.dataPage.Controls.Add(this.detailsListView);
            this.dataPage.Location = new System.Drawing.Point(4, 22);
            this.dataPage.Name = "dataPage";
            this.dataPage.Padding = new System.Windows.Forms.Padding(3);
            this.dataPage.Size = new System.Drawing.Size(512, 362);
            this.dataPage.TabIndex = 2;
            this.dataPage.Text = "数据上传";
            this.dataPage.UseVisualStyleBackColor = true;
            // 
            // detailsListView
            // 
            this.detailsListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.deviceCol,
            this.timeCol,
            this.historyTimeCol});
            this.detailsListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.detailsListView.FullRowSelect = true;
            this.detailsListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.detailsListView.Location = new System.Drawing.Point(3, 3);
            this.detailsListView.MultiSelect = false;
            this.detailsListView.Name = "detailsListView";
            this.detailsListView.Size = new System.Drawing.Size(506, 356);
            this.detailsListView.TabIndex = 0;
            this.detailsListView.UseCompatibleStateImageBehavior = false;
            this.detailsListView.View = System.Windows.Forms.View.Details;
            // 
            // deviceCol
            // 
            this.deviceCol.Text = "设备";
            this.deviceCol.Width = 142;
            // 
            // timeCol
            // 
            this.timeCol.Text = "最新上传时间";
            this.timeCol.Width = 144;
            // 
            // historyTimeCol
            // 
            this.historyTimeCol.Text = "最新历史数据上传时间";
            this.historyTimeCol.Width = 171;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpToolStripMenuItem,
            this.DispToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(520, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // OpToolStripMenuItem
            // 
            this.OpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StartToolStripMenuItem,
            this.PauseToolStripMenuItem,
            this.toolStripSeparator2,
            this.QuitToolStripMenuItem});
            this.OpToolStripMenuItem.Name = "OpToolStripMenuItem";
            this.OpToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
            this.OpToolStripMenuItem.Text = "操作";
            // 
            // StartToolStripMenuItem
            // 
            this.StartToolStripMenuItem.Name = "StartToolStripMenuItem";
            this.StartToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.StartToolStripMenuItem.Text = "启动";
            this.StartToolStripMenuItem.Click += new System.EventHandler(this.StartToolStripMenuItem_Click);
            // 
            // PauseToolStripMenuItem
            // 
            this.PauseToolStripMenuItem.Name = "PauseToolStripMenuItem";
            this.PauseToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.PauseToolStripMenuItem.Text = "暂停";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(149, 6);
            // 
            // QuitToolStripMenuItem
            // 
            this.QuitToolStripMenuItem.Name = "QuitToolStripMenuItem";
            this.QuitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.QuitToolStripMenuItem.Text = "退出";
            this.QuitToolStripMenuItem.Click += new System.EventHandler(this.QuitToolStripMenuItem_Click);
            // 
            // DispToolStripMenuItem
            // 
            this.DispToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LoggerToolStripMenuItem,
            this.ClsToolStripMenuItem1});
            this.DispToolStripMenuItem.Name = "DispToolStripMenuItem";
            this.DispToolStripMenuItem.Size = new System.Drawing.Size(45, 20);
            this.DispToolStripMenuItem.Text = "显示";
            // 
            // LoggerToolStripMenuItem
            // 
            this.LoggerToolStripMenuItem.Name = "LoggerToolStripMenuItem";
            this.LoggerToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.LoggerToolStripMenuItem.Text = "日志";
            this.LoggerToolStripMenuItem.Click += new System.EventHandler(this.LoggerToolStripMenuItem_Click);
            // 
            // ClsToolStripMenuItem1
            // 
            this.ClsToolStripMenuItem1.Name = "ClsToolStripMenuItem1";
            this.ClsToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.ClsToolStripMenuItem1.Text = "清屏";
            // 
            // sysNotifyIcon
            // 
            this.sysNotifyIcon.Text = "数据上传";
            this.sysNotifyIcon.Visible = true;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.startStripButton,
            this.loggerStripButton1});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolStrip1.Location = new System.Drawing.Point(5, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(107, 23);
            this.toolStrip1.TabIndex = 1;
            // 
            // startStripButton
            // 
            this.startStripButton.Image = ((System.Drawing.Image)(resources.GetObject("startStripButton.Image")));
            this.startStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.startStripButton.Name = "startStripButton";
            this.startStripButton.Size = new System.Drawing.Size(53, 20);
            this.startStripButton.Text = "启动";
            // 
            // loggerStripButton1
            // 
            this.loggerStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("loggerStripButton1.Image")));
            this.loggerStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.loggerStripButton1.Name = "loggerStripButton1";
            this.loggerStripButton1.Size = new System.Drawing.Size(53, 20);
            this.loggerStripButton1.Tag = "";
            this.loggerStripButton1.Text = "日志";
            this.loggerStripButton1.Click += new System.EventHandler(this.loggerStripButton1_Click);
            // 
            // connHistoryList
            // 
            this.connHistoryList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.connHistoryList.FormattingEnabled = true;
            this.connHistoryList.Location = new System.Drawing.Point(3, 3);
            this.connHistoryList.Name = "connHistoryList";
            this.connHistoryList.Size = new System.Drawing.Size(506, 356);
            this.connHistoryList.TabIndex = 0;
            // 
            // AgentWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(520, 457);
            this.Controls.Add(this.toolStripContainer1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "AgentWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据中心代理";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AgentWindow_FormClosing);
            this.Load += new System.EventHandler(this.AgentWindow_Load);
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.mainTabCtrl.ResumeLayout(false);
            this.connPage.ResumeLayout(false);
            this.historyPage.ResumeLayout(false);
            this.dataPage.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.NotifyIcon sysNotifyIcon;
        private System.Windows.Forms.TabControl mainTabCtrl;
        private System.Windows.Forms.TabPage connPage;
        private System.Windows.Forms.TabPage historyPage;
        private System.Windows.Forms.ListBox mainListBox;
        private System.Windows.Forms.TabPage dataPage;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem OpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem StartToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem PauseToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem QuitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DispToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ClsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem LoggerToolStripMenuItem;
        private System.Windows.Forms.ListView detailsListView;
        private System.Windows.Forms.ColumnHeader deviceCol;
        private System.Windows.Forms.ColumnHeader timeCol;
        private System.Windows.Forms.ColumnHeader historyTimeCol;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton startStripButton;
        private System.Windows.Forms.ToolStripButton loggerStripButton1;
        private System.Windows.Forms.ListBox connHistoryList;

    }
}

