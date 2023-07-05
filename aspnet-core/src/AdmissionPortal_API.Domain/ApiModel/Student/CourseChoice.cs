using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.ApiModel.Student
{

    public class CourseChoice
    {
        [JsonIgnore]
        public int StudentId { get; set; }
        public List<SubjectCombination> SubjectCombinations { get; set; }


    }
    public class SubjectCombinationForStudent
    {
        
        public List<ChoiceCourseSelected> choiceCourseSelecteds { get; set; }        
    }
    public class ChoiceCourseSelected
    {
        public int CollegeId { get; set; }
        public int CollegeCourseId { get; set; }
        public string collegecourseName { get; set; }
        public string CollegeName { get; set; }
        public string districtName { get; set; }
        public string districtId { get; set; }
        public string CombinationSet { get; set; }
      
        public string PreferenceChoice { get; set; }
        public List<studentCombinationSubject> subjects { get; set; }
    }
    public class studentCombinationSubject
    {
        public string SubjectId { get; set; }
        public string SubjectName { get; set; }
        public string CombinationSet { get; set; }
        
        public string PreferenceChoice { get; set; }
        public string GroupId { get; set; }
        public int CollegeId { get; set; }
        public int CourseId { get; set; }

    }
    public class SubjectCombinationDetail
    {
        public int CollegeId { get; set; }
        public int CollegeCourseId { get; set; }
        public string collegecourseName { get; set; }
        public string subjectCombinationName { get; set; }
        public string CollegeName { get; set; }
        public string PreferenceChoice { get; set; }
        public int SubjectCombinationId { get; set; }
        public string districtName { get; set; }
    }
    public class SubjectCombination
    {
        public int CollegeId { get; set; }
        public int CollegeCourseId { get; set; }
        public int SubjectCombinationId { get; set; }
    }

    public class CourseChoiceFee
    {
        public int CollegeId { get; set; }
        public List<SelectedCourse> SelectedCourses { get; set; }
    }

    public class SelectedCourse
    {
        public int CourseId { get; set; }
    }

    public class SelectedCourseTotalFee
    {
        public int TotalFee { get; set; }
    }
}
