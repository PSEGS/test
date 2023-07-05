using AdmissionPortal_API.Domain.ApiModel.Student;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.Model
{
    public class StudentAcademicMaster
    {
        public int StudentId { get; set; }
        public int QualificationId { get; set; }
        public string Examination { get; set; }
        public string BoardUniversity { get; set; }
        public string BoardUniversityID { get; set; }
        public string BoardUniversityName { get; set; }
        public string SchoolCollegeName { get; set; }
        public string YearOfPassing { get; set; }
        public int? StreamID { get; set; }
        public string StreamName { get; set; }
        public int? ResultStatus { get; set; }
        public string Result { get; set; }
        public string TotalMarks { get; set; }
        public string ObtainedMarks { get; set; }
        public decimal Percentage { get; set; }
        public string CGPA { get; set; }
        public string Rollno { get; set; }
        public string SubjectName { get; set; }
        public int SubjectID { get; set; }
        public int MaxNumber { get; set; }
        public int MinNumber { get; set; }
        public int ObtainedNumber { get; set; }
        public int TwelvethQualificationId { get; set; }
        public string TwelvethExamination { get; set; }
        public string TwelvethBoardUniversity { get; set; }
        public string TwelvethBoardUniversityID { get; set; }
        public string TwelvethBoardUniversityName { get; set; }
        public string TwelvethSchoolCollegeName { get; set; }
        public int? TwelvethStreamID { get; set; }
        public string TwelvethStreamName { get; set; }
        public int? TwelvethResultStatus { get; set; }
        public string TwelvethResult { get; set; }
        public string TwelvethTotalMarks { get; set; }
        public string TwelvethObtainedMarks { get; set; }
        public decimal TwelvethPercentage { get; set; }
        public string TwelvethCGPA { get; set; }
        public string TwelvethYearOfPassing { get; set; }
        public string TwelvethRollNo { get; set; }
        public List<Id> lstSubjectDetails { get; set; }
        public bool IsDiploma { get; set; }
        public bool IsManualData { get; set; }
        public bool IsTwelveth { get; set; }
    }
}
