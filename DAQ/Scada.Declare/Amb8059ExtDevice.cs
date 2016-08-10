using Scada.Common;
using Scada.Config;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;

namespace Scada.Declare
{
    class AmbExtData
    {
        public string Magneticfield;

        public string Electricfield;

        public string Temperature;

        public string Humidity;

        public string Broadband;

        public string EGSM900;

        public string EGSM1800;

        public string UMTS;

        public AmbExtData(Device device)
        {
            // this.Magneticfield = string.Empty;
            this.Electricfield = string.Empty;
            this.Temperature = string.Empty;
            this.Humidity = string.Empty;
            this.Broadband = string.Empty;
            this.EGSM900 = string.Empty;
            this.EGSM1800 = string.Empty;
            this.UMTS = string.Empty;
        }

        public bool IsReady()
        {
            return Temperature.Length > 0 && Electricfield.Length > 0 && this.Broadband.Length > 0;
        }

        public string[] ToArray()
        {
            return new string[] { this.Electricfield, this.Temperature, this.Humidity, // this.Magneticfield 
                this.Broadband,
                this.EGSM900,
                this.EGSM1800,
                this.UMTS
            };
        }

        public override string ToString()
        {
            return string.Format("({0}:{1}:{2}:{3})", Electricfield, Temperature, Humidity, Magneticfield);
        }
    }

    /// <summary>
    /// 四波段
    /// </summary>
    public class Amb8059ExtDevice : Device
    {
        private const int ComDataBits = 8;

        private DeviceEntry entry = null;

        private string detector = string.Empty;

        private SerialPort serialPort;

        private int baudRate = 9600;

        private int dataBits = 8;

        private StopBits stopBits = StopBits.One;

        private Parity parity = Parity.None;

        private bool IsRealDevice = true;

        private bool handled = true;

        private int actionInterval;

        private string error = "No Error";

        private int readTimeout;

        private string actionCondition;

        private int bufferSleep;

        private byte[] actionSend1;

        private byte[] actionSend2;

        private byte[] actionSend3;

        private bool actionSendInHex = false;

        private DataParser dataParser;

        private string insertIntoCommand;

        private FieldConfig[] fieldsConfig;

        private string exampleLine1;

        private string exampleLine2;

        private string exampleLine3;

        private IMessageTimer senderTimer;

        private Timer timer;

        private byte[] currentActionSend;

        private DateTime currentRecordTime;

        private byte[] lastLine;

        private AmbExtData currentData = null;

        public Amb8059ExtDevice(DeviceEntry entry)
        {
            this.entry = entry;
            if (!this.Initialize(entry))
            {
                string initFailedEvent = string.Format("Device '{0}' initialized failed. Error is {1}.", entry[DeviceEntry.Identity], error);
                RecordManager.DoSystemEventRecord(this, initFailedEvent);
            }
        }

