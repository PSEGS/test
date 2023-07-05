using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.ExternalAPI
{
    public class PayGovResponseModel
    {
        public int response { get; set; }
        public string sys_message { get; set; }
        public object data { get; set; }
    }
    public class paymenttestparameter
    {
        public string StudentRegistrationID { get; set; }
        public string AMount { get; set; }
        public string coursetype { get; set; }
        public string mothername { get; set; }
        public string fathername { get; set; }

    }

}
