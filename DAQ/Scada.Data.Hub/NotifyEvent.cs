using System;
using System.Collections.Generic;

namespace Scada.Data.Hub
{
    public struct DeviceSendStruct
    {
        public long countToday;

        public long countSum;

        public DeviceSendStruct(long countToday, long countSum)
        {
            this.countToday = countToday;
            this.countSum = countSum;
        }
    }

    public class DeviceSendInfo
    {
        private int currentDay = 0;

        private long countToday = 0;

        private long countSum = 0;

        private string DeviceKey { get; set; }

        public void AddCount()
        {
            this.countSum += 1;
            this.countToday += 1;
            if (DateTime.Now.Day != this.currentDay)
            {
                this.currentDay = DateTime.Now.Day;
                this.countToday = 1;
            }
        }

        public long GetCountToday()
        {
            return this.countToday;
        }

        public long GetCountSum()
        {
            return this.countSum;
        }
    }

    public class UINotifyEvent
    {
        private static long notifyEventCount = 0;

        private Dictionary<string, DeviceSendStruct> dict;

        public long NotifyEventCount { get; set; }

        public string Message { get; set; }

        private UINotifyEvent(string message, long notifyEventCount)
        {
            this.Message = message;
            this.NotifyEventCount = notifyEventCount;
        }

        internal static UINotifyEvent CreateNotifyEvent(string message)
        {
            UINotifyEvent.notifyEventCount += 1;
            UINotifyEvent e = new UINotifyEvent(message, UINotifyEvent.notifyEventCount);
            return e;
        }

        internal void SetDevicesInfo(Dictionary<string, DeviceSendInfo> dict)
        {
            this.dict = new Dictionary<string, DeviceSendStruct>(dict.Count);
            foreach (var kv in dict)
            {
                DeviceSendInfo info = kv.Value;
                DeviceSendStruct v = new DeviceSendStruct(info.GetCountToday(), info.GetCountSum());
                this.dict.Add(kv.Key, v);
            }
        }

        public Dictionary<string, DeviceSendStruct> GetDevicesSendInfo()
        {
            return this.dict;
        }
    }
}