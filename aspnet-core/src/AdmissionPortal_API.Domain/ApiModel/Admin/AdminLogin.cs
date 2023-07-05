using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static AdmissionPortal_API.Domain.CommonEnam.UserTypeEnum;

namespace AdmissionPortal_API.Domain.ApiModel.Admin
{
   public class AdminLogin
    {
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
                            @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
                            @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
                            ErrorMessage = "Email is not valid")]
        public String Email { get; set; }
        public String UserPassword { get; set; }
        [JsonIgnore]
        public string Browser { get; set; }
        [JsonIgnore]
        public string IPAddress { get; set; }
    }
}

