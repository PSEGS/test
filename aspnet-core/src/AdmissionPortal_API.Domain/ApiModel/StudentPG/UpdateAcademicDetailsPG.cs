using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.ApiModel.StudentPG
{
    public class UpdateAcademicDetailsPG
    {
        [JsonIgnore]
        public int StudentId { get; set; }
        public string TwelvethRollno { get; set; }
        public string TwelvethYearOfPassing { get; set; }
        public string TwelvethExamination { get; set; }
        public string TwelvethBoardUniversity { get; set; }
        public string TwelvethBoardUniversityID { get; set; }
        public string TwelvethSchoolCollegeName { get; set; }
        public int? TwelvethStreamID { get; set; }
        public int? TwelvethResultStatus { get; set; }
        public int? TwelvethTotalMarks { get; set; }
        public int? TwelvethObtainedMarks { get; set; }
        public string TwelvethPercentage { get; set; }
        public string TwelvethCGPA { get; set; }

        public string graduationRollno { get; set; }
        public string graduationYearOfPassing { get; set; }
        public string graduationExamination { get; set; }
        public string graduationUniversity { get; set; }
        public string UniversityID { get; set; }
        public string graduationCollegeName { get; set; }
        public int? graduationCourseID { get; set; }
        public int? graduationResultStatus { get; set; }
        public int? graduationTotalMarks { get; set; }
        public int? graduationObtainedMarks { get; set; }
        public decimal graduationPercentage { get; set; }
        public string graduationCGPA { get; set; }

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
        public decimal? Percentage { get; set; }
        public string CGPA { get; set; }
        public string Rollno { get; set; }

    }

    public class GetStudentAcademicPG
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

        public int graduationQualificationId { get; set; }
        public string graduationExamination { get; set; }
        public string UniversityID { get; set; }
        public string graduationUniversityName { get; set; }
        public string graduationCollegeName { get; set; }
        public string graduationCollege { get; set; }
        public string graduationCollegeId { get; set; }
        public int? graduationCourseID { get; set; }
        public string graduationCourseName { get; set; }
        public int? graduationResultStatus { get; set; }
        public string graduationResult { get; set; }
        public string graduationTotalMarks { get; set; }
        public string graduationObtainedMarks { get; set; }
        public decimal graduationPercentage { get; set; }
        public string graduationCGPA { get; set; }
        public string graduationYearOfPassing { get; set; }
        public string graduationRollNo { get; set; }
    }

}
