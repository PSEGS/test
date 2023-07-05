using AdmissionPortal_API.Domain.ApiModel.CollegeCourseMapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.ApiModel.UniversityCourse
{
    public class UniversityCourse
    {
        public int UniversityId { get; set; }
        [Required(ErrorMessage = "Course is required")]
        public List<universityCourse> courses { get; set; }
        public int CreatedBy { get; set; }
    }
}
