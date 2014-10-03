﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Scada.Chart
{

    public delegate void AppendCurvePoint(Point point);

    public delegate void ClearCurvePoints();

    // Class stands for DataSource
    public class CurveDataContext
    {
        public List<Point> points = new List<Point>();

        public event AppendCurvePoint AppendCurvePoint;

        public event ClearCurvePoints ClearCurvePoints;

        public CurveDataContext(string curveName)
        {
            this.CurveName = curveName;
        }

        public string CurveName
        {
            get;
            set;
        }

        public string DisplayName
        {
            get;
            set;
        }

        public DateTime BeginTime { get; set; }

        public DateTime EndTime { get; set; }

        public double Graduation { get; set; }

        public int GraduationCount { get; set; }

        public int Interval { get; set; }

        private List<Dictionary<string, object>> data;

        private string timeKey;

        public ChartView ChartView { get; set; }

        public void SetDataSource(List<Dictionary<string, object>> data, string valueKey, string timeKey = "time")
        {
            this.data = data;
            this.timeKey = timeKey;

            try
            {
                DateTime b = DateTime.Parse((string)data[0][timeKey]);
                DateTime e = DateTime.Parse((string)data[data.Count - 1][timeKey]);
                this.BeginTime = new DateTime(b.Year, b.Month, b.Day);
                this.EndTime = new DateTime(e.Year, e.Month, e.Day).AddDays(1);
            }
            catch (Exception)
            {
                return;
            }

            this.UpdateTimeAxis(this.BeginTime, this.EndTime);
            this.RenderCurve(this.BeginTime, this.EndTime, valueKey);
            
        }

        private void RenderCurve(DateTime beginTime, DateTime endTime, string valueKey)
        {
            
            foreach (var item in this.data)
            {
                DateTime t = DateTime.Parse((string)item[this.timeKey]);
                if (t >= beginTime && t <= endTime)
                {
                    object v = item[valueKey];
                    this.AddPoint(t, v);
                }
            }
        }

        private void UpdateTimeAxis(DateTime beginTime, DateTime endTime)
        {
            double graduation;
            int graduationCount;
            this.ChartView.UpdateTimeAxis(beginTime, endTime, out graduation, out graduationCount);

            this.Graduation = graduation;
            this.GraduationCount = graduationCount;
        }

        public void AppendData(Dictionary<string, object> item)
        {
            this.data.Add(item);
        }

        public void AddPoint(DateTime time, object value)
        {
            if (value == null)
            {
                var e = default(Point);
                this.points.Add(e);
                this.AppendCurvePoint(e);
                return;
            }
            double d = this.Graduation / this.GraduationCount;
            int index = this.GetIndexByTime(time);
            double x = index * d;

            double y = 0.0;
            if (value is string)
            {
                y = double.Parse((string)value);
            }
            else
            {
                y = (double)value;
            }
            var p = new Point(x, y);
            this.points.Add(p);
            this.AppendCurvePoint(p);
        }


        private int GetIndexByTime(DateTime time)
        {
            int index = (int)((time.Ticks - this.BeginTime.Ticks) / 10000000 / 30);
            return index;
        }

        public void Clear()
        {
            this.points.Clear();
            this.ClearCurvePoints();
        }

        
    }
}
