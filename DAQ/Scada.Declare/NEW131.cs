using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.IO;
using System.Diagnostics;
using System.Threading;
using Scada.Common;
using System.Reflection;
using System.Globalization;
using Scada.Config;
using System.Data.Sql;
using System.Data.SqlClient;

namespace Scada.Declare
{
    public class NEW131 : Device
	{
		private const int ComDataBits = 8;

        private DeviceEntry entry = null;

		private SerialPort serialPort = null;

		private int readTimeout = 12000;		//Receive timeout

		private int baudRate = 9600;

        private int dataBits = 8;

        private StopBits stopBits = StopBits.One;

        private Parity parity = Parity.None;

		private bool isVirtual = false;

        // retrieve gammalong command
		private byte[] actionSend1 = null;

        // retrieve emissionlong command
        private byte[] actionSend2 = null;
       

        private int actionInterval = 0;

		private string linePattern = string.Empty;

		private string insertIntoCommand = string.Empty;

		private FieldConfig[] fieldsConfig = null;

        private IMessageTimer senderTimer = null;

		private Timer timer = null;

		private bool handled = true;

        private string exampleLine;

		private List<byte> exampleBuffer = new List<byte>();

        // do not support virtual device
        private bool IsRealDevice = true;

		private string error = "No Error";

        // private static int MaxDelay = 10;

        private DateTime currentRecordTime = default(DateTime);

        private string temp;

        private string hv;

        private string doserate;

        private int factor;


        

        // Serial port sleep 200 ms as default before read
        private int bufferSleep = 200;

		public NEW131(DeviceEntry entry)
		{
            this.entry = entry;
			if (!this.Initialize(entry))
			{
				string initFailedEvent = string.Format("Device '{0}' initialized failed. Error is {1}.", entry[DeviceEntry.Identity], error);
				RecordManager.DoSystemEventRecord(this, initFailedEvent);
			}
		}

        ~NEW131()
        {
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

            this.actionSend1 = Encoding.ASCII.GetBytes((StringValue)entry["ActionSend1"] + "\r");
            this.actionSend2 = Encoding.ASCII.GetBytes((StringValue)entry["ActionSend2"] + "\r");

            string bufferSleepString = (StringValue)entry["BufferSleep"];
            if (bufferSleepString != null)
            {
                this.bufferSleep = int.Parse(bufferSleepString);
            }

			// this.actionDelay = (StringValue)entry[DeviceEntry.ActionDelay];

            const int DefaultRecordInterval = 30;
            this.actionInterval = this.GetValue(entry, DeviceEntry.ActionInterval, DefaultRecordInterval);
            this.RecordInterval = this.GetValue(entry, DeviceEntry.RecordInterval, DefaultRecordInterval);
            this.recordTimePolicy.Interval = this.RecordInterval;

            // 读取刻度因子
            this.factor = this.GetValue(entry, "Factor1", 1);

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

			return true;
		}

