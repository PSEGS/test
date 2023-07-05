using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.ApiModel.Section
{
    public class GetAllSection
    {
        public int SectionId { get; set; }
        public string CollegeName { get; set; }
        public string CourseName { get; set; }
        public string SectionType { get; set; }
        public int CreatedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
    }
}
