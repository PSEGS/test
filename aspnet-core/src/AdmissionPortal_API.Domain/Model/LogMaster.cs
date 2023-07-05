using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.Model
{
    public class LogMaster
    {
        public int Id { get; set; }
        public int sno { get; set; }
        public string FunctionName { get; set; }
        public string ErrorMessage { get; set; }
        public string StackTrace { get; set; }
        public long StatusCode { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}

