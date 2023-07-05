using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.Model
{
    public class WorkflowProcessMaster
    {
        public int ProcessId { get; set; }
        public int DepartmentIdFk { get; set; }
        public int? ParentProcessIdFk { get; set; }
        public string ProcessTitle { get; set; }
        public string ProcessTitlePb { get; set; }
        public long CreatedByEmp { get; set; }
        public long CreatedByPost { get; set; }

        public int FirstRoleMapping { get; set; }
        [IgnoreDataMember]
        public long PageNumber { get; set; }
        [IgnoreDataMember]
        public long PageSize { get; set; }
        public int TotalCount { get; set; }
        public bool IsActive { get; set; }
    }

}

