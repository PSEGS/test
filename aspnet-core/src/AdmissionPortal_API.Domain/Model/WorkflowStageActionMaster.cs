using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.Model
{
    public class WorkflowStageActionMaster
    {
        public long ActionId { get; set; }
        public int ProcessIdFk { get; set; }
        public string ActionTitle { get; set; }
        public string ActionTitlePb { get; set; }
        public string ActionType { get; set; }
        public long StageIdFk { get; set; }
        public int RoleIdFk { get; set; }
        public string? RedirectionType { get; set; }
        public long? NextStageIdFk { get; set; }
        public int? NextRoleIdFk { get; set; }
        public bool MandatoryDocFlag { get; set; }
        public bool IsDocFlagShown { get; set; }
        
        //public string ActionUIFlag { get; set; }
        //public string ActionUIFormTypeFlag { get; set; }
        //public string ActionDBFlag { get; set; }
        //public string ActionSelectBy { get; set; }
        //public bool IsActive { get; set; }
        //public bool IsDeleted { get; set; }
        //public DateTime CreatedOn { get; set; }
        public long CreatedByEmp { get; set; }
        public long CreatedByPost { get; set; }
        [IgnoreDataMember]
        public long PageNumber { get; set; }
        [IgnoreDataMember]
        public long PageSize { get; set; }
        public int TotalCount { get; set; }
        public bool IsActive { get; set; }
    }
}

