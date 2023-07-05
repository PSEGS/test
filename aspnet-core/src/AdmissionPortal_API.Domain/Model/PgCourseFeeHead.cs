using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.Model
{
   public  class PgCourseFeeHead
    {
        public class AddPgCourseFeeHead
        {
            [JsonIgnore]
            public int UniversityCollegeId { get; set; }
            [Required(ErrorMessage ="Please Provied Course")]
            public int CourseId { get; set; }            
            [Required(ErrorMessage = "Please Provied Head type")]
            public int HeadId { get; set; }
            [Required(ErrorMessage = "Please Provied Fee")]
            [Range(typeof(Decimal), "0", "100000", ErrorMessage = "{0} must be a decimal/number between {1} and {2}.")]

            public decimal? Fee { get; set; } = 0;
            public bool IsWaiver { get; set; }
            
            [JsonIgnore]

            public int CreatedBy { get; set; }

        }
        public class AddPgCourseFeeHeadFee
        {
            [JsonIgnore]
            public int UniversityCollegeId { get; set; }
            //[Required(ErrorMessage = "Please Provied Course")]
            //public int CourseId { get; set; }
            [Required(ErrorMessage = "Please Provied CollegeType")]

            public int CollegeType { get; set; }
            [Required(ErrorMessage = "Please Provied Head type")]
            public int HeadId { get; set; }
            [Range(typeof(Decimal), "0", "100000", ErrorMessage = "{0} must be a decimal/number between {1} and {2}.")]

            public decimal? Fee { get; set; } = 0;
            
            public bool IsWaiver { get; set; }

            [JsonIgnore]

            public int CreatedBy { get; set; }
            public int CourseFundType { get; set; }

        }
        public class UpdatePgCourseFeeHead
        {
            public int Id { get; set; }
            [JsonIgnore]
            public int UniversityCollegeId { get; set; }
            [Required(ErrorMessage = "Please Provied Course")]
            public int CourseId { get; set; }
            [Required(ErrorMessage = "Please Provied Head type")]
            public int HeadId { get; set; }
            [Required(ErrorMessage = "Please Provied Fee")]
            public decimal? Fee { get; set; } = 0;
            [JsonIgnore]
            public int ModifiedBy { get; set; }
        }
        public class PgCourseFeeHeadDetails
        {
            public int Id { get; set; }
            public int UniversityCollegeId { get; set; }
            public string UniversityCollege { get; set; }
            public string CourseName { get; set; }
            public int CourseId { get; set; }
            public int HeadId { get; set; }
            public string HeadName{ get; set; }
            public decimal? Fee { get; set; } = 0;

        }
        public class FeedHeadDetailsByPgCourse
        {
            public string HeadName { get; set; }
            [Range(typeof(Decimal), "0", "100000", ErrorMessage = "{0} must be a decimal/number between {1} and {2}.")]
            public decimal? Fee { get; set; } = 0;
            public string HeadType { get; set; }
            public string IsWaiver { get; set; }
            public string Id { get; set; }
            public Boolean IsLock { get; set; }
        }
        public class PgCourseFeeByCollegId
        {
            public string CourseId { get; set; }
            public string CourseName { get; set; }
            public string Fee { get; set; }
            public Boolean IsLock { get; set; }
        }
        public class PgCourseFeeWaveOff
        {
            [Required]
            public int HeadId { get; set; }
            [Required]
            [JsonIgnore]

            public int UniversityCollege_Id { get; set; }
            [Required]

            public int CategoryType { get; set; }
            [Required]

            public int CategoryValue { get; set; }
            [Required]

            public int CalculationMethod { get; set; }
            [Required]

            public decimal CalculationValue { get; set; }
            [Required]

            public int CollegeType { get; set; }
            [JsonIgnore]

            public int CreatedBy { get; set; }
        }
        public class GetPgCourseFeeWaveOff
        {
             
            public string  HeadName { get; set; }
            public int HeadId { get; set; }


            public int CategoryType { get; set; }
             

            public int CategoryValue { get; set; }
         

            public int CalculationMethod { get; set; }
  

            public decimal CalculationValue { get; set; }
 

            public int CollegeType { get; set; }
        
        }
    }
}
