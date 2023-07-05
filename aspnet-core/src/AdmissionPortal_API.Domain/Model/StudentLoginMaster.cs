using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.Model
{
    public class StudentLoginMaster
    {
        public int Student_ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Token { get; set; }
    }
    public class StudentRegisterResponseModel
    {
        public int ResponseCode { get; set; }
        public string SysMsg { get; set; } 
    }
}
