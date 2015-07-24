using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Scada.Config
{
    public class DeviceConfig
    {
        public DeviceConfig(string deviceId, string deviceName, string deviceType)
        {
            this.deviceId = deviceId;
            this.deviceName = deviceName;
            this.deviceType = deviceType;
        }

        string deviceId;
        string deviceName;
        string deviceType;

        public string DeviceName
        {
            get
            {
                return this.deviceName;
            }
            private set { }
        }
    }

    public class AllDevices
    {
        static bool loaded = false;

        static Dictionary<string, DeviceConfig> devDict = new Dictionary<string, DeviceConfig>();

        public static string[] GetIdArray()
        {
            if (!loaded)
            {
                _LoadDeicesFromXml();
            }

            return devDict.Keys.ToArray<string>();
        }

        private static string GetAttribute(XmlNode node, string attr)
        {
            var xmlAttr = node.Attributes.GetNamedItem(attr);
            return xmlAttr.Value; 
        }

        private static void _LoadDeicesFromXml()
        {
            string fileName = ConfigPath.GetConfigFilePath("all-devices.xml");
            if (File.Exists(fileName))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(fileName);

                var devices = doc.SelectNodes("//device");
                foreach (XmlNode d in devices)
                {
                    string deviceId = GetAttribute(d, "id");
                    string deviceName = GetAttribute(d, "name");
                    string deviceType = GetAttribute(d, "type");

                    devDict.Add(deviceId, new DeviceConfig(deviceId, deviceName, deviceType));
                }
            }
        }

        public static string GetDeviceIdByName(string deviceName)
        {
            foreach (var i in devDict)
            {
                if (i.Value.DeviceName == deviceName)
                {
                    return i.Key;
                }
            }
            return null;
        }

        public static string GetDeviceNameById(string deviceId)
        {
            foreach (var id in devDict.Keys)
            {
                if (id == deviceId)
                {
                    return devDict[id].DeviceName;
                }
            }
            return null;
        }
    }
}
