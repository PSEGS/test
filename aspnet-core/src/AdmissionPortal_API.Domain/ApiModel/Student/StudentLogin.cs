using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.ApiModel.Student
{
    public class StudentLogin
    {
        [Required]
        public string UserName { get; set; }
        public string OTP { get; set; } = null;
        public string UserPassword { get; set; } = null;
        [JsonIgnore]
        public string Browser { get; set; }
        [JsonIgnore]
        public string IPAddress { get; set; }
    }
  
}
