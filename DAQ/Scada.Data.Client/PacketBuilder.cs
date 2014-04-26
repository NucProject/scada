﻿using Newtonsoft.Json.Linq;
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

        internal List<Packet> GetPackets(string deviceKey, DateTime time, string content)
        {
            throw new NotImplementedException();
        }

        internal Packet GetPacket(string deviceKey, Dictionary<string, object> data, bool p)
        {
            Packet packet = new Packet(this.Token);
            packet.AddData(deviceKey, data);
            return packet;
        }

        internal Packet GetReplyCommandPacket()
        {
            Packet packet = new Packet(this.Token);
            return packet;
        }

        private string RenameFileNameForUpload(string fileName)
        {
            string basePath = Path.GetDirectoryName(fileName);
            int fus = fileName.IndexOf('_');
            string partFileName = fileName.Substring(fus + 1);
            string filePath = string.Format("{0}\\S{1}_{2}", basePath, Settings.Instance.Station, partFileName);

            File.Move(fileName, filePath);
            return filePath;
        }

        internal Packet GetFilePacket(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
            {
                Packet packet = new Packet(this.Token);
                packet.Path = RenameFileNameForUpload(fileName);
                packet.IsFilePacket = true;
                return packet;
            }
            return null;
        }

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
