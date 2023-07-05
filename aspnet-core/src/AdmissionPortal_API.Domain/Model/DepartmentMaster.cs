using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.Model
{
    public class DepartmentMaster
    {
        public long Department_ID { get; set; }
        public string Department_Name { get; set; }
        public string Short_Code { get; set; }
        public bool IsActive { get; set; }
    }

}

