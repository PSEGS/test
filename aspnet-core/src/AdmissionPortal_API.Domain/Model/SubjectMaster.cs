using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.Model
{
    public class SubjectMaster
    {
        [Required(ErrorMessage = "Please Provied Subject Name.")]
        [Display(Name = "Subject Name")]
        public string SubjectName { get; set; }

        [Required(ErrorMessage = "Please Provide University.")]
        [Display(Name = "University")]
        public int UniversityId { get; set; }

        [Required(ErrorMessage = "Please Provied Course.")]
        [Display(Name = "Course")]

        public int CourseId { get; set; }
        //[Required(ErrorMessage = "Please Provied Subject Type.")]
        //[Display(Name = "Subject Type")]
        //public int SubjectType { get; set; }
        [JsonIgnore]
        public int Createdby { get; set; }
    }
    public class UpdateSubject

    {
        public int subjectId { get; set; }
        [Required(ErrorMessage = "Please Provied Subject Name.")]
        [Display(Name = "Subject Name")]
        public string SubjectName { get; set; }

        [Required(ErrorMessage = "Please Provide University.")]
        [Display(Name = "University")]
        public int UniversityId { get; set; }

        [Required(ErrorMessage = "Please Provied Course.")]
        [Display(Name = "Course")]

        public int CourseId { get; set; }
        //[Required(ErrorMessage = "Please Provied Subject Type.")]
        //[Display(Name = "Subject Type")]
        //public int SubjectType { get; set; }
        [JsonIgnore]
        public int ModifyBy { get; set; }


    }
    public class SubjectDetails
    {
        public int SubjectId { get; set; }
        public int UniversityId { get; set; }
        public string SubjectName { get; set; }
        //public int SubjectTypeId { get; set; }

        //public string SubjectType { get; set; }

        public int CourseId { get; set; }
        public string CourseName { get; set; }
        public string IsActive { get; set; }
    }
    public class coursesubject
    {
        public string SubjectId { get; set; }
        public int sno { get; set; }
        public string SubjectName { get; set; }
        //public string SubjectType { get; set; }
        //public string SubjectTypeId { get; set; }
        public Int32 TotalCount { get; set; }
    }
    public class studetypesM
    {       
        public List<coursesubject> subjectlist = new List<coursesubject>();

    }
    public class returndata
    {
        public List<studetypesM> root = new List<studetypesM>();
    }

    public class CourseSubjectCombinationCheck
    {
        public string Id { get; set; }
        public int sno { get; set; }
        public int CourseId { get; set; }
        public int UniversityId { get; set; }
        public string CourseName { get; set; }
        public string CompulsorySubject { get; set; }
        public string OptionalSubject { get; set; }
        public Int32 TotalCount { get; set; }
    }
    public class UnvCourseSubjectCombinationCheck
    {
        public int CourseId { get; set; }
        public int UniversityId { get; set; }
        public int CompulsorySubject { get; set; }
        public int OptionalSubject { get; set; }
    }
}
