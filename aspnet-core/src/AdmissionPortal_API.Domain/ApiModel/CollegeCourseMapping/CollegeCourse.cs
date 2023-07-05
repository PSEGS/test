using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.ApiModel.CollegeCourseMapping
{
    public class CollegeCourse
    {
        public int CollegeId { get; set; }
        [Required(ErrorMessage = "Course is required")]
        public List<CreateUgCourse> courses { get; set; }
        public int CreatedBy { get; set; }
    }
    public class CreateUgCourse
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public Boolean IsSelfFinace { get; set; }
        public int CourseExaminationType { get; set; }
        public Boolean selected { get; set; }
    }
     
    public class Course
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public Boolean selected { get; set; }
        public Boolean IsSelfFinace { get; set; }
        public int CourseExaminationType { get; set; }
        public Boolean IsLock { get; set; }
    }
    public class CombinationCourse
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public Boolean selected { get; set; }
        public Boolean IsLock { get; set; }
        public int CompulsorySubject { get; set; }
        public int OptionalSubject { get; set; }
        
    }

    public class universityCourse
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public Boolean selected { get; set; }
        //public Boolean IsSelfFinace { get; set; }
    }

    public class CollegeCoursesListing
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public Int32 TotalSeats { get; set; }
        public Int32 CourseFee { get; set; }
        public Int32 PracticalFee { get; set; }
    }
}
