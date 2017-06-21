using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scada.Data.Client
{
    public class FormValue
    {
        public string Key { get; set; }

        public string Value { get; set; }

        public FormValue(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }

        public override string ToString()
        {
            return this.Key + "=" + this.Value;
        }
    }

    class FormPacket : PacketBase
    {
        // Content
        private List<FormValue> list = new List<FormValue>();

        private int index = 0;

        public string DeviceSn
        {
            get;
            set;
        }

        public FormPacket(string deviceId)
        {
            // this.list.Add("device_sn", deviceSn);
            this.DeviceId = deviceId;
            this.IsFormData = true;
        }

        public void SetData(List<Dictionary<string, object>> items)
        {
            string dataTime = "";
            JObject data = new JObject();

            foreach (var item in items)
            {
                foreach (var field in item)
                {
                    if (field.Key == "time")
                    {
                        continue;
                    }
                    this.list.Add(new FormValue(string.Format("data[{0}][{1}]", this.index, field.Key), field.Value.ToString()));

                }
                this.index += 1;
            }
        }

        public override string ToString()
        {
            return string.Join("&", this.list);
        }
    }
}
