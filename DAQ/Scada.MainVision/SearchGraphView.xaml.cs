﻿
using System;
using System.Collections.Generic;
using System.Windows.Media;
using Scada.Chart;
using System.Windows.Forms;

namespace Scada.MainVision
{
    /// <summary>
    /// Interaction logic for GraphViewPanel.xaml in MainVision
    /// </summary>
    public partial class SearchGraphView : System.Windows.Controls.UserControl
    {
        public const string TimeKey = "Time";

        DateTime now = DateTime.Now;

        static Color[] colors = { Colors.Green, Colors.Red, Colors.Blue, Colors.OrangeRed, Colors.Purple };
        
        private Dictionary<string, CurveDataContext> dataSources = new Dictionary<string, CurveDataContext>();

        public SearchGraphView()
        {
            InitializeComponent();

        }

        public void SetDataSource(List<Dictionary<string, object>> dataSource, string valueKey)
        {
            if (dataSource == null || dataSource.Count == 0)
            {
                return;
            }

            this.SearchChartView.SetDataSource(dataSource, valueKey);
        }

        public int Interval
        {
            get
            {
                return this.SearchChartView.Interval;
            }

            set
            {
                this.SearchChartView.Interval = value;
            }
        }

        public void AddLineName(string deviceKey, string lineName, string displayName)
        {
            // TODO:
            if (lineName.IndexOf("DoseRate") >= 0)
            {
                displayName = displayName.Replace("μSv/h", "nSv/h");
            }

            Config cfg = Config.Instance();
            ConfigEntry entry = cfg[deviceKey];

            ConfigItem item = entry.GetConfigItem(lineName);
            this.SearchChartView.SetValueRange(item.Min, item.Max);

        }

        private void AddTimePoint(DateTime time, Dictionary<string, object> entry)
        {
            foreach (string key in dataSources.Keys)
            {
                // 存在这条曲线
                if (entry.ContainsKey(key))
                {
                    string v = (string)entry[key];
                    double r = 0.0;
                    if (v.Length > 0)
                    {
                        if (!double.TryParse(v, out r))
                        {
                            return;
                        }
                    }

                    //this.CurveDataContext dataContext = dataSources[key];
                    //dataContext.AddPoint(time, r);
                }
            }
        }

        internal void SaveChart()
        {
            string filePath = string.Empty;
            System.Windows.Forms.SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.InitialDirectory = "C://";
            fileDialog.Filter = "曲线图片 (*.bmp)|*.bmp|All files (*.*)|*.*";
            fileDialog.FilterIndex = 1;
            fileDialog.RestoreDirectory = true;
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                filePath = fileDialog.FileName;
            }
            this.SearchChartView.SaveChart(filePath);
        }
    }

}
