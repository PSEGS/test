using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.Model
{
    public class CitizenLoginMaster
    {
        public long CitizenID { get; set; }
        public String Email { get; set; }
        public String UserPassword { get; set; }
        public string Token { get; set; }
        public string ImageCode { get; set; }
        [IgnoreDataMember]
        public string ImageReference { get; set; }
    }
}

