using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Scada.Data.Hub
{
    class WebClientEx : WebClient
    {
        public int Timeout { get; set; }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var request = base.GetWebRequest(address);
            request.Timeout = Timeout;
            return request;
        }

        public string PostData(string api, string data)
        {
            Uri uri = new Uri(api);
            byte[] bytes = Encoding.ASCII.GetBytes(data);
            Byte[] result = this.UploadData(uri, "POST", bytes);
            string results = Encoding.ASCII.GetString(result);

            return results;
        }
    }
}