        private bool Initialize(DeviceEntry entry)
        {
            this.Name = entry[DeviceEntry.Name].ToString();
            this.Id = entry[DeviceEntry.Identity].ToString();
            this.DeviceConfigPath = entry[DeviceEntry.Path].ToString();
            this.Version = entry[DeviceEntry.Version].ToString();

            this.baudRate = this.GetValue(entry, DeviceEntry.BaudRate, 9600);
            this.readTimeout = this.GetValue(entry, DeviceEntry.ReadTimeout, 12000);
            this.dataBits = this.GetValue(entry, DeviceEntry.DataBits, ComDataBits);
            this.stopBits = (StopBits)this.GetValue(entry, DeviceEntry.StopBits, (int)StopBits.One);

            StringValue parity = (StringValue)entry[DeviceEntry.Parity];
            this.parity = SerialPorts.ParseParity(parity);


            // Virtual On
            string isVirtual = (StringValue)entry[DeviceEntry.Virtual];
            if (isVirtual != null && isVirtual.ToLower() == "true")
            {
                this.IsRealDevice = false;
            }

            string bufferSleepString = (StringValue)entry["BufferSleep"];
            if (bufferSleepString != null)
            {
                this.bufferSleep = int.Parse(bufferSleepString);
            }

            this.actionCondition = (StringValue)entry[DeviceEntry.ActionCondition];
            string actionSendInHex = (StringValue)entry[DeviceEntry.ActionSendInHex];
            if (actionSendInHex != "true")
            {
                string actionSend = (StringValue)entry[DeviceEntry.ActionSend];
                if (actionSend != null)
                {
                    actionSend = actionSend.Replace("\\r", "\r");
                    this.actionSend1 = Encoding.ASCII.GetBytes(actionSend);
                }

                string actionSend2 = (StringValue)entry["ActionSend2"];
                if (actionSend2 != null)
                {
                    actionSend2 = actionSend2.Replace("\\r", "\r");
                    this.actionSend2 = Encoding.ASCII.GetBytes(actionSend2);
                }

                string actionSend3 = (StringValue)entry["ActionSend3"];
                if (actionSend3 != null)
                {
                    actionSend3 = actionSend3.Replace("\\r", "\r");
                    this.actionSend3 = Encoding.ASCII.GetBytes(actionSend3);
                }
            }

            const int DefaultRecordInterval = 30;
            this.actionInterval = this.GetValue(entry, DeviceEntry.ActionInterval, DefaultRecordInterval);
            this.RecordInterval = this.GetValue(entry, DeviceEntry.RecordInterval, DefaultRecordInterval);
            this.recordTimePolicy.Interval = this.RecordInterval;

            // Set DataParser & factors
            string dataParserClz = (StringValue)entry[DeviceEntry.DataParser];
            this.dataParser = this.GetDataParser(dataParserClz);
            this.SetDataParserFactors(this.dataParser, entry);

            string tableName = (StringValue)entry[DeviceEntry.TableName];
            if (!string.IsNullOrEmpty(tableName))
            {
                string tableFields = (StringValue)entry[DeviceEntry.TableFields];

                string[] fields = tableFields.Split(',');
                string atList = string.Empty;
                for (int i = 0; i < fields.Length; ++i)
                {
                    string at = string.Format("@{0}, ", i + 1);
                    atList += at;
                }
                atList = atList.TrimEnd(',', ' ');

                // Insert into
                string cmd = string.Format("insert into {0}({1}) values({2})", tableName, tableFields, atList);
                this.insertIntoCommand = cmd;
            }

            string fieldsConfigStr = (StringValue)entry[DeviceEntry.FieldsConfig];
            List<FieldConfig> fieldConfigList = ParseDataFieldConfig(fieldsConfigStr);
            this.fieldsConfig = fieldConfigList.ToArray<FieldConfig>();

            if (!this.IsRealDevice)
            {
                string e1 = (StringValue)entry["VirtualData1"];
                e1 = e1.Replace("\\r", "\r");
                e1 = e1.Replace("\\n", "\n");

                this.exampleLine1 = e1;

                string e2 = (StringValue)entry["VirtualData2"];
                e2 = e2.Replace("\\r", "\r");
                e2 = e2.Replace("\\n", "\n");

                this.exampleLine2 = e2;

                string e3 = (StringValue)entry["VirtualData3"];
                e3 = e3.Replace("\\r", "\r");
                e3 = e3.Replace("\\n", "\n");

                this.exampleLine3 = e3;
            }

            return true;
        }


        public bool Connect(string portName)
        {
            try
            {
                this.serialPort = new SerialPort(portName);

                this.serialPort.BaudRate = this.baudRate;

                this.serialPort.Parity = this.parity;       //Parity none
                this.serialPort.StopBits = this.stopBits;    //(StopBits)this.stopBits;    //StopBits 1
                this.serialPort.DataBits = this.dataBits;               // this.dataBits;   // DataBits 8bit
                this.serialPort.ReadTimeout = 10000;        // this.readTimeout;

                this.serialPort.RtsEnable = true;
                this.serialPort.NewLine = "\r";	        //?
                this.serialPort.DataReceived += this.SerialPortDataReceived;
                

                // Real Devie begins here.
                if (this.IsRealDevice)
                {
                    this.serialPort.Open();

                    if (this.actionInterval > 0)
                    {
                        this.StartSenderTimer(this.actionInterval);
                    }

                    // Set status of starting.
                    PostStartStatus();
                }
                else
                {
                    RecordManager.DoSystemEventRecord(this, "Notice, Virtual Device Started");
                    this.StartVirtualDevice();
                }

            }
            catch (IOException e)
            {
                string message = "IO: " + e.Message;
                RecordManager.DoSystemEventRecord(this, message);
                return false;
            }
            catch (Exception e)
            {
                string message = "Other: " + e.Message;
                RecordManager.DoSystemEventRecord(this, message);
                return false;
            }

            return true;
        }

