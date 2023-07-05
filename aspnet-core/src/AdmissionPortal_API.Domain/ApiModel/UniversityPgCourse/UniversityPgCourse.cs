using AdmissionPortal_API.Domain.ApiModel.PGCollegeCourseMapping;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.ApiModel.UniversityPgCourse
{
    public class UniversityPgCourse
    {
        public int UniversityId { get; set; }
        [Required(ErrorMessage = "Course is required")]
        public List<PGUniversityCourse> courses { get; set; }
        public int CreatedBy { get; set; } 
    }
}
