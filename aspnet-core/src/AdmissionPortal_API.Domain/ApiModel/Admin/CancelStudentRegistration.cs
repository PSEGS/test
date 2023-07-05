using System;

namespace AdmissionPortal_API.Domain.ApiModel.Admin
{
    public class CancelStudentRegistration
    {
        public string RegId { get; set; }

        public string RegType { get; set; }

        public string Remarks { get; set; }

        public Int32 UserId { get; set; }
    }
}
