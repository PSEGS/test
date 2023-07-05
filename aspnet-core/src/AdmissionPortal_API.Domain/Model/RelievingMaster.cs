using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.Model
{
    public class RelievingMaster
    {
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public long DepartmentId { get; set; }
        public DateTime RelievingDate { get; set; }
        public long SerialNumber { get; set; }
    }
}

