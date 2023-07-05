using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.Model
{
    public class CourseModel
    {
        public class AddCourse
        {
            [Required(ErrorMessage = "Please Provied Course Name.")]
            [Display(Name = "Course Name")]

            public string CourseName { get; set; }
            //[Required(ErrorMessage = "Please Provied Course Type")]
            //[Display(Name = "Course Type")]

            //public string CourseType { get; set; }
            [Display(Name = "Course Combination")]

            public bool? isCombination { get; set; } = false;

            [JsonIgnore]
            public int CreatedBy { get; set; }
            //[Required(ErrorMessage = "Please Provied Course Fund Type")]
            //public string CourseFundType { get; set; }


        }
        public class updateCourse
        {
            public int CourseId { get; set; }

            [Required(ErrorMessage = "Please Provied Course Name.")]
            [Display(Name = "Course Name")]

            public string CourseName { get; set; }
            //[Required(ErrorMessage = "Please Provied Course Type")]
            //[Display(Name = "Course Type")]
            //public string CourseType { get; set; }
            [Display(Name = "Course Combination")]
            public bool? isCombination { get; set; }= false;
            public int ModifiedBy { get; set; }
            //[Required(ErrorMessage = "Please Provied Course Fund Type")]

            //public string CourseFundType { get; set; }


        }
        public class CourseDetail
        {
            public int CourseId { get; set; }

            [Display(Name = "Course Name")]
            public string CourseName { get; set; }
            [Display(Name = "Course Type")]

            public string CourseType { get; set; }
            public string CourseTypeId { get; set; }
            public bool? isCombination { get; set; } = false;
            public int totalCount { get; set; }
            public int sno { get; set; }
            public string IsActive { get; set; }
            //public string CourseFundType { get; set; }
            //public string CourseFundTypeId { get; set; }

        }
        public class CourseList
        {
            public int CourseId { get; set; }

            [Display(Name = "Course Name")]
            public string CourseName { get; set; }
        }

        }
}
