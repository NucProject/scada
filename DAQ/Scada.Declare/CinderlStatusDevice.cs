﻿using Scada.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Scada.Declare
{
    public class CinderlStatusDevice : StandardDevice
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        public CinderlStatusDevice(DeviceEntry entry)
            :base(entry)
        {

        }

        public override void OnReceiveData(byte[] line)
        {
        }
    }
}
