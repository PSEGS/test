using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.ApiModel.WorkflowProcess
{
    public class AddWorkflowProcess
    {
        [Required(ErrorMessage ="Department is required")]
        public int DepartmentIdFk { get; set; }
        public int? ParentProcessIdFk { get; set; }
        [Required(ErrorMessage ="Process Title is required")]
        public string ProcessTitle { get; set; }
        public string ProcessTitlePb { get; set; }
        [Required]
        public long CreatedByEmp { get; set; }
        [Required]
        public long CreatedByPost { get; set; }
        [Required(ErrorMessage ="Role Mapping Required")]
        public int FirstRoleMapping { get; set; }
    }
}

