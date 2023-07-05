using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.Model
{
    public class AdminLoginMaster
    {
        public String Email { get; set; }
        public String UserPassword { get; set; }     
        public string Browser { get; set; }        
        public string IPAddress { get; set; }
    }
}

