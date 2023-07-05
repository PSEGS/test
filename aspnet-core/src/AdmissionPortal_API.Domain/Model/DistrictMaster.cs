using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.Model
{
    public class DistrictMaster
    {
        public string DistrictName { get; set; }
        public long LGDCode { get; set; }
        public bool IsActive { get; set; }

    }
}

