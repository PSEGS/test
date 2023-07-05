using AdmissionPortal_API.Domain.ApiModel.CollegeCourseMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Data.RepositoryInterface
{
    public interface ICollegeCourseMappingRepository : IGenericRepository<CollegeCourse>
    {
        Task<int> AddMapping(CollegeCourse addCollege);
        Task<List<Course>> getCoursesByCollegeId(Int32 CollegeId,Int32 universityId,Int32? Type);
        Task<List<Course>> getMappedCoursesByCollegeId(Int32 CollegeId);
        Task<List<CombinationCourse>> getCombinationCoursesByCollegeId(Int32 CollegeId);
        Task<int> LockUnlockCoursesByCollegeID(int CollegeId, int Status, int modifyBy);
    }
}
