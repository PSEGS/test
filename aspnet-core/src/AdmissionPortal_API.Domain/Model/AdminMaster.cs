using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.Model
{
    public class AdminMaster
    {
        public int EmployeeID { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsDeptAdmin { get; set; }
        public string Token { get; set; }
        public string ImageCode { get; set; }
        [IgnoreDataMember]
        public string ImageReference { get; set; }
        public string LoginType { get; set; }
        public string UniversityCollegeId { get; set; }
        public string User_Name { get; set; }
        public string Name { get; set; }
        public string HeadId { get; set; }
        public bool Is_First_Login { get; set; }
 

    }
    public class ChangePasswordResponse
    {
        public int STATUS { get; set; }
        public string Mobile { get; set; }
        public string email { get; set; }
        public string collegeName { get; set; }
    }
}

