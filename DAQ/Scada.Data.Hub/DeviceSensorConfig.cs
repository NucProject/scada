﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Scada.Data.Hub
{
    public class SensorConfig
    {
        public string SensorName{ get; set; }

        public string DisplayName { get; set; }

        public string FieldName { get; set; }
    }

    public class DeviceConfig
    {
        private string version = null;

        public string Name { get; set; }

        public string DisplayName { get; set; }

        private string deviceConfigFile = null;

        private List<SensorConfig> sensorConfigList = new List<SensorConfig>();

        public string TableName { get; set; }

        public bool AtAnyTime { get; internal set; }

        public DeviceConfig(string deviceConfigFile)
        {
            this.deviceConfigFile = deviceConfigFile;
            this.AtAnyTime = false;
        }

        public static DeviceConfig LoadConfigFrom(string deviceConfigFile)
        {
            DeviceConfig deviceConfig = new DeviceConfig(deviceConfigFile);
            deviceConfig.LoadSensorConfig();
            return deviceConfig;
        }

        public List<SensorConfig> GetSensorConfigList()
        {
            return this.sensorConfigList;
        }

        internal void LoadSensorConfig()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(this.deviceConfigFile);
            var deviceNodes = doc.SelectNodes("//device");
            XmlNode deviceNode = deviceNodes[0];
            foreach (XmlNode node in deviceNode.ChildNodes)
            {
                string tagName = node.Name.ToLower();
                if (tagName == "version")
                {
                    this.version = node.InnerText;
                }
                else if (tagName == "name")
                {
                    this.Name = node.InnerText;
                }
                else if (tagName == "displayname")
                {
                    this.DisplayName = node.InnerText;
                }
                else if (tagName == "sensors")
                {
                    var sensorNodes = node.SelectNodes("//sensor");
                    foreach (XmlNode sensor in sensorNodes)
                    {
                        SensorConfig sensorConfig = new SensorConfig();
                        sensorConfig.SensorName = sensor.SelectSingleNode("name").InnerText;
                        sensorConfig.DisplayName = sensor.SelectSingleNode("displayname").InnerText;
                        this.sensorConfigList.Add(sensorConfig);
                    }
                }
            }

        }
    }

    public class HubConfig
    {
        public static string StationId = "";

        private string configPath = null; 

        private List<DeviceConfig> deviceConfigList = new List<DeviceConfig>();

        internal HubConfig(string configPath)
        {
            this.configPath = configPath;
        }

        public List<DeviceConfig> GetAllDeviceConfig()
        {
            return this.deviceConfigList;
        }

        public static HubConfig LoadConfigFromPath(string configPath)
        {
            HubConfig config = new HubConfig(configPath);
            if (config.LoadDeviceConfig())
            {
                // No device?
            }
            return config;
        }

        internal bool LoadDeviceConfig()
        {
            string[] deviceConfigFiles = Directory.GetFiles(this.configPath);
            foreach (var deviceConfigFile in deviceConfigFiles)
            {
                string fileName = Path.GetFileName(deviceConfigFile);
                if (!fileName.StartsWith("!") && fileName.EndsWith("xml"))
                {
                    DeviceConfig deviceConfig = DeviceConfig.LoadConfigFrom(deviceConfigFile);
                    this.deviceConfigList.Add(deviceConfig);
                }
            }
            return true;
        }
    }
}
