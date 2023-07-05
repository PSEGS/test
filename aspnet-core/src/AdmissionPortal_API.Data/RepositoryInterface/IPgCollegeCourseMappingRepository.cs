using AdmissionPortal_API.Domain.ApiModel.PGCollegeCourseMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdmissionPortal_API.Data.RepositoryInterface
{
    public interface IPgCollegeCourseMappingRepository : IGenericRepository<PgCollegeCourse>
    {
        Task<int> AddMapping(PgCollegeCourse addCollege);
        Task<List<PgCourse>> getCoursesByCollegeId(Int32 CollegeId,Int32 universityId,Int32? Type);
        Task<List<PgCourse>> getMappedCoursesByCollegeId(Int32 CollegeId);
        Task<List<PgCourse>> getCombinationCoursesByCollegeId(Int32 CollegeId);
        Task<int> LockUnlockCoursesByCollegeID(int CollegeId, int Status, int modifyBy);
        Task<List<PgMappedCourse>> GetPgMappedCourseByCollege(Int32 CollegeId);

        
    }
}
