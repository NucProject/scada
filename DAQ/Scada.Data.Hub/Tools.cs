using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scada.Data.Hub
{
    class Tools
    {
        public static bool WriteFileLine(string fileName, string line)
        {
            try
            {
                return true;
            }
            catch (FieldAccessException e)
            {
                return false;
            }
            finally
            {
                
            }
            
        }
    }
}
