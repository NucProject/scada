using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada.Data.Hub
{
    public class DBConnectionString
    {
        public string Username
        {
            set;
            get;
        }

        public string Password
        {
            set;
            get;
        }

        public string Database
        {
            set;
            get;
        }

        public string Address
        {
            get;
            set;
        }


        public DBConnectionString(string username, string password, string database)
        {
            this.Username = username;
            this.Password = password;
            this.Database = database;

            this.Address = "127.0.0.1"; // As default
        }

        public DBConnectionString()
            : this("root", "root", "scada")
        {
        }

        public override string ToString()
        {
            return string.Format("datasource={0};username={1};password={2};database={3}", this.Address, this.Username, this.Password, this.Database);
        }

    }

    class DataSource
    {
        private static DataSource instance = new DataSource();

        private MySqlConnection conn = null;


        private DataSource()
        {
            this.GetDBConnection();
        }

        internal MySqlConnection GetDBConnection()
        {
            string connectionString = new DBConnectionString().ToString();
            this.conn = new MySqlConnection(connectionString);

            try
            {
                conn.Open();
                return conn;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static DataSource GetInstance()
        {
            return instance;
        }

        public List<ValueItem> GetValueList(DeviceConfig deviceConfig, DateTime time)
        {
            if (deviceConfig.TimeToSend == DeviceConfig.Anytime)
            {
                return this.GetLatestValueList(deviceConfig, time);
            }
            else
            {
                return this.GetTimedValueList(deviceConfig, time);
            }
        }

        private List<ValueItem> GetLatestValueList(DeviceConfig deviceConfig, DateTime time)
        {
            List<ValueItem> ret = new List<ValueItem>();

            MySqlCommand cmd = this.conn.CreateCommand();
            if (cmd != null)
            {
                if (this.FetchLatestData(cmd, deviceConfig, time, ret))
                {
                    cmd.Dispose();
                    return ret;
                }
                else
                {
                    cmd.Dispose();
                    return null;
                }
            }
            return null;
        }


        private List<ValueItem> GetTimedValueList(DeviceConfig deviceConfig, DateTime time)
        {
            List<ValueItem> ret = new List<ValueItem>();
            MySqlCommand cmd = this.conn.CreateCommand();
            if (cmd != null)
            {
                if (this.FetchTimeData(cmd, deviceConfig, time, ret))
                {
                    cmd.Dispose();
                    return ret;
                }
                else
                {
                    cmd.Dispose();
                    return null;
                }
            }
            return ret;
        }

        private bool FetchLatestData(MySqlCommand cmd, DeviceConfig deviceConfig, DateTime time, List<ValueItem> ret)
        {
            cmd.CommandText = GetNotSendSQL(deviceConfig, time);
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader == null)
            {
                return false;
            }
            if (reader.Read())
            {
                DateTime dataTime = reader.GetDateTime("Time");
                ret.Add(new ValueItem() { Name = "Time", Value = dataTime.ToString("yyyy-MM-dd HH:mm:ss") });
                deviceConfig.lastTime = dataTime;

                foreach (SensorConfig sc in deviceConfig.GetSensorConfigList())
                {
                    string v = reader.GetString(sc.FieldName);
                    ret.Add(new ValueItem() { Name = sc.FieldName, Value = v });
                }
                reader.Close();
                return true;
            }
            reader.Close();
            return false;
        }

        private bool FetchTimeData(MySqlCommand cmd, DeviceConfig deviceConfig, DateTime time, List<ValueItem> ret)
        {
            cmd.CommandText = GetTimeDataSQL(deviceConfig, time);
            MySqlDataReader reader = cmd.ExecuteReader();
            if (reader == null)
            {
                return false;
            }
            if (reader.Read())
            {
                DateTime dataTime = reader.GetDateTime("Time");
                ret.Add(new ValueItem() { Name = "Time", Value = dataTime.ToString("yyyy-MM-dd HH:mm:ss") });
                deviceConfig.lastTime = dataTime;

                foreach (SensorConfig sc in deviceConfig.GetSensorConfigList())
                {
                    string v = reader.GetString(sc.FieldName);
                    ret.Add(new ValueItem() { Name = sc.FieldName, Value = v });
                }
                reader.Close();
                return true;
            }
            reader.Close();
            return false;
        }

        private static string GetNotSendSQL(DeviceConfig deviceConfig, DateTime time)
        {
            // Get the recent <count> entries.
            string format = "select * from {0} where time>'{1}' and send=0 order by time";
            string lastTimeStr = string.Format("{0:yyyy-MM-dd HH:mm:ss}", deviceConfig.lastTime);
            return string.Format(format, deviceConfig.TableName, lastTimeStr);
        }

        private static string GetTimeDataSQL(DeviceConfig deviceConfig, DateTime time)
        {
            string format = "select * from {0} where time='{1}' and send=0";
            string lastTimeStr = string.Format("{0:yyyy-MM-dd HH:mm:ss}", deviceConfig.lastTime);
            return string.Format(format, deviceConfig.TableName, lastTimeStr);
        }

        internal void SetSendStatus(DeviceConfig deviceConfig, DateTime time)
        {
            string format = "update {0} set send=1 where time='{1:yyyy-MM-dd HH:mm:ss}'";
            string sql = string.Format(format, deviceConfig.TableName, time);
            try
            {
                MySqlCommand cmd = this.conn.CreateCommand();
                cmd.CommandText = sql;
                int rows = cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (Exception e)
            {

            }
            
        }
    }
}