        private void StartSenderTimer(int actionInterval)
        {
            if (MainApplication.TimerCreator != null)
            {
                // const int MinInterval = 2;
                this.senderTimer = MainApplication.TimerCreator.CreateTimer(actionInterval);
                // Trigger every 2s.
                this.senderTimer.Start(() =>
                {
                    this.WriteAction(this.actionSend1);
                });

            }
        }

        private void PostStartStatus()
        {
            
        }

        private void StartVirtualDevice()
        {
            if (this.actionInterval > 0)
            {
                this.StartSenderTimer(this.actionInterval);
            }
            else if (this.actionInterval == 0)
            {
                this.OnSendDataToVirtualDevice(null);
            }
        }

        public override bool OnReceiveData(byte[] line)
        {

            return true;   
        }

        public override void Send(byte[] action, DateTime time)
        {
           

        }

        private void Write2ActionsToPort(byte[] action)
        {
            this.currentActionSend = action;
            this.serialPort.Write(action, 0, action.Length);
        }

        private void WriteAction(byte[] action)
        {
            if (this.IsRealDevice)
            {
                this.Write2ActionsToPort(action);
            }
            else
            {
                this.OnSendDataToVirtualDevice(action);
            }
        }

        private void OnSendDataToVirtualDevice(byte[] action)
        {
            if (this.actionInterval > 0)
            {
                this.currentActionSend = action;
                this.SerialPortDataReceived("virtual-device", null);
            }
        }

        public override void Start(string address)
        {
            if (!this.Connect(address))
            {
                RecordManager.DoSystemEventRecord(this, "Connection Failure");
            }
        }

        public override void Stop()
        {
        }

        private void SerialPortDataReceived(object sender, SerialDataReceivedEventArgs evt)
        {
            Debug.Assert(this.DataReceived != null);
            try
            {
                handled = false;
                byte[] buffer = this.ReadData();

                RecordManager.DoSystemEventRecord(this, "SerialPortDataReceived");

                byte[] line = this.GetLineBytes(buffer);
                if (line == null || line.Length == 0)
                {
                    return;
                }

                if (this.OnReceiveData(line))
                {
                    // 存入数据库
                    this.RecordData(line);
                }

            }
            catch (InvalidOperationException e)
            {
                RecordManager.DoSystemEventRecord(this, e.Message);
            }
            finally
            {
                handled = true;
            }
        }

        private byte[] GetLineBytes(byte[] buffer)
        {
            return buffer;
        }

        private void RecordData(byte[] line)
        {
            DeviceData dd;
            DateTime rightTime = DateTime.Now;
            if (!this.GetDeviceData(line, rightTime, out dd))
            {
                /*
                dd = new DeviceData(this, null);
                dd.OriginData = DeviceData.ErrorFlag;
                this.SynchronizationContext.Post(this.DataReceived, dd);
                 * */
                return;
            }

            // Post to Main thread to record.
            dd.OriginData = Encoding.ASCII.GetString(line);
            RecordManager.DoSystemEventRecord(this, dd.OriginData);

            this.SynchronizationContext.Post(this.DataReceived, dd);

            // 只有在存储完成之后，才能记录
            this.currentRecordTime = rightTime;
        }

