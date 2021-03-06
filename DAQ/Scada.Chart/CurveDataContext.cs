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

        public CurveDataContext()
        {
        }

        public CurveDataContext(ChartView chartView)
        {
            this.chartView = chartView;
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

        public DateTime CurrentBaseTime { get; set; }

        public double Graduation { get; set; }

        public int GraduationCount { get; set; }

        public int Interval { get; set; }

        private List<Dictionary<string, object>> data;

        private string timeKey;

        private ChartView chartView;

        private string currentValueKey;

        public void SetDataSource(List<Dictionary<string, object>> data, string valueKey, DateTime beginTime, DateTime endTime, string timeKey = "time")
        {

            this.data = data;
            this.timeKey = timeKey;
            this.BeginTime = beginTime;
            this.EndTime = endTime;

            DateTime dataBeginTime = default(DateTime);
            DateTime dataEndTime = default(DateTime);
            try
            {
                dataBeginTime = DateTime.Parse((string)data[0][timeKey]);
                dataEndTime = DateTime.Parse((string)data[data.Count - 1][timeKey]);
                // this.BeginTime = new DateTime(b.Year, b.Month, b.Day);
                // this.EndTime = new DateTime(e.Year, e.Month, e.Day).AddDays(1);
            }
            catch (Exception)
            {
                return;
            }

            this.currentValueKey = valueKey;
            this.Interval = this.chartView.Interval;
            this.CurrentBaseTime = this.BeginTime;
            this.UpdateTimeAxis(this.BeginTime, this.EndTime);
            this.ClearCurvePoints();
            this.RenderCurve(data, dataBeginTime, dataEndTime, valueKey);
        }

        public void AppendDataSource(List<Dictionary<string, object>> data, string valueKey, string timeKey = "time")
        {
            this.data.AddRange(data);
            this.timeKey = timeKey;

            DateTime beginTime;
            DateTime endTime;
            try
            {
                beginTime = DateTime.Parse((string)data[0][timeKey]);
                endTime = DateTime.Parse((string)data[data.Count - 1][timeKey]);
                // this.BeginTime = new DateTime(b.Year, b.Month, b.Day);
                // this.EndTime = new DateTime(e.Year, e.Month, e.Day).AddDays(1);
            }
            catch (Exception)
            {
                return;
            }

            //this.UpdateTimeAxis(beginTime, endTime);
            // this.UpdateTimeAxis(this.BeginTime, this.EndTime);
            this.CurrentBaseTime = beginTime;
            this.RenderCurve(data, beginTime, endTime, valueKey);
        }

        // This is for Realtime chart now.
        public void SetDataSource2(List<Dictionary<string, object>> data, string valueKey, string timeKey = "time")
        {
            this.data = data;
            this.timeKey = timeKey;

            try
            {
                DateTime b = DateTime.Parse((string)data[0][timeKey]);
                DateTime e = DateTime.Parse((string)data[data.Count - 1][timeKey]);
                this.BeginTime = new DateTime(b.Year, b.Month, b.Day, b.Hour, b.Minute, b.Second / 30 * 30);
                this.EndTime = e;
            }
            catch (Exception)
            {
                return;
            }

            this.currentValueKey = valueKey;
            this.Interval = this.chartView.Interval;
            this.UpdateTimeAxis(this.BeginTime, this.EndTime, false);
            this.RenderCurve(data, this.BeginTime, this.EndTime, valueKey);

        }

        private void RenderCurve(List<Dictionary<string, object>> data, DateTime beginTime, DateTime endTime, string valueKey)
        {
            if (data == null)
                return;

            DateTime lastTime = default(DateTime);
            foreach (var item in data)
            {
                DateTime t = DateTime.Parse((string)item[this.timeKey]);
                if (t >= beginTime && t <= endTime)
                {
                    if (lastTime != default(DateTime) && (t.Ticks - lastTime.Ticks) / 10000000 != this.Interval)
                    {
                        this.AddPoint(t, null);
                    }
                    if (item.ContainsKey(valueKey))
                    {
                        object v = item[valueKey];
                        this.AddPoint(t, v);
                    }
                    else
                    {
                        this.AddPoint(t, null);
                    }
                }
                lastTime = t;
            }
        }

        private void UpdateTimeAxis(DateTime beginTime, DateTime endTime, bool completedDays = true)
        {
            double graduation;
            int graduationCount;
            this.chartView.UpdateTimeAxis(beginTime, endTime, completedDays, out graduation, out graduationCount);
            this.Graduation = graduation;
            this.GraduationCount = graduationCount;
        }

        internal void AddPoint(DateTime time, object value)
        {
            if (value == null)
            {
                var e = default(Point);
                this.points.Add(e);
                this.AppendCurvePoint(e);
                return;
            }
            double d = this.Graduation / this.GraduationCount;
            if (this.Interval == 300)
            {
                d *= 10;
            }
            else if (this.Interval == 3600)
            {
                d *= 120;
            }
            int index = this.GetIndexByTime(time);
            double x = index * d;

            double y = 0.0;
            if (value is string)
            {
                string v = (string)value;
                double.TryParse(v, out y);
            }
            else if (value is bool)
            {
                y = (bool)value ? 50 : 10;
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
            int index = (int)((time.Ticks - this.CurrentBaseTime.Ticks) / 10000000 / this.Interval);
            return index;
        }

        public void Clear()
        {
            this.points.Clear();
            this.ClearCurvePoints();
        }

        private DateTime GetTimeByX(double x)
        {
            
            double s = x * this.GraduationCount * this.Interval / this.Graduation;
            if (!double.IsNaN(s))
            {
                if (this.Interval == 300)
                {
                    s /= 10;
                }
                if (this.Interval == 3600)
                {
                    s /= 120;
                }
                return this.BeginTime.AddSeconds(s);
            }
            return this.BeginTime;
        }

        private DateTime GetRegularTime(DateTime t, int hours = 0)
        {
            if (this.Interval == 3600)
            {
                return new DateTime(t.Year, t.Month, t.Day, 0, 0, 0).AddHours(24);
            }
            else
            {
                int h = t.Hour / 2 * 2;
                return new DateTime(t.Year, t.Month, t.Day, h, 0, 0).AddHours(hours);
            }
        }

        internal void UpdateRange(double beginPointX, double endPointX)
        {
            DateTime beginTime = this.GetTimeByX(beginPointX);
            DateTime endTime = this.GetTimeByX(endPointX);

            beginTime = this.GetRegularTime(beginTime);
            endTime = this.GetRegularTime(endTime, 1);
            this.Clear();

            this.CurrentBaseTime = beginTime;
            this.UpdateTimeAxis(beginTime, endTime, false);
            this.RenderCurve(this.data, beginTime, endTime, this.currentValueKey);

            if (this.chartView.updateRangeAction != null)
            {
                this.chartView.updateRangeAction(beginPointX, endPointX);
            }

            this.chartView.UpdateCurve();
        }

        internal void Reset()
        {
            this.SetDataSource(this.data, this.currentValueKey, this.BeginTime, this.EndTime, this.timeKey);
            if (this.chartView.resetAction != null)
            {
                this.chartView.resetAction();
            }
        }
    }
}
