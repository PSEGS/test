using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.ApiModel.Student
{

    public class CourseChoiceWithSubject
    {
        public int CollegeId { get; set; }
        public int CollegeCourseId { get; set; }
        public string DistrictName { get; set; }
        public string CollegeName { get; set; }
        public string CollegecourseName { get; set; }
        public string combinationSet { get; set; }
        public List<SubjectDetail> Subjects { get; set; }
        public int PreferenceChoice { get; set; }
        public string Type { get; set; }
    }

    public class SubjectCombinationsWithCollege
    {
        [JsonIgnore]
        public int StudentId { get; set; }
        public List<CourseChoiceWithSubject> SubjectCombinations { get; set; }
    }


    public class SubjectDetail
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public int SubjectId { get; set; }
        public string SubjectName { get; set; }
        
        
    }
    public class MergeChoiceCourseWithSubject
    {
        public int StudentId { get; set; }
        public int CollegeId { get; set; }
        public int CourseId { get; set; }
        public int PreferenceChoice { get; set; }
        public string CombinationSet { get; set; }
        public int SubjectId { get; set; }
    
    }
}
