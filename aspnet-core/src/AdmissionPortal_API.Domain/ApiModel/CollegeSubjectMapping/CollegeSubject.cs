using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Domain.ApiModel.CollegeSubjectMapping
{
    public class CollegeCourseSectionSubject
    {
        public Int32 CollegeId { get; set; }
        public Int32 SectionId { get; set; }
        public Int32 CourseId { get; set; }
        public List<CreateSubject> subjects { get; set; }
        public Int32 CreatedBy { get; set; }
        public Int32 Fees { get; set; }
    }
    public class CreateSubject
    {
        public Int32 SubjectId { get; set; }
        public string SubjectName { get; set; }
        public Int32 CombinationId { get; set; }
    }
    public class Subject
    {
        public Int32 SubjectId { get; set; }
        public string SubjectName { get; set; }
        public Int32 CombinationId { get; set; }
        public Boolean IsLock { get; set; }
    }

    public class SubjectListing
    {
        public Int32 Id { get; set; }
        public int sno { get; set; }
        public Int32 SubjectId { get; set; }
        public string SubjectName { get; set; }
        public Int32 CombinationId { get; set; }
        public string CourseName { get; set; }
        public string SubjectType { get; set; }
        public Int32 SubjectTypeId { get; set; }
        public Int32 CourseId { get; set; }
        public Int32 TotalCount { get; set; }
        public Int32 Fees { get; set; }
        public Boolean IsLock { get; set; }
    }
}
