using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada.Data.Hub
{
    public class Packet
    {
        virtual public bool SetSendStatus(DeviceConfig deviceConfig, DateTime time)
        {
            throw new NotImplementedException();
        }
    }
}
