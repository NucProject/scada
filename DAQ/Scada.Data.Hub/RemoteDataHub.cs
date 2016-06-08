using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada.Data.Hub
{
    public class RemoteDataHub
    {
        internal string GetUrl(string v)
        {
            return "http://127.0.0.1:9090/" + v;
        }
    }
}
