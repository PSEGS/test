using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.ApiModel.Admin
{
    public class ViewStudentObjections
    {
        public string RegId { get; set; }

        public string admission { get; set; }

        public string college { get; set; }
    }
    public class StudentObjectionList
    {
        public string ObjectionNo  { get; set; }

       
        public string RegistrationNumber { get; set; }
        public string CollegeName { get; set; }
        public string VerificationDate { get; set; }
        public string VerifierRemarks { get; set; }
        public string RollNo { get; set; }
    }
}
