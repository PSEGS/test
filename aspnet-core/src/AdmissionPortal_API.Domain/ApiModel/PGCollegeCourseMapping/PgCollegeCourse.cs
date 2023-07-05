using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.ApiModel.PGCollegeCourseMapping
{
    public class PgCollegeCourse
    {
        public int CollegeId { get; set; }
        [Required(ErrorMessage = "PG Course is required")]
        public List<CreatePgCourse> courses { get; set; }
        public int CreatedBy { get; set; }
    }
    public class CreatePgCourse
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public Boolean IsSelfFinace { get; set; }
        public int CourseExaminationType { get; set; }
        public Boolean Selected { get; set; }
    }
    public class PgCourse
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public Boolean IsSelfFinace { get; set; }
        public int CourseExaminationType { get; set; }
        public Boolean Selected { get; set; }
        public Boolean IsLock { get; set; }
    }

    public class PGUniversityCourse
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public Boolean selected { get; set; }
        //public Boolean IsSelfFinace { get; set; }
    }

    public class PgCollegeCoursesListing
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public Int32 TotalSeats { get; set; }
        public Int32 CourseFee { get; set; }
        public Int32 PracticalFee { get; set; }
    }
    public class PgMappedCourse
    {
        public string CourseId { get; set; }
        public string CourseName { get; set; }
    }

}
