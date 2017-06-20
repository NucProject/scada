using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scada.Data.Client
{
    public class PacketBase
    {
        public string DeviceKey
        {
            get;
            set;
        }

        public string Id
        {
            get;
            set;
        }

        public bool IsFilePacket
        {
            get;
            set;
        }

        public bool IsFormData
        {
            get;
            set;
        }

        public string Path
        {
            get;
            set;
        }

        public string FileType
        {
            get;
            set;
        }

        public virtual void SetHistory()
        {
        }
    }

    public class Notify
    {
        /// <summary>
        /// Guid or ...
        /// </summary>
        public Dictionary<string, string> Payload
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

        public void SetValue(string key, string value)
        {
            if (this.Payload == null)
            {
                this.Payload = new Dictionary<string, string>();
            }
            this.Payload.Add(key, value);
        }

        public string GetValue(string key)
        {
            if (this.Payload == null)
            {
                return null;
            }
            if (this.Payload.ContainsKey(key))
            {
                return this.Payload[key];
            }
            else
            {
                return null;
            }
        }

    }
}
