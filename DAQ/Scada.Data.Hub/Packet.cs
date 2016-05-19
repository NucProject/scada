using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace Scada.Data.Hub
{
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

        // Content
        private JObject jobject = new JObject();

        private int result = 0;

        private bool hasResult = false;

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

        public Packet(string token)
        {
            this.Station = HubConfig.StationId;
            this.Token = token;
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

        /// <summary>
        /// Interface
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public string GetProperty(string propertyName)
        {
            return Packet.GetProperty(propertyName, this.jobject);
        }

        /// <summary>
        /// GetProperty implements
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="jsonObject"></param>
        /// <returns></returns>
        private static string GetProperty(string propertyName, JObject jsonObject)
        {
            JToken s = jsonObject[propertyName];
            if (s != null)
            {
                return s.ToString();
            }
            return string.Empty;
        }

        public string Station
        {
            get
            {
                return this.GetProperty(StationKey);
            }
            set
            {
                this.jobject[StationKey] = value;
            }
        }

        public string Token
        {
            get
            {
                return this.GetProperty(TokenKey);
            }
            set
            {
                this.jobject[TokenKey] = value;
            }
        }

        public void setRealtime()
        {
            this.jobject["realtime"] = 1;
        }

        public void setHistory()
        {
            this.jobject["history"] = 1;
        }

        public override string ToString()
        {
            if (this.hasResult)
            {
                this.jobject["result"] = this.Result;
            }
            return this.jobject.ToString();
        }

        private JArray GetEntries()
        {
            JArray entries = (JArray)this.jobject[EntryKey];
            if (entries == null)
            {
                entries = new JArray();
                this.jobject[EntryKey] = entries;
            }
            return (JArray)entries;
        }

        internal JObject GetEntry(int index = 0)
        {
            return (JObject)this.GetEntries()[index];
        }

        internal void AppendEntry(JObject entry)
        {
            this.GetEntries().Add(entry);
        }

        private JObject BuildObject(string deviceId, Dictionary<string, object> data, bool realtime = true)
        {
            JObject json = new JObject();
            if (realtime)
            {
                json["type"] = "realtime";
                json["time"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {
                json["type"] = "history";
                json["time"] = "";
            }

            
            json["Device_id"] = deviceId;

            JArray values = new JArray();
            
            json["values"] = values;
            return json;
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
        

        public string Path
        {
            get;
            set;
        }

        public string FileType { get; set; }
    }
}