        private bool GetDeviceData(byte[] line, DateTime time, out DeviceData dd)
        {
            dd = default(DeviceData);

            if (time == default(DateTime))
            {
                time = DateTime.Now;
            }
            string[] data = null;
            try
            {
                data = this.dataParser.Search(line, this.lastLine);
                this.lastLine = line;

                if (data == null || data.Length == 0)
                {
                    RecordManager.DoSystemEventRecord(this, string.Format("GetDeviceData() Error, Data={0}", Encoding.ASCII.GetString(line)));
                    return false;
                }

                // RecordManager.DoSystemEventRecord(this, string.Format("GetDeviceData() OK, Data=({0})", Encoding.ASCII.GetString(line)));

                if (this.currentActionSend == this.actionSend1)
                {
                    if (this.currentData == null)
                    {
                        this.currentData = new AmbExtData(this);
                        // RecordManager.DoSystemEventRecord(this, "new AmbData(111)");
                    }

                    if (this.currentData != null)
                    {
                        this.currentData.Temperature = data[0];
                        this.currentData.Humidity = data[1];
                    }

                    // 第二Action发送
                    this.currentActionSend = this.actionSend2;
                    this.WriteAction(this.actionSend2);

                }
                else if (this.currentActionSend == this.actionSend2)
                {
                    // 第二Action 接受
                    if (this.currentData == null)
                    {
                        this.currentData = new AmbExtData(this);
                        //RecordManager.DoSystemEventRecord(this, "new AmbData(222)");
                    }

                    if (this.currentData != null)
                    {
                        this.currentData.Electricfield = data[0];
                        if (data.Length > 1)
                        {
                            this.currentData.Magneticfield = data[1];
                        }
                    }
                    // 第3 Action发送
                    this.currentActionSend = this.actionSend3;
                    this.WriteAction(this.actionSend3);
                }
                else if (this.currentActionSend == this.actionSend3)
                {
                    // 第3 Action 接受
                    if (this.currentData == null)
                    {
                        this.currentData = new AmbExtData(this);
                        //RecordManager.DoSystemEventRecord(this, "new AmbData(222)");
                    }

                    if (this.currentData != null)
                    {
                        this.currentData.Broadband = data[0].Trim();
                        this.currentData.EGSM900 = data[1].Trim();
                        this.currentData.EGSM1800 = data[2].Trim();
                        this.currentData.UMTS = data[3].Trim();
                    }
                }

                if (this.currentData != null && this.currentData.IsReady())
                {
                    string[] dataArray = this.currentData.ToArray();
                    object[] fields = Device.GetFieldsData(dataArray, time, this.fieldsConfig);
                    dd = new DeviceData(this, fields);
                    dd.InsertIntoCommand = this.insertIntoCommand;
                    this.currentData = null;
                    // RecordManager.DoSystemEventRecord(this, string.Format("GetDeviceData() Ready!"));
                    return true;
                }
                else
                {
                    // RecordManager.DoSystemEventRecord(this, string.Format("GetDeviceData() NotReady{0}", this.currentData));
                    dd = default(DeviceData);
                    return false;
                }
            }
            catch (Exception e)
            {
                string strLine = Encoding.ASCII.GetString(line);
                string errorMsg = string.Format("GetDeviceData() Exception, Data={0}\n{1}\n{2}", strLine, e.Message, e.StackTrace);
                RecordManager.DoSystemEventRecord(this, errorMsg);

                return false;
            }
        }

        private byte[] ReadData()
        {
            if (this.IsRealDevice)
            {
                // important, sleep 400ms to wait all the data come to system buffer, Kaikai
                Thread.Sleep(500);

                int n = this.serialPort.BytesToRead;
                if (n == 0)
                {
                    return null;
                }
                else
                {
                    // byte[] buffer = new byte[n];
                    string line = string.Empty;
                    try
                    {
                        line = this.serialPort.ReadLine();
                        if (line == null || string.IsNullOrEmpty(line.Trim()))
                        {
                            RecordManager.DoSystemEventRecord(this, "SerialPort Read Error");
                            return null;
                        }
                    }
                    catch (Exception e)
                    {
                        RecordManager.DoSystemEventRecord(this, "SerialPort Read Exception" + e.Message);
                        return null;
                    }

                    return Encoding.ASCII.GetBytes(line);
                }
            }
            else // Virtual Device~!
            {
                string v = string.Empty;
                if (this.currentActionSend == this.actionSend1)
                {
                    // 假设: 应答式的数据，都是完整的帧.
                    v = this.exampleLine1;
                }
                else if (this.currentActionSend == this.actionSend2)
                {
                    // 假设: 应答式的数据，都是完整的帧.
                    v = this.exampleLine2;
                }
                else if (this.currentActionSend == this.actionSend3)
                {
                    // 假设: 应答式的数据，都是完整的帧.
                    v = this.exampleLine3;
                }
                return Encoding.ASCII.GetBytes(v);
            }

        }
    }
}
