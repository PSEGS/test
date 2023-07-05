using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.Model
{
    public class PgCourseModel
    {
        public class AddPgCourse
        {
            [Required(ErrorMessage = "Please Provide Course Name.")]
            [Display(Name = "Course Name")]
            public string CourseName { get; set; }


            //[Required(ErrorMessage = "Please Provide Course Type")]
            //[Display(Name = "Course Type")]

            //public string CourseType { get; set; }
            

            [IgnoreDataMember]
            public int CreatedBy { get; set; }

        }
        public class UpdatePgCourse
        {
            public int CourseId { get; set; }

            [Required(ErrorMessage = "Please Provide Course Name.")]
            [Display(Name = "Course Name")]

            public string CourseName { get; set; }


            //[Required(ErrorMessage = "Please Provide Course Type")]
            //[Display(Name = "Course Type")]
            //public string CourseType { get; set; }
            public int ModifiedBy { get; set; }

        }
        public class PgCourseDetail
        {
            public int CourseId { get; set; }

            [Display(Name = "Course Name")]
            public string CourseName { get; set; }


            [Display(Name = "Course Type")]
            public string CourseType { get; set; }

            public string CourseTypeId { get; set; }
            public int totalCount { get; set; }
            public int sno { get; set; }
            public bool IsActive { get; set; }
        }


    }
}
