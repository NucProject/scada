using Newtonsoft.Json.Linq;
using Scada.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Scada.Data.Hub
{
    /// <summary>
    /// 
    /// </summary>
    public enum NotifyEvents
    {
        DebugMessage,
        EventMessage,
        UploadFileOK,
        UploadFileFailed,
        SendDataOK,
        SendDataFailed,
        BadCommand,
        HistoryData
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="agent"></param>
    /// <param name="notifyEvent"></param>
    /// <param name="p"></param>
    public delegate void OnNotifyEvent(DataAgent agent, NotifyEvents notifyEvent, UINotifyEvent p);

    /// <summary>
    /// 
    /// </summary>
    public class DataAgent
    {
        private const string Post = @"POST";

        private const int Timeout = 5000;

        public event OnNotifyEvent NotifyEvent;

        private WebClient commandClient;
        
        internal RemoteDataHub RemoteDataHub
        {
            get;
            set;
        }

        public DataAgent()
        {
            this.RemoteDataHub = new RemoteDataHub();
        }

        /// <summary>
        /// Upload Data Entry
        /// </summary>
        /// <param name="packet"></param>
        /// <param name="time"></param>
        internal bool SendDataPacket(string action, Packet packet, DateTime time)
        {
            if (string.IsNullOrEmpty(action))
            {
                action = this.RemoteDataHub.GetUrl("send/data");
            }
            
            return this.Send(action, packet, time);
        }

        /// <summary>
        /// Commands
        /// </summary>
        internal void FetchCommands()
        {
            // TODO: Maybe need a Lock?
            string stationId = "";
            Uri uri = new Uri(this.RemoteDataHub.GetUrl("command/query/" + stationId));
            try
            {
                if (this.commandClient == null)
                {
                    this.commandClient = new WebClient();

                    this.commandClient.DownloadStringCompleted += (object sender, DownloadStringCompletedEventArgs e) =>
                        {
                            UINotifyEvent notify = UINotifyEvent.CreateNotifyEvent(e.ToString());
                            if (e.Error == null)
                            {
                                this.NotifyEvent(this, NotifyEvents.EventMessage, notify);
                                this.ParseCommand(e.Result);
                            }
                            else
                            {
                                this.NotifyEvent(this, NotifyEvents.EventMessage, notify);
                            }

                        };
                }

                this.commandClient.DownloadStringAsync(uri);
            }
            catch (Exception)
            {
                this.commandClient.Dispose();
                this.commandClient = null;
            }
        }

        private void ParseCommand(string cmd)
        {
            try
            {
                JObject json = JObject.Parse(cmd);

                JToken command = json["results"];

                JToken type = command["type"];
                if (type == null)
                    return;

                if (type.Value<string>() == "history")
                {
                    string device = command["device"].Value<string>();

                    JToken content = command["content"];
                    if (content != null)
                    {
                        JToken times = content["times"];
                        string timesStr = null;
                        if (times != null)
                        {
                            timesStr = times.Value<string>();
                        }

                        JToken sid = content["sid"];
                        string sidStr = null;
                        if (sid != null)
                        {
                            sidStr = sid.Value<string>();
                        }

                        if (device == "hpge")
                        {
                            this.HandleHistoryData(device, sidStr);
                        }
                        else
                        {
                            string start = content["start"].Value<string>();
                            string end = content["end"].Value<string>();
                            this.HandleHistoryData(device, start, end, timesStr);
                        }
                    }

                    
                }

            }
            catch (Exception e)
            {
                //this.NotifyEvent(this, NotifyEvents.BadCommand, new NotifyEvent() { Message = e.Message });
            }
        }

        // 处理其余设备历史数据
        private void HandleHistoryData(string device, string start, string end, string times)
        {
            /*
            Notify n = new Notify();
            n.SetValue("device", device);
            n.SetValue("start", start);
            n.SetValue("end", end);
            n.SetValue("times", times);
            this.NotifyEvent(this, NotifyEvents.HistoryData, n);
            */
        }

        // 处理hpge历史数据
        private void HandleHistoryData(string device, string sid)
        {
            /*
            Notify n = new Notify();
            n.SetValue("device", device);
            n.SetValue("sid", sid);
            this.NotifyEvent(this, NotifyEvents.HistoryData, n);
            */
        }

        /// <summary>
        /// Send Dispatcher
        /// </summary>
        /// <param name="p"></param>
        internal bool SendPacket(Packet p)
        {
            return this.SendPacket(p, false);
        }

        private bool SendPacket(Packet p, bool fromNewThread)
        {
            if (!p.IsFilePacket)
            {
                return this.SendDataPacket("", p, default(DateTime));
            }
            else
            {
                return this.SendFilePacket(p);
            }
        }

        /// <summary>
        /// Upload Data Implements
        /// </summary>
        /// <param name="api"></param>
        /// <param name="packet"></param>
        /// <param name="time"></param>
        private bool Send(string api, Packet packet, DateTime time)
        {
            try
            {
                StreamWriter sw = new StreamWriter("s.txt", true);
                sw.Write(packet.time.ToString() + "\r\n");
                sw.Close();

                Uri uri = new Uri(api);
                byte[] data = Encoding.ASCII.GetBytes(packet.ToJson());
                using (WebClient wc = new WebClient())
                {
                    Byte[] result = wc.UploadData(uri, "POST", data);
                    string strResult = Encoding.ASCII.GetString(result);

                    return true;
                }
            }
            catch (Exception e)
            {
                this.HandleWebException(e);
                StreamWriter sw = new StreamWriter("e.txt", true);
                sw.Write(e.StackTrace + "\r\n");
                sw.Close();
                return false;
            }
        }

        string GetPacketSID(Packet p)
        {
            string tmp = p.Path;
            int endIndex = tmp.LastIndexOf("\\");
            tmp = tmp.Substring(0, endIndex);
            int startIndex = tmp.LastIndexOf("\\");
            return tmp.Substring(startIndex + 1, tmp.Length - startIndex - 1);
        }

        void RemoveOccupiedToken(Packet p)
        {
            string fileNameWithToken = p.Path;

            if (File.Exists(fileNameWithToken))
            {
                string fileName = Path.GetFileName(fileNameWithToken);
                string path = Path.GetDirectoryName(fileNameWithToken);
                fileName = fileName.Substring(1);
                string newFileName = Path.Combine(path, fileName);
                File.Move(fileNameWithToken, newFileName);
            }
        }

        /// <summary>
        /// Upload File
        /// </summary>
        /// <param name="packet"></param>
        internal bool SendFilePacket(Packet packet)
        {
            if (string.IsNullOrEmpty(packet.Path) || !File.Exists(packet.Path))
            {
                // Notify msg = new Notify();
                // msg.Message = "No File Found";
                // this.NotifyEvent(this, NotifyEvents.EventMessage, msg);

                // 上传失败，移除占用标志
                // RemoveOccupiedToken(packet);

                return false;
            }

            string uploadUrl = string.Empty;

            // 判断是哪种设备的文件上传
            if (packet.FileType.Equals("labr", StringComparison.OrdinalIgnoreCase))
            {
                string path = Path.GetDirectoryName(packet.Path);
                var folder1 = Path.GetFileName(Path.GetDirectoryName(path));
                var folder2 = Path.GetFileName(path);
                uploadUrl = this.GetUploadApi(packet.FileType, folder1, folder2);
            }

            Uri uri = new Uri(this.RemoteDataHub.GetUrl(uploadUrl));
            try
            {
                using (WebClient wc = new WebClient())
                {
                    // 同步上传
                    Byte[] result = wc.UploadFile(uri, Post, packet.Path);
                    string strResult = Encoding.UTF8.GetString(result);
                    string msg = string.Format("成功上传 {0}，信息 {1}", this.GetRelFilePath(packet), strResult);
                    //this.NotifyEvent(this, NotifyEvents.UploadFileOK, new NotifyEvent() { Message = msg });

                    return true;
                }
            }
            catch (WebException e)
            {
                // 上传失败，移除占用标志
                RemoveOccupiedToken(packet);

                string msg = string.Format("错误上传 {0}，错误信息 {1}", this.GetRelFilePath(packet), e.Message);
                //this.NotifyEvent(this, NotifyEvents.UploadFileFailed, new NotifyEvent() { Message = msg });

                return false;
            }
        }

        private string GetRelFilePath(Packet packet)
        {
            if (packet.FileType.Equals("labr", StringComparison.OrdinalIgnoreCase))
            {
                string fileName = Path.GetFileName(packet.Path);

                return string.Format("{0}", fileName);
            }
            else if (packet.FileType.Equals("hpge", StringComparison.OrdinalIgnoreCase))
            {
                string fileName = Path.GetFileName(packet.Path);
                string path = Path.GetDirectoryName(packet.Path);
                var sidFolder = Path.GetFileName(path);
                if (fileName.StartsWith("!"))
                {
                    return string.Format("{0}\\{1}", sidFolder, fileName.Remove(0, 1));
                }
                return string.Format("{0}\\{1}", sidFolder, fileName);
            }
            return string.Empty;
        }

        private string ExtractDate(string date)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char i in date.Trim())
            {
                if (i >= '0' && i <= '9')
                {
                    sb.Append(i);
                }
                else if (i == '/' || i == '-' || i == ' ' || i == ':')
                {
                    sb.Append(i);
                }
            }
            return sb.ToString();
        }

        // 处理HPGe文件参数
    
        private void RemovePrefix(string p)
        {
            if (File.Exists(p))
            {
                string fileName = Path.GetFileName(p);
                if (fileName.StartsWith("!"))
                {
                    string dirName = Path.GetDirectoryName(p);
                    int t = 0;
                    while (t < 5)
                    {
                        try
                        {
                            File.Move(p, Path.Combine(dirName, fileName.Substring(1)));
                            break;
                        }
                        catch (IOException)
                        {
                            t++;
                            Thread.Sleep(1000);
                        }
                    }
                }
            }
        }

        private string GetUploadApi(string fileType, string folder)
        {
            string stationId = HubConfig.StationId;

            return string.Format("data/upload/{0}/{1}/{2}", stationId, fileType, folder);
        }

        private string GetUploadApi(string fileType, string folder1, string folder2)
        {
            string stationId = "";  // Settings.Instance.Station;

            return string.Format("data/upload/{0}/{1}/{2}/{3}", stationId, fileType, folder1, folder2);
        }

        internal void DoCheckTodayData(DateTime n, string deviceType, int expect)
        {
            string stationId = "";  // Settings.Instance.Station;

            string api = string.Format("data/check/{0}/{1}", stationId, deviceType);
            
            Uri uri = new Uri(this.RemoteDataHub.GetUrl(api));

            DateTime t = new DateTime(n.Year, n.Month, n.Day);
            DateTime e = t.AddDays(1);
            using (WebClient wc = new WebClient())
            {
                wc.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                string postData = string.Format("start={0}&end={1}&expect={2}&set=1", t.ToString(), e.ToString(), expect);
                Byte[] r = wc.UploadData(uri, "POST", Encoding.UTF8.GetBytes(postData));

                string result = Encoding.UTF8.GetString(r);

                
            }

        }

        private void HandleWebException(Exception e)
        {
            WebException we = e as WebException;

            if (we == null)
            {
                return;
            }

            HttpWebResponse hwr = we.Response as HttpWebResponse;
            if (hwr != null)
            {
                switch (hwr.StatusCode)
                {
                    case HttpStatusCode.InternalServerError:
                        break;
                    default:
                        break;
                }
            }
            else
            {
                // TODO: No response!
            }
        }
      
        // Connect means first HTTP packet to the data Center.
        internal void DoAuth()
        {
        }
    }
}
