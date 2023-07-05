using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Utility.ExtensionMethod
{
   public static class CommonMethod
    {
        public static string GetPublicIp(string serviceUrl = "https://ipinfo.io/ip")
        {
            return Convert.ToString(System.Net.IPAddress.Parse(new System.Net.WebClient().DownloadString(serviceUrl)));
        }
    }
}
