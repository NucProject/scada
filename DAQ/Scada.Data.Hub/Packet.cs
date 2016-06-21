using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace Scada.Data.Hub
{
    public class ValueItem
    {
        public string Name { get; set; }
        public string Value{ get; set; }
    }

    /*
    {
    "type": "realtime",
    "time": "2016-4-27 11: 02: 00",
    "Device_id": "01234567",
    "Lat": 49.368335,
    "Lat_gps":49.79665,
    "Lng": 120.87998,
    "Lng_gps":120.9667,	
    "values": [
        {
            "sensor": "Doserate_int",
            "value": 203.9
        },
        {
            "sensor": "Doserate_ext",
            "value": 179.6
        }
        ]
    } */
    public class Packet
    {
        // Const keys.
        public const string EntryKey = "entry";

        public const string StationKey = "station";

        public const string TokenKey = "token";

        public bool realtime = true;

        public DateTime time { get; set; }

        private int result = 0;

        private bool hasResult = false;

        private List<ValueItem> valueList = null;

        public bool IsFilePacket
        {
            get;
            set;
        }

        public string Options
        {
            get;
            set;
        }

        public Packet()
        {
        }

        private int Result
        {
            get 
            {
                return this.result;
            }
            set
            {
                this.result = value;
                this.hasResult = true;
            }
        }

        public string Id
        {
            get;
            set;
        }

        public string Message
        {
            get;
            set;
        }


        public string DeviceKey
        {
            get;
            set;
        }

        public string ToJson()
        {
            JObject json = new JObject();

            json["type"] = this.realtime ? "realtime" : "history";
            
            json["Device_id"] = this.DeviceKey;

            JArray values = new JArray();
            foreach (ValueItem valueItem in this.valueList)
            {
                if (valueItem.Name.ToLower() == "time")
                {
                    json["time"] = valueItem.Value;
                }
                else
                {
                    JObject sensorValue = new JObject();
                    sensorValue["sensor"] = valueItem.Name;
                    sensorValue["value"] = valueItem.Value;
                    values.Add(sensorValue);
                }
            }
            
            json["values"] = values;
            return json.ToString();
        }

        
        private static DateTime StartTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));

        public static long GetUnixTime(string time)
        {
            DateTime nowTime = DateTime.Now;
            DateTime dateTime = DateTime.Parse(time);
            long unixTime = (long)Math.Round((dateTime - StartTime).TotalMilliseconds, MidpointRounding.AwayFromZero);
            return unixTime;
        }

        public static long GetUnixTime2(string time)
        {
            DateTime nowTime = DateTime.Now;
            DateTime dateTime = DateTime.Parse(time);
            long unixTime = (long)Math.Round((dateTime - StartTime).TotalMilliseconds, MidpointRounding.AwayFromZero);
            return unixTime / 1000;
        }

        public static Packet CreateRealtimePacket(DeviceConfig deviceConfig, DateTime time)
        {
            var p = new Packet();
            p.realtime = true;
            
            p.DeviceKey = deviceConfig.DeviceKey;
            List<ValueItem> valueList = DataSource.GetInstance().GetValueList(deviceConfig, time);
            if (valueList == null)
            {
                return null;
            }
            foreach (ValueItem valueItem in valueList)
            {
                if (valueItem.Name.ToLower() == "time")
                {
                    p.time = DateTime.Parse(valueItem.Value);
                }
            }
            p.valueList = valueList;
            return p;
        }

        public bool SetSendStatus(DeviceConfig deviceConfig, DateTime time)
        {
            DateTime packetTime = this.time;
            DataSource.GetInstance().SetSendStatus(deviceConfig, packetTime);
            return true;
        }

        public string Path
        {
            get;
            set;
        }

        public string FileType { get; set; }
    }
}
