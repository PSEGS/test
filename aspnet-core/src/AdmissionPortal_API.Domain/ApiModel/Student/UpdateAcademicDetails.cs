using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.ApiModel.Student
{
    public class UpdateAcademicDetails
    {
        [JsonIgnore]
        public int? StudentId { get; set; }
        public string Rollno { get; set; }
        public string YearOfPassing { get; set; }
        public string Examination { get; set; }
        public string BoardUniversity { get; set; }
        public string BoardUniversityID { get; set; }
        public string SchoolCollegeName { get; set; }
        public int StreamID { get; set; }
        public int? ResultStatus { get; set; }
        public int? TotalMarks { get; set; } = 0;
        public int? ObtainedMarks { get; set; }=0;
        public decimal? Percentage { get; set; }
        public string CGPA { get; set; }
        public string TwelvethRollno { get; set; }
        public string TwelvethYearOfPassing { get; set; }
        public string TwelvethExamination { get; set; }
        public string TwelvethBoardUniversity { get; set; }
        public string TwelvethBoardUniversityID { get; set; }
        public string TwelvethSchoolCollegeName { get; set; }
        public int TwelvethStreamID { get; set; }
        public int? TwelvethResultStatus { get; set; }
        public int? TwelvethTotalMarks { get; set; } = 0;
        public int? TwelvethObtainedMarks { get; set; } = 0;
        public decimal? TwelvethPercentage { get; set; }    
        public string TwelvethCGPA { get; set; }
        public List<Subjects> lstSubjectDetails { get; set; }
        public bool? IsDiploma { get; set; }
        public bool IsManual { get; set; }
        public bool? IsCMScholarship { get; set; } = false;
        public bool? IsAlreadyScholarship { get; set; } = false;
        
        public string Amount { get; set; }
        public string SchemeName { get; set; }
        public string sponseredby { get; set; }     

    }
    public class Subjects
    {
        public string SubjectName { get; set; }
        public int MaxNumber { get; set; }
        // public int MinNumber { get; set; }
        public int ObtainedNumber { get; set; }
    }

    public class Id : Subjects
    {
        public int SubjectID { get; set; }
    }
}
