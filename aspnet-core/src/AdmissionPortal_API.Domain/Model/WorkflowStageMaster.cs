using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.Model
{
    public class WorkflowStageMaster
    {
        public long StageId { get; set; }
        public int ProcessIdFk { get; set; }
        public string StageTitle { get; set; }
        public string StageTitlePb { get; set; }
        public string StageType { get; set; }
        public long CreatedByEmp { get; set; }
        public long CreatedByPost { get; set; }
        public bool IsFirstStage { get; set; }
        [IgnoreDataMember]
        public long PageNumber { get; set; }
        [IgnoreDataMember]
        public long PageSize { get; set; }
        public int TotalCount { get; set; }
        public bool IsActive { get; set; }

    }
}

