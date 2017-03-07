using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scada.Data.Client
{
    class PacketV2 : PacketBase
    {
        // Content
        private JObject jobject = new JObject();

        public string DeviceSn
        {
            get;
            set;
        }

        public PacketV2(string deviceSn)
        {
            jobject["device_sn"] = deviceSn;
        }

        public void SetData(Dictionary<string, object> items)
        {
            string dataTime = "";
            JObject data = new JObject();
            foreach (var item in items)
            {
                if (string.Compare(item.Key, "time", true) == 0)
                {
                    dataTime = (string)item.Value;
                    continue;
                }
                data[item.Key] = JToken.FromObject(item.Value);
            }

            jobject["data_time"] = dataTime;
            jobject["data"] = data;
        }

        public override string ToString()
        {
            return this.jobject.ToString();
        }
    }
}
