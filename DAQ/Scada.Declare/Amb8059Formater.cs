using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scada.Declare
{
    // 
    class Amb8059Formater : DataParser
    {
        public override byte[] GetLineBytes(byte[] data)
        {
            return new byte[0];
        }

        private static string ParseDouble(string n)
        {
            int sp = n.IndexOf(' ');
            return n.Substring(0, sp);
        }

        public override string[] Search(byte[] data, byte[] lastData)
        {
            string line = Encoding.ASCII.GetString(data);
            int pos = line.IndexOf("=");
            if (pos <= 0)
            {
                return new string[0];
            }

            if (line.IndexOf("TMP") > 0)
            {
                string numbers = line.Substring(pos + 1);
                numbers = numbers.Trim(new char[] { '*', '\n' });
                string[] a = numbers.Split(',');
                string temperature = a[0].Trim();
                string humidity = a[1].Trim(new char[] { '*', '\n' });
                if (humidity.IndexOf("*") > 0)
                {
                    humidity = humidity.Split('*')[0];
                }
                return new string[] { temperature, humidity.TrimEnd('*') };
            }
            else
            {
                if (line.IndexOf("uT") > 0)
                {
                    string numbers = line.Substring(pos + 1);
                    string[] a = numbers.Split(';');
                    return new string[] { ParseDouble(a[0]), ParseDouble(a[2]) };
                }
                else
                {
                    string numbers = line.Substring(pos + 1);
                    string[] a = numbers.Split('V');
                
                    return new string[] { a[0].Trim() };
                }

            }
        }
    }

    // 四频段
    class Amb8059ExtFormater : DataParser
    {
        public override byte[] GetLineBytes(byte[] data)
        {
            return new byte[0];
        }

        private static string ParseDouble(string n)
        {
            int sp = n.IndexOf(' ');
            return n.Substring(0, sp);
        }

        public override string[] Search(byte[] data, byte[] lastData)
        {
            string line = Encoding.ASCII.GetString(data);
            int pos = line.IndexOf("=");
            if (pos <= 0)
            {
                return new string[0];
            }

            if (line.IndexOf("TMP") > 0)
            {
                string numbers = line.Substring(pos + 1);
                numbers = numbers.Trim(new char[] { '*', '\n' });
                string[] a = numbers.Split(',');
                string temperature = a[0].Trim();
                string humidity = a[1].Trim(new char[] { '*', '\n' });
                if (humidity.IndexOf("*") > 0)
                {
                    humidity = humidity.Split('*')[0];
                }
                return new string[] { temperature, humidity.TrimEnd('*') };
            }
            else if (line.IndexOf("LFA") > 0)
            {
                int p1 = line.IndexOf("=");
                int p2 = line.IndexOf("V/m");
                string e = line.Substring(p1 + 1, p2 - p1 - 1);
                return new string[] { e.Trim() };

            }
            else
            {
                int p1 = line.IndexOf("=");
                int p2 = line.IndexOf("V/m");
                string v = line.Substring(p1 + 1, p2 - p1 - 1);
                return v.Split(';');
            }
        }
    }

}
