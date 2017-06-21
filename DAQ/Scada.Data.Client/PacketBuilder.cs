using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Scada.Data.Client
{
    public class PacketBuilder
    {
        internal string Token
        {
            get;
            set;
        }

        internal PacketBuilder()
        {
            this.Token = "";
        }

        public PacketBase GetPacket(string deviceKey, List<Dictionary<string, object>> list, bool p)
        {
            if (Settings.Instance.DataFormatVersion() == 2)
            {
                string deviceSn = Settings.Instance.GetDeviceSn(deviceKey);
                PacketV2 packet = this.GetPacketV2(deviceSn, list);
                return packet;
            }
            if (Settings.Instance.DataFormatVersion() == 3)
            {
                string deviceId = Settings.Instance.GetDeviceId(deviceKey);
                FormPacket packet = this.GetFormPacket(deviceId, list);
                return packet;
            }
            else
            {
                Packet packet = null;
                foreach (Dictionary<string, object> data in list)
                {   
                    packet = this.CombinePacket(packet, this.GetPacketV1(deviceKey, data, p));   
                }
                return packet;
            }
        }

        private FormPacket GetFormPacket(string deviceId, List<Dictionary<string, object>> list)
        {
            FormPacket p = new FormPacket(deviceId);
            p.SetData(list);
            return p;
        }


        // V1
        internal Packet GetPacketV1(string deviceKey, Dictionary<string, object> data, bool p)
        {
            Packet packet = new Packet(this.Token);
            packet.AddData(deviceKey, data);
            return packet;
        }

        // V2
        private PacketV2 GetPacketV2(string deviceSn, List<Dictionary<string, object>> list)
        {
            PacketV2 packet = new PacketV2(deviceSn);
            packet.SetData(list[0]);
            return packet;
        }

        internal Packet GetReplyCommandPacket()
        {
            Packet packet = new Packet(this.Token);
            return packet;
        }

        /*
        private string RenameFileNameForUpload(string fileName)
        {
            string basePath = Path.GetDirectoryName(fileName);
            int fus = fileName.IndexOf('_');
            string partFileName = fileName.Substring(fus + 1);
            string filePath = string.Format("{0}\\S{1}_{2}", basePath, Settings.Instance.Station, partFileName);

            File.Move(fileName, filePath);
            return filePath;
        }
        */

        internal Packet GetFilePacket(string fileName, string fileType)
        {
            if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
            {
                // package占用文件，同时改变其文件名
                int index = fileName.LastIndexOf("\\");
                string fileNameWithToken = fileName.Insert(index + 1, "!");
                if (!File.Exists(fileNameWithToken))
                {
                    File.Move(fileName, fileNameWithToken);
                }

                Packet packet = new Packet(this.Token);
                packet.Path = fileNameWithToken;
                packet.FileType = fileType;
                packet.IsFilePacket = true;
                return packet;
            }
            else { return null; }            
        }

        /// <summary>
        /// 只适用于以前的协议，以前的协议支持合并多个Packet
        /// </summary>
        /// <param name="packet1"></param>
        /// <param name="packet2"></param>
        /// <returns></returns>
        internal Packet CombinePacket(Packet packet1, Packet packet2)
        {
            if (packet1 != null)
            {
                packet1.AppendEntry(packet2.GetEntry());
                return packet1;
            }
            return packet2;
        }

        internal List<Packet> CombinePackets(List<Packet> packets)
        {
            if (packets.Count > 1)
            {
                List<Packet> ret = new List<Packet>();
                Packet pn = null;
                foreach (var p in packets)
                {
                    if (string.IsNullOrEmpty(p.Path))
                    {
                        // 目前认为Path为空的Packet是Data Packet
                        pn = this.CombinePacket(pn, p);
                    }
                    else
                    {
                        ret.Add(p); // p is file packet
                    }
                }
                if (pn != null)
                {
                    ret.Add(pn);
                }
                return ret;
            }
            return packets;
        }

    }
}
