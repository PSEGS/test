using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.ExternalAPI
{
    public class DocumentValidate
    {
        public int Serviceid { get; set; }
        public string Candidate_name { get; set; }
        public string Candidate_dob { get; set; }
        public string Candidate_doc_sr_no { get; set; }
    }

    public class Datum
    {
        public string token { get; set; }
        public string session { get; set; }
    }

    public class AuthenticationResponse
    {
        public int response { get; set; }
        public List<Datum> data { get; set; }
    }
    public class CandidateDetails
    {
        public int response { get; set; }
        public string sys_message { get; set; }
        public object data { get; set; }
    }
}
