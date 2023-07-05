using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.ApiModel.WorkflowStageAction
{
    public class UpdateWorkflowStageAction
    {
        [Required(ErrorMessage = "Action is Required")]
        public long ActionId { get; set; }
        [Required(ErrorMessage = "Process is Required")]
        public int ProcessIdFk { get; set; }
        [Required(ErrorMessage = "Action Title is Required")]
        public string ActionTitle { get; set; }
        public string ActionTitlePb { get; set; }
        [Required(ErrorMessage = "Action Type is Required")]
        public char ActionType { get; set; }
        [Required(ErrorMessage = "Stage is Required")]
        public long StageIdFk { get; set; }

        [Required(ErrorMessage = "Role is Required")]
        public int RoleIdFk { get; set; }
        public char? RedirectionType { get; set; }
        public long? NextStageIdFk { get; set; }
        public int? NextRoleIdFk { get; set; }
        public bool MandatoryDocFlag { get; set; }
        public bool IsDocFlagShown { get; set; }
        public long CreatedByEmp { get; set; }
        public long CreatedByPost { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}

