using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.Model
{
    public class NavigationMaster
    {
        public int NavigationId { get; set; }
        public int sno { get; set; }
        public int ParentNavigationId { get; set; }
        public string NavigationTitle { get; set; }
        public string NavigationUrl { get; set; }
        public string NavigationDescription { get; set; }
        public int NavigationPosition { get; set; }

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
        public bool IsActive { get; set; }

    }
}

