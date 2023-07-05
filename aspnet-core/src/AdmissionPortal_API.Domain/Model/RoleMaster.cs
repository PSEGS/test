using System;
using System.Runtime.Serialization;

namespace AdmissionPortal_API.Domain.Model
{
    public class RoleMaster
    {
        public int RoleId { get; set; }
        public int sno { get; set; }
        public string RoleTitle { get; set; }
        public bool? IsActive { get; set; }
        public long CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public long? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public long StakeHolderId { get; set; }
        [IgnoreDataMember]
        public long PageNumber { get; set; }
        [IgnoreDataMember]
        public long PageSize { get; set; }
        [IgnoreDataMember]
        public string SearchKeyword { get; set; }
        [IgnoreDataMember]
        public string SortBy { get; set; }
        [IgnoreDataMember]
        public string SortOrder { get; set; }
        public int TotalCount { get; set; }
             
        public string NavigationIds { get; set; }
    }
}

