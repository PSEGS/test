using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.ApiModel.StudentPG
{
    public class StudentPGLogin
    {
        [Required]
        public string UserName { get; set; }
        public string OTP { get; set; }
        public string UserPassword { get; set; }
        [JsonIgnore]
        public string Browser { get; set; }
        [JsonIgnore]
        public string IPAddress { get; set; }
    }
}
