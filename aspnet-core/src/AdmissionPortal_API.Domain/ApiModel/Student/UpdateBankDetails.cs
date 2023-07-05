using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.ApiModel.Student
{
    public class UpdateBankDetails
    {
        [JsonIgnore]
        public int? StudentId { get; set; }
        public string BankName { get; set; }
        public string BankAccountNumber { get; set; }
        public string IFSCCode { get; set; }
        public string BankBranch { get; set; }
        public string MICRCode { get; set; }
        public int? BankId { get; set; }

    }
}
