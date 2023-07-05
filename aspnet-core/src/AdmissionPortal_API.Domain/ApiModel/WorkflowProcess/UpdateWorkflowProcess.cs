using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.ApiModel.WorkflowProcess
{
    public class UpdateWorkflowProcess
    {
        [Required(ErrorMessage = "Process is required")]
        public int ProcessId { get; set; }
        [Required(ErrorMessage = "Department is required")]
        public int DepartmentIdFk { get; set; }
        public int? ParentProcessIdFk { get; set; }
        [Required(ErrorMessage = "ProcessTitle is required")]
        public string ProcessTitle { get; set; }
        public string ProcessTitlePb { get; set; }
        public long CreatedByEmp { get; set; }
        public long CreatedByPost { get; set; }
        [Required(ErrorMessage = "Role Mapping Required")]
        public int FirstRoleMapping { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}

