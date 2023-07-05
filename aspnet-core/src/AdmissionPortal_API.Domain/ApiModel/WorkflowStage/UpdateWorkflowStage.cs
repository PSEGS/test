using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.ApiModel.WorkflowStage
{
    public class UpdateWorkflowStage
    {
        [Required(ErrorMessage = "Stage is Required")]
        public long StageId { get; set; }
        [Required(ErrorMessage = "Process is Required")]
        public int ProcessIdFk { get; set; }
        [Required(ErrorMessage = "Stage Title is Required")]
        public string StageTitle { get; set; }
        public string StageTitlePb { get; set; }
        [Required(ErrorMessage = "Stage Type is Required")]
        public char StageType { get; set; }
        [Required]
        public bool IsFirstStage { get; set; }
        public long CreatedByEmp { get; set; }
        public long CreatedByPost { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}