		private bool IsOpen
		{
			get
			{
                return this.IsRealDevice ? this.serialPort.IsOpen : true;
			}
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
                    else 
                    {
                        this.Write();
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

        private void StartSenderTimer(int interval)
        {
            // timer 每5s一次
            int minInterval = 5;
            if (MainApplication.TimerCreator != null)
            {
                this.senderTimer = MainApplication.TimerCreator.CreateTimer(minInterval);

                this.senderTimer.Start(() => 
                {
                    this.Write();
                });
            }
        }

        //////////////////////////////////////////////////////////////////////
        // Virtual-Device.
        private void StartVirtualDevice()
        {
            if (this.actionInterval > 0)
            {
                this.StartSenderTimer(this.actionInterval);
            }
            else
            { }

            return;
        }
        //////////////////////////////////////////////////////////////////////

		private byte[] ReadData()
		{

			if (this.IsRealDevice)
			{
                // important, sleep 400ms to wait all the data come to system buffer, Kaikai
                Thread.Sleep(this.bufferSleep);

                // 读每一行的字符串
                string strReadLine = this.serialPort.ReadLine();

                // 把剩余字节读完
                string strReadExisting = this.serialPort.ReadExisting();

				return Encoding.ASCII.GetBytes(strReadLine);
			}
			else // Virtual Device~!
			{
                if (this.actionInterval > 1)
                {
                    // 假设: 应答式的数据，都是完整的帧.
                    return this.GetExampleLine();
                }
                else
                {
                    return this.GetExampleLine();
                }
			}
		}

		private byte[] GetLineBytes(byte[] data)
		{
            int len = data.Length;

            if (data[len - 1] == (byte)0x0d)
            {
                data[len - 1] = 0;
                return data;
            }

            else { return null; }
		}

		private void SerialPortDataReceived(object sender, SerialDataReceivedEventArgs evt)  
		{
			Debug.Assert(this.DataReceived != null);
			try
			{
				handled = false;
				byte[] buffer = this.ReadData();

                if (this.OnReceiveData(buffer))
                {
                    // 存入数据库
                    this.RecordData(buffer);
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

        internal void RecordData(byte[] line)
        {
            // Defect: HPIC need check the right time here.
            // if ActionInterval == 0, the time trigger not depends send-time.
            DateTime rightTime = default(DateTime);
            if (!this.recordTimePolicy.NowAtRightTime(out rightTime) ||
                this.currentRecordTime == rightTime)
            {
                return;
            }

            DeviceData dd;
            if (!this.GetDeviceData(line, this.currentRecordTime, out dd))
            {
                dd = new DeviceData(this, null);
                dd.OriginData = DeviceData.ErrorFlag;
                this.SynchronizationContext.Post(this.DataReceived, dd);
                return;
            }      

            // Post to Main thread to record.
            dd.OriginData = Encoding.ASCII.GetString(line);
            this.SynchronizationContext.Post(this.DataReceived, dd);

            // 只有在存储完成之后，才能记录
            this.currentRecordTime = rightTime;
        }

        private void PostStartStatus()
        {
            DeviceData dd = new DeviceData(this, null);
            dd.OriginData = DeviceData.BeginFlag;
            this.SynchronizationContext.Post(this.DataReceived, dd);
        }

		protected bool GetDeviceData(byte[] line, DateTime time, out DeviceData dd)
		{
            dd = default(DeviceData);

            string[] data = new string[3];

            string strData = System.Text.Encoding.Default.GetString(line);
            if (strData.Contains("Data display terminated") == true) //若接收到的是停止命令的返回值，则放弃操作
            {
                // 这里只取值，不存储
                return false;
            }
            else  //解析131数据
            {
                string[] ss = strData.Split(new Char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                this.doserate = "0" + ss[4];
                this.hv = ss[5];
                this.temp = ss[7];
            }

            double dDoserate = 0.0;
            try
            {
                if (double.TryParse(this.doserate, out dDoserate))
                {
                    double f = 1.0;
                    if (this.factor > 0)
                    {
                        f = this.factor;
                    }

                    // uGy/h (*1000)==> nGy/h
                    this.doserate = (f * dDoserate * 10000).ToString("0.0");
                }
            }
            catch (Exception e1)
            {
                return false;
            }

            data[0] = this.doserate  ;
            data[1] = this.hv  ;
            data[2] = this.temp ;

            dd.Time = time;
            object[] fields = Device.GetFieldsData(data, time, this.fieldsConfig);
            dd = new DeviceData(this, fields);
            dd.InsertIntoCommand = this.insertIntoCommand;

			return true;
		}

		public override void Start(string address)
        {
            if (!this.Connect(address))
            {
                RecordManager.DoSystemEventRecord(this, "Connection Failure");
            }
        }

		public void Write()
		{
            if (this.serialPort == null || !this.IsOpen)
            {
                return;
            }

            try
            {
                // 发送停止命令
                if (this.IsRealDevice)
                {
                    this.serialPort.Write(this.actionSend1, 0, this.actionSend1.Length);
                }

                Thread.Sleep(500);

                // 发送获取数据命令
                if (this.IsRealDevice)
                {
                    this.serialPort.Write(this.actionSend2, 0, this.actionSend2.Length);
                }
             
                Thread .Sleep (500);
                
               /*


                #region Virtual-Device
                else
                {
                    this.OnSendDataToVirtualDevice(this.actionSend1);
                }
                #endregion
                */
            }
            catch (Exception e)
            {
                RecordManager.DoSystemEventRecord(this, "Write COM Data Error: " + e.Message, RecordType.Error);
            }

            
		}

        public override void Stop()
        {
            if (this.senderTimer != null)
            {
                this.senderTimer.Close();
            }

            if (this.serialPort != null && this.IsOpen)
            {
                this.serialPort.Close();
            }
        }

        public override void Send(byte[] action, DateTime time)
        {
        }

        public override bool OnReceiveData(byte[] line)
        {
            return true;
        }

#region virtual-device
        private void OnSendDataToVirtualDevice(byte[] action)
        {
            if (Bytes.Equals(action, this.actionSend1))
            {
                if (this.actionInterval > 0)
                {
					this.SerialPortDataReceived("virtual-device", null);
                }
                else
                {
                    this.timer = new Timer(new TimerCallback((object state) =>
                    {
						if (handled)
						{
							this.SerialPortDataReceived("virtual-device", null);
						}
					}), null, 2000, 5000);
                }
            }	
        }

        private byte[] GetExampleLine(int rand = 0)
        {
			return Encoding.ASCII.GetBytes(this.exampleLine);
        }
#endregion
	}
}
